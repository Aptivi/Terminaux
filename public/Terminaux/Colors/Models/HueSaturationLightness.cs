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
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Textify.General;

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
        public int HueWhole =>
            (int)(Hue * 360);
        /// <summary>
        /// The saturation of the color, indicating how intense the color is [0 -> 100]
        /// </summary>
        public int SaturationWhole =>
            (int)(Saturation * 100);
        /// <summary>
        /// The lightness of the color, indicating how light the color is [0 -> 100]
        /// </summary>
        public int LightnessWhole =>
            (int)(Lightness * 100);
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0 -> 360]
        /// </summary>
        public double ReverseHueWhole =>
            (int)(ReverseHue * 360);

        /// <summary>
        /// hsl:&lt;H&gt;;&lt;S&gt;;&lt;L&gt;
        /// </summary>
        public override string ToString() =>
            $"hsl:{HueWhole};{SaturationWhole};{LightnessWhole}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueSaturationLightness"/>
        /// </summary>
        /// <param name="specifier">Specifier of HSL</param>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException("Invalid HSL color specifier \"{0}\". Ensure that it's on the correct format".FormatString(specifier) + ": hsl:<hue>;<sat>;<lig>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the HSL whole values! First, check to see if we need to filter the color for the color-blind
                int h = Convert.ToInt32(specifierArray[0]);
                if (h < 0 || h > 360)
                    throw new TerminauxException("The hue level is out of range (0' -> 360' degrees)." + $" {h}");
                int s = Convert.ToInt32(specifierArray[1]);
                if (s < 0 || s > 100)
                    throw new TerminauxException("The saturation level is out of range (0 -> 100)." + $" {s}");
                int l = Convert.ToInt32(specifierArray[2]);
                if (l < 0 || l > 100)
                    throw new TerminauxException("The lightness level is out of range (0 -> 100)." + $" {l}");

                // First, we need to convert from HSL to RGB
                double hPart = (double)h / 360;
                double sPart = (double)s / 100;
                double lPart = (double)l / 100;
                var hsl = new HueSaturationLightness(hPart, sPart, lPart);
                return hsl;
            }
            else
                throw new TerminauxException("Invalid HSL color specifier \"{0}\". The specifier may not be more than three elements. Ensure that it's on the correct format".FormatString(specifier) + ": hsl:<hue>;<sat>;<lig>");
        }

        /// <summary>
        /// Does the string specifier represent a valid HSL specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HSL specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("hsl:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid HSL specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HSL specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int h = Convert.ToInt32(specifierArray[0]);
            if (h < 0 || h > 360)
                return false;
            int s = Convert.ToInt32(specifierArray[1]);
            if (s < 0 || s > 100)
                return false;
            int l = Convert.ToInt32(specifierArray[2]);
            if (l < 0 || l > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueSaturationLightness"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of HSL</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var hsl = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(hsl);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

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
        }
    }
}
