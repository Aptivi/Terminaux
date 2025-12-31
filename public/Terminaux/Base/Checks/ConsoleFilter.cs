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

using SpecProbe.Software.Platform;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Textify.Tools;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// Console filter tools
    /// </summary>
    public static class ConsoleFilter
    {
        internal static List<ConsoleFilterInfo> baseFilters =
        [
            new(new("dumb"), ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist, LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_BASEFILTERS_DUMB_JUSTIFICATION")),
            new(new("unknown"), ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist, LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_BASEFILTERS_UNKNOWN_JUSTIFICATION")),
            new(new("Apple_Terminal"), ConsoleFilterType.Emulator, ConsoleFilterSeverity.Blacklist, LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_BASEFILTERS_APPLETERMINAL_JUSTIFICATION")),
            new(new(@"^((?!-256col).)*$"), ConsoleFilterType.Type, ConsoleFilterSeverity.Graylist, LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_BASEFILTERS_NO256COLORS_JUSTIFICATION")),
        ];
        internal static List<ConsoleFilterInfo> customFilters = [];

        /// <summary>
        /// Adds a match for the terminal type or emulator to the blacklist or the graylist
        /// </summary>
        /// <param name="query">The type query for the console type to match</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="justification">Reason for the blacklist</param>
        public static void AddToFilter(string query, ConsoleFilterType type, ConsoleFilterSeverity severity, string justification) =>
            AddToFilter(new Regex(query), type, severity, justification);

        /// <summary>
        /// Adds a match for the terminal type or emulator to the blacklist or the graylist
        /// </summary>
        /// <param name="query">The type query for the console type to match</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="justification">Reason for the blacklist</param>
        public static void AddToFilter(Regex query, ConsoleFilterType type, ConsoleFilterSeverity severity, string justification)
        {
            // Check the query first
            if (query is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_NOQUERY"));
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_INVALIDQUERY"));

            // Now, add the query and the justification to the list
            if (!IsInFilter(query, type, severity, out _))
            {
                customFilters.Add(new ConsoleFilterInfo(query, type, severity, justification));
                ConsoleLogger.Debug("Adding to filter...");
            }
        }

        /// <summary>
        /// Removes a match for the terminal type from the blacklist
        /// </summary>
        /// <param name="query">The type query for the console type to match</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        public static void RemoveFromFilter(string query, ConsoleFilterType type, ConsoleFilterSeverity severity) =>
            RemoveFromFilter(new Regex(query), type, severity);

        /// <summary>
        /// Removes a match for the terminal type from the blacklist
        /// </summary>
        /// <param name="query">The type query for the console type to match</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        public static void RemoveFromFilter(Regex query, ConsoleFilterType type, ConsoleFilterSeverity severity)
        {
            // Check the query first
            if (query is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_NOQUERY"));
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_INVALIDQUERY"));

            // Now, remove the query from the list
            if (IsInFilter(query, type, severity, out var queryInfo))
            {
                customFilters.Remove(queryInfo!);
                ConsoleLogger.Debug("Removing from filter...");
            }
        }

        /// <summary>
        /// Is the query in the filter?
        /// </summary>
        /// <param name="query">The query to check</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="queryInfo">Output query Info</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsInFilter(string query, ConsoleFilterType type, ConsoleFilterSeverity severity, out ConsoleFilterInfo? queryInfo) =>
            IsInFilter(new Regex(query), type, severity, out queryInfo);

        /// <summary>
        /// Is the query in the filter?
        /// </summary>
        /// <param name="query">The query to check</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="queryInfo">Output query Info</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsInFilter(Regex query, ConsoleFilterType type, ConsoleFilterSeverity severity, out ConsoleFilterInfo? queryInfo)
        {
            // Check the query first
            if (query is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_NOQUERY"));
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException(LanguageTools.GetLocalized("T_BC_CONSOLEFILTER_EXCEPTION_INVALIDQUERY"));

            // Now, check the list for the query
            var queries = GetFilteredQueries();
            foreach (var filtered in queries)
            {
                if (filtered.Expression == query && filtered.Type == type && filtered.Severity == severity)
                {
                    ConsoleLogger.Debug("Found this filter with type {0}, {1}, {2}...", filtered.Type, filtered.Severity, filtered.Expression.ToString());
                    queryInfo = filtered;
                    return true;
                }
            }
            queryInfo = null;
            return false;
        }

        /// <summary>
        /// Gets the filtered queries
        /// </summary>
        /// <returns>Terminal queries with their matches, their types, their severities, and their justifications</returns>
        public static ConsoleFilterInfo[] GetFilteredQueries() =>
            baseFilters.Union(customFilters).ToArray();

        /// <summary>
        /// Checks to see if the current console is filtered
        /// </summary>
        /// <returns>True if filtered; false otherwise</returns>
        public static (bool filtered, string justification) IsConsoleFiltered(ConsoleFilterType type, ConsoleFilterSeverity severity) =>
            IsConsoleFiltered(type == ConsoleFilterType.Type ? PlatformHelper.GetTerminalType() : PlatformHelper.GetTerminalEmulator(), type, severity);

        internal static (bool filtered, string justification) IsConsoleFiltered(string console, ConsoleFilterType type, ConsoleFilterSeverity severity)
        {
            if (string.IsNullOrEmpty(console))
                return (false, "");

            var queries = GetFilteredQueries();
            foreach (var filtered in queries)
            {
                if (filtered.Expression is not null && filtered.Expression.IsMatch(console) && filtered.Type == type && filtered.Severity == severity)
                {
                    ConsoleLogger.Debug("Found console {0} while matching this filter with {1}, {2}, {3}...", console, filtered.Type, filtered.Severity, filtered.Expression.ToString());
                    return (true, filtered.Justification);
                }
            }
            return (false, "");
        }
    }
}
