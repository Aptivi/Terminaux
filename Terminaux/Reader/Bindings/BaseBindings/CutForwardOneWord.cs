﻿
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
    internal class CutForwardOneWord : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('d', ConsoleKey.D, false, true, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (state.CurrentTextPos == 0)
                return;

            // Get the length of a word
            int steps = 0;
            for (int i = state.CurrentTextPos; i < state.CurrentText.Length; i++)
            {
                char currentChar = state.CurrentText[i];
                if (char.IsWhiteSpace(currentChar))
                    steps++;
                if (!char.IsWhiteSpace(currentChar))
                {
                    steps++;
                    if (i == state.CurrentText.Length - 1 || char.IsWhiteSpace(state.CurrentText[i + 1]))
                        break;
                }
            }
            state.KillBuffer.Append(state.CurrentText.ToString().Substring(state.CurrentTextPos, steps));
            state.CurrentText.Remove(state.CurrentTextPos, steps);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(TermReaderSettings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences
            if (state.OneLineWrap)
            {
                int longestSentenceLength = ConsoleTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
            }
            else
            {
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + new string(' ', steps));
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
