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
    /// The blacklist module for the terminal emulator
    /// </summary>
    public static class TerminalEmulatorBlacklist
    {
        internal static List<(Func<string, bool>, string)> baseBlacklist =
        [
            (new((query) => query == "Apple_Terminal"), "This application makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors.")
        ];
        internal static List<(Func<string, bool>, string)> customBlacklist = [];

        /// <summary>
        /// Adds a match for the terminal emulator to the blacklist
        /// </summary>
        /// <param name="emulatorQuery">The emulator query for the terminal emulator to match</param>
        /// <param name="justification">Reason for the blacklist</param>
        public static void AddToBlacklist(Func<string, bool> emulatorQuery, string justification)
        {
            // Check the query first
            if (emulatorQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, add the query and the justification to the list
            if (!IsQueryBlacklisted(emulatorQuery, out _))
                customBlacklist.Add((emulatorQuery, justification));
        }

        /// <summary>
        /// Removes a match for the terminal emulator from the blacklist
        /// </summary>
        /// <param name="emulatorQuery">The emulator query for the terminal emulator to match</param>
        public static void RemoveFromBlacklist(Func<string, bool> emulatorQuery)
        {
            // Check the query first
            if (emulatorQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, remove the query from the list
            if (IsQueryBlacklisted(emulatorQuery, out var query))
                customBlacklist.Remove(query);
        }

        /// <summary>
        /// Is the query blacklisted?
        /// </summary>
        /// <param name="emulatorQuery">The query to check</param>
        /// <param name="queryPair">Output query pair</param>
        /// <returns>True if found; false otherwise.</returns>
        /// <exception cref="TerminauxException"></exception>
        public static bool IsQueryBlacklisted(Func<string, bool> emulatorQuery, out (Func<string, bool>, string) queryPair)
        {
            // Check the query first
            if (emulatorQuery is null)
                throw new TerminauxException("Can't blacklist without any query.");

            // Now, check the list for the query
            var emulators = GetBlacklistedEmulators();
            foreach (var emulator in emulators)
            {
                if (emulator.Item1 == emulatorQuery)
                {
                    queryPair = emulator;
                    return true;
                }
            }
            queryPair = (null, "");
            return false;
        }

        /// <summary>
        /// Gets the blacklisted terminal emulator queries
        /// </summary>
        /// <returns>Terminal emulators with their matches and their justifications</returns>
        public static (Func<string, bool>, string)[] GetBlacklistedEmulators() =>
            baseBlacklist.Union(customBlacklist).ToArray();

        /// <summary>
        /// Checks to see if the current terminal emulator is blacklisted
        /// </summary>
        /// <returns>True if blacklisted; false otherwise</returns>
        public static (bool blacklisted, string justification) IsEmulatorBlacklisted() =>
            IsEmulatorBlacklisted(ConsolePlatform.GetTerminalEmulator());

        internal static (bool blacklisted, string justification) IsEmulatorBlacklisted(string terminalEmulator)
        {
            var emulators = GetBlacklistedEmulators();
            foreach (var emulator in emulators)
            {
                if (emulator.Item1(terminalEmulator))
                    return (true, emulator.Item2);
            }
            return (false, "");
        }
    }
}
