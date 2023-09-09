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
using Terminaux.Base;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class LowercaseOneWord : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('l', ConsoleKey.L, false, true, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (state.CurrentTextPos == state.CurrentText.Length)
                return;

            // Get the length of a word
            for (int i = state.CurrentTextPos; i < state.CurrentText.Length; i++)
            {
                state.CurrentText[i] = char.ToLower(state.CurrentText[i]);
                char currentChar = state.CurrentText[i];
                if (!char.IsWhiteSpace(currentChar))
                {
                    if (i == state.CurrentText.Length - 1 || char.IsWhiteSpace(state.CurrentText[i + 1]))
                        break;
                }
            }
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences
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
                ConsoleWrappers.ActionWriteString(renderedText, state.settings);
            }
            ConsoleWrappers.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
