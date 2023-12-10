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
using System.Collections.Generic;
using System.Diagnostics;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The RYB class instance
    /// </summary>
    [DebuggerDisplay("RYB = {R};{Y};{B}")]
    public class RedYellowBlue : IEquatable<RedYellowBlue>
    {
        /// <summary>
        /// The red color value [0 -> 100]
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The yellow color value [0 -> 100]
        /// </summary>
        public int Y { get; private set; }
        /// <summary>
        /// The blue color value [0 -> 100]
        /// </summary>
        public int B { get; private set; }

        /// <summary>
        /// ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"ryb:{R};{Y};{B}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as RedYellowBlue);

        /// <inheritdoc/>
        public bool Equals(RedYellowBlue other) =>
            other is not null &&
                   R == other.R &&
                   Y == other.Y &&
                   B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -636965442;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(RedYellowBlue left, RedYellowBlue right) =>
            EqualityComparer<RedYellowBlue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(RedYellowBlue left, RedYellowBlue right) =>
            !(left == right);

        internal RedYellowBlue(int r, int y, int b)
        {
            R = r;
            Y = y;
            B = b;
        }
    }
}
