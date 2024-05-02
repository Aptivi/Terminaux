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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class NextHistory : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // If we're at the end of the history, bail.
            if (!ConditionalTools.ShouldNot(state.CurrentHistoryPos >= state.History.Count, state))
                return;

            // If we're in the password mode, bail.
            if (!ConditionalTools.ShouldNot(state.PasswordMode, state))
                return;

            // If we're in the disabled history mode, bail.
            if (!ConditionalTools.ShouldNot(!state.settings.HistoryEnabled, state))
                return;

            // Wipe everything
            TermReaderTools.WipeAll();

            // Now, write the history entry
            TermReaderState.currentHistoryPos++;
            string history = state.CurrentHistoryPos == state.History.Count ? "" : state.History[TermReaderState.currentHistoryPos];
            TermReaderTools.InsertNewText(history, true);
        }
    }
}
