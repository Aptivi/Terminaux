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
    /// Tools to parse HSL specifiers
    /// </summary>
    public static class HslParsingTools
    {
        /// <summary>
        /// Does the string specifier represent a valid HSL specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HSL specifier</param>
        /// <param name="checkParts">Whether to check the parts count or not</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierValid(string specifier, bool checkParts = false) =>
            specifier.Contains(";") &&
            specifier.StartsWith("hsl:") &&
            (!checkParts || (checkParts && specifier.Substring(4).Split(';').Length == 3));

        /// <summary>
        /// Does the string specifier represent a valid HSL specifier?
        /// </summary>
        /// <param name="specifier">Specifier that represents a valid HSL specifier</param>
        /// <returns>True if the specifier is valid; false otherwise.</returns>
        public static bool IsSpecifierAndValueValid(string specifier)
        {
            if (!IsSpecifierValid(specifier, true))
                return false;

            var specifierArray = specifier.Substring(4).Split(';');
            int h = Convert.ToInt32(specifierArray[0]);
            if (h < 0 || h > 360)
                return false;
            int s = Convert.ToInt32(specifierArray[1]);
            if (s < 0 || s > 100)
                return false;
            int l = Convert.ToInt32(specifierArray[2]);
            if (l < 0 || l > 100)
                return false;
            return true;
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueSaturationLightness"/>
        /// </summary>
        /// <param name="specifier">Specifier of HSL</param>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ParseSpecifier(string specifier)
        {
            if (!IsSpecifierValid(specifier))
                throw new TerminauxException($"Invalid HSL color specifier \"{specifier}\". Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the HSL whole values! First, check to see if we need to filter the color for the color-blind
                int h = Convert.ToInt32(specifierArray[0]);
                if (h < 0 || h > 360)
                    throw new TerminauxException($"The hue level is out of range (0' -> 360' degrees). {h}");
                int s = Convert.ToInt32(specifierArray[1]);
                if (s < 0 || s > 100)
                    throw new TerminauxException($"The saturation level is out of range (0 -> 100). {s}");
                int l = Convert.ToInt32(specifierArray[2]);
                if (l < 0 || l > 100)
                    throw new TerminauxException($"The lightness level is out of range (0 -> 100). {l}");

                // First, we need to convert from HSL to RGB
                double hPart = (double)h / 360;
                double sPart = (double)s / 100;
                double lPart = (double)l / 100;
                var hsl = new HueSaturationLightness(hPart, sPart, lPart);
                return hsl;
            }
            else
                throw new TerminauxException($"Invalid HSL color specifier \"{specifier}\". The specifier may not be more than three elements. Ensure that it's on the correct format: hsl:<C>;<M>;<Y>");
        }

        /// <summary>
        /// Parses the specifier and returns an instance of <see cref="HueSaturationLightness"/> converted to <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="specifier">Specifier of HSL</param>
        /// <param name="settings">Settings to use. Use null for global settings</param>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ParseSpecifierToRgb(string specifier, ColorSettings? settings = null)
        {
            var hsl = ParseSpecifier(specifier);
            var rgb = RgbConversionTools.ConvertFrom(hsl);
            int r = rgb.R;
            int g = rgb.G;
            int b = rgb.B;

            // Now, transform
            settings ??= new(ColorTools.GlobalSettings);
            var finalRgb = TransformationTools.GetTransformedColor(r, g, b, settings);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }
    }
}
