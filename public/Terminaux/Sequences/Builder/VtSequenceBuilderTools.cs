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
            if (!Enum.IsDefined(typeof(VtSequenceSpecificTypes), specificType))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SEQUENCES_BUILDER_EXCEPTION_TYPENOTFOUND"), Convert.ToInt32(specificType));

            // Now, check the argument count
            int argCount = sequenceBuilders[specificType].argumentsRequired;
            if (argCount < arguments.Length)
                throw new TerminauxException(LanguageTools.GetLocalized("T_SEQUENCES_BUILDER_EXCEPTION_ARGSMISSING").FormatString(argCount, arguments.Length) + $" {Convert.ToInt32(specificType)}");

            // Now, get the sequence and statically give arguments for performance to try to escape from DynamicInvoke
            var sequenceRegexGenerator = sequenceBuilders[specificType].generator;
            return DeterministicExecution(sequenceRegexGenerator, arguments);
        }

        /// <summary>
        /// Determines the VT sequence type from the given sequence
        /// </summary>
        /// <param name="sequence">The sequence to query</param>
        /// <returns>A tuple of (<see cref="VtSequenceType"/>, <see cref="VtSequenceSpecificTypes"/>) containing information about a sequence type and a sequence command type</returns>
        /// <exception cref="TerminauxException"></exception>
        public static (VtSequenceType, VtSequenceSpecificTypes) DetermineTypeFromSequence(string sequence)
        {
            // First, get all the VT sequence types
            var seqTypeEnumNames = sequenceBuilders.Keys;

            // Then, iterate through all the sequence types until we find an appropriate one that matches the sequence
            foreach (var seqType in seqTypeEnumNames)
            {
                // Now, get the appropriate regex to check to see if there is a match.
                var builder = sequenceBuilders[seqType];
                var regex = builder.matchRegex;
                VtSequenceType sequenceType = builder.sequenceType;
                if (regex.IsMatch(sequence))
                {
                    // It's a match!
                    return (sequenceType, seqType);
                }
            }

            // If still not found, then throw
            throw new TerminauxException("Can't determine type from this sequence. Make sure that you've specified it correctly.");
        }
    }
}
