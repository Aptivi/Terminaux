//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SpecProbe.Software.Platform;
using Terminaux.Base;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Textify.General;
using Textify.Tools;

namespace Terminaux.Shell.Scripting
{
    /// <summary>
    /// UESH shell variable manager
    /// </summary>
    public static class UESHVariables
    {

        internal static Dictionary<string, string> ShellVariables = new()
        {
            { "$FrameworkRid", PlatformHelper.GetCurrentGenericRid() },
            { "$CurrentSysCulture", CultureInfo.CurrentCulture.Name },
            { "$CurrentUiSysCulture", CultureInfo.CurrentUICulture.Name },
        };

        /// <summary>
        /// Checks to see if the variable name starts with the correct format
        /// </summary>
        /// <param name="var">A $variable name</param>
        /// <returns>Sanitized variable name</returns>
        public static string SanitizeVariableName(string var)
        {
            ConsoleLogger.Debug("Sanitizing variable {0}...", var);
            if (!var.StartsWith("$"))
            {
                ConsoleLogger.Warning("Unsanitized variable found. Prepending $...");
                var = $"${var}";
            }
            return var;
        }

        /// <summary>
        /// Initializes a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        public static void InitializeVariable(string var)
        {
            var = SanitizeVariableName(var);
            if (!ShellVariables.ContainsKey(var))
            {
                ShellVariables.Add(var, "");
                ConsoleLogger.Debug("Initialized variable {0}", var);
            }
        }

        /// <summary>
        /// Gets a value of a $variable on command line
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <param name="cmd">A command line in script</param>
        /// <param name="commandType">Command type</param>
        /// <returns>A command line in script that has a value of $variable</returns>
        public static string GetVariableCommand(string var, string cmd, string commandType)
        {
            var CommandArgumentsInfo = ArgumentsParser.ParseShellCommandArguments(cmd, commandType).total[0];
            string NewCommand = $"{CommandArgumentsInfo.Command} ";
            var commands = CommandManager.GetCommands(commandType);
            if (!CommandManager.IsCommandFound(CommandArgumentsInfo.Command) ||
                !ShellManager.GetShellInfo(commandType).Commands.Single((ci) => ci.Command == CommandArgumentsInfo.Command).CommandArgumentInfo.Any((cai) => cai.AcceptsSet))
            {
                foreach (string Word in CommandArgumentsInfo.ArgumentsList)
                {
                    string finalWord = Word;
                    if (finalWord.Contains(var) & finalWord.StartsWith("$"))
                    {
                        finalWord = ShellVariables[var];
                    }
                    NewCommand += $"{finalWord} ";
                }
                ConsoleLogger.Debug("Replaced variable {0} with their values. Result: {1}", var, NewCommand);
                return NewCommand.TrimEnd(' ');
            }
            return cmd;
        }

        /// <summary>
        /// Gets a value of a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <returns>A value of $variable, or a variable name if not found</returns>
        public static string GetVariable(string var)
        {
            try
            {
                string FinalVar = SanitizeVariableName(var);
                if (ShellVariables.TryGetValue(FinalVar, out string? variableValue))
                    return variableValue;
                return var;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Error getting variable {0}: {1}", var, ex.Message);
            }
            return var;
        }

        /// <summary>
        /// Gets the variables and returns the available variables as a dictionary
        /// </summary>
        public static Dictionary<string, string> Variables =>
            ShellVariables;

        /// <summary>
        /// Sets a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <param name="value">A value to set to $variable</param>
        public static bool SetVariable(string var, string value)
        {
            try
            {
                var = SanitizeVariableName(var);
                if (!ShellVariables.ContainsKey(var))
                    InitializeVariable(var);
                ShellVariables[var] = value;
                ConsoleLogger.Debug("Set variable {0} to {1}", var, value);
                return true;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Error setting variable {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Makes an array of a $variable with the chosen number of values (e.g. $variable[0] = first value, $variable[1] = second value, ...)
        /// </summary>
        /// <param name="var">A $variable array name</param>
        /// <param name="values">A set of values to set</param>
        public static bool SetVariables(string var, string[] values)
        {
            try
            {
                var = SanitizeVariableName(var);
                for (int ValueIndex = 0; ValueIndex <= values.Length - 1; ValueIndex++)
                {
                    string VarName = $"{var}[{ValueIndex}]";
                    string VarValue = values[ValueIndex];
                    if (!ShellVariables.ContainsKey(VarName))
                        InitializeVariable(VarName);
                    ShellVariables[VarName] = VarValue;
                    ConsoleLogger.Debug("Set variable {0} to {1}", VarName, VarValue);
                }
                return true;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Error creating variable array {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Removes a variable from the shell variables dictionary
        /// </summary>
        /// <param name="var">Target variable</param>
        public static bool RemoveVariable(string var)
        {
            try
            {
                var = SanitizeVariableName(var);
                return ShellVariables.Remove(var);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Error removing variable {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Parses the system environment variables and converts them to the UESH shell variables
        /// </summary>
        public static void ConvertSystemEnvironmentVariables()
        {
            var EnvVars = Environment.GetEnvironmentVariables();
            foreach (string EnvVar in EnvVars.Keys)
                SetVariable(EnvVar, EnvVars[EnvVar]?.ToString() ?? "");
        }

        /// <summary>
        /// Initializes the UESH variables from the expression
        /// </summary>
        /// <param name="expression">UESH variable expressions in the form of $var=value</param>
        public static void InitializeVariablesFrom(string expression)
        {
            // Get the variable keys and values
            expression = string.IsNullOrEmpty(expression) ? "" : expression;
            var varTuple = GetVariablesFrom(expression);
            var varStoreKeys = varTuple.varStoreKeys;
            var varStoreValues = varTuple.varStoreValues;

            // Now, initialize the variables one by one
            for (int i = 0; i < varStoreKeys.Length; i++)
            {
                // Get the key and the value
                string varStoreKey = varStoreKeys[i];
                string varStoreValue = varStoreValues[i].ReleaseDoubleQuotes();
                ConsoleLogger.Debug("Adding {0} to {1}...", varStoreValue, varStoreKey);

                // Initialize each variable and set them
                InitializeVariable(varStoreKey);
                bool result = SetVariable(varStoreKey, varStoreValue);
                ConsoleLogger.Debug("Added {0} to {1}: {2}", varStoreValue, varStoreKey, result);
            }
        }

        /// <summary>
        /// Gets the UESH variables from the expression
        /// </summary>
        /// <param name="expression">UESH variable expressions in the form of $var=value</param>
        public static (string[] varStoreKeys, string[] varStoreValues) GetVariablesFrom(string expression)
        {
            // Get the variable keys and values
            expression = string.IsNullOrEmpty(expression) ? "" : expression;
            string localVarStoreValuesMatch = /* lang=regex */
                @"((?'key'\$\S+)=(?'value'(""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s\)]|\\.)+))+";
            var varStoreMatches = RegexTools.Matches(expression, localVarStoreValuesMatch);
            List<string> storeKeys = [];
            List<string> storeValues = [];
            foreach (Match match in varStoreMatches)
            {
                storeKeys.Add(match.Groups["key"].Value);
                storeValues.Add(match.Groups["value"].Value);
            }
            string[] varStoreKeys = [.. storeKeys];
            string[] varStoreValues = [.. storeValues];
            ConsoleLogger.Debug("Keys: {0} [{1}], Values: {2} [{3}]", varStoreKeys.Length, string.Join(", ", varStoreKeys), varStoreValues.Length, string.Join(", ", varStoreValues));
            return (varStoreKeys, varStoreValues);
        }

    }
}
