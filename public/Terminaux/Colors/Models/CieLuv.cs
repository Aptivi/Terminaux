//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.Text;
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Models.Parsing;
using Terminaux.Colors.Transformation;
using Textify.General;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CieLuv class instance
    /// </summary>
    [DebuggerDisplay("CieLuv = {L};{U};{V};{Observer};{(int)Illuminant}")]
    public class CieLuv : BaseColorModel, IEquatable<CieLuv>
    {
        private bool implicitlySpecced = false;

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
        /// cieluv:&lt;L&gt;;&lt;U&gt;;&lt;V&gt;;&lt;Observer&gt;;&lt;Illuminant&gt;
        /// </summary>
        /// <remarks>If the observer and the illuminant values were omitted when parsing this model, they are omitted in the resulting string.</remarks>
        public override string ToString()
        {
            var modelBuilder = new StringBuilder($"cieluv:{L:0.##};{U:0.##};{V:0.##}");
            if (!implicitlySpecced)
                modelBuilder.Append($";{Observer};{(int)Illuminant}");
            return modelBuilder.ToString();
        }

        /// <summary>
        /// Does the string specifier represent a valid CieLuv specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLuv specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false)
        {
            if (!specifier.Contains(";") || !specifier.StartsWith("cieluv:"))
                return false;
            if (!checkParts)
                return true;

            // Check the part count now
            int partLength = specifier.Substring(7).Split(';').Length;
            return partLength == 5 || partLength == 3;
        }

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
            if (specifierArray.Length == 5)
            {
                int observer = Convert.ToInt32(specifierArray[3]);
                if (observer != 2 && observer != 10)
                    return false;
                IlluminantType illuminant = (IlluminantType)Convert.ToInt32(specifierArray[4]);
                if (illuminant < IlluminantType.A || illuminant > IlluminantType.F12)
                    return false;
            }
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDCIELUVSPECIFIER").FormatString(specifier) + ": cieluv:<l>;<u>;<v>;<observer>;<illuminant>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            bool implicitlySpecced = specifierArray.Length == 3;
            if (specifierArray.Length == 5 || implicitlySpecced)
            {
                // We got the CieLuv whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELABLLEVEL") + $" {l}");
                double u = Convert.ToDouble(specifierArray[1]);
                if (u < -134 || u > 220)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELUVULEVEL") + $" {u}");
                double v = Convert.ToDouble(specifierArray[2]);
                if (v < -140 || v > 122)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELUVVLEVEL") + $" {v}");

                // Get the observer and the illuminant when needed
                int observer = 2;
                IlluminantType illuminant = IlluminantType.D65;
                if (!implicitlySpecced)
                {
                    // We've explicitly specified the observer and the illuminant
                    observer = Convert.ToInt32(specifierArray[3]);
                    if (observer != 2 && observer != 10)
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_ILLUMINANCEINVALIDOBSERVER") + $": {observer}");
                    illuminant = (IlluminantType)Convert.ToInt32(specifierArray[4]);
                    if (illuminant < IlluminantType.A || illuminant > IlluminantType.F12)
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_ILLUMINANCEINVALIDILLUMINANT") + $": {(int)illuminant}");
                }

                // Finally, return the CieLuv instance
                var CieLuv = new CieLuv(l, u, v, observer, illuminant) { implicitlySpecced = implicitlySpecced };
                return CieLuv;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDCIELUVSPECIFIEREXCEED").FormatString(specifier) + ": cieluv:<l>;<u>;<v>;<observer>;<illuminant>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLuv)obj);

        /// <inheritdoc/>
        public bool Equals(CieLuv other) =>
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
        public static bool operator ==(CieLuv left, CieLuv right) =>
            EqualityComparer<CieLuv>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLuv left, CieLuv right) =>
            !(left == right);

        internal CieLuv(double l, double u, double v, int observer, IlluminantType illuminant)
        {
            L = l;
            U = u;
            V = v;
            Observer = observer;
            Illuminant = illuminant;
        }
    }
}
