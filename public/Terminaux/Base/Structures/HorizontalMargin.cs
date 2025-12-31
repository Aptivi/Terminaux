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
    /// Horizontal margin struct
    /// </summary>
    [DebuggerDisplay("W: {Width}")]
    public struct HorizontalMargin : IEquatable<HorizontalMargin>
    {
        private readonly int left;
        private readonly int right;
        private readonly int width;

        /// <summary>
        /// Gets the final width with margins applied
        /// </summary>
        public readonly int Width =>
            width - left - right < 0 ? 0 : width - left - right;

        /// <summary>
        /// Gets the margin values
        /// </summary>
        public readonly HorizontalPad Margins =>
            new(left, right);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is HorizontalMargin margin && Equals(margin);

        /// <inheritdoc/>
        public bool Equals(HorizontalMargin other)
        {
            return
                left == other.left &&
                right == other.right &&
                width == other.width;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -260290644;
            hashCode = hashCode * -1521134295 + left.GetHashCode();
            hashCode = hashCode * -1521134295 + right.GetHashCode();
            hashCode = hashCode * -1521134295 + width.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HorizontalMargin left, HorizontalMargin right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(HorizontalMargin left, HorizontalMargin right) =>
            !(left == right);

        /// <summary>
        /// Makes a new horizontal margin instance
        /// </summary>
        /// <param name="left">A left margin</param>
        /// <param name="right">A right margin</param>
        /// <param name="width">Width to subtract from to get margins</param>
        public HorizontalMargin(int left, int right, int width) :
            this(new(left, right), width)
        { }

        /// <summary>
        /// Makes a new horizontal margin instance
        /// </summary>
        /// <param name="margins">Margins</param>
        /// <param name="width">Width to subtract from to get margins</param>
        public HorizontalMargin(HorizontalPad margins, int width)
        {
            left = margins.Left;
            right = margins.Right;
            this.width = width;
        }
    }
}
