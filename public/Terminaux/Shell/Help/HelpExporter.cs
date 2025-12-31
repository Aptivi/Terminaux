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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Shell.Aliases;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Textify.General;

namespace Terminaux.Shell.Help
{
    /// <summary>
    /// Help exporter for commands and arguments
    /// </summary>
    public static class HelpExporter
    {
        /// <summary>
        /// Exports a list of commands to the markdown format (for documentation)
        /// </summary>
        /// <param name="shellType">Type of the shell</param>
        /// <param name="commandTypes">Command types to export</param>
        /// <param name="showCount">Shows the command count</param>
        /// <param name="showHidden">Shows the hidden commands</param>
        /// <returns>Markdown fragment that contains commands</returns>
        public static string ExportToMarkdown(string shellType, HelpCommandType commandTypes = HelpCommandType.General, bool showCount = false, bool showHidden = false)
        {
            StringBuilder markdown = new();

            // Get the shell type and get the appropriate commands according to the command types
            var shellInfo = ShellManager.GetShellInfo(shellType);
            var commands = shellInfo.Commands;
            var extraCommands = shellInfo.extraCommands;
            var unifiedCommands = ShellManager.unifiedCommandDict;
            var aliasedCommands = AliasManager.GetAliasListFromType(shellType).Select((ai) => ai.TargetCommand).ToList();

            // Helper function to sanitize strings
            string SanitizeString(string target) =>
                target.ReplaceAllRange(["|", "<", ">"], ["\\|", "\\<", "\\>"]);
            string SanitizeStringSimple(string target) =>
                target.Replace("|", "\\|");

            // Helper function for commands
            void ProcessCommands(List<CommandInfo> commands)
            {
                markdown.AppendLine(
                    $"| {LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_TABLE_COMMAND")} " +
                    $"| {LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_TABLE_ALIAS")} " +
                    $"| {LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_TABLE_USAGE")} " +
                    $"| {LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_TABLE_DESCRIPTION")} " +
                    $"| {LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_TABLE_FLAGS")}");
                markdown.AppendLine("|:--------|:--------|:--------|:--------|:--------");
                foreach (var command in commands)
                {
                    if (!showHidden && command.Flags.HasFlag(CommandFlags.Hidden))
                        continue;

                    // Prepare the necessary variables for export
                    string commandName = SanitizeStringSimple(command.Command);
                    var aliases = command.Aliases;
                    string[] usages = [.. command.CommandArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                    string description = SanitizeString(LanguageTools.GetLocalized(command.HelpDefinition));
                    string flags = SanitizeString(command.Flags.ToString());

                    // Prepare the max threshold for usages and aliases
                    int maxThreshold = Math.Max(aliases.Length, usages.Length);
                    if (maxThreshold > 0)
                    {
                        for (int i = 0; i < maxThreshold; i++)
                        {
                            // Get the alias and the usage
                            string alias = i < aliases.Length ? $"`{SanitizeStringSimple(aliases[i].Alias)}`" : "";
                            string usage = i < usages.Length ? $"`{SanitizeStringSimple(usages[i])}`" : "";

                            // Export those variables to Markdown, escaping the pipe character if necessary
                            if (i == 0)
                                markdown.AppendLine($"| `{commandName}` | {alias} | {usage} | {description} | {flags}");
                            else
                                markdown.AppendLine($"|  | {alias} | {usage} |  |");
                        }
                    }
                    else
                        // See above
                        markdown.AppendLine($"| `{commandName}` |  |  | {description} | {flags}");
                }
                markdown.AppendLine();
            }

            // Make a table for general commands
            if (commandTypes.HasFlag(HelpCommandType.General))
            {
                markdown.Append("## ");
                markdown.Append(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_GENERALCMDS"));
                markdown.AppendLine($" {shellType}{(showCount ? $" [{commands.Count}]" : "")}\n");
                ProcessCommands(commands);
            }

            // Make a table for unified commands
            if (commandTypes.HasFlag(HelpCommandType.Unified))
            {
                markdown.Append("## ");
                markdown.Append(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_UNIFIEDCMDS"));
                markdown.AppendLine($" {shellType}{(showCount ? $" [{unifiedCommands.Count}]" : "")}\n");
                ProcessCommands(unifiedCommands);
            }

            // Make a table for aliased commands
            if (commandTypes.HasFlag(HelpCommandType.Aliases))
            {
                markdown.Append("## ");
                markdown.Append(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_ALIASEDCMDS"));
                markdown.AppendLine($" {shellType}{(showCount ? $" [{aliasedCommands.Count}]" : "")}\n");
                ProcessCommands(aliasedCommands);
            }

            // Make a table for extra commands
            if (commandTypes.HasFlag(HelpCommandType.Extras))
            {
                markdown.Append("## ");
                markdown.Append(LanguageTools.GetLocalized("T_SHELL_BASE_COMMAND_HELP_EXPORTED_EXTRACMDS"));
                markdown.AppendLine($" {shellType}{(showCount ? $" [{extraCommands.Count}]" : "")}\n");
                ProcessCommands(extraCommands);
            }

            // Return the result
            return markdown.ToString();
        }
    }
}
