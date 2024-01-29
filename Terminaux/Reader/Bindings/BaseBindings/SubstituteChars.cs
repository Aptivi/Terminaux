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

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class SubstituteChars : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\u0014', ConsoleKey.T, false, false, true)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // First, check the boundaries
            if (state.CurrentText.Length <= 1)
                return;
            if (state.CurrentTextPos >= state.CurrentText.Length - 1)
                return;

            // Then, get the two characters
            char first = state.CurrentText[state.CurrentTextPos];
            char second = state.CurrentText[state.CurrentTextPos + 1];

            // Now, substitute them
            (state.CurrentText[state.CurrentTextPos], state.CurrentText[state.CurrentTextPos + 1]) = (second, first);
            TermReaderTools.RefreshPrompt(ref state);
        }
    }
}
