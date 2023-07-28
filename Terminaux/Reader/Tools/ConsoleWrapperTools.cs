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
using Terminaux.Reader;

namespace Terminaux.Tools
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles. Taken from Kernel Simulator 0.1.0.
    /// </summary>
    public static class ConsoleWrapperTools
    {
        // Actions to modify the wrapper
        internal static Func<bool> actionIsDumb = () => IsDumb;
        internal static Func<int> actionCursorLeft = () => CursorLeft;
        internal static Func<int> actionCursorTop = () => CursorTop;
        internal static Func<int> actionWindowWidth = () => WindowWidth;
        internal static Func<int> actionWindowHeight = () => WindowHeight;
        internal static Func<int> actionBufferHeight = () => BufferHeight;
        internal static Action<bool> actionCursorVisible = (val) => CursorVisible = val;
        internal static Action<bool> actionTreatCtrlCAsInput = (val) => TreatCtrlCAsInput = val;
        internal static Func<bool> actionKeyAvailable = () => KeyAvailable;
        internal static Action<int, int> actionSetCursorPosition = SetCursorPosition;
        internal static Action<int> actionSetCursorLeft = SetCursorLeft;
        internal static Action<int> actionSetCursorTop = SetCursorTop;
        internal static Action actionBeep = Beep;
        internal static Func<bool, ConsoleKeyInfo> actionReadKey = ReadKey;
        internal static Action<char> actionWrite = Write;
        internal static Action<string> actionWrite1 = Write;
        internal static Action<string, object[]> actionWrite2 = Write;
        internal static Action actionWriteLine = WriteLine;
        internal static Action<string> actionWriteLine1 = WriteLine;
        internal static Action<string, object[]> actionWriteLine2 = WriteLine;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static Func<bool> ActionIsDumb
        { 
            get => actionIsDumb;
            set => actionIsDumb = value ?? (() => IsDumb);
        }
        /// <summary>
        /// The cursor left position
        /// </summary>
        public static Func<int> ActionCursorLeft
        {
            get => actionCursorLeft;
            set => actionCursorLeft = value ?? (() => CursorLeft);
        }
        /// <summary>
        /// The cursor top position
        /// </summary>
        public static Func<int> ActionCursorTop 
        {
            get => actionCursorTop;
            set => actionCursorTop = value ?? (() => CursorTop);
        }
        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static Func<int> ActionWindowWidth 
        {
            get => actionWindowWidth;
            set => actionWindowWidth = value ?? (() => WindowWidth);
        }
        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static Func<int> ActionWindowHeight 
        {
            get => actionWindowHeight;
            set => actionWindowHeight = value ?? (() => WindowHeight);
        }
        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static Func<int> ActionBufferHeight 
        {
            get => actionBufferHeight;
            set => actionBufferHeight = value ?? (() => WindowHeight);
        }
        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static Action<bool> ActionCursorVisible
        {
            get => actionCursorVisible;
            set => actionCursorVisible = value ?? ((val) => CursorVisible = val);
        }
        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static Action<bool> ActionTreatCtrlCAsInput
        {
            get => actionTreatCtrlCAsInput;
            set => actionTreatCtrlCAsInput = value ?? ((val) => TreatCtrlCAsInput = val);
        }
        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static Func<bool> ActionKeyAvailable
        {
            get => actionKeyAvailable;
            set => actionKeyAvailable = value ?? (() => KeyAvailable);
        }
        /// <summary>
        /// Sets the cursor position<br></br><br></br>
        /// - First integer is the X position from 0<br></br>
        /// - Second integer is the Y position from 0
        /// </summary>
        public static Action<int, int> ActionSetCursorPosition
        {
            get => actionSetCursorPosition;
            set => actionSetCursorPosition = value ?? (SetCursorPosition);
        }
        /// <summary>
        /// Sets the cursor left position<br></br><br></br>
        /// - First integer is the X position from 0
        /// </summary>
        public static Action<int> ActionSetCursorLeft
        {
            get => actionSetCursorLeft;
            set => actionSetCursorLeft = value ?? (SetCursorLeft);
        }
        /// <summary>
        /// Sets the cursor top position<br></br><br></br>
        /// - First integer is the Y position from 0
        /// </summary>
        public static Action<int> ActionSetCursorTop
        {
            get => actionSetCursorTop;
            set => actionSetCursorTop = value ?? (SetCursorTop);
        }
        /// <summary>
        /// Beeps the console
        /// </summary>
        public static Action ActionBeep
        {
            get => actionBeep;
            set => actionBeep = value ?? (Beep);
        }
        /// <summary>
        /// Reads a key<br></br><br></br>
        /// - Boolean value indicates whether to intercept
        /// </summary>
        public static Func<bool, ConsoleKeyInfo> ActionReadKey
        {
            get => actionReadKey;
            set => actionReadKey = value ?? (ReadKey);
        }
        /// <summary>
        /// Writes a character to console<br></br><br></br>
        /// - A character
        /// </summary>
        public static Action<char> ActionWriteChar
        {
            get => actionWrite;
            set => actionWrite = value ?? (Write);
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write
        /// </summary>
        public static Action<string> ActionWriteString
        {
            get => actionWrite1;
            set => actionWrite1 = value ?? (Write);
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, object[]> ActionWriteParameterized
        {
            get => actionWrite2;
            set => actionWrite2 = value ?? (Write);
        }
        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static Action ActionWriteLine
        {
            get => actionWriteLine;
            set => actionWriteLine = value ?? (WriteLine);
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write
        /// </summary>
        public static Action<string> ActionWriteLineString
        {
            get => actionWriteLine1;
            set => actionWriteLine1 = value ?? (WriteLine);
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, object[]> ActionWriteLineParameterized
        {
            get => actionWriteLine2;
            set => actionWriteLine2 = value ?? (WriteLine);
        }

        // Wrapper starts here
        private static bool _dumbSet = false;
        private static bool _dumb = true;

        private static bool IsDumb
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

        private static int CursorLeft
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

        private static int CursorTop
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

        private static int WindowWidth
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowWidth;
            }
        }

        private static int WindowHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.WindowHeight;
            }
        }

        private static int BufferHeight
        {
            get
            {
                if (IsDumb)
                    return int.MaxValue;
                return Console.BufferHeight;
            }
        }

        private static bool CursorVisible
        {
            set
            {
                if (!IsDumb)
                    Console.CursorVisible = value;
            }
        }

        private static bool TreatCtrlCAsInput
        {
            set
            {
                if (!IsDumb)
                    Console.TreatControlCAsInput = value;
            }
        }

        private static bool KeyAvailable
        {
            get
            {
                if (IsDumb)
                    return false;
                return Console.KeyAvailable;
            }
        }

        private static void SetCursorPosition(int left, int top)
        {
            if (!IsDumb)
                Console.SetCursorPosition(left, top);
        }

        private static void SetCursorLeft(int left)
        {
            if (!IsDumb)
                Console.CursorLeft = left;
        }

        private static void SetCursorTop(int top)
        {
            if (!IsDumb)
                Console.CursorTop = top;
        }

        private static void Beep() => 
            Console.Beep();

        private static ConsoleKeyInfo ReadKey(bool intercept = false) =>
            Console.ReadKey(intercept);

        private static void Write(char value)
        {
            Console.Write(value);
            if (CursorLeft >= WindowWidth - TerminauxerSettings.RightMargin)
                if (CursorTop != BufferHeight)
                    SetCursorPosition(TerminauxerSettings.LeftMargin, CursorTop + 1);
                else
                    WriteLine();
        }

        private static void Write(string text)
        {
            foreach (char textc in text)
                Write(textc);
        }

        private static void Write(string text, params object[] args)
        {
            foreach (char textc in string.Format(text, args))
                Write(textc);
        }

        private static void WriteLine() => 
            Console.WriteLine();

        private static void WriteLine(string text)
        {
            foreach (char textc in text)
                Write(textc);
            WriteLine();
        }

        private static void WriteLine(string text, params object[] args)
        {
            foreach (char textc in string.Format(text, args))
                Write(textc);
            WriteLine();
        }
    }
}
