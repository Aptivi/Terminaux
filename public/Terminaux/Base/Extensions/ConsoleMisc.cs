﻿//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using SpecProbe.Software.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions.Data;
using Terminaux.Base.Extensions.Native;
using Terminaux.Base.TermInfo;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Miscellaneous extensions for the console
    /// </summary>
    public static class ConsoleMisc
    {
        private static bool isOnAltBuffer = false;
        private static readonly Regex rtlRegex = new("[\u04c7-\u0591\u05D0-\u05EA\u05F0-\u05F4\u0600-\u06FF]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Specifies whether your terminal emulator reverses the RTL text automatically or not
        /// </summary>
        public static bool TerminalReversesRtlText { get; set; }

        /// <summary>
        /// Checks to see whether we're in the alternative buffer or not
        /// </summary>
        public static bool IsOnAltBuffer =>
            isOnAltBuffer;

        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;
            char BellChar = Convert.ToChar(7);
            char EscapeChar = Convert.ToChar(27);
            Text = Text.SplitNewLines()[0];
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
            if (string.IsNullOrEmpty(Text))
                return "";

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
            var lines = text.SplitNewLines();
            foreach (string splitText in lines)
            {
                var sequencesCollections = VtSequenceTools.MatchVTSequences(splitText);
                int vtSeqIdx = 0;
                int totalWidth = 0;
                if (splitText.Length == 0)
                    IncompleteSentences.Add(splitText);
                for (int i = 0; i < splitText.Length; i++)
                {
                    // Check the character to see if we're at the VT sequence
                    char ParagraphChar = splitText[i];
                    bool wouldOverflow = false;
                    bool overflown = false;

                    // Append the character into the incomplete sentence builder, but check the surrogate pairs first.
                    string sequence = ConsolePositioning.BufferChar(splitText, sequencesCollections, ref i, ref vtSeqIdx, out bool isVtSeq);
                    int width = ConsoleChar.EstimateCellWidth(splitText, i);
                    if (i + 1 < splitText.Length && char.IsSurrogatePair(ParagraphChar, splitText[i + 1]))
                    {
                        sequence += splitText[i + 1];
                        i++;
                    }
                    if (!isVtSeq)
                        totalWidth += width;

                    // Also, compensate the zero-width characters and take the full-width ones
                    overflown = totalWidth == maximumLength - indentLength;
                    wouldOverflow = totalWidth > maximumLength - indentLength;
                    if (!wouldOverflow)
                        IncompleteSentenceBuilder.Append(sequence);

                    // Check to see if we're at the maximum character number or at the new line
                    if (overflown | wouldOverflow | i == splitText.Length - 1)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                        totalWidth = 0;

                        // Add the overflown string if found
                        if (wouldOverflow)
                        {
                            totalWidth += width;
                            IncompleteSentenceBuilder.Append(sequence);
                            if (i == splitText.Length - 1)
                                IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());
                        }
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
        /// <remarks>This function returns the same output as <see cref="GetWrappedSentences(string, int, int)"/> if there are any full-width characters.</remarks>
        public static string[] GetWrappedSentencesByWords(string text, int maximumLength) =>
            GetWrappedSentencesByWords(text, maximumLength, 0);

        /// <summary>
        /// Gets the wrapped sentences for text wrapping for console (with word wrapping)
        /// </summary>
        /// <param name="text">Text to be wrapped</param>
        /// <param name="maximumLength">Maximum length of text before wrapping</param>
        /// <param name="indentLength">Indentation length</param>
        /// <remarks>This function returns the same output as <see cref="GetWrappedSentences(string, int, int)"/> if there are any full-width characters.</remarks>
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
                if (splitText.Length == 0)
                {
                    IncompleteSentences.Add(splitText);
                    continue;
                }

                // Split the text by spaces
                var words = splitText.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];

                    // Append the word into the incomplete sentence builder.
                    int finalMaximum = maximumLength - indentLength;
                    int sentenceWidth = ConsoleChar.EstimateCellWidth(IncompleteSentenceBuilder.ToString());
                    int wordWidth = ConsoleChar.EstimateCellWidth(word);
                    if (wordWidth >= finalMaximum)
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
                        }

                        // Process the character split last text
                        string charSplitLastText = charSplit[charSplit.Length - 1];
                        word = charSplitLastText;
                        IncompleteSentenceBuilder.Append(charSplitLastText);
                        sentenceWidth = ConsoleChar.EstimateCellWidth(IncompleteSentenceBuilder.ToString());
                    }
                    else if (sentenceWidth + wordWidth < finalMaximum)
                    {
                        IncompleteSentenceBuilder.Append(word);
                        sentenceWidth = ConsoleChar.EstimateCellWidth(IncompleteSentenceBuilder.ToString());
                    }

                    // Check to see if we're at the maximum length
                    int nextWord = i + 1 >= words.Length ? 1 : ConsoleChar.EstimateCellWidth(words[i + 1]) + 1;
                    if (sentenceWidth + nextWord >= finalMaximum)
                    {
                        // We're at the character number of maximum character. Add the sentence to the list for "wrapping" in columns.
                        IncompleteSentences.Add(IncompleteSentenceBuilder.ToString());

                        // Clean everything up
                        IncompleteSentenceBuilder.Clear();
                        indentLength = 0;
                    }
                    else
                        IncompleteSentenceBuilder.Append(sentenceWidth + nextWord >= finalMaximum || i + 1 >= words.Length ? "" : " ");
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
            if (threshold == 0 || target.Length == 0)
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
            {
                var targetBuilder = new StringBuilder(target);
                while (ConsoleChar.EstimateCellWidth(targetBuilder.ToString()) >= threshold)
                    targetBuilder.Remove(targetBuilder.Length - 1, 1);
                return $"{targetBuilder}...";
            }
            else
                return target;
        }

        /// <summary>
        /// Reverses the right-to-left characters in a string (for terminal printing)
        /// </summary>
        /// <param name="target">Target string to reverse</param>
        /// <returns>A string containing reversed RTL characters for usage with terminal writing. Returns the original string if either the terminal emulator is reversing the order of RTL characters or if there are no RTL characters</returns>
        public static string ReverseRtl(string target)
        {
            if (string.IsNullOrEmpty(target))
                return "";
            if (TerminalReversesRtlText)
                return target;

            // Check to see if we have any RTL character or not
            var rtls = rtlRegex.Matches(target).OfType<Match>().ToArray();
            if (rtls.Length == 0)
                return target;

            // Now, reverse every single RTL character
            var resultBuilder = new StringBuilder(target);
            for (int i = rtls.Length - 1; i >= 0; i--)
            {
                // Reverse the RTL text
                var rtl = rtls[i];
                int rtlIdx = rtl.Index;
                string rtlText = rtl.Value;

                // Some RTL languages have modifiers, such as Arabic modifiers that are found in the \u0600-\u06FF range for
                // the whole language, but check for all of them.
                char[] rtlTextChars = rtlText.ToCharArray();
                var finalRtlReversed = new StringBuilder();
                var finalRtlReversedModifiedLetter = new StringBuilder();
                for (int c = rtlTextChars.Length - 1; c >= 0; c--)
                {
                    int width = TextTools.GetCharWidth(rtlTextChars[c]);
                    if (width == 0)
                    {
                        // Is a modifier
                        finalRtlReversedModifiedLetter.Insert(0, rtlTextChars[c]);
                        continue;
                    }
                    else
                        finalRtlReversedModifiedLetter.Insert(0, rtlTextChars[c]);
                    finalRtlReversed.Append(finalRtlReversedModifiedLetter.ToString());
                    finalRtlReversedModifiedLetter.Clear();
                }

                // Save the changes
                resultBuilder.Replace(rtl.Value, finalRtlReversed.ToString(), rtlIdx, finalRtlReversed.Length);
            }

            // Now, reverse the word order for RTL
            var resultRtls = rtlRegex.Matches(resultBuilder.ToString()).OfType<Match>().ToArray();
            if (resultRtls.Length == 0)
                return resultBuilder.ToString();
            var finalRtlWords = new StringBuilder();
            var finalRtlWordReversed = new StringBuilder();
            for (int i = resultRtls.Length - 1; i >= 0; i--)
            {
                // Reverse the RTL word order
                bool commit = true;
                var rtl = resultRtls[i];
                int rtlIdx = rtl.Index;
                if (i > 0)
                {
                    var secondRtl = resultRtls[i - 1];
                    int secondRtlIdx = secondRtl.Index;
                    string secondRtlText = secondRtl.Value;
                    if (rtlIdx - secondRtlText.Length - 1 == secondRtlIdx)
                        commit = false;
                }
                finalRtlWords.Append($"{rtl}{(i > 0 ? " " : "")}");
                if (commit)
                {
                    string[] splitWords = finalRtlWords.ToString().Split([' '], StringSplitOptions.RemoveEmptyEntries);
                    for (int wordIdx = splitWords.Length - 1; wordIdx >= 0; wordIdx--)
                    {
                        string word = splitWords[wordIdx];
                        finalRtlWordReversed.Append($"{word} ");
                    }

                    // Save the changes
                    string finalReversedToBeReplaced = finalRtlWordReversed.ToString().Trim();
                    string finalReversedToReplaceWith = finalRtlWords.ToString().Trim();
                    resultBuilder.Replace(finalReversedToBeReplaced, finalReversedToReplaceWith, rtlIdx, finalReversedToReplaceWith.Length);
                    finalRtlWords.Clear();
                    finalRtlWordReversed.Clear();
                }
            }
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Shows the main buffer
        /// </summary>
        public static void ShowMainBuffer()
        {
            if (PlatformHelper.IsOnWindows())
                return;
            if (!isOnAltBuffer)
                return;

            TextWriterColor.Write($"{VtSequenceBasicChars.EscapeChar}[?1049l");
            isOnAltBuffer = false;
        }

        /// <summary>
        /// Shows the alternate buffer
        /// </summary>
        public static void ShowAltBuffer()
        {
            if (PlatformHelper.IsOnWindows())
                return;
            if (isOnAltBuffer)
                return;

            TextWriterColor.Write($"{VtSequenceBasicChars.EscapeChar}[?1049h");
            ConsoleWrapper.SetCursorPosition(0, 0);
            ConsoleWrapper.CursorVisible = false;
            isOnAltBuffer = true;
        }

        /// <summary>
        /// Initializes the VT sequence handling for Windows systems.
        /// </summary>
        /// <returns>True if successful; false if not. Alays true on non-Windows systems.</returns>
        public static bool InitializeSequences()
        {
            if (!PlatformHelper.IsOnWindows())
                return true;
            IntPtr stdHandle = NativeMethods.GetStdHandle(-11);
            uint mode = GetMode(stdHandle);
            if ((mode & 4) == 0)
                return NativeMethods.SetConsoleMode(stdHandle, mode | 4);
            return true;
        }

        /// <summary>
        /// Flashes the screen
        /// </summary>
        public static void Flash() =>
            TermInfoDesc.Current.FlashScreen?.RenderSequence();

        /// <summary>
        /// Flashes the screen
        /// </summary>
        /// <param name="delay">Delay as specified</param>
        public static void Flash(int delay)
        {
            TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?5h");
            Thread.Sleep(delay);
            TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?5l");
        }

        /// <summary>
        /// Emits the console bell using either audible or visible bell types
        /// </summary>
        /// <param name="bell">Type of console bell</param>
        /// <param name="delay">Delay of the bell (audible and visual)</param>
        /// <param name="freq">Frequency of the bell (audible)</param>
        public static void Bell(ConsoleBell bell = ConsoleBell.Audible, int delay = -1, int freq = -1)
        {
            switch (bell)
            {
                case ConsoleBell.Audible:
                    if (delay == -1 || freq == -1)
                        ConsoleWrapper.BeepSeq();
                    else
                        ConsoleWrapper.Beep(freq, delay);
                    return;
                case ConsoleBell.Visual:
                    if (delay == -1)
                        Flash();
                    else
                        Flash(delay);
                    return;
                default:
                    return;
            }
        }

        internal static uint GetMode(IntPtr stdHandle)
        {
            NativeMethods.GetConsoleMode(stdHandle, out uint mode);
            return mode;
        }

        internal static void SwapIfSourceLarger(this ref long sourceNumber, ref long targetNumber)
        {
            long source = sourceNumber;
            long target = targetNumber;
            if (sourceNumber > targetNumber)
            {
                sourceNumber = target;
                targetNumber = source;
            }
        }

        static ConsoleMisc()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
