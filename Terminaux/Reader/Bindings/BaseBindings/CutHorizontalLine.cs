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
    internal class CutHorizontalLine : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\\', ConsoleKey.Oem5, false, true, false)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Remove whitespaces before and after the cursor
            string beforeCursor = state.CurrentText.ToString().Substring(0, state.CurrentTextPos);
            string afterCursor = state.CurrentText.ToString().Substring(state.CurrentTextPos);
            int whitespacesBefore = beforeCursor.Length - beforeCursor.TrimEnd().Length;
            int whitespacesAfter = afterCursor.Length - afterCursor.TrimStart().Length;
            state.CurrentText.Remove(state.CurrentTextPos, whitespacesAfter);
            state.CurrentText.Remove(state.CurrentTextPos - whitespacesBefore, whitespacesBefore);
            state.canInsert = true;
            TermReaderTools.RefreshPrompt(ref state, whitespacesBefore, true);
        }
    }
}
