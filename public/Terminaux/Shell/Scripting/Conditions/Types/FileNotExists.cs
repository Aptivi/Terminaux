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

namespace Terminaux.Shell.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if the value of the MESH variable is a file path and doesn't exist
    /// </summary>
    public class FileNotExistsCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "filenex";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 1;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 2;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable) =>
            MESHOperators.MESHVariableFileDoesNotExist(FirstVariable);

    }
}
