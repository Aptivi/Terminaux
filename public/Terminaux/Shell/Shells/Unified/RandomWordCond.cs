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
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.Words;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Generates a random word conditionally
    /// </summary>
    class RandomWordCondCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "randomwordcond";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_RANDOMWORDCOND_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "maxLength", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_MAXLENGTH_DESC",
                        IsNumeric = true,
                    }),
                    new CommandArgumentPart(false, "startsWith", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_STARTSWITH_DESC"
                    }),
                    new CommandArgumentPart(false, "endsWith", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_ENDSWITH_DESC"
                    }),
                    new CommandArgumentPart(false, "exactLength", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RANDOMWORDCOND_ARGUMENT_EXACTLENGTH_DESC",
                        IsNumeric = true,
                    }),
                ],
                [
                    new SwitchInfo("quiet", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_QUIET_DESC", new SwitchOptions()
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
            bool print = !parameters.ContainsSwitch("-quiet");

            // Conditions to process
            string maxLengthStr = parameters.ArgumentsList.Length > 0 ? parameters.ArgumentsList[0] : "0";
            string startsWith = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            string endsWith = parameters.ArgumentsList.Length > 2 ? parameters.ArgumentsList[2] : "";
            string exactLengthStr = parameters.ArgumentsList.Length > 3 ? parameters.ArgumentsList[3] : "0";
            if (!int.TryParse(maxLengthStr, out var maxLength))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_RANDOMWORDCOND_MAXLENGTHINVALID"), ThemeColorType.Error);
                return 1;
            }
            if (!int.TryParse(exactLengthStr, out var exactLength))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_RANDOMWORDCOND_EXACTLENGTHINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            string processed = WordManager.GetRandomWordConditional(maxLength, startsWith, endsWith, exactLength);
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
