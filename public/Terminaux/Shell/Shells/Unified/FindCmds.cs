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

using Terminaux.Colors.Data;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Finds commands
    /// </summary>
    /// <remarks>
    /// This command allows you to find a list of available commands by a given command name pattern.
    /// </remarks>
    class FindCmdsUnifiedCommand : BaseCommand, ICommand
    {

        public override void Execute(CommandParameters parameters)
        {
            var commands = CommandManager.FindCommands(parameters.ArgumentsList[0], ShellManager.CurrentShellType);
            foreach (var command in commands)
            {
                TextWriterColor.WriteColor("- ", false, ConsoleColors.Yellow);
                TextWriterColor.WriteColor(command.Command, ConsoleColors.Olive);
            }
            if (commands.Length == 0)
                TextWriterColor.WriteColor("No commands found.", ConsoleColors.Grey);
        }

    }
}
