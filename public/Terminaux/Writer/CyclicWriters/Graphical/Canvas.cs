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

using System.Linq;
using System.Text;
using Colorimetry;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Canvas renderable
    /// </summary>
    public class Canvas : GraphicalCyclicWriter
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
        /// Whether this canvas is transparent
        /// </summary>
        public bool Transparent { get; set; } = false;

        /// <summary>
        /// Renders a canvas
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Fill the canvas with spaces inside it
            int widthFactor = DoubleWidth ? 2 : 1;
            int heightFactor = HighDensity ? 2 : 1;
            int heightDivisionFactor = HighDensity ? 1 : 2;
            StringBuilder canvas = new();
            if (!Transparent)
            {
                var canvasBackground = new Box()
                {
                    Left = Left,
                    Top = Top,
                    Width = Width * widthFactor,
                    Height = (int)(Height * (heightDivisionFactor / 2d)),
                    Color = Color,
                    UseColors = true,
                };
                canvas.Append(canvasBackground.Render());
            }
            ConsoleLogger.Debug("Canvas width: {0} * {1} ({2})", Width, widthFactor, Width * widthFactor);
            ConsoleLogger.Debug("Canvas height: {0} * {1} ({2})", Height, heightFactor, Height * heightDivisionFactor / 2d);
            ConsoleLogger.Debug("Canvas position: {0}, {1}", Left, Top);
            int actualY = 0;
            for (int y = 0; y < Height; y += heightFactor, actualY++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // Get effective pixels
                    var effectivePixel = Pixels.LastOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y);
                    var effectiveNextRowPixel = Pixels.LastOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y + 1);

                    // Choose how to draw
                    if (HighDensity)
                    {
                        if (effectivePixel is null && effectiveNextRowPixel is null)
                            continue;
                        var highColor = effectivePixel?.CellColor ?? Color;
                        var lowColor = effectiveNextRowPixel?.CellColor ?? Color;
                        canvas.Append(
                            TextWriterWhereColor.RenderWhereColorBack(new('▄', widthFactor), Left + (x * widthFactor), Top + actualY, lowColor, highColor)
                        );
                    }
                    else
                    {
                        if (effectivePixel is null)
                            continue;
                        var color = effectivePixel?.CellColor ?? Color;
                        canvas.Append(
                            TextWriterWhereColor.RenderWhereColorBack(new(' ', widthFactor), Left + (x * widthFactor), Top + actualY, ConsoleColoring.CurrentForegroundColor, color)
                        );
                    }
                }
            }
            canvas.Append(ConsoleColoring.RenderResetColors());
            return canvas.ToString();
        }

        /// <summary>
        /// Makes a new instance of the canvas renderer
        /// </summary>
        public Canvas()
        { }
    }
}
