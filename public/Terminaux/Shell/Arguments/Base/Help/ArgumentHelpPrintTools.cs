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
using System.Collections.Generic;
using Terminaux.Colors.Data;
using Terminaux.Shell.Switches;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;

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
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_NOHELP_ARG"), ThemeColorType.Error, argument);
                return;
            }

            // Now, populate usages for each argument
            string HelpDefinition = argInfo.HelpDefinition;
            var argumentInfos = argInfo.ArgArgumentInfo;
            ConsoleLogger.Debug("Showing usage of {0} with {1} argument info instances", argument, argumentInfos.Length);
            foreach (var argumentInfo in argumentInfos)
            {
                if (argumentInfo is null)
                    continue;
                CommandArgumentPart[] Arguments = argumentInfo.Arguments;
                SwitchInfo[] Switches = argumentInfo.Switches;

                // Print usage information
                ConsoleLogger.Debug("Found {0} arguments and {1} switches", Arguments.Length, Switches.Length);
                if (Arguments.Length != 0 || Switches.Length != 0)
                {
                    // Print the usage information holder
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE") + $": {argument}", false, ThemeColorType.ListEntry);

                    // Enumerate through the available switches first
                    foreach (var Switch in Switches)
                    {
                        bool required = Switch.IsRequired;
                        string switchName = Switch.SwitchName;
                        string renderedSwitch = required ? $" <-{switchName}[=value]>" : $" [-{switchName}[=value]]";
                        ConsoleLogger.Debug("Switch {0} ({1}) rendered as {2}", switchName, required, renderedSwitch);
                        TextWriterColor.Write(renderedSwitch, false, ThemeColorType.ListEntry);
                    }

                    // Enumerate through the available arguments
                    int howManyRequired = argumentInfo.MinimumArguments;
                    int queriedArgs = 1;
                    foreach (var argumentPart in Arguments)
                    {
                        bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                        string renderedArgument = required ? $" <{argumentPart.ArgumentExpression}>" : $" [{argumentPart.ArgumentExpression}]";
                        ConsoleLogger.Debug("Argument {0} ({1}) rendered as {2}", argumentPart.ArgumentExpression, required, renderedArgument);
                        TextWriterColor.Write(renderedArgument, false, ThemeColorType.ListEntry);
                    }
                    TextWriterRaw.Write();
                }
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE") + $": {argument}", true, ThemeColorType.ListEntry);
            }

            // Write the description now
            if (string.IsNullOrEmpty(HelpDefinition))
            {
                ConsoleLogger.Warning("No argument help description for {0}", argument);
                HelpDefinition = LanguageTools.GetLocalized("T_SHELL_BASE_HELP_NOHELPDESC_ARG");
            }
            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_DESC") + $": {HelpDefinition}", true, ThemeColorType.ListValue);
            argInfo.ArgumentBase.HelpHelper();
        }
    }
}
