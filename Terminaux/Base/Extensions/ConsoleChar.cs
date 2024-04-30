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

using Terminaux.Base.Extensions.Data;
using Terminaux.Sequences;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console character manipulation routines
    /// </summary>
    public static class ConsoleChar
    {
        /// <summary>
        /// Whether to use two cells for unassigned characters or only one cell
        /// </summary>
        public static bool UseTwoCellsForUnassignedChars { get; set; }
        /// <summary>
        /// Whether to use two cells for ambiguous characters or only one cell
        /// </summary>
        public static bool UseTwoCellsForAmbiguousChars { get; set; }
        /// <summary>
        /// Whether to use two cells for private characters or only one cell
        /// </summary>
        public static bool UseTwoCellsForPrivateChars { get; set; }

        /// <summary>
        /// Gets the character width
        /// </summary>
        /// <param name="c">A character number (codepoint) to parse</param>
        /// <returns>Either 0 for non-printing characters, 1 for half-width characters, or 2 for full-width characters</returns>
        /// <exception cref="TerminauxInternalException"></exception>
        public static int GetCharWidth(int c)
        {
            // Check the value
            if (c < 0 || c > 0x10FFFF)
                throw new TerminauxInternalException($"Invalid character number {c}.");

            // Use the character cell table defined in a separate code class to be able to determine the width
            int width = 1;
            foreach ((var range, int cells) in CharWidths.ranges)
            {
                // Check for each range if we have a Unicode character that falls under one of the characters that take
                // up either no cells or more than one cell.
                foreach ((int first, int last) in range)
                {
                    if (c >= first && c <= last)
                    {
                        width = cells;
                        break;
                    }
                }
                if (width == 1)
                    continue;

                // Now, check the value of the width
                switch (width)
                {
                    case -1:
                        // Unassigned character. This way, we need to let users select how to handle it by giving them a property
                        // that allows them to set either one (default) or two cells to be taken.
                        width = UseTwoCellsForUnassignedChars ? 2 : 1;
                        break;
                    case -2:
                        // Ambiguous character. See above.
                        width = UseTwoCellsForAmbiguousChars ? 2 : 1;
                        break;
                    case -3:
                        // Private character. See above.
                        width = UseTwoCellsForPrivateChars ? 2 : 1;
                        break;
                }

                // Return that width
                return width;
            }
            return width;
        }

        /// <summary>
        /// Estimates the cell width (how many cells a string takes up)
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <returns>Length of the string by character widths (a.k.a. how many cells this sentence takes up)</returns>
        public static int EstimateCellWidth(string sentence)
        {
            // We don't need to perform operations on null or empty strings
            if (string.IsNullOrEmpty(sentence))
                return 0;

            // We need to filter VT sequences if we suspect that we can find one of them
            if (VtSequenceTools.IsMatchVTSequences(sentence))
                sentence = VtSequenceTools.FilterVTSequences(sentence);

            // Iterate through every character inside this string to get their widths according to the Unicode
            // standards to ensure that we have the correct cell width count that the string takes up.
            int cells = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                char c = sentence[i];

                // Emojis and other characters use surrogate pairs, so we need to check them.
                if (!char.IsSurrogate(c))
                    cells += GetCharWidth(c);
                else if (i + 1 < sentence.Length && char.IsSurrogatePair(c, sentence[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(c, sentence[i + 1]);
                    cells += GetCharWidth(codePoint);
                    i++;
                }
            }
            return cells;
        }

        /// <summary>
        /// Estimates the amount of zero-width characters
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <returns>The amount of zero-width characters that this sentence contains</returns>
        public static int EstimateZeroWidths(string sentence)
        {
            // We don't need to perform operations on null or empty strings
            if (string.IsNullOrEmpty(sentence))
                return 0;

            // We need to filter VT sequences if we suspect that we can find one of them
            if (VtSequenceTools.IsMatchVTSequences(sentence))
                sentence = VtSequenceTools.FilterVTSequences(sentence);

            // Iterate through every character inside this string to get their widths according to the Unicode
            // standards to ensure that we calculate all the zero widths.
            int zeroWidths = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                char c = sentence[i];

                // Emojis and other characters use surrogate pairs, so we need to check them.
                if (!char.IsSurrogate(c))
                {
                    if (GetCharWidth(c) == 0)
                        zeroWidths++;
                }
                else if (i + 1 < sentence.Length && char.IsSurrogatePair(c, sentence[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(c, sentence[i + 1]);
                    if (GetCharWidth(codePoint) == 0)
                        zeroWidths++;
                    i++;
                }
            }
            return zeroWidths;
        }

        /// <summary>
        /// Estimates the amount of zero-width characters
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <returns>The amount of zero-width characters that this sentence contains</returns>
        public static int EstimateFullWidths(string sentence)
        {
            // We don't need to perform operations on null or empty strings
            if (string.IsNullOrEmpty(sentence))
                return 0;

            // We need to filter VT sequences if we suspect that we can find one of them
            if (VtSequenceTools.IsMatchVTSequences(sentence))
                sentence = VtSequenceTools.FilterVTSequences(sentence);

            // Iterate through every character inside this string to get their widths according to the Unicode
            // standards to ensure that we calculate all the full widths.
            int fullWidths = 0;
            for (int i = 0; i < sentence.Length; i++)
            {
                char c = sentence[i];

                // Emojis and other characters use surrogate pairs, so we need to check them.
                if (!char.IsSurrogate(c))
                {
                    if (GetCharWidth(c) == 2)
                        fullWidths++;
                }
                else if (i + 1 < sentence.Length && char.IsSurrogatePair(c, sentence[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(c, sentence[i + 1]);
                    if (GetCharWidth(codePoint) == 2)
                        fullWidths++;
                    i++;
                }
            }
            return fullWidths;
        }
    }
}
