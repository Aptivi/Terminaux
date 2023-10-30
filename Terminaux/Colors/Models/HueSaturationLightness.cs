
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Hue, Saturation, and Lightness (HSL) model
    /// </summary>
    public class HueSaturationLightness : IEquatable<HueSaturationLightness>
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
        /// Converts this instance of HSL color to RGB model
        /// </summary>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        public RedGreenBlue ConvertToRgb() =>
            new(this);

        /// <summary>
        /// Converts this instance of HSL color to CMYK model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellowKey"/></returns>
        public CyanMagentaYellowKey ConvertToCmyk() =>
            new(this);

        /// <summary>
        /// Converts this instance of HSL color to CMY model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellow"/></returns>
        public CyanMagentaYellow ConvertToCmy() =>
            new(this);

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as HueSaturationLightness);

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

        /// <summary>
        /// Converts the CMYK color model to HSL
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationLightness(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to HSL!");

            // Get the RGB values
            var rgb = cmyk.ConvertToRgb();

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        /// <summary>
        /// Converts the RGB color model to HSL
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationLightness(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSL!");

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
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

        /// <summary>
        /// Converts the CMY color model to HSL
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationLightness(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to HSL!");

            // Get the RGB values
            var rgb = cmy.ConvertToRgb();

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

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

        private (double hue, double saturation, double lightness) GetHslFromRgb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSL!");

            // Get the level of each color
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Get the minimum and maximum color level. .NET's Math.Max doesn't support three variables, so this workaround is added
            double minRgLevel = Math.Min(levelR, levelG);
            double minLevel = Math.Min(minRgLevel, levelB);
            double maxRgLevel = Math.Max(levelR, levelG);
            double maxLevel = Math.Max(maxRgLevel, levelB);

            // Get the delta color level
            double deltaLevel = maxLevel - minLevel;

            // Get the lightness
            double lightness = (maxLevel + minLevel) / 2;

            // Get the hue and the saturation
            double hue = 0.0d;
            double saturation = 0.0d;
            if (deltaLevel != 0)
            {
                // First, the saturation based on the lightness value
                saturation =
                    lightness < 0.5d ?
                    deltaLevel / (maxLevel + minLevel) :
                    deltaLevel / (2 - maxLevel - minLevel);

                // Now, get the delta of R, G, and B values so that we can calculate the hue
                double deltaR = (((maxLevel - levelR) / 6) + (deltaLevel / 2)) / deltaLevel;
                double deltaG = (((maxLevel - levelG) / 6) + (deltaLevel / 2)) / deltaLevel;
                double deltaB = (((maxLevel - levelB) / 6) + (deltaLevel / 2)) / deltaLevel;

                // Now, calculate the hue
                if (levelR == maxLevel)
                    hue = deltaB - deltaG;
                else if (levelG == maxLevel)
                    hue = (1 / 3.0d) + deltaR - deltaB;
                else if (levelB == maxLevel)
                    hue = (2 / 3.0d) + deltaG - deltaR;

                // Verify the hue value so that we don't overflow
                if (hue < 0)
                    hue++;
                if (hue > 1)
                    hue--;
            }

            // Return the resulting values
            return (hue, saturation, lightness);
        }
    }
}
