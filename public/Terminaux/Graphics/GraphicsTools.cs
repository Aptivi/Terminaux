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

using System;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Graphics
{
    /// <summary>
    /// Graphics tools
    /// </summary>
    [Obsolete("We're phasing out GraphicsTools in favor of CyclicWriters.Renderer.Tools.")]
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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine(int startX, int startY, int endX, int endY) =>
            RenderLine((startX, startY), (endX, endY), ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine((int startX, int startY) firstPoint, (int endX, int endY) secondPoint) =>
            RenderLine(firstPoint, secondPoint, ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine(Coordinate firstPoint, Coordinate secondPoint) =>
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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine(int startX, int startY, int endX, int endY, Color lineColor) =>
            RenderLine((startX, startY), (endX, endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine((int startX, int startY) firstPoint, (int endX, int endY) secondPoint, Color lineColor) =>
            RenderLine(new Coordinate(firstPoint.startX, firstPoint.startY), new Coordinate(secondPoint.endX, secondPoint.endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm">Bresenham's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLine(Coordinate firstPoint, Coordinate secondPoint, Color lineColor) =>
            new Line()
            {
                StartPos = firstPoint,
                EndPos = secondPoint,
                Color = lineColor
            }.Render();

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="startX">Line start position (zero-based X position)</param>
        /// <param name="startY">Line start position (zero-based Y position)</param>
        /// <param name="endX">Line end position (zero-based X position)</param>
        /// <param name="endY">Line end position (zero-based Y position)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth(int startX, int startY, int endX, int endY) =>
            RenderLineSmooth((startX, startY), (endX, endY), ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth((int startX, int startY) firstPoint, (int endX, int endY) secondPoint) =>
            RenderLineSmooth(firstPoint, secondPoint, ColorTools.CurrentForegroundColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth(Coordinate firstPoint, Coordinate secondPoint) =>
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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth(int startX, int startY, int endX, int endY, Color lineColor) =>
            RenderLineSmooth((startX, startY), (endX, endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth((int startX, int startY) firstPoint, (int endX, int endY) secondPoint, Color lineColor) =>
            RenderLineSmooth(new Coordinate(firstPoint.startX, firstPoint.startY), new Coordinate(secondPoint.endX, secondPoint.endY), lineColor);

        /// <summary>
        /// Renders a line using the <see href="https://en.wikipedia.org/wiki/Xiaolin_Wu%27s_line_algorithm">Xiaolin Wu's line algorithm</see>
        /// </summary>
        /// <param name="firstPoint">Line start positions (zero-based X and Y positions)</param>
        /// <param name="secondPoint">Line end position (zero-based X and Y positions)</param>
        /// <param name="lineColor">Line color</param>
        /// <returns>A rendered line that you can print with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderLineSmooth(Coordinate firstPoint, Coordinate secondPoint, Color lineColor) =>
            new Line()
            {
                AntiAlias = true,
                StartPos = firstPoint,
                EndPos = secondPoint,
                Color = lineColor
            }.Render();
    }
}
