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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Reader;
using Terminaux.Themes.Colors;
using Textify.General;

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
            Write(Text, true, ThemeColorType.NeutralText, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, params object[] vars) =>
            Write(Text, Line, ThemeColorType.NeutralText, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, ThemeColorType colorType = ThemeColorType.NeutralText, params object[] vars) =>
            Write(Text, true, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ThemeColorType colorType = ThemeColorType.NeutralText, params object[] vars) =>
            Write(Text, Line, colorType, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, ThemeColorType colorTypeForeground = ThemeColorType.NeutralText, ThemeColorType colorTypeBackground = ThemeColorType.Background, params object[] vars) =>
            Write(Text, true, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ThemeColorType colorTypeForeground = ThemeColorType.NeutralText, ThemeColorType colorTypeBackground = ThemeColorType.Background, params object[] vars) =>
            WriteForReader(false, Text, null, Line, colorTypeForeground, colorTypeBackground, vars);

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
            WriteColorBack(Text, Line, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

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
        public static void WriteForReader(string Text, TermReaderSettings? settings, params object[] vars) =>
            WriteForReader(Text, settings, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings? settings, bool Line, params object[] vars) =>
            WriteForReaderColor(Text, settings, Line, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings? settings, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteForReader(Text, settings, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings? settings, bool Line, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteForReader(Text, settings, Line, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings? settings, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteForReader(Text, settings, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(string Text, TermReaderSettings? settings, bool Line, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteForReader(false, Text, settings, Line, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(string Text, TermReaderSettings? settings, Color color, params object[] vars) =>
            WriteForReaderColor(Text, settings, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(string Text, TermReaderSettings? settings, bool Line, Color color, params object[] vars) =>
            WriteForReaderColorBack(Text, settings, Line, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColorBack(string Text, TermReaderSettings? settings, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
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
        public static void WriteForReaderColorBack(string Text, TermReaderSettings? settings, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteForReaderColorBack(false, Text, settings, Line, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, params object[] vars) =>
            Write(legacy, Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, bool Line, params object[] vars) =>
            WriteColor(legacy, Text, Line, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            Write(legacy, Text, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, bool Line, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            Write(legacy, Text, Line, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            Write(legacy, Text, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(bool legacy, string Text, bool Line, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteForReader(legacy, Text, null, Line, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(bool legacy, string Text, Color color, params object[] vars) =>
            WriteColor(legacy, Text, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(bool legacy, string Text, bool Line, Color color, params object[] vars) =>
            WriteColorBack(legacy, Text, Line, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(bool legacy, string Text, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteColorBack(legacy, Text, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(bool legacy, string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteForReaderColorBack(legacy, Text, null, Line, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, params object[] vars) =>
            WriteForReader(legacy, Text, settings, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, bool Line, params object[] vars) =>
            WriteForReaderColor(legacy, Text, settings, Line, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteForReader(legacy, Text, settings, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, bool Line, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteForReader(legacy, Text, settings, Line, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteForReader(legacy, Text, settings, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReader(bool legacy, string Text, TermReaderSettings? settings, bool Line, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars)
        {
            // Get the colors
            var foreground = ThemeColorsTools.GetColor(ForegroundColor);
            var background = ThemeColorsTools.GetColor(BackgroundColor);

            // Write the text to console
            WriteForReaderColorBack(legacy, Text, settings, Line, foreground, background, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(bool legacy, string Text, TermReaderSettings? settings, Color color, params object[] vars) =>
            WriteForReaderColor(legacy, Text, settings, true, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColor(bool legacy, string Text, TermReaderSettings? settings, bool Line, Color color, params object[] vars) =>
            WriteForReaderColorBack(legacy, Text, settings, Line, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColorBack(bool legacy, string Text, TermReaderSettings? settings, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteForReaderColorBack(legacy, Text, settings, true, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteForReaderColorBack(bool legacy, string Text, TermReaderSettings? settings, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write the text to console
                    string sequence = RenderColorBack(legacy, Text, ForegroundColor, BackgroundColor, vars);
                    TextWriterRaw.WritePlain(sequence, settings, Line, vars);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(string Text, params object[] vars) =>
            RenderColor(Text, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            Render(Text, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColorBack(string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            Render(false, Text, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, Color color, params object[] vars) =>
            RenderColorBack(Text, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            RenderColorBack(false, Text, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(bool legacy, string Text, params object[] vars) =>
            RenderColor(legacy, Text, ThemeColorsTools.GetColor(ThemeColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(bool legacy, string Text, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            Render(legacy, Text, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(bool legacy, string Text, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars)
        {
            // Get the colors
            var foreground = ThemeColorsTools.GetColor(ForegroundColor);
            var background = ThemeColorsTools.GetColor(BackgroundColor);

            return RenderColorBack(legacy, Text, foreground, background, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(bool legacy, string Text, Color color, params object[] vars) =>
            RenderColorBack(legacy, Text, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="legacy">Use <see cref="ConsoleColoring.RenderSetConsoleColor(Color, bool, bool, bool)"/> instead of text formatting</param>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColorBack(bool legacy, string Text, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    var buffered = new StringBuilder();
                    string final = TextTools.FormatString(Text, vars);
                    if (legacy)
                    {
                        buffered.Append(
                            ConsoleColoring.RenderSetConsoleColor(ForegroundColor, true) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor) +
                            final +
                            ConsoleColoring.RenderRevertForeground() +
                            ConsoleColoring.RenderRevertBackground()
                        );
                    }
                    else
                    {
                        buffered.Append(
                            ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Reverse) +
                            ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                            ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) +
                            final +
                            ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Default) +
                            ConsoleColoring.RenderRevertForeground() +
                            ConsoleColoring.RenderRevertBackground()
                        );
                    }
                    return buffered.ToString();
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
                return "";
            }
        }
    }
}
