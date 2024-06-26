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

using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Colors;
using System;

namespace Terminaux.Graphics.Shapes
{
    /// <summary>
    /// A trapezoid
    /// </summary>
    public class Trapezoid : IGeometricShape
    {
        /// <summary>
        /// Trapezoid width
        /// </summary>
        public int Width =>
            TopWidth;

        /// <summary>
        /// Trapezoid top width
        /// </summary>
        public int TopWidth { get; }

        /// <summary>
        /// Trapezoid bottom width
        /// </summary>
        public int BottomWidth { get; }

        /// <summary>
        /// Trapezoid height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Zero-based left position of the terminal to write this trapezoid to
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Zero-based top position of the terminal to write this trapezoid to
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// Whether to print this filled trapezoid or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <inheritdoc/>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a trapezoid
        /// </summary>
        /// <returns>A rendered trapezoid using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public string Render()
        {
            StringBuilder buffer = new();
            int width = Math.Max(TopWidth, BottomWidth);
            buffer.Append(GraphicsTools.RenderLine((Left + (width / 2) - (TopWidth / 2), Top), (Left + (width / 2) - (BottomWidth / 2), Top + Height), ShapeColor));
            buffer.Append(GraphicsTools.RenderLine((Left + (width / 2) + (TopWidth / 2), Top), (Left + (width / 2) + (BottomWidth / 2), Top + Height), ShapeColor));
            buffer.Append(ColorTools.RenderSetConsoleColor(ShapeColor, true));
            buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + (width / 2) - (TopWidth / 2) + 1, Top + 1));
            buffer.Append(new string(' ', TopWidth));
            buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + (width / 2) - (BottomWidth / 2) + 1, Top + Height + 1));
            buffer.Append(new string(' ', BottomWidth));
            if (Filled)
            {
                for (int y = 0; y < Height; y++)
                {
                    int widthDiff = BottomWidth - TopWidth;
                    int widthDiffByHeight = (int)(widthDiff * ((double)y / Height));
                    int lineWidth = TopWidth + widthDiffByHeight;
                    int pos = (width / 2) - (lineWidth / 2);
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + pos + 1, Top + y + 1));
                    buffer.Append(new string(' ', lineWidth));
                }
            }
            buffer.Append(ColorTools.RenderRevertBackground());
            return buffer.ToString();
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
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ColorTools.CurrentForegroundColor;
        }
    }
}
