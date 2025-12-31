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

using System;
using Terminaux.Base;

namespace Terminaux.Shell.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a MESH variable is not of the correct type
    /// </summary>
    public class IsNotCondition : BaseCondition, ICondition
    {

        /// <inheritdoc/>
        public override string ConditionName => "isnot";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 2;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 3;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            // SecondVariable is actually a data type needed for parsing.
            if (!IsCondition.DataTypes.TryGetValue(SecondVariable, out Func<string, bool>? dataFunc))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_SCRIPTING_CONDITIONS_EXCEPTION_INVALIDDATATYPE"), SecondVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return !dataFunc(FirstVariable);
        }

    }
}
