
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
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The RGB class instance
    /// </summary>
    [DebuggerDisplay("RGB = {R};{G};{B}")]
    public class RedGreenBlue : IEquatable<RedGreenBlue>
    {
        /// <summary>
        /// The red color value
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The green color value
        /// </summary>
        public int G { get; private set; }
        /// <summary>
        /// The blue color value
        /// </summary>
        public int B { get; private set; }

        /// <summary>
        /// Converts this instance of RGB color to CMY model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellow"/></returns>
        public CyanMagentaYellow ConvertToCmy() =>
            new(this);

        /// <summary>
        /// Converts this instance of RGB color to CMYK model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellowKey"/></returns>
        public CyanMagentaYellowKey ConvertToCmyk() =>
            new(this);

        /// <summary>
        /// Converts this instance of RGB color to HSL model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        public HueSaturationLightness ConvertToHsl() =>
            new(this);

        /// <summary>
        /// Converts this instance of RGB color to HSV model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationValue"/></returns>
        public HueSaturationValue ConvertToHsv() =>
            new(this);

        /// <summary>
        /// Converts this instance of RGB color to RYB model
        /// </summary>
        /// <returns>An instance of <see cref="RedYellowBlue"/></returns>
        public RedYellowBlue ConvertToRyb() =>
            new(this);

        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"{R};{G};{B}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as RedGreenBlue);

        /// <inheritdoc/>
        public bool Equals(RedGreenBlue other) =>
            other is not null &&
            R == other.R &&
            G == other.G &&
            B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1520100960;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(RedGreenBlue left, RedGreenBlue right) =>
            EqualityComparer<RedGreenBlue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(RedGreenBlue left, RedGreenBlue right) =>
            !(left == right);

        /// <summary>
        /// Converts the CMYK color model to RGB
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public RedGreenBlue(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to RGB!");

            // Get the level of each color
            double levelC = 1 - (double)cmyk.CMY.CWhole / 100;
            double levelM = 1 - (double)cmyk.CMY.MWhole / 100;
            double levelY = 1 - (double)cmyk.CMY.YWhole / 100;
            double levelK = 1 - (double)cmyk.KWhole / 100;

            // Now, get the Cyan, Magenta, and Yellow values
            int r = (int)Math.Round(255 * levelC * levelK);
            int g = (int)Math.Round(255 * levelM * levelK);
            int b = (int)Math.Round(255 * levelY * levelK);

            // Install the values
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Converts the HSL color model to RGB
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public RedGreenBlue(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to RGB!");

            // Adjust the RGB values according to saturation
            int r, g, b;
            if (hsl.Saturation == 0)
            {
                // The saturation is zero, so no need to do actual conversion. Just use the lightness.
                r = (int)Math.Round(hsl.Lightness * 255);
                g = (int)Math.Round(hsl.Lightness * 255);
                b = (int)Math.Round(hsl.Lightness * 255);
            }
            else
            {
                // First, get the required variables needed for conversion
                double variable1, variable2;
                if (hsl.Lightness < 0.5)
                    variable2 = hsl.Lightness * (1 + hsl.Saturation);
                else
                    variable2 = (hsl.Lightness + hsl.Saturation) - (hsl.Saturation * hsl.Lightness);
                variable1 = 2 * hsl.Lightness - variable2;

                // Now, do the actual work
                r = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue + (1 / 3.0d)));
                g = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue));
                b = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue - (1 / 3.0d)));
            }

            // Install the values
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Converts the CMY color model to RGB
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public RedGreenBlue(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to RGB!");

            // Get the level of each color
            double levelC = 1 - (double)cmy.CWhole / 100;
            double levelM = 1 - (double)cmy.MWhole / 100;
            double levelY = 1 - (double)cmy.YWhole / 100;

            // Now, get the Cyan, Magenta, and Yellow values
            int r = (int)Math.Round(255 * levelC);
            int g = (int)Math.Round(255 * levelM);
            int b = (int)Math.Round(255 * levelY);

            // Install the values
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Converts the HSV color model to RGB
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public RedGreenBlue(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to RGB!");

            // Get the saturation
            double rFractional = 0.0d, gFractional = 0.0d, bFractional = 0.0d;
            int r, g, b;
            double saturation = hsv.Saturation;
            double value = hsv.Value;
            if (saturation <= 0)
            {
                rFractional = hsv.Value;
                gFractional = hsv.Value;
                bFractional = hsv.Value;
            }
            else
            {
                double hue = hsv.Hue * 6;
                if (hue == 6)
                    hue = 0;
                double i = Math.Floor(hue);
                double colorVal1 = value * (1 - saturation);
                double colorVal2 = value * (1 - saturation * (hue - i));
                double colorVal3 = value * (1 - saturation * (1 - (hue - i)));

                switch (i)
                {
                    case 0:
                        rFractional = value;
                        gFractional = colorVal3;
                        bFractional = colorVal1;
                        break;
                    case 1:
                        rFractional = colorVal2;
                        gFractional = value;
                        bFractional = colorVal1;
                        break;
                    case 2:
                        rFractional = colorVal1;
                        gFractional = value;
                        bFractional = colorVal3;
                        break;
                    case 3:
                        rFractional = colorVal1;
                        gFractional = colorVal2;
                        bFractional = value;
                        break;
                    case 4:
                        rFractional = colorVal3;
                        gFractional = colorVal1;
                        bFractional = value;
                        break;
                    case 5:
                        rFractional = value;
                        gFractional = colorVal1;
                        bFractional = colorVal2;
                        break;
                }
            }

            // Now, get the Cyan, Magenta, and Yellow values
            r = (int)Math.Round(255 * rFractional);
            g = (int)Math.Round(255 * gFractional);
            b = (int)Math.Round(255 * bFractional);

            // Install the values
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Converts the RYB color model to RGB
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public RedGreenBlue(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to RGB!");

            // Get the whiteness and remove it from all the colors
            double white = Math.Min(Math.Min(ryb.R, ryb.Y), ryb.B);
            double redNoWhite = ryb.R - white;
            double yellowNoWhite = ryb.Y - white;
            double blueNoWhite = ryb.B - white;

            // Get the resulting max value
            double maxRyb = Math.Max(Math.Max(redNoWhite, yellowNoWhite), blueNoWhite);

            // Now, remove the green from the yellow and the blue values
            double greenNoWhite = Math.Min(yellowNoWhite, blueNoWhite);
            yellowNoWhite -= greenNoWhite;
            blueNoWhite -= greenNoWhite;

            // Now, check to see if the blue and the green colors are not zeroes. If true, cut these values to half.
            if (blueNoWhite != 0 && greenNoWhite != 0)
            {
                blueNoWhite /= 2.0;
                greenNoWhite /= 2.0;
            }

            // Now, redistribute the yellow remnants to the red and the green level
            redNoWhite += yellowNoWhite;
            greenNoWhite += yellowNoWhite;

            // Do some normalization
            double maxRgb = Math.Max(Math.Max(redNoWhite, greenNoWhite), blueNoWhite);
            if (maxRgb != 0)
            {
                double normalizationFactor = maxRyb / maxRgb;
                redNoWhite *= normalizationFactor;
                greenNoWhite *= normalizationFactor;
                blueNoWhite *= normalizationFactor;
            }

            // Now, restore the white color to the three components
            redNoWhite += white;
            greenNoWhite += white;
            blueNoWhite += white;

            // Install the values
            R = (int)redNoWhite;
            G = (int)greenNoWhite;
            B = (int)blueNoWhite;
        }

        internal RedGreenBlue(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        private double GetRgbValueFromHue(double variable1, double variable2, double variableHue)
        {
            // Check the hue
            if (variableHue < 0)
                variableHue++;
            if (variableHue > 1)
                variableHue--;

            // Now, get the actual value according to the hue
            if ((6 * variableHue) < 1)
                return variable1 + (variable2 - variable1) * 6 * variableHue;
            else if ((2 * variableHue) < 1)
                return variable2;
            else if ((3 * variableHue) < 2)
                return variable1 + (variable2 - variable1) * ((2 / 3.0d - variableHue) * 6);
            return variable1;
        }
    }
}
