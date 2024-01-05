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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// The blacklist module for the terminal type
    /// </summary>
    public static class TerminalTypeBlacklist
    {
        internal static List<(Func<string, bool>, string)> baseBlacklist = [];
        internal static List<(Func<string, bool>, string)> customBlacklist = [];

        /// <summary>
        /// Adds a match for the terminal type to the blacklist
        /// </summary>
        /// <param name="typeQuery">The type query for the console type to match</param>
        /// <param name="justification">Reason for the blacklist</param>
        public static void AddToBlacklist(Func<string, bool> typeQuery, string justification)
        {
            // Check the query first
            if (typeQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, add the query and the justification to the list
            if (!IsQueryBlacklisted(typeQuery, out _))
                customBlacklist.Add((typeQuery, justification));
        }

        /// <summary>
        /// Removes a match for the terminal type from the blacklist
        /// </summary>
        /// <param name="typeQuery">The type query for the console type to match</param>
        public static void RemoveFromBlacklist(Func<string, bool> typeQuery)
        {
            // Check the query first
            if (typeQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, remove the query from the list
            if (IsQueryBlacklisted(typeQuery, out var query))
                customBlacklist.Remove(query);
        }

        /// <summary>
        /// Is the query blacklisted?
        /// </summary>
        /// <param name="typeQuery">The query to check</param>
        /// <param name="queryPair">Output query pair</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsQueryBlacklisted(Func<string, bool> typeQuery, out (Func<string, bool>, string) queryPair)
        {
            // Check the query first
            if (typeQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, check the list for the query
            var types = GetBlacklistedTypes();
            foreach (var type in types)
            {
                if (type.Item1 == typeQuery)
                {
                    queryPair = type;
                    return true;
                }
            }
            queryPair = (null, "");
            return false;
        }

        /// <summary>
        /// Gets the blacklisted terminal type queries
        /// </summary>
        /// <returns>Terminal types with their matches and their justifications</returns>
        public static (Func<string, bool>, string)[] GetBlacklistedTypes() =>
            baseBlacklist.Union(customBlacklist).ToArray();

        /// <summary>
        /// Checks to see if the current terminal type is blacklisted
        /// </summary>
        /// <returns>True if blacklisted; false otherwise</returns>
        public static (bool blacklisted, string justification) IsTypeBlacklisted() =>
            IsTypeBlacklisted(ConsolePlatform.GetTerminalType());

        internal static (bool blacklisted, string justification) IsTypeBlacklisted(string terminalType)
        {
            if (string.IsNullOrEmpty(terminalType))
                return (false, "");

            var types = GetBlacklistedTypes();
            foreach (var type in types)
            {
                if (type.Item1(terminalType))
                    return (true, type.Item2);
            }
            return (false, "");
        }
    }
}
