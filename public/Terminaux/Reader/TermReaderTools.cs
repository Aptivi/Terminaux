//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Reader
{
    /// <summary>
    /// Terminal reader tools
    /// </summary>
    public static class TermReaderTools
    {
        internal static bool interrupting = false;
        internal static bool isWaitingForInput = false;

        /// <summary>
        /// Specifies whether the terminal reader is busy waiting for input or not (more general)
        /// </summary>
        public static bool Busy =>
            isWaitingForInput;

        /// <summary>
        /// Waits indefinitely for an input. Returns immediately if not busy.
        /// </summary>
        public static void WaitForInput() =>
            SpinWait.SpinUntil(() => !Busy);

        /// <summary>
        /// Interrupts the reading process
        /// </summary>
        public static void Interrupt()
        {
            if (isWaitingForInput)
                interrupting = true;
        }

        /// <summary>
        /// Refreshes the reader prompt
        /// </summary>
        public static void Refresh()
        {
            var state = TermReaderState.CurrentState;
            if (state is null)
                return;
            if (!state.RefreshRequired)
                return;

            // Check to see if the console has been resized before
            if (ConsoleResizeHandler.WasResized(false))
            {
                // It's been resized. Now, get the current text position
                int textPos = state.CurrentTextPos;
                ConsoleLogger.Debug("Resize detected. Fixing up values to represent pos {0} correctly...", textPos);

                // Go to the leftmost position
                PositioningTools.GoLeftmost(ref state);

                // Fix the values up
                ConsoleLogger.Debug("Initial values: (i: {0}, {1}) (c: {2}, {3})", state.inputPromptLeft, state.inputPromptTop, state.currentCursorPosLeft, state.currentCursorPosTop);
                state.inputPromptLeft = ConsoleChar.EstimateCellWidth(state.InputPromptLastLine);
                state.inputPromptTop = state.InputPromptTopBegin + state.InputPromptHeight - 1;
                if (state.inputPromptTop >= ConsoleWrapper.WindowHeight)
                    state.inputPromptTop = ConsoleWrapper.WindowHeight - 1;
                state.currentCursorPosLeft = state.InputPromptLeft;
                state.currentCursorPosTop = state.InputPromptTop;
                ConsoleLogger.Debug("Final values: (i: {0}, {1}) (c: {2}, {3})", state.inputPromptLeft, state.inputPromptTop, state.currentCursorPosLeft, state.currentCursorPosTop);

                // Go to the old position
                PositioningTools.SeekTo(textPos, ref state);
            }

            // Refresh the prompt
            RefreshPrompt(ref state);

            // Save the state
            state.RefreshRequired = false;
            TermReaderState.SaveState(state);
        }

        /// <summary>
        /// Gets the maximum input length for the current reader session
        /// </summary>
        /// <returns>The maximum input length of an input. -1 if there is no reader.</returns>
        public static int GetMaximumInputLength()
        {
            if (TermReader.state is null)
                return -1;

            // Get the current state
            var state = TermReaderState.CurrentState;
            if (state is null)
                return -1;
            return GetMaximumInputLength(state);
        }

        /// <summary>
        /// Inserts new text to the current reader
        /// </summary>
        /// <param name="newText">New text to insert or append</param>
        /// <param name="append">Whether to append the new text to the end of the input or to insert text to the current position</param>
        /// <param name="step">Whether to move the cursor forward after inserting or not</param>
        public static void InsertNewText(string newText, bool append = false, bool step = true)
        {
            if (TermReader.state is null)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;
            if (state is null)
                return;
            InsertNewText(ref state, newText, append, step);
            TermReaderState.SaveState(state);
        }

        /// <summary>
        /// Removes text from the current position
        /// </summary>
        /// <param name="length">Length of characters to remove</param>
        /// <param name="step">Whether to step backwards after removing characters</param>
        public static void RemoveText(int length, bool step = false)
        {
            if (TermReader.state is null)
                return;
            if (TermReaderState.CurrentState is null)
                return;

            RemoveText(TermReaderState.CurrentState.CurrentTextPos, length, step);
        }

        /// <summary>
        /// Removes text from the current position
        /// </summary>
        /// <param name="startIndex">Zero-based index of where to start removing <paramref name="length"/> characters</param>
        /// <param name="length">Length of characters to remove</param>
        /// <param name="step">Whether to step backwards after removing characters</param>
        public static void RemoveText(int startIndex, int length, bool step = false)
        {
            if (TermReader.state is null)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;
            if (state is null)
                return;
            RemoveText(ref state, startIndex, length, step);
            TermReaderState.SaveState(state);
        }

        /// <summary>
        /// Wipes the entire input
        /// </summary>
        public static void WipeAll()
        {
            if (TermReader.state is null)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;
            if (state is null)
                return;
            WipeAll(ref state);
            TermReaderState.SaveState(state);
        }

        internal static ConsoleKeyInfo GetInput(bool interruptible)
        {
            ConsoleKeyInfo cki = new();
            if (interruptible)
            {
                InputEventInfo data = new();
                isWaitingForInput = true;
                SpinWait.SpinUntil(() =>
                {
                    data = Input.ReadPointerOrKeyNoBlock();
                    return data.EventType == InputEventType.Keyboard || interrupting;
                });
                isWaitingForInput = false;
                if (interrupting)
                {
                    interrupting = false;
                    cki = new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);
                }
                else
                    cki = data.ConsoleKeyInfo ?? default;
                return cki;
            }
            else
                return ConsoleWrapper.ReadKey(true);
        }

        internal static int GetMaximumInputLength(TermReaderState state) =>
            GetMaximumInputLength(state.CurrentText.ToString(), state);

        internal static int GetMaximumInputLength(string text, TermReaderState state)
        {
            // First, get the maximum width and height
            int height = ConsoleWrapper.BufferHeight;

            // Then, get the length for each width and height, subtracting it by one to avoid wrapping
            int marginWidth = state.LongestSentenceLengthFromLeftForGeneralLine + 1;
            int cells = marginWidth * height;
            int length = cells - 1 - (state.LongestSentenceLengthFromLeftForGeneralLine - state.LongestSentenceLengthFromLeftForFirstLine);

            // We need to get the longest sentence width in case we encounter a multiline input prompt
            int longestSentenceLength = state.MaximumInputPositionLeft;
            string[] inputPromptLines = state.InputPromptText.SplitNewLines();
            int inputPromptLineTimes = inputPromptLines.Length - 1;

            // Subtract the length accordingly
            for (int i = 0; i < inputPromptLineTimes; i++)
                length -= longestSentenceLength + 1;

            // Compensate and take the length according to the character widths
            int fullWidths = ConsoleChar.EstimateFullWidths(text);
            bool hasFullWidths = fullWidths > 0;
            length += ConsoleChar.EstimateZeroWidths(text);

            // Take the unoccupied characters
            if (!hasFullWidths)
            {
                ConsoleLogger.Debug("Max input length (has no full widths): {0}", length);
                return length;
            }
            length -= fullWidths;
            var sentences = ConsoleMisc.GetWrappedSentences(state.currentText.ToString(), state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            for (int i = 0; i < sentences.Length; i++)
            {
                string sentence = sentences[i];
                int width = ConsoleChar.EstimateCellWidth(sentence);
                if (width == longestSentenceLength && i < sentences.Length - 1)
                    length--;
            }

            // Return the number of length available
            ConsoleLogger.Debug("Max input length (has full widths of {0}): {1}", length, fullWidths);
            return length;
        }

        internal static void InsertNewText(ref TermReaderState state, string newText, bool append = false, bool step = true)
        {
            // If we can't insert, do nothing
            if (!state.CanInsert)
                return;

            // Replace some of the characters
            newText = newText.Replace("\t", new string(' ', ConsoleMisc.TabWidth)).ReplaceAll(CharManager.GetAllControlChars(), "");

            // Get the longest sentence width and crop the text appropriately
            if (!state.OneLineWrap)
            {
                int longest = GetMaximumInputLength(state.CurrentText.ToString() + newText, state);
                var strInfoNew = new StringInfo(newText);
                var strInfoOld = new StringInfo(state.CurrentText.ToString());
                if (strInfoOld.LengthInTextElements + strInfoNew.LengthInTextElements > longest)
                {
                    state.canInsert = false;
                    int len = longest - (strInfoOld.LengthInTextElements + strInfoNew.LengthInTextElements);
                    if (len < 0)
                        len = longest - strInfoOld.LengthInTextElements;
                    newText = strInfoNew.SubstringByTextElements(0, len <= 0 ? 0 : len - 1);
                }
            }

            // Now, insert the text
            state.oldText = state.CurrentText.ToString();
            if (append)
                state.CurrentText.Append(newText);
            else
                state.CurrentText.Insert(state.CurrentTextPos, newText);

            // Refresh
            RefreshPrompt(ref state, step ? newText.Length : 0);
            state.oldText = "";
        }

        internal static void RemoveText(ref TermReaderState state, int length, bool step = false) =>
            RemoveText(ref state, state.CurrentTextPos, length, step);

        internal static void RemoveText(ref TermReaderState state, int startIndex, int length, bool step = false)
        {
            // Check for surrogate pairs in case a user tries to remove them. Surrogate pairs require two characters: high and low.
            if (state.CurrentText.Length >= 2)
            {
                bool isStartIndexHigh = char.IsHighSurrogate(state.CurrentText[startIndex]);
                bool isStartIndexLow = char.IsLowSurrogate(state.CurrentText[startIndex]);
                if (isStartIndexHigh)
                {
                    // We're at the high surrogate character. Check the index, then get the low one.
                    int lowIndex = startIndex + 1;
                    if (lowIndex >= state.CurrentText.Length)
                        return;

                    bool isLowIndexLow = char.IsLowSurrogate(state.CurrentText[lowIndex]);
                    if (isLowIndexLow && length == 1)
                        length++;
                }
                else if (isStartIndexLow)
                {
                    // We're at the low surrogate character. Check the index, then get the high one.
                    int highIndex = startIndex - 1;
                    if (highIndex >= state.CurrentText.Length)
                        return;

                    bool isHighIndexHigh = char.IsHighSurrogate(state.CurrentText[highIndex]);
                    if (isHighIndexHigh)
                    {
                        startIndex = highIndex;
                        if (length == 1)
                            length++;
                    }
                }
            }

            // Remove this amount of characters
            Wipe(ref state, startIndex, length, step);
        }

        internal static void Wipe(ref TermReaderState state, int start, int count, bool step = true)
        {
            // Wipe everything
            BlankOut(ref state);

            // Now, remove the requested text
            if (step)
                PositioningTools.GoBack(count, ref state);
            state.CurrentText.Remove(start, count);
            state.canInsert = true;

            // Refresh
            RefreshPrompt(ref state);
        }

        internal static void WipeAll(ref TermReaderState state)
        {
            // Wipe everything
            BlankOut(ref state);

            // Now, remove everything
            PositioningTools.GoLeftmost(ref state);
            state.CurrentText.Clear();
            state.canInsert = true;

            // Refresh
            RefreshPrompt(ref state);
        }

        internal static void BlankOut(ref TermReaderState state)
        {
            // Wipe everything
            string renderedBlanks;
            if (state.OneLineWrap)
                renderedBlanks = new(' ', state.LongestSentenceLengthFromLeftForFirstLine);
            else
            {
                string[] incompleteSentences = ConsoleMisc.GetWrappedSentences(state.CurrentText.ToString(), state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
                int inputWidth = ConsoleChar.EstimateCellWidth(state.InputPromptLastLine);
                int length = 0;
                for (int i = 0; i < incompleteSentences.Length; i++)
                {
                    string sentence = incompleteSentences[i];
                    int sentenceWidth = ConsoleChar.EstimateCellWidth(sentence);
                    length += sentenceWidth;
                    if (sentenceWidth + (i == 0 ? inputWidth : 0) == state.MaximumInputPositionLeft && i < incompleteSentences.Length - 1)
                        length++;
                }
                renderedBlanks = new(' ', length);
            }
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            TextWriterColor.WriteForReader(renderedBlanks, state.settings, false);
        }

        internal static void RefreshPrompt(ref TermReaderState state, int steps = 0, bool backward = false)
        {
            // Determine the foreground and the background color
            var foreground = state.Commentized ? new Color(ConsoleColors.Green) : state.Settings.InputForegroundColor;
            var background = state.Settings.InputBackgroundColor;

            // Determine if the input prompt text is either overflowing or intentionally placing
            // the newlines using the "incomplete sentences" feature, then refresh the input
            // prompt.
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeftBegin, state.InputPromptTopBegin);
            state.writingPrompt = true;
            TextWriterColor.WriteForReaderColorBack(state.InputPromptText, state.Settings, false, state.Settings.InputPromptForegroundColor, background);
            state.writingPrompt = false;

            // Now, render the current text
            string renderedText =
                state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) :
                state.concealing ? new string(' ', ConsoleChar.EstimateCellWidth(state.currentText.ToString())) :
                state.currentText.ToString();

            // Take highlighting into account
            renderedText = GetHighlightedInput(renderedText, state);

            // Change the rendered text according to the mode.
            int spacesLength = 0;
            if (state.OneLineWrap)
            {
                // We're in the one-line wrap mode!
                int longestSentenceLength = state.LongestSentenceLengthFromLeftForFirstLine;
                string[] incompleteSentences = ConsoleMisc.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = GetOneLineWrappedSentenceToRender(incompleteSentences, state);
                spacesLength = longestSentenceLength - state.settings.LeftMargin - ConsoleChar.EstimateCellWidth(renderedText);
            }

            // Now, render the text
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            TextWriterColor.WriteForReaderColorBack(renderedText, state.settings, false, foreground, background);
            if (state.OneLineWrap)
                TextWriterColor.WriteForReaderColorBack(new string(' ', spacesLength < 0 ? 0 : spacesLength == 0 && !state.OneLineWrap ? 1 : spacesLength), state.settings, false, foreground, background);

            // If stepping, go either backward or forward, and commit the positioning changes.
            if (steps > 0)
            {
                if (backward)
                    PositioningTools.GoBack(steps, ref state);
                else
                    PositioningTools.GoForward(steps, ref state);
            }
            else
                PositioningTools.SeekTo(state.CurrentTextPos, ref state);
            PositioningTools.Commit(state);
        }

        internal static string GetHighlightedInput(string renderedText, TermReaderState state)
        {
            // If any of the items is null, return the original string
            if (state.Settings is null || state.Settings.SyntaxHighlighter is null || state.Settings.SyntaxHighlighter.Components is null)
                return renderedText;

            // Condition for highlight
            bool highlight =
                state.CurrentText.Length > 0 &&
                !state.PasswordMode &&
                !state.Commentized &&
                state.Settings.SyntaxHighlighterEnabled &&
                state.Settings.SyntaxHighlighter.Components.Count > 0;

            // If highlight is not enabled, return the original string
            if (!highlight)
                return renderedText;

            // Now, highlight the rendered text
            StringBuilder finalString = new(renderedText);
            List<(Match match, Color fgColor, Color bgColor, bool fgEnabled, bool bgEnabled)> finalMatches = [];
            foreach (var component in state.Settings.SyntaxHighlighter.Components.Values)
            {
                var match = component.ComponentMatch;
                var fgColor = component.ComponentForegroundColor;
                var bgColor = component.ComponentBackgroundColor;
                bool fgEnabled = component.UseForegroundColor;
                bool bgEnabled = component.UseBackgroundColor;
                if (match is null || fgColor is null || bgColor is null)
                    continue;

                // Now, match the original string
                Match[] matches = match.Matches(renderedText).OfType<Match>().ToArray();
                foreach (var finalMatch in matches)
                {
                    ConsoleLogger.Debug("Highlighting match {0} with {1}, {2}, {3}, {4} [idx: {5}]", finalMatch.Value, fgColor, bgColor, fgEnabled, bgEnabled, finalMatch.Index);
                    finalMatches.Add((finalMatch, fgColor, bgColor, fgEnabled, bgEnabled));
                }
            }

            // Sort the matches and apply
            finalMatches = [.. finalMatches.OrderByDescending((match) => match.match?.Index)];
            foreach (var (match, fgColor, bgColor, fgEnabled, bgEnabled) in finalMatches)
            {
                var idx = match.Index;
                var endIdx = idx + match.Length;
                if (fgEnabled)
                    finalString.Insert(endIdx, ConsoleColoring.RenderSetConsoleColor(state.Settings.InputForegroundColor));
                if (bgEnabled)
                    finalString.Insert(endIdx, ConsoleColoring.RenderSetConsoleColor(state.Settings.InputBackgroundColor, true));
                if (fgEnabled)
                    finalString.Insert(idx, fgColor.VTSequenceForeground());
                if (bgEnabled)
                    finalString.Insert(idx, bgColor.VTSequenceBackground());
            }

            // Return the final string
            return finalString.ToString();
        }

        internal static string GetOneLineWrappedSentenceToRender(string[] incompleteSentences, TermReaderState state) =>
            GetOneLineWrappedSentenceToRender(incompleteSentences, state.CurrentTextPos);

        internal static string GetOneLineWrappedSentenceToRender(string[] incompleteSentences, int targetIndex)
        {
            string finalRenderedString = "";

            // Deal with trying to count the characters incrementally for each incomplete sentence until we find an index
            // that we want, then give the rendered string back.
            int currentIndex = 0;
            foreach (string sentence in incompleteSentences)
            {
                finalRenderedString = sentence;
                for (int i = 0; i < sentence.Length && currentIndex != targetIndex; i++)
                    currentIndex++;
                if (currentIndex == targetIndex)
                    break;
            }

            // Return it!
            return finalRenderedString;
        }

        internal static string GetLineFromCurrentPos(string[] incompleteSentences, TermReaderState state) =>
            GetLineFromPos(incompleteSentences, state.CurrentTextPos);

        internal static string GetLineFromPos(string[] incompleteSentences, int index)
        {
            // Deal with trying to count the characters incrementally for each incomplete sentence until we find an index
            // that we want, then give the rendered string back.
            int currentIndex = 0;
            foreach (string sentence in incompleteSentences)
            {
                currentIndex += sentence.Length;
                bool skip = currentIndex < index;
                if (!skip)
                    return sentence;
            }
            return "";
        }
    }
}
