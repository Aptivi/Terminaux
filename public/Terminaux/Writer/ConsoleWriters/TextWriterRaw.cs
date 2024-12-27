//
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

using System.Diagnostics;
using System;
using Terminaux.Base.Extensions;
using Terminaux.Base;
using Terminaux.Reader;
using SpecProbe.Software.Platform;
using Textify.General;

namespace Terminaux.Writer.ConsoleWriters
{
    /// <summary>
    /// Raw text writers for console
    /// </summary>
    public static class TextWriterRaw
    {
        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the new line into the terminal prompt.
        /// </summary>
        public static void Write()
        {
            lock (WriteLock)
            {
                try
                {
                    ConsoleWrapper.WriteLine();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt plainly with a newline terminator.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, params object[] vars) =>
            WritePlain(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteRaw(string Text, params object[] vars) =>
            WritePlain(Text, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, bool Line, params object[] vars) =>
            WritePlain(Text, null, Line, vars);

        /// <summary>
        /// Outputs the new line into the terminal prompt (stderr).
        /// </summary>
        public static void WriteError()
        {
            lock (WriteLock)
            {
                try
                {
                    ConsoleWrapper.WriteLineError();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt (stderr) plainly with a newline terminator.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteErrorPlain(string Text, params object[] vars) =>
            WriteErrorPlain(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt (stderr) plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteErrorRaw(string Text, params object[] vars) =>
            WriteErrorPlain(Text, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt (stderr) plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteErrorPlain(string Text, bool Line, params object[] vars) =>
            WriteInternal(Text, null, Line, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly with a newline terminator. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, TermReaderSettings? settings, params object[] vars) =>
            WritePlain(Text, settings, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteRaw(string Text, TermReaderSettings? settings, params object[] vars) =>
            WritePlain(Text, settings, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly. Use for TermReader custom bindings.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="settings">Terminal reader settings</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, TermReaderSettings? settings, bool Line, params object[] vars) =>
            WriteInternal(Text, settings, Line, false, vars);

        internal static void WriteInternal(string Text, TermReaderSettings? settings, bool Line, bool stdErr, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    if (PlatformHelper.IsRunningFromMono())
                    {
                        var pos = ConsolePositioning.GetFilteredPositions(Text, Line, vars);
                        FilteredLeft = pos.Item1;
                        FilteredTop = pos.Item2;
                    }

                    // Reverse all the RTL characters
                    if (settings is null)
                        Text = ConsoleMisc.ReverseRtl(Text);

                    // Actually write
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            if (settings is null)
                                if (stdErr)
                                    ConsoleWrapper.WriteLineError(Text, vars);
                                else
                                    ConsoleWrapper.WriteLine(Text, vars);
                            else
                                WriteLineNonStandalone(Text, settings, vars);
                        }
                        else
                        {
                            if (settings is null)
                                if (stdErr)
                                    ConsoleWrapper.WriteLineError(Text);
                                else
                                    ConsoleWrapper.WriteLine(Text);
                            else
                                WriteLineNonStandalone(Text, settings);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        if (settings is null)
                            if (stdErr)
                                ConsoleWrapper.WriteError(Text, vars);
                            else
                                ConsoleWrapper.Write(Text, vars);
                        else
                            WriteNonStandalone(Text, settings, vars);
                    }
                    else
                    {
                        if (settings is null)
                            if (stdErr)
                                ConsoleWrapper.WriteError(Text);
                            else
                                ConsoleWrapper.Write(Text);
                        else
                            WriteNonStandalone(Text, settings);
                    }

                    // Return to the processed position
                    if (PlatformHelper.IsRunningFromMono())
                        ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                    Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
                }
            }
        }

        private static void WriteNonStandalone(string text, TermReaderSettings settings)
        {
            if (settings.state is null)
                throw new TerminauxInternalException(nameof(settings.state));
            int top = settings.state.inputPromptTop;
            int topBegin = settings.state.inputPromptTopBegin;
            var wrapped = ConsoleMisc.GetWrappedSentences(text, settings.state.LongestSentenceLengthFromLeftForGeneralLine + 1, settings.state.writingPrompt ? settings.state.LeftMargin : settings.state.InputPromptLeft - settings.state.LeftMargin);
            for (int i = 0; i < wrapped.Length; i++)
            {
                int wrapTop = top + i;
                string textWrapped = wrapped[i];
                int width = ConsoleChar.EstimateCellWidth(textWrapped);
                WriteRaw(textWrapped);
                if (i + 1 < wrapped.Length)
                {
                    if (width == (i == 0 ? settings.state.LongestSentenceLengthFromLeftForFirstLine : settings.state.LongestSentenceLengthFromLeftForGeneralLine))
                        WriteRaw(" ");
                    ConsoleWrapper.WriteLine();
                    ConsoleWrapper.CursorLeft = settings.LeftMargin;
                }
                if (wrapTop >= ConsoleWrapper.BufferHeight && !settings.state.writingPrompt && top > 0)
                {
                    top--;
                    topBegin--;
                    settings.state.currentCursorPosTop--;
                    ConsoleWrapper.CursorLeft = settings.LeftMargin;
                    if (i == wrapped.Length - 1)
                        ConsoleWrapper.CursorLeft += width;
                }
            }
            settings.state.inputPromptTop = top;
            settings.state.inputPromptTopBegin = topBegin;
        }

        private static void WriteNonStandalone(string text, TermReaderSettings settings, params object[] args)
        {
            string formatted = TextTools.FormatString(text, args);
            WriteNonStandalone(formatted, settings);
        }

        private static void WriteLineNonStandalone(string text, TermReaderSettings settings)
        {
            WriteNonStandalone(text, settings);
            ConsoleWrapper.WriteLine();
        }

        private static void WriteLineNonStandalone(string text, TermReaderSettings settings, params object[] args)
        {
            WriteNonStandalone(text, settings, args);
            ConsoleWrapper.WriteLine();
        }
    }
}
