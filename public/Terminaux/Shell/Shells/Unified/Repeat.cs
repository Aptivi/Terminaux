//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Repeats a command
    /// </summary>
    /// <remarks>
    /// You can repeat either the last command entered or the specified command.
    /// </remarks>
    class RepeatUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string timesString = parameters.ArgumentsList[0];
            bool valid = uint.TryParse(timesString, out uint times);
            if (!valid)
            {
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_REPEAT_INVALIDTIMES"), true, ConsoleColors.Red);
                return 18;
            }

            // Get the command to be executed
            string lastCommand = ShellManager.lastCommand;
            if (parameters.ArgumentsList.Length > 1)
                lastCommand = parameters.ArgumentsList[1];

            // Check to see if we're trying to call repeat
            var argumentInfo = ArgumentsParser.ParseShellCommandArguments(lastCommand, ShellManager.CurrentShellType).total[0];
            if (argumentInfo.Command == "repeat")
            {
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_REPEAT_REPEATSELF"), true, ConsoleColors.Red);
                return 19;
            }

            // First, initialize the alternative command thread
            var AltThreads = ShellManager.ShellStack[ShellManager.ShellStack.Count - 1].AltCommandThreads;
            if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
            {
                ConsoleLogger.Debug("Making alt thread for repeated command {0}...", lastCommand);
                var WrappedCommand = new Thread((cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                ShellManager.ShellStack[ShellManager.ShellStack.Count - 1].AltCommandThreads.Add(WrappedCommand);
            }

            // Now, execute the command n times
            for (uint i = 1; i <= times; i++)
                ShellManager.GetLine(lastCommand);
            return 0;
        }

    }
}
