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
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Graphical.Shapes
{
    /// <summary>
    /// An ellipsis
    /// </summary>
    public class Ellipsis : GraphicalCyclicWriter
    {
        /// <summary>
        /// Whether to print this filled ellipsis or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders an ellipsis
        /// </summary>
        /// <returns>A rendered ellipsis using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            // Make a new arc
            var arc = new Arc(Height, Left, Top, ShapeColor)
            {
                InnerRadius = Filled ? 0 : Height / 2,
                OuterRadius = Height / 2,
                AngleStart = 0,
                AngleEnd = 360,
            };

            // Check to see if it's a circle or an ellipsis
            if (Width != Height)
            {
                arc.Width = Width;
                arc.Height = Height;
                arc.RadiusX = Width / 2;
                arc.RadiusY = Height / 2;
            }

            // Render the arc
            return arc.Render();
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
            ShapeColor = shapeColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        }
    }
}
