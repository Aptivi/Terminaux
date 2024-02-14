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
using Terminaux.Base;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Tools to parse YIQ specifiers
    /// </summary>
    public static class YiqParsingTools
    {
        /// <summary>
        /// Does the string specifier represent a valid YIQ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YIQ specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("yiq:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid YIQ specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid YIQ specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int y = Convert.ToInt32(specifierArray[0]);
            if (y < 0 || y > 255)
                return false;
            int i = Convert.ToInt32(specifierArray[1]);
            if (i < 0 || i > 255)
                return false;
            int q = Convert.ToInt32(specifierArray[2]);
            if (q < 0 || q > 255)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaInPhaseQuadrature"/>
        /// </summary>
        /// <param name="specifier">Specifier of YIQ</param>
        /// <returns>An instance of <see cref="LumaInPhaseQuadrature"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid YIQ color specifier \"{specifier}\". Ensure that it's on the correct format: yiq:<Y>;<I>;<Q>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the YIQ whole values! First, check to see if we need to filter the color for the color-blind
                int y = Convert.ToInt32(specifierArray[0]);
                if (y < 0 || y > 255)
                    throw new TerminauxException($"The luma level is out of range (0 -> 255). {y}");
                int i = Convert.ToInt32(specifierArray[1]);
                if (i < 0 || i > 255)
                    throw new TerminauxException($"The in-phase level is out of range (0 -> 255). {i}");
                int q = Convert.ToInt32(specifierArray[2]);
                if (q < 0 || q > 255)
                    throw new TerminauxException($"The quadrature level is out of range (0 -> 255). {q}");

                // First, we need to convert from YIQ to RGB
                var yiq = new LumaInPhaseQuadrature(y, i, q);
                return yiq;
            }
            else
                throw new TerminauxException($"Invalid YIQ color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: yiq:<Y>;<I>;<Q>");
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="LumaInPhaseQuadrature"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var yiq = ParseSpecifier(specifier);
            var rgb = RgbConversionTools.ConvertFrom(yiq);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings = settings is null ? ColorTools.GlobalSettings : settings;
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }
    }
}
