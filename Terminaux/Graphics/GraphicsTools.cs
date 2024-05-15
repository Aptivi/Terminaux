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
            RenderLine((startX, startY), (endX, endY));

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public static string RenderLine((int startX, int startY) firstPoint, (int endX, int endY) secondPoint)
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
            return buffer.ToString();
        }
    }
}
