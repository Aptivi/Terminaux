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
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Shell.Commands
{
    /// <summary>
    /// Command executor module
    /// </summary>
    public static class CommandExecutor
    {

        internal static void StartCommandThread(CommandExecutorParameters ThreadParams)
        {
            var ShellInstance = ThreadParams.ShellInstance;
            var StartCommandThread = ShellInstance.ShellCommandThread;
            if (!StartCommandThread.IsAlive)
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
                throw new TerminauxException("Thread parameters are not specified.");
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
                    TextWriterColor.WriteColor("Required arguments are not provided for all usages below:", ConsoleColors.Red);
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
                                Value = "Unknown argument"
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
                            TextWriterColor.WriteColor("Required arguments are not provided.", ConsoleColors.Olive);
                        }

                        // Check for required switches
                        if (!unsatisfied.RequiredSwitchesProvided)
                        {
                            ConsoleLogger.Warning("User hasn't provided enough switches for {0}", command);
                            TextWriterColor.WriteColor("Required switches are not provided.", ConsoleColors.Olive);
                        }

                        // Check for required switch arguments
                        if (!unsatisfied.RequiredSwitchArgumentsProvided)
                        {
                            ConsoleLogger.Warning("User hasn't provided a value for one of the switches for {0}", command);
                            TextWriterColor.WriteColor("One of the switches requires a value that is not provided.", ConsoleColors.Olive);
                        }

                        // Check for unknown switches
                        if (unsatisfied.UnknownSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided unknown switches {0}", command);
                            TextWriterColor.WriteColor("Switches that are listed below are unknown.", ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.UnknownSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for conflicting switches
                        if (unsatisfied.ConflictingSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided conflicting switches for {0}", command);
                            TextWriterColor.WriteColor("Switches that are listed below conflict with each other.", ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.ConflictingSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for switches that don't accept values
                        if (unsatisfied.NoValueSwitchesList.Length > 0)
                        {
                            ConsoleLogger.Warning("User has provided switches that don't accept values for {0}", command);
                            TextWriterColor.WriteColor("The below switches don't accept values.", ConsoleColors.Olive);
                            TextWriterRaw.WriteRaw(new Listing()
                            {
                                Objects = unsatisfied.NoValueSwitchesList,
                            }.Render() + "\n");
                        }

                        // Check for invalid number in numeric arguments
                        if (!unsatisfied.NumberProvided)
                        {
                            ConsoleLogger.Warning("User has provided invalid number for one or more of the arguments for {0}", command);
                            TextWriterColor.WriteColor("One or more of the arguments expect a numeric value, but you provided an invalid number.", ConsoleColors.Olive);
                        }

                        // Check for invalid exact wording
                        if (!unsatisfied.ExactWordingProvided)
                        {
                            ConsoleLogger.Warning("User has provided non-exact wording for {0}", command);
                            TextWriterColor.WriteColor("One or more of the arguments expect an exact wording, but you provided an invalid word.", ConsoleColors.Olive);
                        }

                        // Check for invalid number in numeric switches
                        if (!unsatisfied.SwitchNumberProvided)
                        {
                            ConsoleLogger.Warning("User has provided invalid number for one or more of the switches for {0}", command);
                            TextWriterColor.WriteColor("One or more of the switches expect a numeric value, but you provided an invalid number.", ConsoleColors.Olive);
                        }
                    }
                    TextWriterColor.WriteColor("Consult the help entry for this command for more info", ConsoleColors.Red);
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

                // Execute the command
                if (argSatisfied)
                {
                    // Prepare the command parameter instance
                    var parameters = new CommandParameters(StrArgs, Args, StrArgsOrig, ArgsOrig, Switches, Command);

                    // Now, get the base command and execute it
                    var CommandBase = RequestedCommandInfo.CommandBase;
                    CommandDelegate(CommandBase, parameters);
                }
            }
            catch (ThreadInterruptedException)
            {
                ConsoleLogger.Warning("Command {0} is being interrupted...", RequestedCommand);
                TextWriterColor.WriteColor("Command is being interrupted...", ConsoleColors.Red);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to execute command {0}", RequestedCommand);
                TextWriterColor.WriteColor("Error trying to execute command {2}." + CharManager.NewLine + "Error" + " {0}: {1}", ConsoleColors.Red, ex.GetType().FullName ?? "<null>", ex.Message, RequestedCommand);
            }
        }

        private static void CommandDelegate(BaseCommand CommandBase, CommandParameters parameters)
        {
            try
            {
                if (ConsoleChecker.IsDumb)
                    CommandBase.ExecuteDumb(parameters);
                else
                    CommandBase.Execute(parameters);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor("Command aborted for the following reason" + $": {ex.Message}", ConsoleColors.Red);
            }
        }

    }
}
