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
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Reader.Inputs;
using System.Diagnostics;
using Terminaux.Reader;
using Terminaux.Colors;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Info box writer with color support
    /// </summary>
    public static class InfoBoxColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text, params object[] vars) =>
            WriteInfoBoxPlain(text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxPlain(text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, waitForInput, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxPlain(string text,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, bool waitForInput, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2 - 1;
                BorderColor.WriteBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);

                // Render text inside it
                Console.CursorVisible = false;
                bool appendMinusOne = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line, borderX + 1, borderY + 1 + i % maxHeight - (appendMinusOne ? 1 : 0));
                    if (i % maxHeight == 0 && i > 0)
                    {
                        // Reached the end of the box. Wait for keypress then clear the box
                        appendMinusOne = true;
                        if (waitForInput)
                            Input.DetectKeypress();
                        else
                            Thread.Sleep(5000);
                        BorderColor.WriteBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    }
                }

                // Wait until the user presses any key to close the box
                if (waitForInput)
                    Input.DetectKeypress();
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainInput(string text, params object[] vars) =>
            WriteInfoBoxPlainInput(text,
                                   BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                                   BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                                   BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                                   BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxPlainInput(string text,
                                                    char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                                    char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Now, we need an extra space for input
                splitFinalLines.Add("");

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2 - 1;
                BorderColor.WriteBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);

                // Render text inside it
                bool appendMinusOne = false;
                Console.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line, borderX + 1, borderY + 1 + i % maxHeight - (appendMinusOne ? 1 : 0));
                    if (i % maxHeight == 0 && i > 0)
                    {
                        // Reached the end of the box. Wait for keypress then clear the box
                        appendMinusOne = true;
                        Input.DetectKeypress();
                        BorderColor.WriteBorderPlain(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                    }
                }

                // Wait until the user presses any key to close the box
                var settings = new TermReaderSettings()
                {
                    RightMargin = borderX,
                };
                string input = Input.ReadLineWrapped("", "", settings);
                return input;
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
            return "";
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, params object[] vars) =>
            WriteInfoBox(text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBox(text, true,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBox(text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBox(text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, Color BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2 - 1;
                BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);

                // Render text inside it
                bool appendMinusOne = false;
                Console.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line, borderX + 1, borderY + 1 + i % maxHeight - (appendMinusOne ? 1 : 0), InfoBoxColor, BackgroundColor);
                    if (i % maxHeight == 0 && i > 0)
                    {
                        // Reached the end of the box. Wait for keypress then clear the box
                        appendMinusOne = true;
                        if (waitForInput)
                            Input.DetectKeypress();
                        else
                            Thread.Sleep(5000);
                        BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);
                    }
                }

                // Wait until the user presses any key to close the box
                if (waitForInput)
                    Input.DetectKeypress();
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBox(text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBox(string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2 - 1;
                BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);

                // Render text inside it
                bool appendMinusOne = false;
                Console.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line, borderX + 1, borderY + 1 + i % maxHeight - (appendMinusOne ? 1 : 0), InfoBoxColor, BackgroundColor);
                    if (i % maxHeight == 0 && i > 0)
                    {
                        // Reached the end of the box. Wait for keypress then clear the box
                        appendMinusOne = true;
                        if (waitForInput)
                            Input.DetectKeypress();
                        else
                            Thread.Sleep(5000);
                        BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);
                    }
                }

                // Wait until the user presses any key to close the box
                if (waitForInput)
                    Input.DetectKeypress();
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, params object[] vars) =>
            WriteInfoBoxInput(text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBoxInput(text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxInput(text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxInput(text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="InfoBoxColor">InfoBox color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBox background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text, Color InfoBoxColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxInput(text,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxInput(text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, params object[] vars) =>
            WriteInfoBoxInput(text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxColor, Color BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Now, we need an extra space for input
                splitFinalLines.Add("");

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2;
                BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);

                // Render text inside it
                Console.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line.Truncate(maxRenderWidth), borderX + 1, borderY + 1 + i, InfoBoxColor, BackgroundColor);
                    if (i % maxHeight == 0 && i > 0)
                        Input.DetectKeypress();
                }

                // Wait until the user presses any key to close the box
                var settings = new TermReaderSettings()
                {
                    RightMargin = borderX,
                };
                string input = Input.ReadLineWrapped("", "", settings);
                return input;
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
            return "";
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, params object[] vars) =>
            WriteInfoBoxInput(text, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxColor), new Color(ConsoleColors.Black), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxColor">InfoBox color</param>
        /// <param name="BackgroundColor">InfoBox background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static string WriteInfoBoxInput(string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = Console.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = ConsoleExtensions.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = new();
                foreach (var line in splitLines)
                {
                    var lineSentences = ConsoleExtensions.GetWrappedSentences(line, Console.WindowWidth - 4);
                    foreach (var lineSentence in lineSentences)
                        splitFinalLines.Add(lineSentence);
                }

                // Trim the new lines until we reach a full line
                for (int i = splitFinalLines.Count - 1; i >= 0; i--)
                {
                    string line = splitFinalLines[i];
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                    splitFinalLines.RemoveAt(i);
                }

                // Now, we need an extra space for input
                splitFinalLines.Add("");

                // Fill the info box with text inside it
                int maxWidth = splitFinalLines.Max((str) => str.Length);
                if (maxWidth >= Console.WindowWidth)
                    maxWidth = Console.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count;
                if (maxHeight >= Console.WindowHeight)
                    maxHeight = Console.WindowHeight - 4;
                int maxRenderWidth = Console.WindowWidth - 6;
                int borderX = Console.WindowWidth / 2 - maxWidth / 2;
                int borderY = Console.WindowHeight / 2 - maxHeight / 2;
                BorderColor.WriteBorder(borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxColor, BackgroundColor);

                // Render text inside it
                Console.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    TextWriterWhereColor.WriteWhere(line.Truncate(maxRenderWidth), borderX + 1, borderY + 1 + i, InfoBoxColor, BackgroundColor);
                    if (i % maxHeight == 0 && i > 0)
                        Input.DetectKeypress();
                }

                // Wait until the user presses any key to close the box
                var settings = new TermReaderSettings()
                {
                    RightMargin = borderX,
                };
                string input = Input.ReadLineWrapped("", "", settings);
                return input;
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = initialCursorVisible;
            }
            return "";
        }
    }
}
