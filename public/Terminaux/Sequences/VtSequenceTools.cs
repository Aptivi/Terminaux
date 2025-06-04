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
            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(types);
            if (tokens.Length <= 0)
                return Text;

            // Make a string builder to track indexes
            var builder = new StringBuilder(Text);
            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                // Reverse, because deleting from the left means that our indexes will be invalid.
                VtSequenceInfo? token = tokens[i];
                int start = token.Start;
                int length = token.FullSequence.Length;
                builder.Remove(start, length);

                // Optionally, put a replacement string
                if (!string.IsNullOrEmpty(replace))
                    builder.Insert(start, replace);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Matches all of the VT sequences
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>The array of <see cref="VtSequenceInfo"/> that contain the information for the found VT sequences</returns>
        public static (VtSequenceType type, VtSequenceInfo[] matches)[] MatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            List<(VtSequenceType, VtSequenceInfo[])> matchCollections = [];

            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(type);
            if (tokens.Length <= 0)
                return [];

            // Match all sequences according to the list of types
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                VtSequenceType seqType = typeValues[i];
                List<VtSequenceInfo> infos = [];

                // Using the existing token array, just add them if they match.
                foreach (var token in tokens)
                    if (token.Type == seqType)
                        infos.Add(token);

                // Add the match for this type
                matchCollections.Add((seqType, [.. infos]));
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
            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(type);
            return tokens.Length > 0;
        }

        /// <summary>
        /// Does the string contain all of the VT sequences or a VT sequence of any specific type?
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="type">VT sequence type</param>
        /// <returns>A dictionary of each VT sequence type with either true/false for any type test.</returns>
        public static Dictionary<VtSequenceType, bool> IsMatchVTSequencesSpecific(string Text, VtSequenceType type = VtSequenceType.All)
        {
            Dictionary<VtSequenceType, bool> results = [];

            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(type);
            if (tokens.Length <= 0)
                return [];

            // Filter all sequences according to the list of types
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                VtSequenceType seqType = typeValues[i];
                if (tokens.Length == 0)
                    results.Add(seqType, false);
                foreach (var token in tokens)
                    results.Add(seqType, token.Type == seqType);
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
            List<string> split = [];

            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(types);
            if (tokens.Length <= 0)
                return [Text];

            // Use the tokens to split the strings according to the given indexes
            int startIdx = 0;
            int endIdx;
            string part;
            foreach (var token in tokens)
            {
                endIdx = token.Start;

                // Get this substring
                part = Text.Substring(startIdx, endIdx - startIdx);
                split.Add(part);

                startIdx = token.End;
            }

            // Get the last substring
            part = Text.Substring(startIdx);
            split.Add(part);
            return [.. split];
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

            // Cycle through them to add successful matches
            int finalTypesInt = 0;
            foreach (var match in matches)
            {
                var type = match.Key;
                var success = match.Value;
                if (success)
                    finalTypesInt += (int)type;
            }
            if (!Enum.TryParse(finalTypesInt.ToString(), out VtSequenceType finalTypes))
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
