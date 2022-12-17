/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.IO;
using System.Text;

namespace TermRead.Wrappers
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles. Taken from Kernel Simulator 0.1.0.
    /// </summary>
    public static class ConsoleWrapper
    {
        private static bool _dumbSet = false;
        private static bool _dumb = true;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static bool IsDumb
        {
            get
            {
                try
                {
                    // Get terminal type
                    string TerminalType = Environment.GetEnvironmentVariable("TERM") ?? "";

                    // Try to cache the value
                    if (!_dumbSet)
                    {
                        _dumbSet = true;
                        int _ = Console.CursorLeft;

                        // If it doesn't get here without throwing exceptions, assume console is dumb. Now, check to see if terminal type is dumb
                        if (TerminalType != "dumb")
                            _dumb = false;
                    }
                }
                catch { }
                return _dumb;
            }
        }

        /// <summary>
        /// The standard output stream that the console uses
        /// </summary>
        public static TextWriter Out => Console.Out;

        /// <summary>
        /// The cursor left position
        /// </summary>
        public static int CursorLeft
        {
            get
            {
                if (IsDumb)
                    return 0;
                return Console.CursorLeft;
            }
            set
            {
                if (!IsDumb)
                    Console.CursorLeft = value;
            }
        }

        /// <summary>
        /// The cursor top position
        /// </summary>
        public static int CursorTop
        {
            get
            {
                if (IsDumb)
                    return 0;
                return Console.CursorTop;
            }
            set
            {
                if (!IsDumb)
                    Console.CursorTop = value;
            }
        }

        /// <summary>
        /// The console window top (rows)
        /// </summary>
        public static int WindowTop
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowTop;
            }
        }

        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowWidth;
            }
        }

        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowHeight;
            }
        }

        /// <summary>
        /// The console buffer width (columns)
        /// </summary>
        public static int BufferWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferWidth;
            }
        }

        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferHeight;
            }
        }

        /// <summary>
        /// The foreground color
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.White;
                return Console.ForegroundColor;
            }
            set
            {
                if (!IsDumb)
                    Console.ForegroundColor = value;
            }
        }

        /// <summary>
        /// The background color
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get
            {
                if (IsDumb)
                    return ConsoleColor.Black;
                return Console.BackgroundColor;
            }
            set
            {
                if (!IsDumb)
                    Console.BackgroundColor = value;
            }
        }

        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static bool CursorVisible
        {
            set
            {
                if (!IsDumb)
                    Console.CursorVisible = value;
            }
        }

        /// <summary>
        /// The output encoding
        /// </summary>
        public static Encoding OutputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return Console.OutputEncoding;
            }
            set
            {
                if (!IsDumb)
                    Console.OutputEncoding = value;
            }
        }

        /// <summary>
        /// The input encoding
        /// </summary>
        public static Encoding InputEncoding
        {
            get
            {
                if (IsDumb)
                    return Encoding.Default;
                return Console.InputEncoding;
            }
            set
            {
                if (!IsDumb)
                    Console.InputEncoding = value;
            }
        }

        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return Console.KeyAvailable;
            }
        }

        /// <summary>
        /// Clears the console screen, filling it with spaces with the selected background color.
        /// </summary>
        public static void Clear()
        {
            if (!IsDumb)
                Console.Clear();
        }

        /// <summary>
        /// Sets the cursor position
        /// </summary>
        /// <param name="left">The left to be set (from 0)</param>
        /// <param name="top">The top to be set (from 0)</param>
        public static void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Resets console colors
        /// </summary>
        public static void ResetColor()
        {
            if (!IsDumb)
                Console.ResetColor();
        }

        /// <summary>
        /// Opens the standard input
        /// </summary>
        public static Stream OpenStandardInput() => Console.OpenStandardInput();

        /// <summary>
        /// Opens the standard output
        /// </summary>
        public static Stream OpenStandardOutput() => Console.OpenStandardOutput();

        /// <summary>
        /// Opens the standard error
        /// </summary>
        public static Stream OpenStandardError() => Console.OpenStandardError();

        /// <summary>
        /// Sets console output
        /// </summary>
        /// <param name="newOut">New output</param>
        public static void SetOut(TextWriter newOut)
        {
            // We need to reset dumb state because the new output may not support usual console features other then reading/writing.
            _dumbSet = false;
            _dumb = true;
            Console.SetOut(newOut);
        }

        /// <summary>
        /// Beeps the console
        /// </summary>
        public static void Beep() => Console.Beep();

        /// <summary>
        /// Reads a key
        /// </summary>
        /// <param name="intercept">Whether to intercept</param>
        public static ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);

        /// <summary>
        /// Writes a character to console
        /// </summary>
        /// <param name="value">A character</param>
        public static void Write(char value) => Console.Write(value);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void Write(string text) => Console.Write(text);

        /// <summary>
        /// Writes text to console
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void Write(string text, params object[] args) => Console.Write(text, args);

        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static void WriteLine() => Console.WriteLine();

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public static void WriteLine(string text) => Console.WriteLine(text);

        /// <summary>
        /// Writes text to console with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public static void WriteLine(string text, params object[] args) => Console.WriteLine(text, args);
    }
}
