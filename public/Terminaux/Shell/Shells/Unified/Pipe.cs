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

using Terminaux.Shell.Commands;
using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Themes.Colors;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Pipes the command output to another command as parameters
    /// </summary>
    /// <remarks>
    /// If you want to redirect the command output as the other command's parameters, you can use this command.
    /// </remarks>
    class PipeUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string sourceCommand = parameters.ArgumentsList[0];
            StringBuilder targetCommandBuilder = new(parameters.ArgumentsList[1] + " ");
            bool quoted = parameters.ContainsSwitch("-quoted");

            // First, get the source command output
            var currentShell = ShellManager.ShellStack[ShellManager.ShellStack.Count - 1];
            var currentType = currentShell.ShellType;
            bool buildingTarget = true;
            ConsoleLogger.Debug($"Writing piped output to the buffer for {sourceCommand}...");
            try
            {
                // Execute the source command
                ConsoleLogger.Debug($"Executing {sourceCommand} to the buffer for {currentType}...");
                string contents = CommandExecutor.BufferCommand(sourceCommand);
                variableValue = contents;
                buildingTarget = false;

                // Build the command based on the output and execute the target command
                ConsoleLogger.Debug($"Executing {targetCommandBuilder} for {currentType} with contents {contents}...");
                targetCommandBuilder.Append(quoted ? $"\"{contents}\"" : contents);
                ShellManager.GetLine($"{targetCommandBuilder}", "", currentType, true, false);
            }
            catch (Exception ex)
            {
                if (buildingTarget)
                {
                    ConsoleLogger.Error(ex, $"Execution of {sourceCommand} to the buffer failed.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLS_WRAP_SOURCEFAILED"), ThemeColorType.Error);
                }
                else
                {
                    ConsoleLogger.Error(ex, $"Execution of {targetCommandBuilder} failed.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLS_WRAP_TARGETFAILED") + $"\n    {targetCommandBuilder}", ThemeColorType.Error);
                }
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_SHELLS_WRAP_FAILED"), ThemeColorType.Error);
                ConsoleLogger.Error(ex, $"Reason for failure: {ex.Message}.");
                return ex.GetHashCode();
            }
            return 0;
        }

    }
}
