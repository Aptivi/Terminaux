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

using System.Text;
using Terminaux.Writer.ConsoleWriters;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Graphical.Shapes
{
    /// <summary>
    /// A circle
    /// </summary>
    public class Circle : GraphicalCyclicWriter
    {
        /// <summary>
        /// Whether to print this filled circle or just the outline
        /// </summary>
        public bool Filled { get; }

        /// <summary>
        /// Shape color
        /// </summary>
        public Color ShapeColor { get; }

        /// <summary>
        /// Renders a circle
        /// </summary>
        /// <returns>A rendered circle using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        public override string Render()
        {
            StringBuilder buffer = new();
            buffer.Append(new Ellipsis(Width, Height, Left, Top, Filled, ShapeColor).Render());
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new circle
        /// </summary>
        /// <param name="height">Circle height</param>
        /// <param name="left">Zero-based left position of the terminal to write this circle to</param>
        /// <param name="top">Zero-based top position of the terminal to write this circle to</param>
        /// <param name="filled">Whether to print this filled circle or just the outline</param>
        /// <param name="shapeColor">Shape color. Null equals the current foreground color.</param>
        public Circle(int height, int left, int top, bool filled = false, Color? shapeColor = null)
        {
            Height = height;
            Width = height;
            Left = left;
            Top = top;
            Filled = filled;
            ShapeColor = shapeColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        }
    }
}
