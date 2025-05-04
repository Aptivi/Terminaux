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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using System;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Writer.CyclicWriters.Graphical.Shapes
{
    /// <summary>
    /// An arc
    /// </summary>
    public class Arc : GraphicalCyclicWriter
    {
        /// <summary>
        /// Specifies the inner radius
        /// </summary>
        public int InnerRadius { get; set; }

        /// <summary>
        /// Specifies the outer radius
        /// </summary>
        public int OuterRadius { get; set; }

        /// <summary>
        /// Starting angle
        /// </summary>
        public int AngleStart { get; set; }

        /// <summary>
        /// Ending angle
        /// </summary>
        public int AngleEnd { get; set; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders an arc
        /// </summary>
        /// <returns>A rendered arc using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            double halfPi = Math.PI / 2;
            double quarterPi = Math.PI / 4;

            // Process the angle inputs
            bool inverted = AngleStart > AngleEnd;
            bool full = AngleStart == AngleEnd;
            int angleStart = inverted ? AngleEnd : AngleStart;
            int angleEnd = inverted ? AngleStart : AngleEnd;
            ConsoleLogger.Debug("Arc angles: {0} -> {1}", angleStart, angleEnd);

            // Get the center X and Y positions, since we're dealing with the upper left corner positions, so that we know
            // the radius of the circle
            int centerX = Width / 2;
            int centerY = Height / 2;
            ConsoleLogger.Debug("Center position: {0}, {1}", centerX, centerY);

            // Trace the arc radius
            var canvas = new Canvas()
            {
                Transparent = true,
                Height = Height,
                Width = Width,
                Left = Left,
                Top = Top,
            };
            var pixels = new List<CellOptions>();
            for (int r = InnerRadius; r <= OuterRadius; r++)
            {
                int x = 0;
                int y = r;
                int d = r - 1;
                while (y >= x)
                {
                    // Get the circle percentage
                    int ratio = (int)Math.Floor((halfPi - Math.Atan2(y, x)) * 32 / quarterPi);

                    // Fill the pixels
                    if (full || (ratio >= angleStart && ratio < angleEnd) ^ inverted)
                        pixels.Add(new(centerX + y + 1, centerY - x + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + angleEnd) > 63 && (ratio + angleStart) <= 63) ^ inverted)
                        pixels.Add(new(centerX + x + 1, centerY - y + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + 64) >= angleStart && (ratio + 64) < angleEnd) ^ inverted)
                        pixels.Add(new(centerX - x + 1, centerY - y + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + angleEnd) > 127 && (ratio + angleStart) <= 127) ^ inverted)
                        pixels.Add(new(centerX - y + 1, centerY - x + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + 128) >= angleStart && (ratio + 128) < angleEnd) ^ inverted)
                        pixels.Add(new(centerX - y + 1, centerY + x + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + angleEnd) > 191 && (ratio + angleStart) <= 191) ^ inverted)
                        pixels.Add(new(centerX - x + 1, centerY + y + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + 192) >= angleStart && (ratio + 192) < angleEnd) ^ inverted)
                        pixels.Add(new(centerX + x + 1, centerY + y + 1) { CellColor = ShapeColor });
                    if (full || ((ratio + angleEnd) > 255 && (ratio + angleStart) <= 255) ^ inverted)
                        pixels.Add(new(centerX + y + 1, centerY + x + 1) { CellColor = ShapeColor });

                    // Now, go to the next pixel
                    if (d >= 2 * x)
                    {
                        d -= 2 * x - 1;
                        x++;
                    }
                    else if (d < 2 * (r - y))
                    {
                        d += 2 * y - 1;
                        y--;
                    }
                    else
                    {
                        d += 2 * (y - x - 1);
                        x++;
                        y--;
                    }
                }
            }
            canvas.Pixels = [.. pixels];
            return canvas.Render();
        }

        /// <summary>
        /// Makes a new arc
        /// </summary>
        /// <param name="height">Arc height</param>
        /// <param name="left">Zero-based left position of the terminal to write this arc to</param>
        /// <param name="top">Zero-based top position of the terminal to write this arc to</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Arc(int height, int left, int top, Color? shapeColor = null)
        {
            Height = height;
            Width = height;
            Left = left;
            Top = top;
            ShapeColor = shapeColor ?? ColorTools.CurrentForegroundColor;
        }
    }
}
