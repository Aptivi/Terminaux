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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Editor;
using Terminaux.Sequences;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Wrapped writer similar to <c>less</c> and <c>more</c> commands on Unix
    /// </summary>
    public static class WrappedWriter
    {
        /// <summary>
        /// Opens the text viewer similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="args">Arguments to format the text</param>
        public static void OpenWrapped(string text, bool force = false, params object?[]? args) =>
            OpenWrapped(text, ThemeColorType.NeutralText, force, args);

        /// <summary>
        /// Opens the text viewer similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void OpenWrapped(string text, ThemeColorType color = ThemeColorType.NeutralText, bool force = false, params object?[]? args) =>
            OpenWrapped(text, color, ThemeColorType.Background, force, args);

        /// <summary>
        /// Opens the text viewer similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void OpenWrapped(string text, ThemeColorType foregroundColor, ThemeColorType backgroundColor, bool force = false, params object?[]? args) =>
            OpenWrapped(text, ThemeColorsTools.GetColor(foregroundColor), ThemeColorsTools.GetColor(backgroundColor), force, args);

        /// <summary>
        /// Opens the text viewer similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void OpenWrapped(string text, Color color, bool force = false, params object?[]? args) =>
            OpenWrapped(text, color, ThemeColorsTools.GetColor(ThemeColorType.Background), force, args);

        /// <summary>
        /// Opens the text viewer similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="text">Text to write. If it's shorter than the console height, it just prints the text</param>
        /// <param name="force">Forces the text viewer to open, even if the text doesn't exceed the console height</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="args">Arguments to format the text</param>
        public static void OpenWrapped(string text, Color foregroundColor, Color backgroundColor, bool force = false, params object?[]? args)
        {
            try
            {
                // Use the text viewer to avoid code repetition
                text = text.FormatString(args);
                var lines = ConsoleMisc.GetWrappedSentencesByWords(text, ConsoleWrapper.WindowWidth).ToList();
                if (!ConsoleWrapper.IsDumb && (force || lines.Count >= ConsoleWrapper.WindowHeight))
                {
                    TextEditInteractive.OpenInteractive(ref lines, new()
                    {
                        PaneSelectedItemBackColor = foregroundColor,
                        BackgroundColor = backgroundColor,
                    }, true, false);
                }
                else
                    TextWriterColor.WriteColorBack(text, foregroundColor, backgroundColor);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrapped(string Text, bool Line, ThemeColorType colorType = ThemeColorType.NeutralText, params object[] vars)
        {
            try
            {
                var foreground = ThemeColorsTools.GetColor(colorType);
                WriteWrapped(Text, Line, foreground, vars);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing wrapped text. {ex.Message}");
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, wraps the long terminal output if needed, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrapped(string Text, bool Line, ThemeColorType colorTypeForeground = ThemeColorType.NeutralText, ThemeColorType colorTypeBackground = ThemeColorType.Background, params object[] vars)
        {
            try
            {
                var foreground = ThemeColorsTools.GetColor(colorTypeForeground);
                var background = ThemeColorsTools.GetColor(colorTypeBackground);
                WriteWrapped(Text, Line, foreground, background, vars);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing wrapped text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrapped(string Text, bool Line, Color color, params object[] vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ConsoleColoring.SetConsoleColorDry(color);

                // Write wrapped output
                WriteWrappedPlain(Text, Line, vars);

                // Reset the colors
                ConsoleColoring.ResetColors();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing wrapped text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="foregroundColor">A foreground color that will be changed to.</param>
        /// <param name="backgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrapped(string Text, bool Line, Color foregroundColor, Color backgroundColor, params object[] vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ConsoleColoring.SetConsoleColorDry(foregroundColor);
                ConsoleColoring.SetConsoleColorDry(backgroundColor, true);

                // Write wrapped output
                WriteWrappedPlain(Text, Line, vars);

                // Reset the colors
                ConsoleColoring.ResetColors();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing wrapped text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the text in a pager similar to <c>less</c> and <c>more</c> commands on Unix
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            // Check for dumb console
            if (ConsoleWrapper.IsDumb)
            {
                TextWriterColor.Write(Text, vars);
                return;
            }

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
                            var key = Input.ReadKey().Key;
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
                            break;
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
                ConsoleLogger.Error(ex, $"There is a serious error when printing wrapped text. {ex.Message}");
            }
        }
    }
}
