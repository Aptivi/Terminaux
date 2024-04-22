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
using Terminaux.Colors;
using System.Text;
using Textify.General;
using Terminaux.Base;
using System.Diagnostics;
using Terminaux.Base.Extensions;
using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Reader;
using Terminaux.Base.Checks;

namespace Terminaux.Writer.DynamicWriters
{
    /// <summary>
    /// Console text writer with color support (wrapped)
    /// </summary>
    public static class TextWriterWrappedColor
    {
        /// <summary>
        /// Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                var LinesMade = 0;
                try
                {
                    // Format string as needed
                    if (vars.Length > 0)
                        Text = TextTools.FormatString(Text, vars);
                    Text = Text.Replace(Convert.ToChar(13), default);

                    // First, split the text to wrap
                    string[] sentences = ConsoleMisc.GetWrappedSentencesByWords(Text, ConsoleWrapper.WindowWidth);

                    // Iterate through sentences
                    var buffered = new StringBuilder();
                    for (int idx = 0; idx < sentences.Length; idx++)
                    {
                        string sentence = sentences[idx];

                        // Grab each VT sequence from the paragraph and fetch their indexes
                        var sequences = VtSequenceTools.MatchVTSequences(sentence);
                        int vtSeqIdx = 0;
                        bool bail = false;
                        for (int i = 0; i < sentence.Length; i++)
                        {
                            char TextChar = sentence[i];

                            // Write a character individually
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                TextWriterRaw.WriteRaw(buffered.ToString());
                                buffered.Clear();
                                var key = TermReader.ReadKey().Key;
                                switch (key)
                                {
                                    case ConsoleKey.Escape:
                                        return;
                                    case ConsoleKey.PageUp:
                                        bail = true;
                                        idx -= ConsoleWrapper.WindowHeight * 2 - 1;
                                        if (idx < 0)
                                            idx = -1;
                                        break;
                                    case ConsoleKey.PageDown:
                                        bail = true;
                                        if (idx > sentences.Length - 1 - ConsoleWrapper.WindowHeight)
                                            idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                        else
                                            idx--;
                                        break;
                                    case ConsoleKey.UpArrow:
                                        bail = true;
                                        idx -= ConsoleWrapper.WindowHeight + 1;
                                        if (idx < 0)
                                            idx = -1;
                                        break;
                                    case ConsoleKey.DownArrow:
                                        bail = true;
                                        if (idx >= sentences.Length - 1)
                                        {
                                            idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                            break;
                                        }
                                        idx -= ConsoleWrapper.WindowHeight - 1;
                                        if (idx < 0)
                                            idx = -1;
                                        break;
                                    case ConsoleKey.Home:
                                        bail = true;
                                        idx = -1;
                                        break;
                                    case ConsoleKey.End:
                                        bail = true;
                                        idx = sentences.Length - 1 - ConsoleWrapper.WindowHeight;
                                        if (idx < 0)
                                            idx = -1;
                                        break;
                                }
                                LinesMade = 0;
                            }
                            if (bail)
                            {
                                ColorTools.LoadBack();
                                break;
                            }
                            buffered.Append(ConsolePositioning.BufferChar(sentence, sequences, ref i, ref vtSeqIdx, out _));
                        }
                        if (!bail && idx < sentences.Length - 1)
                        {
                            buffered.AppendLine();
                            LinesMade++;
                        }
                    }
                    TextWriterRaw.WriteRaw(buffered.ToString());
                    buffered.Clear();
                    if (Line)
                        TextWriterRaw.Write();
                }
                catch (Exception ex)
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
        public static void WriteWrappedColor(string Text, bool Line, Color color, params object[] vars) =>
            WriteWrappedColorBack(Text, Line, color, ColorTools.currentBackgroundColor, vars);

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
            lock (TextWriterRaw.WriteLock)
            {
                try
                {
                    // Set the console color to selected background and foreground colors
                    ColorTools.SetConsoleColorDry(ForegroundColor);
                    ColorTools.SetConsoleColorDry(BackgroundColor, true);

                    // Write wrapped output
                    WriteWrappedPlain(Text, Line, vars);

                    // Reset the colors
                    ColorTools.ResetColors();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        static TextWriterWrappedColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
