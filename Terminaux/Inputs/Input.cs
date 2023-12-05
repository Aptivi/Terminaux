//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using Terminaux.Base;
using Terminaux.Reader;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {
        internal static TermReaderSettings globalSettings = new();
        internal static string currentMask = "*";

        /// <summary>
        /// Current mask character
        /// </summary>
        public static string CurrentMask =>
            currentMask;

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        public static string ReadLine() =>
            ReadLine("", "", globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLine(string InputText) =>
            ReadLine(InputText, "", globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLine(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, globalSettings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings) =>
            ReadLine(InputText, DefaultValue, false, settings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        public static string ReadLineWrapped() =>
            ReadLineWrapped("", "", globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        public static string ReadLineWrapped(string InputText) =>
            ReadLineWrapped(InputText, "", globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue) =>
            ReadLineWrapped(InputText, DefaultValue, globalSettings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings) =>
            ReadLine(InputText, DefaultValue, true, settings);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        public static string ReadLine(string InputText, string DefaultValue, bool OneLineWrap = false) =>
            ReadLine(InputText, DefaultValue, OneLineWrap, globalSettings);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLine(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings settings = null) =>
            TermReader.Read(InputText, DefaultValue, settings, false, OneLineWrap);

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
        public static string ReadLineNoInput(TermReaderSettings settings)
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0], settings);
            else
                return ReadLineNoInput(Convert.ToChar("\0"), settings);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar) =>
            ReadLineNoInput(MaskChar, new TermReaderSettings());

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        public static string ReadLineNoInput(char MaskChar, TermReaderSettings settings)
        {
            settings.PasswordMaskChar = MaskChar;
            string pass = TermReader.ReadPassword(settings);
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            if (!ConsoleWrapper.KeyAvailable)
                throw new TerminauxContinuableException("User didn't provide any input in a timely fashion.");
            return ConsoleWrapper.ReadKey(Intercept);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress()
        {
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
            return ConsoleWrapper.ReadKey(true);
        }

    }
}
