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

using System;
using System.Linq;
using Terminaux.Base;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.Words.Profanity;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Finds profanity in a string
    /// </summary>
    class FindProfanityCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "findprofanity";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_FINDPROFANITY_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                    }),
                    new CommandArgumentPart(false, "profanitytype", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_FINDPROFANITY_ARGUMENT_PROFANITYTYPE_DESC",
                        AutoCompleter = (_) => Enum.GetNames(typeof(ProfanitySearchType)),
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

            // Text to process
            string text = parameters.ArgumentsList[0];
            string profanityType = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "Shallow";
            if (!Enum.TryParse(profanityType, out ProfanitySearchType profanitySearchType))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_FINDPROFANITY_PROFANITYTYPEINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            var occurrences = ProfanityManager.GetProfanities(text, profanitySearchType);
            string[] profanities = [.. occurrences.Select((poi) => $"({poi.ProfaneWord}, {poi.ProfaneIndex})")];
            variableValue = string.Join("\n", profanities);
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
