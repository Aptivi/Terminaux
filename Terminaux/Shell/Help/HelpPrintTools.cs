//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Aliases;

namespace Terminaux.Shell.Help
{
    internal static class HelpPrintTools
    {
        internal static void ShowCommandList(string commandType, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false)
        {
            // Get general commands
            var commands = CommandManager.GetCommands(commandType);
            var commandList = ShellManager.GetShellInfo(commandType).Commands;

            // Add every command from each extra and alias
            var ExtraCommandList = ShellManager.GetShellInfo(commandType).extraCommands;
            var unifiedCommandList = ShellManager.unifiedCommandDict;
            var AliasedCommandList = AliasManager.GetAliasListFromType(commandType)
                .ToDictionary((ai) => ai, (ai) => ai.TargetCommand);
            TextWriterRaw.WritePlain("Available commands:" + (showCount ? " [{0}]" : ""), commands.Length);

            // The built-in commands
            if (showGeneral)
            {
                TextWriterRaw.WritePlain(CharManager.NewLine + "General commands:" + (showCount ? " [{0}]" : ""), commandList.Count);
                if (commandList.Count == 0)
                    TextWriterRaw.WritePlain("  - No shell commands.");
                foreach (var cmd in commandList)
                {
                    TextWriterRaw.WritePlain("  - {0}: ", false, cmd.Command);
                    TextWriterRaw.WritePlain("{0}", cmd.HelpDefinition);
                }
            }

            // The extra commands
            if (showExtra)
            {
                TextWriterRaw.WritePlain(CharManager.NewLine + "Extra commands:" + (showCount ? " [{0}]" : ""), ExtraCommandList.Count);
                if (ExtraCommandList.Count == 0)
                    TextWriterRaw.WritePlain("  - No extra commands.");
                foreach (var cmd in ExtraCommandList)
                {
                    TextWriterRaw.WritePlain("  - {0}: ", false, cmd.Command);
                    TextWriterRaw.WritePlain("{0}", cmd.HelpDefinition);
                }
            }

            // The alias commands
            if (showAlias)
            {
                TextWriterRaw.WritePlain(CharManager.NewLine + "Alias commands:" + (showCount ? " [{0}]" : ""), AliasedCommandList.Count);
                if (AliasedCommandList.Count == 0)
                    TextWriterRaw.WritePlain("  - No alias commands.");
                foreach (var cmd in AliasedCommandList)
                {
                    TextWriterRaw.WritePlain("  - {0} -> {1}: ", false, cmd.Key.Alias, cmd.Value.Command);
                    TextWriterRaw.WritePlain("{0}", cmd.Value.HelpDefinition);
                }
            }

            // The unified commands
            if (showUnified)
            {
                TextWriterRaw.WritePlain(CharManager.NewLine + "Unified commands:" + (showCount ? " [{0}]" : ""), unifiedCommandList.Count);
                if (unifiedCommandList.Count == 0)
                    TextWriterRaw.WritePlain("  - No unified commands.");
                foreach (var cmd in unifiedCommandList)
                {
                    TextWriterRaw.WritePlain("  - {0}: ", false, cmd.Command);
                    TextWriterRaw.WritePlain("{0}", cmd.HelpDefinition);
                }
            }
        }

        internal static void ShowCommandListSimplified(string commandType)
        {
            // Get visible commands
            var commands = CommandManager.GetCommandNames(commandType);
            TextWriterColor.Write(string.Join(", ", commands));
        }

        internal static void ShowHelpUsage(string command, string commandType)
        {
            // Determine command type
            var CommandList = ShellManager.GetShellInfo(commandType).Commands;

            // Add every command from each mod, extra, and alias
            var ExtraCommandList = ShellManager.GetShellInfo(commandType).extraCommands;
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
                string HelpDefinition = FinalCommandList[FinalCommand].HelpDefinition;

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = "Command defined by " + command;
                TextWriterRaw.WritePlain("Command:", false);
                TextWriterRaw.WritePlain($" {FinalCommand}");
                TextWriterRaw.WritePlain("Description:", false);
                TextWriterRaw.WritePlain($" {HelpDefinition}");

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
                    TextWriterRaw.WritePlain("Usage:", false);
                    TextWriterRaw.WritePlain($" {FinalCommand} {renderedUsage}");

                    // If we have arguments, print their descriptions
                    if (Arguments.Length != 0)
                    {
                        TextWriterRaw.WritePlain("This command has the below arguments that change how it works:");
                        foreach (var argument in Arguments)
                        {
                            string argumentName = argument.ArgumentExpression;
                            string argumentDesc = argument.Options.ArgumentDescription;
                            TextWriterRaw.WritePlain($"  {argumentName}: ", false);
                            TextWriterRaw.WritePlain(argumentDesc);
                        }
                    }

                    // If we have switches, print their descriptions
                    if (Switches.Length != 0)
                    {
                        TextWriterRaw.WritePlain("This command has the below switches that change how it works:");
                        foreach (var Switch in Switches)
                        {
                            string switchName = Switch.SwitchName;
                            string switchDesc = Switch.HelpDefinition;
                            TextWriterRaw.WritePlain($"  -{switchName}: ", false);
                            TextWriterRaw.WritePlain(switchDesc);
                        }
                    }
                }

                // Extra help action for some commands
                FinalCommandList[FinalCommand].CommandBase?.HelpHelper();
            }
            else
                TextWriterRaw.WritePlain("No help for command \"{0}\".", command);
        }
    }
}
