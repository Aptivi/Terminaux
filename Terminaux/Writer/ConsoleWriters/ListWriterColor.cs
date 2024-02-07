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
using System.Collections;
using System.Collections.Generic;
using Terminaux.Colors;
using System.Text;
using System.Diagnostics;
using Terminaux.Colors.Data;
using System.Linq;
using Terminaux.Writer.DynamicWriters;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// List writer with color support
    /// </summary>
    public static class ListWriterColor
    {
        #region Dictionary (generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List) =>
            WriteListPlain(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List) =>
            WriteList(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, bool Wrap) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Gray, Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, false);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor) =>
            RenderList(List, ListKeyColor, ListValueColor, true);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool useColor) =>
            RenderList(List as IDictionary, ListKeyColor, ListValueColor, useColor);
        #endregion

        #region Dictionary (non-generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain(IDictionary List) =>
            WriteListPlain(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain(IDictionary List, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList(IDictionary List) =>
            WriteList(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList(IDictionary List, bool Wrap) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Gray, Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList(IDictionary List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, false);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList(IDictionary List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList(IDictionary List) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList(IDictionary List, Color ListKeyColor, Color ListValueColor) =>
            RenderList(List, ListKeyColor, ListValueColor, true);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderList(IDictionary List, Color ListKeyColor, Color ListValueColor, bool useColor)
        {
            var listBuilder = new StringBuilder();
            foreach (var ListEntry in List.Keys)
            {
                var Values = new List<object>();
                var value = List[ListEntry];
                if (value as IEnumerable is not null & value as string is null)
                {
                    foreach (var Value in (IEnumerable)value)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine(
                        $"{(useColor ? ListKeyColor.VTSequenceForeground : "")}" +
                        $"- {ListEntry}: " +
                        $"{(useColor ? ListValueColor.VTSequenceForeground : "")}" +
                        $"{valuesString}"
                    );
                }
                else
                    listBuilder.AppendLine(
                        $"{(useColor ? ListKeyColor.VTSequenceForeground : "")}" +
                        $"- {ListEntry}: " +
                        $"{(useColor ? ListValueColor.VTSequenceForeground : "")}" +
                        $"{value}"
                    );
            }
            if (useColor)
                listBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor));
            return listBuilder.ToString();
        }
        #endregion

        #region Enumerables (generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List) =>
            WriteListPlain(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<T>(IEnumerable<T> List) =>
            WriteList(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, bool Wrap) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Gray, Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, false);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList<T>(IEnumerable<T> List) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor) =>
            RenderList(List, ListKeyColor, ListValueColor, true);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool useColor)
        {
            var listBuilder = new StringBuilder();
            int EntryNumber = 1;
            foreach (T ListEntry in List)
            {
                var Values = new List<object>();
                if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                {
                    foreach (var Value in (IEnumerable)ListEntry)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine(
                        $"{(useColor ? ListKeyColor.VTSequenceForeground : "")}" +
                        $"- {EntryNumber}: " +
                        $"{(useColor ? ListValueColor.VTSequenceForeground : "")}" +
                        $"{valuesString}"
                    );
                }
                else
                    listBuilder.AppendLine(
                        $"{(useColor ? ListKeyColor.VTSequenceForeground : "")}" +
                        $"- {EntryNumber}: " +
                        $"{(useColor ? ListValueColor.VTSequenceForeground : "")}" +
                        $"{ListEntry}"
                    );
                EntryNumber += 1;
            }
            if (useColor)
                listBuilder.Append(ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor));
            return listBuilder.ToString();
        }
        #endregion

        #region Enumerables (non-generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain(IEnumerable List) =>
            WriteListPlain(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain(IEnumerable List, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList(IEnumerable List) =>
            WriteList(List, false);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList(IEnumerable List, bool Wrap) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Gray, Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList(IEnumerable List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, false);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList(IEnumerable List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    if (Wrap)
                        TextWriterWrappedColor.WriteWrappedPlain(buffered, false);
                    else
                        TextWriterRaw.WritePlain(buffered, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList(IEnumerable List) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), false);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList(IEnumerable List, Color ListKeyColor, Color ListValueColor) =>
            RenderList(List, ListKeyColor, ListValueColor, true);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderList(IEnumerable List, Color ListKeyColor, Color ListValueColor, bool useColor) =>
            RenderList(List.OfType<object>(), ListKeyColor, ListValueColor, useColor);
        #endregion
    }
}
