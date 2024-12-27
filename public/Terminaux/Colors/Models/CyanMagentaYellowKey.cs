//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CMYK class instance
    /// </summary>
    [DebuggerDisplay("Black Key = {KWhole}, CMY = {CMY?.CWhole};{CMY?.MWhole};{CMY?.YWhole}")]
    public class CyanMagentaYellowKey : BaseColorModel, IEquatable<CyanMagentaYellowKey>
    {
        /// <summary>
        /// The black key color value [0.0 -> 1.0]
        /// </summary>
        public double K { get; private set; }
        /// <summary>
        /// The black key color value [0 -> 100]
        /// </summary>
        public int KWhole =>
            (int)Math.Round(K * 100);
        /// <summary>
        /// The Cyan, Magenta, and Yellow color values
        /// </summary>
        public CyanMagentaYellow CMY { get; private set; }

        /// <summary>
        /// cmyk:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;;&lt;K&gt;
        /// </summary>
        public override string ToString() =>
            $"cmyk:{CMY.CWhole};{CMY.MWhole};{CMY.YWhole};{KWhole}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellowKey"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMYK</param>
        /// <returns>An instance of <see cref="CyanMagentaYellowKey"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CMYK color specifier \"{specifier}\". Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(5).Split(';');
            if (specifierArray.Length == 4)
            {
                // We got the CMYK whole values! First, check to see if we need to filter the color for the color-blind
                int c = Convert.ToInt32(specifierArray[0]);
                if (c < 0 || c > 100)
                    throw new TerminauxException($"The cyan color level is out of range (0 -> 100). {c}");
                int m = Convert.ToInt32(specifierArray[1]);
                if (m < 0 || m > 100)
                    throw new TerminauxException($"The magenta color level is out of range (0 -> 100). {m}");
                int y = Convert.ToInt32(specifierArray[2]);
                if (y < 0 || y > 100)
                    throw new TerminauxException($"The yellow color level is out of range (0 -> 100). {y}");
                int k = Convert.ToInt32(specifierArray[3]);
                if (k < 0 || k > 100)
                    throw new TerminauxException($"The black key level is out of range (0 -> 100). {k}");

                // First, we need to convert from CMYK to RGB
                double cPart = (double)c / 100;
                double mPart = (double)m / 100;
                double yPart = (double)y / 100;
                double kPart = (double)k / 100;
                var cmyk = new CyanMagentaYellowKey(kPart, new(cPart, mPart, yPart));
                return cmyk;
            }
            else
                throw new TerminauxException($"Invalid CMY color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cmy:<C>;<M>;<Y>");
        }

        /// <summary>
        /// Does the string specifier represent a valid CMYK specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CMYK specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cmyk:") &&
            (!checkParts || (checkParts && specifier.Substring(5).Split(';').Length == 4));

        /// <summary>
        /// Does the string specifier represent a valid CMYK specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CMYK specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(5).Split(';');
            int c = Convert.ToInt32(specifierArray[0]);
            if (c < 0 || c > 100)
                return false;
            int m = Convert.ToInt32(specifierArray[1]);
            if (m < 0 || m > 100)
                return false;
            int y = Convert.ToInt32(specifierArray[2]);
            if (y < 0 || y > 100)
                return false;
            int k = Convert.ToInt32(specifierArray[3]);
            if (k < 0 || k > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellowKey"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMYK</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var cmyk = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(cmyk);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CyanMagentaYellowKey)obj);

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

        internal CyanMagentaYellowKey(double k, CyanMagentaYellow cmy)
        {
            K = k;
            CMY = cmy;
        }
    }
}
