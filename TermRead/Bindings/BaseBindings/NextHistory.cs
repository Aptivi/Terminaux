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

using Extensification.StringExts;
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
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleWrapper.Write(" ".Repeat(length));
            PositioningTools.SeekTo(0, ref state);
            ConsoleWrapper.SetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);

            // Now, write the history entry
            TermReaderState.currentHistoryPos++;
            string history = state.CurrentHistoryPos == state.History.Count ? "" : state.History[TermReaderState.currentHistoryPos];
            ConsoleWrapper.Write(history);
            state.CurrentText.Append(history);
            PositioningTools.GoForward(history.Length, true, ref state);
            ConsoleWrapper.SetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
