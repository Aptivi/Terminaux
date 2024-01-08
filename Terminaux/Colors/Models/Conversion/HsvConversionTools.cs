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
    /// Color model conversion tools (to HSV)
    /// </summary>
    public static class HsvConversionTools
    {
        /// <summary>
        /// Converts the CMYK color model to HSV
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmyk);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the RGB color model to HSV
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSV!");

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the CMY color model to HSV
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmy);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the HSL color model to HSV
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsl);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the RYB color model to HSV
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(ryb);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the YIQ color model to HSV
        /// </summary>
        /// <param name="yiq">Instance of YIQ</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(LumaInPhaseQuadrature yiq)
        {
            if (yiq is null)
                throw new TerminauxException("Can't convert a null YIQ instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(yiq);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the YUV color model to HSV
        /// </summary>
        /// <param name="yuv">Instance of YUV</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ConvertFrom(LumaChromaUv yuv)
        {
            if (yuv is null)
                throw new TerminauxException("Can't convert a null YUV instance to HSV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(yuv);

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            return new(hue, saturation, value);
        }

        private static (double hue, double saturation, double value) GetHsvFromRgb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSV!");

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

            // Get the value
            double value = maxLevel;

            // Get the saturation
            double saturation =
                value == 0 ?
                0.0d :
                deltaLevel / maxLevel;

            // Get the hue
            double hue = 0.0d;
            if (saturation != 0)
            {
                if (value == levelR)
                    hue = 0.0 + (levelG - levelB) / deltaLevel;
                else if (value == levelG)
                    hue = 2.0 + (levelB - levelR) / deltaLevel;
                else
                    hue = 4.0 + (levelR - levelG) / deltaLevel;
                hue *= 60;
                if (hue < 0)
                    hue += 360;
                hue /= 360;
            }

            // Return the resulting values
            return (hue, saturation, value);
        }
    }
}
