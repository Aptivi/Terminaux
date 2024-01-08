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
    /// Color model conversion tools (to CMYK)
    /// </summary>
    public static class CmykConversionTools
    {
        /// <summary>
        /// Converts the RGB color model to CMYK
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to CMYK!");

            // Get the level of each color
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the HSL color model to CMYK
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(hsl);
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the CMY color model to CMYK
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(cmy);
            var (resCmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, resCmy);
        }

        /// <summary>
        /// Converts the HSV color model to CMYK
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(hsv);
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the RYB color model to CMYK
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(ryb);
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the YIQ color model to CMYK
        /// </summary>
        /// <param name="yiq">Instance of YIQ</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(LumaInPhaseQuadrature yiq)
        {
            if (yiq is null)
                throw new TerminauxException("Can't convert a null YIQ instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(yiq);
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the YUV color model to CMYK
        /// </summary>
        /// <param name="yuv">Instance of YUV</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ConvertFrom(LumaChromaUv yuv)
        {
            if (yuv is null)
                throw new TerminauxException("Can't convert a null YUV instance to CMYK!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(yuv);
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            return new(key, cmy);
        }

        private static (CyanMagentaYellow cmy, double k) GetCmykFromRgb(RedGreenBlue rgb)
        {
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Get the black key (K). .NET's Math.Max doesn't support three variables, so this workaround is added
            double maxRgLevel = Math.Max(levelR, levelG);
            double maxLevel = Math.Max(maxRgLevel, levelB);
            double key = 1 - maxLevel;

            // Now, get the Cyan, Magenta, and Yellow values
            double c = (1 - levelR - key) / (1 - key);
            double m = (1 - levelG - key) / (1 - key);
            double y = (1 - levelB - key) / (1 - key);
            if (double.IsNaN(c))
                c = 0;
            if (double.IsNaN(m))
                m = 0;
            if (double.IsNaN(y))
                y = 0;
            var cmy = new CyanMagentaYellow(c, m, y);
            return (cmy, key);
        }
    }
}
