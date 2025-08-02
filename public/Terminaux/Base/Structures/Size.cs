//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
    /// Size struct
    /// </summary>
    [DebuggerDisplay("W: {Width}, H: {Height} ({Width}x{Height})")]
    public struct Size : IEquatable<Size>
    {
        private readonly int width;
        private readonly int height;

        /// <summary>
        /// Gets the width
        /// </summary>
        public readonly int Width =>
            width;

        /// <summary>
        /// Gets the height
        /// </summary>
        public readonly int Height =>
            height;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Size Size && Equals(Size);

        /// <inheritdoc/>
        public bool Equals(Size other)
        {
            return
                Width == other.Width &&
                Height == other.Height;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Generates a string containing sizes
        /// </summary>
        /// <returns>Width and height in a string</returns>
        public override string ToString() =>
            $"{Width}x{Height}";

        /// <inheritdoc/>
        public static bool operator ==(Size left, Size right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Size left, Size right) =>
            !(left == right);

        /// <summary>
        /// Makes a new size instance
        /// </summary>
        /// <param name="width">A zero-based width</param>
        /// <param name="height">A zero-based height</param>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
