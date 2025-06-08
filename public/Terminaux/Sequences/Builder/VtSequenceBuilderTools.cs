//
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

using System;
using Terminaux.Base;
using Textify.General;

namespace Terminaux.Sequences.Builder
{
    /// <summary>
    /// VT sequence builder tools
    /// </summary>
    public static partial class VtSequenceBuilderTools
    {
        /// <summary>
        /// Builds a VT sequence using specific types
        /// </summary>
        /// <param name="specificType">A specific type</param>
        /// <param name="arguments">List of arguments. Review the parameters of the generated methods for more info.</param>
        /// <returns>A VT sequence that you can insert to the string builder or print to the console</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string BuildVtSequence(VtSequenceSpecificTypes specificType, params object[] arguments)
        {
            // Check the type
            ConsoleLogger.Debug("Type is {0}", specificType);
            if (!Enum.IsDefined(typeof(VtSequenceSpecificTypes), specificType))
                throw new TerminauxException("Cannot build VT sequence for nonexistent type {0}", Convert.ToInt32(specificType));

            // Now, check the argument count
            int argCount = sequenceBuilders[specificType].argumentsRequired;
            ConsoleLogger.Debug("Passed {0} arguments, expected {1} arguments", arguments.Length, argCount);
            if (argCount < arguments.Length)
                throw new TerminauxException("Cannot build VT sequence with missing arguments. Expected {0} arguments, got {1} arguments.".FormatString(argCount, arguments.Length) + $" {Convert.ToInt32(specificType)}");

            // Now, get the sequence and statically give arguments for performance to try to escape from DynamicInvoke
            var sequenceRegexGenerator = sequenceBuilders[specificType].generator;
            return DeterministicExecution(sequenceRegexGenerator, arguments);
        }
    }
}
