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

namespace Terminaux.Base.Extensions
{
    /// <summary>
    /// Console taskbar progress enumeration
    /// </summary>
    public enum ConsoleTaskbarProgressEnum
    {
        /// <summary>
        /// There is no progress being done
        /// </summary>
        NoProgress = 0,
        /// <summary>
        /// This progress is indeterminate
        /// </summary>
        Indeterminate = 0x1,
        /// <summary>
        /// This progress is normal
        /// </summary>
        Normal = 0x2,
        /// <summary>
        /// This progress indicates an error
        /// </summary>
        Error = 0x4,
        /// <summary>
        /// This progress is paused
        /// </summary>
        Paused = 0x8,
        /// <summary>
        /// This progress indicates a warning
        /// </summary>
        Warning = Paused
    }
}
