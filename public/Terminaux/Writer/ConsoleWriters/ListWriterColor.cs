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
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// List writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Also, this writer doesn't support indeterminate progress bars. Please use its cyclic writer equivalent.")]
    public static class ListWriterColor
    {
        #region Dictionary (generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, keyStringifier, valueStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Silver, keyStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor, keyStringifier, valueStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), keyStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ListKeyColor, ListValueColor, true, keyStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        /// <param name="keyStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        internal static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool useColor, Func<TKey, string>? keyStringifier = null, Func<TValue, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            var objectKeyStringifier = keyStringifier is not null ? new Func<object, string>((obj) => keyStringifier((TKey)obj)) : null;
            var objectValueStringifier = valueStringifier is not null ? new Func<object, string>((obj) => valueStringifier((TValue)obj)) : null;
            return RenderList(List, ListKeyColor, ListValueColor, useColor, objectKeyStringifier, objectValueStringifier, recursiveStringifier);
        }
        #endregion

        #region Dictionary (non-generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteListPlain(IDictionary List, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, keyObjectStringifier, valueStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList(IDictionary List, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Silver, keyObjectStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList(IDictionary List, Color ListKeyColor, Color ListValueColor, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor, keyObjectStringifier, valueStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList(IDictionary List, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), keyObjectStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList(IDictionary List, Color ListKeyColor, Color ListValueColor, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ListKeyColor, ListValueColor, true, keyObjectStringifier, valueStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        /// <param name="keyObjectStringifier">A function that stringifies a key.</param>
        /// <param name="valueStringifier">A function that stringifies a value.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        internal static string RenderList(IDictionary List, Color ListKeyColor, Color ListValueColor, bool useColor, Func<object, string>? keyObjectStringifier = null, Func<object, string>? valueStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            var listBuilder = new StringBuilder();
            foreach (var ListEntry in List.Keys)
            {
                var value = List[ListEntry];
                if (value is IEnumerable enums && value is not string)
                {
                    var strings = new List<object>();
                    foreach (var Value in enums)
                        strings.Add(recursiveStringifier is not null ? recursiveStringifier(Value) : Value);
                    string valuesString = string.Join(", ", strings);
                    listBuilder.AppendLine(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ListKeyColor) : "")}" +
                        $"- {(keyObjectStringifier is not null ? keyObjectStringifier(ListEntry) : ListEntry)}: " +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ListValueColor) : "")}" +
                        $"{valuesString}"
                    );
                }
                else
                    listBuilder.AppendLine(
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ListKeyColor) : "")}" +
                        $"- {(keyObjectStringifier is not null ? keyObjectStringifier(ListEntry) : ListEntry)}: " +
                        $"{(useColor ? ColorTools.RenderSetConsoleColor(ListValueColor) : "")}" +
                        $"{(valueStringifier is not null ? valueStringifier(value) : value)}"
                    );
            }
            if (useColor)
                listBuilder.Append(ColorTools.RenderRevertForeground());
            return listBuilder.ToString();
        }
        #endregion

        #region Enumerables (generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, stringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList<T>(IEnumerable<T> List, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Silver, stringifier, recursiveStringifier);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor, stringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList<T>(IEnumerable<T> List, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), stringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ListKeyColor, ListValueColor, true, stringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        /// <param name="stringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        internal static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool useColor, Func<T, string>? stringifier = null, Func<object, string>? recursiveStringifier = null) =>
            Listing.RenderList(List, ListKeyColor, ListValueColor, useColor, new((obj) => stringifier?.Invoke((T)obj) ?? obj.ToString()), recursiveStringifier);
        #endregion

        #region Enumerables (non-generic)
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteListPlain(IEnumerable List, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, objectStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList(IEnumerable List, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            WriteList(List, ConsoleColors.Yellow, ConsoleColors.Silver, objectStringifier, recursiveStringifier);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static void WriteList(IEnumerable List, Color ListKeyColor, Color ListValueColor, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor, objectStringifier, recursiveStringifier);
                    TextWriterRaw.WriteRaw(buffered);
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
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList(IEnumerable List, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ColorTools.GetGray(), ColorTools.GetGray(), objectStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        public static string RenderList(IEnumerable List, Color ListKeyColor, Color ListValueColor, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List, ListKeyColor, ListValueColor, true, objectStringifier, recursiveStringifier);

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        /// <param name="objectStringifier">A function that stringifies an entry.</param>
        /// <param name="recursiveStringifier">A function that stringifies a recursed entry.</param>
        internal static string RenderList(IEnumerable List, Color ListKeyColor, Color ListValueColor, bool useColor, Func<object, string>? objectStringifier = null, Func<object, string>? recursiveStringifier = null) =>
            RenderList(List.OfType<object>(), ListKeyColor, ListValueColor, useColor, objectStringifier, recursiveStringifier);
        #endregion

        static ListWriterColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
