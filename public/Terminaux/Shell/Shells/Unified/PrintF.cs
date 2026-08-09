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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using System.Linq;
using Textify.General;
using Terminaux.Shell.Arguments;
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Prints the text with formatting
    /// </summary>
    class PrintFCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "printf";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_PRINTF_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                    }),
                    new CommandArgumentPart(false, "parameters", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_PRINTF_ARGUMENT_PARAMETERS_DESC"
                    }),
                ], true, true)
            ];

        public override CommandFlags Flags => 
            CommandFlags.Hidden;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Text to process
            string text = parameters.ArgumentsList[0];
            string[] formatting = [.. parameters.ArgumentsList.Skip(1)];

            // Set the MESH variable to contain the result
            string formatted = text.FormatString(formatting);
            TextWriterColor.Write(formatted);
            variableValue = formatted;
            return 0;
        }
    }
}
