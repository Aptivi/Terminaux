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
    internal class Home : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\0', ConsoleKey.Home, false, false, false),
            new ConsoleKeyInfo('\u0001', ConsoleKey.A, false, false, true)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (state.CurrentTextPos == 0)
                return;

            // Just set the positions to zero!
            if (state.OneLineWrap)
            {
                string renderedText = state.CurrentText.ToString();
                int longestSentenceLength = ConsoleTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                PositioningTools.GoBackOneLineWrapAware(state.currentTextPos, ref state, incompleteSentences);
            }
            else
                PositioningTools.GoBack(state.currentTextPos, ref state);
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
