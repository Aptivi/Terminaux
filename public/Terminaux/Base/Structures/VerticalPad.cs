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
    /// Vertical padding struct
    /// </summary>
    [DebuggerDisplay("T: {Top}, B: {Bottom}")]
    public struct VerticalPad : IEquatable<VerticalPad>
    {
        private readonly int top;
        private readonly int bottom;

        /// <summary>
        /// Gets the top padding
        /// </summary>
        public readonly int Top =>
            top;

        /// <summary>
        /// Gets the bottom padding
        /// </summary>
        public readonly int Bottom =>
            bottom;

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is VerticalPad padding && Equals(padding);

        /// <inheritdoc/>
        public bool Equals(VerticalPad other)
        {
            return
                top == other.top &&
                bottom == other.bottom;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -971476797;
            hashCode = hashCode * -1521134295 + top.GetHashCode();
            hashCode = hashCode * -1521134295 + bottom.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(VerticalPad left, VerticalPad right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(VerticalPad left, VerticalPad right) =>
            !(left == right);

        /// <summary>
        /// Makes a new padding instance
        /// </summary>
        /// <param name="top">A top padding</param>
        /// <param name="bottom">A bottom padding</param>
        public VerticalPad(int top, int bottom)
        {
            this.top = top;
            this.bottom = bottom;
        }
    }
}
