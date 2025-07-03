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
using Textify.General;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The CieLab class instance (Observer = 2 degs, Illuminant = D65)
    /// </summary>
    [DebuggerDisplay("CieLab = {L};{A};{B}")]
    public class CieLab : CieLabFull, IEquatable<CieLab>
    {
        /// <summary>
        /// cielab:&lt;L&gt;;&lt;A&gt;;&lt;B&gt;
        /// </summary>
        public override string ToString() =>
            $"cielab:{L:0.##};{A:0.##};{B:0.##}";

        /// <summary>
        /// Does the string specifier represent a valid CieLab specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid CieLab specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static new bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("cielab:") &&
            (!checkParts || (checkParts && specifier.Substring(7).Split(';').Length == 3));

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
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CieLab"/> converted to <see cref="RedGreenBlue"/>
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
        /// Parses the specifier and returns an instance of <see cref="CieLab"/>
        /// </summary>
        /// <param name="specifier">Specifier of CieLab</param>
        /// <returns>An instance of <see cref="CieLab"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public new static CieLab ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDCIELABSPECIFIER").FormatString(specifier) + ": cielab:<l>;<a>;<b>;<observer>;<illuminant>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(7).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the CieLab whole values! First, check to see if we need to filter the color for the color-blind
                double l = Convert.ToDouble(specifierArray[0]);
                if (l < 0 || l > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELABLLEVEL") + $" {l}");
                double a = Convert.ToDouble(specifierArray[1]);
                if (a < -128 || a > 128)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELABALEVEL") + $" {a}");
                double b = Convert.ToDouble(specifierArray[2]);
                if (b < -128 || b > 128)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSECIELABBLEVEL") + $" {b}");

                // First, we need to convert from CieLab to RGB
                var CieLab = new CieLab(l, a, b);
                return CieLab;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDCIELABSPECIFIEREXCEED").FormatString(specifier) + ": cielab:<l>;<a>;<b>;<observer>;<illuminant>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((CieLab)obj);

        /// <inheritdoc/>
        public bool Equals(CieLab other) =>
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
        public static bool operator ==(CieLab left, CieLab right) =>
            EqualityComparer<CieLab>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(CieLab left, CieLab right) =>
            !(left == right);

        internal CieLab(double l, double a, double b) :
            base(l, a, b, 2, IlluminantType.D65)
        { }
    }
}
