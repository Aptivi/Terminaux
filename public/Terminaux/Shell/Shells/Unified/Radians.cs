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
using Terminaux.Base;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Converts the angle from degrees to radians
    /// </summary>
    class RadiansCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "radians";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_RADIANS_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "degrees", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_RADIANS_ARGUMENT_DEGREES_DESC",
                        IsNumeric = true,
                    }),
                ],
                [
                    new SwitchInfo("verbose", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_VERBOSE_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Flags
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string angleStr = parameters.ArgumentsList[0];
            if (!double.TryParse(angleStr, out var angle))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_DEGREES_ANGLEINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Calculate the radians
            double radians = angle * (Math.PI / 180);

            // Set the MESH variable to contain the result
            string processed = $"{radians}";
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
