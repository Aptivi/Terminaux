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
using Terminaux.Colors.Models.Parsing;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CieLch class instance (Observer = 2 degs, Illuminant = D65)
    /// </summary>
    [DebuggerDisplay("CieLch = {L};{C};{H}")]
    public class CieLch : CieLchFull, IEquatable<CieLch>
    {
        /// <summary>
        /// cielch:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"cielch:{L:0.##};{C:0.##};{H:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid CieLch specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLch specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cielch:") &&
            (!checkParts || (checkParts && specifier.Substring(7).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid CieLch specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLch specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(7).Split(';');
            double l = Convert.ToDouble(specifierArray[0]);
            if (l < 0 || l > 100)
                return false;
            double c = Convert.ToDouble(specifierArray[1]);
            if (c < 0 || c > 131)
                return false;
            double h = Convert.ToDouble(specifierArray[2]);
            if (h < 0 || h > 230)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CieLch"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var CieLch = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(CieLch);
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
        /// Parses the specifier and returns an instance of <see cref="CieLch"/>
        /// </summary>
        /// <param name="specifier">Specifier of CieLch</param>
        /// <returns>An instance of <see cref="CieLch"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public new static CieLch ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CieLch color specifier \"{specifier}\". Ensure that it's on the correct format: cielch:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the CieLch whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException($"The L value is out of range (0.0 -> 100.0). {l}");
                double c = Convert.ToDouble(specifierArray[1]);
                if (c < 0 || c > 131)
                    throw new TerminauxException($"The C value is out of range (-128.0 -> 128.0). {c}");
                double h = Convert.ToDouble(specifierArray[2]);
                if (h < 0 || h > 230)
                    throw new TerminauxException($"The H value is out of range (-128.0 -> 128.0). {h}");

                // First, we need to convert from CieLch to RGB
                var CieLch = new CieLch(l, c, h);
                return CieLch;
            }
            else
                throw new TerminauxException($"Invalid CieLch color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cielch:<red>;<yellow>;<blue>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLch)obj);

        /// <inheritdoc/>
        public bool Equals(CieLch other) =>
            other is not null &&
            L == other.L &&
            C == other.C &&
            H == other.H;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 429811092;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(CieLch left, CieLch right) =>
            EqualityComparer<CieLch>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLch left, CieLch right) =>
            !(left == right);

        internal CieLch(double l, double c, double h) :
            base(l, c, h, 2, IlluminantType.D65)
        { }
    }
}
