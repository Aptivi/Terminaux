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
using System;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using System.Collections.Generic;
using Terminaux.Base;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Graphical.Shapes
{
    /// <summary>
    /// A trapezoid
    /// </summary>
    public class Trapezoid : GraphicalCyclicWriter
    {
        /// <summary>
        /// Trapezoid top width
        /// </summary>
        public int TopWidth { get; }

        /// <summary>
        /// Trapezoid bottom width
        /// </summary>
        public int BottomWidth { get; }

        /// <summary>
        /// Whether to print this filled trapezoid or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a trapezoid
        /// </summary>
        /// <returns>A rendered trapezoid using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            int width = Math.Max(TopWidth, BottomWidth);
            ConsoleLogger.Debug("{0} is the maximum width ({1}, {2})", width, TopWidth, BottomWidth);
            var canvas = new Canvas()
            {
                Transparent = true,
                Height = Height,
                Width = width,
                Left = Left,
                Top = Top,
            };
            var pixels = new List<CellOptions>();
            pixels.AddRange(Line.GetCellParamsNoAntiAlias(new(width / 2 - TopWidth / 2, 0), new(width / 2 - BottomWidth / 2, Height), ShapeColor));
            pixels.AddRange(Line.GetCellParamsNoAntiAlias(new(width / 2 + TopWidth / 2, 0), new(width / 2 + BottomWidth / 2, Height), ShapeColor));
            for (int i = 0; i < TopWidth; i++)
                pixels.Add(new(width / 2 - TopWidth / 2 + 1 + i, 1) { CellColor = ShapeColor });
            for (int i = 0; i < BottomWidth; i++)
                pixels.Add(new(width / 2 - BottomWidth / 2 + 1 + i, Height + 1) { CellColor = ShapeColor });
            if (Filled)
            {
                for (int y = 0; y < Height; y++)
                {
                    int widthDiff = BottomWidth - TopWidth;
                    int widthDiffByHeight = (int)(widthDiff * ((double)y / Height));
                    int lineWidth = TopWidth + widthDiffByHeight;
                    int pos = width / 2 - lineWidth / 2;
                    for (int i = 0; i < lineWidth; i++)
                        pixels.Add(new(pos + i + 1, y + 1) { CellColor = ShapeColor });
                }
            }
            canvas.Pixels = [.. pixels];
            ConsoleLogger.Debug("{0} pixels of trapezoid", pixels.Count);
            return canvas.Render();
        }

        /// <summary>
        /// Makes a new trapezoid
        /// </summary>
        /// <param name="topWidth">Trapezoid top width</param>
        /// <param name="bottomWidth">Trapezoid bottom width</param>
        /// <param name="height">Trapezoid height</param>
        /// <param name="left">Zero-based left position of the terminal to write this trapezoid to</param>
        /// <param name="top">Zero-based top position of the terminal to write this trapezoid to</param>
        /// <param name="filled">Whether to print this filled trapezoid or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Trapezoid(int topWidth, int bottomWidth, int height, int left, int top, bool filled = false, Color? shapeColor = null)
        {
            TopWidth = topWidth;
            BottomWidth = bottomWidth;
            Height = height;
            Width = TopWidth;
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        }
    }
}
