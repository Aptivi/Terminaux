
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
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class GoLeft : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
            new ConsoleKeyInfo('\u0002', ConsoleKey.B, false, false, true)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the text, bail.
            if (state.CurrentTextPos == 0)
                return;

            // Just set the position one character closer to the input start position!
            PositioningTools.GoBack(ref state);
        }
    }
}
