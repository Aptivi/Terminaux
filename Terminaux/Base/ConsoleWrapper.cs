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
using Terminaux.Reader;

namespace Terminaux.Base
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles. This connects to the console wrapper that Terminaux manages.
    /// </summary>
    public static class ConsoleWrapper
    {

        /// <summary>
        /// Checks to see if the console is dumb
        /// </summary>
        public static bool IsDumb =>
            ConsoleWrapperTools.ActionIsDumb();

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
            ConsoleWrapperTools.ActionWindowWidth();

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight =>
            ConsoleWrapperTools.ActionWindowHeight();

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
            get => ConsoleWrapperTools.ActionGetCursorVisible();
            set => ConsoleWrapperTools.ActionCursorVisible(value);
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
        /// Clears the console screen while loading the background.
        /// </summary>
        public static void ClearLoadBack() =>
            ConsoleWrapperTools.ActionClearLoadBack();

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
        /// Beeps the console (VT Sequence method)
        /// </summary>
        public static void BeepSeq() =>
            ConsoleWrapperTools.ActionBeepSeq();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            TermReaderTools.isWaitingForInput = true;
            var key = ConsoleWrapperTools.ActionReadKey(intercept);
            TermReaderTools.isWaitingForInput = false;
            return key;
        }

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value) =>
            ConsoleWrapperTools.ActionWriteChar(value);

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
        internal static void Write(string text, TermReaderSettings settings) =>
            ConsoleWrapperTools.ActionWriteStringNonStandalone(text, settings);

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
        internal static void Write(string text, TermReaderSettings settings, params object[] args) =>
            ConsoleWrapperTools.ActionWriteLineParameterizedNonStandalone(text, settings, args);

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
            ConsoleWrapperTools.ActionWriteLineString(text);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="settings">Reader settings</param>
        public static void WriteLine(string text, TermReaderSettings settings) =>
            ConsoleWrapperTools.ActionWriteLineStringNonStandalone(text, settings);

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
        public static void WriteLine(string text, TermReaderSettings settings, params object[] args) =>
            ConsoleWrapperTools.ActionWriteLineParameterizedNonStandalone(text, settings, args);
    }
}
