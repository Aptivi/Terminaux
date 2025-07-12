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
        private static ConsoleColorData[] cachedOrderedColorDataAscending = [];
        private static ConsoleColorData[] cachedOrderedColorDataDescending = [];

        /// <summary>
        /// Gets a color data instance that matches the available color instances
        /// </summary>
        /// <param name="color">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(Color color) =>
            MatchColorData(color.RGB);

        /// <summary>
        /// Gets a color data instance that matches the available color instances
        /// </summary>
        /// <param name="rgb">Color to match</param>
        /// <returns>Either an instance of <see cref="ConsoleColorData"/> if found, or <see langword="null"/> if not found</returns>
        public static ConsoleColorData MatchColorData(RedGreenBlue rgb) =>
            MatchColorData(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Gets a color data instance that matches the available color instances
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
        /// Orders color data instances by their RGB code
        /// </summary>
        /// <param name="descending">Descending order</param>
        /// <returns>An array of <see cref="ConsoleColorData"/> ordered by ascending or descending RGB code that can be get by <see cref="GetOrderCode"/></returns>
        public static ConsoleColorData[] OrderColorData(bool descending = false)
        {
            if (cachedOrderedColorDataAscending.Length > 0 && !descending)
                return cachedOrderedColorDataAscending;
            else if (cachedOrderedColorDataDescending.Length > 0 && descending)
                return cachedOrderedColorDataDescending;
            var data = GetColorData();
            if (descending)
            {
                data = [.. data.OrderByDescending((cp) => cp.GetOrderCode())];
                cachedOrderedColorDataDescending = data;
            }
            else
            {
                data = [.. data.OrderBy((cp) => cp.GetOrderCode())];
                cachedOrderedColorDataAscending = data;
            }
            return data;
        }

        /// <summary>
        /// Gets the nearest color from the built-in X11 color palette
        /// </summary>
        /// <param name="color">True color instance</param>
        /// <returns>The nearest color for the indicated color, or <see langword="null"/> if it doesn't exist</returns>
        public static ConsoleColorData GetNearestColor(Color color) =>
            GetNearestColor(color.RGB);

        /// <summary>
        /// Gets the nearest color from the built-in X11 color palette
        /// </summary>
        /// <param name="rgb">RGB instance</param>
        /// <returns>The nearest color for the indicated color, or <see langword="null"/> if it doesn't exist</returns>
        public static ConsoleColorData GetNearestColor(RedGreenBlue rgb) =>
            GetNearestColor(rgb.R, rgb.G, rgb.B);

        /// <summary>
        /// Gets the nearest color from the built-in X11 color palette
        /// </summary>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        /// <returns>The nearest color for the indicated color, or <see langword="null"/> if it doesn't exist</returns>
        public static ConsoleColorData GetNearestColor(int r, int g, int b)
        {
            // We need to get a color map representing RGB values and their names
            var data = OrderColorData();

            // Now, using the three-dimensional vector instance, we need to calculate the nearest color
            // by getting the distance between our current color and the target color by calculating it
            // like this:
            //
            //   √((COL.RGB.R - CCD.RGB.R)^2 + (COL.RGB.G - CCD.RGB.G)^2 + (COL.RGB.B - CCD.RGB.B)^2)
            //
            // ...where COL refers to this Color instance and CCD refers to the target color data in the
            // loop below.
            float minimum = float.PositiveInfinity;
            Vector3 vector = new(r, g, b);
            ConsoleColorData result = Black;
            for (int i = 0; i < data.Length; i++)
            {
                ConsoleColorData? colorData = data[i];

                // Calculate the distance using the formula above.
                float distance = Vector3.DistanceSquared(vector, colorData.Vector);
                if (distance < minimum)
                {
                    // Keep getting distance until we find the correct color.
                    minimum = distance;
                    result = colorData;
                }
            }
            return result;
        }
    }
}
