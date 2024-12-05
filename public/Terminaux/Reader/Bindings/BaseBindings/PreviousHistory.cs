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
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class PreviousHistory : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the start of the history, bail.
            if (!ConditionalTools.ShouldNot(state.CurrentHistoryPos == 0, state))
                return;

            // If we're in the password mode, bail.
            if (state.PasswordMode)
                return;

            // If we're in the disabled history mode, bail.
            if (!state.settings.HistoryEnabled)
                return;

            // Wipe everything
            TermReaderTools.WipeAll();

            // Now, write the history entry
            state.currentHistoryPos--;
            string history = state.History[state.currentHistoryPos];
            TermReaderTools.InsertNewText(history, true);
        }
    }
}
