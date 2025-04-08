//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using Terminaux.Base;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// The command executor class
    /// </summary>
    public abstract class BaseCommand : ICommand
    {

        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        public abstract void Execute(CommandParameters parameters);

        /// <summary>
        /// Executes a command on dumb consoles
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        public virtual void ExecuteDumb(CommandParameters parameters) =>
            Execute(parameters);

        /// <summary>
        /// The help helper
        /// </summary>
        public virtual void HelpHelper() =>
            ConsoleLogger.Debug("No additional information found.");

    }
}
