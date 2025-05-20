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
    /// Horizontal padding struct
    /// </summary>
    [DebuggerDisplay("L: {Left}, R: {Right}")]
    public struct HorizontalPad : IEquatable<HorizontalPad>
    {
        private readonly int left;
        private readonly int right;

        /// <summary>
        /// Gets the left padding
        /// </summary>
        public readonly int Left =>
            left;

        /// <summary>
        /// Gets the right padding
        /// </summary>
        public readonly int Right =>
            right;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is HorizontalPad padding && Equals(padding);

        /// <inheritdoc/>
        public bool Equals(HorizontalPad other)
        {
            return
                left == other.left &&
                right == other.right;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -971476797;
            hashCode = hashCode * -1521134295 + left.GetHashCode();
            hashCode = hashCode * -1521134295 + right.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HorizontalPad left, HorizontalPad right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(HorizontalPad left, HorizontalPad right) =>
            !(left == right);

        /// <summary>
        /// Makes a new horizontal padding instance
        /// </summary>
        /// <param name="left">A left padding</param>
        /// <param name="right">A right padding</param>
        public HorizontalPad(int left, int right)
        {
            this.left = left;
            this.right = right;
        }
    }
}
