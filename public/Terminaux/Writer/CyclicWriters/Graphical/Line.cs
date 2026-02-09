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

using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Structures;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Line renderable
    /// </summary>
    public class Line : SimpleCyclicWriter
    {
        private Coordinate startPos = new();
        private Coordinate endPos = new();
        private bool antiAlias = false;
        private Color lineColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);

        /// <summary>
        /// Starting position
        /// </summary>
        public Coordinate StartPos
        {
            get => startPos;
            set => startPos = value;
        }

        /// <summary>
        /// Ending position
        /// </summary>
        public Coordinate EndPos
        {
            get => endPos;
            set => endPos = value;
        }

        /// <summary>
        /// Whether to enable anti-aliasing
        /// </summary>
        /// <remarks>
        /// When this option is enabled, the line will be rendered using <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>. Else, the renderer uses <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>.
        /// </remarks>
        public bool AntiAlias
        {
            get => antiAlias;
            set => antiAlias = value;
        }

        /// <summary>
        /// Whether to treat the line as double-width or single-width
        /// </summary>
        public bool DoubleWidth { get; set; } = true;

        /// <summary>
        /// Line color
        /// </summary>
        public Color Color
        {
            get => lineColor;
            set => lineColor = value;
        }

        /// <summary>
        /// Renders a line
        /// </summary>
        /// <returns>Rendered line that will be used by the renderer</returns>
        public override string Render()
        {
            StringBuilder buffer = new();

            // Get the parameters and make a canvas
            var parameters =
                antiAlias ?
                GetCellParamsAntiAlias(StartPos, EndPos, lineColor) :
                GetCellParamsNoAntiAlias(StartPos, EndPos, lineColor);
            ConsoleLogger.Debug("{0} pixels of a line ({1})", parameters.Length, antiAlias);
            ConsoleLogger.Debug("Start: ({0})", StartPos);
            ConsoleLogger.Debug("End: ({0})", EndPos);
            var canvas = new Canvas()
            {
                Transparent = true,
                Height = ConsoleWrapper.WindowHeight,
                Width = ConsoleWrapper.WindowWidth,
                Pixels = parameters,
                DoubleWidth = DoubleWidth,
            };
            buffer.Append(canvas.Render());
            return buffer.ToString();
        }

        internal static CellOptions[] GetCellParamsNoAntiAlias(Coordinate firstPoint, Coordinate secondPoint, Color lineColor)
        {
            List<CellOptions> parameters = [];

            // Get the width and the height
            int posX = firstPoint.X;
            int posY = firstPoint.Y;
            int width = secondPoint.X - firstPoint.X;
            int height = secondPoint.Y - firstPoint.Y;

            // Get the differences according to the given points
            int differenceX1 = width < 0 ? -1 : width > 0 ? 1 : 0;
            int differenceX2 = width < 0 ? -1 : width > 0 ? 1 : 0;
            int differenceY1 = height < 0 ? -1 : height > 0 ? 1 : 0;
            int differenceY2 = 0;

            // Get the longest and the shortest width and height
            int longestLine = Math.Abs(width);
            int shortestLine = Math.Abs(height);
            if (longestLine <= shortestLine)
            {
                longestLine = Math.Abs(height);
                shortestLine = Math.Abs(width);
                differenceY2 = height < 0 ? -1 : height > 0 ? 1 : 0;
                differenceX2 = 0;
            }

            // Now, render a line and move on
            int determine = longestLine >> 1;
            for (int i = 0; i <= longestLine; i++)
            {
                parameters.Add(new(posX + 1, posY + 1) { CellColor = lineColor });
                determine += shortestLine;
                if (determine >= longestLine)
                {
                    // We've reached the longest iteration!
                    determine -= longestLine;
                    posX += differenceX1;
                    posY += differenceY1;
                }
                else
                {
                    // Keep going...
                    posX += differenceX2;
                    posY += differenceY2;
                }
            }
            return [.. parameters];
        }

        internal static CellOptions[] GetCellParamsAntiAlias(Coordinate firstPoint, Coordinate secondPoint, Color lineColor)
        {
            List<CellOptions> parameters = [];

            // Some math helper functions
            int IntPart(double x) =>
                (int)x;

            double FractionalPart(double x)
            {
                if (x > 0)
                    return x - IntPart(x);
                else
                    return x - (IntPart(x) + 1);
            }

            double RFractionalPart(double x) =>
                1 - FractionalPart(x);

            // Store the points, since we may need to modify them
            int x1 = firstPoint.X;
            int x2 = secondPoint.X;
            int y1 = firstPoint.Y;
            int y2 = secondPoint.Y;

            // Check to see if the two points are steep or not
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                (x1, y1) = (y1, x1);
                (x2, y2) = (y2, x2);
            }

            // In case user specified the starting X point bigger than the ending X point
            if (x1 > x2)
            {
                (x1, x2) = (x2, x1);
                (y1, y2) = (y2, y1);
            }

            // Get the differences according to the given points
            int differenceX = x2 - x1;
            int differenceY = y2 - y1;
            if (differenceX == 0 || differenceY == 0)
                return GetCellParamsNoAntiAlias(firstPoint, secondPoint, lineColor);

            // Get the gradient according to the differences
            double gradient;
            if (differenceX == 0)
                gradient = 1.0;
            else
                gradient = (double)differenceY / differenceX;

            // Now, the main loop
            int xPixel1 = x1;
            int xPixel2 = x2;
            double intersection = y1;
            for (int x = xPixel1; x <= xPixel2; x++)
            {
                var intersect1 = new Color(lineColor.RGB.R, lineColor.RGB.G, lineColor.RGB.B, new(ColorTools.GlobalSettings) { Opacity = (int)(FractionalPart(intersection) * 255) });
                var intersect2 = new Color(lineColor.RGB.R, lineColor.RGB.G, lineColor.RGB.B, new(ColorTools.GlobalSettings) { Opacity = (int)(RFractionalPart(intersection) * 255) });
                if (steep)
                {
                    parameters.Add(new(IntPart(intersection) + 1, x + 1) { CellColor = intersect1 });
                    parameters.Add(new(IntPart(intersection), x + 1) { CellColor = intersect2 });
                }
                else
                {
                    parameters.Add(new(x + 1, IntPart(intersection) + 1) { CellColor = intersect1 });
                    parameters.Add(new(x + 1, IntPart(intersection)) { CellColor = intersect2 });
                }
                intersection += gradient;
            }
            return [.. parameters];
        }

        /// <summary>
        /// Makes a new instance of the line renderer
        /// </summary>
        public Line()
        { }
    }
}
