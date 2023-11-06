
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
    /// The CMYK class instance
    /// </summary>
    [DebuggerDisplay("Black Key = {KWhole}, CMY = {CMY?.CWhole};{CMY?.MWhole};{CMY?.YWhole}")]
    public class CyanMagentaYellowKey : IEquatable<CyanMagentaYellowKey>
    {
        /// <summary>
        /// The black key color value [0.0 -> 1.0]
        /// </summary>
        public double K { get; private set; }
        /// <summary>
        /// The black key color value [0 -> 100]
        /// </summary>
        public int KWhole { get; private set; }
        /// <summary>
        /// The Cyan, Magenta, and Yellow color values
        /// </summary>
        public CyanMagentaYellow CMY { get; private set; }

        /// <summary>
        /// Converts this instance of CMYK color to RGB model
        /// </summary>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        public RedGreenBlue ConvertToRgb() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMYK color to HSL model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        public HueSaturationLightness ConvertToHsl() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMYK color to HSV model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationValue"/></returns>
        public HueSaturationValue ConvertToHsv() =>
            new(this);

        /// <summary>
        /// Converts this instance of CMYK color to RYB model
        /// </summary>
        /// <returns>An instance of <see cref="RedYellowBlue"/></returns>
        public RedYellowBlue ConvertToRyb() =>
            new(this);

        /// <summary>
        /// cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;
        /// </summary>
        public override string ToString() =>
            $"cmyk:{CMY.CWhole};{CMY.MWhole};{CMY.YWhole};{KWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as CyanMagentaYellowKey);

        /// <inheritdoc/>
        public bool Equals(CyanMagentaYellowKey other) =>
            other is not null &&
            K == other.K &&
            EqualityComparer<CyanMagentaYellow>.Default.Equals(CMY, other.CMY);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1604861130;
            hashCode = hashCode * -1521134295 + K.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<CyanMagentaYellow>.Default.GetHashCode(CMY);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CyanMagentaYellowKey left, CyanMagentaYellowKey right) =>
            EqualityComparer<CyanMagentaYellowKey>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CyanMagentaYellowKey left, CyanMagentaYellowKey right) =>
            !(left == right);

        /// <summary>
        /// Converts the RGB color model to CMYK
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellowKey(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to CMYK!");

            // Get the level of each color
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            K = key;
            KWhole = (int)(key * 100);
            CMY = cmy;
        }

        /// <summary>
        /// Converts the HSL color model to CMYK
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellowKey(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to CMYK!");

            // Get the level of each color
            var rgb = hsl.ConvertToRgb();
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            K = key;
            KWhole = (int)Math.Round(key * 100);
            CMY = cmy;
        }

        /// <summary>
        /// Converts the CMY color model to CMYK
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellowKey(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to CMYK!");

            // Get the level of each color
            var rgb = cmy.ConvertToRgb();
            var (resCmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            K = key;
            KWhole = (int)Math.Round(key * 100);
            CMY = resCmy;
        }

        /// <summary>
        /// Converts the HSV color model to CMYK
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellowKey(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException("Can't convert a null HSV instance to CMYK!");

            // Get the level of each color
            var rgb = hsv.ConvertToRgb();
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            K = key;
            KWhole = (int)Math.Round(key * 100);
            CMY = cmy;
        }

        /// <summary>
        /// Converts the RYB color model to CMYK
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public CyanMagentaYellowKey(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException("Can't convert a null RYB instance to CMYK!");

            // Get the level of each color
            var rgb = ryb.ConvertToRgb();
            var (cmy, key) = GetCmykFromRgb(rgb);

            // Install the values
            K = key;
            KWhole = (int)Math.Round(key * 100);
            CMY = cmy;
        }

        internal CyanMagentaYellowKey(double k, CyanMagentaYellow cmy)
        {
            K = k;
            KWhole = (int)Math.Round(k * 100);
            CMY = cmy;
        }

        private (CyanMagentaYellow cmy, double k) GetCmykFromRgb(RedGreenBlue rgb)
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
