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

using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var commands = CommandManager.FindCommands(parameters.ArgumentsList[0], ShellManager.CurrentShellType);
            foreach (var command in commands)
            {
                TextWriterColor.Write("- ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(command.Command, ThemeColorType.ListValue);
            }
            if (commands.Length == 0)
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_FIND_NOTFOUND"), ThemeColorType.Error);
            return 0;
        }

    }
}
