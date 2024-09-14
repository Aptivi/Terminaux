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
using Terminaux.Colors.Transformation.Formulas.ColorBlind;

namespace Terminaux.Colors.Transformation.Formulas
{
    internal class DeutanVienot : BaseTransformationFormula, ITransformationFormula
    {
        public override (int, int, int) Transform(int r, int g, int b, ColorSettings settings)
        {
            // Check values
            if (r < 0 || r > 255)
                throw new ArgumentOutOfRangeException("r");
            if (g < 0 || g > 255)
                throw new ArgumentOutOfRangeException("g");
            if (b < 0 || b > 255)
                throw new ArgumentOutOfRangeException("b");

            settings ??= new(ColorTools.GlobalSettings);
            var transformed = Vienot.Transform(r, g, b, TransformationFormula.DeutanVienot, settings.ColorBlindnessSeverity);
            return transformed;
        }
    }
}
