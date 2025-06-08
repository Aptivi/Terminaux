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
using Textify.General;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The RYB class instance
    /// </summary>
    [DebuggerDisplay("RYB = {R};{Y};{B}")]
    public class RedYellowBlue : BaseColorModel, IEquatable<RedYellowBlue>
    {
        /// <summary>
        /// The red color value [0 -> 255]
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The yellow color value [0 -> 255]
        /// </summary>
        public int Y { get; private set; }
        /// <summary>
        /// The blue color value [0 -> 255]
        /// </summary>
        public int B { get; private set; }
        /// <summary>
        /// The red color value [0.0 -> 0.1]
        /// </summary>
        public double RNormalized =>
            R / 255d;
        /// <summary>
        /// The yellow color value [0.0 -> 0.1]
        /// </summary>
        public double YNormalized =>
            Y / 255d;
        /// <summary>
        /// The blue color value [0.0 -> 0.1]
        /// </summary>
        public double BNormalized =>
            B / 255d;

        /// <summary>
        /// ryb:&lt;R&gt;;&lt;Y&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"ryb:{R};{Y};{B}";

        /// <summary>
        /// Does the string specifier represent a valid RYB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RYB specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("ryb:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid RYB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RYB specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int r = Convert.ToInt32(specifierArray[0]);
            if (r < 0 || r > 255)
                return false;
            int y = Convert.ToInt32(specifierArray[1]);
            if (y < 0 || y > 255)
                return false;
            int b = Convert.ToInt32(specifierArray[2]);
            if (b < 0 || b > 255)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedYellowBlue"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var ryb = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(ryb);
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
        /// Parses the specifier and returns an instance of <see cref="RedYellowBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RYB</param>
        /// <returns>An instance of <see cref="RedYellowBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException("Invalid RYB color specifier \"{0}\". Ensure that it's on the correct format".FormatString(specifier) + ": ryb:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the RYB whole values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 255)
                    throw new TerminauxException("The red level is out of range (0 -> 255)." + $" {r}");
                int y = Convert.ToInt32(specifierArray[1]);
                if (y < 0 || y > 255)
                    throw new TerminauxException("The yellow level is out of range (0 -> 255)." + $" {y}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 255)
                    throw new TerminauxException("The blue level is out of range (0 -> 255)." + $" {b}");

                // First, we need to convert from RYB to RGB
                var ryb = new RedYellowBlue(r, y, b);
                return ryb;
            }
            else
                throw new TerminauxException("Invalid RYB color specifier \"{0}\". The specifier may not be more than three elements. Ensure that it's on the correct format".FormatString(specifier) + ": ryb:<red>;<yellow>;<blue>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((RedYellowBlue)obj);

        /// <inheritdoc/>
        public bool Equals(RedYellowBlue other) =>
            other is not null &&
                   R == other.R &&
                   Y == other.Y &&
                   B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -636965442;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(RedYellowBlue left, RedYellowBlue right) =>
            EqualityComparer<RedYellowBlue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(RedYellowBlue left, RedYellowBlue right) =>
            !(left == right);

        internal RedYellowBlue(int r, int y, int b)
        {
            R = r;
            Y = y;
            B = b;
        }
    }
}
