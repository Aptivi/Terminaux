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

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command flags
    /// </summary>
    public enum CommandFlags
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,
        /// <summary>
        /// The command is obsolete.
        /// </summary>
        Obsolete = 1,
        /// <summary>
        /// Redirection is supported, meaning that all the output to the commands can be redirected to a file.
        /// </summary>
        RedirectionSupported = 2,
        /// <summary>
        /// This command is wrappable to pages.
        /// </summary>
        Wrappable = 4,
        /// <summary>
        /// This command is hidden from the help page
        /// </summary>
        Hidden = 8,
    }
}
