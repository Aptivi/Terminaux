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
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Terminaux.Colors.Models;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}, {GetOrderCode()}]")]
    public partial class ConsoleColorData : IEquatable<ConsoleColorData>
    {
        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="color">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(Color color)
        {
            if (color.RGB is null)
                throw new ArgumentNullException(nameof(color));
            return MatchColorData(color.RGB);
        }

        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="rgb">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(RedGreenBlue rgb) =>
            MatchColorData(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Gets a color data instance that matches 
        /// </summary>
        /// <param name="r">Red color level to match</param>
        /// <param name="g">Green color level to match</param>
        /// <param name="b">Blue color level to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(int r, int g, int b)
        {
            var instances = GetColorData();

            // Get an instance that matches the conditions
            var instance = instances.FirstOrDefault((data) =>
                data.RGB is not null &&
                data.RGB.R == r &&
                data.RGB.G == g &&
                data.RGB.B == b
            );
            return instance;
        }

        /// <summary>
        /// Gets the three-dimension vector values from RGB color
        /// </summary>
        public Vector3 Vector =>
            RGB is not null ? new(RGB.R, RGB.G, RGB.B) : default;
    }
}
