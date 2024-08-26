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

namespace Terminaux.Colors.Transformation
{
    /// <summary>
    /// Color transformation formula
    /// </summary>
    public enum TransformationFormula
    {
        /// <summary>
        /// Red/green color blindness. It makes red look more green
        /// </summary>
        Protan,
        /// <summary>
        /// Red/green color blindness. It makes green look more red
        /// </summary>
        Deutan,
        /// <summary>
        /// Blue/yellow color blindness.
        /// </summary>
        Tritan,
        /// <summary>
        /// Red/green color blindness. It makes red look more green (Vienot, simple formula)
        /// </summary>
        ProtanVienot,
        /// <summary>
        /// Red/green color blindness. It makes green look more red (Vienot, simple formula)
        /// </summary>
        DeutanVienot,
        /// <summary>
        /// Blue/yellow color blindness. (Vienot, simple formula)
        /// </summary>
        TritanVienot,
        /// <summary>
        /// Full color blindness, can only see grayscale.
        /// </summary>
        Monochromacy,
        /// <summary>
        /// Inverse colors
        /// </summary>
        Inverse,
        /// <summary>
        /// Blue tinted monochromacy
        /// </summary>
        BlueScale,
        /// <summary>
        /// Green tinted monochromacy
        /// </summary>
        GreenScale,
        /// <summary>
        /// Red tinted monochromacy
        /// </summary>
        RedScale,
        /// <summary>
        /// Yellow tinted monochromacy
        /// </summary>
        YellowScale,
        /// <summary>
        /// Aqua tinted monochromacy
        /// </summary>
        AquaScale,
        /// <summary>
        /// Pink tinted monochromacy
        /// </summary>
        PinkScale,
    }
}
