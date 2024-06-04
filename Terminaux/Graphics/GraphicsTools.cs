//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Graphics
{
    /// <summary>
    /// Graphics tools
    /// </summary>
    public static class GraphicsTools
    {
        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="startX">Line start position (zero-based X position)</param>
        /// <param name="startY">Line start position (zero-based Y position)</param>
        /// <param name="endX">Line end position (zero-based X position)</param>
        /// <param name="endY">Line end position (zero-based Y position)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLine(int startX, int startY, int endX, int endY) =>
            RenderLine((startX, startY), (endX, endY), ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLine((int startX, int startY) firstPoint, (int endX, int endY) secondPoint) =>
            RenderLine(firstPoint, secondPoint, ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="startX">Line start position (zero-based X position)</param>
        /// <param name="startY">Line start position (zero-based Y position)</param>
        /// <param name="endX">Line end position (zero-based X position)</param>
        /// <param name="endY">Line end position (zero-based Y position)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLine(int startX, int startY, int endX, int endY, Color lineColor) =>
            RenderLine((startX, startY), (endX, endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLine((int startX, int startY) firstPoint, (int endX, int endY) secondPoint, Color lineColor)
        {
            StringBuilder buffer = new();

            // Get the width and the height
            int posX = firstPoint.startX;
            int posY = firstPoint.startY;
            int width = secondPoint.endX - firstPoint.startX;
            int height = secondPoint.endY - firstPoint.startY;

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
            buffer.Append(ColorTools.RenderSetConsoleColor(lineColor, true));
            for (int i = 0; i <= longestLine; i++)
            {
                buffer.Append(CsiSequences.GenerateCsiCursorPosition(posX + 1, posY + 1));
                buffer.Append(' ');
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
            buffer.Append(ColorTools.RenderRevertBackground());
            return buffer.ToString();
        }

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="startX">Line start position (zero-based X position)</param>
        /// <param name="startY">Line start position (zero-based Y position)</param>
        /// <param name="endX">Line end position (zero-based X position)</param>
        /// <param name="endY">Line end position (zero-based Y position)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLineSmooth(int startX, int startY, int endX, int endY) =>
            RenderLineSmooth((startX, startY), (endX, endY), ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLineSmooth((int startX, int startY) firstPoint, (int endX, int endY) secondPoint) =>
            RenderLineSmooth(firstPoint, secondPoint, ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="startX">Line start position (zero-based X position)</param>
        /// <param name="startY">Line start position (zero-based Y position)</param>
        /// <param name="endX">Line end position (zero-based X position)</param>
        /// <param name="endY">Line end position (zero-based Y position)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLineSmooth(int startX, int startY, int endX, int endY, Color lineColor) =>
            RenderLineSmooth((startX, startY), (endX, endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLineSmooth((int startX, int startY) firstPoint, (int endX, int endY) secondPoint, Color lineColor)
        {
            StringBuilder buffer = new();

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
            int x1 = firstPoint.startX;
            int x2 = secondPoint.endX;
            int y1 = firstPoint.startY;
            int y2 = secondPoint.endY;

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
                return RenderLine(firstPoint, secondPoint, lineColor);

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
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(IntPart(intersection) + 1, x + 1));
                    buffer.Append(ColorTools.RenderSetConsoleColor(intersect1, true));
                    buffer.Append(' ');
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(IntPart(intersection), x + 1));
                    buffer.Append(ColorTools.RenderSetConsoleColor(intersect2, true));
                    buffer.Append(' ');
                }
                else
                {
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(x + 1, IntPart(intersection) + 1));
                    buffer.Append(ColorTools.RenderSetConsoleColor(intersect1, true));
                    buffer.Append(' ');
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(x + 1, IntPart(intersection)));
                    buffer.Append(ColorTools.RenderSetConsoleColor(intersect2, true));
                    buffer.Append(' ');
                }
                intersection += gradient;
            }
            buffer.Append(ColorTools.RenderRevertBackground());
            return buffer.ToString();
        }
    }
}
