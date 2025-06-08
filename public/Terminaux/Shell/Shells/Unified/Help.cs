﻿//
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
using Terminaux.Shell.Help;
using Terminaux.Shell.Switches;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Opens the help page
    /// </summary>
    /// <remarks>
    /// This command allows you to get help for any specific command, including its usage. If no command is specified, all commands are listed.
    /// </remarks>
    class HelpUnifiedCommand : BaseCommand, ICommand
    {

        public override void Execute(CommandParameters parameters)
        {
            // Determine which type to show
            bool useSimplified = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-simplified");
            bool showCount = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-count");
            bool showGeneral = parameters.SwitchesList.Length == 0 ||
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-general") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all");
            bool showAlias = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-alias") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));
            bool showUnified = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-unified") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));
            bool showExtra = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-extra") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));

            // Now, show the help
            if (string.IsNullOrWhiteSpace(parameters.ArgumentsText))
                HelpPrint.ShowHelpExtended(useSimplified, showGeneral, showAlias, showUnified, showExtra, showCount);
            else
                HelpPrint.ShowHelpExtended(parameters.ArgumentsList[0], useSimplified);
        }

    }
}
