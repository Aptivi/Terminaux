
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
using Terminaux.Reader.Inputs;
using Textify.General;
using Textify.Sequences.Tools;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support (wrapped)
    /// </summary>
    public static class TextWriterWrappedColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt, wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                var LinesMade = 0;
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        Text = ConsoleExtensions.FormatString(Text, vars);
                    Text = Text.Replace(Convert.ToChar(13), default);

                    // First, split the text to wrap
                    string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);

                    // Iterate through sentences
                    var buffered = new StringBuilder();
                    bool exiting = false;
                    foreach (string sentence in sentences)
                    {
                        if (exiting)
                            break;

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(sentence);
                        int vtSeqIdx = 0;
                        for (int i = 0; i < sentence.Length && !exiting; i++)
                        {
                            char TextChar = sentence[i];

                            // Write a character individually
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                Console.Write(buffered.ToString());
                                buffered.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    exiting = true;
                                LinesMade = 0;
                            }
                            buffered.Append(ConsoleExtensions.BufferChar(sentence, sequences, ref i, ref vtSeqIdx, out _));
                        }
                        if (!exiting)
                        {
                            buffered.AppendLine();
                            LinesMade++;
                        }
                    }
                    Console.Write(buffered.ToString());
                    buffered.Clear();
                    if (Line)
                        Console.WriteLine();
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedColor(string Text, bool Line, ConsoleColors color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(color));
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);

                    // Write wrapped output
                    WriteWrappedPlain(Text, Line, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedColorBack(string Text, bool Line, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(ForegroundColor));
                    ColorTools.SetConsoleColor(new Color(BackgroundColor));

                    // Write wrapped output
                    WriteWrappedPlain(Text, Line, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedColor(string Text, bool Line, Color color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Set the console color to selected background and foreground colors
                    ColorTools.SetConsoleColor(color);
                    ColorTools.SetConsoleColor(new Color(ConsoleColors.Black), true);

                    // Write wrapped output
                    WriteWrappedPlain(Text, Line, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support and wraps the long terminal output if needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedColorBack(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Set the console color to selected background and foreground colors
                    ColorTools.SetConsoleColor(ForegroundColor);
                    ColorTools.SetConsoleColor(BackgroundColor, true);

                    // Write wrapped output
                    WriteWrappedPlain(Text, Line, vars);
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
