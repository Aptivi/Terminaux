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

using System.Text.RegularExpressions;

namespace Terminaux.Base.Checks
{
    /// <summary>
    /// Filtered console information
    /// </summary>
    public class ConsoleFilterInfo
    {
        private readonly Regex? expression;
        private readonly ConsoleFilterType filterType;
        private readonly ConsoleFilterSeverity severity;
        private readonly string justification = "Unknown reason";

        /// <summary>
        /// Expression to filter consoles
        /// </summary>
        public Regex Expression =>
            expression ?? throw new TerminauxException("There is no expression in the console filter.");

        /// <summary>
        /// Console filter type (console type or emulator)
        /// </summary>
        public ConsoleFilterType Type =>
            filterType;

        /// <summary>
        /// Console filter severity (blacklisted or graylisted)
        /// </summary>
        public ConsoleFilterSeverity Severity =>
            severity;

        /// <summary>
        /// Reason as to why this console type or emulator that matches the <see cref="Expression">expression</see> has been filtered
        /// </summary>
        public string Justification =>
            justification;

        internal ConsoleFilterInfo(Regex? expression, ConsoleFilterType filterType, ConsoleFilterSeverity severity, string justification)
        {
            this.expression = expression ??
                throw new TerminauxException("There is no expression in the console filter.");
            this.filterType = filterType;
            this.severity = severity;
            this.justification = justification ??
                "Unknown reason";
        }
    }
}
