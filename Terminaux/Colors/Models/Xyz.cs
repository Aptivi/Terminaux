﻿//
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
    /// The XYZ class instance
    /// </summary>
    [DebuggerDisplay("XYZ = {X};{Y};{Z}")]
    public class Xyz : BaseColorModel, IEquatable<Xyz>
    {
        /// <summary>
        /// The X value [0.0 -> 1.0]
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// The Y value [0.0 -> 1.0]
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// The Z value [0.0 -> 1.0]
        /// </summary>
        public double Z { get; private set; }

        /// <summary>
        /// xyz:&lt;X&gt;;&lt;Y&gt;;&lt;Z&gt;
        /// </summary>
        public override string ToString() =>
            $"xyz:{X:0.##};{Y:0.##};{Z:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid XYZ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid XYZ specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("xyz:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid XYZ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid XYZ specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int r = Convert.ToInt32(specifierArray[0]);
            if (r < 0 || r > 100)
                return false;
            int y = Convert.ToInt32(specifierArray[1]);
            if (y < 0 || y > 100)
                return false;
            int b = Convert.ToInt32(specifierArray[2]);
            if (b < 0 || b > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="Xyz"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var xyz = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(xyz);
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
        /// Parses the specifier and returns an instance of <see cref="Xyz"/>
        /// </summary>
        /// <param name="specifier">Specifier of XYZ</param>
        /// <returns>An instance of <see cref="Xyz"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static Xyz ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid XYZ color specifier \"{specifier}\". Ensure that it's on the correct format: xyz:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the XYZ whole values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 100)
                    throw new TerminauxException($"The red level is out of range (0 -> 100). {r}");
                int y = Convert.ToInt32(specifierArray[1]);
                if (y < 0 || y > 100)
                    throw new TerminauxException($"The yellow level is out of range (0 -> 100). {y}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 100)
                    throw new TerminauxException($"The blue level is out of range (0 -> 100). {b}");

                // First, we need to convert from XYZ to RGB
                var xyz = new Xyz(r, y, b);
                return xyz;
            }
            else
                throw new TerminauxException($"Invalid XYZ color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: xyz:<C>;<M>;<Y>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((Xyz)obj);

        /// <inheritdoc/>
        public bool Equals(Xyz other) =>
            other is not null &&
                   X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Xyz left, Xyz right) =>
            EqualityComparer<Xyz>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(Xyz left, Xyz right) =>
            !(left == right);

        internal Xyz(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
