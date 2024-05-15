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
    /// A triangle
    /// </summary>
    public class Triangle : IGeometricShape
    {
        /// <summary>
        /// Triangle width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Triangle height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Zero-based left position of the terminal to write this triangle to
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Zero-based top position of the terminal to write this triangle to
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// Whether to print this filled triangle or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <inheritdoc/>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a triangle
        /// </summary>
        /// <returns>A rendered triangle using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public string Render()
        {
            StringBuilder buffer = new();
            buffer.Append(ShapeColor.VTSequenceBackgroundTrueColor);
            buffer.Append(GraphicsTools.RenderLine((Left + (Width / 2), Top), (Left, Top + Height)));
            buffer.Append(GraphicsTools.RenderLine((Left + (Width / 2), Top), (Left + Width, Top + Height)));
            buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + Height + 1));
            buffer.Append(new string(' ', Width));
            if (Filled)
            {
                for (int y = 0; y < Height; y++)
                {
                    int widthThreshold = Width * (y + 1) / Height;
                    int LeftPosShift = (Width - widthThreshold) / 2;
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(Left + LeftPosShift + 2, Top + y + 1));
                    buffer.Append(new string(' ', widthThreshold - 1));
                }
            }
            buffer.Append(ColorTools.CurrentBackgroundColor.VTSequenceBackgroundTrueColor);
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new triangle
        /// </summary>
        /// <param name="width">Triangle width</param>
        /// <param name="height">Triangle height</param>
        /// <param name="left">Zero-based left position of the terminal to write this triangle to</param>
        /// <param name="top">Zero-based top position of the terminal to write this triangle to</param>
        /// <param name="filled">Whether to print this filled triangle or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Triangle(int width, int height, int left, int top, bool filled = false, Color? shapeColor = null)
        {
            Width = width;
            Height = height;
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ColorTools.CurrentForegroundColor;
        }
    }
}
