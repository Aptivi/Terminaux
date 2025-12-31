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
using System.Collections.Generic;
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_SEQUENCES_BUILDER_EXCEPTION_TYPENOTFOUND"), Convert.ToInt32(specificType));

            // Now, check the argument count
            int argCount = sequenceBuilders[specificType].argumentsRequired;
            ConsoleLogger.Debug("Passed {0} arguments, expected {1} arguments", arguments.Length, argCount);
            if (argCount < arguments.Length)
                throw new TerminauxException(LanguageTools.GetLocalized("T_SEQUENCES_BUILDER_EXCEPTION_ARGSMISSING").FormatString(argCount, arguments.Length) + $" {Convert.ToInt32(specificType)}");

            // Now, get the sequence and statically give arguments for performance to try to escape from DynamicInvoke
            var sequenceRegexGenerator = sequenceBuilders[specificType].generator;
            return DeterministicExecution(sequenceRegexGenerator, arguments);
        }

        /// <summary>
        /// Determines the VT sequence types from the given sequence
        /// </summary>
        /// <param name="sequence">The sequence to query</param>
        /// <returns>An array of a tuple of (<see cref="VtSequenceType"/>, <see cref="VtSequenceSpecificTypes"/>) containing information about a sequence type and a sequence command type, or an empty array if nothing is found</returns>
        public static IEnumerable<(VtSequenceType, VtSequenceSpecificTypes)> DetermineTypesFromSequence(string sequence)
        {
            // Get the sequence matches
            var matches = VtSequenceTools.MatchVTSequences(sequence);

            // Iterate through all matches to make a new tuple containing info that we'll need to add to an array
            foreach (var match in matches)
            {
                var seqType = match.Key;
                var sequences = match.Value;
                foreach (var sequenceInstance in sequences)
                {
                    var seqSpecificType = sequenceInstance.SpecificType;
                    yield return (seqType, seqSpecificType);
                }
            }
        }
    }
}
