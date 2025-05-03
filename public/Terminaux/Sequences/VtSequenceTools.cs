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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Terminaux.Sequences
{
    /// <summary>
    /// Provides all the tools for manipulating with the VT sequences
    /// </summary>
    public static partial class VtSequenceTools
    {
        private static readonly VtSequenceType[] typeValues = (VtSequenceType[])Enum.GetValues(typeof(VtSequenceType));

        /// <summary>
        /// Filters all of the VT sequences with support for multiple types
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="replace">Replace the sequences with this text</param>
        /// <param name="types">VT sequence types</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text, string replace = "", VtSequenceType types = VtSequenceType.All)
        {
            static string FilterIndividual(string Text, string replace = "", VtSequenceType type = VtSequenceType.All)
            {
                // Filter all sequences according to the list of flags
                var sequenceFilterRegex = GetSequenceFilterRegexFromType(type);
                Text = sequenceFilterRegex.Replace(Text, replace);
                return Text;
            }

            // Filter all sequences according to the list of flags
            if (types != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = typeValues[i];
                    if (types.HasFlag(typeValueEnum))
                        Text = FilterIndividual(Text, replace, typeValueEnum);
                }
            }
            else
                // We don't want to go through all the types just for "all sequences", because we need this for performance.
                // We don't want to show that VtSequenceRegexes.AllVTSequences is unimportant and unnecessary.
                Text = FilterIndividual(Text, replace);
            return Text;
        }

        /// <summary>
        /// Matches all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>The array of <see cref="Match"/>es that contain the capture and group information for the found VT sequences</returns>
        public static (VtSequenceType type, Match[] matches)[] MatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            static Match[] MatchIndividual(string Text, VtSequenceType type = VtSequenceType.All)
            {
                // Match all sequences according to the type
                var sequenceFilterRegex = GetSequenceFilterRegexFromType(type);
                return sequenceFilterRegex.Matches(Text).OfType<Match>().ToArray();
            }

            // Match all sequences according to the list of flags
            List<(VtSequenceType, Match[])> matchCollections = [];
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                // Check to see if there is a flag denoting a type
                VtSequenceType typeValueEnum = typeValues[i];
                if (type.HasFlag(typeValueEnum))
                    matchCollections.Add((typeValueEnum, MatchIndividual(Text, typeValueEnum)));
            }
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
            static bool IsMatchIndividual(string Text, VtSequenceType type = VtSequenceType.All)
            {
                // Match all VT sequences according to the type
                var sequenceFilterRegex = GetSequenceFilterRegexFromType(type);
                return sequenceFilterRegex.IsMatch(Text);
            }

            // Match all VT sequences according to the type
            List<bool> results = [];
            if (type != VtSequenceType.All)
            {
                for (int i = 1; i < typeValues.Length - 1; i++)
                {
                    // Check to see if there is a flag denoting a type
                    VtSequenceType typeValueEnum = typeValues[i];
                    if (type.HasFlag(typeValueEnum))
                        results.Add(IsMatchIndividual(Text, typeValueEnum));
                }
            }
            else
                results.Add(IsMatchIndividual(Text));
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
            Dictionary<VtSequenceType, bool> results = [];
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                // Check to see if there is a flag denoting a type
                VtSequenceType typeValueEnum = typeValues[i];
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
        /// <param name="types">VT sequence types</param>
        /// <returns>The group of texts that don't contain the VT sequences</returns>
        public static string[] SplitVTSequences(string Text, VtSequenceType types = VtSequenceType.All)
        {
            var regex = GetSequenceFilterRegexFromTypes(types);
            return regex.Split(Text);
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
        /// Gets the sequence filter regular expression from all the VT sequences (<see cref="VtSequenceType"/>)
        /// </summary>
        /// <returns>Regular expression from all the VT sequences</returns>
        public static Regex GetSequenceFilterRegexFromType() =>
            GetSequenceFilterRegexFromType(VtSequenceType.All);

        /// <summary>
        /// Gets the sequence filter regular expression from the provided VT sequence <paramref name="type"/> (<see cref="VtSequenceType"/>)
        /// </summary>
        /// <param name="type">VT sequence type</param>
        /// <returns>Regular expression from the provided VT sequence <paramref name="type"/></returns>
        public static partial Regex GetSequenceFilterRegexFromType(VtSequenceType type);

        /// <summary>
        /// Gets the sequence filter regular expression from the provided VT sequence <paramref name="types"/> (<see cref="VtSequenceType"/>)
        /// </summary>
        /// <param name="types">VT sequence types</param>
        /// <returns>Regular expression from the provided VT sequence <paramref name="types"/></returns>
        public static Regex GetSequenceFilterRegexFromTypes(VtSequenceType types = VtSequenceType.All)
        {
            if (types == VtSequenceType.All)
                return VtSequenceRegexes.AllVTSequences;

            // Check the enum to get the needed regular expression for the specific types
            var pattern = new StringBuilder();
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                // Check to see if there is a flag denoting a type
                VtSequenceType typeValueEnum = typeValues[i];
                if (types.HasFlag(typeValueEnum))
                {
                    // Go ahead and add the result to the dictionary with the tested type.
                    var sequenceFilterRegex = GetSequenceFilterRegexFromType(typeValueEnum);
                    pattern.Append(sequenceFilterRegex.ToString());
                    if (i < typeValues.Length - 1)
                        pattern.Append("|");
                }
            }
            if (pattern.Length > 0)
                pattern.Remove(pattern.Length - 1, 1);
            return new(pattern.ToString());
        }
    }
}
