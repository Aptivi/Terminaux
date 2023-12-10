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
using System.Threading;
using Terminaux.Base;
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

            // Check for maximum length
            int length = GetMaximumInputLength(state);
            if (state.currentText.Length + newText.Length >= length)
            {
                state.canInsert = false;
                newText = newText.Substring(0, length - state.currentText.Length);
            }

            // Get the longest sentence width and insert the character
            if (append)
                state.CurrentText.Append(newText);
            else
                state.CurrentText.Insert(state.CurrentTextPos, newText);

            // Refresh
            RefreshPrompt(ref state, step ? newText.Length : 0);
        }

        internal static void RefreshPrompt(ref TermReaderState state, int steps = 0, bool backward = false, int spaces = 0)
        {
            // Determine if the input prompt text is either overflowing or intentionally placing
            // the newlines using the "incomplete sentences" feature, then refresh the input
            // prompt.
            int longestSentenceLength = state.LongestSentenceLengthFromLeft;
            string[] wrapped = TextTools.GetWrappedSentences(state.InputPromptText, longestSentenceLength, state.inputPromptLeft + state.settings.LeftMargin);
            ConsoleWrapper.SetCursorPosition(state.settings.LeftMargin, state.InputPromptTop - wrapped.Length + 1);
            state.writingPrompt = true;
            TextWriterColor.WriteForReader(state.InputPromptText, state.Settings, false);
            state.writingPrompt = false;

            // Now, render the current text
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            string[] incompleteSentences = TextTools.GetWrappedSentences(renderedText, longestSentenceLength - state.settings.LeftMargin, wrapped[wrapped.Length - 1].Length);
            if (state.OneLineWrap)
            {
                // We're in the one-line wrap mode!
                longestSentenceLength = state.LongestSentenceLengthFromLeftForFirstLine;
                incompleteSentences = TextTools.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WriteForReader(renderedText + new string(' ', longestSentenceLength - state.settings.LeftMargin - renderedText.Length), state.settings, false);
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
                int spacesLength = longestSentenceLength - (state.inputPromptLeft + state.settings.LeftMargin) - incompleteSentences[incompleteSentences.Length - 1].Length;
                if (spacesLength < 0)
                    spacesLength = 0;
                if (spacesLength == 0)
                    spacesLength++;
                if (spaces > 0)
                    spacesLength = spaces;
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WriteForReader(renderedText + new string(' ', spacesLength), state.settings, false);
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
    }
}
