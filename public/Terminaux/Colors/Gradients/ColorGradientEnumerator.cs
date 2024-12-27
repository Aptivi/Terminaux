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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Colors.Gradients
{
    /// <summary>
    /// The color gradient enumerator
    /// </summary>
    [DebuggerDisplay("{pos + 1} out of {gradients.Count}")]
    public class ColorGradientEnumerator : IEnumerator<ColorGradient>
    {
        internal readonly List<ColorGradient> gradients = [];
        private int pos = -1;

        /// <inheritdoc/>
        public ColorGradient Current
        {
            get
            {
                var gradient = gradients[pos] ??
                    throw new TerminauxInternalException("Are you sure that this gradient is not null?");
                return gradient;
            }
        }

        /// <inheritdoc/>
        public void Dispose() =>
            Reset();

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (pos < gradients.Count - 1)
            {
                pos++;
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        public void Reset() =>
            pos = -1;

        object IEnumerator.Current =>
            Current;

        internal ColorGradientEnumerator()
        { }
    }
}
