
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
using Terminaux.Base;
using Terminaux.Tools;

namespace Terminaux.Reader.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {
        internal static string currentMask = "*";
        private static bool isWrapperInitialized;

        /// <summary>
        /// Current mask character
        /// </summary>
        public static string CurrentMask =>
            currentMask;

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        public static string ReadLine() =>
            ReadLine("", "");

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLine(string InputText) =>
            ReadLine(InputText, "");

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLine(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, false);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        public static string ReadLineWrapped() =>
            ReadLineWrapped("", "");

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLineWrapped(string InputText) =>
            ReadLineWrapped(InputText, "");

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, true);

        /// <summary>
        /// Reads the line from the console.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        public static string ReadLine(string InputText, string DefaultValue, bool OneLineWrap = false) =>
            TermReader.Read(InputText, DefaultValue, false, OneLineWrap);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        public static string ReadLineNoInput()
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0]);
            else
                return ReadLineNoInput(Convert.ToChar("\0"));
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar)
        {
            TermReaderSettings.PasswordMaskChar = MaskChar;
            string pass = TermReader.ReadPassword();
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            SpinWait.SpinUntil(() => Console.KeyAvailable, Timeout);
            if (!Console.KeyAvailable)
                throw new Exception("User didn't provide any input in a timely fashion.");
            return Console.ReadKey(Intercept);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress()
        {
            SpinWait.SpinUntil(() => Console.KeyAvailable);
            return Console.ReadKey(true);
        }

        internal static void InitializeInputWrappers()
        {
            if (isWrapperInitialized)
                return;

            // Initialize console wrappers for TermRead
            ConsoleTools.ActionBeep = Console.Beep;
            ConsoleTools.ActionBufferHeight = () => Console.BufferHeight;
            ConsoleTools.ActionCursorLeft = () => Console.CursorLeft;
            ConsoleTools.ActionCursorTop = () => Console.CursorTop;
            ConsoleTools.ActionCursorVisible = (value) => Console.CursorVisible = value;
            ConsoleTools.ActionIsDumb = () => ConsoleChecker.IsDumb;
            ConsoleTools.ActionKeyAvailable = () => Console.KeyAvailable;
            ConsoleTools.ActionReadKey = Console.ReadKey;
            ConsoleTools.ActionSetCursorPosition = Console.SetCursorPosition;
            ConsoleTools.ActionWindowHeight = () => Console.WindowHeight;
            ConsoleTools.ActionWindowWidth = () => Console.WindowWidth;
            ConsoleTools.ActionWriteChar = Console.Write;
            ConsoleTools.ActionWriteLine = Console.WriteLine;
            ConsoleTools.ActionWriteLineParameterized = Console.WriteLine;
            ConsoleTools.ActionWriteLineString = Console.WriteLine;
            ConsoleTools.ActionWriteParameterized = Console.Write;
            ConsoleTools.ActionWriteString = Console.Write;
            isWrapperInitialized = true;
        }

    }
}
