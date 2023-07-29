
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using Newtonsoft.Json.Linq;

namespace Terminaux.Colors
{
    /// <summary>
    /// 255 colors tools
    /// </summary>
    public static class Color255
    {
        /// <summary>
        /// The 255 console colors data JSON token to get information about these colors
        /// </summary>
        public static readonly JToken ColorDataJson = JToken.Parse(Properties.Resources.ConsoleColorsData);

        /// <summary>
        /// A simplification for <see cref="Convert.ToChar(int)"/> function to return the ESC character
        /// </summary>
        /// <returns>ESC</returns>
        internal static char GetEsc() => Convert.ToChar(0x1B);
    }
}
