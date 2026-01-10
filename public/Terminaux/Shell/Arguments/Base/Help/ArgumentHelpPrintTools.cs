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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Shell.Arguments.Base.Help
{
    internal static class ArgumentHelpPrintTools
    {
        internal static void ShowArgumentList(Dictionary<string, ArgumentInfo> arguments)
        {
            foreach (string arg in arguments.Keys)
            {
                var argumentInstance = arguments[arg];
                string[] usages = [.. argumentInstance.ArgArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                TextWriterRaw.WriteRaw(new ListEntry()
                {
                    Entry = "  --{0}{1}".FormatString(argumentInstance.Argument, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : ""),
                    Value = LanguageTools.GetLocalized(argumentInstance.HelpDefinition),
                    Indicator = false,
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
            
            // Write the description now
            string HelpDefinition = LanguageTools.GetLocalized(argInfo.HelpDefinition);
            if (string.IsNullOrEmpty(HelpDefinition))
            {
                ConsoleLogger.Warning("No argument help description for {0}", argument);
                HelpDefinition = LanguageTools.GetLocalized("T_SHELL_BASE_HELP_NOHELPDESC_ARG");
            }
            TextWriterRaw.WriteRaw(new ListEntry()
            {
                Entry = LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_DESC"),
                Value = HelpDefinition,
                Indicator = false,
            }.Render() + "\n");

            // Now, populate usages for each argument
            var argumentInfos = argInfo.ArgArgumentInfo;
            ConsoleLogger.Debug("Showing usage of {0} with {1} argument info instances", argument, argumentInfos.Length);
            foreach (var argumentInfo in argumentInfos)
            {
                var Arguments = Array.Empty<CommandArgumentPart>();
                var Switches = Array.Empty<SwitchInfo>();
                string renderedUsage = "";

                // Populate help usages
                if (argumentInfo is not null)
                {
                    Arguments = argumentInfo.Arguments;
                    Switches = argumentInfo.Switches;
                    renderedUsage = argumentInfo.RenderedUsage;
                }

                // Print usage information
                TextWriterRaw.Write();
                TextWriterRaw.WriteRaw(new ListEntry()
                {
                    Entry = LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE"),
                    Value = $"--{argument} {renderedUsage}",
                    Indicator = false,
                }.Render() + "\n");

                // If we have arguments, print their descriptions
                if (Arguments.Length != 0)
                {
                    TextWriterColor.Write("* " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_ARGSLIST"), ThemeColorType.ListTitle);
                    foreach (var argumentPart in Arguments)
                    {
                        string argumentName = argumentPart.ArgumentExpression;
                        string argumentDesc = LanguageTools.GetLocalized(argumentPart.Options.ArgumentDescription);
                        if (string.IsNullOrWhiteSpace(argumentDesc))
                            argumentDesc = LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_ARGDESCUNSPECIFIED");
                        TextWriterRaw.WriteRaw(new ListEntry()
                        {
                            Entry = argumentName,
                            Value = argumentDesc,
                            Indentation = 1,
                            Indicator = false,
                        }.Render() + "\n");
                    }
                }

                // If we have switches, print their descriptions
                if (Switches.Length != 0)
                {
                    TextWriterColor.Write("* " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_SWITCHESLIST"), ThemeColorType.ListTitle);
                    foreach (var Switch in Switches)
                    {
                        string switchName = Switch.SwitchName;
                        string switchDesc = LanguageTools.GetLocalized(Switch.HelpDefinition);
                        if (string.IsNullOrWhiteSpace(switchDesc))
                            switchDesc = LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_SWITCHDESCUNSPECIFIED");
                        TextWriterRaw.WriteRaw(new ListEntry()
                        {
                            Entry = $"-{switchName}",
                            Value = switchDesc,
                            Indentation = 1,
                            Indicator = false,
                        }.Render() + "\n");
                    }
                }
            }

            // Extra help action for some arguments
            argInfo.ArgumentBase.HelpHelper();
        }
    }
}
