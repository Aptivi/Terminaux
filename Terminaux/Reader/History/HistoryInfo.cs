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

using Newtonsoft.Json;
using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Reader.History
{
    /// <summary>
    /// Terminal reader history information class
    /// </summary>
    public class HistoryInfo
    {
        /// <summary>
        /// History name
        /// </summary>
        [JsonProperty(nameof(HistoryName))]
        public string HistoryName { get; internal set; } = "";

        /// <summary>
        /// History entries
        /// </summary>
        [JsonProperty(nameof(HistoryEntries))]
        public List<string> HistoryEntries { get; } = [];

        [JsonConstructor]
        private HistoryInfo()
        { }

        /// <summary>
        /// Makes a new instance of the history info class
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="historyEntries">History entries</param>
        /// <exception cref="TerminauxException"></exception>
        public HistoryInfo(string historyName, List<string> historyEntries)
        {
            if (string.IsNullOrEmpty(historyName))
                throw new TerminauxException("History name is not specified");
            if (historyEntries is null)
                throw new TerminauxException("History entries are not specified");
            HistoryName = historyName;
            HistoryEntries = historyEntries;
        }
    }
}
