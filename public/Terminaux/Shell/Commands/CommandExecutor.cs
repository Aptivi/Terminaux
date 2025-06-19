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
using System.Threading;
using System.Linq;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Terminaux.Shell.Arguments;
using Terminaux.Base.Checks;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Shell.Shells;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Shell.Scripting;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Switches;
using Terminaux.Base.Wrappers;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command executor module
    /// </summary>
    public static class CommandExecutor
    {

        internal static void StartCommandThread(CommandExecutorParameters ThreadParams)
        {
            // Since we're probably trying to run a command using the alternative command threads, if the main shell command thread
            // is running, use that to execute the command. This ensures that commands like "wrap" that also execute commands from the
            // shell can do their job.
            var ShellInstance = ThreadParams.ShellInstance;
            var StartCommandThread = ShellInstance.ShellCommandThread;
            bool CommandThreadValid = true;
            if (StartCommandThread.IsAlive)
            {
                ConsoleLogger.Warning("Can't make another main command thread. Using alternatives...");
                if (ShellInstance.AltCommandThreads.Count > 0)
                {
                    ConsoleLogger.Info("Using last alt command thread...");
                    ShellInstance.AltCommandThreads[ShellInstance.AltCommandThreads.Count - 1] = ShellManager.RegenerateCommandThread(ShellInstance.ShellType);
                    StartCommandThread = ShellInstance.AltCommandThreads[ShellInstance.AltCommandThreads.Count - 1];
                }
                else
                {
                    ConsoleLogger.Warning("Cmd exec {0} failed: Alt command threads are not there.", ThreadParams.RequestedCommand);
                    CommandThreadValid = false;
                }
            }
            if (CommandThreadValid)
            {
                ConsoleLogger.Info("Starting command thread...");
                StartCommandThread.Start(ThreadParams);
                StartCommandThread.Join();
                ShellInstance.shellCommandThread = ShellManager.RegenerateCommandThread(ShellInstance.ShellType);
            }
        }

        internal static void ExecuteCommand(CommandExecutorParameters? ThreadParams)
        {
            if (ThreadParams is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_EXCEPTION_THREADPARAMS"));
            var RequestedCommand = ThreadParams.RequestedCommand;
            var RequestedCommandInfo = ThreadParams.RequestedCommandInfo;
            string ShellType = ThreadParams.ShellType;
            var ShellInstance = ThreadParams.ShellInstance;
            bool argSatisfied = true;
            try
            {
                // Variables
                var (satisfied, total) = ArgumentsParser.ParseShellCommandArguments(RequestedCommand, RequestedCommandInfo, ShellType);

                // Check to see if we have satisfied arguments list
                argSatisfied = satisfied is not null;
                if (satisfied is null)
                {
                    ConsoleLogger.Warning("Arguments not satisfied.");
                    TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGSNOTPROVIDEDLIST"), ConsoleColors.Red);
                    for (int i = 0; i < total.Length; i++)
                    {
                        ProvidedArgumentsInfo unsatisfied = total[i];
                        string command = unsatisfied.Command;
                        var argInfo = unsatisfied.ArgumentInfo;
                        if (argInfo is null)
                        {
                            TextWriterRaw.WriteRaw(new ListEntry()
                            {
                                Entry = $"- [{i + 1}] {command}: ",
                                Value = LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNKNOWNARG")
                            }.Render() + "\n");
                            continue;
                        }

                        // Write usage number
                        string renderedUsage = !string.IsNullOrEmpty(argInfo.RenderedUsage) ? " " + argInfo.RenderedUsage : "";
                        TextWriterColor.WriteColor($"- [{i + 1}] {command}{renderedUsage}", ConsoleColors.Yellow);

                        // Check for required arguments
                        if (!unsatisfied.RequiredArgumentsProvided)
                        {
                            ConsoleLogger.Warning("User hasn't provided enough arguments for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGSNOTPROVIDED"), ConsoleColors.Olive);
                        }

                        // Check for required switches
                        if (!unsatisfied.RequiredSwitchesProvided)
                        {
                            ConsoleLogger.Warning("User hasn't provided enough switches for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNOTPROVIDED"), ConsoleColors.Olive);
                        }

                        // Check for required switch arguments
                        if (!unsatisfied.RequiredSwitchArgumentsProvided)
                        {
                            ConsoleLogger.Warning("User hasn't provided a value for one of the switches for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNEEDVALUE"), ConsoleColors.Olive);
                        }

                        // Check for unknown switches
                        if (unsatisfied.UnknownSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided unknown switches {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_UNKNOWNSWITCHES"), ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.UnknownSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for conflicting switches
                        if (unsatisfied.ConflictingSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided conflicting switches for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESCONFLICT"), ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.ConflictingSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for switches that don't accept values
                        if (unsatisfied.NoValueSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided switches that don't accept values for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNOVALUES"), ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.NoValueSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for invalid number in numeric arguments
                        if (!unsatisfied.NumberProvided)
                        {
                            ConsoleLogger.Warning("User has provided invalid number for one or more of the arguments for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGNUMERIC"), ConsoleColors.Olive);
                        }

                        // Check for invalid exact wording
                        if (!unsatisfied.ExactWordingProvided)
                        {
                            ConsoleLogger.Warning("User has provided non-exact wording for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGEXACT"), ConsoleColors.Olive);
                        }

                        // Check for invalid number in numeric switches
                        if (!unsatisfied.SwitchNumberProvided)
                        {
                            ConsoleLogger.Warning("User has provided invalid number for one or more of the switches for {0}", command);
                            TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHNUMERIC"), ConsoleColors.Olive);
                        }
                    }
                    TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_CONSULTHELP"), ConsoleColors.Red);
                    ShellInstance.LastErrorCode = -6;
                    return;
                }

                // Now, assume that an argument is satisfied
                var ArgumentInfo = satisfied;
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList.Select(TextTools.Unescape).ToArray();
                var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
                var Switches = ArgumentInfo.SwitchesList.Select(TextTools.Unescape).ToArray();
                string StrArgs = TextTools.Unescape(ArgumentInfo.ArgumentsText);
                string StrArgsOrig = ArgumentInfo.ArgumentsTextOrig;
                bool containsSetSwitch = SwitchManager.ContainsSwitch(Switches, "-set");
                string variable = "";

                // Check to see if a requested command is obsolete
                if (RequestedCommandInfo.Flags.HasFlag(CommandFlags.Obsolete))
                {
                    ConsoleLogger.Debug("The command requested {0} is obsolete", Command);
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_OBSOLETECMD"));
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                var ArgInfos = RequestedCommandInfo.CommandArgumentInfo;
                for (int i = 0; i < ArgInfos.Length; i++)
                {
                    argSatisfied = true;
                    CommandArgumentInfo ArgInfo = ArgInfos[i];
                    bool isLast = i == ArgInfos.Length - 1;
                    if (ArgInfo is not null)
                    {
                        // Trim the -set switch
                        if (containsSetSwitch)
                        {
                            // First, work on the string
                            string setValue = $"-set={SwitchManager.GetSwitchValue(Switches, "-set")}";

                            // Work on the list
                            if (Switches.Contains(setValue))
                            {
                                for (int j = 0; j < Switches.Length; j++)
                                {
                                    string @switch = Switches[j];
                                    if (@switch == setValue && ArgInfo.AcceptsSet)
                                    {
                                        variable = SwitchManager.GetSwitchValue(Switches, "-set");
                                        Switches = Switches.Except([setValue]).ToArray();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (argSatisfied)
                        break;
                }

                // Prepare the command parameter instance and run the argument handler if any
                var parameters = new CommandParameters(StrArgs, Args, StrArgsOrig, ArgsOrig, Switches, Command)
                {
                    SwitchSetPassed = containsSetSwitch
                };
                int argCheckerReturnCode = 0;
                if (argSatisfied)
                {
                    argCheckerReturnCode = satisfied.ArgumentInfo?.ArgChecker.Invoke(parameters) ?? 0;
                    ConsoleLogger.Debug("Command {0} with args {1} argument checker returned {2}", Command, StrArgs, argCheckerReturnCode);
                    argSatisfied = argCheckerReturnCode == 0;
                }

                // Execute the command
                if (argSatisfied)
                {
                    // Now, get the base command and execute it
                    ConsoleLogger.Debug("Really executing command {0} with args {1}", Command, StrArgs);
                    var CommandBase = RequestedCommandInfo.CommandBase;
                    string value = "";
                    CommandDelegate(ShellInstance, CommandBase, parameters, ref value);

                    // Set the error code and set the MESH variable as appropriate
                    ConsoleLogger.Debug("Error code is {0}", ShellInstance.LastErrorCode);
                    if (containsSetSwitch)
                    {
                        // Check to see if the value contains newlines
                        if (value.Contains('\n'))
                        {
                            // Assume that we're setting an array.
                            ConsoleLogger.Debug("Array variable to set is {0}", variable);
                            string[] values = value.Replace((char)13, default).Split('\n');
                            MESHVariables.SetVariables(variable, values);
                        }
                        else if (value.StartsWith("[") && value.EndsWith("]"))
                        {
                            // Assume that we're setting an array
                            ConsoleLogger.Debug("Array variable to set is {0}", variable);
                            value = value.Substring(1, value.Length - 1);
                            string[] values = value.Split([", "], StringSplitOptions.None);
                            MESHVariables.SetVariables(variable, values);
                        }
                        else
                        {
                            ConsoleLogger.Debug("Variable to set {0} is {1}", value, variable);
                            MESHVariables.SetVariable(variable, value);
                        }
                    }
                }
                else
                {
                    ConsoleLogger.Warning("Arguments not satisfied.");
                    ShellInstance.LastErrorCode = argCheckerReturnCode != 0 ? argCheckerReturnCode : -6;
                }
            }
            catch (ThreadInterruptedException)
            {
                ConsoleLogger.Warning("Command {0} is being interrupted...", RequestedCommand);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_COMMANDINTERRUPT"), ConsoleColors.Red);
                CancellationHandlers.CancelRequested = false;
                ShellInstance.LastErrorCode = -5;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to execute command {0}", RequestedCommand);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ERRORCOMMAND1") + CharManager.NewLine + LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ERRORCOMMAND2") + " {0}: {1}", ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message, RequestedCommand);
                ShellInstance.LastErrorCode = ex.GetHashCode();
            }
        }
        /// <summary>
        /// Executes a command in a wrapped mode (must be run from a separate command execution entry point, <see cref="BaseCommand.Execute(CommandParameters, ref string)"/>.)
        /// </summary>
        /// <param name="Command">Requested command with its arguments and switches</param>
        public static void ExecuteCommandWrapped(string Command)
        {
            var currentShell = ShellManager.ShellStack[ShellManager.ShellStack.Count - 1];
            var currentType = currentShell.ShellType;
            var StartCommandThread = currentShell.ShellCommandThread;
            var (_, total) = ArgumentsParser.ParseShellCommandArguments(Command, currentType);
            string CommandToBeWrapped = total[0].Command;

            // Check to see if the command is found
            if (!CommandManager.IsCommandFound(CommandToBeWrapped, currentType))
            {
                ConsoleLogger.Error("Wrappable command {0} not found", Command);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_WRAPPABLECMDNOTFOUND"), true, ConsoleColors.Red);
                return;
            }

            // Check to see if we can start an alternative thread
            if (!StartCommandThread.IsAlive)
            {
                ConsoleLogger.Error("Can't directly execute command {0} in wrapped mode.", Command);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_WRAPSHOULDBEMAIN"), true, ConsoleColors.Red);
                return;
            }

            // Now, check to see if the command is wrappable
            if (!CommandManager.GetCommand(CommandToBeWrapped, currentType).Flags.HasFlag(CommandFlags.Wrappable))
            {
                var WrappableCmds = GetWrappableCommands(currentType);
                ConsoleLogger.Error("Unwrappable command {0}! Wrappable commands: [{1}]", Command, string.Join(", ", WrappableCmds));
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_WRAPUNWRAPPABLE"), true, ConsoleColors.Red);
                for (int i = 0; i < WrappableCmds.Length; i++)
                {
                    string? wrappableCmd = WrappableCmds[i];
                    ListEntryWriterColor.WriteListEntry($"{i + 1}", wrappableCmd);
                }
                return;
            }

            bool buffered = false;
            try
            {
                // Buffer the target command output
                ConsoleLogger.Debug("Buffering...");
                var wrapOutput = BufferCommand(Command);

                // Now, print the output
                ConsoleLogger.Debug("Printing...");
                WrappedWriter.WriteWrapped(wrapOutput, false);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to wrap command {0}: {1}", CommandToBeWrapped, ex.Message);
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_WRAPFAILED") + ": {0}", true, ConsoleColors.Red, ex.Message);
            }

            // In case error happens
            if (!buffered)
                ConsoleWrapperTools.UnsetWrapperLocal();
        }

        /// <summary>
        /// Gets the wrappable commands
        /// </summary>
        /// <param name="shellType">Shell type</param>
        /// <returns>List of commands that one of their flags contains <see cref="CommandFlags.Wrappable"/></returns>
        public static string[] GetWrappableCommands(string shellType)
        {
            // Get shell info
            var shellInfo = ShellManager.GetShellInfo(shellType);

            // Get wrappable commands
            var WrappableCmds = shellInfo.Commands
                .Where(CommandInfo => CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                .Select(CommandInfo => CommandInfo.Command)
                .ToArray();
            var WrappableUnified = ShellManager.UnifiedCommands
                .Where(CommandInfo => CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                .Select(CommandInfo => CommandInfo.Command)
                .ToArray();
            var WrappableAliases = AliasManager.GetAliasListFromType(shellType)
                .Where((info) => WrappableCmds.Contains(info.Command) || WrappableUnified.Contains(info.Command))
                .Select((info) => info.Alias)
                .ToArray();
            var finalWrappables = WrappableCmds
                .Union(WrappableAliases)
                .Union(WrappableUnified)
                .ToArray();

            return finalWrappables;
        }

        internal static string BufferCommand(string command)
        {
            var currentShell = ShellManager.ShellStack[ShellManager.ShellStack.Count - 1];
            var currentType = currentShell.ShellType;
            var (satisfied, total) = ArgumentsParser.ParseShellCommandArguments(command, currentType);
            string commandName = total[0].Command;

            // Check to see if the requested command is wrappable
            if (!CommandManager.GetCommand(commandName, currentType).Flags.HasFlag(CommandFlags.Wrappable))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_COMMANDS_EXCEPTION_BUFFERUNWRAPPABLE"));

            string bufferOutput = "";
            bool buffered = false;
            try
            {
                // First, initialize the alternative command thread
                var AltThreads = currentShell.AltCommandThreads;
                if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
                {
                    ConsoleLogger.Debug("Making alt thread for buffered command {0}...", command);
                    var BufferedCommand = new Thread((cmdThreadParams) => ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                    currentShell.AltCommandThreads.Add(BufferedCommand);
                }

                // Then, initialize the buffered writer and execute the commands
                ConsoleWrapperTools.SetWrapperLocal(nameof(Buffered));
                ConsoleLogger.Debug("Buffering...");
                ShellManager.GetLine(command, "", currentType, false, false);
                CancellationHandlers.AllowCancel();
                buffered = true;

                // Extract the buffer and then end the local driver
                var buffer = ((Buffered)ConsoleWrapperTools.Wrapper).consoleBuffer;
                bufferOutput = buffer.ToString();
                buffer.Clear();
                ConsoleWrapperTools.UnsetWrapperLocal();
            }
            catch
            {
                // There is some error, so propagate the error to the caller once we revert the driver.
                if (!buffered)
                    ConsoleWrapperTools.UnsetWrapperLocal();
                throw;
            }
            return bufferOutput;
        }

        private static void CommandDelegate(ShellExecuteInfo ShellInstance, BaseCommand CommandBase, CommandParameters parameters, ref string value)
        {
            try
            {
                if (ConsoleWrapperTools.Wrapper.IsDumb)
                    ShellInstance.LastErrorCode = CommandBase.ExecuteDumb(parameters, ref value);
                else
                    ShellInstance.LastErrorCode = CommandBase.Execute(parameters, ref value);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"Command aborted: {ex.Message}");
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ABORTEXCEPTION") + $": {ex.Message}", ConsoleColors.Red);
            }
        }

    }
}
