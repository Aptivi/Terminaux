
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
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
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
            if (!state.settings.HistoryEnabled)
                return;

            // Wipe everything
            int length = state.CurrentText.Length;
            state.CurrentText.Clear();
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                string renderedBlanks = new(' ', longestSentenceLength);
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedBlanks, state.settings);
                PositioningTools.SeekToOneLineWrapAware(0, ref state);
                state.currentTextPos = 0;
                state.currentCursorPosLeft = state.InputPromptLeft;
                state.currentCursorPosTop = state.InputPromptTop;
            }
            else
            {
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(new string(' ', length), state.settings);
                PositioningTools.SeekTo(0, ref state);
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);

            // Now, write the history entry
            TermReaderState.currentHistoryPos++;
            string history = state.CurrentHistoryPos == state.History.Count ? "" : state.History[TermReaderState.currentHistoryPos];

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(history, longestSentenceLength, 0);
                string renderedHistory = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, history.Length) : history;
                ConsoleTools.ActionWriteString(renderedHistory + new string(' ', longestSentenceLength - renderedHistory.Length), state.settings);
                state.CurrentText.Append(history);
                PositioningTools.GoForwardOneLineWrapAware(history.Length, ref state);
            }
            else
            {
                ConsoleTools.ActionWriteString(history, state.settings);
                state.CurrentText.Append(history);
                PositioningTools.HandleTopChangeForInput(ref state);
                PositioningTools.GoForward(history.Length, true, ref state);
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
