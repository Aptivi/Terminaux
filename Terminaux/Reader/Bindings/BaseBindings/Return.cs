﻿//
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
    internal class Return : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            // for Windows
            new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false),
            new ConsoleKeyInfo('\n', ConsoleKey.J, false, false, true),

            // for Linux
            new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false),
        ];

        /// <inheritdoc/>
        public override bool IsExit => true;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            if (state.Commentized)
                state.CurrentText.Clear();
        }
    }
}
