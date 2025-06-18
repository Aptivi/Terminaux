//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using SpecProbe.Software.Platform;
using System;
using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Shell.Scripting.Conditions.Types
{
    /// <summary>
    /// Checks to see if a UESH variable is of the correct type
    /// </summary>
    public class IsPlatCondition : BaseCondition, ICondition
    {
        internal static Dictionary<string, Func<bool>> Platforms = new()
        {
            { "win",        PlatformHelper.IsOnWindows },
            { "mac",        PlatformHelper.IsOnMacOS },
            { "unix",       PlatformHelper.IsOnUnix },
            { "android",    PlatformHelper.IsOnAndroid },
        };

        /// <inheritdoc/>
        public override string ConditionName => "isplat";

        /// <inheritdoc/>
        public override int ConditionPosition { get; } = 1;

        /// <inheritdoc/>
        public override int ConditionRequiredArguments { get; } = 2;

        /// <inheritdoc/>
        public override bool IsConditionSatisfied(string FirstVariable, string SecondVariable)
        {
            // FirstVariable is actually a platform needed for parsing.
            if (!Platforms.TryGetValue(FirstVariable, out Func<bool>? platFunc))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_SCRIPTING_CONDITIONS_EXCEPTION_INVALIDPLATFORM"), FirstVariable);

            // Get the action needed to get the comparer and test the condition defined above
            return platFunc();
        }

    }
}
