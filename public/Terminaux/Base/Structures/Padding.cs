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

using System;
using System.Diagnostics;

namespace Terminaux.Base.Structures
{
    /// <summary>
    /// Padding struct
    /// </summary>
    [DebuggerDisplay("L: {Left}, T: {Top}, R: {Right}, B: {Bottom}")]
    public struct Padding : IEquatable<Padding>
    {
        private readonly int left;
        private readonly int top;
        private readonly int right;
        private readonly int bottom;

        /// <summary>
        /// Gets the left padding
        /// </summary>
        public readonly int Left =>
            left;

        /// <summary>
        /// Gets the top padding
        /// </summary>
        public readonly int Top =>
            top;

        /// <summary>
        /// Gets the right padding
        /// </summary>
        public readonly int Right =>
            right;

        /// <summary>
        /// Gets the bottom padding
        /// </summary>
        public readonly int Bottom =>
            bottom;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Padding padding && Equals(padding);

        /// <inheritdoc/>
        public bool Equals(Padding other)
        {
            return
                left == other.left &&
                top == other.top &&
                right == other.right &&
                bottom == other.bottom;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -971476797;
            hashCode = hashCode * -1521134295 + left.GetHashCode();
            hashCode = hashCode * -1521134295 + top.GetHashCode();
            hashCode = hashCode * -1521134295 + right.GetHashCode();
            hashCode = hashCode * -1521134295 + bottom.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Padding left, Padding right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Padding left, Padding right) =>
            !(left == right);

        /// <summary>
        /// Makes a new padding instance
        /// </summary>
        /// <param name="left">A left padding</param>
        /// <param name="top">A top padding</param>
        /// <param name="right">A right padding</param>
        /// <param name="bottom">A bottom padding</param>
        public Padding(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }
}
