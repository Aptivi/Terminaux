
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
