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

using Terminaux.Colors.Models;

namespace Terminaux.Colors.Transformation.Contrast
{
    /// <summary>
    /// The color contrast type
    /// </summary>
    public enum ColorContrastType
    {
        /// <summary>
        /// Uses the half-white contrast method to determine the color contrast used for mixing (black or white)
        /// </summary>
        Half,
        /// <summary>
        /// Uses the light vs. dark contrast method to determine the color contrast used for mixing (black or white)
        /// </summary>
        Light,
        /// <summary>
        /// Uses the NTSC contrast method (see <see cref="LumaInPhaseQuadrature"/> for more info) to determine the color contrast used for mixing (black or white)
        /// </summary>
        Ntsc,
    }
}
