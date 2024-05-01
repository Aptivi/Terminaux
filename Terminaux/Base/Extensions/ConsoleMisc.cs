﻿//
// Terminaux  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.Tools;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Miscellaneous extensions for the console
    /// </summary>
    public static class ConsoleMisc
    {
        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text)
        {
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            string Sequence = $"{EscapeChar}]0;{Text}{BellChar}";
            TextWriterRaw.WriteRaw(Sequence);
        }

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * ((ConsoleWrapper.WindowWidth - WidthOffset) * 0.01d));

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="TargetWidth">The target width</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeatTargeted(int CurrentNumber, int MaximumNumber, int TargetWidth) =>
            (int)Math.Round(CurrentNumber * 100 / (double)MaximumNumber * (TargetWidth * 0.01d));

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text)
        {
            // Filter all sequences
            Text = VtSequenceTools.FilterVTSequences(Text);
            return Text;
        }

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console (with character wrapping)
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        public static string[] GetWrappedSentences(string text, int maximumLength) =>
            GetWrappedSentences(text, maximumLength, 0);

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console (with character wrapping)
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
        public static string[] GetWrappedSentences(string text, int maximumLength, int indentLength)
        {
            if (string.IsNullOrEmpty(text))
                return [""];

            // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
            // sizes.
            var IncompleteSentences = new List<string>();
            var IncompleteSentenceBuilder = new StringBuilder();

            // Make the text look like it came from Linux
            text = text.Replace(Convert.ToString(Convert.ToChar(13)), "");

            // Convert tabs to four spaces
            text = text.Replace("\t", "    ");

            // This indent length count tells us how many spaces are used for indenting the paragraph. This is only set for
            // the first time and will be reverted back to zero after the incomplete sentence is formed.
            foreach (string splitText in text.SplitNewLines())
            {
                var sequencesCollections = VtSequenceTools.MatchVTSequences(splitText);
                int vtSeqIdx = 0;
                int compensate = 0;
                int take = 0;
                if (splitText.Length == 0)
                    IncompleteSentences.Add(splitText);
                for (int i = 0; i < splitText.Length; i++)
                {
                    // Check the character to see if we're at the VT sequence
                    bool explicitNewLine = splitText[splitText.Length - 1] == '\n';
                    char ParagraphChar = splitText[i];
                    bool isNewLine = splitText[i] == '\n';
                    bool wouldOverflow = false;
                    bool overflown = false;
                    string seq = "";
                    foreach ((var _, var sequences) in sequencesCollections)
                    {
                        if (sequences.Length > 0 && sequences[vtSeqIdx].Index == i)
                        {
                            // We're at an index which is the same as the captured VT sequence. Get the sequence
                            seq = sequences[vtSeqIdx].Value;

                            // Raise the index in case we have the next sequence, but only if we're sure that we have another
                            if (vtSeqIdx + 1 < sequences.Length)
                                vtSeqIdx++;

                            // Raise the paragraph index by the length of the sequence
                            i += seq.Length - 1;
                            compensate += seq.Length;
                        }
                    }

                    // Append the character into the incomplete sentence builder.
                    string sequence = !string.IsNullOrEmpty(seq) ? seq : ParagraphChar.ToString();
                    int width = ConsoleChar.EstimateCellWidth(splitText, i);

                    // Also, compensate the zero-width characters and take the full-width ones
                    if (width == 0)
                        compensate++;
                    if (width == 2)
                        take++;
                    if (!isNewLine)
                    {
                        overflown = IncompleteSentenceBuilder.Length + width == maximumLength - indentLength + compensate - take;
                        wouldOverflow = IncompleteSentenceBuilder.Length + width > maximumLength - indentLength + compensate - take;
                        if (!wouldOverflow)
                            IncompleteSentenceBuilder.Append(sequence);
                    }

                    // Check to see if we're at the maximum character number or at the new line
                    if (overflown | wouldOverflow | i == splitText.Length - 1 | isNewLine)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                        if (explicitNewLine)
                            IncompleteSentences.Add("");

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                        compensate = 0;
                        take = 0;

                        // Add the overflown string if found
                        if (wouldOverflow)
                            IncompleteSentenceBuilder.Append(sequence);
                    }
                }
            }

            return [.. IncompleteSentences];
        }

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console (with word wrapping)
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        public static string[] GetWrappedSentencesByWords(string text, int maximumLength) =>
            GetWrappedSentencesByWords(text, maximumLength, 0);

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console (with word wrapping)
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
        public static string[] GetWrappedSentencesByWords(string text, int maximumLength, int indentLength)
        {
            if (string.IsNullOrEmpty(text))
                return [""];

            // Split the paragraph into sentences that have the length of maximum characters that can be printed in various terminal
            // sizes.
            var IncompleteSentences = new List<string>();
            var IncompleteSentenceBuilder = new StringBuilder();

            // Make the text look like it came from Linux
            text = text.Replace(Convert.ToString(Convert.ToChar(13)), "");

            // Convert tabs to four spaces
            text = text.Replace("\t", "    ");

            // This indent length count tells us how many spaces are used for indenting the paragraph. This is only set for
            // the first time and will be reverted back to zero after the incomplete sentence is formed.
            var lines = text.SplitNewLines();
            foreach (string splitText in lines)
            {
                int compensate = 0;
                int take = 0;
                if (splitText.Length == 0)
                {
                    IncompleteSentences.Add(splitText);
                    continue;
                }

                // Helper functions
                void CompensateLengths(string text)
                {
                    var sequencesCollections = VtSequenceTools.MatchVTSequences(text);
                    foreach ((var _, var sequences) in sequencesCollections)
                    {
                        if (sequences.Length == 0)
                            continue;
                        foreach (var sequence in sequences)
                            compensate += sequence.Value.Length;
                    }
                }

                // Split the text by spaces
                var words = splitText.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    // Check the character to see if we're at the VT sequence
                    string word = words[i];
                    CompensateLengths(word);

                    // Compensate the zero-width characters and take the full-width ones
                    for (int c = 0; c < word.Length; c++)
                    {
                        int width = ConsoleChar.EstimateCellWidth(splitText, c);
                        if (width == 0)
                            compensate++;
                        if (width == 2)
                            take++;
                    }

                    // Append the word into the incomplete sentence builder.
                    int finalMaximum = maximumLength - indentLength + compensate - take;
                    if (word.Length >= finalMaximum)
                    {
                        var charSplit = GetWrappedSentences(word, maximumLength, indentLength);
                        for (int splitIdx = 0; splitIdx < charSplit.Length - 1; splitIdx++)
                        {
                            string charSplitText = charSplit[splitIdx];

                            // We need to add character splits, except the last one.
                            if (IncompleteSentenceBuilder.Length > 0)
                                IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                            IncompleteSentences.Add(charSplitText);

                            // Clean everything up
                            IncompleteSentenceBuilder.Clear();
                            indentLength = 0;
                            compensate = 0;
                        }

                        // Process the character split last text
                        string charSplitLastText = charSplit[charSplit.Length - 1];
                        word = charSplitLastText;
                        CompensateLengths(charSplitLastText);
                        finalMaximum = maximumLength - indentLength + compensate;
                        IncompleteSentenceBuilder.Append(charSplitLastText);
                    }
                    else if (IncompleteSentenceBuilder.Length + word.Length < finalMaximum)
                        IncompleteSentenceBuilder.Append(word);

                    // Check to see if we're at the maximum length
                    int nextWord = i + 1 >= words.Length ? 1 : words[i + 1].Length + 1;
                    if (IncompleteSentenceBuilder.Length + nextWord >= finalMaximum)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                        compensate = 0;
                    }
                    else
                        IncompleteSentenceBuilder.Append(IncompleteSentenceBuilder.Length + nextWord >= finalMaximum || i + 1 >= words.Length ? "" : " ");
                }
                if (IncompleteSentenceBuilder.Length > 0)
                    IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                IncompleteSentenceBuilder.Clear();
            }

            return [.. IncompleteSentences];
        }

        /// <summary>
        /// Truncates the string if the string is larger than the threshold, otherwise, returns an unchanged string
        /// </summary>
        /// <param name="target">Source string to truncate</param>
        /// <param name="threshold">Max number of string characters</param>
        /// <returns>Truncated string</returns>
        public static string Truncate(this string target, int threshold)
        {
            if (target is null)
                throw new TerminauxException("The target may not be null");
            if (threshold < 0)
                threshold = 0;
            if (threshold == 0)
                return "";

            // Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
            // the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
            // terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
            // the three dots.
            //
            // In case VT sequences are inserted to the target string, it can mess with the actual length returned, so only
            // truncate the non-VT sequences.
            //
            // The problem here is that we're dealing with the console, so we need to estimate the cell width and not the
            // string length to handle characters that occupy two cells.
            int newLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(target));
            if (newLength > threshold)
                return target.Substring(0, threshold) + "...";
            else
                return target;
        }

        static ConsoleMisc()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
