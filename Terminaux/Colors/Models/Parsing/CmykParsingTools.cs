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
    /// Tools to parse CMYK specifiers
    /// </summary>
    public static class CmykParsingTools
    {
        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellowKey"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMYK</param>
        /// <returns>An instance of <see cref="CyanMagentaYellowKey"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ParseSpecifier(string specifier)
        {
            if (!specifier.Contains(";") || !specifier.StartsWith("cmyk:"))
                throw new TerminauxException($"Invalid CMYK color specifier \"{specifier}\". Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(5).Split(';');
            if (specifierArray.Length == 4)
            {
                // We got the CMYK whole values! First, check to see if we need to filter the color for the color-blind
                int c = Convert.ToInt32(specifierArray[0]);
                if (c < 0 || c > 100)
                    throw new TerminauxException($"The cyan color level is out of range (0 -> 100). {c}");
                int m = Convert.ToInt32(specifierArray[1]);
                if (m < 0 || m > 100)
                    throw new TerminauxException($"The magenta color level is out of range (0 -> 100). {m}");
                int y = Convert.ToInt32(specifierArray[2]);
                if (y < 0 || y > 100)
                    throw new TerminauxException($"The yellow color level is out of range (0 -> 100). {y}");
                int k = Convert.ToInt32(specifierArray[3]);
                if (k < 0 || k > 100)
                    throw new TerminauxException($"The black key level is out of range (0 -> 100). {k}");

                // First, we need to convert from CMYK to RGB
                double cPart = (double)c / 100;
                double mPart = (double)m / 100;
                double yPart = (double)y / 100;
                double kPart = (double)k / 100;
                var cmyk = new CyanMagentaYellowKey(kPart, new(cPart, mPart, yPart));
                return cmyk;
            }
            else
                throw new TerminauxException($"Invalid CMY color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: cmy:<C>;<M>;<Y>");
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="CyanMagentaYellowKey"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of CMYK</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier)
        {
            var cmyk = ParseSpecifier(specifier);
            var rgb = RgbConversionTools.ConvertFrom(cmyk);
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
