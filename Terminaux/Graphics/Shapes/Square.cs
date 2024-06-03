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

namespace Terminaux.Graphics.Shapes
{
    /// <summary>
    /// A square
    /// </summary>
    public class Square : IGeometricShape
    {
        /// <summary>
        /// Square width
        /// </summary>
        public int Width =>
            Height * 2;

        /// <summary>
        /// Square height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Zero-based left position of the terminal to write this square to
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Zero-based top position of the terminal to write this square to
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// Whether to print this filled square or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <inheritdoc/>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a square
        /// </summary>
        /// <returns>A rendered square using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public string Render()
        {
            StringBuilder buffer = new();
            buffer.Append(ColorTools.RenderSetConsoleColor(ShapeColor, true));
            for (int y = 0; y < Height; y++)
            {
                buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + y + 1));
                bool isOutline = y == 0 || y == Height - 1;
                if (isOutline || Filled)
                    buffer.Append(new string(' ', Width));
                else
                {
                    buffer.Append("  ");
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + Width - 1, Top + y + 1));
                    buffer.Append("  ");
                }
            }
            buffer.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true));
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new square
        /// </summary>
        /// <param name="height">Square height</param>
        /// <param name="left">Zero-based left position of the terminal to write this square to</param>
        /// <param name="top">Zero-based top position of the terminal to write this square to</param>
        /// <param name="filled">Whether to print this filled square or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Square(int height, int left, int top, bool filled = false, Color? shapeColor = null)
        {
            Height = height;
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ColorTools.CurrentForegroundColor;
        }
    }
}
