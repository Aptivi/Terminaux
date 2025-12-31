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

namespace Terminaux.Colors.Transformation.Tools.ColorBlind
{
    /// <summary>
    /// Deficiency type of the color blindness
    /// </summary>
    public enum ColorBlindDeficiency
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
    }
}
