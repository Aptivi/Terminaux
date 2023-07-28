/*
 * MIT License
 * 
 * Copyright (c) 2022 Aptivi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Terminaux.Colors.Accessibility;
using System;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color type enumeration
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// Color is a true color
        /// </summary>
        TrueColor,
        /// <summary>
        /// Color is a 8-bit color
        /// </summary>
        _255Color,
        /// <summary>
        /// Color is a 4-bit color
        /// </summary>
        _16Color
    }

    /// <summary>
    /// Color tools and management
    /// </summary>
    public static class ColorTools
    {
        internal static Color _empty;
        private static double _deficiency = 0.6;
        /// <summary>
        /// Enables the color transformation to adjust to color blindness upon making a new instance of color
        /// </summary>
        public static bool EnableColorTransformation { get; set; } = false;
        /// <summary>
        /// Enables the simple color transformation. This changes formula from Brettel 1997 (value is false) to Vienot 1999 (value is true)
        /// </summary>
        public static bool EnableSimpleColorTransformation { get; set; } = false;

        /// <summary>
        /// The color deficiency or color blindness type
        /// </summary>
        public static Deficiency ColorDeficiency { get; set; } = Deficiency.Protan;

        /// <summary>
        /// The color deficiency severity
        /// </summary>
        public static double ColorDeficiencySeverity { 
            get => _deficiency;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                if (value > 1)
                    throw new ArgumentOutOfRangeException("value");

                _deficiency = value;
            }
        }

        /// <summary>
        /// Converts from sRGB to Linear RGB using a color number
        /// </summary>
        /// <param name="colorNum">Color number from 0 to 255</param>
        /// <returns>Linear RGB number ranging from 0 to 1</returns>
        public static double SRGBToLinearRGB(int colorNum)
        {
            // Check the value
            if (colorNum < 0)
                colorNum = 0;
            if (colorNum > 255)
                colorNum = 255;

            // Now, convert sRGB to linear RGB (domain is [0, 1])
            double colorNumDbl = colorNum / 255d;
            if (colorNumDbl < 0.04045d)
                return colorNumDbl / 12.92d;
            return Math.Pow((colorNumDbl + 0.055d) / 1.055d, 2.4d);
        }

        /// <summary>
        /// Converts from Linear RGB to sRGB using a linear RGB number
        /// </summary>
        /// <param name="linear">Linear RGB number from 0 to 1</param>
        /// <returns>sRGB value from 0 to 255</returns>
        public static int LinearRGBTosRGB(double linear)
        {
            // Check the value
            if (linear <= 0)
                return 0;
            if (linear >= 1)
                return 255;

            // Now, convert linear value to RGB representation (domain is [0, 255])
            if (linear < 0.0031308d)
                return (int)(0.5d + (linear * 255d * 12.92));
            return (int)(255d * (Math.Pow(linear, 1d / 2.4d) * 1.055d - 0.055d));
        }
    }
}
