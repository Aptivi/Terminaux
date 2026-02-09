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

using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;
using Textify.Data.Figlet;
using Textify.Data.Figlet.Utilities.Lines;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Figlet text renderable
    /// </summary>
    public class FigletText : SimpleCyclicWriter
    {
        private FigletFont figletFont = FigletFonts.GetByName("small");
        private string text = "";
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private bool useColors = true;

        /// <summary>
        /// Top position
        /// </summary>
        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        /// Width of the aligned figlet text
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Foreground color of the text
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color of the text
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
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
        /// Figlet font to render the text with
        /// </summary>
        public FigletFont Font
        {
            get => figletFont;
            set => figletFont = value;
        }

        /// <summary>
        /// Renders an aligned figlet text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            var builder = new StringBuilder();
            builder.Append(
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ForegroundColor) : "")}" +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                FigletTools.RenderFiglet(Text, Font, Width) +
                $"{(UseColors ? ConsoleColoring.RenderRevertForeground() : "")}" +
                $"{(UseColors ? ConsoleColoring.RenderRevertBackground() : "")}"
            );
            return builder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the aligned figlet text renderer
        /// </summary>
        /// <param name="figletFont">Figlet font to render with</param>
        /// <param name="text">Text to use</param>
        /// <param name="vars">Variables to format the text with</param>
        public FigletText(FigletFont figletFont, Mark? text = null, params object[] vars)
        {
            // Install the values
            this.text = TextTools.FormatString(text ?? "", vars);
            this.figletFont = figletFont;
        }
    }
}
