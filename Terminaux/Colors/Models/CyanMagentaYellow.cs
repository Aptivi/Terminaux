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
    /// The CMY class instance
    /// </summary>
    [DebuggerDisplay("CMY = {CWhole};{MWhole};{YWhole}")]
    public class CyanMagentaYellow : IEquatable<CyanMagentaYellow>
    {
        /// <summary>
        /// The cyan color value [0.0 -> 1.0]
        /// </summary>
        public double C { get; private set; }
        /// <summary>
        /// The magenta color value [0.0 -> 1.0]
        /// </summary>
        public double M { get; private set; }
        /// <summary>
        /// The yellow color value [0.0 -> 1.0]
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// The cyan color value [0 -> 100]
        /// </summary>
        public int CWhole { get; private set; }
        /// <summary>
        /// The magenta color value [0 -> 100]
        /// </summary>
        public int MWhole { get; private set; }
        /// <summary>
        /// The yellow color value [0 -> 100]
        /// </summary>
        public int YWhole { get; private set; }

        /// <summary>
        /// cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;
        /// </summary>
        public override string ToString() =>
            $"cmy:{CWhole};{MWhole};{YWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as CyanMagentaYellow);

        /// <inheritdoc/>
        public bool Equals(CyanMagentaYellow other) =>
            other is not null &&
                   C == other.C &&
                   M == other.M &&
                   Y == other.Y;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 125415294;
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            hashCode = hashCode * -1521134295 + M.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CyanMagentaYellow left, CyanMagentaYellow right) =>
            EqualityComparer<CyanMagentaYellow>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CyanMagentaYellow left, CyanMagentaYellow right) =>
            !(left == right);

        internal CyanMagentaYellow(double c, double m, double y)
        {
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }
    }
}
