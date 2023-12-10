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
using Terminaux.Base;

namespace Terminaux.Colors.Models.Conversion
{
    /// <summary>
    /// Color model conversion tools (to HSL)
    /// </summary>
    public static class HslConversionTools
    {
        /// <summary>
        /// Converts the CMYK color model to HSL
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to HSL!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmyk);

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            return new(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts the RGB color model to HSL
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSL!");

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            return new(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts the CMY color model to HSL
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to HSL!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmy);

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            return new(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts the HSV color model to HSL
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to HSL!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsv);

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            return new(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts the RYB color model to HSL
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to HSL!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(ryb);

            // Do the conversion
            var (hue, saturation, lightness) = GetHslFromRgb(rgb);
            return new(hue, saturation, lightness);
        }

        private static (double hue, double saturation, double lightness) GetHslFromRgb(RedGreenBlue rgb)
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
