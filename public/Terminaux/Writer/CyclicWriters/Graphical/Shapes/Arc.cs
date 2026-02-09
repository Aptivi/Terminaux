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
        /// Center X position of the circle within the canvas
        /// </summary>
        public int CenterPosX { get; set; } = -1;

        /// <summary>
        /// Center Y position of the circle within the canvas
        /// </summary>
        public int CenterPosY { get; set; } = -1;

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
            // the radius of the circle. However, CenterPosX and CenterPosY may override the width and the height halves.
            int centerX = CenterPosX < 0 ? Width / 2 : CenterPosX;
            int centerY = CenterPosY < 0 ? Height / 2 : CenterPosY;
            ConsoleLogger.Debug("Center position: {0}, {1}", centerX, centerY);
            ConsoleLogger.Debug("Overridden positions: {0}, {1}", CenterPosX, CenterPosY);

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
            var plotted = new HashSet<(int x, int y)>();

            // Helper function to add pixel
            void AddPixel(int pixelX, int pixelY)
            {
                var pos = (centerX + pixelX + 1, centerY - pixelY + 1);
                if (plotted.Contains(pos))
                    return;
                double angle = Math.Atan2(pixelY, pixelX);
                if (angle < 0)
                    angle += 2 * Math.PI;
                int ratio = (int)Math.Round(angle * 180 / Math.PI) % 360;
                if (full || (ratio >= angleStart && ratio < angleEnd) ^ inverted)
                {
                    pixels.Add(new(pos.Item1, pos.Item2) { CellColor = ShapeColor });
                    plotted.Add(pos);
                }
            }
            for (int layer = 0; layer <= (OuterRadius - InnerRadius); layer++)
            {
                int layerRadius = InnerRadius + layer;
                int currentRadiusA = (int)(layerRadius * RadiusX / (double)OuterRadius);
                int currentRadiusB = (int)(layerRadius * RadiusY / (double)OuterRadius);

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
                    AddPixel(x, y);
                    AddPixel(-x, y);
                    AddPixel(-x, -y);
                    AddPixel(x, -y);

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
                    AddPixel(x, y);
                    AddPixel(-x, y);
                    AddPixel(-x, -y);
                    AddPixel(x, -y);

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

                int startAngle = full ? 0 : angleStart;
                int endAngle = full ? 360 : angleEnd;
                if (inverted && !full)
                {
                    for (int angle = startAngle; angle < 360; angle++)
                    {
                        double rad = angle * Math.PI / 180;
                        int pX = (int)Math.Round(currentRadiusA * Math.Cos(rad));
                        int pY = (int)Math.Round(currentRadiusB * Math.Sin(rad));
                        AddPixel(pX, pY);
                    }
                    for (int angle = 0; angle <= endAngle; angle++)
                    {
                        double rad = angle * Math.PI / 180;
                        int pX = (int)Math.Round(currentRadiusA * Math.Cos(rad));
                        int pY = (int)Math.Round(currentRadiusB * Math.Sin(rad));
                        AddPixel(pX, pY);
                    }
                }
                else
                {
                    for (int angle = startAngle; angle < endAngle; angle++)
                    {
                        double rad = angle * Math.PI / 180;
                        int pX = (int)Math.Round(currentRadiusA * Math.Cos(rad));
                        int pY = (int)Math.Round(currentRadiusB * Math.Sin(rad));
                        AddPixel(pX, pY);
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
            ShapeColor = shapeColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        }
    }
}
