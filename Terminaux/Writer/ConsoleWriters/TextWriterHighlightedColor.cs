﻿//
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
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Reader;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support
    /// </summary>
    public static class TextWriterHighlightedColor
    {

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, params object[] vars) =>
            Write(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, params object[] vars) =>
            WriteColor(Text, Line, ColorTools.currentForegroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, Color color, params object[] vars) =>
            WriteColor(Text, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, Color color, params object[] vars) =>
            WriteColorBack(Text, Line, color, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteColorBack(Text, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteForReaderColorBack(Text, null, Line, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings settings, params object[] vars) =>
            WriteForReader(Text, settings, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings settings, bool Line, params object[] vars) =>
            WriteForReaderColor(Text, settings, Line, ColorTools.currentForegroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(string Text, TermReaderSettings settings, Color color, params object[] vars) =>
            WriteForReaderColor(Text, settings, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(string Text, TermReaderSettings settings, bool Line, Color color, params object[] vars) =>
            WriteForReaderColorBack(Text, settings, Line, color, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColorBack(string Text, TermReaderSettings settings, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteForReaderColorBack(Text, settings, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColorBack(string Text, TermReaderSettings settings, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColorDry(ForegroundColor, true);
                    ColorTools.SetConsoleColorDry(BackgroundColor);

                    // Write the text to console
                    TextWriterColor.WritePlain(Text, settings, false, vars);
                    ColorTools.SetConsoleColorDry(ForegroundColor);
                    ColorTools.SetConsoleColorDry(BackgroundColor, true);
                    TextWriterColor.WritePlain("", settings, Line);

                    // Reset the colors
                    ConsoleExtensions.ResetColors();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
                }
            }
        }

    }
}
