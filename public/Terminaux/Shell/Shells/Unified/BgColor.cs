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

using Colorimetry;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Gets the background color
    /// </summary>
    class BgColorCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "bgcolor";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_BGCOLOR_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "specifier", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_SPECIFIER_DESC"
                    }),
                ],
                [
                    new SwitchInfo("plain", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_PLAIN_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("truevt", /* Localizable */ "T_SHELL_UNIFIED_SWITCH_TRUEVT_DESC", new SwitchOptions()
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
            bool print = parameters.ContainsSwitch("-verbose");
            bool plainMode = parameters.ContainsSwitch("-plain");
            bool trueVtMode = parameters.ContainsSwitch("-truevt");

            // Parse the color instance
            string colorSpecifier = parameters.ArgumentsList[0];
            if (!ColorTools.TryParseColor(colorSpecifier))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_UNIFIED_BGCOLOR_COLORINVALID"), ThemeColorType.Error);
                return 1;
            }

            // Set the MESH variable to contain the result
            var color = new Color(colorSpecifier);
            string plain = color.PlainSequence;
            string processed = trueVtMode ?
                color.VTSequenceBackgroundTrueColor() :
                plainMode ?
                    plain :
                    color.VTSequenceBackground();
            variableValue = processed;
            if (print)
                TextWriterRaw.WritePlain(plain);
            return 0;
        }
    }
}
