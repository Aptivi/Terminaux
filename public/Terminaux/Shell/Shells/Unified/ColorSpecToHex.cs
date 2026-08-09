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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Colorimetry;
using Terminaux.Shell.Arguments;
using Terminaux.Base;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Converts the color specifier to hex.
    /// </summary>
    /// <remarks>
    /// If you want to get the target color model representation in hex from the source color model specifier, you can use this command.
    /// </remarks>
    class ColorSpecToHexCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "colorspectohex";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_COMMAND_COLORSPECTOHEX_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "specifier", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_COMMAND_ARGUMENT_SPECIFIER_DESC"
                    }),
                ], true)
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Do the job
            var color = new Color(parameters.ArgumentsList[0]).Hex;
            TextWriterColor.Write(color, ThemeColorType.NeutralText);
            variableValue = color;
            return 0;
        }

    }
}
