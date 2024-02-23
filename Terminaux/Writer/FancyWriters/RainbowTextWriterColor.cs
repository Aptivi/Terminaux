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
using System.Diagnostics;
using System.Text;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Sequences;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with rainbow color as foreground color
    /// </summary>
    public static class RainbowTextWriterColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt with rainbow bands as foregound colors.
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
            WriteColor(Text, Line, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, Color backgroundColor, params object[] vars) =>
            WriteColor(Text, true, backgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, Color backgroundColor, params object[] vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderColor(Text, Line, backgroundColor, vars)
                );
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with rainbow bands as foregound colors.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(string Text, params object[] vars) =>
            Render(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string Render(string Text, bool Line, params object[] vars) =>
            RenderColor(Text, Line, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, Color backgroundColor, params object[] vars) =>
            RenderColor(Text, true, backgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, bool Line, Color backgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    var buffered = new StringBuilder();
                    string formatted = TextTools.FormatString(Text, vars);
                    string filtered = VtSequenceTools.FilterVTSequences(formatted);
                    int length = filtered.Length;
                    for (int i = 0; i < length; i++)
                    {
                        char filteredChar = filtered[i];
                        double width = (double)i / length;
                        int hue = (int)(360 * width);
                        buffered.Append(
                            ColorTools.RenderSetConsoleColor(new Color($"hsl:{hue};100;50"), false, true, true) +
                            ColorTools.RenderSetConsoleColor(backgroundColor, true) +
                            $"{filteredChar}"
                        );
                    }
                    buffered.Append(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                    if (Line)
                        buffered.AppendLine();
                    return buffered.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
                return "";
            }
        }

        static RainbowTextWriterColor()
        {
            if (GeneralColorTools.CheckConsoleOnCall)
                ConsoleChecker.CheckConsole();
        }
    }
}
