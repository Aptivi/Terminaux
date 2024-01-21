﻿//
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
using System.Collections.Generic;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color contrast tools
    /// </summary>
    public static class ColorContrast
    {
        internal static List<int> unseeables = [
            (int)ConsoleColors.Black,
            (int)ConsoleColors.Grey0,
            (int)ConsoleColors.Grey3,
            (int)ConsoleColors.Grey7,
        ];

        /// <summary>
        /// Gets the color contrast appropriate for the specified color using the luma part of the YIQ color space (NTSC 1953)
        /// </summary>
        /// <param name="color">Target color to work on</param>
        /// <returns>Black if the luma info is less than or equal to 128, or white if greater than 128</returns>
        public static Color GetContrastColorNtsc(this Color color)
        {
            var black = new Color(ConsoleColors.Black);
            var white = new Color(ConsoleColors.White);
            int r = color.RGB.R;
            int g = color.RGB.G;
            int b = color.RGB.B;
            double lumaInfo = ((r * (0.299d * 1000)) + (g * (0.587d * 1000)) + (b * (0.114d * 1000))) / 1000;
            return lumaInfo >= 128 ? black : white;
        }

        /// <summary>
        /// Gets the color contrast appropriate for the specified color using the half white integer
        /// </summary>
        /// <param name="color">Target color to work on</param>
        /// <returns>Black if the color is greater than half white; otherwise, false.</returns>
        public static Color GetContrastColorHalf(this Color color)
        {
            var black = new Color(ConsoleColors.Black);
            var white = new Color(ConsoleColors.White);
            int rgbDecimal = Convert.ToInt32(color.Hex.Substring(1), 16);
            int rgbDecimalHalfWhite = 0xffffff / 2;
            return rgbDecimal > rgbDecimalHalfWhite ? black : white;
        }

        /// <summary>
        /// Checks to see if the specified color is considered seeable
        /// </summary>
        /// <param name="type">The color type to use</param>
        /// <param name="colorLevel">The color level that is in the range of 0-255</param>
        /// <param name="colorR">The red color level</param>
        /// <param name="colorG">The green color level</param>
        /// <param name="colorB">The blue color level</param>
        /// <returns>True if the specified color is considered "seeable." False otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsSeeable(ColorType type, int colorLevel, int colorR, int colorG, int colorB)
        {
            // First, check the values
            if (type < ColorType.TrueColor || type > ColorType._16Color)
                throw new TerminauxException("Color type is invalid");
            if (!ColorTools.TryParseColor(type == ColorType.TrueColor ? $"{colorR};{colorG};{colorB}" : $"{colorLevel}"))
                throw new TerminauxException("Color specifier for seeability is invalid");

            // Forbid setting these colors as they're considered too dark
            bool seeable = true;
            if (type == ColorType.TrueColor)
            {
                // Consider any color with all of the color levels less than 30 unseeable
                if (colorR < 30 && colorG < 30 && colorB < 30)
                    seeable = false;
            }
            else
            {
                // Consider any blacklisted color as unseeable
                if (unseeables.Contains(colorLevel))
                    seeable = false;
            }
            return seeable;
        }
    }
}
