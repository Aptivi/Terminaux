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
using System.Collections.ObjectModel;
using System.Text;

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
        public static ReadOnlyDictionary<VtSequenceType, VtSequenceInfo[]> MatchVTSequences(string Text, VtSequenceType type = VtSequenceType.All)
        {
            Dictionary<VtSequenceType, VtSequenceInfo[]> matchCollections = [];

            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(type);
            if (tokens.Length <= 0)
                return new(new Dictionary<VtSequenceType, VtSequenceInfo[]>());

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
                matchCollections.Add(seqType, [.. infos]);
            }
            return new(matchCollections);
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
        public static ReadOnlyDictionary<VtSequenceType, bool> IsMatchVTSequencesSpecific(string Text, VtSequenceType type = VtSequenceType.All)
        {
            Dictionary<VtSequenceType, bool> results = [];

            // Parse the VT sequences
            var tokenizer = new VtSequenceTokenizer(Text.ToCharArray());
            var tokens = tokenizer.Parse(type);
            if (tokens.Length <= 0)
                return new(new Dictionary<VtSequenceType, bool>());

            // Filter all sequences according to the list of types
            for (int i = 1; i < typeValues.Length - 1; i++)
            {
                VtSequenceType seqType = typeValues[i];
                if (tokens.Length == 0)
                    results.Add(seqType, false);
                foreach (var token in tokens)
                    results.Add(seqType, token.Type == seqType);
            }
            return new(results);
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
    }
}
