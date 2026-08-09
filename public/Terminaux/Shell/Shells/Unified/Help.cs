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
using Terminaux.Shell.Help;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command => 
            "help";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_HELP_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                    {
                        AutoCompleter = (_) => CommandManager.GetCommandNames(ShellManager.CurrentShellType),
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_HELP_ARGUMENT_COMMAND_DESC"
                    })
                ],
                [
                    new SwitchInfo("general", /* Localizable */ "T_SHELL_UNIFIED_HELP_GENERAL_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("alias", /* Localizable */ "T_SHELL_UNIFIED_HELP_ALIAS_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("unified", /* Localizable */ "T_SHELL_UNIFIED_HELP_UNIFIED_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("extra", /* Localizable */ "T_SHELL_UNIFIED_HELP_EXTRA_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("all", /* Localizable */ "T_SHELL_UNIFIED_HELP_ALL_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("simplified", /* Localizable */ "T_SHELL_UNIFIED_HELP_SIMPLIFIED_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("hidden", /* Localizable */ "T_SHELL_UNIFIED_HELP_HIDDEN_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("count", /* Localizable */ "T_SHELL_UNIFIED_HELP_COUNT_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("markdown", /* Localizable */ "T_SHELL_UNIFIED_HELP_MARKDOWN_SWITCH_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ], false)
            ];

        public override CommandFlags Flags => 
            CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Determine which type to show
            bool useSimplified = parameters.ContainsSwitch("-simplified");
            bool showCount = parameters.ContainsSwitch("-count");
            bool showHidden = parameters.ContainsSwitch("-hidden");
            bool showMarkdown = parameters.ContainsSwitch("-markdown");
            bool showGeneral = parameters.SwitchesList.Length == 0 || !parameters.ContainsAnySwitches(["-general", "-unified", "-alias", "-extra"]) || parameters.ContainsAnySwitches(["-general", "-all"]);
            bool showAlias = parameters.SwitchesList.Length > 0 && parameters.ContainsAnySwitches(["-alias", "-all"]);
            bool showUnified = parameters.SwitchesList.Length == 0 || !parameters.ContainsAnySwitches(["-general", "-unified", "-alias", "-extra"]) || parameters.ContainsAnySwitches(["-unified", "-all"]);
            bool showExtra = parameters.SwitchesList.Length > 0 && parameters.ContainsAnySwitches(["-extra", "-all"]);

            // Check to see if we're in markdown mode
            if (showMarkdown)
            {
                // Prepare the appropriate flags
                HelpCommandType commandTypes = HelpCommandType.None;
                if (showGeneral)
                    commandTypes |= HelpCommandType.General;
                if (showAlias)
                    commandTypes |= HelpCommandType.Aliases;
                if (showUnified)
                    commandTypes |= HelpCommandType.Unified;
                if (showExtra)
                    commandTypes |= HelpCommandType.Extras;

                // Show the markdown representation
                string exported = HelpExporter.ExportToMarkdown(ShellManager.CurrentShellType, commandTypes, showCount, showHidden);
                TextWriterRaw.WritePlain(exported);
            }
            else
            {
                // Now, show the help
                if (string.IsNullOrWhiteSpace(parameters.ArgumentsText))
                    HelpPrint.ShowHelpExtended(useSimplified, showGeneral, showAlias, showUnified, showExtra, showCount, showHidden);
                else
                    HelpPrint.ShowHelpExtended(parameters.ArgumentsList[0], useSimplified);
            }
            return 0;
        }

    }
}
