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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder;
using Textify.General;

namespace Terminaux.Base.TermInfo.Tabsets
{
    /// <summary>
    /// Tabset parser
    /// </summary>
    public static class TabsetParser
    {
        private static readonly Dictionary<string, Tabset> tabsets = new()
        {
            { "std", ParseTabset(typeof(TermInfoDesc).Assembly.GetManifestResourceStream("Terminaux.Resources.TermTabsets.std")) },
            { "stdcrt", ParseTabset(typeof(TermInfoDesc).Assembly.GetManifestResourceStream("Terminaux.Resources.TermTabsets.stdcrt")) },
            { "vt100", ParseTabset(typeof(TermInfoDesc).Assembly.GetManifestResourceStream("Terminaux.Resources.TermTabsets.vt100")) },
            { "vt300", ParseTabset(typeof(TermInfoDesc).Assembly.GetManifestResourceStream("Terminaux.Resources.TermTabsets.vt300")) },
        };
        private static readonly Dictionary<string, Tabset> customTabsets = [];

        /// <summary>
        /// Tabset names
        /// </summary>
        public static string[] TabsetNames =>
            [.. tabsets.Keys, .. customTabsets.Keys];

        /// <summary>
        /// Gets the tabset instance
        /// </summary>
        /// <param name="name">Tabset name</param>
        /// <returns>Tabset instance</returns>
        /// <exception cref="TerminauxException"></exception>
        public static Tabset GetTabset(string name)
        {
            if (!tabsets.TryGetValue(name, out var tabset))
                if (!customTabsets.TryGetValue(name, out tabset))
                    throw new TerminauxException("Tabset is not found");
            return tabset;
        }

        /// <summary>
        /// Checks to see if the tabset is built-in
        /// </summary>
        /// <param name="name">Name of the tabset</param>
        /// <returns>True if built-in; false otherwise</returns>
        public static bool IsTabsetBuiltin(string name) =>
            tabsets.ContainsKey(name);

        /// <summary>
        /// Checks to see if the tabset is registered
        /// </summary>
        /// <param name="name">Name of the tabset</param>
        /// <returns>True if registered; false otherwise</returns>
        public static bool IsTabsetRegistered(string name) =>
            customTabsets.ContainsKey(name) || IsTabsetBuiltin(name);

        /// <summary>
        /// Registers the tabset
        /// </summary>
        /// <param name="name">Name of the tabset</param>
        /// <param name="tabset">Tabset instance (use <see cref="ParseTabset(string)"/> to get a tabset instance)</param>
        /// <exception cref="TerminauxException"></exception>
        public static void RegisterTabset(string name, Tabset tabset)
        {
            if (IsTabsetRegistered(name))
                throw new TerminauxException("Tabset is already found");
            if (string.IsNullOrEmpty(name))
                throw new TerminauxException("Tabset name may not be empty");
            customTabsets.Add(name, tabset);
        }

        /// <summary>
        /// Unregisters the tabset
        /// </summary>
        /// <param name="name">Name of the tabset</param>
        /// <exception cref="TerminauxException"></exception>
        public static void UnregisterTabset(string name)
        {
            if (!IsTabsetRegistered(name))
                throw new TerminauxException("Tabset is not found");
            if (string.IsNullOrEmpty(name))
                throw new TerminauxException("Tabset name may not be empty");
            if (IsTabsetBuiltin(name))
                throw new TerminauxException("Built-in tabsets may not be removed");
            if (!customTabsets.Remove(name))
                throw new TerminauxException("Tabset removal failed");
        }

        /// <summary>
        /// Parses the tabset from the tabset file
        /// </summary>
        /// <param name="path">Path to a terminal tabset file</param>
        /// <returns>Tabset instance</returns>
        /// <exception cref="TerminauxException"></exception>
        public static Tabset ParseTabset(string path)
        {
            // Check the file
            if (!File.Exists(path))
                throw new TerminauxException("Tabset file is not found");

            // Open the file and parse using the stream
            using var fileStream = File.OpenRead(path);
            return ParseTabset(fileStream);
        }

        /// <summary>
        /// Parses the tabset from the stream
        /// </summary>
        /// <param name="content">Stream containing the content</param>
        /// <returns>Tabset instance</returns>
        /// <exception cref="TerminauxException"></exception>
        public static Tabset ParseTabset(Stream content)
        {
            // Read the contents
            using var reader = new StreamReader(content);
            string tabset = reader.ReadToEnd();
            return ParseTabsetInternal(tabset);
        }

        internal static Tabset ParseTabsetInternal(string content)
        {
            // Trim the content and check it
            content = content.Trim();
            if (string.IsNullOrEmpty(content))
                throw new TerminauxException("Tabset contents may not be empty");

            // Some necessary variables
            List<int> tabStops = [];
            StringBuilder init = new();
            TabsetType type = TabsetType.Numeric;

            // Split the new lines
            var lines = content.SplitNewLines(false);

            // Process each line
            bool initialization = true;
            foreach (var line in lines)
            {
                // Trim the line
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Check for initialization sequence
                if (line.Contains(VtSequenceBasicChars.EscapeChar) && line.Contains('[') && !line.Contains(" "))
                {
                    type = TabsetType.Escape;
                    init.Append(line);
                }
                if (initialization)
                {
                    initialization = false;
                    continue;
                }

                // Check for numeric sequence
                if (line.Contains("P2$t"))
                {
                    // Get the prefix index
                    int posPrefix = line.IndexOf("P2$t");

                    // Start from there, but skip the prefix
                    int marker = posPrefix + 4;
                    char character = line[marker];
                    List<char> digits = [];
                    while (character != '\\')
                    {
                        // Check for digit
                        if (char.IsDigit(character))
                        {
                            // Add the digit
                            digits.Add(character);
                        }

                        // Check for separator
                        if (character == '/')
                        {
                            // Add the resultant number to the tabstop list
                            int result = VtSequenceTokenTools.NumberizeArray(digits);
                            digits.Clear();
                            tabStops.Add(result);
                        }

                        // Go to the next character
                        character = line[++marker];
                    }

                    // Add the remaining number
                    int finalTabStop = VtSequenceTokenTools.NumberizeArray(digits);
                    tabStops.Add(finalTabStop);
                    continue;
                }

                // Check for position count
                if (line.Contains('H'))
                {
                    // Get the first H character
                    int posPrefix = line.IndexOf("H");
                    int lastPrefix = line.LastIndexOf("H");

                    // Start from there, but skip the prefix
                    int marker = posPrefix + 1;
                    char character = line[marker];
                    int processed = 0;
                    while (marker <= lastPrefix)
                    {
                        // Check for space
                        if (character == ' ')
                            processed++;

                        // Check for separator
                        if (character == 'H')
                            tabStops.Add(processed);

                        // Go to the next character
                        if (++marker < line.Length)
                            character = line[marker];
                    }
                    continue;
                }

                // Get the tabstop numbers
                var numbers = line.Split([" ", "\t", $"{VtSequenceBasicChars.EscapeChar}"], StringSplitOptions.RemoveEmptyEntries);

                // Now, process them to start with column 1
                int column = 1;
                foreach (var number in numbers)
                {
                    if (int.TryParse(number, out int tabWidth))
                    {
                        tabStops.Add(column);
                        column += tabWidth;
                    }
                }
            }

            // Make a new instance of the tabset
            var tabset = new Tabset([.. tabStops], init.ToString(), type);
            return tabset;
        }
    }
}
