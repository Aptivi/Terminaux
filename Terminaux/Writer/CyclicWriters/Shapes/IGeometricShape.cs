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

using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Writer.CyclicWriters.Shapes
{
    /// <summary>
    /// Geometric shape interface
    /// </summary>
    public interface IGeometricShape
    {
        /// <summary>
        /// Geometric shape width
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Geometric shape height
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Zero-based left position of the terminal to write this shape to
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Zero-based top position of the terminal to write this shape to
        /// </summary>
        int Top { get; }

        /// <summary>
        /// Shape color. Null equals the current foreground color.
        /// </summary>
        Color ShapeColor { get; }

        /// <summary>
        /// Renders this geometric shape
        /// </summary>
        /// <returns>A rendered geometric shape using a string that you can print to the terminal using <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        string Render();
    }
}
