
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
using Terminaux.Base;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class Refresh : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('r', ConsoleKey.R, false, true, false),
        };

        /// <inheritdoc/>
        public override bool IsExit => false;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Determine if the input prompt text is either overflowing or intentionally placing
            // the newlines using the "incomplete sentences" feature, then refresh the input
            // prompt.
            int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin;
            string[] wrapped = ConsoleExtensions.GetWrappedSentences(state.InputPromptText, longestSentenceLength, state.inputPromptLeft + state.settings.LeftMargin);
            ConsoleTools.ActionSetCursorPosition(state.settings.LeftMargin, state.InputPromptTop - wrapped.Length + 1);
            ConsoleTools.ActionWriteString(state.InputPromptText, state.Settings);

            // Now, render the current text
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
            string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, state.inputPromptLeft + state.settings.LeftMargin);
            if (state.OneLineWrap)
            {
                // We're in the one-line wrap mode!
                longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
            }
            else
            {
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - incompleteSentences[incompleteSentences.Length - 1].Length), state.settings);
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
