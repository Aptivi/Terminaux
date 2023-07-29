
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
using System.Diagnostics;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support
    /// </summary>
    public static class TextWriterColor
    {

        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the new line into the terminal prompt, and sets colors as needed.
        /// </summary>
        public static void Write()
        {
            lock (WriteLock)
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    var pos = ConsoleExtensions.GetFilteredPositions(Line ? Text + "\n" : Text, vars);
                    FilteredLeft = pos.Item1;
                    FilteredTop = pos.Item2;

                    // Actually write
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            Console.WriteLine(Text, vars);
                        }
                        else
                        {
                            Console.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        Console.Write(Text, vars);
                    }
                    else
                    {
                        Console.Write(Text);
                    }

                    // Return to the processed position
                    Console.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, params object[] vars) => Write(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, params object[] vars) => Write(Text, Line, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, params object[] vars) => Write(Text, Line, Highlight, new Color(ConsoleColors.Gray), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColors color, params object[] vars) => Write(Text, Line, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColors color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(color), Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(new Color(color));
                        ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) => Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(ForegroundColor), Highlight);
                    ColorTools.SetConsoleColor(new Color(BackgroundColor), !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(new Color(ForegroundColor));
                        ColorTools.SetConsoleColor(new Color(BackgroundColor), true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color color, params object[] vars) => Write(Text, Line, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(color, Highlight, Highlight);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), !Highlight, Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(color);
                        ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) => Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(ForegroundColor, Highlight, Highlight);
                    ColorTools.SetConsoleColor(BackgroundColor, !Highlight, Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(ForegroundColor);
                        ColorTools.SetConsoleColor(BackgroundColor, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

    }
}
