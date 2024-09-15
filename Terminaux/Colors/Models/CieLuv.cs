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
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CieLuv class instance (Observer = 2 degs, Illuminant = D65)
    /// </summary>
    [DebuggerDisplay("CieLuv = {L};{U};{V}")]
    public class CieLuv : BaseColorModel, IEquatable<CieLuv>
    {
        /// <summary>
        /// The L value [0.0 -> 100.0]
        /// </summary>
        public double L { get; private set; }
        /// <summary>
        /// The U value [-134.0 -> 220.0]
        /// </summary>
        public double U { get; private set; }
        /// <summary>
        /// The V value [-140.0 -> 122.0]
        /// </summary>
        public double V { get; private set; }

        /// <summary>
        /// cieluv:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"cieluv:{L:0.##};{U:0.##};{V:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid CieLuv specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLuv specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cieluv:") &&
            (!checkParts || (checkParts && specifier.Substring(7).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid CieLuv specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLuv specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(7).Split(';');
            double l = Convert.ToDouble(specifierArray[0]);
            if (l < 0 || l > 100)
                return false;
            double u = Convert.ToDouble(specifierArray[1]);
            if (u < -134 || u > 220)
                return false;
            double v = Convert.ToDouble(specifierArray[2]);
            if (v < -140 || v > 122)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CieLuv"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var CieLuv = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(CieLuv);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CieLuv"/>
        /// </summary>
        /// <param name="specifier">Specifier of CieLuv</param>
        /// <returns>An instance of <see cref="CieLuv"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CieLuv ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CieLuv color specifier \"{specifier}\". Ensure that it's on the correct format: cieluv:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the CieLuv whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException($"The L value is out of range (0.0 -> 100.0). {l}");
                double u = Convert.ToDouble(specifierArray[1]);
                if (u < -134 || u > 220)
                    throw new TerminauxException($"The U value is out of range (-134.0 -> 220.0). {u}");
                double v = Convert.ToDouble(specifierArray[2]);
                if (v < -140 || v > 122)
                    throw new TerminauxException($"The V value is out of range (-140.0 -> 122.0). {v}");

                // First, we need to convert from CieLuv to RGB
                var CieLuv = new CieLuv(l, u, v);
                return CieLuv;
            }
            else
                throw new TerminauxException($"Invalid CieLuv color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cieluv:<C>;<M>;<Y>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLuv)obj);

        /// <inheritdoc/>
        public bool Equals(CieLuv other) =>
            other is not null &&
            L == other.L &&
            U == other.U &&
            V == other.V;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 921976076;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + U.GetHashCode();
            hashCode = hashCode * -1521134295 + V.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CieLuv left, CieLuv right) =>
            EqualityComparer<CieLuv>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLuv left, CieLuv right) =>
            !(left == right);

        internal CieLuv(double l, double a, double b)
        {
            L = l;
            U = a;
            V = b;
        }
    }
}
