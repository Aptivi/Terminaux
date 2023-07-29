
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Reader.Inputs;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// List writer with color support
    /// </summary>
    public static class ListWriterColor
    {
        #region Dictionary
        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor, bool Wrap) =>
            WriteList(List, new Color(ListKeyColor), new Color(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;
                    int OldTop;

                    // Try to write list to console
                    OldTop = Console.CursorTop;
                    foreach (TKey ListEntry in List.Keys)
                    {
                        var Values = new List<object>();
                        if (List[ListEntry] as IEnumerable is not null & List[ListEntry] as string is null)
                        {
                            foreach (var Value in (IEnumerable)List[ListEntry])
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, ListEntry);
                            TextWriterColor.Write("{0}", true, ListValueColor, List[ListEntry]);
                        }
                        if (Wrap)
                        {
                            LinesMade += Console.CursorTop - OldTop;
                            OldTop = Console.CursorTop;
                            if (LinesMade == Console.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }
        #endregion

        #region Enumerables
        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor, bool Wrap) =>
            WriteList(List, new Color(ListKeyColor), new Color(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;
                    int OldTop;
                    int EntryNumber = 1;

                    // Try to write list to console
                    OldTop = Console.CursorTop;
                    foreach (T ListEntry in List)
                    {
                        var Values = new List<object>();
                        if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                        {
                            foreach (var Value in (IEnumerable)ListEntry)
                                Values.Add(Value);
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, ListValueColor, string.Join(", ", Values));
                        }
                        else
                        {
                            TextWriterColor.Write("- {0}: ", false, ListKeyColor, EntryNumber);
                            TextWriterColor.Write("{0}", true, ListValueColor, ListEntry);
                        }
                        EntryNumber += 1;
                        if (Wrap)
                        {
                            LinesMade += Console.CursorTop - OldTop;
                            OldTop = Console.CursorTop;
                            if (LinesMade == Console.WindowHeight - 1)
                            {
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (Console.KeyAvailable)
                        {
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }
        #endregion
    }
}
