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

using Terminaux.Base;

namespace Terminaux.Colors.Models.Conversion
{
    /// <summary>
    /// Color model conversion tools (to CMY)
    /// </summary>
    public static class CmyConversionTools
    {
        /// <summary>
        /// Converts the RGB color model to CMY
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to CMY!");

            // Get the level of each color
            var (c, m, y) = GetCmyFromRgb(rgb);

            // Install the values
            return new(c, m, y);
        }

        /// <summary>
        /// Converts the HSL color model to CMY
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to CMY!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(hsl);
            var (c, m, y) = GetCmyFromRgb(rgb);

            // Install the values
            return new(c, m, y);
        }

        /// <summary>
        /// Converts the CMYK color model to CMY
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to CMY!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(cmyk);
            var (c, m, y) = GetCmyFromRgb(rgb);

            // Install the values
            return new(c, m, y);
        }

        /// <summary>
        /// Converts the HSV color model to CMY
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to CMY!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(hsv);
            var (c, m, y) = GetCmyFromRgb(rgb);

            // Install the values
            return new(c, m, y);
        }

        /// <summary>
        /// Converts the RYB color model to CMY
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ConvertFrom(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to CMY!");

            // Get the level of each color
            var rgb = RgbConversionTools.ConvertFrom(ryb);
            var (c, m, y) = GetCmyFromRgb(rgb);

            // Install the values
            return new(c, m, y);
        }

        private static (double c, double m, double y) GetCmyFromRgb(RedGreenBlue rgb)
        {
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Now, get the Cyan, Magenta, and Yellow values
            double c = 1 - levelR;
            double m = 1 - levelG;
            double y = 1 - levelB;
            if (double.IsNaN(c))
                c = 0;
            if (double.IsNaN(m))
                m = 0;
            if (double.IsNaN(y))
                y = 0;

            return (c, m, y);
        }
    }
}
