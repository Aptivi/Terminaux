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

using System.Collections.Generic;
using Terminaux.Colors.Transformation.Formulas;

namespace Terminaux.Colors.Transformation
{
    internal static class TransformationTools
    {
        private static readonly Dictionary<TransformationFormula, BaseTransformationFormula> formulas = new()
        {
            { TransformationFormula.Monochromacy, new Monochromacy() },
            { TransformationFormula.Inverse, new Inverse() },
            { TransformationFormula.Protan, new ColorBlind() },
            { TransformationFormula.Deutan, new ColorBlind() },
            { TransformationFormula.Tritan, new ColorBlind() },
        };

        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput)
        {
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                var transformed = formulas[ColorTools.ColorTransformationFormula].Transform(rInput, gInput, bInput);
                return transformed;
            }
            return (rInput, gInput, bInput);
        }
    }
}
