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
    /// A rectangle
    /// </summary>
    public class Rectangle : GraphicalCyclicWriter
    {
        /// <summary>
        /// Whether to print this filled rectangle or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a rectangle
        /// </summary>
        /// <returns>A rendered rectangle using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            var canvas = new Canvas()
            {
                Transparent = true,
                Height = Height,
                Width = Width,
                Left = Left,
                Top = Top,
            };
            var pixels = new List<CellOptions>();
            for (int y = 0; y < Height; y++)
            {
                bool isOutline = y == 0 || y == Height - 1;
                if (isOutline || Filled)
                {
                    for (int i = 0; i < Width; i++)
                        pixels.Add(new(i + 1, y + 1) { CellColor = ShapeColor });
                }
                else
                {
                    pixels.Add(new(1, y + 1) { CellColor = ShapeColor });
                    pixels.Add(new(Width - 1 + 1, y + 1) { CellColor = ShapeColor });
                }
            }
            canvas.Pixels = [.. pixels];
            ConsoleLogger.Debug("{0} pixels of rectangle", pixels.Count);
            return canvas.Render();
        }

        /// <summary>
        /// Makes a new rectangle
        /// </summary>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="left">Zero-based left position of the terminal to write this rectangle to</param>
        /// <param name="top">Zero-based top position of the terminal to write this rectangle to</param>
        /// <param name="filled">Whether to print this filled rectangle or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Rectangle(int width, int height, int left, int top, bool filled = false, Color? shapeColor = null)
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
