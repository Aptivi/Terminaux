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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Terminaux.Sequences.Tools
{
    /// <summary>
    /// Provides all the tools for manipulating with the VT sequences
    /// </summary>
    public static class VtSequenceTools
    {
        /// <summary>
        /// Filters all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="replace">Replace the sequences with this text</param>
        /// <param name="type">VT sequence type</param>
        /// <param name="options">Regular expression options</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text, string replace = "", VtSequenceType type = VtSequenceType.All, RegexOptions options = RegexOptions.None)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            string sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        Text = Regex.Replace(Text, GetSequenceFilterRegexFromType(typeValueEnum), replace, options);
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                Text = Regex.Replace(Text, sequenceFilterRegex, replace, options);
            return Text;
        }

        /// <summary>
        /// Matches all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <param name="options">Regular expression options</param>
        /// <returns>The array of <see cref="MatchCollection"/>s that contain the capture and group information for the found VT sequences</returns>
        public static MatchCollection[] MatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All, RegexOptions options = RegexOptions.None)
        {
            // Match all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            string sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            List<MatchCollection> matchCollections = new();
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        matchCollections.Add(Regex.Matches(Text, GetSequenceFilterRegexFromType(typeValueEnum), options));
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                matchCollections.Add(Regex.Matches(Text, sequenceFilterRegex, options));
            return matchCollections.ToArray();
        }

        /// <summary>
        /// Does the string contain all of the VT sequences or a VT sequence of any type?
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <param name="options">Regular expression options</param>
        /// <returns>True if any of the provided VT types are found; otherwise, false.</returns>
        public static bool IsMatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All, RegexOptions options = RegexOptions.None)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            string sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            List<bool> results = new();
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        results.Add(Regex.IsMatch(Text, GetSequenceFilterRegexFromType(typeValueEnum), options));
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                results.Add(Regex.IsMatch(Text, sequenceFilterRegex, options));
            return results.Contains(true);
        }

        /// <summary>
        /// Does the string contain all of the VT sequences or a VT sequence of any specific type?
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <param name="options">Regular expression options</param>
        /// <returns>A dictionary of each VT sequence type with either true/false for any type test.</returns>
        public static Dictionary<VtSequenceType, bool> IsMatchVTSequencesSpecific(string Text, VtSequenceType type = VtSequenceType.All, RegexOptions options = RegexOptions.None)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            Dictionary<VtSequenceType, bool> results = new();
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                // Check to see if there is a flag denoting a type
                VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                if (type.HasFlag(typeValueEnum))
                    // Go ahead and add the result to the dictionary with the tested type.
                    results.Add(typeValueEnum, Regex.IsMatch(Text, GetSequenceFilterRegexFromType(typeValueEnum), options));
            }
            return results;
        }

        /// <summary>
        /// Splits all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <param name="options">Regular expression options</param>
        /// <returns>The group of texts that don't contain the VT sequences</returns>
        public static string[] SplitVTSequences(string Text, VtSequenceType type = VtSequenceType.All, RegexOptions options = RegexOptions.None) =>
            // Here, we don't support multiple types.
            Regex.Split(Text, GetSequenceFilterRegexFromType(type), options);

        /// <summary>
        /// Determines the VT sequence type from the given text
        /// </summary>
        /// <param name="Text">Text that contains escape sequences</param>
        /// <returns>The type that contains all the VT escape sequence types found in the <paramref name="Text"/></returns>
        public static VtSequenceType DetermineTypeFromText(string Text)
        {
            if (string.IsNullOrEmpty(Text))
                return VtSequenceType.None;

            // Use IsMatchVTSequencesSpecific so that we can determine the successful matches against all the VT sequences.
            var matches = IsMatchVTSequencesSpecific(Text);
            var successMatches = matches.Keys.Where((seqType) => matches[seqType] == true).ToArray();
            int finalTypesInt = 0;
            VtSequenceType finalTypes = VtSequenceType.None;

            // Add the values of the types to determine the correct flag combination
            foreach (VtSequenceType successMatch in successMatches)
                finalTypesInt += (int)successMatch;
            if (!Enum.TryParse(finalTypesInt.ToString(), out finalTypes))
                return VtSequenceType.None;
            return finalTypes;
        }

        /// <summary>
        /// Gets the sequence filter regular expression from the provided VT sequence <paramref name="type"/> (<see cref="VtSequenceType"/>)
        /// </summary>
        /// <param name="type">VT sequence type</param>
        /// <returns>regular expression from the provided VT sequence <paramref name="type"/></returns>
        public static string GetSequenceFilterRegexFromType(VtSequenceType type = VtSequenceType.All)
        {
            // Check the enum to get the needed regular expression for the specific type
            string sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            switch (type)
            {
                case VtSequenceType.Csi:
                    sequenceFilterRegex = VtSequenceRegexes.CSISequences;
                    break;
                case VtSequenceType.Osc:
                    sequenceFilterRegex = VtSequenceRegexes.OSCSequences;
                    break;
                case VtSequenceType.Esc:
                    sequenceFilterRegex = VtSequenceRegexes.ESCSequences;
                    break;
                case VtSequenceType.Apc:
                    sequenceFilterRegex = VtSequenceRegexes.APCSequences;
                    break;
                case VtSequenceType.Dcs:
                    sequenceFilterRegex = VtSequenceRegexes.DCSSequences;
                    break;
                case VtSequenceType.Pm:
                    sequenceFilterRegex = VtSequenceRegexes.PMSequences;
                    break;
                case VtSequenceType.C1:
                    sequenceFilterRegex = VtSequenceRegexes.C1Sequences;
                    break;
                case VtSequenceType.None:
                    sequenceFilterRegex = "";
                    break;
            }
            return sequenceFilterRegex;
        }
    }
}
