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
    /// Color model conversion tools (to RYB)
    /// </summary>
    public static class RybConversionTools
    {
        /// <summary>
        /// Converts the RGB color model to RYB
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to RYB!");

            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the HSL color model to RYB
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(hsl);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the CMYK color model to RYB
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(cmyk);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the CMY color model to RYB
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(cmy);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the HSV color model to RYB
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(hsv);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the YIQ color model to RYB
        /// </summary>
        /// <param name="yiq">Instance of YIQ</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(LumaInPhaseQuadrature yiq)
        {
            if (yiq is null)
                throw new TerminauxException("Can't convert a null YIQ instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(yiq);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        /// <summary>
        /// Converts the YUV color model to RYB
        /// </summary>
        /// <param name="yuv">Instance of YUV</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ConvertFrom(LumaChromaUv yuv)
        {
            if (yuv is null)
                throw new TerminauxException("Can't convert a null YUV instance to RYB!");

            var rgb = RgbConversionTools.ConvertFrom(yuv);
            var (r, y, b) = GetRybFromRgb(rgb);

            // Install the values
            return new(r, y, b);
        }

        private static (int r, int y, int b) GetRybFromRgb(RedGreenBlue rgb)
        {
            // Get the whiteness and remove it from all the colors
            double white = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            double redNoWhite = rgb.R - white;
            double greenNoWhite = rgb.G - white;
            double blueNoWhite = rgb.B - white;

            // Get the resulting max value
            double maxRgb = Math.Max(Math.Max(redNoWhite, greenNoWhite), blueNoWhite);

            // Now, remove the yellow from the red and the green values
            double yellowNoWhite = Math.Min(redNoWhite, greenNoWhite);
            redNoWhite -= yellowNoWhite;
            greenNoWhite -= yellowNoWhite;

            // Now, check to see if the blue and the green colors are not zeroes. If true, cut these values to half.
            if (greenNoWhite != 0 && blueNoWhite != 0)
            {
                greenNoWhite /= 2.0;
                blueNoWhite /= 2.0;
            }

            // Now, redistribute the green remnants to the yellow and the blue level
            yellowNoWhite += greenNoWhite;
            blueNoWhite += greenNoWhite;

            // Do some normalization
            double maxRyb = Math.Max(Math.Max(redNoWhite, yellowNoWhite), blueNoWhite);
            if (maxRyb != 0)
            {
                double normalizationFactor = maxRgb / maxRyb;
                redNoWhite *= normalizationFactor;
                yellowNoWhite *= normalizationFactor;
                blueNoWhite *= normalizationFactor;
            }

            // Now, restore the white color to the three components
            redNoWhite += white;
            yellowNoWhite += white;
            blueNoWhite += white;

            // Cast everything to integers
            int red = (int)Math.Round(redNoWhite);
            int yellow = (int)Math.Round(yellowNoWhite);
            int blue = (int)Math.Round(blueNoWhite);

            // Check the values
            if (red < 0)
                red = 0;
            if (red > 255)
                red = 255;
            if (yellow < 0)
                yellow = 0;
            if (yellow > 255)
                yellow = 255;
            if (blue < 0)
                blue = 0;
            if (blue > 255)
                blue = 255;

            // Return the values
            return (red, yellow, blue);
        }
    }
}
