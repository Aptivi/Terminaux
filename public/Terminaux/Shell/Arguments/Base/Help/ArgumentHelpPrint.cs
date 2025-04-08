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

using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Shell.Arguments.Base.Help
{
    /// <summary>
    /// Argument help system module
    /// </summary>
    public static class ArgumentHelpPrint
    {
        internal static bool acknowledged = false;

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="arguments">A dictionary of argument info instances</param>
        public static void ShowArgsHelp(Dictionary<string, ArgumentInfo> arguments) =>
            ShowArgsHelp("", arguments);

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="Argument">A specified argument</param>
        /// <param name="arguments">A dictionary of argument info instances</param>
        /// <param name="simple">Simple help printing</param>
        public static void ShowArgsHelp(string Argument, Dictionary<string, ArgumentInfo> arguments, bool simple = false)
        {
            acknowledged = true;

            // Check to see if argument exists
            ConsoleLogger.Debug("{0} arguments [{1}]", arguments.Count, string.Join(", ", arguments));
            if (!string.IsNullOrWhiteSpace(Argument))
            {
                ConsoleLogger.Debug("Printing argument help for {0}...", Argument);
                ArgumentHelpPrintTools.ShowHelpUsage(Argument, arguments);
            }
            else
            {
                // List the available arguments
                ConsoleLogger.Debug("Printing argument list (simple: {0})...", simple);
                if (!simple)
                    ArgumentHelpPrintTools.ShowArgumentList(arguments);
                else
                    ArgumentHelpPrintTools.ShowArgumentListSimple(arguments);
            }
        }

    }
}
