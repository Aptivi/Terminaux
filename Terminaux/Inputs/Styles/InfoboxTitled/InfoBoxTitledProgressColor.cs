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
using System.Threading;
using System.Collections.Generic;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;
using Terminaux.Base;
using Terminaux.Writer.FancyWriters;
using Textify.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;

namespace Terminaux.Inputs.Styles.InfoboxTitled
{
    /// <summary>
    /// Info box writer with progress and color support
    /// </summary>
    public static class InfoBoxTitledProgressColor
    {
        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressPlain(string title, double progress, string text, params object[] vars) =>
            WriteInfoBoxTitledProgressPlain(title, progress, text,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
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
        public static void WriteInfoBoxTitledProgressPlain(string title, double progress, string text,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = TextTools.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = [];
                foreach (var line in splitLines)
                {
                    var lineSentences = TextTools.GetWrappedSentences(line, ConsoleWrapper.WindowWidth - 4);
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
                int maxWidth = ConsoleWrapper.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count + 5;
                if (maxHeight >= ConsoleWrapper.WindowHeight)
                    maxHeight = ConsoleWrapper.WindowHeight - 4;
                int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
                var boxBuffer = new StringBuilder();
                string border = BorderTextColor.RenderBorderTextPlain(title, borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                boxBuffer.Append(border);

                // Render text inside it
                ConsoleWrapper.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    if (i % (maxHeight - 5) == 0 && i > 0)
                    {
                        // Reached the end of the box. Bail, because we need to print the progress.
                        break;
                    }
                    boxBuffer.Append($"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}{line}");
                }

                // Render the final result and write the progress bar
                int progressPosX = borderX + 4;
                int progressPosY = borderY + maxHeight - 3;
                int maxProgressWidth = maxWidth - 4;
                TextWriterColor.WritePlain(boxBuffer.ToString(), false);
                ProgressBarColor.WriteProgressPlain(progress, progressPosX, progressPosY, maxProgressWidth);
                boxBuffer.Clear();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            finally
            {
                ConsoleWrapper.CursorVisible = initialCursorVisible;
            }
        }

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgress(string title, double progress, string text, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgress(string title, double progress, string text, bool waitForInput, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, ConsoleColors InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledProgressColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, bool waitForInput, ConsoleColors InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledProgressColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, ConsoleColors InfoBoxTitledProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, bool waitForInput, ConsoleColors InfoBoxTitledProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(InfoBoxTitledProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledProgressColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, bool waitForInput, Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledProgressColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, bool waitForInput, Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        InfoBoxTitledProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
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
        public static void WriteInfoBoxTitledProgress(string title, double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
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
        public static void WriteInfoBoxTitledProgress(string title, double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar,
                UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar,
                LeftFrameChar, RightFrameChar,
                new Color(ConsoleColors.Gray), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxTitledProgressColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(
                title, progress, text, waitForInput,
                UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar,
                UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar,
                new Color(InfoBoxTitledProgressColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledProgressColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(InfoBoxTitledProgressColor), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors InfoBoxTitledProgressColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, true, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledProgressColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColor(string title, double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledProgressColor, params object[] vars) =>
            WriteInfoBoxTitledProgressColorBack(title, progress, text, waitForInput, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, InfoBoxTitledProgressColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="progress">Progress percentage from 0 to 100</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for info box</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for info box</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for info box</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for info box</param>
        /// <param name="UpperFrameChar">Upper frame character for info box</param>
        /// <param name="LowerFrameChar">Lower frame character for info box</param>
        /// <param name="LeftFrameChar">Left frame character for info box</param>
        /// <param name="RightFrameChar">Right frame character for info box</param>
        /// <param name="InfoBoxTitledProgressColor">InfoBoxTitledProgress color</param>
        /// <param name="BackgroundColor">InfoBoxTitledProgress background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="waitForInput">Waits for input or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteInfoBoxTitledProgressColorBack(string title, double progress, string text, bool waitForInput,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color InfoBoxTitledProgressColor, Color BackgroundColor, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            try
            {
                // Deal with the lines to actually fit text in the infobox
                string finalInfoRendered = TextTools.FormatString(text, vars);
                string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
                List<string> splitFinalLines = [];
                foreach (var line in splitLines)
                {
                    var lineSentences = TextTools.GetWrappedSentences(line, ConsoleWrapper.WindowWidth - 4);
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
                int maxWidth = ConsoleWrapper.WindowWidth - 4;
                int maxHeight = splitFinalLines.Count + 5;
                if (maxHeight >= ConsoleWrapper.WindowHeight)
                    maxHeight = ConsoleWrapper.WindowHeight - 4;
                int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
                int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
                int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
                var boxBuffer = new StringBuilder();
                string border = BorderTextColor.RenderBorderTextPlain(title, borderX, borderY, maxWidth, maxHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
                boxBuffer.Append(
                    $"{InfoBoxTitledProgressColor.VTSequenceForeground}" +
                    $"{BackgroundColor.VTSequenceBackground}" +
                    $"{border}"
                );

                // Render text inside it
                ConsoleWrapper.CursorVisible = false;
                for (int i = 0; i < splitFinalLines.Count; i++)
                {
                    var line = splitFinalLines[i];
                    if (i % (maxHeight - 5) == 0 && i > 0)
                    {
                        // Reached the end of the box. Bail, because we need to print the progress.
                        break;
                    }
                    boxBuffer.Append($"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + i % maxHeight + 1)}{line}");
                }

                // Render the final result and write the progress bar
                int progressPosX = borderX + 4;
                int progressPosY = borderY + maxHeight - 3;
                int maxProgressWidth = maxWidth - 4;
                boxBuffer.Append(
                    ColorTools.currentForegroundColor.VTSequenceForeground +
                    ColorTools.currentBackgroundColor.VTSequenceBackground
                );
                TextWriterColor.WritePlain(boxBuffer.ToString(), false);
                ProgressBarColor.WriteProgress(progress, progressPosX, progressPosY, progressPosX * 2 + 2, InfoBoxTitledProgressColor, InfoBoxTitledProgressColor, BackgroundColor);
                boxBuffer.Clear();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("There is a serious error when printing text. {0}", ex.Message);
            }
            finally
            {
                ConsoleWrapper.CursorVisible = initialCursorVisible;
            }
        }
    }
}
