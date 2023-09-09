
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

namespace Terminaux.Reader.Inputs
{
    /// <summary>
    /// Console input module
    /// </summary>
    public static class Input
    {
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
            ReadLine("", "");

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLine(TermReaderSettings settings) =>
            ReadLine("", "", settings);

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
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLine(string InputText, TermReaderSettings settings) =>
            ReadLine(InputText, "", settings);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLine(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, new TermReaderSettings(), false);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings) =>
            ReadLine(InputText, DefaultValue, settings, false);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        public static string ReadLineWrapped() =>
            ReadLineWrapped("", "");

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLineWrapped(TermReaderSettings settings) =>
            ReadLineWrapped("", "", settings);

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
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLineWrapped(string InputText, TermReaderSettings settings) =>
            ReadLineWrapped(InputText, "", settings);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue) =>
            ReadLine(InputText, DefaultValue, new TermReaderSettings(), true);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings) =>
            ReadLine(InputText, DefaultValue, settings, true);

        /// <summary>
        /// Reads the line from the console.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings, bool OneLineWrap = false) =>
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
        /// <param name="MaskChar">Specifies the password mask character</param>
        public static string ReadLineNoInput(char MaskChar)
        {
            string pass = TermReader.ReadPassword(new TermReaderSettings() { PasswordMaskChar = MaskChar });
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            SpinWait.SpinUntil(() => ConsoleWrappers.ActionKeyAvailable(), Timeout);
            if (!ConsoleWrappers.ActionKeyAvailable())
                throw new TerminauxContinuableException("User didn't provide any input in a timely fashion.");
            return ConsoleWrappers.ActionReadKey(Intercept);
        }

        /// <summary>
        /// Detects the keypress
        /// </summary>
        public static ConsoleKeyInfo DetectKeypress()
        {
            SpinWait.SpinUntil(() => ConsoleWrappers.ActionKeyAvailable());
            return ConsoleWrappers.ActionReadKey(true);
        }

    }
}
