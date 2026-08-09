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

using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Replaces a substring with a substring
    /// </summary>
    class ReplaceCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "replace";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_REPLACE_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                    }),
                    new CommandArgumentPart(true, "toreplace", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_REPLACE_ARGUMENT_TOREPLACE_DESC"
                    }),
                    new CommandArgumentPart(true, "replacewith", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_REPLACEWITH_DESC"
                    }),
                ],
                [
                    new SwitchInfo("regex", /* Localizable */ "T_SHELL_UNIFIED_REPLACE_SWITCH_REGEX_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ], true)
            ];

        public override CommandFlags Flags => 
            CommandFlags.Hidden;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool regex = parameters.ContainsSwitch("-regex");
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string text = parameters.ArgumentsList[0];
            string toReplace = parameters.ArgumentsList[1];
            string replaceWith = parameters.ArgumentsList[2];

            // Set the MESH variable to contain the result
            string processed = regex ?
                Regex.Replace(text, toReplace, replaceWith) :
                text.Replace(toReplace, replaceWith);
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
