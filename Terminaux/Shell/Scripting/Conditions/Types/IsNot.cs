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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System;
using Terminaux.Shell.Scripting.Conditions;

namespace Terminaux.Shell.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable is not of the correct type
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
                throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("Data type {0} specified is invalid."), SecondVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return !dataFunc(FirstVariable);
        }

    }
}
