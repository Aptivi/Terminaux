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
using Terminaux.Base;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Information for the console colors that fit within the 256 colors
    /// </summary>
    [DebuggerDisplay("RGB = {R};{G};{B}, ID = {ColorID}")]
    public class ConsoleColorsInfo : IEquatable<ConsoleColorsInfo>
    {

        /// <summary>
        /// The color ID
        /// </summary>
        public int ColorID { get; }
        /// <summary>
        /// The red color value
        /// </summary>
        public int R { get; }
        /// <summary>
        /// The green color value
        /// </summary>
        public int G { get; }
        /// <summary>
        /// The blue color value
        /// </summary>
        public int B { get; }
        /// <summary>
        /// A <see cref="Colors.Color"/> instance to get more information
        /// </summary>
        public Color Color =>
            new(R, G, B);

        /// <summary>
        /// Makes a new instance of 255-color console color information
        /// </summary>
        /// <param name="ColorValue">A 255-color console color</param>
        public ConsoleColorsInfo(ConsoleColors ColorValue)
        {
            if (!((int)ColorValue < 0 | (int)ColorValue > 255))
            {
                var ColorData = ConsoleColorData.GetColorData()[Convert.ToInt32(ColorValue)];
                if (ColorData.RGB is null)
                    throw new TerminauxInternalException("The RGB instance of the color is null.");
                ColorID = ColorData.ColorId;
                R = ColorData.RGB.R;
                G = ColorData.RGB.G;
                B = ColorData.RGB.B;
            }
            else
                throw new ArgumentOutOfRangeException(nameof(ColorValue), ColorValue, "The color value is outside the range of 0-255.");
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            base.Equals(obj);

        /// <summary>
        /// Checks to see if this instance of <see cref="ConsoleColorsInfo"/> is equal to another instance of <see cref="ConsoleColorsInfo"/>
        /// </summary>
        /// <param name="other">Another instance of <see cref="ConsoleColorsInfo"/> to compare with this instance</param>
        /// <returns>True if both <see cref="ConsoleColorsInfo"/> instances match; otherwise, false.</returns>
        public bool Equals(ConsoleColorsInfo other)
            => Equals(this, other);

        /// <summary>
        /// Checks to see if this instance of <see cref="ConsoleColorsInfo"/> is equal to another instance of <see cref="ConsoleColorsInfo"/>
        /// </summary>
        /// <param name="other">Another instance of <see cref="ConsoleColorsInfo"/> to compare with another instance</param>
        /// <param name="other2">Another instance of <see cref="ConsoleColorsInfo"/> to compare with another instance</param>
        /// <returns>True if both <see cref="ConsoleColorsInfo"/> instances match; otherwise, false.</returns>
        public bool Equals(ConsoleColorsInfo other, ConsoleColorsInfo other2)
        {
            // We can't perform this operation on null.
            if (other is null)
                return false;

            // Check all the properties
            return
                other.R == other2.R &&
                other.G == other2.G &&
                other.B == other2.B &&
                other.ColorID == other2.ColorID
            ;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1523239773;
            hashCode = hashCode * -1521134295 + ColorID.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(ConsoleColorsInfo a, ConsoleColorsInfo b)
            => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(ConsoleColorsInfo a, ConsoleColorsInfo b)
            => !a.Equals(b);
    }
}
