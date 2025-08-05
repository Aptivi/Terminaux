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
using Terminaux.Base.Structures;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

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
            ConsoleWrapperTools.Wrapper.IsDumb;

        /// <summary>
        /// Has the console moved? Should be set by Write*, Set*, and all console functions that have to do with moving the console.
        /// </summary>
        public static bool MovementDetected =>
            ConsoleWrapperTools.Wrapper.MovementDetected;

        /// <summary>
        /// The cursor left position
        /// </summary>
        public static int CursorLeft
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.Wrapper.CursorLeft;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetCursorLeft(value);
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
                    return ConsoleWrapperTools.Wrapper.CursorTop;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetCursorTop(value);
                }
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static Coordinate GetCursorPosition() =>
            ConsoleWrapperTools.Wrapper.GetCursorPosition;

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                lock (Console.In)
                {
                    return ConsoleWrapperTools.Wrapper.WindowWidth;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetWindowWidth(value);
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
                    return ConsoleWrapperTools.Wrapper.WindowHeight;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetWindowHeight(value);
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
                    return ConsoleWrapperTools.Wrapper.BufferWidth;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetBufferWidth(value);
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
                    return ConsoleWrapperTools.Wrapper.BufferHeight;
                }
            }
            set
            {
                lock (Console.In)
                {
                    ConsoleWrapperTools.Wrapper.SetBufferHeight(value);
                }
            }
        }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static bool CursorVisible
        {
            get => ConsoleWrapperTools.Wrapper.CursorVisible;
            set => ConsoleWrapperTools.Wrapper.CursorVisible = value;
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static bool KeyAvailable =>
            ConsoleWrapperTools.Wrapper.KeyAvailable;

        /// <summary>
        /// Whether to treat Ctrl + C as input or not
        /// </summary>
        public static bool TreatCtrlCAsInput
        {
            get => ConsoleWrapperTools.Wrapper.TreatCtrlCAsInput;
            set => ConsoleWrapperTools.Wrapper.TreatCtrlCAsInput = value;
        }

        /// <summary>
        /// Clears the console screen.
        /// </summary>
        public static void Clear() =>
            ConsoleWrapperTools.Wrapper.Clear();

        /// <summary>
        /// Clears the console screen while loading the background.
        /// </summary>
        public static void ClearLoadBack() =>
            ConsoleWrapperTools.Wrapper.ClearLoadBack();

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public static void SetCursorPosition(int left, int top) =>
            ConsoleWrapperTools.Wrapper.SetCursorPosition(left, top);

        /// <summary>
        /// Sets the window dimensions
        /// </summary>
        /// <param name="width">The window width to be set (from 0)</param>
        /// <param name="height">The window height to be set (from 0)</param>
        public static void SetWindowDimensions(int width, int height) =>
            ConsoleWrapperTools.Wrapper.SetWindowDimensions(width, height);

        /// <summary>
        /// Sets the buffer dimensions
        /// </summary>
        /// <param name="width">The buffer width to be set (from 0)</param>
        /// <param name="height">The buffer height to be set (from 0)</param>
        public static void SetBufferDimensions(int width, int height) =>
            ConsoleWrapperTools.Wrapper.SetBufferDimensions(width, height);

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep() =>
            ConsoleWrapperTools.Wrapper.Beep();

        /// <summary>
        /// Beeps the console
        /// </summary>
        /// <param name="freq">Frequency in hertz</param>
        /// <param name="ms">Duration in milliseconds</param>
        public static void Beep(int freq, int ms) =>
            ConsoleWrapperTools.Wrapper.BeepCustom(freq, ms);

        /// <summary>
        /// Beeps the console (VT Sequence method)
        /// </summary>
        public static void BeepSeq() =>
            ConsoleWrapperTools.Wrapper.BeepSeq();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            TermReaderTools.isWaitingForInput = true;
            var key = ConsoleWrapperTools.Wrapper.ReadKey(intercept);
            TermReaderTools.isWaitingForInput = false;
            return key;
        }

        /// <summary>
        /// Writes a character to console (stdout)
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.Write(value);
            }
        }

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.Write(text);
            }
        }

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.Write(text, args);
            }
        }

        /// <summary>
        /// Writes new line to console (stdout)
        /// </summary>
        public static void WriteLine()
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteLine();
            }
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteLine(text);
            }
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteLine(text, args);
            }
        }

        /// <summary>
        /// Writes a character to console (stderr)
        /// </summary>
        /// <param name="value">A character</param>
        public static void WriteError(char value)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteError(value);
            }
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteError(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteError(text);
            }
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteError(string text, params object[] args)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteError(text, args);
            }
        }

        /// <summary>
        /// Writes new line to console (stderr)
        /// </summary>
        public static void WriteLineError()
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteErrorLine();
            }
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLineError(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteErrorLine(text);
            }
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLineError(string text, params object[] args)
        {
            lock (TextWriterRaw.WriteLock)
            {
                ConsoleWrapperTools.Wrapper.WriteErrorLine(text, args);
            }
        }
    }
}
