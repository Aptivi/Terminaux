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
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
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
                for (int i = 0; i <= ArgumentsInput.Length - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    if (arguments.TryGetValue(ArgumentName, out ArgumentInfo? argInfoVal))
                    {
                        // Variables
                        var (satisfied, total) = ArgumentsParser.ParseArgumentArguments(Argument, arguments);
                        for (int j = 0; j < total.Length; j++)
                        {
                            var ArgumentInfo = total[j];
                            var Arg = argInfoVal;
                            var Args = ArgumentInfo.ArgumentsList;
                            var ArgOrig = ArgumentInfo.ArgumentsTextOrig;
                            var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
                            var Switches = ArgumentInfo.SwitchesList;
                            string strArgs = ArgumentInfo.ArgumentsText;
                            bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                            // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                            for (int idx = 0; idx < Arg.ArgArgumentInfo.Length; idx++)
                            {
                                var argInfo = Arg.ArgArgumentInfo[idx];
                                bool isLast = idx == Arg.ArgArgumentInfo.Length - 1 && j == total.Length - 1;
                                if (argInfo.ArgumentsRequired & RequiredArgumentsProvided | !argInfo.ArgumentsRequired)
                                {
                                    // Prepare the argument parameter instance
                                    var parameters = new ArgumentParameters(strArgs, Args, ArgOrig, ArgsOrig, Switches, Argument);

                                    // Now, get the base command and execute it
                                    var ArgumentBase = Arg.ArgumentBase;
                                    ArgumentBase.Execute(parameters);
                                }
                                else if (isLast)
                                    TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_NOTENOUGHARGS"), ConsoleColors.Red);
                            }
                        }
                    }
                    else
                        TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNKNOWNARG") + $" {Argument}", ConsoleColors.Red);
                }
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteColor(LanguageTools.GetLocalized("T_SHELL_BASE_ARGPARSE_UNRECOVERABLEERROR") + $": {ex.Message}", ConsoleColors.Red);
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
                        break;
                }
                return found;
            }
            catch
            {
                return false;
            }
        }

        private static string[] FinalizeArguments(string[] argumentsInput, Dictionary<string, ArgumentInfo> arguments)
        {
            // Parse the arguments. Assume that unknown arguments are parameters of arguments
            List<string> finalArguments = [];
            StringBuilder builder = new();
            foreach (var argInput in argumentsInput)
            {
                if (arguments.TryGetValue(argInput, out ArgumentInfo? argInfoVal))
                {
                    // If we came across a valid argument, add the result and clear the builder
                    if (builder.Length > 0)
                    {
                        finalArguments.Add(builder.ToString().Trim());
                        builder.Clear();
                    }
                }

                // Add the argument name
                builder.Append(argInput + " ");
            }
            if (builder.Length > 0)
                finalArguments.Add(builder.ToString().Trim());
            return [.. finalArguments];
        }
    }
}
