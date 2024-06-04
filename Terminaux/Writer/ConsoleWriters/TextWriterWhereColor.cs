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
using Terminaux.Colors;
using System.Text;
using Terminaux.Base;
using System.Diagnostics;
using Textify.General;
using Terminaux.Base.Extensions;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (positional write)
    /// </summary>
    public static class TextWriterWhereColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWherePlain(string msg, int Left, int Top, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, false, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars) =>
            WriteWherePlain(msg, Left, Top, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWherePlain(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Render as necessary
                    ConsoleWrapper.Write(RenderWhere(msg, Left, Top, Return, RightMargin, vars));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, params object[] vars) =>
            WriteWhere(msg, Left, Top, false, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, params object[] vars) =>
            WriteWhere(msg, Left, Top, Return, 0, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, RightMargin, vars);
                }
                catch (Exception ex)
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
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColor(string msg, int Left, int Top, Color color, params object[] vars) =>
            WriteWhereColor(msg, Left, Top, false, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColor(string msg, int Left, int Top, bool Return, Color color, params object[] vars) =>
            WriteWhereColor(msg, Left, Top, Return, 0, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColor(string msg, int Left, int Top, bool Return, int RightMargin, Color color, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    ConsoleWrapper.Write(RenderWhereColorBack(msg, Left, Top, Return, RightMargin, color, ColorTools.currentBackgroundColor, vars));
                }
                catch (Exception ex)
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
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColorBack(string msg, int Left, int Top, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereColorBack(msg, Left, Top, false, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColorBack(string msg, int Left, int Top, bool Return, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteWhereColorBack(msg, Left, Top, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhereColorBack(string msg, int Left, int Top, bool Return, int RightMargin, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    ConsoleWrapper.Write(RenderWhereColorBack(msg, Left, Top, Return, RightMargin, ForegroundColor, BackgroundColor, vars));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhere(string msg, int Left, int Top, params object[] vars) =>
            RenderWhere(msg, Left, Top, false, 0, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhere(string msg, int Left, int Top, bool Return, params object[] vars) =>
            RenderWhere(msg, Left, Top, Return, 0, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhere(string msg, int Left, int Top, bool Return, int RightMargin, params object[] vars) =>
            RenderWhere(msg, Left, Top, Return, RightMargin, ColorTools.CurrentForegroundColor, ColorTools.CurrentBackgroundColor, false, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColor(string msg, int Left, int Top, Color ForegroundColor, params object[] vars) =>
            RenderWhereColorBack(msg, Left, Top, false, 0, ForegroundColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColor(string msg, int Left, int Top, bool Return, Color ForegroundColor, params object[] vars) =>
            RenderWhereColorBack(msg, Left, Top, Return, 0, ForegroundColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColor(string msg, int Left, int Top, bool Return, int RightMargin, Color ForegroundColor, params object[] vars) =>
            RenderWhereColorBack(msg, Left, Top, Return, RightMargin, ForegroundColor, ColorTools.CurrentBackgroundColor, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColorBack(string msg, int Left, int Top, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            RenderWhereColorBack(msg, Left, Top, false, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColorBack(string msg, int Left, int Top, bool Return, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            RenderWhereColorBack(msg, Left, Top, Return, 0, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string RenderWhereColorBack(string msg, int Left, int Top, bool Return, int RightMargin, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            RenderWhere(msg, Left, Top, Return, RightMargin, ForegroundColor, BackgroundColor, true, vars);

        /// <summary>
        /// Renders the text with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColors">Whether to use colors or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        internal static string RenderWhere(string msg, int Left, int Top, bool Return, int RightMargin, Color ForegroundColor, Color BackgroundColor, bool useColors, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Format the message as necessary
                    if (vars.Length > 0)
                        msg = TextTools.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    int width = ConsoleWrapper.WindowWidth - RightMargin;
                    var Paragraphs = msg.SplitNewLines();
                    if (RightMargin > 0)
                        Paragraphs = ConsoleMisc.GetWrappedSentencesByWords(msg, width);
                    var buffered = new StringBuilder();

                    // Set the colors and the positions as appropriate
                    if (useColors)
                    {
                        buffered.Append(
                            ColorTools.RenderSetConsoleColor(ForegroundColor) +
                            ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                        );
                    }
                    buffered.Append(
                        CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)
                    );
                    for (int MessageParagraphIndex = 0; MessageParagraphIndex <= Paragraphs.Length - 1; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(MessageParagraph);
                        int vtSeqIdx = 0;

                        // Now, parse every character
                        int pos = OldLeft;
                        for (int i = 0; i < MessageParagraph.Length; i++)
                        {
                            // If there is a new line, or if we're about to hit the maximum width, we need to go down
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
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (MessageParagraphIndex != Paragraphs.Length - 1)
                        {
                            buffered.Append($"{CharManager.GetEsc()}[1B");
                            buffered.Append($"{CharManager.GetEsc()}[{Left + 1}G");
                            pos = OldLeft;
                        }
                    }

                    // Return if we're told to
                    if (Return)
                        buffered.Append(CsiSequences.GenerateCsiCursorPosition(OldLeft + 1, OldTop + 1));

                    // Write the resulting buffer
                    if (useColors)
                    {
                        buffered.Append(
                            ColorTools.RenderRevertForeground() +
                            ColorTools.RenderRevertBackground()
                        );
                    }
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

        static TextWriterWhereColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
