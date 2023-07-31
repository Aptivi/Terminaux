
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Terminaux.Sequences.Tools;

namespace Terminaux.Sequences.Builder
{
    /// <summary>
    /// VT sequence builder tools
    /// </summary>
    public static class VtSequenceBuilderTools
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
            string result;

            // Check the type
            if (!Enum.IsDefined(typeof(VtSequenceSpecificTypes), specificType))
                throw new Exception($"Cannot build VT sequence for nonexistent type {Convert.ToInt32(specificType)}");

            // Now, get the method and the sequence regex using reflection
            var regexGettingType = new Regex("(?<=[a-z0-9])[A-Z].*");
            string typeName = $"{regexGettingType.Replace(specificType.ToString(), "")}Sequences";
            string generatorName = $"Generate{specificType}";
            var sequencesType = Type.GetType($"Terminaux.Sequences.Builder.Types.{typeName}");
            var sequenceRegexGenerator = sequencesType.GetMethod(generatorName);

            // Now, get the sequence
            result = sequenceRegexGenerator.Invoke(null, arguments).ToString();
            return result;
        }

        /// <summary>
        /// Determines the VT sequence type from the given sequence
        /// </summary>
        /// <param name="sequence">The sequence to query</param>
        /// <returns>A tuple of (<see cref="VtSequenceType"/>, <see cref="VtSequenceSpecificTypes"/>) containing information about a sequence type and a sequence command type</returns>
        /// <exception cref="Exception"></exception>
        public static (VtSequenceType, VtSequenceSpecificTypes) DetermineTypeFromSequence(string sequence)
        {
            // First, get all the VT sequence types except "None" and "All"
            var seqTypeEnumNames = Type.GetType($"Terminaux.Sequences.Tools.{nameof(VtSequenceType)}").GetEnumNames().Where((enumeration) => enumeration != "None" && enumeration != "All").ToArray();

            // Then, iterate through all the sequence types until we find an appropriate one that matches the sequence
            foreach (string seqType in seqTypeEnumNames)
            {
                // Get the class that contains regexes of all the sequences
                string typeName = $"{seqType}Sequences";
                var sequencesType = Type.GetType($"Terminaux.Sequences.Builder.Types.{typeName}");
                var sequenceRegexes = sequencesType.GetProperties();
                foreach (var property in sequenceRegexes)
                {
                    // Now, get the appropriate regex to check to see if there is a match.
                    string regexValue = property.GetValue(null).ToString();
                    var regex = new Regex(regexValue);
                    if (regex.IsMatch(sequence))
                    {
                        // It's a match! Get the type and the specific type of the sequence and return them
                        string enumName = $"{property.Name.Replace("SequenceRegex", "")}";
                        VtSequenceType sequenceType = (VtSequenceType)Enum.Parse(typeof(VtSequenceType), seqType);
                        VtSequenceSpecificTypes sequenceSpecificType = (VtSequenceSpecificTypes)Enum.Parse(typeof(VtSequenceSpecificTypes), enumName);
                        return (sequenceType, sequenceSpecificType);
                    }
                }
            }

            // If still not found, then throw
            throw new Exception("Can't determine type from this sequence. Make sure that you've specified it correctly.");
        }
    }
}
