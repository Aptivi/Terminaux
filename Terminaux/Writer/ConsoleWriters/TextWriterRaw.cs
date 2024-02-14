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

using System.Diagnostics;
using System;
using Terminaux.Base.Extensions;
using Terminaux.Base;
using Terminaux.Reader;
using SpecProbe.Platform;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Raw text writers for console
    /// </summary>
    public static class TextWriterRaw
    {
        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the new line into the terminal prompt.
        /// </summary>
        public static void Write()
        {
            lock (WriteLock)
            {
                try
                {
                    ConsoleWrapper.WriteLine();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt plainly with a newline terminator.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, params object[] vars) =>
            WritePlain(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteRaw(string Text, params object[] vars) =>
            WritePlain(Text, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, bool Line, params object[] vars) =>
            WritePlain(Text, null, Line, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly with a newline terminator. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, TermReaderSettings? settings, params object[] vars) =>
            WritePlain(Text, settings, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteRaw(string Text, TermReaderSettings? settings, params object[] vars) =>
            WritePlain(Text, settings, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, TermReaderSettings? settings, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    if (PlatformHelper.IsRunningFromMono())
                    {
                        var pos = ConsolePositioning.GetFilteredPositions(Text, Line, vars);
                        FilteredLeft = pos.Item1;
                        FilteredTop = pos.Item2;
                    }

                    // Actually write
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            if (settings is null)
                                ConsoleWrapper.WriteLine(Text, vars);
                            else
                                ConsoleWrapper.WriteLine(Text, settings, vars);
                        }
                        else
                        {
                            if (settings is null)
                                ConsoleWrapper.WriteLine(Text);
                            else
                                ConsoleWrapper.WriteLine(Text, settings);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        if (settings is null)
                            ConsoleWrapper.Write(Text, vars);
                        else
                            ConsoleWrapper.Write(Text, settings, vars);
                    }
                    else
                    {
                        if (settings is null)
                            ConsoleWrapper.Write(Text);
                        else
                            ConsoleWrapper.Write(Text, settings);
                    }

                    // Return to the processed position
                    if (PlatformHelper.IsRunningFromMono())
                        ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }
    }
}
