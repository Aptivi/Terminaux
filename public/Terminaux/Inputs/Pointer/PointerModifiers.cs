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

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer modifiers
    /// </summary>
    public enum PointerModifiers
    {
        /// <summary>
        /// No key was pressed at the time of the event
        /// </summary>
        None = 0,
        /// <summary>
        /// CTRL key was pressed at the time of the event
        /// </summary>
        Ctrl = 1,
        /// <summary>
        /// ALT key was pressed at the time of the event
        /// </summary>
        Alt = 2,
        /// <summary>
        /// SHIFT key was pressed at the time of the event
        /// </summary>
        Shift = 4,
    }
}
