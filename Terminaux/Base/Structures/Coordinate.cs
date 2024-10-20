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

using System.Diagnostics;

namespace Terminaux.Base.Structures
{
    /// <summary>
    /// Coordinate struct
    /// </summary>
    [DebuggerDisplay("({X}, {Y})")]
    public struct Coordinate
    {
        private int x;
        private int y;

        /// <summary>
        /// Gets the X position
        /// </summary>
        public readonly int X =>
            x;

        /// <summary>
        /// Gets the Y position
        /// </summary>
        public readonly int Y =>
            y;

        /// <summary>
        /// Makes a new coordinate instance
        /// </summary>
        /// <param name="x">A zero-based X coordinate</param>
        /// <param name="y">A zero-based Y coordinate</param>
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
