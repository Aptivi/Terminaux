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
using System.Text;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Shell.Arguments.Base
{
    /// <summary>
    /// Argument parser class
    /// </summary>
    public static class ArgumentParse
    {
        /// <summary>
        /// Parses specified arguments (modern way)
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        /// <param name="arguments">A dictionary of argument info instances</param>
        public static void ParseArguments(string[]? ArgumentsInput, Dictionary<string, ArgumentInfo> arguments)
        {
            ArgumentsInput ??= [];
            string[] finalArguments = FinalizeArguments(ArgumentsInput, arguments);
            ParseArgumentsInternal(finalArguments, arguments);
        }
        
        /// <summary>
        /// Parses specified arguments (legacy way)
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        /// <param name="arguments">A dictionary of argument info instances</param>
        public static void ParseArgumentsLegacy(string[]? ArgumentsInput, Dictionary<string, ArgumentInfo> arguments) =>
            ParseArgumentsInternal(ArgumentsInput, arguments);

        internal static void ParseArgumentsInternal(string[]? ArgumentsInput, Dictionary<string, ArgumentInfo> arguments)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput ??= [];

                // Parse them now
                ConsoleLogger.Debug("{0} argument input strings [{1}]", ArgumentsInput.Length, string.Join(", ", ArgumentsInput));
                for (int i = 0; i <= ArgumentsInput.Length - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    ConsoleLogger.Debug("Checking {0} from [{1}]", ArgumentName, string.Join(", ", arguments.Keys));
                    if (arguments.TryGetValue(ArgumentName, out ArgumentInfo? argInfoVal))
                    {
                        // Variables
                        var (satisfied, total) = ArgumentsParser.ParseArgumentArguments(Argument, arguments);
                        ConsoleLogger.Debug("Parsed argument {0} and found {1} total arguments.", Argument, total.Length);
                        bool argSatisfied = satisfied is not null;
                        if (satisfied is null)
                        {
                            ConsoleLogger.Warning("Arguments not satisfied.");
                            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGSNOTPROVIDEDLIST"), ThemeColorType.Error);
                            for (int t = 0; t < total.Length; t++)
                            {
                                ProvidedArgumentsInfo unsatisfied = total[t];
                                string command = unsatisfied.Command;
                                var argInfo = unsatisfied.ArgumentInfo;
                                if (argInfo is null)
                                {
                                    TextWriterRaw.WriteRaw(new ListEntry()
                                    {
                                        Entry = $"- [{t + 1}] {command}: ",
                                        Value = LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNKNOWNARG")
                                    }.Render() + "\n");
                                    continue;
                                }

                                // Write usage number
                                string renderedUsage = !string.IsNullOrEmpty(argInfo.RenderedUsage) ? " " + argInfo.RenderedUsage : "";
                                TextWriterColor.Write($"- [{i + 1}] {command}{renderedUsage}", ThemeColorType.ListEntry);

                                // Check for required arguments
                                if (!unsatisfied.RequiredArgumentsProvided)
                                {
                                    ConsoleLogger.Warning("User hasn't provided enough arguments for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGSNOTPROVIDED"), ThemeColorType.ListValue);
                                }

                                // Check for required switches
                                if (!unsatisfied.RequiredSwitchesProvided)
                                {
                                    ConsoleLogger.Warning("User hasn't provided enough switches for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNOTPROVIDED"), ThemeColorType.ListValue);
                                }

                                // Check for required switch arguments
                                if (!unsatisfied.RequiredSwitchArgumentsProvided)
                                {
                                    ConsoleLogger.Warning("User hasn't provided a value for one of the switches for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNEEDVALUE"), ThemeColorType.ListValue);
                                }

                                // Check for unknown switches
                                if (unsatisfied.UnknownSwitchesList.Length > 0)
                                {
                                    ConsoleLogger.Warning("User has provided unknown switches {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_UNKNOWNSWITCHES"), ThemeColorType.ListValue);
                                    TextWriterRaw.WriteRaw(new Listing()
                                    {
                                        Objects = unsatisfied.UnknownSwitchesList,
                                    }.Render() + "\n");
                                }

                                // Check for conflicting switches
                                if (unsatisfied.ConflictingSwitchesList.Length > 0)
                                {
                                    ConsoleLogger.Warning("User has provided conflicting switches for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESCONFLICT"), ThemeColorType.ListValue);
                                    TextWriterRaw.WriteRaw(new Listing()
                                    {
                                        Objects = unsatisfied.ConflictingSwitchesList,
                                    }.Render() + "\n");
                                }

                                // Check for switches that don't accept values
                                if (unsatisfied.NoValueSwitchesList.Length > 0)
                                {
                                    ConsoleLogger.Warning("User has provided switches that don't accept values for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHESNOVALUES"), ThemeColorType.ListValue);
                                    TextWriterRaw.WriteRaw(new Listing()
                                    {
                                        Objects = unsatisfied.NoValueSwitchesList,
                                    }.Render() + "\n");
                                }

                                // Check for invalid number in numeric arguments
                                if (!unsatisfied.NumberProvided)
                                {
                                    ConsoleLogger.Warning("User has provided invalid number for one or more of the arguments for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGNUMERIC"), ThemeColorType.ListValue);
                                }

                                // Check for invalid exact wording
                                if (!unsatisfied.ExactWordingProvided)
                                {
                                    ConsoleLogger.Warning("User has provided non-exact wording for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_ARGEXACT"), ThemeColorType.ListValue);
                                }

                                // Check for invalid number in numeric switches
                                if (!unsatisfied.SwitchNumberProvided)
                                {
                                    ConsoleLogger.Warning("User has provided invalid number for one or more of the switches for {0}", command);
                                    TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_SWITCHNUMERIC"), ThemeColorType.ListValue);
                                }
                            }
                            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_CONSULTHELP"), ThemeColorType.Error);
                            continue;
                        }

                        // Now, assume that an argument is satisfied
                        var ArgumentInfo = satisfied;
                        var Arg = argInfoVal;
                        var Args = ArgumentInfo.ArgumentsList;
                        var ArgOrig = ArgumentInfo.ArgumentsTextOrig;
                        var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
                        var Switches = ArgumentInfo.SwitchesList;
                        string strArgs = ArgumentInfo.ArgumentsText;

                        // Prepare the command parameter instance and run the argument handler if any
                        var argCheckerParameters = new CommandParameters(strArgs, Args, ArgOrig, ArgsOrig, Switches, Argument);
                        int argCheckerReturnCode = 0;
                        if (argSatisfied)
                        {
                            argCheckerReturnCode = satisfied.ArgumentInfo?.ArgChecker.Invoke(argCheckerParameters) ?? 0;
                            ConsoleLogger.Debug("Argument {0} with args {1} argument checker returned {2}", Argument, strArgs, argCheckerReturnCode);
                            argSatisfied = argCheckerReturnCode == 0;
                        }
                        if (!argSatisfied)
                        {
                            ConsoleLogger.Error("Not enough arguments {0}", ArgumentName);
                            TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_NOTENOUGHARGS"), ThemeColorType.Error);
                            continue;
                        }

                        // Prepare the argument parameter instance
                        ConsoleLogger.Debug("Argument {0} [P: {1}] [S: {2}], {3} info instances.", Argument, string.Join(", ", Args), string.Join(", ", Switches), Arg.ArgArgumentInfo.Length);
                        var parameters = new ArgumentParameters(strArgs, Args, ArgOrig, ArgsOrig, Switches, Argument);

                        // Now, get the base command and execute it
                        var ArgumentBase = Arg.ArgumentBase;
                        ConsoleLogger.Debug("Got argument base resolving to {0}, executing...", ArgumentName);
                        ArgumentBase.Execute(parameters);
                    }
                    else
                    {
                        ConsoleLogger.Error("No such argument {0}", ArgumentName);
                        TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNKNOWNARG") + $" {Argument}", ThemeColorType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Argument execution failed");
                TextWriterColor.Write(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNRECOVERABLEERROR") + $": {ex.Message}", ThemeColorType.Error);
            }
        }

        /// <summary>
        /// Checks to see if the specific argument name is passed
        /// </summary>
        /// <param name="ArgumentsInput">List of passed arguments</param>
        /// <param name="argumentName">Argument name to check</param>
        /// <param name="arguments">A dictionary of argument info instances</param>
        /// <returns>True if found in the arguments list and passed. False otherwise.</returns>
        public static bool IsArgumentPassed(string[]? ArgumentsInput, string argumentName, Dictionary<string, ArgumentInfo> arguments)
        {
            ArgumentsInput ??= [];
            string[] finalArguments = FinalizeArguments(ArgumentsInput, arguments);
            return IsArgumentPassedInternal(finalArguments, argumentName, arguments);
        }

        /// <summary>
        /// Checks to see if the specific argument name is passed (legacy way)
        /// </summary>
        /// <param name="ArgumentsInput">List of passed arguments</param>
        /// <param name="argumentName">Argument name to check</param>
        /// <param name="arguments">A dictionary of argument info instances</param>
        /// <returns>True if found in the arguments list and passed. False otherwise.</returns>
        public static bool IsArgumentPassedLegacy(string[]? ArgumentsInput, string argumentName, Dictionary<string, ArgumentInfo> arguments) =>
            IsArgumentPassedInternal(ArgumentsInput, argumentName, arguments);

        internal static bool IsArgumentPassedInternal(string[]? ArgumentsInput, string argumentName, Dictionary<string, ArgumentInfo> arguments)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput ??= [];

                // Parse them now
                bool found = false;
                for (int i = 0; i <= ArgumentsInput.Length - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    found = ArgumentName == argumentName && arguments.ContainsKey(ArgumentName);
                    if (found)
                    {
                        ConsoleLogger.Debug("Argument passed {0}", argumentName);
                        break;
                    }
                }
                return found;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Argument passed {0}", argumentName);
                return false;
            }
        }

        private static string[] FinalizeArguments(string[] argumentsInput, Dictionary<string, ArgumentInfo> arguments)
        {
            // Parse the arguments
            List<string> finalArguments = [];
            StringBuilder builder = new();
            foreach (var argInput in argumentsInput)
            {
                // Check to see if we're passing as value or as argument by the dash at the beginning of the word
                bool isArg = argInput.StartsWith("--");
                string argument = isArg ? argInput.RemovePrefix("--") : argInput;
                if (isArg)
                {
                    // If we came across an argument, add the result and clear the builder
                    if (builder.Length > 0)
                    {
                        ConsoleLogger.Debug("Adding result \"{0}\" to final arguments list", builder.ToString());
                        finalArguments.Add(builder.ToString().Trim());
                        builder.Clear();
                    }
                }

                // Add the argument name
                ConsoleLogger.Debug("Adding argument name {0}", argument);
                builder.Append(argument + " ");
            }
            if (builder.Length > 0)
            {
                ConsoleLogger.Debug("Adding final argument name {0}", builder.ToString());
                finalArguments.Add(builder.ToString().Trim());
            }

            // Verify the arguments
            for (int i = finalArguments.Count - 1; i >= 0; i--)
            {
                // Split the arguments
                string[] argumentSplit = finalArguments[i].Split(' ');
                if (argumentSplit.Length == 0)
                    continue;

                // Get the argument and verify it
                string argument = argumentSplit[0];
                if (!arguments.TryGetValue(argument, out ArgumentInfo argInfo) || argInfo.Argument != argument)
                    finalArguments.RemoveAt(i);
            }

            // Return the final arguments
            ConsoleLogger.Debug("Final argument count: {0}", finalArguments.Count);
            return [.. finalArguments];
        }
    }
}
