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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Ruler renderable
    /// </summary>
    public class Ruler : SimpleCyclicWriter
    {
        private int width = ConsoleWrapper.WindowWidth;
        private int height = ConsoleWrapper.WindowHeight;
        private BorderSettings settings = new(BorderSettings.GlobalSettings);
        private TextAlignment alignment = TextAlignment.Left;
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Whether this ruler is a vertical ruler or not
        /// </summary>
        public bool Vertical { get; set; }

        /// <summary>
        /// Whether this ruler has intersection indicators
        /// </summary>
        public bool IntersectionIndicator { get; set; }

        /// <summary>
        /// Text to render (only for horizontal rulers)
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Alignment of text
        /// </summary>
        public TextAlignment Alignment
        {
            get => alignment;
            set => alignment = value;
        }

        /// <summary>
        /// Width of the ruler
        /// </summary>
        public int Width
        {
            get => width;
            set => width = value;
        }

        /// <summary>
        /// Height of the ruler
        /// </summary>
        public int Height
        {
            get => height;
            set => height = value;
        }

        /// <summary>
        /// Ruler style settings
        /// </summary>
        public BorderSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors { get; set; } = true;

        /// <summary>
        /// Renders a ruler
        /// </summary>
        /// <returns>Rendered ruler text that will be used by the renderer</returns>
        public override string Render()
        {
            // Set the colors
            var rendered = new StringBuilder();
            if (UseColors)
            {
                rendered.Append(
                    ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                    ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                );
            }

            // Determine whether we're rendering the horizontal or the vertical ruler
            if (Vertical)
            {
                // Vertical ruler is required
                for (int i = 0; i < Height; i++)
                {
                    string finalCharString =
                        i == 0 && IntersectionIndicator ? $"{Settings.BorderTopVerticalIntersectionChar}" :
                        i == Height - 1 && IntersectionIndicator ? $"{Settings.BorderBottomVerticalIntersectionChar}" :
                        $"{Settings.BorderVerticalIntersectionChar}";
                    rendered.AppendLine(finalCharString);
                    i += ConsoleChar.EstimateCellWidth(finalCharString) - 1;
                }
            }
            else
            {
                // Horizontal ruler is required
                string finalText = Text.Truncate(Width - 8);
                int startX = TextWriterTools.DetermineTextAlignment(finalText, Width - 8, Alignment, 4);
                for (int i = 0; i < Width; i++)
                {
                    // Determine whether we're starting the ruler, ending it, or extending it
                    string finalCharString =
                        i == 0 && IntersectionIndicator ? $"{Settings.BorderLeftHorizontalIntersectionChar}" :
                        i == Width - 1 && IntersectionIndicator ? $"{Settings.BorderRightHorizontalIntersectionChar}" :
                        $"{Settings.BorderHorizontalIntersectionChar}";
                    
                    // Now, determine whether we need to write text or not
                    if (!string.IsNullOrEmpty(finalText))
                    {
                        // We have text, so we need to print the whole string if we've reached the boundaries
                        if (IntersectionIndicator && i == startX - 2)
                            finalCharString = $"{Settings.BorderRightHorizontalIntersectionChar} ";
                        else if (i == startX)
                            finalCharString = finalText;
                        else if (IntersectionIndicator && i == startX + ConsoleChar.EstimateCellWidth(finalText))
                            finalCharString = $" {Settings.BorderLeftHorizontalIntersectionChar}";
                    }
                    rendered.Append(finalCharString);
                    i += ConsoleChar.EstimateCellWidth(finalCharString) - 1;
                }
            }

            // Finalize the rendered ruler
            if (UseColors)
                rendered.Append(ConsoleColoring.RenderResetColors());
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of the ruler renderer
        /// </summary>
        public Ruler()
        { }
    }
}
