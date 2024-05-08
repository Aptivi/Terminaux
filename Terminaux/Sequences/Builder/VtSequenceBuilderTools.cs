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
using System.Text.RegularExpressions;
using Terminaux.Base;

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
        /// <param name="specificType"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string BuildVtSequence(VtSequenceSpecificTypes specificType, params object[] arguments)
        {
            // Check the type
            if (!Enum.IsDefined(typeof(VtSequenceSpecificTypes), specificType))
                throw new TerminauxException($"Cannot build VT sequence for nonexistent type {Convert.ToInt32(specificType)}");

            // Now, check the argument count
            int argCount = sequenceBuilders[specificType].argumentsRequired;
            if (argCount < arguments.Length)
                throw new TerminauxException($"Cannot build VT sequence with missing arguments. Expected {argCount} arguments, got {arguments.Length} arguments. {Convert.ToInt32(specificType)}");

            // Now, get the sequence and statically give arguments for performance to try to escape from DynamicInvoke
            var sequenceRegexGenerator = sequenceBuilders[specificType].generator;
            return DeterministicExecution(sequenceRegexGenerator, arguments);
        }

        /// <summary>
        /// Determines the VT sequence type from the given sequence
        /// </summary>
        /// <param name="sequence">The sequence to query</param>
        /// <returns>A tuple of (<see cref="VtSequenceType"/>, <see cref="VtSequenceSpecificTypes"/>) containing information about a sequence type and a sequence command type</returns>
        /// <exception cref="Exception"></exception>
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

        private static string DeterministicExecution(Delegate generator, params object[] arguments)
        {
            if (generator is Func<string> generatorNoArgs)
                return generatorNoArgs.Invoke();
            else if (generator is Func<string, string> generatorParameterized1)
                return generatorParameterized1.Invoke(arguments[0].ToString());
            else if (generator is Func<string, int, int, int, int, string> generatorParameterized2)
                return generatorParameterized2.Invoke(arguments[0].ToString(), (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, int, int, int, string> generatorParameterized3)
                return generatorParameterized3.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4], (int)arguments[5], (int)arguments[6], (int)arguments[7]);
            else if (generator is Func<int, string> generatorParameterized4)
                return generatorParameterized4.Invoke((int)arguments[0]);
            else if (generator is Func<int, int, string> generatorParameterized5)
                return generatorParameterized5.Invoke((int)arguments[0], (int)arguments[1]);
            else if (generator is Func<int, int, int, int, string> generatorParameterized6)
                return generatorParameterized6.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3]);
            else if (generator is Func<char, int, int, int, int, string> generatorParameterized7)
                return generatorParameterized7.Invoke((char)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, string> generatorParameterized8)
                return generatorParameterized8.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4]);
            else if (generator is Func<int, int, int, int, int, int, string> generatorParameterized9)
                return generatorParameterized9.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3], (int)arguments[4], (int)arguments[5]);
            else if (generator is Func<int, int, string, string> generatorParameterized10)
                return generatorParameterized10.Invoke((int)arguments[0], (int)arguments[1], arguments[2].ToString());
            else if (generator is Func<int, string, string> generatorParameterized11)
                return generatorParameterized11.Invoke((int)arguments[0], arguments[1].ToString());
            else if (generator is Func<int, int, int, string> generatorParameterized12)
                return generatorParameterized12.Invoke((int)arguments[0], (int)arguments[1], (int)arguments[2]);
            return generator.DynamicInvoke(arguments).ToString();
        }
    }
}
