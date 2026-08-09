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
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Checks suffix in the string
    /// </summary>
    class ChkSuffixCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "chksuffix";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_CHKSUFFIX_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                    }),
                    new CommandArgumentPart(true, "suffix", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SUFFIX_DESC"
                    }),
                ],
                [
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
            bool print = parameters.ContainsSwitch("-verbose");

            // Text and suffix to process
            string text = parameters.ArgumentsList[0];
            string suffix = parameters.ArgumentsList[1];

            // Set the MESH variable to contain the result
            int errorCode = text.VerifySuffix(suffix) ? 0 : 1;
            variableValue = errorCode.ToString();
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return errorCode;
        }
    }
}
