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
using System.Text;
using System.Linq;
using Terminaux.Reader;
using System.Collections.ObjectModel;
using Textify.General;
using Terminaux.Base;
using Terminaux.Reader.History;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Prompts;
using Terminaux.Shell.Switches;
using Terminaux.Shell.Shells.Unified;
using Terminaux.Colors.Data;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Shell.Shells
{
    /// <summary>
    /// Base shell module
    /// </summary>
    public static class ShellManager
    {
        internal static List<ShellExecuteInfo> ShellStack = [];
        internal static string lastCommand = "";

        internal readonly static List<CommandInfo> unifiedCommandDict =
        [
            new CommandInfo("exit", LanguageTools.GetLocalized("T_SHELL_UNIFIED_EXIT_HELP_DESC"),
                [
                    new CommandArgumentInfo()
                ], new ExitUnifiedCommand()),

            new CommandInfo("findcmds", LanguageTools.GetLocalized("T_SHELL_UNIFIED_FINDCMDS_HELP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "search")
                    ], false)
                ], new FindCmdsUnifiedCommand()),

            new CommandInfo("help", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_HELP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "command", new CommandArgumentPartOptions()
                        {
                            AutoCompleter = (_) => CommandManager.GetCommandNames(CurrentShellType)
                        })
                    ],
                    [
                        new SwitchInfo("general", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_GENERAL_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("alias", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_ALIAS_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("unified", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_UNIFIED_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("extra", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_EXTRA_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("all", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_ALL_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                        new SwitchInfo("simplified", LanguageTools.GetLocalized("T_SHELL_UNIFIED_HELP_SIMPLIFIED_SWITCH_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ], false)
                ], new HelpUnifiedCommand()),

            new CommandInfo("presets", LanguageTools.GetLocalized("T_SHELL_UNIFIED_PRESETS_HELP_DESC"),
                [
                    new CommandArgumentInfo()
                ], new PresetsUnifiedCommand()),
        ];

        internal readonly static Dictionary<string, BaseShellInfo> availableShells = [];

        /// <summary>
        /// List of unified commands
        /// </summary>
        public static CommandInfo[] UnifiedCommands =>
            [.. unifiedCommandDict];

        /// <summary>
        /// List of available shells
        /// </summary>
        public static ReadOnlyDictionary<string, BaseShellInfo> AvailableShells =>
            new(availableShells);

        /// <summary>
        /// Current shell type
        /// </summary>
        public static string CurrentShellType =>
            ShellStack[ShellStack.Count - 1].ShellType;

        /// <summary>
        /// Last shell type
        /// </summary>
        public static string LastShellType
        {
            get
            {
                if (ShellStack.Count == 0)
                {
                    // We don't have any shell. Return Shell.
                    ConsoleLogger.Warning("Trying to call LastShellType on empty shell stack. Assuming UESH...");
                    return "Shell";
                }
                else if (ShellStack.Count == 1)
                {
                    // We only have one shell. Consider current as last.
                    ConsoleLogger.Warning("Trying to call LastShellType on shell stack containing only one shell. Assuming current...");
                    return CurrentShellType;
                }
                else
                {
                    // We have more than one shell. Return the shell type for a shell before the last one.
                    var type = ShellStack[ShellStack.Count - 2].ShellType;
                    return type;
                }
            }
        }

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <remarks>All new shells implemented should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine() =>
            GetLine("", CurrentShellType, true);

        /// <summary>
        /// Inputs for command then parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <remarks>All new shells implemented should use this routine to allow effective and consistent line parsing.</remarks>
        public static void GetLine(string FullCommand) =>
            GetLine(FullCommand, CurrentShellType, true);

        /// <summary>
        /// Parses a specified command.
        /// </summary>
        /// <param name="FullCommand">The full command string</param>
        /// <param name="ShellType">Shell type</param>
        /// <param name="enableHistory">Whether to enable history support or not</param>
        /// <remarks>All new shells implemented should use this routine to allow effective and consistent line parsing.</remarks>
        internal static void GetLine(string FullCommand, string ShellType = "Shell", bool enableHistory = true)
        {
            // Check for sanity
            if (string.IsNullOrEmpty(FullCommand))
                FullCommand = "";

            // Get the shell info
            var shellInfo = GetShellInfo(ShellType);

            // Now, initialize the command autocomplete handler. This will not be invoked if we have auto completion disabled.
            var settings = new TermReaderSettings()
            {
                Suggestions = (text, index, _) => CommandAutoComplete.GetSuggestions(text, index),
                SuggestionsDelimiters = [' '],
                TreatCtrlCAsInput = true,
                InputForegroundColor = ConsoleColors.Silver,
                HistoryName = ShellType,
                HistoryEnabled = enableHistory,
            };

            // Check to see if the full command string ends with the semicolon
            while (FullCommand.EndsWith(";") || string.IsNullOrEmpty(FullCommand))
            {
                // Enable cursor
                ConsoleWrapper.CursorVisible = true;

                // Tell the user to provide the command
                StringBuilder commandBuilder = new(FullCommand);

                // Populate the prompt
                string prompt = "";
                var preset = PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType);
                if (!string.IsNullOrEmpty(FullCommand))
                    prompt = preset.PresetPromptCompletion;
                else
                    prompt = preset.PresetPrompt;

                // Wait for command
                string strcommand = TermReader.Read(prompt, "", settings, oneLineWrap: shellInfo.OneLineWrap);
                if (strcommand == ";")
                    strcommand = "";

                // Add command to command builder and return the final result. The reason to add the extra space before the second command written is that
                // because if we need to provide a second command to the shell in a separate line, we usually add the semicolon at the end of the primary
                // command input.
                if (!string.IsNullOrEmpty(FullCommand) && !string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(' ');

                // There are cases when strcommand may be empty, so ignore that if it's empty.
                if (!string.IsNullOrEmpty(strcommand))
                    commandBuilder.Append(strcommand);
                FullCommand = commandBuilder.ToString();

                // There are cases when the kernel panics or reboots in the middle of the command input. If reboot is requested,
                // ensure that we're really gone.
                var ShellInstance = ShellStack[ShellStack.Count - 1];
                if (ShellInstance.interrupting)
                    return;
            }

            // Check for a type of command
            var SplitCommands = FullCommand.Split(new[] { " ; " }, StringSplitOptions.RemoveEmptyEntries);
            var Commands = CommandManager.GetCommands(ShellType);
            for (int i = 0; i < SplitCommands.Length; i++)
            {
                string Command = SplitCommands[i];

                // Then, check to see if this shell uses the slash command
                if (shellInfo.SlashCommand)
                {
                    // Check if we need to remove the slash
                    if (!Command.StartsWith("/"))
                    {
                        // Not a slash command. Do things differently
                        var ShellInstance = ShellStack[ShellStack.Count - 1];
                        var Params = new CommandExecutorParameters(Command, shellInfo.NonSlashCommandInfo ?? BaseShellInfo.fallbackNonSlashCommand, ShellType, ShellInstance);
                        CommandExecutor.StartCommandThread(Params);
                        continue;
                    }
                    else
                        Command = Command.Substring(1).Trim();
                }

                // Check to see if the command is a comment
                if (!string.IsNullOrWhiteSpace(Command) && !Command.StartsWithAnyOf([" ", "#"]))
                {
                    // Get the command name
                    var words = Command.SplitEncloseDoubleQuotes();
                    string commandName = words[0].ReleaseDoubleQuotes();

                    // Verify that we aren't tricked into processing an empty command
                    if (string.IsNullOrEmpty(commandName))
                        break;

                    // Now, split the arguments
                    string arguments = string.Join(" ", words.Skip(1));

                    // Reads command written by user
                    try
                    {
                        // Check the command
                        bool exists = Commands.Any((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));
                        if (exists)
                        {
                            // Execute the command
                            var cmdInfo = Commands.Single((ci) => ci.Command == commandName || ci.Aliases.Any((ai) => ai.Alias == commandName));

                            if (!string.IsNullOrEmpty(commandName) || !commandName.StartsWithAnyOf([" ", "#"]))
                            {
                                // Check the command before starting
                                var ShellInstance = ShellStack[ShellStack.Count - 1];
                                var Params = new CommandExecutorParameters(Command, cmdInfo, ShellType, ShellInstance);
                                CommandExecutor.StartCommandThread(Params);
                            }
                        }
                        {
                            ConsoleLogger.Warning("Cmd exec {0} failed: command {0} not found", commandName);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDNOTFOUND"), ConsoleColors.Red, commandName);
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleLogger.Error(ex, "Cmd exec {0} failed: an error occurred", commandName);
                        TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_CMDEXECUTEERROR") + CharManager.NewLine + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ERRORCOMMAND2") + " {0}: {1}", ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message);
                    }
                }
            }
            lastCommand = FullCommand;
        }

        /// <summary>
        /// Gets the shell information instance
        /// </summary>
        /// <param name="shellType">Shell type name</param>
        public static BaseShellInfo GetShellInfo(string shellType) =>
            AvailableShells.TryGetValue(shellType, out BaseShellInfo? baseShellInfo) ? baseShellInfo : AvailableShells["Shell"];

        /// <summary>
        /// Starts the shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellArgs">Arguments to pass to shell</param>
        public static void StartShell(string ShellType, params object[] ShellArgs)
        {
            int shellCount = ShellStack.Count;
            try
            {
                // Make a shell executor based on shell type to select a specific executor (if the shell type is not MESH, and if the new shell isn't a mother shell)
                // Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
                var ShellExecute = GetShellExecutor(ShellType) ??
                    throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_EXCEPTION_NOEXECUTOR"), ShellType.ToString());

                // Make a new instance of shell information
                var ShellCommandThread = RegenerateCommandThread(ShellType);
                var ShellInfo = new ShellExecuteInfo(ShellType, ShellExecute, ShellCommandThread);

                // Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
                ShellStack.Add(ShellInfo);
                if (!HistoryTools.IsHistoryRegistered(ShellType))
                    HistoryTools.LoadFromInstance(new HistoryInfo(ShellType, []));

                // Reset title in case we're going to another shell
                ShellExecute.InitializeShell(ShellArgs);
            }
            catch (Exception ex)
            {
                // There is an exception trying to run the shell. Throw the message to the caller.
                ConsoleLogger.Error("Failed initializing shell!!! Type: {0}, Message: {1}", ShellType, ex.Message);
                ConsoleLogger.Error("Additional info: Args: {0} [{1}], Shell Stack: {2} shells, shellCount: {3} shells", ShellArgs.Length, string.Join(", ", ShellArgs), ShellStack.Count, shellCount);
                ConsoleLogger.Error(ex, "This shell needs to be killed in order for the shell manager to proceed. Passing exception to caller...");
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_SHELLMANAGER_EXCEPTION_SHELLINIT"), ex);
            }
            finally
            {
                // There is either an unknown shell error trying to be initialized or a shell has manually set Bail to true prior to exiting, like the JSON shell
                // that sets this property when it fails to open the JSON file due to syntax error or something. If we haven't added the shell to the shell stack,
                // do nothing. Else, purge that shell with KillShell(). Otherwise, we'll get another shell's commands in the wrong shell and other problems will
                // occur until the ghost shell has exited either automatically or manually, so check to see if we have added the newly created shell to the shell
                // stack and kill that faulted shell so that we can have the correct shell in the most recent shell, ^1, from the stack.
                int newShellCount = ShellStack.Count;
                if (newShellCount > shellCount)
                    KillShell();
            }
        }

        /// <summary>
        /// Kills the last running shell
        /// </summary>
        public static void KillShell()
        {
            if (ShellStack.Count >= 1)
            {
                var shell = ShellStack[ShellStack.Count - 1];
                var shellBase = ShellStack[ShellStack.Count - 1].ShellBase;
                if (shellBase is not null)
                {
                    shell.interrupting = true;
                    shellBase.Bail = true;
                }
                PurgeShells();
            }
        }

        /// <summary>
        /// Cleans up the shell stack
        /// </summary>
        public static void PurgeShells() =>
            // Remove these shells from the stack
            ShellStack.RemoveAll(x => x.ShellBase?.Bail ?? true);

        /// <summary>
        /// Gets the shell executor based on the shell type
        /// </summary>
        /// <param name="ShellType">The requested shell type</param>
        public static BaseShell? GetShellExecutor(string ShellType) =>
            GetShellInfo(ShellType).ShellBase;

        /// <summary>
        /// Registers the custom shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        /// <param name="ShellTypeInfo">The shell type information</param>
        public static void RegisterShell(string ShellType, BaseShellInfo ShellTypeInfo)
        {
            if (!ShellTypeExists(ShellType))
            {
                // First, add the shell
                availableShells.Add(ShellType, ShellTypeInfo);

                // Then, add the default preset if the current preset is not found
                if (PromptPresetManager.CurrentPresets.ContainsKey(ShellType))
                    return;

                // Rare state.
                var presets = ShellTypeInfo.ShellPresets;
                var basePreset = new PromptPresetBase();
                if (presets is not null)
                {
                    // Add a default preset
                    if (presets.ContainsKey("Default"))
                        PromptPresetManager.CurrentPresets.Add(ShellType, "Default");
                    else
                        PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
                else
                {
                    // Make a base shell preset and set it as default.
                    PromptPresetManager.CurrentPresets.Add(ShellType, basePreset.PresetName);
                }
            }
        }

        /// <summary>
        /// Unregisters the custom shell
        /// </summary>
        /// <param name="ShellType">The shell type</param>
        public static void UnregisterShell(string ShellType)
        {
            if (ShellTypeExists(ShellType))
            {
                // First, remove the shell
                availableShells.Remove(ShellType);

                // Then, remove the preset
                PromptPresetManager.CurrentPresets.Remove(ShellType);
            }
        }

        /// <summary>
        /// Does the shell exist?
        /// </summary>
        /// <param name="ShellType">Shell type to check</param>
        /// <returns>True if it exists; false otherwise.</returns>
        public static bool ShellTypeExists(string ShellType) =>
            AvailableShells.ContainsKey(ShellType);

        internal static Thread RegenerateCommandThread(string ShellType) =>
            new((cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams))
            {
                Name = $"{ShellType} Command Thread"
            };
    }
}
