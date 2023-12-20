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
using System.Globalization;
using Terminaux.Base;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Tools to parse RGB specifiers for general use
    /// </summary>
    public static class ParsingTools
    {
        /// <summary>
        /// Does the string specifier represent a valid model-agnostic specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid model-agnostic specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier) =>
            specifier.Contains(";");

        /// <summary>
        /// Does the string specifier represent either a color name taken from <see cref="ConsoleColors"/> or a color number from 0 to 255?
        /// </summary>
        /// <param name="specifier">Specifier that represents either a color name taken from <see cref="ConsoleColors"/> or a color number from 0 to 255</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierConsoleColors(string specifier)
        {
            if (double.TryParse(specifier, out double specifierNum))
                return specifierNum >= 0 && specifierNum <= 255;
            return Enum.IsDefined(typeof(ConsoleColors), specifier);
        }

        /// <summary>
        /// Does the string specifier represent a valid RGB hash (#RGB or #RRGGBB) as in HTML?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RGB hash (#RGB or #RRGGBB) as in HTML</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValidRgbHash(string specifier)
        {
            if (!specifier.StartsWith("#"))
                return false;
            string finalSpecifier = specifier.Substring(1);
            return
                finalSpecifier.Length == 3 ||
                finalSpecifier.Length == 6;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifier(string specifier)
        {
            var rgb =
                    specifier.StartsWith("cmyk:") ? CmykParsingTools.ParseSpecifierToRgb(specifier) :
                    specifier.StartsWith("cmy:") ? CmyParsingTools.ParseSpecifierToRgb(specifier) :
                    specifier.StartsWith("hsl:") ? HslParsingTools.ParseSpecifierToRgb(specifier) :
                    specifier.StartsWith("hsv:") ? HsvParsingTools.ParseSpecifierToRgb(specifier) :
                    specifier.StartsWith("ryb:") ? RybParsingTools.ParseSpecifierToRgb(specifier) :
                    RgbParsingTools.ParseSpecifierToRgb(specifier);
            return rgb;
        }

        /// <summary>
        /// Parses the specifier that holds the color name and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Color name defined in <see cref="ConsoleColors"/></param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static (RedGreenBlue rgb, ConsoleColorsInfo cci) ParseSpecifierRgbName(string specifier)
        {
            if (!IsSpecifierConsoleColors(specifier))
                throw new TerminauxException($"Invalid color specifier \"{specifier}\". Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Form the sequences using the information from the color details
            var parsedEnum = (ConsoleColors)Enum.Parse(typeof(ConsoleColors), specifier);
            var ColorsInfo = new ConsoleColorsInfo(parsedEnum);

            // Check to see if we need to transform color. Else, be sane.
            int r = Convert.ToInt32(ColorsInfo.R);
            if (r < 0 || r > 255)
                throw new TerminauxException($"The red color level is out of range (0 -> 255). {r}");
            int g = Convert.ToInt32(ColorsInfo.G);
            if (g < 0 || g > 255)
                throw new TerminauxException($"The green color level is out of range (0 -> 255). {g}");
            int b = Convert.ToInt32(ColorsInfo.B);
            if (b < 0 || b > 255)
                throw new TerminauxException($"The blue color level is out of range (0 -> 255). {b}");

            // Now, transform
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b);

            // Make a new RGB class
            return (new(finalRgb.r, finalRgb.g, finalRgb.b), ColorsInfo);
        }

        /// <summary>
        /// Parses the hex representation of RGB and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB in hex representation</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierRgbHash(string specifier)
        {
            if (!IsSpecifierValidRgbHash(specifier))
                throw new TerminauxException($"Invalid color hex specifier \"{specifier}\". This specifier must start with the hash tag. Ensure that it's on the correct format: #RRGGBB");

            // Get the integral value of the total color
            string finalSpecifier = specifier.Substring(1);
            if (finalSpecifier.Length == 3)
            {
                char first = finalSpecifier[0];
                char second = finalSpecifier[1];
                char third = finalSpecifier[2];
                finalSpecifier = $"{first}{first}{second}{second}{third}{third}";
            }
            else if (finalSpecifier.Length != 6)
                throw new TerminauxException($"Invalid color hex length \"{specifier}\". Ensure that it's on the correct format: #RRGGBB");

            int ColorDecimal = Convert.ToInt32(finalSpecifier, 16);

            // Convert the RGB values to numbers
            int r = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
            int g = (byte)((ColorDecimal & 0xFF00) >> 8);
            int b = (byte)(ColorDecimal & 0xFF);

            // Now, transform
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }
    }
}
