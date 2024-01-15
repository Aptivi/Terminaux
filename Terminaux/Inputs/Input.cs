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
        /// The global reader settings
        /// </summary>
        public static TermReaderSettings GlobalReaderSettings =>
            globalSettings;

        /// <summary>
        /// Current mask character
        /// </summary>
        public static string CurrentMask =>
            currentMask;

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(bool interruptible = true) =>
            ReadLine("", "", GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(string InputText, bool interruptible = true) =>
            ReadLine(InputText, "", GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(string InputText, string DefaultValue, bool interruptible = true) =>
            ReadLine(InputText, DefaultValue, GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(string InputText, string DefaultValue, TermReaderSettings settings, bool interruptible = true) =>
            ReadLine(InputText, DefaultValue, false, settings, interruptible);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineWrapped(bool interruptible = true) =>
            ReadLineWrapped("", "", GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineWrapped(string InputText, bool interruptible = true) =>
            ReadLineWrapped(InputText, "", GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue, bool interruptible = true) =>
            ReadLineWrapped(InputText, DefaultValue, GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console (wrapped to one line)
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineWrapped(string InputText, string DefaultValue, TermReaderSettings settings, bool interruptible = true) =>
            ReadLine(InputText, DefaultValue, true, settings, interruptible);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(string InputText, string DefaultValue, bool OneLineWrap = false, bool interruptible = true) =>
            ReadLine(InputText, DefaultValue, OneLineWrap, GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the line from the console unsafely. This doesn't wait until the screensaver lock mode is released.
        /// </summary>
        /// <param name="InputText">Input text to write</param>
        /// <param name="DefaultValue">Default value</param>
        /// <param name="OneLineWrap">Whether to wrap the input to one line</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLine(string InputText, string DefaultValue, bool OneLineWrap = false, TermReaderSettings settings = null, bool interruptible = true) =>
            TermReader.Read(InputText, DefaultValue, settings, false, OneLineWrap, interruptible);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineNoInput(bool interruptible = true)
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0], interruptible);
            else
                return ReadLineNoInput(Convert.ToChar("\0"), interruptible);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="settings">Reader settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineNoInput(TermReaderSettings settings, bool interruptible = true)
        {
            if (!string.IsNullOrEmpty(CurrentMask))
                return ReadLineNoInput(CurrentMask[0], settings, interruptible);
            else
                return ReadLineNoInput(Convert.ToChar("\0"), settings, interruptible);
        }

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineNoInput(char MaskChar, bool interruptible = true) =>
            ReadLineNoInput(MaskChar, GlobalReaderSettings, interruptible);

        /// <summary>
        /// Reads the next line of characters from the standard input stream without showing input being written by user.
        /// </summary>
        /// <param name="MaskChar">Specifies the password mask character</param>
        /// <param name="settings">Reader settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadLineNoInput(char MaskChar, TermReaderSettings settings, bool interruptible = true)
        {
            settings.PasswordMaskChar = MaskChar;
            string pass = TermReader.ReadPassword(settings, interruptible);
            return pass;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static ConsoleKeyInfo ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            TermReaderTools.isWaitingForInput = true;
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            TermReaderTools.isWaitingForInput = false;
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
