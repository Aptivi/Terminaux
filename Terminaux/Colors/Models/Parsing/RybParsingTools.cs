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
    /// Tools to parse RYB specifiers
    /// </summary>
    public static class RybParsingTools
    {
        /// <summary>
        /// Does the string specifier represent a valid RYB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RYB specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("ryb:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid RYB specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RYB specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValid(string specifier)
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
        /// Parses the specifier and returns an instance of <see cref="RedYellowBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RYB</param>
        /// <returns>An instance of <see cref="RedYellowBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid RYB color specifier \"{specifier}\". Ensure that it's on the correct format: ryb:<red>;<yellow>;<blue>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the RYB whole values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 255)
                    throw new TerminauxException($"The red level is out of range (0 -> 255). {r}");
                int y = Convert.ToInt32(specifierArray[1]);
                if (y < 0 || y > 255)
                    throw new TerminauxException($"The yellow level is out of range (0 -> 255). {y}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 255)
                    throw new TerminauxException($"The blue level is out of range (0 -> 255). {b}");

                // First, we need to convert from RYB to RGB
                var ryb = new RedYellowBlue(r, y, b);
                return ryb;
            }
            else
                throw new TerminauxException($"Invalid RYB color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: ryb:<C>;<M>;<Y>");
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedYellowBlue"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier)
        {
            var ryb = ParseSpecifier(specifier);
            var rgb = RgbConversionTools.ConvertFrom(ryb);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }
    }
}
