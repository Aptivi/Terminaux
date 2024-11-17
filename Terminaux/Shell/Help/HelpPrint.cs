//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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

using Terminaux.Shell.Shells;

namespace Terminaux.Shell.Help
{
    /// <summary>
    /// Help system for shells module
    /// </summary>
    public static class HelpPrint
    {
        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelp(bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false) =>
            ShowHelpExtended("", ShellManager.CurrentShellType, false, showGeneral, showAlias, showUnified, showExtra, showCount);

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelp(string command, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false) =>
            ShowHelpExtended(command, ShellManager.CurrentShellType, false, showGeneral, showAlias, showUnified, showExtra, showCount);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelp(string command, string commandType, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false) =>
            ShowHelpExtended(command, commandType, false, showGeneral, showAlias, showUnified, showExtra, showCount);

        /// <summary>
        /// Shows the list of commands under the current shell type
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelpExtended(bool simplified = false, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false) =>
            ShowHelpExtended("", ShellManager.CurrentShellType, simplified, showGeneral, showAlias, showUnified, showExtra, showCount);

        /// <summary>
        /// Shows the help of a command, or command list under the current shell type if nothing is specified
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="command">A specified command</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelpExtended(string command, bool simplified = false, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false) =>
            ShowHelpExtended(command, ShellManager.CurrentShellType, simplified, showGeneral, showAlias, showUnified, showExtra, showCount);

        /// <summary>
        /// Shows the help of a command, or command list under the specified shell type if nothing is specified
        /// </summary>
        /// <param name="simplified">Uses simplified help</param>
        /// <param name="command">A specified command</param>
        /// <param name="commandType">A specified shell type</param>
        /// <param name="showGeneral">Shows all general commands</param>
        /// <param name="showCount">Shows command count</param>
        /// <param name="showAlias">Shows all aliased commands</param>
        /// <param name="showUnified">Shows all unified commands</param>
        /// <param name="showExtra">Shows all kernel extra commands</param>
        public static void ShowHelpExtended(string command, string commandType, bool simplified = false, bool showGeneral = true, bool showAlias = false, bool showUnified = false, bool showExtra = false, bool showCount = false)
        {
            // Check to see if command exists
            if (!string.IsNullOrWhiteSpace(command))
                HelpPrintTools.ShowHelpUsage(command, commandType);
            else if (string.IsNullOrWhiteSpace(command))
            {
                // List the available commands
                if (simplified)
                    HelpPrintTools.ShowCommandListSimplified(commandType);
                else
                    HelpPrintTools.ShowCommandList(commandType, showGeneral, showAlias, showUnified, showExtra, showCount);
            }
        }
    }
}
