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

using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Writer.MiscWriters.Tools;

namespace Terminaux.Writer.FancyWriters.Tools
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
        /// <param name="rightMargin">Right margin (zero-based)</param>
        /// <returns>A zero-based X position of the aligned text</returns>
        public static int DetermineTextAlignment(string text, TextAlignment alignment, int leftMargin = 0, int rightMargin = 0) =>
            DetermineTextAlignment(text, ConsoleWrapper.WindowWidth - 1, alignment, leftMargin, rightMargin);

        /// <summary>
        /// Determines the text alignment X position (zero-based)
        /// </summary>
        /// <param name="text">Text to process. Only the first line is taken, so split the sentences using <see cref="ConsoleMisc.GetWrappedSentencesByWords(string, int)"/> or <see cref="ConsoleMisc.GetWrappedSentences(string, int)"/>.</param>
        /// <param name="width">Target width (zero-based)</param>
        /// <param name="alignment">Text alignment</param>
        /// <param name="leftMargin">Left margin (zero-based)</param>
        /// <param name="rightMargin">Right margin (zero-based)</param>
        /// <returns>A zero-based X position of the aligned text</returns>
        public static int DetermineTextAlignment(string text, int width, TextAlignment alignment, int leftMargin = 0, int rightMargin = 0)
        {
            // First, get the text console width after wrapping it in a width
            string[] wrappedLines = ConsoleMisc.GetWrappedSentencesByWords(text, width);
            string wrappedLine = wrappedLines.Length > 0 ? wrappedLines[0] : "";
            int maxWidth = ConsoleChar.EstimateCellWidth(wrappedLine);

            // Then, process the positions depending on the alignment
            int x = leftMargin;
            switch (alignment)
            {
                case TextAlignment.Right:
                    x = leftMargin + width - maxWidth - rightMargin;
                    break;
                case TextAlignment.Middle:
                    x = leftMargin + (width / 2) - (maxWidth / 2) - rightMargin;
                    break;
            }
            if (x < 0)
                x = 0;
            return x;
        }
    }
}
