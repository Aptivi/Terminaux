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

namespace Terminaux.Colors.Accessibility
{
    internal static class TransformationTools
    {
        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput)
        {
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                (int, int, int) transformed;
                if (ColorTools.ColorDeficiency == Deficiency.Monochromacy)
                    transformed = Monochromacy.Transform(rInput, gInput, bInput);
                else
                {
                    if (ColorTools.EnableSimpleColorTransformation)
                        transformed = Vienot1999.Transform(rInput, gInput, bInput, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    else
                        transformed = Brettel1997.Transform(rInput, gInput, bInput, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                }
                return (transformed.Item1, transformed.Item2, transformed.Item3);
            }
            return (rInput, gInput, bInput);
        }
    }
}
