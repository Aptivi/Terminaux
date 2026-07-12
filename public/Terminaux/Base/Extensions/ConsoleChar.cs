//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using System.Collections.Generic;
using Terminaux.Base.Extensions.Data;
using Terminaux.Sequences;
using Textify.General;
using Textify.General.Structures;
using Textify.Tools;

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console character manipulation routines
    /// </summary>
    public static class ConsoleChar
    {
        private static readonly object consoleCharLock = new();

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
            sentence = VtSequenceTools.FilterVTSequences(sentence);
            if (sentence.Contains("\t"))
                sentence = sentence.Replace("\t", new string(' ', ConsoleMisc.TabWidth));

            // Iterate through every character inside this string to get their widths according to the Unicode
            // standards to ensure that we have the correct cell width count that the string takes up.
            int cells = 0;
            int i = 0;
            while (i < sentence.Length)
            {
                int len = GraphemeCluster.GetLength(sentence, i);
                if (len <= 0)
                    len = 1;
                cells += GraphemeCluster.GetClusterWidth(sentence, i);
                i += len;
            }
            return cells;
        }

        /// <summary>
        /// Estimates the cell width (how many cells a string takes up) of a character
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <param name="index">Index of a character within a sentence</param>
        /// <param name="processed">Whether to process the sentence
        /// <br/><br/><strong>(WARNING: Indexes WILL change if there ARE VT sequences in the sentence! Carefully choose a value!)</strong></param>
        /// <returns>Length of a character by character widths (a.k.a. how many cells this sentence takes up), or -1 if empty</returns>
        public static int EstimateCellWidth(string sentence, int index, bool processed = false)
        {
            lock (consoleCharLock)
            {
                // We don't need to perform operations on null or empty strings
                if (string.IsNullOrEmpty(sentence))
                    return -1;

                // We need to filter VT sequences if we suspect that we can find one of them
                if (processed)
                    sentence = VtSequenceTools.FilterVTSequences(sentence);
                if (sentence.Contains("\t"))
                    sentence = sentence.Replace("\t", new string(' ', ConsoleMisc.TabWidth));

                // Process index
                if (index > sentence.Length - 1)
                    index = sentence.Length - 1;

                // Iterate through every character inside this string to get their widths according to the Unicode
                // standards to ensure that we have the correct cell width count that the string takes up.
                int clusterStart = GraphemeCluster.GetStart(sentence, index);
                if (clusterStart != index)
                    return 0;

                return GraphemeCluster.GetClusterWidth(sentence, index);
            }
        }

        /// <summary>
        /// Estimates the amount of zero-width characters
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <returns>The amount of zero-width characters that this sentence contains</returns>
        public static int EstimateZeroWidths(string sentence)
        {
            lock (consoleCharLock)
            {
                // We don't need to perform operations on null or empty strings
                if (string.IsNullOrEmpty(sentence))
                    return 0;

                // We need to filter VT sequences if we suspect that we can find one of them
                sentence = VtSequenceTools.FilterVTSequences(sentence);
                if (sentence.Contains("\t"))
                    sentence = sentence.Replace("\t", new string(' ', ConsoleMisc.TabWidth));

                // Iterate through every character inside this string to get their widths according to the Unicode
                // standards to ensure that we calculate all the zero widths.
                int zeroWidths = 0;
                int i = 0;
                while (i < sentence.Length)
                {
                    int len = GraphemeCluster.GetLength(sentence, i);
                    if (len <= 0)
                        len = 1;
                    if (GraphemeCluster.GetClusterWidth(sentence, i) == 0)
                        zeroWidths++;
                    i += len;
                }
                return zeroWidths;
            }
        }

        /// <summary>
        /// Estimates the amount of zero-width characters
        /// </summary>
        /// <param name="sentence">A sentence to process</param>
        /// <returns>The amount of zero-width characters that this sentence contains</returns>
        public static int EstimateFullWidths(string sentence)
        {
            lock (consoleCharLock)
            {
                // We don't need to perform operations on null or empty strings
                if (string.IsNullOrEmpty(sentence))
                    return 0;

                // We need to filter VT sequences if we suspect that we can find one of them
                sentence = VtSequenceTools.FilterVTSequences(sentence);
                if (sentence.Contains("\t"))
                    sentence = sentence.Replace("\t", new string(' ', ConsoleMisc.TabWidth));

                // Iterate through every character inside this string to get their widths according to the Unicode
                // standards to ensure that we calculate all the full widths.
                int fullWidths = 0;
                int i = 0;
                while (i < sentence.Length)
                {
                    int len = GraphemeCluster.GetLength(sentence, i);
                    if (len <= 0)
                        len = 1;
                    if (GraphemeCluster.GetClusterWidth(sentence, i) == 2)
                        fullWidths++;
                    i += len;
                }
                return fullWidths;
            }
        }

        /// <summary>
        /// Gets the cell width for this character
        /// </summary>
        /// <returns>Width of the character</returns>
        public static int GetWidth(this WideChar wideChar) =>
            EstimateCellWidth(wideChar.ToString());

        /// <summary>
        /// Gets a list of wide characters from a sentence
        /// </summary>
        /// <param name="sentence">Sentence to form</param>
        /// <returns>Wide character instances</returns>
        public static WideChar[] GetWideChars(string sentence)
        {
            List<WideChar> wideChars = [];
            for (int i = 0; i < sentence.Length; i++)
            {
                char c = sentence[i];
                if (!char.IsSurrogate(c))
                    wideChars.Add((WideChar)c);
                else if (i + 1 < sentence.Length && char.IsSurrogatePair(c, sentence[i + 1]))
                {
                    wideChars.Add(new WideChar(sentence[i + 1], c));
                    i++;
                }
                else
                    wideChars.Add((WideChar)c);
            }
            return [.. wideChars];
        }
    }
}
