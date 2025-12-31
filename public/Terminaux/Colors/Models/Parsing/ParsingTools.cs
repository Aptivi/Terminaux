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
using System.Globalization;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Textify.General;

namespace Terminaux.Colors.Models.Parsing
{
    /// <summary>
    /// Tools to parse RGB specifiers for general use
    /// </summary>
    public static class ParsingTools
    {
        private static readonly HashSet<string> colorNames = [.. Enum.GetNames(typeof(ConsoleColors))];

        /// <summary>
        /// Does the string specifier represent a valid model-agnostic specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid model-agnostic specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier) =>
            BaseColorModel.IsSpecifierValid(specifier);

        /// <summary>
        /// Does the string specifier represent a valid model-agnostic specifier and contain valid values?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid model-agnostic specifier and contains valid values</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValid(string specifier) =>
            BaseColorModel.IsSpecifierAndValueValid(specifier);

        /// <summary>
        /// Does the string specifier represent either a color name taken from <see cref="ConsoleColors"/>, a color number from 0 to 255, or a color code?
        /// </summary>
        /// <param name="specifier">Specifier that represents either a color name taken from <see cref="ConsoleColors"/>, a color number from 0 to 255, or a color code</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierConsoleColors(string specifier)
        {
            if (double.TryParse(specifier, out double specifierNum))
                return specifierNum >= 0;
            return colorNames.Contains(specifier);
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
        /// Does the string specifier represent a valid RGB hash (#RGB or #RRGGBB) as in HTML?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid RGB hash (#RGB or #RRGGBB) as in HTML</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValidRgbHash(string specifier)
        {
            if (!IsSpecifierValidRgbHash(specifier))
                return false;

            // Get the integral value of the total color
            string finalSpecifier = specifier.Substring(1);
            if (finalSpecifier.Length == 3)
            {
                char first = finalSpecifier[0];
                char second = finalSpecifier[1];
                char third = finalSpecifier[2];
                finalSpecifier = $"{first}{first}{second}{second}{third}{third}";
                ConsoleLogger.Debug("From {0} -> {1}", specifier, finalSpecifier);
            }
            else if (finalSpecifier.Length != 6)
                return false;

            return int.TryParse(finalSpecifier, NumberStyles.HexNumber, null, out _);
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifier(string specifier, ColorSettings? settings = null) =>
            BaseColorModel.ParseSpecifierToRgb(specifier, settings);

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB</param>
        /// <param name="rgb">Output for the RGB component for 256- and 16-color modes</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool TryParseSpecifier(string specifier, out RedGreenBlue? rgb)
        {
            try
            {
                rgb = ParseSpecifier(specifier);
                return true;
            }
            catch
            {
                rgb = null;
                return false;
            }
        }

        /// <summary>
        /// Parses the specifier that holds the color name and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Color name defined in <see cref="ConsoleColors"/></param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierRgbName(string specifier, ColorSettings? settings = null)
        {
            if (!IsSpecifierConsoleColors(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDSPECIFIER").FormatString(specifier));

            // Form the sequences using the information from the color details
            ConsoleColorData data;
            if (int.TryParse(specifier, out int colorCode) && colorCode > 255)
            {
                var rgb = ColorTools.GetRgbFromColorCode(colorCode);
                data = ConsoleColorData.GetNearestColor(rgb);
            }
            else
            {
                var parsedEnum = (ConsoleColors)Enum.Parse(typeof(ConsoleColors), specifier);
                data = ConsoleColorData.GetColorData(parsedEnum);
            }

            // Check to see if we need to transform color. Else, be sane.
            int r = Convert.ToInt32(data.RGB.R);
            if (r < 0 || r > 255)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSERGBREDLEVEL") + $" {r}");
            int g = Convert.ToInt32(data.RGB.G);
            if (g < 0 || g > 255)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSERGBGREENLEVEL") + $" {g}");
            int b = Convert.ToInt32(data.RGB.B);
            if (b < 0 || b > 255)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSERGBBLUELEVEL") + $" {b}");

            // Now, transform
            ConsoleLogger.Debug("From {0}, {1}, {2}...", r, g, b);
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            ConsoleLogger.Debug("...to {0}, {1}, {2}", finalRgb.r, finalRgb.g, finalRgb.b);
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        /// <summary>
        /// Parses the hex representation of RGB and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB in hex representation</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierRgbHash(string specifier, ColorSettings? settings = null)
        {
            if (!IsSpecifierValidRgbHash(specifier))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHEXSPECIFIER").FormatString(specifier) + ": #RRGGBB");

            // Get the integral value of the total color
            string finalSpecifier = specifier.Substring(1);
            if (finalSpecifier.Length == 3)
            {
                char first = finalSpecifier[0];
                char second = finalSpecifier[1];
                char third = finalSpecifier[2];
                finalSpecifier = $"{first}{first}{second}{second}{third}{third}";
                ConsoleLogger.Debug("From {0} -> {1}", specifier, finalSpecifier);
            }
            else if (finalSpecifier.Length != 6)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEINVALIDHEXLENGTH").FormatString(specifier) + ": #RRGGBB");

            bool valid = int.TryParse(finalSpecifier, NumberStyles.HexNumber, null, out int ColorDecimal);
            if (!valid)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_PARSEHEXUNRESOLVABLE").FormatString(specifier));

            // Convert the RGB values to numbers
            int r = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
            int g = (byte)((ColorDecimal & 0xFF00) >> 8);
            int b = (byte)(ColorDecimal & 0xFF);

            // Now, transform
            ConsoleLogger.Debug("From {0}, {1}, {2}...", r, g, b);
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            ConsoleLogger.Debug("...to {0}, {1}, {2}", finalRgb.r, finalRgb.g, finalRgb.b);
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        /// <summary>
        /// Parses the hex representation of RGB and returns an instance of <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of RGB in hex representation</param>
        /// <param name="output">Output for the RGB component</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool TryParseSpecifierRgbHash(string specifier, out RedGreenBlue? output)
        {
            try
            {
                output = ParseSpecifierRgbHash(specifier);
                return true;
            }
            catch
            {
                output = null;
                return false;
            }
        }
    }
}
