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

using Terminaux.Colors.Transformation.Formulas;

namespace Terminaux.Colors.Transformation
{
    internal static class TransformationTools
    {
        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput)
        {
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                var transformed = ColorTools.ColorTransformationFormula switch
                {
                    TransformationFormula.Monochromacy =>
                        Monochromacy.Transform(rInput, gInput, bInput),
                    TransformationFormula.Inverse =>
                        Inverse.Transform(rInput, gInput, bInput),
                    _ =>
                        ColorBlind.Transform(rInput, gInput, bInput),
                };
                return transformed;
            }
            return (rInput, gInput, bInput);
        }
    }
}
