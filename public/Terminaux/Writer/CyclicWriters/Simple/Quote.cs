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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Textify.General.Structures;
using Terminaux.Shell.Shells.Unified;
using Textify.General;
using System.Collections.Generic;
using System.Text;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Quote cyclic renderer
    /// </summary>
    public class Quote : SimpleCyclicWriter
    {
        private WideChar padChar = (WideChar)"▒";
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color padColor = ThemeColorsTools.GetColor(ThemeColorType.Separator);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Quote padding character at the beginning of each line
        /// </summary>
        public WideChar PadChar
        {
            get => padChar;
            set => padChar = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        /// <summary>
        /// Quote pad color
        /// </summary>
        public Color PadColor
        {
            get => padColor;
            set => padColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        /// <summary>
        /// Quote to render to the console
        /// </summary>
        public string QuoteText { get; set; } = "";

        /// <summary>
        /// Author of the quote
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        /// Whether to wrap lines or not
        /// </summary>
        public bool WrapLines { get; set; } = true;

        /// <summary>
        /// Line wrap width for each line
        /// </summary>
        public int WrapWidth { get; set; } = 75;

        /// <summary>
        /// Renders a quote
        /// </summary>
        /// <returns>A string representation of the quote</returns>
        public override string Render()
        {
            // Build the required "prosery" text
            var proseryBuilder = new StringBuilder();
            string[] quoteTextLines = QuoteText.SplitNewLines();
            quoteTextLines = ConsoleMisc.TrimNewLines(quoteTextLines);
            proseryBuilder.Append(
                $"{ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Italic)}" +
                $"“{string.Join("\n", quoteTextLines)}”\n\n" +
                $"-- {Author}" +
                $"{ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Default)}"
            );

            // Return the rendered prosery with set parameters
            var prosery = new Prosery()
            {
                BackgroundColor = BackgroundColor,
                ForegroundColor = ForegroundColor,
                PadColor = PadColor,
                PadChar = PadChar,
                UseColors = UseColors,
                WrapLines = WrapLines,
                WrapWidth = WrapWidth,
                Prose = proseryBuilder.ToString()
            };
            return prosery.Render();
        }
    }
}
