﻿//
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
    /// Margin struct
    /// </summary>
    [DebuggerDisplay("H: {Height}")]
    public struct VerticalMargin : IEquatable<VerticalMargin>
    {
        private readonly int top;
        private readonly int bottom;
        private readonly int height;

        /// <summary>
        /// Gets the final height with margins applied
        /// </summary>
        public readonly int Height =>
            height - top - bottom < 0 ? 0 : height - top - bottom;

        /// <summary>
        /// Gets the margin values
        /// </summary>
        public readonly VerticalPad Margins =>
            new(top, bottom);

        /// <inheritdoc/>
        public override bool Equals(object? obj) =>
            obj is VerticalMargin margin && Equals(margin);

        /// <inheritdoc/>
        public bool Equals(VerticalMargin other)
        {
            return
                top == other.top &&
                bottom == other.bottom &&
                height == other.height;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -260290644;
            hashCode = hashCode * -1521134295 + top.GetHashCode();
            hashCode = hashCode * -1521134295 + bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(VerticalMargin left, VerticalMargin right) =>
            left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(VerticalMargin left, VerticalMargin right) =>
            !(left == right);

        /// <summary>
        /// Makes a new vertical margin instance
        /// </summary>
        /// <param name="top">A top margin</param>
        /// <param name="bottom">A bottom margin</param>
        /// <param name="height">Height to subtract from to get margins</param>
        public VerticalMargin(int top, int bottom, int height) :
            this(new(top, bottom), height)
        { }

        /// <summary>
        /// Makes a new vertical margin instance
        /// </summary>
        /// <param name="margins">Margins</param>
        /// <param name="height">Height to subtract from to get margins</param>
        public VerticalMargin(VerticalPad margins, int height)
        {
            top = margins.Top;
            bottom = margins.Bottom;
            this.height = height;
        }
    }
}
