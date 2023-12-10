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
    /// The RGB class instance
    /// </summary>
    [DebuggerDisplay("RGB = {R};{G};{B}")]
    public class RedGreenBlue : IEquatable<RedGreenBlue>
    {
        /// <summary>
        /// The red color value
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The green color value
        /// </summary>
        public int G { get; private set; }
        /// <summary>
        /// The blue color value
        /// </summary>
        public int B { get; private set; }

        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"{R};{G};{B}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as RedGreenBlue);

        /// <inheritdoc/>
        public bool Equals(RedGreenBlue other) =>
            other is not null &&
            R == other.R &&
            G == other.G &&
            B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1520100960;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(RedGreenBlue left, RedGreenBlue right) =>
            EqualityComparer<RedGreenBlue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(RedGreenBlue left, RedGreenBlue right) =>
            !(left == right);

        internal RedGreenBlue(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
