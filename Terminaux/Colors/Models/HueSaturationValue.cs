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
    /// The Hue, Saturation, and Value (HSV) model
    /// </summary>
    [DebuggerDisplay("HSV = {HueWhole};{SaturationWhole};{ValueWhole}")]
    public class HueSaturationValue : IEquatable<HueSaturationValue>
    {
        /// <summary>
        /// The position on the color wheel, known as the Hue [0.0 -> 1.0, 0.5 being 180 degrees]
        /// </summary>
        public double Hue { get; private set; }
        /// <summary>
        /// The saturation of the color, indicating how intense the color is [0.0 -> 1.0]
        /// </summary>
        public double Saturation { get; private set; }
        /// <summary>
        /// The value of the color [0.0 -> 1.0]
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0.0 -> 1.0, 0.5 being 180 degrees]
        /// </summary>
        public double ReverseHue { get; private set; }
        /// <summary>
        /// The position on the color wheel, known as the Hue [0 -> 360]
        /// </summary>
        public int HueWhole { get; private set; }
        /// <summary>
        /// The saturation of the color, indicating how intense the color is [0 -> 100]
        /// </summary>
        public int SaturationWhole { get; private set; }
        /// <summary>
        /// The value of the color [0 -> 100]
        /// </summary>
        public int ValueWhole { get; private set; }
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0 -> 360]
        /// </summary>
        public double ReverseHueWhole { get; private set; }

        /// <summary>
        /// hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;
        /// </summary>
        public override string ToString() =>
            $"hsv:{HueWhole};{SaturationWhole};{ValueWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((HueSaturationValue)obj);

        /// <inheritdoc/>
        public bool Equals(HueSaturationValue other) =>
            other is not null &&
            Hue == other.Hue &&
            Saturation == other.Saturation &&
            Value == other.Value &&
            HueWhole == other.HueWhole &&
            SaturationWhole == other.SaturationWhole &&
            ValueWhole == other.ValueWhole;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -871917995;
            hashCode = hashCode * -1521134295 + Hue.GetHashCode();
            hashCode = hashCode * -1521134295 + Saturation.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + HueWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + SaturationWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueWhole.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HueSaturationValue left, HueSaturationValue right) =>
            EqualityComparer<HueSaturationValue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HueSaturationValue left, HueSaturationValue right) =>
            !(left == right);

        internal HueSaturationValue(double hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }
    }
}
