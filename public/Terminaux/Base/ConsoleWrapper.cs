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

using System;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
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
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionCursorLeft();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetCursorLeft(value);
                }
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static int CursorTop
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionCursorTop();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetCursorTop(value);
                }
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static Coordinate GetCursorPosition() =>
            ConsoleWrapperTools.ActionGetCursorPosition();

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionWindowWidth();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetWindowWidth(value);
                }
            }
        }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionWindowHeight();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetWindowHeight(value);
                }
            }
        }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        public static int BufferWidth
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionBufferWidth();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetBufferWidth(value);
                }
            }
        }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static int BufferHeight
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.ActionBufferHeight();
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.ActionSetBufferHeight(value);
                }
            }
        }

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
        /// Sets the window dimensions
        /// </summary>
        /// <param name="width">The window width to be set (from 0)</param>
        /// <param name="height">The window height to be set (from 0)</param>
        public static void SetWindowDimensions(int width, int height) =>
            ConsoleWrapperTools.ActionSetWindowDimensions(width, height);

        /// <summary>
        /// Sets the buffer dimensions
        /// </summary>
        /// <param name="width">The buffer width to be set (from 0)</param>
        /// <param name="height">The buffer height to be set (from 0)</param>
        public static void SetBufferDimensions(int width, int height) =>
            ConsoleWrapperTools.ActionSetBufferDimensions(width, height);

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep() =>
            ConsoleWrapperTools.ActionBeep();

        /// <summary>
        /// Beeps the console
        /// </summary>
        /// <param name="freq">Frequency in hertz</param>
        /// <param name="ms">Duration in milliseconds</param>
        public static void Beep(int freq, int ms) =>
            ConsoleWrapperTools.ActionBeepCustom(freq, ms);

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
        /// Writes a character to console (stdout)
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value) =>
            ConsoleWrapperTools.ActionWriteChar(value);

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text) =>
            ConsoleWrapperTools.ActionWriteString(text);

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteParameterized(text, args);

        /// <summary>
        /// Writes new line to console (stdout)
        /// </summary>
        public static void WriteLine() =>
            ConsoleWrapperTools.ActionWriteLine();

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text) =>
            ConsoleWrapperTools.ActionWriteLineString(text);

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteLineParameterized(text, args);

        /// <summary>
        /// Writes a character to console (stderr)
        /// </summary>
        /// <param name="value">A character</param>
        public static void WriteError(char value) =>
            ConsoleWrapperTools.ActionWriteErrorChar(value);

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteError(string text) =>
            ConsoleWrapperTools.ActionWriteErrorString(text);

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteError(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteErrorParameterized(text, args);

        /// <summary>
        /// Writes new line to console (stderr)
        /// </summary>
        public static void WriteLineError() =>
            ConsoleWrapperTools.ActionWriteErrorLine();

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLineError(string text) =>
            ConsoleWrapperTools.ActionWriteErrorLineString(text);

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLineError(string text, params object[] args) =>
            ConsoleWrapperTools.ActionWriteErrorLineParameterized(text, args);
    }
}
