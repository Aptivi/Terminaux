﻿//
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
using System.Diagnostics;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Console text writer with rainbow color as background color
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class RainbowBackTextWriterColor
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
            WriteColor(Text, Line, ColorTools.currentForegroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, Color foregroundColor, params object[] vars) =>
            WriteColor(Text, true, foregroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, Color foregroundColor, params object[] vars)
        {
            try
            {
                TextWriterRaw.WriteRaw(
                    RenderColor(Text, Line, foregroundColor, vars)
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
            RenderColor(Text, Line, ColorTools.currentForegroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, Color foregroundColor, params object[] vars) =>
            RenderColor(Text, true, foregroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderColor(string Text, bool Line, Color foregroundColor, params object[] vars) =>
            new AlignedText(Text, vars)
            {
                Rainbow = true,
                RainbowBg = true,
                Text = Line ? Text + "\n" : Text,
                ForegroundColor = foregroundColor,
            }.Render();

        static RainbowBackTextWriterColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
