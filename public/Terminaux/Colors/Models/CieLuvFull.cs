﻿//
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
using Terminaux.Colors.Models.Parsing;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CieLuv class instance
    /// </summary>
    [DebuggerDisplay("CieLuv = {L};{U};{V};{Observer};{(int)Illuminant}")]
    public class CieLuvFull : BaseColorModel, IEquatable<CieLuvFull>
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
        /// The observer [either 2 degs or 10 degs]
        /// </summary>
        public int Observer { get; private set; } = 2;
        /// <summary>
        /// The illuminant
        /// </summary>
        public IlluminantType Illuminant { get; private set; } = IlluminantType.D65;

        /// <summary>
        /// cieluv:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;;&lt;Observer&gt;;&lt;Illuminant&gt;
        /// </summary>
        public override string ToString() =>
            $"cieluv:{L:0.##};{U:0.##};{V:0.##};{Observer};{(int)Illuminant}";

        /// <summary>
        /// Does the string specifier represent a valid CieLuv specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLuv specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cieluv:") &&
            (!checkParts || (checkParts && specifier.Substring(7).Split(';').Length == 5));

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
            int observer = Convert.ToInt32(specifierArray[3]);
            if (observer != 2 && observer != 10)
                return false;
            IlluminantType illuminant = (IlluminantType)Convert.ToInt32(specifierArray[4]);
            if (illuminant < IlluminantType.A || illuminant > IlluminantType.F12)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CieLuvFull"/> converted to <see cref="RedGreenBlue"/>
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
        /// Parses the specifier and returns an instance of <see cref="CieLuvFull"/>
        /// </summary>
        /// <param name="specifier">Specifier of CieLuv</param>
        /// <returns>An instance of <see cref="CieLuvFull"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CieLuvFull ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CieLuv color specifier \"{specifier}\". Ensure that it's on the correct format: cieluv:<red>;<yellow>;<blue>;<observer>;<illuminant>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            if (specifierArray.Length == 5)
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
                int observer = Convert.ToInt32(specifierArray[3]);
                if (observer != 2 && observer != 10)
                    throw new TerminauxException($"Observer must be either 2 or 10. {observer}");
                IlluminantType illuminant = (IlluminantType)Convert.ToInt32(specifierArray[4]);
                if (illuminant < IlluminantType.A || illuminant > IlluminantType.F12)
                    throw new TerminauxException($"Illuminant is invalid. {(int)illuminant}");

                // First, we need to convert from CieLuv to RGB
                var CieLuv = new CieLuvFull(l, u, v, observer, illuminant);
                return CieLuv;
            }
            else
                throw new TerminauxException($"Invalid CieLuv color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cieluv:<red>;<yellow>;<blue>;<observer>;<illuminant>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLuvFull)obj);

        /// <inheritdoc/>
        public bool Equals(CieLuvFull other) =>
            other is not null &&
            L == other.L &&
            U == other.U &&
            V == other.V &&
            Observer == other.Observer &&
            Illuminant == other.Illuminant;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1640533391;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + U.GetHashCode();
            hashCode = hashCode * -1521134295 + V.GetHashCode();
            hashCode = hashCode * -1521134295 + Observer.GetHashCode();
            hashCode = hashCode * -1521134295 + Illuminant.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CieLuvFull left, CieLuvFull right) =>
            EqualityComparer<CieLuvFull>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLuvFull left, CieLuvFull right) =>
            !(left == right);

        internal CieLuvFull(double l, double u, double v, int observer, IlluminantType illuminant)
        {
            L = l;
            U = u;
            V = v;
            Observer = observer;
            Illuminant = illuminant;
        }
    }
}