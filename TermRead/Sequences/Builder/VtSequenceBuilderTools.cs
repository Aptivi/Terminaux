/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Linq;
using System.Text.RegularExpressions;
using TermRead.Sequences.Tools;

namespace TermRead.Sequences.Builder
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
            var sequencesType = Type.GetType($"TermRead.Builder.Types.{typeName}");
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
            var seqTypeEnumNames = Type.GetType($"TermRead.Tools.{nameof(VtSequenceType)}").GetEnumNames().Where((enumeration) => enumeration != "None" && enumeration != "All").ToArray();

            // Then, iterate through all the sequence types until we find an appropriate one that matches the sequence
            foreach (string seqType in seqTypeEnumNames)
            {
                // Get the class that contains regexes of all the sequences
                string typeName = $"{seqType}Sequences";
                var sequencesType = Type.GetType($"TermRead.Builder.Types.{typeName}");
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
