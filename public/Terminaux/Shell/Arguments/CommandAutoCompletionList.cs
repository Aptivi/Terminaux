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
using Terminaux.Base;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Scripting;
using Terminaux.Shell.Shells;

namespace Terminaux.Shell.Arguments
{
    /// <summary>
    /// The list of known command auto completion patterns
    /// </summary>
    public static class CommandAutoCompletionList
    {
        private static readonly Dictionary<string, Func<string[], string[]>> completions = new()
        {
            { "$variable",  (_) => MESHVariables.Variables.Keys.ToArray() },
            { "cmd",        (_) => PopulateCommands() },
            { "command",    (_) => PopulateCommands() },
            { "shell",      (_) => ShellManager.AvailableShells.Keys.ToArray() },
        };
        private static readonly Dictionary<string, Func<string[], string[]>> customCompletions = [];

        /// <summary>
        /// Registers the completion function
        /// </summary>
        /// <param name="expression">An expression to add (usually an argument name)</param>
        /// <param name="completionFunction">Completion function that returns suggestions</param>
        /// <exception cref="TerminauxException"></exception>
        public static void RegisterCompletionFunction(string expression, Func<string[], string[]> completionFunction)
        {
            if (IsCompletionFunctionRegistered(expression))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_CAC_EXCEPTION_COMPFUNCALREADYREGISTERED"));
            customCompletions.Add(expression, completionFunction);
        }

        /// <summary>
        /// Unregisters the completion function
        /// </summary>
        /// <param name="expression">An expression to add (usually an argument name)</param>
        /// <exception cref="TerminauxException"></exception>
        public static void UnregisterCompletionFunction(string expression)
        {
            if (!IsCompletionFunctionRegistered(expression))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_CAC_EXCEPTION_COMPFUNCNOTREGISTERED"));
            if (IsCompletionFunctionBuiltin(expression))
                throw new TerminauxException(LanguageTools.GetLocalized("T_SHELL_BASE_CAC_EXCEPTION_COMPFUNCREMOVEBUILTIN"));
            customCompletions.Remove(expression);
        }

        /// <summary>
        /// Checks to see if the completion function is registered or not
        /// </summary>
        /// <param name="expression">An expression to query</param>
        /// <returns>True if registered; false otherwise.</returns>
        public static bool IsCompletionFunctionRegistered(string expression) =>
            GetCompletionFunction(expression) is not null;

        /// <summary>
        /// Checks to see if the completion function is builtin or not
        /// </summary>
        /// <param name="expression">An expression to query</param>
        /// <returns>True if builtin; false otherwise.</returns>
        public static bool IsCompletionFunctionBuiltin(string expression) =>
            completions.ContainsKey(expression);

        /// <summary>
        /// Gets a completion function for a known expression
        /// </summary>
        /// <param name="expression">An expression to query</param>
        /// <returns>A function that points to the completion, or null if not found.</returns>
        public static Func<string[], string[]>? GetCompletionFunction(string expression)
        {
            expression = expression.ToLower();
            if (!completions.TryGetValue(expression, out Func<string[], string[]>? func))
                if (!customCompletions.TryGetValue(expression, out func))
                    return null;
            return func;
        }

        private static string[] PopulateCommands()
        {
            var shellType = ShellManager.CurrentShellType;
            var ShellCommands = CommandManager.GetCommandNames(shellType);
            return ShellCommands;
        }
    }
}
