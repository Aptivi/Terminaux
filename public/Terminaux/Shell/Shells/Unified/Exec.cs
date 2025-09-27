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

using Terminaux.Shell.Commands;
using System.Linq;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Commands.ProcessExecution;

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
            string arguments = string.Join(" ", [.. parameters.ArgumentsList.Skip(1)]);
            if (parameters.ContainsSwitch("-forked"))
            {
                ProcessExecutor.ExecuteProcessForked(command, arguments);
                return 0;
            }
            return ProcessExecutor.ExecuteProcess(command, arguments);
        }

    }
}
