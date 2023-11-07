
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;
using Terminaux.Reader.Tools;
using Terminaux.Sequences.Tools;

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
                SpinWait.SpinUntil(() => ConsoleWrappers.ActionKeyAvailable() || interrupting);
                if (interrupting)
                {
                    interrupting = false;
                    if (ConsoleWrappers.ActionKeyAvailable())
                        ConsoleWrappers.ActionReadKey(true);
                    return new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false);
                }
                else
                    return ConsoleWrappers.ActionReadKey(true);
            }
            else
                return ConsoleWrappers.ActionReadKey(true);
        }

        internal static int GetMaximumInputLength(TermReaderState state)
        {
            // First, get the maximum width and height
            int width = ConsoleWrappers.ActionWindowWidth();
            int height = ConsoleWrappers.ActionBufferHeight();

            // Then, get the length for each width and height, subtracting it by one to avoid wrapping
            int marginWidth = width - state.settings.RightMargin - state.settings.LeftMargin;
            int cells = marginWidth * height;
            int length = cells - 1;

            // We need to get the longest sentence width in case we encounter a multiline input prompt
            int longestSentenceLength = width - state.settings.RightMargin - 1;
            string[] inputPromptLines = state.InputPromptText.SplitNewLines();
            string inputPromptLastLine = VtSequenceTools.FilterVTSequences(inputPromptLines[inputPromptLines.Length - 1]);
            int inputPromptLineTimes = inputPromptLines.Length - 1;

            // Subtract the length accordingly
            for (int i = 0; i < inputPromptLineTimes; i++)
                length -= longestSentenceLength;
            length -= inputPromptLastLine.Length + inputPromptLineTimes;

            // Return the number of length available
            return length;
        }

        internal static void InsertNewText(ref TermReaderState state, string newText, bool append = false, bool step = true)
        {
            // If we can't insert, do nothing
            if (!state.CanInsert)
                return;

            // Get the longest sentence width and insert the character
            int width = ConsoleWrappers.ActionWindowWidth();
            int height = ConsoleWrappers.ActionBufferHeight();
            int longestSentenceLength = width - state.settings.RightMargin;
            string[] incompleteSentencesPrimary = ConsoleExtensions.GetWrappedSentences(state.CurrentText.ToString(), longestSentenceLength, state.inputPromptLeft + state.settings.LeftMargin);
            if (append)
                state.CurrentText.Append(newText);
            else
                state.CurrentText.Insert(state.CurrentTextPos, newText);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            string[] incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, state.inputPromptLeft);

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                longestSentenceLength = width - state.settings.RightMargin - state.inputPromptLeft - 1;
                incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                if (step)
                    PositioningTools.GoForwardOneLineWrapAware(newText.Length, ref state);
            }
            else
            {
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText, state.settings);
                if (step)
                {
                    PositioningTools.HandleTopChangeForInput(ref state);
                    PositioningTools.GoForward(newText.Length, ref state);
                    int length = GetMaximumInputLength(state);
                    if (renderedText.Length == length)
                    {
                        state.canInsert = false;
                    }
                    else
                    {
                        if (state.inputPromptTop + incompleteSentences.Length > height)
                        {
                            state.inputPromptTop -= incompleteSentences.Length - incompleteSentencesPrimary.Length;
                            state.currentCursorPosTop -= incompleteSentences.Length - incompleteSentencesPrimary.Length;
                        }
                        if (state.currentCursorPosTop >= height)
                        {
                            state.currentCursorPosTop = height - 1;
                            state.inputPromptTop -= 1;
                            ConsoleWrappers.ActionWriteLine();
                        }
                    }
                }
            }
            ConsoleWrappers.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }

        internal static void RefreshPrompt(ref TermReaderState state, int steps = 0, bool backward = false)
        {
            // Determine if the input prompt text is either overflowing or intentionally placing
            // the newlines using the "incomplete sentences" feature, then refresh the input
            // prompt.
            int longestSentenceLength = ConsoleWrappers.ActionWindowWidth() - state.settings.RightMargin;
            string[] wrapped = ConsoleExtensions.GetWrappedSentences(state.InputPromptText, longestSentenceLength, state.inputPromptLeft + state.settings.LeftMargin);
            ConsoleWrappers.ActionSetCursorPosition(state.settings.LeftMargin, state.InputPromptTop - wrapped.Length + 1);
            ConsoleWrappers.ActionWriteString(state.InputPromptText, state.Settings);

            // Now, render the current text
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            string[] incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength - state.settings.LeftMargin, wrapped[wrapped.Length - 1].Length);
            if (state.OneLineWrap)
            {
                // We're in the one-line wrap mode!
                longestSentenceLength = ConsoleWrappers.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText + new string(' ', longestSentenceLength - state.settings.LeftMargin - renderedText.Length), state.settings);
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
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText + new string(' ', longestSentenceLength - state.settings.LeftMargin - incompleteSentences[incompleteSentences.Length - 1].Length), state.settings);
                if (steps > 0)
                {
                    if (backward)
                        PositioningTools.GoBack(steps, ref state);
                    else
                        PositioningTools.GoForward(steps, ref state);
                }
            }
            ConsoleWrappers.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
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
