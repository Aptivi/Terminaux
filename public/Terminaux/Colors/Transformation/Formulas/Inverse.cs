﻿//
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
    /// Inverse colors
    /// </summary>
    public class Inverse : BaseTransformationFormula, ITransformationFormula
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

            // Inverse the RGB
            int invR = 255 - r;
            int invG = 255 - g;
            int invB = 255 - b;
            var final = TransformationTools.BlendColor((r, g, b), (invR, invG, invB), Frequency);
            ConsoleLogger.Debug("Transformed to {0} using parameters: freq: {1}, blend: {2}", (final.RGB.R, final.RGB.G, final.RGB.B), Frequency, (invR, invG, invB));
            return (final.RGB.R, final.RGB.G, final.RGB.B);
        }
    }
}
