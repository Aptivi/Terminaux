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
using System.Diagnostics;

namespace Terminaux.Colors.Data
{
    /// <summary>
    /// Console color data
    /// </summary>
    [DebuggerDisplay("{Name} [{ColorId}, {HexString}, {GetOrderCode()}]")]
    public partial class ConsoleColorData : IEquatable<ConsoleColorData>
    {
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is ConsoleColorData data)
                return Equals(data);
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(ConsoleColorData other) =>
            other is not null &&
            ColorId == other.ColorId;

        /// <inheritdoc/>
        public override int GetHashCode() =>
            -1308032243 + ColorId.GetHashCode();

        /// <summary>
        /// Gets the RGB order code
        /// </summary>
        /// <returns>RGB order code in decimal RRRGGGBBB format</returns>
        public int GetOrderCode() =>
#if GENERATOR
            (RGB.b << 16) | (RGB.g << 8) | RGB.r;
#else
            ((RGB?.B << 16) | (RGB?.G << 8) | RGB?.R) ?? 0;
#endif

        /// <inheritdoc/>
        public static bool operator ==(ConsoleColorData? left, ConsoleColorData? right)
        {
            if (left is null || right is null)
                return false;
            return EqualityComparer<ConsoleColorData>.Default.Equals(left, right);
        }

        /// <inheritdoc/>
        public static bool operator !=(ConsoleColorData? left, ConsoleColorData? right) =>
            !(left == right);
    }
}
