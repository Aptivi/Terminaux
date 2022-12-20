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
using TermRead.Bindings;
using TermRead.Wrappers;

namespace TermRead.Reader
{
    /// <summary>
    /// Terminal input reader module
    /// </summary>
    public static class TermReader
    {
        private static readonly object readLock = new();

        /// <summary>
        /// Reads the input
        /// </summary>
        public static string Read() => 
            Read("", "");

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        public static string Read(string inputPrompt) => 
            Read(inputPrompt, "");

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        public static string ReadPassword() =>
            Read("", "", true);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        public static string ReadPassword(string inputPrompt) =>
            Read(inputPrompt, "", true);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        public static string Read(string inputPrompt, string defaultValue, bool password = false)
        {
            lock (readLock)
            {
                var struckKey = new ConsoleKeyInfo();
                var readState = new TermReaderState();

                // Print the input
                ConsoleWrapper.Write(inputPrompt);

                // Save current state of input
                readState.inputPromptLeft = ConsoleWrapper.CursorLeft;
                readState.inputPromptTop = ConsoleWrapper.CursorTop;
                readState.inputPromptText = inputPrompt;
                readState.passwordMode = password;

                // Get input
                while (struckKey.Key != ConsoleKey.Enter)
                {
                    // Get a key
                    struckKey = ConsoleWrapper.ReadKey(true);

                    // Install necessary values
                    readState.currentCursorPosLeft = ConsoleWrapper.CursorLeft;
                    readState.currentCursorPosTop = ConsoleWrapper.CursorTop;
                    readState.pressedKey = struckKey;

                    // Handle it
                    BindingsReader.Execute(readState);
                }

                // Return the input
                return readState.CurrentText.Length == 0 ?
                       defaultValue :
                       readState.CurrentText.ToString();
            }
        }
    }
}
