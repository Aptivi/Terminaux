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
    /// The YXY class instance
    /// </summary>
    [DebuggerDisplay("YXY = {Y1};{X};{Y2}")]
    public class Yxy : BaseColorModel, IEquatable<Yxy>
    {
        /// <summary>
        /// The Y value [0.0 -> 1.0]
        /// </summary>
        public double Y2 { get; private set; }
        /// <summary>
        /// The x value [0.0 -> 1.0]
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// The y value [0.0 -> 1.0]
        /// </summary>
        public double Y1 { get; private set; }

        /// <summary>
        /// yxy:&lt;Y1&gt;;&lt;X&gt;;&lt;Y2&gt;
        /// </summary>
        public override string ToString() =>
            $"yxy:{Y1:0.##};{X:0.##};{Y2:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid YXY specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YXY specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("yxy:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid YXY specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YXY specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int y1 = Convert.ToInt32(specifierArray[0]);
            if (y1 < 0 || y1 > 100)
                return false;
            int x = Convert.ToInt32(specifierArray[1]);
            if (x < 0 || x > 100)
                return false;
            int y2 = Convert.ToInt32(specifierArray[2]);
            if (y2 < 0 || y2 > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="Yxy"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var yxy = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(yxy);
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
        /// Parses the specifier and returns an instance of <see cref="Yxy"/>
        /// </summary>
        /// <param name="specifier">Specifier of YXY</param>
        /// <returns>An instance of <see cref="Yxy"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static Yxy ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid YXY color specifier \"{specifier}\". Ensure that it's on the correct format: yxy:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the YXY whole values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 100)
                    throw new TerminauxException($"The red level is out of range (0 -> 100). {r}");
                int y = Convert.ToInt32(specifierArray[1]);
                if (y < 0 || y > 100)
                    throw new TerminauxException($"The yellow level is out of range (0 -> 100). {y}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 100)
                    throw new TerminauxException($"The blue level is out of range (0 -> 100). {b}");

                // First, we need to convert from YXY to RGB
                var yxy = new Yxy(r, y, b);
                return yxy;
            }
            else
                throw new TerminauxException($"Invalid YXY color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: yxy:<C>;<M>;<Y>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((Yxy)obj);

        /// <inheritdoc/>
        public bool Equals(Yxy other) =>
            other is not null &&
            Y1 == other.Y1 &&
            X == other.X &&
            Y2 == other.Y2;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1299691120;
            hashCode = hashCode * -1521134295 + Y2.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y1.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Yxy left, Yxy right) =>
            EqualityComparer<Yxy>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(Yxy left, Yxy right) =>
            !(left == right);

        internal Yxy(double y1, double x, double y2)
        {
            Y1 = y1;
            X = x;
            Y2 = y2;
        }
    }
}
