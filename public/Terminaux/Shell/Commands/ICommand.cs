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
    /// Base command executor
    /// </summary>
    public interface ICommand
    {

        /// <summary>
        /// Executes the command with the given argument
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        void Execute(CommandParameters parameters);

        /// <summary>
        /// Executes the command with the given argument on dumb consoles
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        void ExecuteDumb(CommandParameters parameters);

        /// <summary>
        /// Shows additional information for the command when "help command" is invoked
        /// </summary>
        void HelpHelper();

    }
}
