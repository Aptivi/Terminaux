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
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Scripting;
using Terminaux.Shell.Switches;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Removes a MESH variable from the list
    /// </summary>
    /// <remarks>
    /// You can remove a MESH variable from the list or wipe its variable value
    /// </remarks>
    class UnsetCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "unset";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_UNSET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "$variable", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_UNSET_ARGUMENT_VARIABLE_DESC"
                    }),
                ],
                [
                    new SwitchInfo("justwipe", /* Localizable */ "T_SHELL_UNIFIED_UNSET_SWITCH_JUSTWIPE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool destructive = !parameters.ContainsSwitch("-justwipe");
            MESHVariables.SetVariable(parameters.ArgumentsList[0], "");
            if (destructive)
                MESHVariables.RemoveVariable(parameters.ArgumentsList[0]);
            return 0;
        }
    }
}
