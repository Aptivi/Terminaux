
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
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text, string replace = "", VtSequenceType type = VtSequenceType.All)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            var sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        Text = sequenceFilterRegex.Replace(Text, replace);
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                Text = sequenceFilterRegex.Replace(Text, replace);
            return Text;
        }

        /// <summary>
        /// Matches all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>The array of <see cref="MatchCollection"/>s that contain the capture and group information for the found VT sequences</returns>
        public static MatchCollection[] MatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            // Match all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            var sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            List<MatchCollection> matchCollections = [];
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        matchCollections.Add(sequenceFilterRegex.Matches(Text));
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                matchCollections.Add(sequenceFilterRegex.Matches(Text));
            return [.. matchCollections];
        }

        /// <summary>
        /// Does the string contain all of the VT sequences or a VT sequence of any type?
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>True if any of the provided VT types are found; otherwise, false.</returns>
        public static bool IsMatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            var sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
            List<bool> results = [];
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                    if (type.HasFlag(typeValueEnum))
                        results.Add(sequenceFilterRegex.IsMatch(Text));
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                results.Add(sequenceFilterRegex.IsMatch(Text));
            return results.Contains(true);
        }

        /// <summary>
        /// Does the string contain all of the VT sequences or a VT sequence of any specific type?
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>A dictionary of each VT sequence type with either true/false for any type test.</returns>
        public static Dictionary<VtSequenceType, bool> IsMatchVTSequencesSpecific(string Text, VtSequenceType type = VtSequenceType.All)
        {
            // Filter all sequences according to the list of flags
            var typeValues = Enum.GetValues(typeof(VtSequenceType));
            Dictionary<VtSequenceType, bool> results = [];
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                // Check to see if there is a flag denoting a type
                VtSequenceType typeValueEnum = (VtSequenceType)typeValues.GetValue(i);
                if (type.HasFlag(typeValueEnum))
                {
                    // Go ahead and add the result to the dictionary with the tested type.
                    var sequenceFilterRegex = GetSequenceFilterRegexFromType(typeValueEnum);
                    results.Add(typeValueEnum, sequenceFilterRegex.IsMatch(Text));
                }
            }
            return results;
        }

        /// <summary>
        /// Splits all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>The group of texts that don't contain the VT sequences</returns>
        public static string[] SplitVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            // Here, we don't support multiple types.
            var sequenceFilterRegex = GetSequenceFilterRegexFromType(type);
            return sequenceFilterRegex.Split(Text);
        }

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
        public static Regex GetSequenceFilterRegexFromType(VtSequenceType type = VtSequenceType.All)
        {
            // Check the enum to get the needed regular expression for the specific type
            var sequenceFilterRegex = VtSequenceRegexes.AllVTSequences;
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
                    sequenceFilterRegex = new Regex(@"\b\B");
                    break;
            }
            return sequenceFilterRegex;
        }

        /// <summary>
        /// A simplification for <see cref="Convert.ToChar(int)"/> function to return the ESC character
        /// </summary>
        /// <returns>ESC</returns>
        internal static char GetEsc() =>
            Convert.ToChar(0x1B);
    }
}
