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
    /// Color model conversion tools (to YIQ)
    /// </summary>
    public static class YiqConversionTools
    {
        /// <summary>
        /// Converts the CMYK color model to YIQ
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmyk);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the RGB color model to YIQ
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to YIQ!");

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the CMY color model to YIQ
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmy);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the HSL color model to YIQ
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsl);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the RYB color model to YIQ
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(ryb);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the HSV color model to YIQ
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsv);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the YUV color model to YIQ
        /// </summary>
        /// <param name="yuv">Instance of YUV</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ConvertFrom(LumaChromaUv yuv)
        {
            if (yuv is null)
                throw new TerminauxException("Can't convert a null YUV instance to YIQ!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(yuv);

            // Do the conversion
            var (y, i, q) = GetYiqFromRgb(rgb);
            return new(y, i, q);
        }

        private static (double y, double i, double q) GetYiqFromRgb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to YIQ!");

            // Get the YIQ values
            double y = ((0.299 * rgb.R) + (0.587 * rgb.G) + (0.114 * rgb.B)) / 255;
            double i = ((0.596 * rgb.R) + (-0.275 * rgb.G) + (-0.321 * rgb.B)) / 255;
            double q = ((0.212 * rgb.R) + (-0.523 * rgb.G) + (0.311 * rgb.B)) / 255;

            // Return the resulting values
            return (y, i, q);
        }
    }
}
