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
    internal class UndoChanges : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override bool IsBindingOverridable => true;

        /// <inheritdoc/>
        public override bool AppendsChangesList => false;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            if (!ConditionalTools.ShouldNot(state.changes.Count == 0, state))
                return;

            PositioningTools.GoLeftmost(ref state);
            state.CurrentText.Clear();
            TermReaderTools.InsertNewText(state.changes[0]);
            state.changes.Clear();
            state.RefreshRequired = true;
        }
    }
}
