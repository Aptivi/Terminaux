﻿//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class CutBackwardOneWord : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override bool IsBindingOverridable => true;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (!ConditionalTools.ShouldNot(state.CurrentTextPos == 0, state))
                return;

            // Get the length of a word
            int steps = 0;
            for (int i = state.CurrentTextPos - 1; i >= 0; i--)
            {
                char currentChar = state.CurrentText[i];
                if (char.IsWhiteSpace(currentChar))
                    steps++;
                if (!char.IsWhiteSpace(currentChar))
                {
                    steps++;
                    if (i == 0 || char.IsWhiteSpace(state.CurrentText[i - 1]))
                        break;
                }
            }
            state.KillBuffer.Append(state.CurrentText.ToString().Substring(state.CurrentTextPos - steps, steps));
            TermReaderTools.RemoveText(state.CurrentTextPos - steps, steps, true);
        }
    }
}
