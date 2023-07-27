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
using TermRead.Tools;

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
        /// <param name="oneLineWrap">Whether to warp overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, string defaultValue, bool password = false, bool oneLineWrap = false, bool interruptible = false)
        {
            lock (readLock)
            {
                var struckKey = new ConsoleKeyInfo();
                var readState = new TermReaderState();

                // Print the input
                ConsoleWrapperTools.ActionSetCursorLeft(ConsoleWrapperTools.ActionCursorLeft() + TermReaderSettings.LeftMargin);
                ConsoleWrapperTools.ActionWriteString(inputPrompt);

                // Save current state of input
                readState.inputPromptLeft = ConsoleWrapperTools.ActionCursorLeft();
                readState.inputPromptTop = ConsoleWrapperTools.ActionCursorTop();
                readState.inputPromptText = inputPrompt;
                readState.passwordMode = password;
                readState.oneLineWrap = oneLineWrap;
                ConsoleWrapperTools.ActionTreatCtrlCAsInput(TermReaderSettings.TreatCtrlCAsInput);

                // Get input
                (int, int) cachedPos = (ConsoleWrapperTools.ActionCursorLeft(), ConsoleWrapperTools.ActionCursorTop());
                while (!BindingsReader.IsTerminate(struckKey))
                {
                    // Get a key
                    TermReaderTools.isWaitingForInput = true;
                    struckKey = TermReaderTools.GetInput(interruptible);
                    ConsoleWrapperTools.ActionCursorVisible(false);
                    TermReaderTools.isWaitingForInput = false;

                    // Install necessary values
                    readState.currentCursorPosLeft = cachedPos.Item1;
                    readState.currentCursorPosTop = cachedPos.Item2;
                    readState.pressedKey = struckKey;

                    // Handle it
                    BindingsReader.Execute(readState);

                    // Cursor is visible, but fix cursor on Linux
                    cachedPos = (readState.currentCursorPosLeft, readState.currentCursorPosTop);
                    ConsoleWrapperTools.ActionCursorVisible(true);
                }

                // Seek to the end of the text and write a new line
                if (!readState.OneLineWrap)
                {
                    PositioningTools.SeekTo(readState.CurrentText.Length, ref readState);
                    ConsoleWrapperTools.ActionSetCursorPosition(readState.CurrentCursorPosLeft, readState.CurrentCursorPosTop);
                }
                ConsoleWrapperTools.ActionWriteLine();

                // Return the input after adding it to history
                string input = readState.CurrentText.Length == 0 ?
                               defaultValue :
                               readState.CurrentText.ToString();
                if (!password && TermReaderSettings.HistoryEnabled)
                {
                    // We don't want passwords in the history. Also, check to see if the history entry can be added or not based
                    // on the following conditions:
                    //
                    // - If the input is not empty
                    // - If the last input is not the same as the currently supplied input
                    // - Can also be added if the history is zero
                    if (!string.IsNullOrWhiteSpace(input) &&
                        ((TermReaderState.history.Count > 0 && TermReaderState.history[TermReaderState.history.Count - 1] != input) || TermReaderState.history.Count == 0))
                        TermReaderState.history.Add(input);
                }

                // Reset the auto complete position and suggestions
                TermReaderState.currentSuggestionsPos = 0;
                TermReaderState.currentHistoryPos = TermReaderState.history.Count;
                TermReaderSettings.Suggestions = ((_, _, _) => Array.Empty<string>());
                return input;
            }
        }
    }
}
