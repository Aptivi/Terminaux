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
    /// Rectangle struct
    /// </summary>
    [DebuggerDisplay("W: {Size.Width}, H: {Size.Height}, X: {Coordinate.X}, Y: {Coordinate.Y}")]
    public struct Rect : IEquatable<Rect>
    {
        private readonly Coordinate coord;
        private readonly Size size;

        /// <summary>
        /// Coordinate of the rectangle (starting point, upper left corner)
        /// </summary>
        public readonly Coordinate Coordinate =>
            coord;

        /// <summary>
        /// Coordinate of the rectangle (ending point, lower right corner)
        /// </summary>
        public readonly Coordinate CoordinateEnd =>
            new(coord.X + size.Width - 1, coord.Y + size.Height - 1);

        /// <summary>
        /// Coordinate of the rectangle (upper right corner)
        /// </summary>
        public readonly Coordinate CoordinateUpperRight =>
            new(coord.X + size.Width - 1, coord.Y);

        /// <summary>
        /// Coordinate of the rectangle (lower left corner)
        /// </summary>
        public readonly Coordinate CoordinateLowerLeft =>
            new(coord.X, coord.Y + size.Height - 1);

        /// <summary>
        /// Size of the rectangle
        /// </summary>
        public readonly Size Size =>
            size;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Rect Rect && Equals(Rect);

        /// <inheritdoc/>
        public bool Equals(Rect other)
        {
            return
                Size == other.Size &&
                Coordinate == other.Coordinate;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            hashCode = hashCode * -1521134295 + Coordinate.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Generates a string containing rectangle info
        /// </summary>
        /// <returns>Rectangle info in a string</returns>
        public override string ToString() =>
            $"(S: {Size.Width}x{Size.Height}, C: {Coordinate.X}x{Coordinate.Y})";

        /// <inheritdoc/>
        public static bool operator ==(Rect left, Rect right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Rect left, Rect right) =>
            !(left == right);

        /// <summary>
        /// Makes a new rectangle instance
        /// </summary>
        /// <param name="width">A zero-based width</param>
        /// <param name="height">A zero-based height</param>
        /// <param name="x">A zero-based X coordinate</param>
        /// <param name="y">A zero-based Y coordinate</param>
        public Rect(int width, int height, int x, int y) :
            this(new(width, height), new(x, y))
        { }

        /// <summary>
        /// Makes a new rectangle instance
        /// </summary>
        /// <param name="size">Zero-based width and height</param>
        /// <param name="coord">Zero-based X and Y coordinates</param>
        public Rect(Size size, Coordinate coord)
        {
            this.coord = coord;
            this.size = size;
        }
    }
}
