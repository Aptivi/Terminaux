/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using TermRead.Reader;
using TermRead.Tools;

namespace TermRead.Bindings.BaseBindings
{
    internal class NextHistory : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } = 
        { 
            new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the end of the history, bail.
            if (state.CurrentHistoryPos >= state.History.Count)
                return;

            // If we're in the password mode, bail.
            if (state.PasswordMode)
                return;

            // If we're in the disabled history mode, bail.
            if (!TermReaderSettings.HistoryEnabled)
                return;

            // Wipe everything
            int length = state.CurrentText.Length;
            state.CurrentText.Clear();
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string renderedBlanks = new(' ', longestSentenceLength);
                ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrapperTools.ActionWriteString(renderedBlanks);
                PositioningTools.SeekToOneLineWrapAware(0, ref state, new string[] { "" });
                state.currentTextPos = 0;
                state.currentCursorPosLeft = state.InputPromptLeft;
                state.currentCursorPosTop = state.InputPromptTop;
            }
            else
            {
                ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrapperTools.ActionWriteString(new string(' ', length));
                PositioningTools.SeekTo(0, ref state);
            }
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);

            // Now, write the history entry
            TermReaderState.currentHistoryPos++;
            string history = state.CurrentHistoryPos == state.History.Count ? "" : state.History[TermReaderState.currentHistoryPos];

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(history, longestSentenceLength, 0);
                string renderedHistory = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, history.Length) : history;
                ConsoleWrapperTools.ActionWriteString(renderedHistory + new string(' ', longestSentenceLength - renderedHistory.Length));
                state.CurrentText.Append(history);
                PositioningTools.GoForwardOneLineWrapAware(history.Length, ref state, incompleteSentences);
            }
            else
            {
                ConsoleWrapperTools.ActionWriteString(history);
                state.CurrentText.Append(history);
                PositioningTools.HandleTopChangeForInput(ref state);
                PositioningTools.GoForward(history.Length, true, ref state);
            }
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
