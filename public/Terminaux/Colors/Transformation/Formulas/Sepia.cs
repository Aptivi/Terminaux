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
using Terminaux.Base;

namespace Terminaux.Colors.Transformation.Formulas
{
    /// <summary>
    /// Sepia color filtering.
    /// </summary>
    public class Sepia : BaseTransformationFormula, ITransformationFormula
    {
        /// <inheritdoc/>
        public override (int, int, int) Transform(int r, int g, int b)
        {
            // Check values
            if (r < 0 || r > 255)
                throw new ArgumentOutOfRangeException("r");
            if (g < 0 || g > 255)
                throw new ArgumentOutOfRangeException("g");
            if (b < 0 || b > 255)
                throw new ArgumentOutOfRangeException("b");

            // Transform the color linear values while applying sepia
            int sepiaR = Math.Min((int)((0.393 * r) + (0.769 * g) + (0.189 * b)), 255);
            int sepiaG = Math.Min((int)((0.349 * r) + (0.686 * g) + (0.168 * b)), 255);
            int sepiaB = Math.Min((int)((0.272 * r) + (0.534 * g) + (0.131 * b)), 255);
            ConsoleLogger.Debug("Transformed to {0} with sepia filter", (sepiaR, sepiaG, sepiaB));
            return (sepiaR, sepiaG, sepiaB);
        }
    }
}
