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
    /// Margin struct
    /// </summary>
    [DebuggerDisplay("W: {Width}, H: {Height}")]
    public struct Margin : IEquatable<Margin>
    {
        private readonly int left;
        private readonly int top;
        private readonly int right;
        private readonly int bottom;
        private readonly int width;
        private readonly int height;

        /// <summary>
        /// Gets the final width with margins applied
        /// </summary>
        public readonly int Width =>
            width - left - right < 0 ? 0 : width - left - right;

        /// <summary>
        /// Gets the final height with margins applied
        /// </summary>
        public readonly int Height =>
            height - top - bottom < 0 ? 0 : height - top - bottom;

        /// <summary>
        /// Gets the margin values
        /// </summary>
        public readonly Padding Margins =>
            new(left, top, right, bottom);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is Margin margin && Equals(margin);

        /// <inheritdoc/>
        public bool Equals(Margin other)
        {
            return
                left == other.left &&
                top == other.top &&
                right == other.right &&
                bottom == other.bottom &&
                width == other.width &&
                height == other.height;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -260290644;
            hashCode = hashCode * -1521134295 + left.GetHashCode();
            hashCode = hashCode * -1521134295 + top.GetHashCode();
            hashCode = hashCode * -1521134295 + right.GetHashCode();
            hashCode = hashCode * -1521134295 + bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + width.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Margin left, Margin right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(Margin left, Margin right) =>
            !(left == right);

        /// <summary>
        /// Makes a new margin instance
        /// </summary>
        /// <param name="left">A left margin</param>
        /// <param name="top">A top margin</param>
        /// <param name="right">A right margin</param>
        /// <param name="bottom">A bottom margin</param>
        /// <param name="width">Width to subtract from to get margins</param>
        /// <param name="height">Height to subtract from to get margins</param>
        public Margin(int left, int top, int right, int bottom, int width, int height) :
            this(new(left, top, right, bottom), width, height)
        { }

        /// <summary>
        /// Makes a new margin instance
        /// </summary>
        /// <param name="margins">Margins</param>
        /// <param name="width">Width to subtract from to get margins</param>
        /// <param name="height">Height to subtract from to get margins</param>
        public Margin(Padding margins, int width, int height)
        {
            left = margins.Left;
            top = margins.Top;
            right = margins.Right;
            bottom = margins.Bottom;
            this.width = width;
            this.height = height;
        }
    }
}
