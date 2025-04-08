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

using System;
using System.Collections.Generic;
using System.Linq;
using Textify.General;
using Terminaux.Shell.Shells;
using System.Diagnostics.CodeAnalysis;
using Terminaux.Base;
using Textify.Tools;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command management class
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// Checks to see if the command is found in selected shell command type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static bool IsCommandFound(string Command, string ShellType)
        {
            ConsoleLogger.Debug("Command: {0}, ShellType: {1}", Command, ShellType);
            return GetCommands(ShellType).Any((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
        }

        /// <summary>
        /// Checks to see if the command is found in all the shells
        /// </summary>
        /// <param name="Command">A command</param>
        /// <returns>True if found; False if not found.</returns>
        public static bool IsCommandFound(string Command)
        {
            ConsoleLogger.Debug("Checking command: {0}", Command);
            bool found = false;
            foreach (var ShellType in ShellManager.AvailableShells.Keys)
            {
                found = GetCommands(ShellType).Any((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
                if (found)
                    break;
            }
            return found;
        }

        /// <summary>
        /// Gets the command list according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] GetCommands(string ShellType)
        {
            // Individual shells
            var shellInfo = ShellManager.GetShellInfo(ShellType);
            var extraCommands = ShellManager.GetShellInfo(ShellType).extraCommands;
            List<CommandInfo> FinalCommands = shellInfo.Commands;

            // Unified commands
            foreach (var UnifiedCommand in ShellManager.UnifiedCommands)
            {
                if (!FinalCommands.Contains(UnifiedCommand))
                    FinalCommands.Add(UnifiedCommand);
            }

            // Extra commands
            foreach (var ExtraCommand in extraCommands)
            {
                if (!FinalCommands.Contains(ExtraCommand))
                    FinalCommands.Add(ExtraCommand);
            }

            return [.. FinalCommands];
        }

        /// <summary>
        /// Gets the command names according to the shell type
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static string[] GetCommandNames(string ShellType)
        {
            // Return command names
            string[] FinalCommands = GetCommands(ShellType).Select((ci) => ci.Command).ToArray();
            return FinalCommands;
        }

        /// <summary>
        /// Gets the command list according to the shell type by searching for the partial command name
        /// </summary>
        /// <param name="namePattern">A valid regex pattern for command name</param>
        /// <param name="ShellType">The shell type</param>
        public static CommandInfo[] FindCommands([StringSyntax(StringSyntaxAttribute.Regex)] string namePattern, string ShellType)
        {
            // Verify that the provided regex is valid
            if (!RegexTools.IsValidRegex(namePattern))
                throw new TerminauxException("Invalid command pattern provided.");

            // Get all the commands first
            var allCommands = GetCommands(ShellType);

            // Now, find the commands that match the specified regex pattern.
            var foundCommands = allCommands
                .Where((info) => RegexTools.IsMatch(info.Command, namePattern))
                .ToArray();
            return foundCommands;
        }

        /// <summary>
        /// Gets a command, specified by the shell type
        /// </summary>
        /// <param name="Command">A command</param>
        /// <param name="ShellType">The shell type name</param>
        /// <returns>True if found; False if not found or shell type is invalid.</returns>
        public static CommandInfo GetCommand(string Command, string ShellType)
        {
            ConsoleLogger.Debug("Command: {0}, ShellType: {1}", Command, ShellType);
            var commandList = GetCommands(ShellType);
            if (!IsCommandFound(Command, ShellType))
                throw new TerminauxException("Command not found.");
            return commandList.Single((ci) => ci.Command == Command || ci.Aliases.Any((ai) => ai.Alias == Command));
        }

        /// <summary>
        /// Registers a command to the mod command list
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandBase">Custom command base to register</param>
        public static void RegisterCustomCommand(string ShellType, CommandInfo? commandBase)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new TerminauxException("Shell type {0} doesn't exist.", ShellType);
            if (commandBase is null)
                throw new TerminauxException("You must provide the command base.");
            string command = commandBase.Command;
            ConsoleLogger.Debug("Trying to register {0}, ShellType: {1}", command, ShellType);

            // Check the command name
            if (string.IsNullOrEmpty(command))
                throw new TerminauxException("You must provide the command.");

            // Check to see if the command conflicts with pre-existing shell commands
            if (IsCommandFound(command, ShellType))
            {
                ConsoleLogger.Error("Command {0} conflicts with available shell commands.", command);
                throw new TerminauxException("The command specified is already added.");
            }

            // Check to see if the help definition is full
            if (string.IsNullOrEmpty(commandBase.HelpDefinition))
            {
                ConsoleLogger.Warning("No definition, {0}.Def = \"Command not defined\"", command);
                commandBase.HelpDefinition = "Command not defined";
            }

            // Now, add the command to the extra list
            ConsoleLogger.Debug("Adding command {0} for {1}...", command, ShellType);
            var info = ShellManager.GetShellInfo(ShellType);
            if (!info.extraCommands.Contains(commandBase))
                info.extraCommands.Add(commandBase);
            ConsoleLogger.Debug("Registered {0}, ShellType: {1}", command, ShellType);
        }

        /// <summary>
        /// Registers a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandBases">Custom command bases to register</param>
        public static void RegisterCustomCommands(string ShellType, CommandInfo[] commandBases)
        {
            List<string> failedCommands = [];
            foreach (var commandBase in commandBases)
            {
                try
                {
                    RegisterCustomCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"Can't register custom command: {ex.Message}");
                    failedCommands.Add($"  - {(commandBase is not null ? commandBase.Command : "???")}: {ex.Message}");
                }
            }
            if (failedCommands.Count > 0)
                throw new TerminauxException("Some of the custom commands can't be loaded." + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }

        /// <summary>
        /// Unregisters a custom command
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandName">Custom command name to unregister</param>
        public static void UnregisterCustomCommand(string ShellType, string commandName)
        {
            // First, check the values
            if (!ShellManager.ShellTypeExists(ShellType))
                throw new TerminauxException("Shell type {0} doesn't exist.", ShellType);
            if (string.IsNullOrEmpty(commandName))
                throw new TerminauxException("You must provide the command.");

            // Check to see if we have this command
            if (!GetCommandNames(ShellType).Contains(commandName))
                throw new TerminauxException("The custom command specified is not found.");
            else
            {
                // We have the command. Remove it.
                var cmd = GetCommand(commandName, ShellType);
                var info = ShellManager.GetShellInfo(ShellType);
                info.extraCommands.Remove(cmd);
            }
        }

        /// <summary>
        /// Unregisters a group of custom commands
        /// </summary>
        /// <param name="ShellType">Type of a shell, including your custom type and other mod's custom type to extend it</param>
        /// <param name="commandNames">Custom command names to unregister</param>
        public static void UnregisterCustomCommands(string ShellType, string[] commandNames)
        {
            List<string> failedCommands = [];
            foreach (string commandBase in commandNames)
            {
                try
                {
                    UnregisterCustomCommand(ShellType, commandBase);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"Can't unregister custom command: {ex.Message}");
                    failedCommands.Add($"  - {(!string.IsNullOrEmpty(commandBase) ? commandBase : "???")}: {ex.Message}");
                }
            }
            if (failedCommands.Count > 0)
                throw new TerminauxException("Some of the custom commands can't be unloaded." + CharManager.NewLine + string.Join(CharManager.NewLine, failedCommands));
        }
    }
}
