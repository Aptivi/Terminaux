﻿
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
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors;
using Textify.General;
using Textify.Sequences.Builder;
using Textify.Sequences.Tools;

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
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    int width = ConsoleWrapper.WindowWidth - RightMargin;
                    var Paragraphs = msg.SplitNewLines();
                    if (RightMargin > 0)
                        Paragraphs = TextTools.GetWrappedSentences(msg, width);
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
                                buffered.Append($"{VtSequenceBasicChars.EscapeChar}[1B");
                                buffered.Append($"{VtSequenceBasicChars.EscapeChar}[{Left + 1}G");
                                pos = OldLeft;
                            }

                            // Write a character individually
                            if (MessageParagraph[i] != '\n')
                            {
                                string bufferedChar = ConsoleExtensions.BufferChar(MessageParagraph, sequences, ref i, ref vtSeqIdx, out bool isVtSequence);
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
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            buffered.Append($"{VtSequenceBasicChars.EscapeChar}[1B");
                            buffered.Append($"{VtSequenceBasicChars.EscapeChar}[{Left + 1}G");
                            pos = OldLeft;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        public static void WriteWhereSlowly(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, ConsoleColors color, params object[] vars) =>
            WriteWhereSlowlyColor(msg, Line, Left, Top, MsEachLetter, false, 0, color, vars);

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
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ConsoleColors color, params object[] vars) =>
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
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ConsoleColors color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(new Color(color));
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
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
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
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
        public static void WriteWhereSlowlyColorBack(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(new Color(ForegroundColor));
                    ColorTools.SetConsoleColor(new Color(BackgroundColor));

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
        public static void WriteWhereSlowlyColor(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, int RightMargin, Color color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(color);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
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
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    ColorTools.SetConsoleColor(ForegroundColor);
                    ColorTools.SetConsoleColor(BackgroundColor, true);

                    // Write text in another place slowly
                    WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, Return, RightMargin, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

    }
}
