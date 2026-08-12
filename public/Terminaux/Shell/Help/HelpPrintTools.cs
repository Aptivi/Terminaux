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
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Shell.Help
{
    internal static class HelpPrintTools
    {
        internal static void ShowCommandList(string commandType, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false, bool showHidden = false)
        {
            // Get general commands
            var shellInfo = ShellManager.GetShellInfo(commandType);
            var commands = CommandManager.GetCommands(commandType);
            var commandList = shellInfo.Commands;

            // Add every command from each extra and alias
            var ExtraCommandList = shellInfo.extraCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetAliasListFromType(commandType);
            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_AVAILABLECMDS") + (showCount ? " [{0}]" : ""), ThemeColorType.ListTitle, commands.Length);

            // The unified commands
            if (showUnified && unifiedCommandList.Length > 0)
            {
                TextWriterColor.Write("  " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_UNIFIEDCMDS") + (showCount ? " [{0}]" : ""), ThemeColorType.ListTitle, unifiedCommandList.Length);
                ShowCommandListInternal(unifiedCommandList, showHidden, showCount);
                if (showGeneral && commandList.Length > 0)
                    TextWriterRaw.Write();
            }

            // The built-in commands
            if (showGeneral && commandList.Length > 0)
            {
                TextWriterColor.Write("  " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_GENERALCMDS") + (showCount ? " [{0}]" : ""), ThemeColorType.ListTitle, commandList.Length);
                ShowCommandListInternal(commandList, showHidden, showCount);
                if (showExtra && ExtraCommandList.Count > 0)
                    TextWriterRaw.Write();
            }

            // The extra commands
            if (showExtra && ExtraCommandList.Count > 0)
            {
                TextWriterColor.Write("  " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXTRACMDS") + (showCount ? " [{0}]" : ""), ThemeColorType.ListTitle, ExtraCommandList.Count);
                ShowCommandListInternal([.. ExtraCommandList], showHidden, showCount);
                if (showAlias && AliasedCommandList.Count > 0)
                    TextWriterRaw.Write();
            }

            // The alias commands
            if (showAlias && AliasedCommandList.Count > 0)
            {
                TextWriterColor.Write("  " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_ALIASCMDS") + (showCount ? " [{0}]" : ""), ThemeColorType.ListTitle, AliasedCommandList.Count);
                ShowAliasCommandListInternal(AliasedCommandList, showHidden, showCount);
            }
        }

        internal static void ShowCommandListSimplified(string commandType, bool showHidden = false)
        {
            // Get visible commands
            string[] commands = [.. CommandManager.GetCommands(commandType).Where((ci) => !ci.Flags.HasFlag(CommandFlags.Hidden) && !showHidden).Select((ci) => ci.Command)];
            TextWriterColor.Write(string.Join(", ", commands), ThemeColorType.ListTitle);
        }

        internal static void ShowHelpUsage(string command, string commandType)
        {
            // Determine command type
            var shellInfo = ShellManager.GetShellInfo(commandType);
            var CommandList = shellInfo.Commands;

            // Add every command from each mod, extra, and alias
            var ExtraCommandList = shellInfo.extraCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);
            var totalCommandList = CommandManager.GetCommands(commandType);

            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command) &&
                (CommandList.Any((ci) => ci.Command == command) ||
                 AliasedCommandList.Any((info) => info.Key.Alias == command) ||
                 ExtraCommandList.Any((ci) => ci.Command == command) ||
                 unifiedCommandList.Any((ci) => ci.Command == command)))
            {
                // Found!
                bool IsAlias = AliasedCommandList.Any((info) => info.Key.Alias == command);
                bool IsExtra = ExtraCommandList.Any((ci) => ci.Command == command);
                bool IsUnified = unifiedCommandList.Any((ci) => ci.Command == command);
                var FinalCommandList =
                    IsExtra ? ExtraCommandList.ToDictionary((info) => info.Command, (info) => info) :
                    IsAlias ? AliasedCommandList.ToDictionary((info) => info.Key.Command, (info) => info.Key.TargetCommand) :
                    IsUnified ? unifiedCommandList.ToDictionary((info) => info.Command, (info) => info) :
                    CommandList.ToDictionary((info) => info.Command, (info) => info);
                string FinalCommand =
                    IsExtra || IsUnified ? command :
                    IsAlias ? AliasManager.GetAlias(command, commandType).Command :
                    command;
                string HelpDefinition = LanguageTools.GetLocalized(FinalCommandList[FinalCommand].HelpDefinition);

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_CMDDEFINEDBY") + $" {command}";
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_HELP_CMD"), FinalCommand, needsIndent: false);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_DESC"), HelpDefinition, needsIndent: false);

                // Iterate through command argument information instances
                var argumentInfos = FinalCommandList[FinalCommand].CommandArgumentInfo ?? [];
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
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("T_SHELL_BASE_HELP_USAGEINFO_USAGE"), $"{FinalCommand} {renderedUsage}", needsIndent: false);

                    // If we have arguments, print their descriptions
                    if (Arguments.Length != 0)
                    {
                        TextWriterRaw.Write();
                        TextWriterColor.Write("* " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_ARGSLIST"), ThemeColorType.ListTitle);
                        foreach (var argument in Arguments)
                        {
                            string argumentName = argument.ArgumentExpression;
                            string argumentDesc = LanguageTools.GetLocalized(argument.Options.ArgumentDescription);
                            if (string.IsNullOrWhiteSpace(argumentDesc))
                                argumentDesc = LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_ARGDESCUNSPECIFIED");
                            ListEntryWriterColor.WriteListEntry(argumentName, argumentDesc, indent: 1, needsIndent: false);
                        }
                    }

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriterRaw.Write();
                        TextWriterColor.Write("* " + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_SWITCHESLIST"), ThemeColorType.ListTitle);
                        foreach (var Switch in Switches)
                        {
                            string switchName = Switch.SwitchName;
                            string switchDesc = LanguageTools.GetLocalized(Switch.HelpDefinition);
                            if (string.IsNullOrWhiteSpace(switchDesc))
                                switchDesc = LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_SWITCHDESCUNSPECIFIED");
                            ListEntryWriterColor.WriteListEntry($"-{switchName}", switchDesc, indent: 1, needsIndent: false);
                        }
                    }
                }

                // Extra help action for some commands
                FinalCommandList[FinalCommand].HelpHelper(shellInfo.ShellBase);
            }
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_CMDNOHELP"), ThemeColorType.Error, command);
        }

        private static void ShowCommandListInternal(BaseCommand[] commands, bool showHidden, bool showCount)
        {
            int hiddenProcessed = 0;
            foreach (var cmd in commands)
            {
                if (cmd.Flags.HasFlag(CommandFlags.Hidden) && !showHidden)
                {
                    hiddenProcessed++;
                    continue;
                }
                string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                string commandEntry = "{0}{1}".FormatString(cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                ListEntryWriterColor.WriteListEntry(commandEntry, LanguageTools.GetLocalized(cmd.HelpDefinition), indent: 1);
            }
            if (hiddenProcessed > 0 && showCount)
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_HIDDENCMDS"), ThemeColorType.Tip, hiddenProcessed);
        }

        private static void ShowAliasCommandListInternal(List<AliasInfo> aliases, bool showHidden, bool showCount)
        {
            int hiddenProcessed = 0;
            foreach (var alias in aliases)
            {
                var cmd = alias.TargetCommand;
                if (cmd.Flags.HasFlag(CommandFlags.Hidden) && !showHidden)
                {
                    hiddenProcessed++;
                    continue;
                }
                string[] usages = [.. cmd.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                string aliasEntry = "{0} -> {1}{2}".FormatString(alias.Alias, cmd.Command, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                ListEntryWriterColor.WriteListEntry(aliasEntry, LanguageTools.GetLocalized(cmd.HelpDefinition), indent: 1);
            }
            if (hiddenProcessed > 0 && showCount)
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_HIDDENCMDS"), ThemeColorType.Tip, hiddenProcessed);
        }
    }
}
