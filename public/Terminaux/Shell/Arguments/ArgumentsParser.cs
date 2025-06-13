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
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Textify.General;
using Textify.Tools;

namespace Terminaux.Shell.Arguments
{
    /// <summary>
    /// Argument parser tools
    /// </summary>
    public static class ArgumentsParser
    {
        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, string CommandType) =>
            ParseShellCommandArguments(CommandText, null, CommandType);

        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="cmdInfo">Command information</param>
        /// <param name="CommandType">Shell command type.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, CommandInfo? cmdInfo, string CommandType)
        {
            string Command;
            CommandInfo[] ShellCommands;

            // Change the available commands list according to command type
            ShellCommands = CommandManager.GetCommands(CommandType);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            var wordsOrig = CommandText.SplitEncloseDoubleQuotesNoRelease();
            string arguments = string.Join(" ", words.Skip(1));
            string argumentsOrig = string.Join(" ", wordsOrig.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                ConsoleLogger.Debug("Word {0}: {1}", i + 1, words[i]);
            Command = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var aliases = AliasManager.GetAliasListFromType(CommandType);
            var CommandInfo = ShellCommands.Any((info) => info.Command == Command) ? ShellCommands.Single((info) => info.Command == Command) :
                              aliases.Any((info) => info.Alias == Command) ? aliases.Single((info) => info.Alias == Command).TargetCommand :
                              cmdInfo;
            var fallback = new ProvidedArgumentsInfo(Command, arguments, words.Skip(1).ToArray(), argumentsOrig, wordsOrig.Skip(1).ToArray(), [], true, true, true, [], [], [], true, true, true, new());

            // Change the command if a command with no slash is entered on slash-enabled shells
            var shellInfo = ShellManager.GetShellInfo(CommandType);
            if (shellInfo.SlashCommand)
            {
                // Either change the command info or strip the slash
                if (!CommandText.StartsWith("/"))
                    CommandInfo = cmdInfo;
                else
                    CommandText = CommandText.Substring(1).Trim();
            }

            // Now, process the arguments
            if (CommandInfo != null)
                return ProcessArgumentOrShellCommandArguments(CommandText, CommandInfo, null);
            else
                return (fallback, new[] { fallback });
        }

        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="ArgumentText">Argument text that the user provided</param>
        /// <param name="argsDict">A dictionary of argument info instances</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed argument</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseArgumentArguments(string ArgumentText, Dictionary<string, ArgumentInfo> argsDict)
        {
            string Argument;

            // Split the requested argument string into words
            var words = ArgumentText.SplitEncloseDoubleQuotes();
            var wordsOrig = ArgumentText.SplitEncloseDoubleQuotesNoRelease();
            string arguments = string.Join(" ", words.Skip(1));
            string argumentsOrig = string.Join(" ", wordsOrig.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                ConsoleLogger.Debug("Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var ArgumentInfo = argsDict.TryGetValue(Argument, out ArgumentInfo? argInfo) ? argInfo : null;
            var fallback = new ProvidedArgumentsInfo(Argument, arguments, words.Skip(1).ToArray(), argumentsOrig, wordsOrig.Skip(1).ToArray(), [], true, true, true, [], [], [], true, true, true, new());
            if (ArgumentInfo != null)
                return ProcessArgumentOrShellCommandArguments(ArgumentText, null, ArgumentInfo);
            else
                return (fallback, new[] { fallback });
        }

        private static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ProcessArgumentOrShellCommandArguments(string CommandText, CommandInfo? CommandInfo, ArgumentInfo? argumentInfo)
        {
            ProvidedArgumentsInfo? satisfiedArg = null;
            List<ProvidedArgumentsInfo> totalArgs = [];

            // Split the switches properly now
            string switchRegex =
                /* lang=regex */ @"((?<= )-\S+=((""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+))|(?<= )-\S+";
            var matches = RegexTools.Matches(CommandText, switchRegex);
            List<string> enclosedSwitchList = [];
            foreach (Match match in matches)
                enclosedSwitchList.Add(match.Value);
            string[] enclosedSwitches = [.. enclosedSwitchList];
            CommandText = RegexTools.Filter(CommandText, switchRegex);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            var wordsOrig = CommandText.SplitEncloseDoubleQuotesNoRelease();

            // Split the arguments with enclosed quotes
            var EnclosedArgMatches = words.Skip(1);
            var EnclosedArgMatchesOrig = wordsOrig.Skip(1);
            var EnclosedArgs = EnclosedArgMatches.ToArray();
            var EnclosedArgsOrig = EnclosedArgMatches.ToArray();
            ConsoleLogger.Debug("{0} arguments parsed: {1}", EnclosedArgs.Length, string.Join(", ", EnclosedArgs));

            // Get the string of arguments
            string strArgs = words.Length > 0 ? string.Join(" ", EnclosedArgMatches) : "";
            string strArgsOrig = words.Length > 0 ? string.Join(" ", EnclosedArgMatchesOrig) : "";
            ConsoleLogger.Debug("Finished strArgs: {0}", strArgs);
            ConsoleLogger.Debug("Finished strArgsOrig: {0}", strArgsOrig);

            // Split the switches to their key-value counterparts
            var EnclosedSwitchKeyValuePairs = SwitchManager.GetSwitchValues(enclosedSwitches, true);

            // Check to see if we're optionalizing some required arguments starting from the last required argument
            int minimumArgumentsOffset = 0;
            string[] unknownSwitchesList = [];
            string[] conflictingSwitchesList = [];
            string[] noValueSwitchesList = [];
            var argInfos = (CommandInfo is not null ? CommandInfo.CommandArgumentInfo : argumentInfo?.ArgArgumentInfo) ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_ARGSPARSE_EXCEPTION_ARGINFO"));
            if (argInfos.Length == 0)
                argInfos = [new CommandArgumentInfo()];
            foreach (var argInfo in argInfos)
            {
                bool RequiredArgumentsProvided = true;
                bool RequiredSwitchesProvided = true;
                bool RequiredSwitchArgumentsProvided = true;
                bool numberProvided = true;
                bool switchNumberProvided = true;
                bool exactWordingProvided = true;

                // Optionalize some of the arguments if there are switches that optionalize them
                ConsoleLogger.Debug("Argument info is full? {0}", argInfo is not null);
                if (argInfo is not null)
                {
                    foreach (string enclosedSwitch in enclosedSwitches)
                    {
                        ConsoleLogger.Debug("Optionalizer is processing switch {0}...", enclosedSwitch);
                        var switches = argInfo.Switches.Where((switchInfo) => switchInfo.SwitchName == enclosedSwitch.Substring(1));
                        if (switches.Any())
                            foreach (var switchInfo in switches.Where(switchInfo => minimumArgumentsOffset < switchInfo.OptionalizeLastRequiredArguments))
                                minimumArgumentsOffset = switchInfo.OptionalizeLastRequiredArguments;
                        ConsoleLogger.Debug("Minimum arguments offset is now {0}", minimumArgumentsOffset);
                    }
                }
                int finalRequiredArgs = argInfo is not null ? argInfo.MinimumArguments - minimumArgumentsOffset : 0;
                if (finalRequiredArgs < 0)
                    finalRequiredArgs = 0;
                ConsoleLogger.Debug("Required arguments count is now {0}", finalRequiredArgs);

                // Check to see if the caller has provided required number of arguments
                if (argInfo is not null)
                    RequiredArgumentsProvided =
                        EnclosedArgs.Length >= finalRequiredArgs ||
                        !argInfo.ArgumentsRequired;
                else
                    RequiredArgumentsProvided = true;
                ConsoleLogger.Debug("RequiredArgumentsProvided is {0}. Refer to the value of argument info.", RequiredArgumentsProvided);

                // Check to see if the caller has provided required number of switches
                if (argInfo is not null)
                    RequiredSwitchesProvided =
                        argInfo.Switches.Length == 0 ||
                        enclosedSwitches.Length >= argInfo.Switches.Where((@switch) => @switch.IsRequired).Count() ||
                        !argInfo.Switches.Any((@switch) => @switch.IsRequired);
                else
                    RequiredSwitchesProvided = true;
                ConsoleLogger.Debug("RequiredSwitchesProvided is {0}. Refer to the value of argument info.", RequiredSwitchesProvided);

                // Check to see if the caller has provided required number of switches that require arguments
                if (argInfo is not null)
                {
                    if (argInfo.Switches.Length == 0 || enclosedSwitches.Length == 0 ||
                        !argInfo.Switches.Any((@switch) => @switch.ArgumentsRequired))
                        RequiredSwitchArgumentsProvided = true;
                    else
                    {
                        var allSwitches = argInfo.Switches.Where((@switch) => @switch.ArgumentsRequired).Select((@switch) => @switch.SwitchName).ToArray();
                        var allProvidedSwitches = enclosedSwitches.Where((@switch) => allSwitches.Contains($"{@switch.Substring(1)}")).ToArray();
                        foreach (var providedSwitch in allProvidedSwitches)
                        {
                            if (string.IsNullOrWhiteSpace(EnclosedSwitchKeyValuePairs.Single((kvp) => kvp.Item1 == providedSwitch).Item2))
                                RequiredSwitchArgumentsProvided = false;
                        }
                    }
                }
                else
                    RequiredSwitchArgumentsProvided = true;
                ConsoleLogger.Debug("RequiredSwitchArgumentsProvided is {0}. Refer to the value of argument info.", RequiredSwitchArgumentsProvided);

                // Check to see if the caller has provided switches that don't accept values with the values
                if (argInfo is not null)
                {
                    var allSwitches = argInfo.Switches.Where((@switch) => !@switch.AcceptsValues).Select((@switch) => @switch.SwitchName).ToArray();
                    var allProvidedSwitches = enclosedSwitches
                        .Where((@switch) => @switch.Contains('='))
                        .Where((@switch) => allSwitches.Contains($"{@switch.Substring(1, @switch.IndexOf("="))}"))
                        .Select((@switch) => $"{@switch.Substring(0, @switch.IndexOf("="))}")
                        .ToArray();
                    List<string> rejected = [];
                    foreach (var providedSwitch in allProvidedSwitches)
                    {
                        if (!string.IsNullOrWhiteSpace(EnclosedSwitchKeyValuePairs.Single((kvp) => kvp.Item1 == providedSwitch).Item2))
                            rejected.Add(providedSwitch);
                    }
                    noValueSwitchesList = [.. rejected];
                }
                ConsoleLogger.Debug("RequiredSwitchArgumentsProvided is {0}. Refer to the value of argument info.", RequiredSwitchArgumentsProvided);

                // Check to see if the caller has provided non-existent switches
                if (argInfo is not null)
                    unknownSwitchesList = EnclosedSwitchKeyValuePairs
                        .Select((kvp) => kvp.Item1)
                        .Where((key) => !argInfo.Switches.Any((switchInfo) => switchInfo.SwitchName == key.Substring(1)))
                        .ToArray();
                ConsoleLogger.Debug("Unknown switches: {0}", unknownSwitchesList.Length);

                // Check to see if the caller has provided conflicting switches
                if (argInfo is not null)
                {
                    List<string> processed = [];
                    List<string> conflicts = [];
                    foreach (var kvp in EnclosedSwitchKeyValuePairs)
                    {
                        // Check to see if the switch exists
                        string @switch = kvp.Item1;
                        if (unknownSwitchesList.Contains(@switch))
                            continue;
                        ConsoleLogger.Debug("Processing switch: {0}", @switch);

                        // Get the switch and its conflicts list
                        var switchEnumerator = argInfo.Switches
                            .Where((switchInfo) => $"-{switchInfo.SwitchName}" == @switch);
                        if (switchEnumerator.Any())
                        {
                            // We have a switch! Now, process it.
                            var initialConflicts = switchEnumerator.First().ConflictsWith ?? [];
                            string[] switchConflicts = initialConflicts
                                .Select((conflicting) => $"-{conflicting}")
                                .ToArray();
                            ConsoleLogger.Debug("Switch conflicts: {0} [{1}]", switchConflicts.Length, string.Join(", ", switchConflicts));

                            // Now, get the last switch and check to see if it's provided with the conflicting switch
                            string lastSwitch = processed.Count > 0 ? processed[processed.Count - 1] : "";
                            if (switchConflicts.Contains(lastSwitch))
                            {
                                ConsoleLogger.Debug("Conflict! {0} and {1} conflict with each other.", @switch, lastSwitch);
                                conflicts.Add($"{@switch} vs. {lastSwitch}");
                            }
                            processed.Add(@switch);
                            ConsoleLogger.Debug("Marked conflicts: {0} [{1}]", conflicts.Count, string.Join(", ", conflicts));
                            ConsoleLogger.Debug("Processed: {0} [{1}]", processed.Count, string.Join(", ", processed));
                        }
                    }
                    conflictingSwitchesList = [.. conflicts];
                }

                // Check to see if the caller has provided a non-numeric value to an argument that expects number
                if (argInfo is not null)
                {
                    for (int i = 0; i < argInfo.Arguments.Length && i < EnclosedArgs.Length; i++)
                    {
                        // Get the argument and the part
                        string arg = EnclosedArgs[i];
                        var argPart = argInfo.Arguments[i];

                        // Check to see if the argument expects a number and that the provided argument is numeric
                        // or if the argument allows string values
                        if (argPart.Options.IsNumeric && !double.TryParse(arg, out _))
                            numberProvided = false;
                    }
                }

                // Check to see if the caller has provided a wording other than the expected exact wording if found
                if (argInfo is not null)
                {
                    for (int i = 0; i < argInfo.Arguments.Length && i < EnclosedArgs.Length; i++)
                    {
                        // Get the argument and the part
                        string arg = EnclosedArgs[i];
                        var argPart = argInfo.Arguments[i];

                        // Check to see if the argument expects a number and that the provided argument is numeric
                        // or if the argument allows string values
                        if (argPart.Options.ExactWording.Length > 0 && !argPart.Options.ExactWording.Contains(arg))
                            exactWordingProvided = false;
                    }
                }

                // Check to see if the caller has provided a non-numeric value to a switch that expects numbers
                if (argInfo is not null)
                {
                    var switchesList = argInfo.Switches.Where((si) => si.SwitchName != "set").ToArray();
                    for (int i = 0; i < switchesList.Length && i < EnclosedSwitchKeyValuePairs.Count; i++)
                    {
                        // Get the switch and the part
                        var switches = EnclosedSwitchKeyValuePairs[i];
                        var switchPart = switchesList[i];

                        // Check to see if the switch expects a number and that the provided switch is numeric
                        // or if the switch allows string values
                        if (switchPart.IsNumeric && !double.TryParse(switches.Item2, out _))
                            switchNumberProvided = false;
                    }
                }

                // If all is well, bail.
                var paiInstance = new ProvidedArgumentsInfo
                (
                    words[0],
                    strArgs,
                    EnclosedArgs,
                    strArgsOrig,
                    EnclosedArgsOrig,
                    enclosedSwitches,
                    RequiredArgumentsProvided,
                    RequiredSwitchesProvided,
                    RequiredSwitchArgumentsProvided,
                    unknownSwitchesList,
                    conflictingSwitchesList,
                    noValueSwitchesList,
                    numberProvided,
                    exactWordingProvided,
                    switchNumberProvided,
                    argInfo
                );
                if (RequiredArgumentsProvided &&
                    RequiredSwitchesProvided &&
                    RequiredSwitchArgumentsProvided &&
                    unknownSwitchesList.Length == 0 &&
                    conflictingSwitchesList.Length == 0 &&
                    numberProvided &&
                    exactWordingProvided &&
                    switchNumberProvided)
                    satisfiedArg = paiInstance;
                totalArgs.Add(paiInstance);
            }

            // Install the parsed values to the new class instance
            return (satisfiedArg, totalArgs.ToArray());
        }
    }
}
