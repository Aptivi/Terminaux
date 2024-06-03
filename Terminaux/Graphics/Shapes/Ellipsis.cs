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
    /// An ellipsis
    /// </summary>
    public class Ellipsis : IGeometricShape
    {
        /// <summary>
        /// Ellipsis width
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Ellipsis height
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Zero-based left position of the terminal to write this ellipsis to
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// Zero-based top position of the terminal to write this ellipsis to
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// Whether to print this filled ellipsis or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <inheritdoc/>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders an ellipsis
        /// </summary>
        /// <returns>A rendered ellipsis using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public string Render()
        {
            StringBuilder buffer = new();
            buffer.Append(ColorTools.RenderSetConsoleColor(ShapeColor, true));

            // Get the center X and Y positions, since we're dealing with the upper left corner positions, so that we know
            // the radius of the circle
            int centerX = Left + Width / 2;
            int centerY = Top + Height / 2;

            // Now, draw the ellipsis
            for (int i = 0; i < Width; i++)
            {
                // Get the final positions
                int diffX = i - Width / 2;
                int prevDiffX = i - 1 - Width / 2;
                int nextDiffX = i + 1 - Width / 2;
                int drawX = centerX + diffX;

                // Now, get the height difference
                int height = (int)Math.Round(Height * Math.Sqrt(Math.Pow(Width, 2) / 4.0 - Math.Pow(diffX, 2)) / Width);
                int prevHeight = (int)Math.Round(Height * Math.Sqrt(Math.Pow(Width, 2) / 4.0 - Math.Pow(prevDiffX, 2)) / Width);
                int nextHeight = (int)Math.Round(Height * Math.Sqrt(Math.Pow(Width, 2) / 4.0 - Math.Pow(nextDiffX, 2)) / Width);
                for (int diffY = 1; diffY <= height; diffY++)
                {
                    if (((height - prevHeight >= 0 && diffY < prevHeight) || (height - nextHeight >= 0 && diffY < nextHeight)) && !Filled)
                        continue;
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(drawX + 1, centerY + diffY + 1));
                    buffer.Append(' ');
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(drawX + 1, centerY - diffY + 1));
                    buffer.Append(' ');
                }

                // Make sure that we don't have two semi-circles
                if (height > 0 && (Filled || (!Filled && (i == 1 || i == Width - 1))))
                {
                    buffer.Append(CsiSequences.GenerateCsiCursorPosition(drawX + 1, centerY + 1));
                    buffer.Append(' ');
                }
            }
            buffer.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true));
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new ellipsis
        /// </summary>
        /// <param name="width">Ellipsis width</param>
        /// <param name="height">Ellipsis height</param>
        /// <param name="left">Zero-based left position of the terminal to write this ellipsis to</param>
        /// <param name="top">Zero-based top position of the terminal to write this ellipsis to</param>
        /// <param name="filled">Whether to print this filled ellipsis or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Ellipsis(int width, int height, int left, int top, bool filled = false, Color? shapeColor = null)
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
