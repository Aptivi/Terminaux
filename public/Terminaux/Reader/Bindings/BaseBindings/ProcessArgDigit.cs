//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class ProcessArgDigit : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override bool ArgumentNumberIsRepetition => false;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Check to see if this key is a minus key
            bool isMinus = state.PressedKey.KeyChar == '-';
            if (isMinus)
            {
                // Add or remove the negation
                if (state.argNumbers.Count > 0 && state.argNumbers[0] == '-')
                    state.argNumbers.RemoveAt(0);
                else
                {
                    state.argNumbers.Insert(0, '-');
                    if (state.argNumbers.Count == 1)
                        state.argNumbers.Add('1');
                }
            }
            else
            {
                // Write the argument digit implicitly
                state.argNumbers.Add(state.PressedKey.KeyChar);
            }
        }
    }
}
