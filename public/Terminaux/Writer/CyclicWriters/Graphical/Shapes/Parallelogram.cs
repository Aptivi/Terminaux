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

using Terminaux.Writer.ConsoleWriters;
using Colorimetry;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Base;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Graphical.Shapes
{
    /// <summary>
    /// A parallelogram
    /// </summary>
    public class Parallelogram : GraphicalCyclicWriter
    {
        /// <summary>
        /// Whether to print this filled parallelogram or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a parallelogram
        /// </summary>
        /// <returns>A rendered parallelogram using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            var canvas = new Canvas()
            {
                Transparent = true,
                Height = Height,
                Width = Width * 2,
                Left = Left,
                Top = Top,
            };
            var pixels = new List<CellOptions>();
            for (int y = 0; y < Height; y++)
            {
                int widthThreshold = Width * (y + 1) / Height;
                int LeftPosShift = (Width - widthThreshold) / 2;
                int nextWidthThreshold = Width * (y + 2) / Height;
                int thresholdDiff = nextWidthThreshold - widthThreshold;
                bool isOutline = y == 0 || y == Height - 1;
                if (isOutline || Filled)
                {
                    for (int i = 0; i < Width; i++)
                        pixels.Add(new(LeftPosShift + i + 1, y + 1) { CellColor = ShapeColor });
                }
                else
                {
                    for (int i = 0; i < thresholdDiff; i++)
                    {
                        pixels.Add(new(LeftPosShift + thresholdDiff - i, y + 1) { CellColor = ShapeColor });
                        pixels.Add(new(LeftPosShift + Width - thresholdDiff + i + 1, y + 1) { CellColor = ShapeColor });
                    }
                }
            }
            canvas.Pixels = [.. pixels];
            ConsoleLogger.Debug("{0} pixels of parallelogram", pixels.Count);
            return canvas.Render();
        }

        /// <summary>
        /// Makes a new parallelogram
        /// </summary>
        /// <param name="width">Parallelogram width</param>
        /// <param name="height">Parallelogram height</param>
        /// <param name="left">Zero-based left position of the terminal to write this parallelogram to</param>
        /// <param name="top">Zero-based top position of the terminal to write this parallelogram to</param>
        /// <param name="filled">Whether to print this filled parallelogram or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Parallelogram(int width, int height, int left, int top, bool filled = false, Color? shapeColor = null)
        {
            Width = width;
            Height = height;
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        }
    }
}
