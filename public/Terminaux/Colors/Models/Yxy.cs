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
    /// The YXY class instance
    /// </summary>
    [DebuggerDisplay("YXY = {Y2};{X};{Y1}")]
    public class Yxy : BaseColorModel, IEquatable<Yxy>
    {
        /// <summary>
        /// The Y value [0.0 -> 100.0]
        /// </summary>
        public double Y1 { get; private set; }
        /// <summary>
        /// The x value [0.0 -> 1.0]
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// The y value [0.0 -> 1.0]
        /// </summary>
        public double Y2 { get; private set; }

        /// <summary>
        /// yxy:&lt;Y1&gt;;&lt;X&gt;;&lt;Y2&gt;
        /// </summary>
        public override string ToString() =>
            $"yxy:{Y2:0.##};{X:0.##};{Y1:0.##}";

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
            if (x < 0 || x > 1)
                return false;
            int y2 = Convert.ToInt32(specifierArray[2]);
            if (y2 < 0 || y2 > 1)
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDYXYSPECIFIER").FormatString(specifier) + ": yxy:<y>;<x>;<y>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the YXY whole values! First, check to see if we need to filter the color for the color-blind
                double y1 = Convert.ToDouble(specifierArray[0]);
                if (y1 < 0 || y1 > 100)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYXYY1LEVEL") + $" {y1}");
                double x = Convert.ToDouble(specifierArray[1]);
                if (x < 0 || x > 1)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYXYX1LEVEL") + $" {x}");
                double y2 = Convert.ToDouble(specifierArray[2]);
                if (y2 < 0 || y2 > 1)
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEYXYY2LEVEL") + $" {y2}");

                // First, we need to convert from YXY to RGB
                var yxy = new Yxy(y1, x, y2);
                return yxy;
            }
            else
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDYXYSPECIFIEREXCEED").FormatString(specifier) + ": yxy:<y>;<x>;<y>");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals((Yxy)obj);

        /// <inheritdoc/>
        public bool Equals(Yxy other) =>
            other is not null &&
            Y2 == other.Y2 &&
            X == other.X &&
            Y1 == other.Y1;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1299691120;
            hashCode = hashCode * -1521134295 + Y1.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y2.GetHashCode();
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
            Y2 = y1;
            X = x;
            Y1 = y2;
        }
    }
}
