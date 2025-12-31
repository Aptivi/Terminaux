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

using System;
using System.Diagnostics;

namespace Terminaux.Base.Structures
{
    /// <summary>
    /// Coordinate struct
    /// </summary>
    [DebuggerDisplay("({X}, {Y})")]
    public struct Coordinate : IEquatable<Coordinate>
    {
        private readonly int x;
        private readonly int y;

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

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Coordinate coordinate && Equals(coordinate);

        /// <inheritdoc/>
        public bool Equals(Coordinate other)
        {
            return
                X == other.X &&
                Y == other.Y;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Coordinate left, Coordinate right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Coordinate left, Coordinate right) =>
            !(left == right);

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
