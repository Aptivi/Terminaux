//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using System;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Colors.Data;
using Terminaux.Shell.Switches;
using Terminaux.Base;

namespace Terminaux.Shell.Arguments.Base.Help
{
    internal static class ArgumentHelpPrintTools
    {
        internal static void ShowArgumentList(Dictionary<string, ArgumentInfo> arguments)
        {
            foreach (string arg in arguments.Keys)
            {
                string entry = arguments[arg].HelpDefinition;
                TextWriterRaw.WriteRaw(new ListEntry()
                {
                    Entry = arg,
                    Value = entry,
                }.Render() + "\n");
            }
        }

        internal static void ShowArgumentListSimple(Dictionary<string, ArgumentInfo> arguments) =>
            TextWriterColor.Write(string.Join(", ", arguments.Keys));

        internal static void ShowHelpUsage(string argument, Dictionary<string, ArgumentInfo> arguments)
        {
            // Check to see if we have this argument
            if (!arguments.TryGetValue(argument, out ArgumentInfo? argInfo))
            {
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_NOHELP_ARG"), ConsoleColors.Red, argument);
                return;
            }

            // Now, populate usages for each argument
            string HelpDefinition = argInfo.HelpDefinition;
            var argumentInfos = argInfo.ArgArgumentInfo;
            foreach (var argumentInfo in argumentInfos)
            {
                var Arguments = Array.Empty<CommandArgumentPart>();
                var Switches = Array.Empty<SwitchInfo>();

                // Populate help usages
                if (argumentInfo is not null)
                {
                    Arguments = argumentInfo.Arguments;
                    Switches = argumentInfo.Switches;
                }
                else
                    continue;

                // Print usage information
                if (Arguments.Length != 0 || Switches.Length != 0)
                {
                    // Print the usage information holder
                    TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE") + $": {argument}", false, ConsoleColors.Yellow);

                    // Enumerate through the available switches first
                    foreach (var Switch in Switches)
                    {
                        bool required = Switch.IsRequired;
                        string switchName = Switch.SwitchName;
                        string renderedSwitch = required ? $" <-{switchName}[=value]>" : $" [-{switchName}[=value]]";
                        TextWriterColor.WriteColor(renderedSwitch, false, ConsoleColors.Yellow);
                    }

                    // Enumerate through the available arguments
                    int howManyRequired = argumentInfo.MinimumArguments;
                    int queriedArgs = 1;
                    foreach (var argumentPart in Arguments)
                    {
                        bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                        string renderedArgument = required ? $" <{argumentPart.ArgumentExpression}>" : $" [{argumentPart.ArgumentExpression}]";
                        TextWriterColor.WriteColor(renderedArgument, false, ConsoleColors.Yellow);
                    }
                    TextWriterRaw.Write();
                }
                else
                    TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE") + $": {argument}", true, ConsoleColors.Yellow);
            }

            // Write the description now
            if (string.IsNullOrEmpty(HelpDefinition))
            {
                ConsoleLogger.Warning("No argument help description for {0}", argument);
                HelpDefinition = LanguageTools.GetLocalized("T_SHELL_BASE_HELP_NOHELPDESC_ARG");
            }
            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_DESC") + $": {HelpDefinition}", true, ConsoleColors.Olive);
            argInfo.ArgumentBase.HelpHelper();
        }
    }
}
