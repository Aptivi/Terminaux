//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}, {GetOrderCode()}]")]
    public partial class ConsoleColorData : IEquatable<ConsoleColorData>
    {
        /// <summary>
        /// Gets the console colors data
        /// </summary>
        public static partial ConsoleColorData[] GetColorData();

        /// <summary>
        /// Gets the console color data
        /// </summary>
        /// <param name="colors">Console color to get the data from</param>
        /// <returns>A console color data instance</returns>
        public static ConsoleColorData GetColorData(ConsoleColors colors)
        {
            if (colors < ConsoleColors.Black || colors > ConsoleColors.Grey93)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_DATA_EXCEPTION_COLOROUTOFRANGE"));
            return GetColorData()[(int)colors];
        }
    }
}
