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
using Textify.SpaceManager.Analysis;
using Textify.SpaceManager.Conversion;

namespace Terminaux.Shell.Shells.Unified
{
    /// <summary>
    /// Normalizes spaces in a string
    /// </summary>
    class NormalizeSpacesCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "normalizespaces";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("T_SHELL_UNIFIED_NORMALIZESPACES_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "text", new()
                    {
                        ArgumentDescription = /* Localizable */ "T_SHELL_UNIFIED_ARGUMENT_TEXT_DESC"
                    }),
                ],
                [
                    new SwitchInfo("simple", /* Localizable */ "T_SHELL_UNIFIED_NORMALIZESPACES_SWITCH_SIMPLE_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["analytical"],
                        AcceptsValues = false
                    }),
                    new SwitchInfo("analytical", /* Localizable */ "T_SHELL_UNIFIED_NORMALIZESPACES_SWITCH_ANALYTICAL_DESC", new SwitchOptions()
                    {
                        ConflictsWith = ["simple"],
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
            bool simple = parameters.ContainsSwitch("-simple");
            bool analytical = parameters.ContainsSwitch("-analytical");
            bool print = parameters.ContainsSwitch("-verbose");

            // Text to process
            string text = parameters.ArgumentsList[0];

            // Set the MESH variable to contain the result
            string result = "";
            if (simple)
                result = SpaceConversionTools.ConvertSpacesSimple(text);
            else
            {
                var analysis = SpaceAnalysisTools.AnalyzeSpaces(text);
                result = SpaceConversionTools.ConvertSpacesToString(analysis);
            }
            variableValue = result;
            if (print)
                TextWriterRaw.WritePlain(variableValue);
            return 0;
        }
    }
}
