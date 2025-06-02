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
        /// Horizontal radius of the arc
        /// </summary>
        public int RadiusX { get; set; }

        /// <summary>
        /// Vertical radius of the arc
        /// </summary>
        public int RadiusY { get; set; }

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
            // Check the arc radius
            if (RadiusX == 0)
                RadiusX = OuterRadius;
            if (RadiusY == 0)
                RadiusY = OuterRadius;

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

            // Helper function to add pixel
            void PlotPoint(int ratio, int x, int y)
            {
                if (full || (ratio >= angleStart && ratio < angleEnd) ^ inverted)
                    pixels.Add(new(centerX + x + 1, centerY - y + 1) { CellColor = ShapeColor });

                int ratio2 = (180 - ratio) % 360;
                if (full || (ratio2 >= angleStart && ratio2 < angleEnd) ^ inverted)
                    pixels.Add(new(centerX - x + 1, centerY - y + 1) { CellColor = ShapeColor });

                int ratio3 = (180 + ratio) % 360;
                if (full || (ratio3 >= angleStart && ratio3 < angleEnd) ^ inverted)
                    pixels.Add(new(centerX - x + 1, centerY + y + 1) { CellColor = ShapeColor });

                int ratio4 = (360 - ratio) % 360;
                if (full || (ratio4 >= angleStart && ratio4 < angleEnd) ^ inverted)
                    pixels.Add(new(centerX + x + 1, centerY + y + 1) { CellColor = ShapeColor });
            }
            for (int layer = 0; layer <= (OuterRadius - InnerRadius); layer++)
            {
                int currentRadiusA = InnerRadius + layer;
                int currentRadiusB = (int)(currentRadiusA * RadiusY / (double)RadiusX);

                int x = 0;
                int y = currentRadiusB;
                int radiusA = currentRadiusA * currentRadiusA;
                int radiusB = currentRadiusB * currentRadiusB;
                int twoRadiusA = 2 * radiusA;
                int twoRadiusB = 2 * radiusB;
                int p1 = radiusB - (radiusA * currentRadiusB) + (radiusA + 2) / 4;
                int dX = 0;
                int dY = twoRadiusA * y;
                while (dX < dY)
                {
                    double angle = Math.Atan2(y, x);
                    if (angle < 0)
                        angle += 2 * Math.PI;
                    int ratio = (int)Math.Floor(angle * 180 / Math.PI);

                    // Fill the pixels
                    PlotPoint(ratio, x, y);

                    // Now, go to the next pixel
                    if (p1 < 0)
                    {
                        x++;
                        dX = twoRadiusB * x;
                        p1 += dX + radiusB;
                    }
                    else
                    {
                        x++;
                        y--;
                        dX = twoRadiusB * x;
                        dY = twoRadiusA * y;
                        p1 += dX - dY + radiusB;
                    }
                }

                int p2 = radiusB * (x + 1) * (x + 1) + radiusA * (y - 1) * (y - 1) - radiusA * radiusB;
                while (y >= 0)
                {
                    double angle = Math.Atan2(y, x);
                    if (angle < 0)
                        angle += 2 * Math.PI;
                    int ratio = (int)Math.Floor(angle * 180 / Math.PI);

                    // Fill the pixels
                    PlotPoint(ratio, x, y);

                    // Now, go to the next pixel
                    if (p2 > 0)
                    {
                        y--;
                        dY = twoRadiusA * y;
                        p2 -= dY + radiusA;
                    }
                    else
                    {
                        x++;
                        y--;
                        dX = twoRadiusB * x;
                        dY = twoRadiusA * y;
                        p2 += dX - dY + radiusA;
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
