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
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// List entry writer with color support
    /// </summary>
    public static class ListEntryWriterColor
    {
        /// <summary>
        /// Outputs a list entry and value into the terminal prompt plainly.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        public static void WriteListEntryPlain(string entry, string value, int indent = 0)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list entry
                    string buffered = RenderListEntry(entry, value, indent);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        public static void WriteListEntry(string entry, string value, int indent = 0) =>
            WriteListEntry(entry, value, ConsoleColors.Yellow, ConsoleColors.Silver, indent);

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, Color ListKeyColor, Color ListValueColor, int indent = 0)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the list entry
                    string buffered = RenderListEntry(entry, value, ListKeyColor, ListValueColor, indent);
                    TextWriterRaw.WritePlain(buffered);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders a list entry and value.
        /// </summary>
        /// <param name="entry">A list entry that will be listed.</param>
        /// <param name="value">A list value that will be listed.</param>
        /// <param name="indent">Indentation level</param>
        /// <returns>A list entry without the new line at the end</returns>
        public static string RenderListEntry(string entry, string value, int indent = 0) =>
            ListEntry.RenderListEntry(entry, value, ConsoleColors.Yellow, ConsoleColors.Silver, indent, false);

        /// <summary>
        /// Renders a list entry and value.
        /// </summary>
        /// <param name="entry">A list entry that will be listed.</param>
        /// <param name="value">A list value that will be listed.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list entry without the new line at the end</returns>
        public static string RenderListEntry(string entry, string value, Color ListKeyColor, Color ListValueColor, int indent = 0) =>
            ListEntry.RenderListEntry(entry, value, ListKeyColor, ListValueColor, indent, true);

        static ListEntryWriterColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
