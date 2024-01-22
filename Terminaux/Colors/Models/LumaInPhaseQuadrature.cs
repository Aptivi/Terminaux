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
    /// The Luma, In-phase, and Quadrature (YIQ, NTSC 1953) model
    /// </summary>
    [DebuggerDisplay("YIQ = {Luma};{InPhase};{Quadrature}")]
    public class LumaInPhaseQuadrature : IEquatable<LumaInPhaseQuadrature>
    {
        /// <summary>
        /// The Y component, known as luma information [0 -> 255]
        /// </summary>
        public int Luma { get; private set; }
        /// <summary>
        /// The I component, known as chrominance information [0 -> 255]
        /// </summary>
        public int InPhase { get; private set; }
        /// <summary>
        /// The Q component, known as chrominance information [0 -> 255]
        /// </summary>
        public int Quadrature { get; private set; }
        /// <summary>
        /// The Y component, known as luma information [0.0 -> 1.0]
        /// </summary>
        public double LumaNormalized { get; private set; }
        /// <summary>
        /// The I component, known as chrominance information [-0.5957 -> 0.5957]
        /// </summary>
        public double InPhaseNormalized { get; private set; }
        /// <summary>
        /// The Q component, known as chrominance information [-0.5226 -> 0.5226]
        /// </summary>
        public double QuadratureNormalized { get; private set; }

        /// <summary>
        /// yiq:&lt;Y&gt;;&lt;I&gt;;&lt;Q&gt;
        /// </summary>
        public override string ToString() =>
            $"yiq:{Luma};{InPhase};{Quadrature}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as LumaInPhaseQuadrature);

        /// <inheritdoc/>
        public bool Equals(LumaInPhaseQuadrature other) =>
            other is not null &&
            Luma == other.Luma &&
            InPhase == other.InPhase &&
            Quadrature == other.Quadrature;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1746433734;
            hashCode = hashCode * -1521134295 + Luma.GetHashCode();
            hashCode = hashCode * -1521134295 + InPhase.GetHashCode();
            hashCode = hashCode * -1521134295 + Quadrature.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(LumaInPhaseQuadrature left, LumaInPhaseQuadrature right) =>
            EqualityComparer<LumaInPhaseQuadrature>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(LumaInPhaseQuadrature left, LumaInPhaseQuadrature right) =>
            !(left == right);

        internal LumaInPhaseQuadrature(int luma, int inphase, int quadrature)
        {
            Luma = luma;
            InPhase = inphase;
            Quadrature = quadrature;
            LumaNormalized = luma / 255d;
            InPhaseNormalized = (inphase - 128) / 255d;
            QuadratureNormalized = (quadrature - 128) / 255d;
        }
    }
}
