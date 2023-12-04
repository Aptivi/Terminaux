//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Terminaux.Reader;
using Textify.General;

namespace Terminaux.Base
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles. This connects to the console wrapper that Terminaux manages.
    /// </summary>
    public static class ConsoleWrapper
    {

        private static bool cursorVisible = true;

        /// <summary>
        /// The cursor left position
        /// </summary>
        public static int CursorLeft
        {
            get => ConsoleWrapperTools.ActionCursorLeft();
            set => ConsoleWrapperTools.ActionSetCursorLeft(value);
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static int CursorTop
        {
            get => ConsoleWrapperTools.ActionCursorTop();
            set => ConsoleWrapperTools.ActionSetCursorTop(value);
        }

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth =>
            ConsoleResizeListener.GetCurrentConsoleSize().Width;

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight =>
            ConsoleResizeListener.GetCurrentConsoleSize().Height;

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static int BufferHeight =>
            ConsoleWrapperTools.ActionBufferHeight();

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static bool CursorVisible
        {
            get => cursorVisible;
            set
            {
                // We can't call Get accessor of the primary CursorVisible since this is Windows only, so we have this decoy variable
                // to make it work on Linux, Android, and macOS.
                cursorVisible = value;
                ConsoleWrapperTools.ActionCursorVisible(value);
            }
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static bool KeyAvailable =>
            ConsoleWrapperTools.ActionKeyAvailable();

        /// <summary>
        /// Whether to treat Ctrl + C as input or not
        /// </summary>
        public static bool TreatCtrlCAsInput
        {
            get => ConsoleWrapperTools.ActionGetTreatCtrlCAsInput();
            set => ConsoleWrapperTools.ActionTreatCtrlCAsInput(value);
        }

        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Clear() =>
            ConsoleWrapperTools.ActionClear();

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public static void SetCursorPosition(int left, int top) =>
            ConsoleWrapperTools.ActionSetCursorPosition(left, top);

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep() =>
            ConsoleWrapperTools.ActionBeep();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false) =>
            ConsoleWrapperTools.ActionReadKey(intercept);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value) =>
            ConsoleWrapperTools.ActionWriteChar(value);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        /// <param name="settings">Reader settings</param>
        internal static void Write(char value, TermReaderSettings settings)
        {
            ConsoleWrapperTools.ActionWriteCharNonStandalone(value, settings);
            if (CursorLeft >= WindowWidth - settings.RightMargin)
            {
                if (CursorTop != BufferHeight)
                    SetCursorPosition(settings.LeftMargin, CursorTop + 1);
                else
                    WriteLine();
            }
        }

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text) =>
            ConsoleWrapperTools.ActionWriteString(text);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="settings">Reader settings</param>
        internal static void Write(string text, TermReaderSettings settings)
        {
            if (settings.RightMargin > 0 || settings.LeftMargin > 0)
            {
                var wrapped = TextTools.GetWrappedSentences(text, WindowWidth - settings.RightMargin - settings.LeftMargin, settings.LeftMargin - 1);
                for (int i = 0; i < wrapped.Length; i++)
                {
                    string textWrapped = wrapped[i];
                    Write(textWrapped);
                    if (i + 1 < wrapped.Length)
                    {
                        WriteLine();
                        CursorLeft = settings.LeftMargin;
                    }
                }
            }
            else
                Write(text);
        }

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteParameterized(text, args);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="args">The arguments to evaluate</param>
        internal static void Write(string text, TermReaderSettings settings, params object[] args)
        {
            string finalText = string.Format(text, args);
            Write(finalText, settings);
        }

        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static void WriteLine() =>
            ConsoleWrapperTools.ActionWriteLine();

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text) =>
            ConsoleWrapperTools.ActionWriteString(text);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="settings">Reader settings</param>
        public static void WriteLine(string text, TermReaderSettings settings)
        {
            Write(text, settings);
            WriteLine();
        }

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteLineParameterized(text, args);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, TermReaderSettings settings, params object[] args)
        {
            Write(text, settings, args);
            WriteLine();
        }
    }
}
