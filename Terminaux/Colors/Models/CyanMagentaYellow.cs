
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
    /// The CMY class instance
    /// </summary>
    [DebuggerDisplay("CMY = {CWhole};{MWhole};{YWhole}")]
    public class CyanMagentaYellow : IEquatable<CyanMagentaYellow>
    {
        /// <summary>
        /// The cyan color value [0.0 -> 1.0]
        /// </summary>
        public double C { get; private set; }
        /// <summary>
        /// The magenta color value [0.0 -> 1.0]
        /// </summary>
        public double M { get; private set; }
        /// <summary>
        /// The yellow color value [0.0 -> 1.0]
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// The cyan color value [0 -> 100]
        /// </summary>
        public int CWhole { get; private set; }
        /// <summary>
        /// The magenta color value [0 -> 100]
        /// </summary>
        public int MWhole { get; private set; }
        /// <summary>
        /// The yellow color value [0 -> 100]
        /// </summary>
        public int YWhole { get; private set; }

        /// <summary>
        /// Converts this instance of CMY color to CMYK model
        /// </summary>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        public CyanMagentaYellowKey ConvertToCmyk() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMY color to RGB model
        /// </summary>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        public RedGreenBlue ConvertToRgb() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMY color to HSL model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        public HueSaturationLightness ConvertToHsl() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMY color to HSV model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationValue"/></returns>
        public HueSaturationValue ConvertToHsv() =>
            new(this);

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as CyanMagentaYellow);

        /// <inheritdoc/>
        public bool Equals(CyanMagentaYellow other) =>
            other is not null &&
                   C == other.C &&
                   M == other.M &&
                   Y == other.Y;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 125415294;
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            hashCode = hashCode * -1521134295 + M.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CyanMagentaYellow left, CyanMagentaYellow right) =>
            EqualityComparer<CyanMagentaYellow>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CyanMagentaYellow left, CyanMagentaYellow right) =>
            !(left == right);

        /// <summary>
        /// Converts the RGB color model to CMY
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellow(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to CMY!");

            // Get the level of each color
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

            // Install the values
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }

        /// <summary>
        /// Converts the HSL color model to CMY
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellow(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to CMY!");

            // Get the level of each color
            var rgb = hsl.ConvertToRgb();
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

            // Install the values
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }

        /// <summary>
        /// Converts the CMYK color model to CMY
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellow(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to CMY!");

            // Get the level of each color
            var rgb = cmyk.ConvertToRgb();
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

            // Install the values
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }

        /// <summary>
        /// Converts the HSV color model to CMY
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellow(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to CMY!");

            // Get the level of each color
            var rgb = hsv.ConvertToRgb();
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

            // Install the values
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }

        internal CyanMagentaYellow(double c, double m, double y)
        {
            C = c;
            M = m;
            Y = y;
            CWhole = (int)(c * 100);
            MWhole = (int)(m * 100);
            YWhole = (int)(y * 100);
        }
    }
}
