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
    /// The Hue, Saturation, and Lightness (HSL) model
    /// </summary>
    [DebuggerDisplay("HSL = {HueWhole};{SaturationWhole};{LightnessWhole}")]
    public class HueSaturationLightness : BaseColorModel, IEquatable<HueSaturationLightness>
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
        /// The lightness of the color, indicating how light the color is [0.0 -> 1.0]
        /// </summary>
        public double Lightness { get; private set; }
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
        /// The lightness of the color, indicating how light the color is [0 -> 100]
        /// </summary>
        public int LightnessWhole { get; private set; }
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0 -> 360]
        /// </summary>
        public double ReverseHueWhole { get; private set; }

        /// <summary>
        /// hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;
        /// </summary>
        public override string ToString() =>
            $"hsl:{HueWhole};{SaturationWhole};{LightnessWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((HueSaturationLightness)obj);

        /// <inheritdoc/>
        public bool Equals(HueSaturationLightness other) =>
            other is not null &&
            Hue == other.Hue &&
            Saturation == other.Saturation &&
            Lightness == other.Lightness &&
            HueWhole == other.HueWhole &&
            SaturationWhole == other.SaturationWhole &&
            LightnessWhole == other.LightnessWhole;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -871917995;
            hashCode = hashCode * -1521134295 + Hue.GetHashCode();
            hashCode = hashCode * -1521134295 + Saturation.GetHashCode();
            hashCode = hashCode * -1521134295 + Lightness.GetHashCode();
            hashCode = hashCode * -1521134295 + HueWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + SaturationWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + LightnessWhole.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HueSaturationLightness left, HueSaturationLightness right) =>
            EqualityComparer<HueSaturationLightness>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HueSaturationLightness left, HueSaturationLightness right) =>
            !(left == right);

        internal HueSaturationLightness(double hue, double saturation, double lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            LightnessWhole = (int)(Lightness * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }
    }
}
