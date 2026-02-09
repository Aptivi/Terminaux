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
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (slow write)
    /// </summary>
    public static class TextWriterSlowColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        msg = msg.FormatString(vars);

                    // Grab each VT sequence from the message and fetch their indexes
                    var sequences = VtSequenceTools.MatchVTSequences(msg);
                    int vtSeqIdx = 0;

                    // Write text slowly
                    for (int i = 0; i < msg.Length; i++)
                    {
                        // Sleep for a while
                        Thread.Sleep((int)Math.Round(MsEachLetter));

                        // Write a character individually
                        ConsoleWrapper.Write(ConsolePositioning.BufferChar(msg, sequences, ref i, ref vtSeqIdx, out _));
                    }

                    // If we're writing a new line, write it
                    if (Line)
                        ConsoleWrapper.WriteLine();
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteSlowly(msg, Line, MsEachLetter, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowly(string msg, bool Line, double MsEachLetter, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteSlowlyColorBack(msg, Line, MsEachLetter, ThemeColorsTools.GetColor(ForegroundColor), ThemeColorsTools.GetColor(BackgroundColor), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowlyColor(string msg, bool Line, double MsEachLetter, Color color, params object[] vars) =>
            WriteSlowlyColorBack(msg, Line, MsEachLetter, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt slowly with color support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteSlowlyColorBack(string msg, bool Line, double MsEachLetter, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    ConsoleColoring.SetConsoleColorDry(ForegroundColor);
                    ConsoleColoring.SetConsoleColorDry(BackgroundColor, true);

                    // Write text slowly
                    WriteSlowlyPlain(msg, Line, MsEachLetter, vars);

                    // Reset the colors
                    ConsoleColoring.ResetColors();
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }
    }
}
