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
using Terminaux.Base;
using Terminaux.Colors.Transformation.Tools.ColorBlind;

namespace Terminaux.Colors.Transformation.Formulas
{
    /// <summary>
    /// Color blindness simulation formula
    /// </summary>
    public class ColorBlind : BaseTransformationFormula, ITransformationFormula
    {
        /// <inheritdoc/>
        public override double Frequency { get; set; } = 0.6;

        /// <summary>
        /// Color blindness deficiency
        /// </summary>
        public ColorBlindDeficiency Deficiency { get; set; } = ColorBlindDeficiency.Protan;

        /// <summary>
        /// Uses Vienot's formula insteas of Brettel's
        /// </summary>
        public bool Simple { get; set; } = false;

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

            var transformed = Simple ?
                Vienot.Transform(r, g, b, Deficiency, Frequency) :
                Brettel.Transform(r, g, b, Deficiency, Frequency);
            ConsoleLogger.Debug("Transformed to {0} using parameters: def: {1}, freq: {2}", transformed, Deficiency, Frequency);
            return transformed;
        }
    }
}
