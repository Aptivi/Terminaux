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
    /// The CieLab class instance
    /// </summary>
    [DebuggerDisplay("CieLab = {L};{A};{B};{Observer};{(int)Illuminant}")]
    public class CieLabFull : BaseColorModel, IEquatable<CieLabFull>
    {
        /// <summary>
        /// The L value [0.0 -> 100.0]
        /// </summary>
        public double L { get; private set; }
        /// <summary>
        /// The A value [-128.0 -> 128.0]
        /// </summary>
        public double A { get; private set; }
        /// <summary>
        /// The B value [-128.0 -> 128.0]
        /// </summary>
        public double B { get; private set; }
        /// <summary>
        /// The observer [either 2 degs or 10 degs]
        /// </summary>
        public int Observer { get; private set; } = 2;
        /// <summary>
        /// The illuminant
        /// </summary>
        public IlluminantType Illuminant { get; private set; } = IlluminantType.D65;

        /// <summary>
        /// cielab:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;;&lt;Observer&gt;;&lt;Illuminant&gt;
        /// </summary>
        public override string ToString() =>
            $"cielab:{L:0.##};{A:0.##};{B:0.##};{Observer};{(int)Illuminant}";

        /// <summary>
        /// Does the string specifier represent a valid CieLab specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLab specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cielab:") &&
            (!checkParts || (checkParts && specifier.Substring(7).Split(';').Length == 5));

        /// <summary>
        /// Does the string specifier represent a valid CieLab specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLab specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(7).Split(';');
            double l = Convert.ToDouble(specifierArray[0]);
            if (l < 0 || l > 100)
                return false;
            double a = Convert.ToDouble(specifierArray[1]);
            if (a < -128 || a > 128)
                return false;
            double b = Convert.ToDouble(specifierArray[2]);
            if (b < -128 || b > 128)
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
        /// Parses the specifier and returns an instance of <see cref="CieLabFull"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var CieLab = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(CieLab);
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
        /// Parses the specifier and returns an instance of <see cref="CieLabFull"/>
        /// </summary>
        /// <param name="specifier">Specifier of CieLab</param>
        /// <returns>An instance of <see cref="CieLab"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CieLabFull ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CieLab color specifier \"{specifier}\". Ensure that it's on the correct format: cielab:<red>;<yellow>;<blue>;<observer>;<illuminant>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            if (specifierArray.Length == 5)
            {
                // We got the CieLab whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException($"The L value is out of range (0.0 -> 100.0). {l}");
                double a = Convert.ToDouble(specifierArray[1]);
                if (a < -128 || a > 128)
                    throw new TerminauxException($"The A value is out of range (-128.0 -> 128.0). {a}");
                double b = Convert.ToDouble(specifierArray[2]);
                if (b < -128 || b > 128)
                    throw new TerminauxException($"The B value is out of range (-128.0 -> 128.0). {b}");
                int observer = Convert.ToInt32(specifierArray[3]);
                if (observer != 2 && observer != 10)
                    throw new TerminauxException($"Observer must be either 2 or 10. {observer}");
                IlluminantType illuminant = (IlluminantType)Convert.ToInt32(specifierArray[4]);
                if (illuminant < IlluminantType.A || illuminant > IlluminantType.F12)
                    throw new TerminauxException($"Illuminant is invalid. {(int)illuminant}");

                // First, we need to convert from CieLab to RGB
                var CieLab = new CieLabFull(l, a, b, observer, illuminant);
                return CieLab;
            }
            else
                throw new TerminauxException($"Invalid CieLab color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cielab:<red>;<yellow>;<blue>;<observer>;<illuminant>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLabFull)obj);

        /// <inheritdoc/>
        public bool Equals(CieLabFull other) =>
            other is not null &&
            L == other.L &&
            A == other.A &&
            B == other.B &&
            Observer == other.Observer &&
            Illuminant == other.Illuminant;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1929569655;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + Observer.GetHashCode();
            hashCode = hashCode * -1521134295 + Illuminant.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CieLabFull left, CieLabFull right) =>
            EqualityComparer<CieLabFull>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLabFull left, CieLabFull right) =>
            !(left == right);

        internal CieLabFull(double l, double a, double b, int observer, IlluminantType illuminant)
        {
            L = l;
            A = a;
            B = b;
            Observer = observer;
            Illuminant = illuminant;
        }
    }
}
