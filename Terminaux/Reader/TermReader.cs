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
using Terminaux.Reader.Bindings;
using Terminaux.Tools;

namespace Terminaux.Reader
{
    /// <summary>
    /// Terminal input reader module
    /// </summary>
    public static class Terminauxer
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
                var readState = new TerminauxerState();

                // Print the input
                ConsoleWrapperTools.ActionSetCursorLeft(ConsoleWrapperTools.ActionCursorLeft() + TerminauxerSettings.LeftMargin);
                ConsoleWrapperTools.ActionWriteString(inputPrompt);

                // Save current state of input
                readState.inputPromptLeft = ConsoleWrapperTools.ActionCursorLeft();
                readState.inputPromptTop = ConsoleWrapperTools.ActionCursorTop();
                readState.inputPromptText = inputPrompt;
                readState.passwordMode = password;
                readState.oneLineWrap = oneLineWrap;
                ConsoleWrapperTools.ActionTreatCtrlCAsInput(TerminauxerSettings.TreatCtrlCAsInput);

                // Get input
                (int, int) cachedPos = (ConsoleWrapperTools.ActionCursorLeft(), ConsoleWrapperTools.ActionCursorTop());
                while (!BindingsReader.IsTerminate(struckKey))
                {
                    // Get a key
                    TerminauxerTools.isWaitingForInput = true;
                    struckKey = TerminauxerTools.GetInput(interruptible);
                    ConsoleWrapperTools.ActionCursorVisible(false);
                    TerminauxerTools.isWaitingForInput = false;

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
                if (!password && TerminauxerSettings.HistoryEnabled)
                {
                    // We don't want passwords in the history. Also, check to see if the history entry can be added or not based
                    // on the following conditions:
                    //
                    // - If the input is not empty
                    // - If the last input is not the same as the currently supplied input
                    // - Can also be added if the history is zero
                    if (!string.IsNullOrWhiteSpace(input) &&
                        ((TerminauxerState.history.Count > 0 && TerminauxerState.history[TerminauxerState.history.Count - 1] != input) || TerminauxerState.history.Count == 0))
                        TerminauxerState.history.Add(input);
                }

                // Reset the auto complete position and suggestions
                TerminauxerState.currentSuggestionsPos = 0;
                TerminauxerState.currentHistoryPos = TerminauxerState.history.Count;
                TerminauxerSettings.Suggestions = ((_, _, _) => Array.Empty<string>());
                return input;
            }
        }
    }
}
