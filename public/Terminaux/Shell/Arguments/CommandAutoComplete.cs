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

using System.Linq;
using Terminaux.Base;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Textify.General;

namespace Terminaux.Shell.Arguments
{
    /// <summary>
    /// Command auto completion class
    /// </summary>
    internal static class CommandAutoComplete
    {

        internal static string[] GetSuggestions(string text, int index)
        {
            // First, cut the text to index
            text = text.Substring(0, index);
            ConsoleLogger.Debug("Text to auto complete: {0} (idx: {1})", text, index);

            // Then, check to see if we have shells
            ConsoleLogger.Debug("Shell count: {0}", ShellManager.ShellStack.Count);
            if (ShellManager.ShellStack.Count <= 0)
                return [];

            // Get the commands based on the current shell type
            var shellType = ShellManager.CurrentShellType;
            var ShellCommandNames = CommandManager.GetCommandNames(shellType);
            ConsoleLogger.Debug("Commands count for type {0}: {1}", shellType, ShellCommandNames.Length);

            // If text is not provided, return the command list without filtering
            if (string.IsNullOrEmpty(text))
                return ShellCommandNames;

            // Get the provided command and argument information
            var commandArgumentInfo = ArgumentsParser.ParseShellCommandArguments(text, shellType).total[0];

            // We're providing completion for argument.
            string CommandName = commandArgumentInfo.Command;
            string finalCommandArgs = commandArgumentInfo.ArgumentsText;
            string[] finalCommandArgsEnclosed = finalCommandArgs.SplitEncloseDoubleQuotes();
            int LastArgumentIndex = finalCommandArgsEnclosed.Length - 1;
            string LastArgument = finalCommandArgsEnclosed.Length > 0 ? finalCommandArgsEnclosed[LastArgumentIndex] : "";
            ConsoleLogger.Debug("Command name: {0}", CommandName);
            ConsoleLogger.Debug("Command arguments [{0}]: {1}", finalCommandArgsEnclosed.Length, finalCommandArgs);
            ConsoleLogger.Debug("last argument: {0}", LastArgument);

            // Make a list
            string[] finalCompletions = [];
            if (string.IsNullOrEmpty(finalCommandArgs))
            {
                ConsoleLogger.Debug("Creating list of commands starting with command {0} [{1}]...", CommandName, CommandName.Length);
                finalCompletions = ShellCommandNames
                    .Where(x => x.StartsWith(CommandName))
                    .Select(x => x.Substring(CommandName.Length))
                    .ToArray();
                ConsoleLogger.Debug("Initially invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
            }

            // Check to see if there is such command
            ConsoleLogger.Debug("Command {0} exists? {1}", CommandName, ShellCommandNames.Contains(CommandName));
            if (!ShellCommandNames.Contains(CommandName))
                return finalCompletions;

            // We have the command. Check its entry for argument info
            var cmdInfo = CommandManager.GetCommand(CommandName, shellType);
            var CommandArgumentInfos = cmdInfo.CommandArgumentInfo;
            foreach (var CommandArgumentInfo in CommandArgumentInfos)
            {
                ConsoleLogger.Debug("Command {0} has argument info? {1}", CommandName, CommandArgumentInfo is not null);
                if (CommandArgumentInfo is null)
                    // No arguments. Return nothing
                    return finalCompletions;

                // There are arguments! Now, check to see if it has the accessible auto completer from the last argument
                var AutoCompleter =
                    LastArgumentIndex < CommandArgumentInfo.Arguments.Length && LastArgumentIndex >= 0 ?
                    CommandArgumentInfo.Arguments[LastArgumentIndex].Options.AutoCompleter :
                    null;
                ConsoleLogger.Debug("Command {0} has auto complete info? {1}", CommandName, AutoCompleter is not null);
                if (AutoCompleter is null)
                    // No delegate. Return nothing
                    return finalCompletions;

                // We have the delegate! Invoke it.
                ConsoleLogger.Debug("If we reach here, it means we have a delegate! Executing delegate with {0} [{1}]...", LastArgument, LastArgument.Length);
                finalCompletions = AutoCompleter.Invoke(finalCommandArgsEnclosed)
                    .Where(x => x.StartsWith(LastArgument))
                    .Select(x => x.Substring(LastArgument.Length))
                    .ToArray();
                ConsoleLogger.Debug("Invoked, and got {0} autocompletion suggestions. [{1}]", finalCompletions.Length, string.Join(", ", finalCompletions));
                return finalCompletions;
            }
            return finalCompletions;
        }

    }
}
