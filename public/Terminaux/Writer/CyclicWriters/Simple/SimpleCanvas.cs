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
using System.Linq;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Canvas renderable
    /// </summary>
    public class SimpleCanvas : SimpleCyclicWriter
    {
        private Color canvasColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Canvas color
        /// </summary>
        public Color Color
        {
            get => canvasColor;
            set => canvasColor = value;
        }

        /// <summary>
        /// Pixel representations
        /// </summary>
        public CellOptions[] Pixels { get; set; } = [];

        /// <summary>
        /// Whether this canvas is double-width or single-width
        /// </summary>
        public bool DoubleWidth { get; set; } = true;

        /// <summary>
        /// Whether this canvas is high density or low density
        /// </summary>
        public bool HighDensity { get; set; } = false;

        /// <summary>
        /// Maximum width of the canvas (0 to automatically determine based on content)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Maximum height of the canvas (0 to automatically determine based on content)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Renders a canvas
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Fill the canvas with spaces inside it
            int widthFactor = DoubleWidth ? 2 : 1;
            int heightFactor = HighDensity ? 2 : 1;
            StringBuilder canvas = new();
            ConsoleLogger.Debug("Canvas width: {0} * {1} ({2})", Width, widthFactor, Width * widthFactor);
            ConsoleLogger.Debug("Canvas height: {0} * {1} ({2})", Height, heightFactor, Height * heightFactor / 2d);
            ConsoleLogger.Debug("Canvas height is half: {0}, {1}", Height, HighDensity);

            // Check to see if the maximum width and height is zero
            if (Width <= 0)
                Width = Pixels.Max((co) => co.ColumnNumber);
            if (Height <= 0)
                Height = Pixels.Max((co) => co.RowNumber);

            // Now, populate the pixels by scanlines
            for (int y = 0; y < Height; y += heightFactor)
            {
                for (int x = 0; x < Width; x++)
                {
                    // Get effective pixels
                    var effectivePixel = Pixels.LastOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y);
                    var effectiveNextRowPixel = Pixels.LastOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y + 1);

                    // Choose how to draw
                    if (HighDensity)
                    {
                        var highColor = effectivePixel?.CellColor ?? Color;
                        var lowColor = effectiveNextRowPixel?.CellColor ?? Color;
                        canvas.Append(
                            ConsoleColoring.RenderSetConsoleColor(highColor) +
                            ConsoleColoring.RenderSetConsoleColor(lowColor, true) +
                            new string('▀', widthFactor)
                        );
                    }
                    else
                    {
                        var color = effectivePixel?.CellColor ?? Color;
                        canvas.Append(
                            ConsoleColoring.RenderSetConsoleColor(color) +
                            new string('█', widthFactor)
                        );
                    }
                }
                if (y < Height - 1)
                    canvas.AppendLine();
            }
            canvas.Append(
                ConsoleColoring.RenderRevertForeground() +
                ConsoleColoring.RenderRevertBackground()
            );
            return canvas.ToString();
        }

        /// <summary>
        /// Makes a new instance of the simple canvas renderer
        /// </summary>
        public SimpleCanvas()
        { }
    }
}
