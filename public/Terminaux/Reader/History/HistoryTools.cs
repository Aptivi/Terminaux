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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Terminaux.Base;
using Terminaux.Base.Extensions;

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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYPATHNEEDED"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYJSONNEEDED"));
            var info = JsonConvert.DeserializeObject<HistoryInfo>(historyJson) ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYJSONMALFORMED"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYINFONEEDED"));
            if (string.IsNullOrEmpty(info.HistoryName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNAMENOTFOUND"));
            if (info.HistoryEntries is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYENTRIESNOTFOUND"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            if (historyName == generalHistory)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_UNLOADINGGENERALHISTORY"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYINFONEEDED"));
            if (!histories.Contains(info))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTREGISTERED"));
            if (info.HistoryName == generalHistory)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_UNLOADINGGENERALHISTORY"));
            ConsoleLogger.Debug("Removing history {0} with {1} entries", info.HistoryName, info.HistoryEntries.Count);
            if (!histories.Remove(info))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYUNLOADFAILED"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            int idx = GetHistoryIndexFrom(historyName);
            HistoryInfo info = histories[idx];
            ConsoleLogger.Debug("Returning {0} entries", info.HistoryEntries.Count);
            return [.. info.HistoryEntries];
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            if (entry is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYENTRYNEEDED"));
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Adding {0} to {1}", entry, historyName);
            histories[idx].HistoryEntries.Add(entry);
        }

        /// <summary>
        /// Inserts an entry somewhere in the history entry list
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="entry">Entry to insert</param>
        /// <param name="entryIdx">Index of the history entry to insert this entry to</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Insert(string historyName, string entry, int entryIdx)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            if (entry is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYENTRYNEEDED"));
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Adding {0} to {1}", entry, historyName);
            histories[idx].HistoryEntries.Insert(entryIdx, entry);
        }

        /// <summary>
        /// Removes an entry somewhere in the history entry list
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <param name="entryIdx">Index of the history entry to remove</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Remove(string historyName, int entryIdx)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Removing entry index [{0}] from {1}", entryIdx, historyName);
            histories[idx].HistoryEntries.RemoveAt(entryIdx);
        }

        /// <summary>
        /// Clears the history entries
        /// </summary>
        /// <param name="historyName">History name</param>
        /// <exception cref="TerminauxException"></exception>
        public static void Clear(string historyName)
        {
            if (!IsHistoryRegistered(historyName))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            int idx = GetHistoryIndexFrom(historyName);
            ConsoleLogger.Debug("Clearing {0} entries from {1}", histories[idx].HistoryEntries.Count, historyName);
            histories[idx].HistoryEntries.Clear();
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
            if (entries is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYENTRYLISTNEEDED"));
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTFOUND_NAMED"), historyName);
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYINFONEEDED"));
            if (!histories.Contains(info))
                throw new TerminauxException(LanguageTools.GetLocalized("T_READER_HISTORY_TOOLS_EXCEPTION_HISTORYNOTREGISTERED"));
            string historyInfoString = JsonConvert.SerializeObject(info, Formatting.Indented);
            return historyInfoString;
        }

        internal static void SaveHistories()
        {
            foreach (var historyInfo in histories)
                File.WriteAllText($"{ConsoleFilesystem.GetSubPath("Histories")}/{historyInfo.HistoryName}.json", JsonConvert.SerializeObject(historyInfo, Formatting.Indented));
        }

        internal static void LoadHistories()
        {
            var histories = Directory.GetFiles(ConsoleFilesystem.GetSubPath("Histories"), "*.json");
            foreach (string historyFile in histories)
            {
                string historyJson = File.ReadAllText(historyFile);
                var historyInstance = JsonConvert.DeserializeObject<HistoryInfo>(historyJson);
                if (historyInstance is null)
                    continue;
                if (!IsHistoryRegistered(historyInstance.HistoryName))
                    LoadFromInstance(historyInstance);
                else
                    Switch(historyInstance.HistoryName, [.. historyInstance.HistoryEntries]);
            }
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
