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
    /// The CMY class instance
    /// </summary>
    [DebuggerDisplay("CMY = {CWhole};{MWhole};{YWhole}")]
    public class CyanMagentaYellow : BaseColorModel, IEquatable<CyanMagentaYellow>
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
        public int CWhole =>
            (int)(C * 100);
        /// <summary>
        /// The magenta color value [0 -> 100]
        /// </summary>
        public int MWhole =>
            (int)(M * 100);
        /// <summary>
        /// The yellow color value [0 -> 100]
        /// </summary>
        public int YWhole =>
            (int)(Y * 100);

        /// <summary>
        /// cmy:&lt;C&gt;;&lt;M&gt;;&lt;Y&gt;
        /// </summary>
        public override string ToString() =>
            $"cmy:{CWhole};{MWhole};{YWhole}";

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellow"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMY</param>
        /// <returns>An instance of <see cref="CyanMagentaYellow"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid CMY color specifier \"{specifier}\". Ensure that it's on the correct format: cmy:<C>;<M>;<Y>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the CMY whole values! First, check to see if we need to filter the color for the color-blind
                int c = Convert.ToInt32(specifierArray[0]);
                if (c < 0 || c > 100)
                    throw new TerminauxException($"The cyan color level is out of range (0 -> 100). {c}");
                int m = Convert.ToInt32(specifierArray[1]);
                if (m < 0 || m > 100)
                    throw new TerminauxException($"The magenta color level is out of range (0 -> 100). {m}");
                int y = Convert.ToInt32(specifierArray[2]);
                if (y < 0 || y > 100)
                    throw new TerminauxException($"The yellow color level is out of range (0 -> 100). {y}");

                // First, we need to convert from CMY to RGB
                double cPart = (double)c / 100;
                double mPart = (double)m / 100;
                double yPart = (double)y / 100;
                var cmy = new CyanMagentaYellow(cPart, mPart, yPart);
                return cmy;
            }
            else
                throw new TerminauxException($"Invalid CMY color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cmy:<C>;<M>;<Y>");
        }

        /// <summary>
        /// Does the string specifier represent a valid CMY specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CMY specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cmy:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid CMY specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CMY specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int c = Convert.ToInt32(specifierArray[0]);
            if (c < 0 || c > 100)
                return false;
            int m = Convert.ToInt32(specifierArray[1]);
            if (m < 0 || m > 100)
                return false;
            int y = Convert.ToInt32(specifierArray[2]);
            if (y < 0 || y > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellow"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMY</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var cmy = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(cmy);
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
            Equals((CyanMagentaYellow)obj);

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

        internal CyanMagentaYellow(double c, double m, double y)
        {
            C = c;
            M = m;
            Y = y;
        }
    }
}
