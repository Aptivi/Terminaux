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
using System.Threading;
using Terminaux.Shell.Arguments;
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    class SleepCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "sleep";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_SLEEP_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "ms", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_SLEEP_ARGUMENT_MS_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            Thread.CurrentThread.Join(int.Parse(parameters.ArgumentsList[0]));
            return 0;
        }
    }
}
