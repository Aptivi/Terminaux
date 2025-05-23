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
    /// Cyanotype color filtering.
    /// </summary>
    public class Cyanotype : BaseTransformationFormula, ITransformationFormula
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

            // These are constants for the cyanotype filter.
            int[] darkCyan = [18, 40, 76];
            int[] lightCyan = [86, 136, 199];

            // Transform the color linear values while applying cyanotype.
            double luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;
            int cyanotypeR = Math.Min((int)((1 - luminance) * darkCyan[0] + luminance * lightCyan[0]), 255);
            int cyanotypeG = Math.Min((int)((1 - luminance) * darkCyan[1] + luminance * lightCyan[1]), 255);
            int cyanotypeB = Math.Min((int)((1 - luminance) * darkCyan[2] + luminance * lightCyan[2]), 255);
            ConsoleLogger.Debug("Transformed to {0} with cyanotype filter (weight: {1})", (cyanotypeR, cyanotypeG, cyanotypeB), luminance);
            return (cyanotypeR, cyanotypeG, cyanotypeB);
        }
    }
}
