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
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;
using Textify.General;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Hue, Whiteness, and Blackness (HWB) model
    /// </summary>
    [DebuggerDisplay("HWB = {HueWhole};{WhitenessWhole};{BlacknessWhole}")]
    public class HueWhiteBlack : BaseColorModel, IEquatable<HueWhiteBlack>
    {
        /// <summary>
        /// The position on the color wheel, known as the Hue [0.0 -> 1.0, 0.5 being 180 degrees]
        /// </summary>
        public double Hue { get; private set; }
        /// <summary>
        /// The whiteness of the color, indicating how intense the color is [0.0 -> 1.0]
        /// </summary>
        public double Whiteness { get; private set; }
        /// <summary>
        /// The blackness of the color [0.0 -> 1.0]
        /// </summary>
        public double Blackness { get; private set; }
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
        /// The whiteness of the color, indicating how intense the color is [0 -> 100]
        /// </summary>
        public int WhitenessWhole =>
            (int)(Whiteness * 100);
        /// <summary>
        /// The blackness of the color [0 -> 100]
        /// </summary>
        public int BlacknessWhole =>
            (int)(Blackness * 100);
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0 -> 360]
        /// </summary>
        public double ReverseHueWhole =>
            (int)(ReverseHue * 360);

        /// <summary>
        /// hwb:&lt;H&gt;;&lt;W&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"hwb:{HueWhole};{WhitenessWhole};{BlacknessWhole}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueWhiteBlack"/>
        /// </summary>
        /// <param name="specifier">Specifier of HWB</param>
        /// <returns>An instance of <see cref="HueWhiteBlack"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static HueWhiteBlack ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHWBSPECIFIER").FormatString(specifier) + ": hwb:<hue>;<whiteness>;<blackness>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the HWB whole blacknesss! First, check to see if we need to filter the color for the color-blind
                int h = Convert.ToInt32(specifierArray[0]);
                if (h < 0 || h > 360)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHSLHLEVEL") + $" {h}");
                int s = Convert.ToInt32(specifierArray[1]);
                if (s < 0 || s > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHWBWLEVEL") + $" {s}");
                int v = Convert.ToInt32(specifierArray[2]);
                if (v < 0 || v > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHWBBLEVEL") + $" {v}");

                // First, we need to convert from HWB to RGB
                double hPart = (double)h / 360;
                double wPart = (double)s / 100;
                double bPart = (double)v / 100;
                var hwb = new HueWhiteBlack(hPart, wPart, bPart);
                return hwb;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHWBSPECIFIEREXCEED").FormatString(specifier) + ": hwb:<hue>;<whiteness>;<blackness>");
        }

        /// <summary>
        /// Does the string specifier represent a valid HWB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HWB specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("hwb:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid HWB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HWB specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int h = Convert.ToInt32(specifierArray[0]);
            if (h < 0 || h > 360)
                return false;
            int w = Convert.ToInt32(specifierArray[1]);
            if (w < 0 || w > 100)
                return false;
            int b = Convert.ToInt32(specifierArray[2]);
            if (b < 0 || b > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueWhiteBlack"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of HWB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var hwb = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(hwb);
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
            Equals((HueWhiteBlack)obj);

        /// <inheritdoc/>
        public bool Equals(HueWhiteBlack other) =>
            other is not null &&
            Hue == other.Hue &&
            Whiteness == other.Whiteness &&
            Blackness == other.Blackness &&
            HueWhole == other.HueWhole &&
            WhitenessWhole == other.WhitenessWhole &&
            BlacknessWhole == other.BlacknessWhole;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -871917995;
            hashCode = hashCode * -1521134295 + Hue.GetHashCode();
            hashCode = hashCode * -1521134295 + Whiteness.GetHashCode();
            hashCode = hashCode * -1521134295 + Blackness.GetHashCode();
            hashCode = hashCode * -1521134295 + HueWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + WhitenessWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + BlacknessWhole.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HueWhiteBlack left, HueWhiteBlack right) =>
            EqualityComparer<HueWhiteBlack>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HueWhiteBlack left, HueWhiteBlack right) =>
            !(left == right);

        internal HueWhiteBlack(double hue, double whiteness, double blackness)
        {
            Hue = hue;
            Whiteness = whiteness;
            Blackness = blackness;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;
        }
    }
}
