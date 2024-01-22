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
using Terminaux.Base;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class PreviousHistory : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the history, bail.
            if (state.CurrentHistoryPos == 0)
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
                int longestSentenceLength = state.LongestSentenceLengthFromLeftForFirstLine;
                string renderedBlanks = new(' ', longestSentenceLength);
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WritePlain(renderedBlanks, state.settings);
                PositioningTools.SeekTo(0, ref state);
                state.currentTextPos = 0;
                state.currentCursorPosLeft = state.InputPromptLeft;
                state.currentCursorPosTop = state.InputPromptTop;
            }
            else
            {
                ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                TextWriterColor.WritePlain(new string(' ', length), state.settings);
                PositioningTools.SeekTo(0, ref state);
            }
            PositioningTools.Commit(state);

            // Now, write the history entry
            TermReaderState.currentHistoryPos--;
            string history = state.History[TermReaderState.currentHistoryPos];
            TermReaderTools.InsertNewText(ref state, history, true);
        }
    }
}
