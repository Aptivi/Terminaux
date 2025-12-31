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

namespace Terminaux.Shell.Help
{
    /// <summary>
    /// Help command type for filtering
    /// </summary>
    public enum HelpCommandType
    {
        /// <summary>
        /// No commands (undefined)
        /// </summary>
        None = 0,
        /// <summary>
        /// General commands
        /// </summary>
        General = 1 << 0,
        /// <summary>
        /// Unified commands
        /// </summary>
        Unified = 1 << 1,
        /// <summary>
        /// Aliased commands
        /// </summary>
        Aliases = 1 << 2,
        /// <summary>
        /// Extra commands
        /// </summary>
        Extras = 1 << 3,
    }
}
