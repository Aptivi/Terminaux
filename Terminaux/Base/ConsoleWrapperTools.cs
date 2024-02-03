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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Base
{
    /// <summary>
    /// Wrapper for the <see cref="Console"/> class to ensure safety against dumb consoles and Windows-only features.
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
        internal static Func<bool> actionGetCursorVisible = () => CursorVisible;
        internal static Action<bool> actionTreatCtrlCAsInput = (val) => TreatCtrlCAsInput = val;
        internal static Func<bool> actionGetTreatCtrlCAsInput = () => TreatCtrlCAsInput;
        internal static Func<bool> actionKeyAvailable = () => KeyAvailable;
        internal static Action<int, int> actionSetCursorPosition = SetCursorPosition;
        internal static Action<int> actionSetCursorLeft = SetCursorLeft;
        internal static Action<int> actionSetCursorTop = SetCursorTop;
        internal static Action actionBeep = Beep;
        internal static Action actionBeepSeq = BeepSeq;
        internal static Action actionClear = Clear;
        internal static Action actionClearLoadBack = ClearLoadBack;
        internal static Func<bool, ConsoleKeyInfo> actionReadKey = ReadKey;
        internal static Action<string, TermReaderSettings> actionWriteNonStandalone1 = WriteNonStandalone;
        internal static Action<string, TermReaderSettings, object[]> actionWriteNonStandalone2 = WriteNonStandalone;
        internal static Action<string, TermReaderSettings> actionWriteLineNonStandalone1 = WriteLineNonStandalone;
        internal static Action<string, TermReaderSettings, object[]> actionWriteLineNonStandalone2 = WriteLineNonStandalone;
        internal static Action<char> actionWrite = Write;
        internal static Action<string> actionWrite1 = Write;
        internal static Action<string, object[]> actionWrite2 = Write;
        internal static Action<string> actionWriteLine1 = WriteLine;
        internal static Action<string, object[]> actionWriteLine2 = WriteLine;
        internal static Action actionWriteLine = WriteLine;

        /// <summary>
        /// Is the console a dumb console?
        /// </summary>
        public static Func<bool> ActionIsDumb
        {
            internal get => actionIsDumb;
            set => actionIsDumb = value ?? (() => IsDumb);
        }
        /// <summary>
        /// The cursor left position
        /// </summary>
        public static Func<int> ActionCursorLeft
        {
            internal get => actionCursorLeft;
            set => actionCursorLeft = value ?? (() => CursorLeft);
        }
        /// <summary>
        /// The cursor top position
        /// </summary>
        public static Func<int> ActionCursorTop
        {
            internal get => actionCursorTop;
            set => actionCursorTop = value ?? (() => CursorTop);
        }
        /// <summary>
        /// The console window width (columns)
        /// </summary>
        public static Func<int> ActionWindowWidth
        {
            internal get => actionWindowWidth;
            set => actionWindowWidth = value ?? (() => WindowWidth);
        }
        /// <summary>
        /// The console window height (rows)
        /// </summary>
        public static Func<int> ActionWindowHeight
        {
            internal get => actionWindowHeight;
            set => actionWindowHeight = value ?? (() => WindowHeight);
        }
        /// <summary>
        /// The console buffer height (rows)
        /// </summary>
        public static Func<int> ActionBufferHeight
        {
            internal get => actionBufferHeight;
            set => actionBufferHeight = value ?? (() => WindowHeight);
        }
        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static Action<bool> ActionCursorVisible
        {
            internal get => actionCursorVisible;
            set => actionCursorVisible = value ?? ((val) => CursorVisible = val);
        }
        /// <summary>
        /// The cursor visibility mode
        /// </summary>
        public static Func<bool> ActionGetCursorVisible
        {
            internal get => actionGetCursorVisible;
            set => actionGetCursorVisible = value ?? (() => CursorVisible);
        }
        /// <summary>
        /// Whether to treat CTRL + C as input
        /// </summary>
        public static Action<bool> ActionTreatCtrlCAsInput
        {
            internal get => actionTreatCtrlCAsInput;
            set => actionTreatCtrlCAsInput = value ?? ((val) => TreatCtrlCAsInput = val);
        }
        /// <summary>
        /// Whether to treat CTRL + C as input
        /// </summary>
        public static Func<bool> ActionGetTreatCtrlCAsInput
        {
            internal get => actionGetTreatCtrlCAsInput;
            set => actionGetTreatCtrlCAsInput = value ?? (() => TreatCtrlCAsInput);
        }
        /// <summary>
        /// Whether a key is pressed
        /// </summary>
        public static Func<bool> ActionKeyAvailable
        {
            internal get => actionKeyAvailable;
            set => actionKeyAvailable = value ?? (() => KeyAvailable);
        }
        /// <summary>
        /// Sets the cursor position<br></br><br></br>
        /// - First integer is the X position from 0<br></br>
        /// - Second integer is the Y position from 0
        /// </summary>
        public static Action<int, int> ActionSetCursorPosition
        {
            internal get => actionSetCursorPosition;
            set => actionSetCursorPosition = value ?? SetCursorPosition;
        }
        /// <summary>
        /// Sets the cursor left position<br></br><br></br>
        /// - First integer is the X position from 0
        /// </summary>
        public static Action<int> ActionSetCursorLeft
        {
            internal get => actionSetCursorLeft;
            set => actionSetCursorLeft = value ?? SetCursorLeft;
        }
        /// <summary>
        /// Sets the cursor top position<br></br><br></br>
        /// - First integer is the Y position from 0
        /// </summary>
        public static Action<int> ActionSetCursorTop
        {
            internal get => actionSetCursorTop;
            set => actionSetCursorTop = value ?? SetCursorTop;
        }
        /// <summary>
        /// Beeps the console
        /// </summary>
        public static Action ActionBeep
        {
            internal get => actionBeep;
            set => actionBeep = value ?? Beep;
        }
        /// <summary>
        /// Beeps the console (VT Sequence method)
        /// </summary>
        public static Action ActionBeepSeq
        {
            internal get => actionBeepSeq;
            set => actionBeepSeq = value ?? BeepSeq;
        }
        /// <summary>
        /// Clears the console
        /// </summary>
        public static Action ActionClear
        {
            internal get => actionClear;
            set => actionClear = value ?? Clear;
        }
        /// <summary>
        /// Clears the console while loading the background
        /// </summary>
        public static Action ActionClearLoadBack
        {
            internal get => actionClearLoadBack;
            set => actionClearLoadBack = value ?? ClearLoadBack;
        }
        /// <summary>
        /// Reads a key<br></br><br></br>
        /// - Boolean value indicates whether to intercept
        /// </summary>
        public static Func<bool, ConsoleKeyInfo> ActionReadKey
        {
            internal get => actionReadKey;
            set => actionReadKey = value ?? ReadKey;
        }
        /// <summary>
        /// Writes a character to console<br></br><br></br>
        /// - A character
        /// </summary>
        public static Action<char> ActionWriteChar
        {
            internal get => actionWrite;
            set => actionWrite = value ?? Write;
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write
        /// </summary>
        public static Action<string> ActionWriteString
        {
            internal get => actionWrite1;
            set => actionWrite1 = value ?? Write;
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, object[]> ActionWriteParameterized
        {
            internal get => actionWrite2;
            set => actionWrite2 = value ?? Write;
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write
        /// </summary>
        public static Action<string> ActionWriteLineString
        {
            internal get => actionWriteLine1;
            set => actionWriteLine1 = value ?? WriteLine;
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, object[]> ActionWriteLineParameterized
        {
            internal get => actionWriteLine2;
            set => actionWriteLine2 = value ?? WriteLine;
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write
        /// </summary>
        public static Action<string, TermReaderSettings> ActionWriteStringNonStandalone
        {
            internal get => actionWriteNonStandalone1;
            set => actionWriteNonStandalone1 = value ?? WriteNonStandalone;
        }
        /// <summary>
        /// Writes text to console<br></br><br></br>
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, TermReaderSettings, object[]> ActionWriteParameterizedNonStandalone
        {
            internal get => actionWriteNonStandalone2;
            set => actionWriteNonStandalone2 = value ?? WriteNonStandalone;
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write
        /// </summary>
        public static Action<string, TermReaderSettings> ActionWriteLineStringNonStandalone
        {
            internal get => actionWriteLineNonStandalone1;
            set => actionWriteLineNonStandalone1 = value ?? WriteLineNonStandalone;
        }
        /// <summary>
        /// Writes text to console with line terminator
        /// - The text to write<br></br>
        /// - The arguments to evaluate
        /// </summary>
        public static Action<string, TermReaderSettings, object[]> ActionWriteLineParameterizedNonStandalone
        {
            internal get => actionWriteLineNonStandalone2;
            set => actionWriteLineNonStandalone2 = value ?? WriteLineNonStandalone;
        }
        /// <summary>
        /// Writes new line to console
        /// </summary>
        public static Action ActionWriteLine
        {
            internal get => actionWriteLine;
            set => actionWriteLine = value ?? WriteLine;
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
            get => ConsoleCursor.CursorVisible;
            set => ConsoleCursor.CursorVisible = value;
        }

        private static bool TreatCtrlCAsInput
        {
            get
            {
                if (IsDumb)
                    return false;
                return Console.TreatControlCAsInput;
            }
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

        private static void BeepSeq() =>
            Write('\a');

        private static void Clear() =>
            Console.Clear();

        private static void ClearLoadBack()
        {
            Write(ColorTools.CurrentBackgroundColor.VTSequenceBackground);
            Clear();
        }

        private static ConsoleKeyInfo ReadKey(bool intercept = false) =>
            Console.ReadKey(intercept);

        private static void WriteNonStandalone(string text, TermReaderSettings settings)
        {
            int top = settings.state.inputPromptTop;
            int topBegin = settings.state.inputPromptTopBegin;
            var wrapped = TextTools.GetWrappedSentences(text, settings.state.LongestSentenceLengthFromLeftForGeneralLine + 1, settings.state.InputPromptLeft - settings.state.LeftMargin);
            for (int i = 0; i < wrapped.Length; i++)
            {
                int wrapTop = top + i;
                string textWrapped = wrapped[i];
                TextWriterRaw.WritePlain(textWrapped, false);
                if (i + 1 < wrapped.Length)
                {
                    WriteLine();
                    CursorLeft = settings.LeftMargin;
                }
                if (wrapTop >= BufferHeight && !settings.state.writingPrompt && top > 0)
                {
                    top--;
                    topBegin--;
                    settings.state.currentCursorPosTop--;
                    CursorLeft = settings.LeftMargin;
                }
            }
            settings.state.inputPromptTop = top;
            settings.state.inputPromptTopBegin = topBegin;
        }

        private static void WriteNonStandalone(string text, TermReaderSettings settings, params object[] args)
        {
            string formatted = string.Format(text, args);
            WriteNonStandalone(formatted, settings);
        }

        private static void WriteLineNonStandalone(string text, TermReaderSettings settings)
        {
            WriteNonStandalone(text, settings);
            WriteLine();
        }

        private static void WriteLineNonStandalone(string text, TermReaderSettings settings, params object[] args)
        {
            WriteNonStandalone(text, settings, args);
            WriteLine();
        }

        private static void Write(char value) =>
            Console.Write(value);

        private static void Write(string text) =>
            Console.Write(text);

        private static void Write(string text, params object[] args)
        {
            string formatted = string.Format(text, args);
            Write(formatted);
        }

        private static void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        private static void WriteLine(string text, params object[] args)
        {
            Write(text, args);
            WriteLine();
        }

        private static void WriteLine() =>
            Console.WriteLine();
    }
}
