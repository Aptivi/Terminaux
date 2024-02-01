﻿//
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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Reader.Bindings;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Reader
{
    /// <summary>
    /// Terminal input reader module
    /// </summary>
    public static class TermReader
    {
        internal static (int, int) cachedPos = default;
        internal static readonly List<TermReaderState> states = [];
        private static readonly object readLock = new();

        /// <summary>
        /// Reads the input
        /// </summary>
        public static string Read() => 
            Read("", "");

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string Read(TermReaderSettings settings) => 
            Read("", "", settings);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        public static string Read(string inputPrompt) => 
            Read(inputPrompt, "");

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        public static string Read(string inputPrompt, TermReaderSettings settings) => 
            Read(inputPrompt, "", settings);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(bool interruptible = true) =>
            Read("", "", true, false, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(TermReaderSettings settings, bool interruptible = true) =>
            Read("", "", settings, true, false, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(string inputPrompt, bool interruptible = true) =>
            Read(inputPrompt, "", true, false, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(string inputPrompt, TermReaderSettings settings, bool interruptible = true) =>
            Read(inputPrompt, "", settings, true, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to warp overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, string defaultValue, bool password = false, bool oneLineWrap = false, bool interruptible = true) =>
            Read(inputPrompt, defaultValue, Input.GlobalReaderSettings, password, oneLineWrap, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="settings">Settings containing reader-related settings</param>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to warp overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, string defaultValue, TermReaderSettings settings, bool password = false, bool oneLineWrap = false, bool interruptible = true)
        {
            lock (readLock)
            {
                // Wait until the previous input is complete
                SpinWait.SpinUntil(() => !TermReaderTools.Busy);

                // Initialize everything
                string input = defaultValue;
                var struckKey = new ConsoleKeyInfo();
                var readState = new TermReaderState
                {
                    settings = settings
                };
                states.Add(readState);

                // Save some variable states
                bool ctrlCAsInput = ConsoleWrapper.TreatCtrlCAsInput;
                bool initialVisible = ConsoleWrapper.CursorVisible;

                // Handle all possible errors
                try
                {
                    // Print the input
                    ConsoleWrapper.CursorLeft += settings.LeftMargin;
                    readState.settings.state = readState;
                    readState.inputPromptText = inputPrompt;
                    readState.inputPromptLeftBegin = ConsoleWrapper.CursorLeft;
                    readState.inputPromptTopBegin = ConsoleWrapper.CursorTop - (ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1 ? readState.InputPromptHeight - 1 : 0);
                    readState.writingPrompt = true;
                    TextWriterColor.WriteForReader(inputPrompt, settings, false);
                    readState.writingPrompt = false;

                    // Save current state of input
                    readState.inputPromptLeft = ConsoleWrapper.CursorLeft;
                    readState.inputPromptTop = ConsoleWrapper.CursorTop;
                    readState.passwordMode = password;
                    readState.oneLineWrap = oneLineWrap;
                    ConsoleWrapper.TreatCtrlCAsInput = settings.TreatCtrlCAsInput;

                    // Get input
                    cachedPos = (ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
                    ConsoleWrapper.CursorVisible = true;
                    while (!BindingsReader.IsTerminate(struckKey))
                    {
                        // Get a key
                        TermReaderTools.isWaitingForInput = true;
                        struckKey = TermReaderTools.GetInput(interruptible);
                        ConsoleWrapper.CursorVisible = false;
                        TermReaderTools.isWaitingForInput = false;

                        // Install necessary values
                        readState.currentCursorPosLeft = cachedPos.Item1;
                        readState.currentCursorPosTop = cachedPos.Item2;
                        readState.pressedKey = struckKey;

                        // Handle it
                        BindingsReader.Execute(readState);
                        bool differed =
                            cachedPos.Item1 != readState.currentCursorPosLeft ||
                            cachedPos.Item2 != readState.currentCursorPosTop;
                        if (differed)
                            PositioningTools.Commit(readState);

                        // Cursor is visible, but fix cursor on Linux
                        cachedPos = (readState.currentCursorPosLeft, readState.currentCursorPosTop);
                        ConsoleWrapper.CursorVisible = true;
                    }

                    // Seek to the end of the text and write a new line
                    if (!readState.OneLineWrap)
                    {
                        PositioningTools.SeekTo(readState.CurrentText.Length, ref readState);
                        PositioningTools.Commit(readState);
                    }
                    TextWriterColor.Write();

                    // Return the input after adding it to history
                    input =
                        readState.CurrentText.Length == 0 ?
                        defaultValue :
                        readState.CurrentText.ToString();
                    if (!password && settings.HistoryEnabled)
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
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteColor($"Input reader has failed: {ex.Message}", ConsoleColors.Red);
                    TextWriterColor.WriteColor(ex.StackTrace, ConsoleColors.Red);
                }
                finally
                {
                    // Reset the auto complete position and suggestions
                    TermReaderState.currentSuggestionsPos = 0;
                    TermReaderState.currentHistoryPos = TermReaderState.history.Count;
                    settings.Suggestions = (_, _, _) => Array.Empty<string>();

                    // Reset the CTRL + C state and the cursor visibility state
                    ConsoleWrapper.TreatCtrlCAsInput = ctrlCAsInput;
                    ConsoleWrapper.CursorVisible = initialVisible;
                }
                states.Remove(readState);
                return input;
            }
        }
    }
}
