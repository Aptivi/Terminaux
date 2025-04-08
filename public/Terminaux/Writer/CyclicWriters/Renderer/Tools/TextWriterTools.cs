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

using System.Collections.Generic;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Text writer tools
    /// </summary>
    public static class TextWriterTools
    {
        /// <summary>
        /// Determines the text alignment X position (zero-based)
        /// </summary>
        /// <param name="text">Text to process. Only the first line is taken, so split the sentences using <see cref="ConsoleMisc.GetWrappedSentencesByWords(string, int)"/> or <see cref="ConsoleMisc.GetWrappedSentences(string, int)"/>.</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">Left margin (zero-based)</param>
        /// <returns>A zero-based X position of the aligned text</returns>
        public static int DetermineTextAlignment(string text, TextAlignment alignment, int leftMargin = 0) =>
            DetermineTextAlignment(text, ConsoleWrapper.WindowWidth - 1, alignment, leftMargin);

        /// <summary>
        /// Determines the text alignment X position (zero-based)
        /// </summary>
        /// <param name="text">Text to process. Only the first line is taken, so split the sentences using <see cref="ConsoleMisc.GetWrappedSentencesByWords(string, int)"/> or <see cref="ConsoleMisc.GetWrappedSentences(string, int)"/>.</param>
        /// <param name="width">Target width (zero-based)</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">Left margin (zero-based)</param>
        /// <returns>A zero-based X position of the aligned text</returns>
        public static int DetermineTextAlignment(string text, int width, TextAlignment alignment, int leftMargin = 0)
        {
            // First, get the text console width after wrapping it in a width
            string[] wrappedLines = ConsoleMisc.GetWrappedSentencesByWords(text, width);
            string wrappedLine = wrappedLines.Length > 0 ? wrappedLines[0] : "";
            int maxWidth = ConsoleChar.EstimateCellWidth(wrappedLine);

            // Then, process the positions depending on the alignment
            int x = leftMargin;
            ConsoleLogger.Debug("Initial x position: {0}, alignment {1}.", x, alignment);
            switch (alignment)
            {
                case TextAlignment.Right:
                    x = leftMargin + width - maxWidth;
                    break;
                case TextAlignment.Middle:
                    x = leftMargin + width / 2 - maxWidth / 2;
                    break;
            }
            ConsoleLogger.Debug("Final x position: {0}", x);
            if (x < 0)
                x = 0;
            return x;
        }

        internal static string[] GetFinalLines(string text, params object[] vars) =>
            GetFinalLines(text, ConsoleWrapper.WindowWidth - 4, vars);

        internal static string[] GetFinalLines(string text, int width, params object[] vars)
        {
            // Deal with the lines to actually fit text in the infobox
            string finalInfoRendered = text.FormatString(vars);
            string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
            List<string> splitFinalLines = [];
            foreach (var line in splitLines)
            {
                var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, width);
                foreach (var lineSentence in lineSentences)
                    splitFinalLines.Add(lineSentence);
            }

            // Trim the new lines until we reach a full line
            for (int i = splitFinalLines.Count - 1; i >= 0; i--)
            {
                string line = splitFinalLines[i];
                if (!string.IsNullOrWhiteSpace(line))
                    break;
                splitFinalLines.RemoveAt(i);
                ConsoleLogger.Debug("Trimmed away {0}", i);
            }
            return [.. splitFinalLines];
        }
    }
}
