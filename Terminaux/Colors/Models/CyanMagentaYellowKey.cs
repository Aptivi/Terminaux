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
    /// The CMYK class instance
    /// </summary>
    [DebuggerDisplay("Black Key = {KWhole}, CMY = {CMY?.CWhole};{CMY?.MWhole};{CMY?.YWhole}")]
    public class CyanMagentaYellowKey : BaseColorModel, IEquatable<CyanMagentaYellowKey>
    {
        /// <summary>
        /// The black key color value [0.0 -> 1.0]
        /// </summary>
        public double K { get; private set; }
        /// <summary>
        /// The black key color value [0 -> 100]
        /// </summary>
        public int KWhole { get; private set; }
        /// <summary>
        /// The Cyan, Magenta, and Yellow color values
        /// </summary>
        public CyanMagentaYellow CMY { get; private set; }

        /// <summary>
        /// cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;
        /// </summary>
        public override string ToString() =>
            $"cmyk:{CMY.CWhole};{CMY.MWhole};{CMY.YWhole};{KWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CyanMagentaYellowKey)obj);

        /// <inheritdoc/>
        public bool Equals(CyanMagentaYellowKey other) =>
            other is not null &&
            K == other.K &&
            EqualityComparer<CyanMagentaYellow>.Default.Equals(CMY, other.CMY);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1604861130;
            hashCode = hashCode * -1521134295 + K.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<CyanMagentaYellow>.Default.GetHashCode(CMY);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CyanMagentaYellowKey left, CyanMagentaYellowKey right) =>
            EqualityComparer<CyanMagentaYellowKey>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CyanMagentaYellowKey left, CyanMagentaYellowKey right) =>
            !(left == right);

        internal CyanMagentaYellowKey(double k, CyanMagentaYellow cmy)
        {
            K = k;
            KWhole = (int)Math.Round(k * 100);
            CMY = cmy;
        }
    }
}
