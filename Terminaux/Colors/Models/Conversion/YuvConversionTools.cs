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
    /// Color model conversion tools (to YUV)
    /// </summary>
    public static class YuvConversionTools
    {
        /// <summary>
        /// Converts the CMYK color model to YUV
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmyk);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the RGB color model to YUV
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to YUV!");

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the CMY color model to YUV
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(cmy);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the HSL color model to YUV
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsl);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the RYB color model to YUV
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(ryb);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the HSV color model to YUV
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(hsv);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the YIQ color model to YUV
        /// </summary>
        /// <param name="yiq">Instance of YIQ</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ConvertFrom(LumaInPhaseQuadrature yiq)
        {
            if (yiq is null)
                throw new TerminauxException("Can't convert a null YIQ instance to YUV!");

            // Get the RGB values
            var rgb = RgbConversionTools.ConvertFrom(yiq);

            // Do the conversion
            var (y, u, v) = GetYuvFromRgb(rgb);
            return new(y, u, v);
        }

        private static (double y, double u, double v) GetYuvFromRgb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to YUV!");

            // Get the YUV values
            double y = (0.299 * rgb.R) + (0.587 * rgb.G) + (0.114 * rgb.B);
            double u = (-0.168736 * rgb.R) + (-0.331264 * rgb.G) + (0.500000 * rgb.B) + 128;
            double v = (0.500000 * rgb.R) + (-0.418688 * rgb.G) + (-0.081312 * rgb.B) + 128;

            // Return the resulting values
            return (y, u, v);
        }
    }
}
