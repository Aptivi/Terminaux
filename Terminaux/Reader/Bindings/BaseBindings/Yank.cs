
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
            int maxChars = TermReaderTools.GetMaximumInputLength(state);
            if (state.CurrentText.Length + toYank.Length > maxChars)
                toYank = toYank.Substring(0, maxChars - state.CurrentText.Length);
            
            TermReaderTools.InsertNewText(ref state, toYank);
        }
    }
}
