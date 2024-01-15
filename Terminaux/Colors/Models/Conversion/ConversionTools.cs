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
using Terminaux.Colors.Data;

namespace Terminaux.Colors.Models.Conversion
{
    /// <summary>
    /// Color conversion tools
    /// </summary>
    public static class ConversionTools
    {
        /// <summary>
        /// Translates the color from .NET's <see cref="ConsoleColor"/> to X11's representation, <see cref="ConsoleColors"/>
        /// </summary>
        /// <param name="color">.NET's <see cref="ConsoleColor"/> to translate this color to</param>
        /// <returns>X11's representation of this color, <see cref="ConsoleColors"/></returns>
        public static ConsoleColors TranslateToX11ColorMap(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => ConsoleColors.Black,
                ConsoleColor.DarkBlue => ConsoleColors.DarkRed,
                ConsoleColor.DarkGreen => ConsoleColors.DarkGreen,
                ConsoleColor.DarkCyan => ConsoleColors.DarkYellow,
                ConsoleColor.DarkRed => ConsoleColors.DarkBlue,
                ConsoleColor.DarkMagenta => ConsoleColors.DarkMagenta,
                ConsoleColor.DarkYellow => ConsoleColors.DarkCyan,
                ConsoleColor.Gray => ConsoleColors.Gray,
                ConsoleColor.DarkGray => ConsoleColors.DarkGray,
                ConsoleColor.Blue => ConsoleColors.Red,
                ConsoleColor.Green => ConsoleColors.Green,
                ConsoleColor.Cyan => ConsoleColors.Yellow,
                ConsoleColor.Red => ConsoleColors.Blue,
                ConsoleColor.Magenta => ConsoleColors.Magenta,
                ConsoleColor.Yellow => ConsoleColors.Cyan,
                ConsoleColor.White => ConsoleColors.White,
                _ => ConsoleColors.Black,
            };
        }

        /// <summary>
        /// Translates the color from X11's <see cref="ConsoleColors"/> to .NET's representation, <see cref="ConsoleColor"/>
        /// </summary>
        /// <param name="color">X11's <see cref="ConsoleColors"/> to translate this color to</param>
        /// <returns>.NET's representation of this color, <see cref="ConsoleColor"/></returns>
        public static ConsoleColor TranslateToStandardColorMap(ConsoleColors color)
        {
            return color switch
            {
                ConsoleColors.Black => ConsoleColor.Black,
                ConsoleColors.DarkRed => ConsoleColor.DarkBlue,
                ConsoleColors.DarkGreen => ConsoleColor.DarkGreen,
                ConsoleColors.DarkYellow => ConsoleColor.DarkCyan,
                ConsoleColors.DarkBlue => ConsoleColor.DarkRed,
                ConsoleColors.DarkMagenta => ConsoleColor.DarkMagenta,
                ConsoleColors.DarkCyan => ConsoleColor.DarkYellow,
                ConsoleColors.Gray => ConsoleColor.Gray,
                ConsoleColors.DarkGray => ConsoleColor.DarkGray,
                ConsoleColors.Red => ConsoleColor.Blue,
                ConsoleColors.Green => ConsoleColor.Green,
                ConsoleColors.Yellow => ConsoleColor.Cyan,
                ConsoleColors.Blue => ConsoleColor.Red,
                ConsoleColors.Magenta => ConsoleColor.Magenta,
                ConsoleColors.Cyan => ConsoleColor.Yellow,
                ConsoleColors.White => ConsoleColor.White,
                _ => ConsoleColor.Black,
            };
        }

        /// <summary>
        /// Corrects the color map for <see cref="ConsoleColor"/> according to the X11 specification
        /// </summary>
        /// <param name="color">.NET's <see cref="ConsoleColor"/> to correct this color</param>
        /// <returns>Corrected <see cref="ConsoleColor"/></returns>
        public static ConsoleColor CorrectStandardColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => ConsoleColor.Black,
                ConsoleColor.DarkBlue => ConsoleColor.DarkRed,
                ConsoleColor.DarkGreen => ConsoleColor.DarkGreen,
                ConsoleColor.DarkCyan => ConsoleColor.DarkYellow,
                ConsoleColor.DarkRed => ConsoleColor.DarkBlue,
                ConsoleColor.DarkMagenta => ConsoleColor.DarkMagenta,
                ConsoleColor.DarkYellow => ConsoleColor.DarkCyan,
                ConsoleColor.Gray => ConsoleColor.Gray,
                ConsoleColor.DarkGray => ConsoleColor.DarkGray,
                ConsoleColor.Blue => ConsoleColor.Red,
                ConsoleColor.Green => ConsoleColor.Green,
                ConsoleColor.Cyan => ConsoleColor.Yellow,
                ConsoleColor.Red => ConsoleColor.Blue,
                ConsoleColor.Magenta => ConsoleColor.Magenta,
                ConsoleColor.Yellow => ConsoleColor.Cyan,
                ConsoleColor.White => ConsoleColor.White,
                _ => ConsoleColor.Black,
            };
        }
    }
}
