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

using System.Linq;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Commands.ProcessExecution;
using Terminaux.Shell.Switches;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Executes an external command
    /// </summary>
    /// <remarks>
    /// If you need to take a look at a process output, it's wise to use this command.
    /// </remarks>
    class ExecUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string command = parameters.ArgumentsList[0];
            string arguments = string.Join(' ', parameters.ArgumentsList.Skip(1).ToArray());
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-forked"))
            {
                ProcessExecutor.ExecuteProcessForked(command, arguments);
                return 0;
            }
            return ProcessExecutor.ExecuteProcess(command, arguments);
        }

    }
}
