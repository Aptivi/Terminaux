
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
using Terminaux.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class Yank : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\u0019', ConsoleKey.Y, false, false, true)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Re-write the text and set the current cursor position as appropriate
            string toYank = state.KillBuffer.ToString();
            state.CurrentText.Insert(state.CurrentTextPos, toYank);
            string renderedText = state.PasswordMode ? new string(TermReaderSettings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
                PositioningTools.GoBackOneLineWrapAware(toYank.Length, ref state, incompleteSentences);
            }
            else
            {
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText);
                PositioningTools.GoBack(toYank.Length, ref state);
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
