//
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Reader.Tools;
using Terminaux.Sequences;
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
        /// Specifies whether the terminal reader is busy waiting for input or not
        /// </summary>
        public static bool Busy =>
            isWaitingForInput;

        /// <summary>
        /// Interrupts the reading process
        /// </summary>
        public static void Interrupt()
        {
            if (isWaitingForInput)
                interrupting = true;
        }

        /// <summary>
        /// Sets the history
        /// </summary>
        /// <param name="History">List of history entries</param>
        public static void SetHistory(List<string> History)
        {
            TermReaderState.history = History;
            TermReaderState.currentHistoryPos = History.Count;
        }

        /// <summary>
        /// Clears the history
        /// </summary>
        public static void ClearHistory()
        {
            TermReaderState.history.Clear();
            TermReaderState.currentHistoryPos = 0;
        }

        /// <summary>
        /// Refreshes the reader prompt
        /// </summary>
        public static void Refresh()
        {
            if (TermReader.states.Count == 0)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;

            // Check to see if the console has been resized before
            if (ConsoleResizeHandler.WasResized(false))
            {
                // It's been resized. Now, get the current text position
                int textPos = state.CurrentTextPos;

                // Go to the leftmost position
                PositioningTools.GoLeftmost(ref state);

                // Fix the values up
                state.inputPromptLeft = state.InputPromptLastLineLength;
                state.inputPromptTop = state.InputPromptTopBegin + state.InputPromptHeight - 1;
                if (state.inputPromptTop >= ConsoleWrapper.WindowHeight)
                    state.inputPromptTop = ConsoleWrapper.WindowHeight - 1;
                state.currentCursorPosLeft = state.InputPromptLeft;
                state.currentCursorPosTop = state.InputPromptTop;

                // Go to the old position
                PositioningTools.SeekTo(textPos, ref state);
                TermReader.cachedPos = (state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            }

            // Refresh the prompt
            RefreshPrompt(ref state);

            // Save the state
            TermReaderState.SaveState(state);
        }

        /// <summary>
        /// Gets the maximum input length for the current reader session
        /// </summary>
        /// <returns>The maximum input length of an input. -1 if there is no reader.</returns>
        public static int GetMaximumInputLength()
        {
            if (TermReader.states.Count == 0)
                return -1;

            // Get the current state
            var state = TermReaderState.CurrentState;
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
            if (TermReader.states.Count == 0)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;
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
            if (TermReader.states.Count == 0)
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
            if (TermReader.states.Count == 0)
                return;

            // Get the current state
            var state = TermReaderState.CurrentState;
            RemoveText(ref state, startIndex, length, step);
            TermReaderState.SaveState(state);
        }

        internal static ConsoleKeyInfo GetInput(bool interruptible)
        {
            if (interruptible)
            {
                SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable || interrupting);
                if (interrupting)
                {
                    interrupting = false;
                    if (ConsoleWrapper.KeyAvailable)
                        ConsoleWrapper.ReadKey(true);
                    return new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);
                }
                else
                    return ConsoleWrapper.ReadKey(true);
            }
            else
                return ConsoleWrapper.ReadKey(true);
        }

        internal static int GetMaximumInputLength(TermReaderState state)
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

            // Return the number of length available
            return length;
        }

        internal static void InsertNewText(ref TermReaderState state, string newText, bool append = false, bool step = true)
        {
            // If we can't insert, do nothing
            if (!state.CanInsert)
                return;

            // Get the longest sentence width and crop the text appropriately
            int longest = GetMaximumInputLength(state);
            if (state.CurrentText.Length + newText.Length > longest && state.settings.LimitConsoleChars)
            {
                state.canInsert = false;
                int len = longest - (state.CurrentText.Length + newText.Length);
                if (len < 0)
                    len = longest - state.CurrentText.Length;
                newText = newText.Substring(0, len);
            }

            // Now, insert the text
            if (append)
                state.CurrentText.Append(newText);
            else
                state.CurrentText.Insert(state.CurrentTextPos, newText);

            // Refresh
            RefreshPrompt(ref state, step ? newText.Length : 0);
        }

        internal static void RemoveText(ref TermReaderState state, int length, bool step = false) =>
            RemoveText(ref state, state.CurrentTextPos, length, step);

        internal static void RemoveText(ref TermReaderState state, int startIndex, int length, bool step = false)
        {
            // Remove this amount of characters
            int old = state.CurrentText.Length;
            state.CurrentText.Remove(startIndex, length);
            state.canInsert = true;

            // Refresh
            RefreshPrompt(ref state, step ? length : 0, true, old - state.CurrentText.Length);
        }

        internal static void RefreshPrompt(ref TermReaderState state, int steps = 0, bool backward = false, int spaces = 0)
        {
            // Determine the foreground and the background color
            var foreground = state.Commentized ? new Color(ConsoleColors.Green) : state.Settings.InputForegroundColor;
            var background = state.Settings.InputBackgroundColor;

            // Determine if the input prompt text is either overflowing or intentionally placing
            // the newlines using the "incomplete sentences" feature, then refresh the input
            // prompt.
            int longestSentenceLength = state.LongestSentenceLengthFromLeft;
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeftBegin, state.InputPromptTopBegin);
            state.writingPrompt = true;
            TextWriterColor.WriteForReaderColorBack(state.InputPromptText, state.Settings, false, foreground, background);
            state.writingPrompt = false;

            // Now, render the current text
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            string[] incompleteSentences = TextTools.GetWrappedSentences(renderedText, longestSentenceLength - state.settings.LeftMargin, state.InputPromptLastLineLength);

            // Check to see if we're at the end of the maximum input length
            int maxLength = GetMaximumInputLength(state);
            if (renderedText.Length - 1 >= maxLength)
            {
                (int offset, int take) = GetRenderedStringOffsets(incompleteSentences, state);
                string[] intermediateSplit = incompleteSentences.Skip(offset).Take(take).ToArray();
                string intermediateText = string.Join("", intermediateSplit);
                renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, intermediateText.Length) : intermediateText;
            }

            // Take highlighting into account
            string originalText = renderedText;
            renderedText = GetHighlightedInput(renderedText, state);

            // Now, render the input.
            if (state.OneLineWrap)
            {
                // We're in the one-line wrap mode!
                longestSentenceLength = state.LongestSentenceLengthFromLeftForFirstLine;
                incompleteSentences = TextTools.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WriteForReaderColorBack(renderedText + new string(' ', longestSentenceLength - state.settings.LeftMargin - originalText.Length), state.settings, false, foreground, background);
                if (steps > 0)
                {
                    if (backward)
                        PositioningTools.GoBackOneLineWrapAware(steps, ref state);
                    else
                        PositioningTools.GoForwardOneLineWrapAware(steps, ref state);
                }
            }
            else
            {
                // We're in the multi-line wrap mode!
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WriteForReaderColorBack(renderedText, state.settings, false, foreground, background);
                
                // Render appropriate amount of spaces
                if (spaces == 0 && state.RightMargin == 0)
                    TextWriterColor.WriteForReaderColorBack(ConsoleClearing.GetClearLineToRightSequence(), state.settings, false, foreground, background);
                else
                {
                    incompleteSentences = TextTools.GetWrappedSentences(renderedText + " ", longestSentenceLength - state.settings.LeftMargin, state.InputPromptLeft - state.settings.LeftMargin);
                    string last = VtSequenceTools.FilterVTSequences(incompleteSentences[incompleteSentences.Length - 1]);
                    int spacesLength = longestSentenceLength - state.RightMargin - last.Length - (incompleteSentences.Length == 1 ? state.InputPromptLeft - state.settings.LeftMargin : 0);
                    if (spacesLength < 0)
                        spacesLength = 0;
                    if (spaces > 0)
                        spacesLength = spaces;
                    TextWriterColor.WriteForReaderColorBack(new string(' ', spacesLength), state.settings, false, foreground, background);
                }

                // If stepping, go either backward or forward.
                if (steps > 0)
                {
                    if (backward)
                        PositioningTools.GoBackOneLineWrapDisabled(steps, ref state);
                    else
                        PositioningTools.GoForwardOneLineWrapDisabled(steps, ref state);
                }
            }
            PositioningTools.Commit(state);
        }

        internal static string GetHighlightedInput(string renderedText, TermReaderState state)
        {
            // Condition for highlight
            bool highlight =
                state.CurrentText.Length > 0 &&
                !state.PasswordMode &&
                !state.Commentized &&
                state.Settings is not null &&
                state.Settings.SyntaxHighlighterEnabled &&
                state.Settings.SyntaxHighlighter is not null &&
                state.Settings.SyntaxHighlighter.Components is not null &&
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

                // Now, match the original string
                Match[] matches = match.Matches(renderedText).OfType<Match>().ToArray();
                foreach (var finalMatch in matches)
                    finalMatches.Add((finalMatch, fgColor, bgColor, fgEnabled, bgEnabled));
            }

            // Sort the matches and apply
            finalMatches = [.. finalMatches.OrderByDescending((match) => match.match.Index)];
            foreach (var (match, fgColor, bgColor, fgEnabled, bgEnabled) in finalMatches)
            {
                var idx = match.Index;
                var endIdx = idx + match.Length;
                if (fgEnabled)
                    finalString.Insert(endIdx, state.Settings.InputForegroundColor.VTSequenceForeground);
                if (bgEnabled)
                    finalString.Insert(endIdx, ColorTools.RenderSetConsoleColor(state.Settings.InputBackgroundColor, true));
                if (fgEnabled)
                    finalString.Insert(idx, fgColor.VTSequenceForeground);
                if (bgEnabled)
                    finalString.Insert(idx, bgColor.VTSequenceBackground);
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

        internal static (int offset, int take) GetRenderedStringOffsets(string[] incompleteSentences, TermReaderState state)
        {
            // Deal with trying to count the characters incrementally for each incomplete sentence until we find an index
            // that we want, then give the rendered string back.
            int currentIndex = 0;
            int skipFirst = 0;
            int take = 0;
            List<string> rendered = [];
            foreach (string sentence in incompleteSentences)
            {
                bool incrementSkip = currentIndex < state.CurrentTextPos;
                currentIndex += sentence.Length;
                take++;
                if (take > ConsoleWrapper.BufferHeight)
                {
                    take--;
                    if (incrementSkip)
                        skipFirst++;
                    else
                        break;
                }
            }

            // Return it!
            return (skipFirst, take);
        }
    }
}
