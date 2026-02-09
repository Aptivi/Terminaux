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
using Colorimetry;
using System.Text;
using Terminaux.Base;
using Textify.General;
using Terminaux.Base.Extensions;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (slow positional write)
    /// </summary>
    public static class TextWriterWhereSlowColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        msg = msg.FormatString(vars);

                    // Write text in another place slowly
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    int width = ConsoleWrapper.WindowWidth - RightMargin;
                    var Paragraphs = msg.SplitNewLines();
                    if (RightMargin > 0)
                        Paragraphs = ConsoleMisc.GetWrappedSentencesByWords(msg, width);
                    var buffered = new StringBuilder();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0; MessageParagraphIndex <= Paragraphs.Length - 1; MessageParagraphIndex++)
                    {
                        // Get the paragraph
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(MessageParagraph);
                        int vtSeqIdx = 0;

                        // Buffer the characters and then write when done
                        int pos = OldLeft;
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            // Sleep for a few milliseconds
                            Thread.Sleep((int)Math.Round(MsEachLetter));
                            if (MessageParagraph[i] == '\n' || RightMargin > 0 && pos > width)
                            {
                                buffered.Append($"{CharManager.GetEsc()}[1B");
                                buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                                pos = OldLeft;
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                            {
                                string bufferedChar = ConsolePositioning.BufferChar(MessageParagraph, sequences, ref i, ref vtSeqIdx, out bool isVtSequence);
                                buffered.Append(bufferedChar);
                                if (!isVtSequence)
                                    pos += bufferedChar.Length;
                            }

                            // If we're writing a new line, write it
                            if (Line)
                                ConsoleWrapper.WriteLine(buffered.ToString());
                            else
                                ConsoleWrapper.Write(buffered.ToString());
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (MessageParagraphIndex != Paragraphs.Length - 1)
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            pos = OldLeft;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, false, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, ThemeColorType.NeutralText, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ThemeColorType color = ThemeColorType.NeutralText, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, color, ThemeColorType.Background, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteWhereSlowly(msg, Line, Left, Top, MsEachLetter, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ThemeColorType ForegroundColor = ThemeColorType.NeutralText, ThemeColorType BackgroundColor = ThemeColorType.Background, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, ThemeColorsTools.GetColor(ForegroundColor), ThemeColorsTools.GetColor(BackgroundColor), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, Color color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, Color color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, Return, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, Color color, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, color, ThemeColorsTools.GetColor(ThemeColorType.Background), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereSlowlyColorBack(msg, Line, Left, Top, MsEachLetter, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="MsEachLetter">Time in milliseconds to delay writing</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    ConsoleColoring.SetConsoleColorDry(ForegroundColor);
                    ConsoleColoring.SetConsoleColorDry(BackgroundColor, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);

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
