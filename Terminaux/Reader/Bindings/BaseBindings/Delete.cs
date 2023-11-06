
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
using Terminaux.Base;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class Delete : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\0', ConsoleKey.Delete, false, false, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the end of the text, bail.
            if (state.CurrentTextPos == state.CurrentText.Length)
                return;

            // Remove one character from the current text position
            state.CurrentText.Remove(state.CurrentTextPos, 1);
            state.canInsert = true;

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleWrappers.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
            }
            else
            {
                ConsoleWrappers.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrappers.ActionWriteString(renderedText + " ", state.settings);
            }
            ConsoleWrappers.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
