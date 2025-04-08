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

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Terminaux.Base;

namespace Terminaux.Reader.History
{
    /// <summary>
    /// History tools for the terminal reader
    /// </summary>
    public static class HistoryTools
    {
        internal const string generalHistory = "General";
        internal static List<HistoryInfo> histories =
        [
            new(generalHistory, [])
        ];

        /// <summary>
        /// Gets a list of history names
        /// </summary>
        public static string[] HistoryNames =>
            histories.Select((hi) => hi.HistoryName).ToArray();

        /// <summary>
        /// Checks whether the history is registered or not
        /// </summary>
        /// <param name="historyName">History name to query</param>
        /// <returns>True if registered; false if not</returns>
        public static bool IsHistoryRegistered(string historyName) =>
            HistoryNames.Contains(historyName);

        /// <summary>
        /// Loads the history from a file
        /// </summary>
        /// <param name="filePath">File path containing JSON representation of a history instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void LoadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new TerminauxException("History file path is not provided.");
            string fileContents = File.ReadAllText(filePath);
            LoadFromJson(fileContents);
        }

        /// <summary>
        /// Loads the history from a JSON representation
        /// </summary>
        /// <param name="historyJson">JSON representation of a history instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void LoadFromJson([StringSyntax(StringSyntaxAttribute.Json)] string historyJson)
        {
            if (string.IsNullOrEmpty(historyJson))
                throw new TerminauxException("History JSON representation is not provided.");
            var info = JsonConvert.DeserializeObject<HistoryInfo>(historyJson) ??
                throw new TerminauxException("JSON parsed, but history JSON representation is not provided.");
            LoadFromInstance(info);
        }

        /// <summary>
        /// Loads the history from an instance
        /// </summary>
        /// <param name="info">History instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void LoadFromInstance(HistoryInfo info)
        {
            if (info is null)
                throw new TerminauxException("History info is not provided.");
            if (string.IsNullOrEmpty(info.HistoryName))
                throw new TerminauxException("History name is not provided. Are you sure that you have specified the name in your JSON representation?");
            if (info.HistoryEntries is null)
                throw new TerminauxException("History entries are not provided. Are you sure that you have specified the entries in your JSON representation?");
            ConsoleLogger.Debug("Adding history {0} with {1} entries", info.HistoryName, info.HistoryEntries.Count);
            histories.Add(info);
        }

        /// <summary>
        /// Unloads a history from a history name
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Unload(string historyName)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            if (historyName == generalHistory)
                throw new TerminauxException("General history can't be removed, but can be cleared.");
            int idx = GetHistoryIndexFrom(historyName);
            HistoryInfo info = histories[idx];
            Unload(info);
        }

        /// <summary>
        /// Unloads a history from a history info instance
        /// </summary>
        /// <param name="info">History info instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Unload(HistoryInfo info)
        {
            if (info is null)
                throw new TerminauxException("History info is not provided.");
            if (!histories.Contains(info))
                throw new TerminauxException("History is not part of the registered histories.");
            if (info.HistoryName == generalHistory)
                throw new TerminauxException("General history can't be removed, but can be cleared.");
            ConsoleLogger.Debug("Removing history {0} with {1} entries", info.HistoryName, info.HistoryEntries.Count);
            if (!histories.Remove(info))
                throw new TerminauxException("Failed to delete history.");
        }

        /// <summary>
        /// Gets the history entries from the history name
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <returns>History entries</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string[] GetHistoryEntries(string historyName)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            int idx = GetHistoryIndexFrom(historyName);
            HistoryInfo info = histories[idx];
            ConsoleLogger.Debug("Returning {0} entries", info.HistoryEntries.Count);
            return [.. info.HistoryEntries];
        }

        /// <summary>
        /// Clears the history entries
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Clear(string historyName)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Clearing {0} entries from {1}", histories[idx].HistoryEntries.Count, historyName);
            histories[idx].HistoryEntries.Clear();
        }

        /// <summary>
        /// Appends an entry at the end of the history entry list
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="entry">Entry to append</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Append(string historyName, string entry)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            if (entry is null)
                throw new TerminauxException("History entry is not provided.");
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Adding {0} to {1}", entry, historyName);
            histories[idx].HistoryEntries.Add(entry);
        }

        /// <summary>
        /// Replaces the current entry list with the specified entry list
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="entries">Entries to replace the current entry with</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Switch(string historyName, string[] entries)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            if (entries is null)
                throw new TerminauxException("History entry list is not provided.");
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Switching {0} entries in {1}", histories[idx].HistoryEntries.Count, historyName);
            histories[idx].HistoryEntries.Clear();
            histories[idx].HistoryEntries.AddRange(entries);
        }

        /// <summary>
        /// Saves a history to a JSON representation
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <returns>A string containing a JSON representation of the history</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string SaveToString(string historyName)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException($"History {historyName} not found.");
            int idx = GetHistoryIndexFrom(historyName);
            return SaveToString(histories[idx]);
        }

        /// <summary>
        /// Saves a history to a JSON representation
        /// </summary>
        /// <param name="info">History info instance</param>
        /// <returns>A string containing a JSON representation of the history</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string SaveToString(HistoryInfo info)
        {
            if (info is null)
                throw new TerminauxException("History info is not provided.");
            if (!histories.Contains(info))
                throw new TerminauxException("History is not part of the registered histories.");
            string historyInfoString = JsonConvert.SerializeObject(info, Formatting.Indented);
            return historyInfoString;
        }

        private static int GetHistoryIndexFrom(string historyName)
        {
            for (int i = 0; i < HistoryNames.Length; i++)
                if (HistoryNames[i] == historyName)
                    return i;
            return -1;
        }
    }
}
