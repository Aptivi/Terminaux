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
    /// The HunterLab class instance
    /// </summary>
    [DebuggerDisplay("HunterLab = {L};{A};{B}")]
    public class HunterLab : BaseColorModel, IEquatable<HunterLab>
    {
        /// <summary>
        /// The L value [0.0 -> 100.0]
        /// </summary>
        public double L { get; private set; }
        /// <summary>
        /// The A value [-128.0 -> 127.0]
        /// </summary>
        public double A { get; private set; }
        /// <summary>
        /// The B value [-128.0 -> 127.0]
        /// </summary>
        public double B { get; private set; }

        /// <summary>
        /// hunterlab:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"hunterlab:{L:0.##};{A:0.##};{B:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid HunterLab specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HunterLab specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("hunterlab:") &&
            (!checkParts || (checkParts && specifier.Substring(10).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid HunterLab specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HunterLab specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(10).Split(';');
            double l = Convert.ToDouble(specifierArray[0]);
            if (l < 0 || l > 100)
                return false;
            double a = Convert.ToDouble(specifierArray[1]);
            if (a < -128 || a > 127)
                return false;
            double b = Convert.ToDouble(specifierArray[2]);
            if (b < -128 || b > 127)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HunterLab"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static new RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var HunterLab = ParseSpecifier(specifier);
            var rgb = ConversionTools.ToRgb(HunterLab);
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
        /// Parses the specifier and returns an instance of <see cref="HunterLab"/>
        /// </summary>
        /// <param name="specifier">Specifier of HunterLab</param>
        /// <returns>An instance of <see cref="HunterLab"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static HunterLab ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHUNTERLABSPECIFIER").FormatString(specifier) + ": hunterlab:<l>;<a>;<b>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(10).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the HunterLab whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELABLLEVEL") + $" {l}");
                double a = Convert.ToDouble(specifierArray[1]);
                if (a < -128 || a > 127)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHUNTERLABALEVEL") + $" {a}");
                double b = Convert.ToDouble(specifierArray[2]);
                if (b < -128 || b > 127)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHUNTERLABBLEVEL") + $" {b}");

                // First, we need to convert from HunterLab to RGB
                var HunterLab = new HunterLab(l, a, b);
                return HunterLab;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHUNTERLABSPECIFIEREXCEED").FormatString(specifier) + ": hunterlab:<l>;<a>;<b>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((HunterLab)obj);

        /// <inheritdoc/>
        public bool Equals(HunterLab other) =>
            other is not null &&
            L == other.L &&
            A == other.A &&
            B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 921976076;
            hashCode = hashCode * -1521134295 + L.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HunterLab left, HunterLab right) =>
            EqualityComparer<HunterLab>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HunterLab left, HunterLab right) =>
            !(left == right);

        internal HunterLab(double l, double a, double b)
        {
            L = l;
            A = a;
            B = b;
        }
    }
}
