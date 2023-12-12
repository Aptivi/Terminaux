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
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Tools to parse RGB specifiers
    /// </summary>
    public static class RgbParsingTools
    {
        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier)
        {
            if (!specifier.Contains(";"))
                throw new TerminauxException($"Invalid color specifier \"{specifier}\". Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the RGB values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 255)
                    throw new TerminauxException($"The red color level is out of range (0 -> 255). {r}");
                int g = Convert.ToInt32(specifierArray[1]);
                if (g < 0 || g > 255)
                    throw new TerminauxException($"The green color level is out of range (0 -> 255). {g}");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 255)
                    throw new TerminauxException($"The blue color level is out of range (0 -> 255). {b}");

                // Now, transform
                var finalRgb = TransformationTools.GetTransformedColor(r, g, b);

                // Make a new RGB class
                return new(finalRgb.r, finalRgb.g, finalRgb.b);
            }
            else
                throw new TerminauxException($"Invalid RGB color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: rgb:<C>;<M>;<Y>");
        }
    }
}
