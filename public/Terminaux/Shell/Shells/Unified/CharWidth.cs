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
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Gets the character width
    /// </summary>
    class CharWidthCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "charwidth";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_CHARWIDTH_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "char", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_CHAR_DESC"
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

            // Character to process
            string text = parameters.ArgumentsList[0];
            var wideChars = text.GetWideChars();

            // Set the MESH variable to contain the result
            int charWidth = wideChars.Length == 0 ? 0 : wideChars[0].GetWidth();
            variableValue = charWidth.ToString();
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
