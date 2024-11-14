﻿//
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var commands = CommandManager.FindCommands(parameters.ArgumentsList[0], ShellManager.CurrentShellType);
            foreach (var command in commands)
            {
                TextWriters.Write("- ", false, KernelColorType.ListEntry);
                TextWriters.Write(command.Command, KernelColorType.ListValue);
            }
            if (commands.Length == 0)
                TextWriters.Write(Translate.DoTranslation("No commands found."), KernelColorType.Error);
            return 0;
        }

    }
}
