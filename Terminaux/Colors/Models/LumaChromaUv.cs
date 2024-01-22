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
using System.Collections.Generic;
using System.Diagnostics;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Luma chroma (YUV, PAL, SDTV with BT.470) model
    /// </summary>
    [DebuggerDisplay("YUV = {Luma};{ChromaU};{ChromaV}")]
    public class LumaChromaUv : IEquatable<LumaChromaUv>
    {
        /// <summary>
        /// The Y component, known as luma information [0 -> 255]
        /// </summary>
        public int Luma { get; private set; }
        /// <summary>
        /// The U component, known as chroma [0 -> 255]
        /// </summary>
        public int ChromaU { get; private set; }
        /// <summary>
        /// The V component, known as chroma [0 -> 255]
        /// </summary>
        public int ChromaV { get; private set; }
        /// <summary>
        /// The Y component, known as luma information [0.0 -> 1.0]
        /// </summary>
        public double LumaNormalized { get; private set; }
        /// <summary>
        /// The U component, known as chrominance information [-0.5019 -> 0.5019]
        /// </summary>
        public double ChromaUNormalized { get; private set; }
        /// <summary>
        /// The V component, known as chrominance information [-0.5019 -> 0.5019]
        /// </summary>
        public double ChromaVNormalized { get; private set; }

        /// <summary>
        /// yuv:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;
        /// </summary>
        public override string ToString() =>
            $"yuv:{Luma};{ChromaU};{ChromaV}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as LumaChromaUv);

        /// <inheritdoc/>
        public bool Equals(LumaChromaUv other) =>
            other is not null &&
            Luma == other.Luma &&
            ChromaU == other.ChromaU &&
            ChromaV == other.ChromaV;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -485171435;
            hashCode = hashCode * -1521134295 + Luma.GetHashCode();
            hashCode = hashCode * -1521134295 + ChromaU.GetHashCode();
            hashCode = hashCode * -1521134295 + ChromaV.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(LumaChromaUv left, LumaChromaUv right) =>
            EqualityComparer<LumaChromaUv>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(LumaChromaUv left, LumaChromaUv right) =>
            !(left == right);

        internal LumaChromaUv(int luma, int chromaU, int chromaV)
        {
            Luma = luma;
            ChromaU = chromaU;
            ChromaV = chromaV;
            LumaNormalized = luma / 255d;
            ChromaUNormalized = (chromaU - 128) / 255d;
            ChromaVNormalized = (chromaV - 128) / 255d;
        }
    }
}
