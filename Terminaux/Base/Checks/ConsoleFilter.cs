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
        internal static List<(Regex?, ConsoleFilterType, ConsoleFilterSeverity, string)> baseFilters =
        [
            (new("dumb"), ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist, "Console type only supports basic writing."),
            (new("unknown"), ConsoleFilterType.Type, ConsoleFilterSeverity.Blacklist, "Console type is of unknown nature."),
            (new("Apple_Terminal"), ConsoleFilterType.Emulator, ConsoleFilterSeverity.Blacklist, "This application makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors."),
            (new(@"^((?!-256col).)*$"), ConsoleFilterType.Type, ConsoleFilterSeverity.Graylist, "Console type doesn't support 256 colors."),
        ];
        internal static List<(Regex?, ConsoleFilterType, ConsoleFilterSeverity, string)> customFilters = [];

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
                throw new TerminauxException("Can't filter without any query.");
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException("Can't filter with an invalid query.");

            // Now, add the query and the justification to the list
            if (!IsInFilter(query, type, severity, out _))
                customFilters.Add((query, type, severity, justification));
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
                throw new TerminauxException("Can't filter without any query.");
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException("Can't filter with an invalid query.");

            // Now, remove the query from the list
            if (IsInFilter(query, type, severity, out var queryTuple))
                customFilters.Remove(queryTuple);
        }

        /// <summary>
        /// Is the query in the filter?
        /// </summary>
        /// <param name="query">The query to check</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="queryTuple">Output query tuple</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsInFilter(string query, ConsoleFilterType type, ConsoleFilterSeverity severity, out (Regex? query, ConsoleFilterType type, ConsoleFilterSeverity severity, string justification) queryTuple) =>
            IsInFilter(new Regex(query), type, severity, out queryTuple);

        /// <summary>
        /// Is the query in the filter?
        /// </summary>
        /// <param name="query">The query to check</param>
        /// <param name="type">Filter type</param>
        /// <param name="severity">Filter severity</param>
        /// <param name="queryTuple">Output query tuple</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsInFilter(Regex query, ConsoleFilterType type, ConsoleFilterSeverity severity, out (Regex? query, ConsoleFilterType type, ConsoleFilterSeverity severity, string justification) queryTuple)
        {
            // Check the query first
            if (query is null)
                throw new TerminauxException("Can't filter without any query.");
            if (!RegexTools.IsValidRegex(query))
                throw new TerminauxException("Can't filter with an invalid query.");

            // Now, check the list for the query
            var queries = GetFilteredQueries();
            foreach (var filtered in queries)
            {
                if (filtered.query == query && filtered.type == type && filtered.severity == severity)
                {
                    queryTuple = filtered;
                    return true;
                }
            }
            queryTuple = (null, (ConsoleFilterType)(-1), (ConsoleFilterSeverity)(-1), "");
            return false;
        }

        /// <summary>
        /// Gets the filtered queries
        /// </summary>
        /// <returns>Terminal queries with their matches, their types, their severities, and their justifications</returns>
        public static (Regex? query, ConsoleFilterType type, ConsoleFilterSeverity severity, string justification)[] GetFilteredQueries() =>
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
                if (filtered.query is not null && filtered.query.IsMatch(console) && filtered.type == type && filtered.severity == severity)
                    return (true, filtered.justification);
            }
            return (false, "");
        }
    }
}
