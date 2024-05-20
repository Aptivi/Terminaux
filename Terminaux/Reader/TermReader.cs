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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Pointer;
using Terminaux.Reader.Bindings;
using Terminaux.Reader.History;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Reader
{
    /// <summary>
    /// Terminal input reader module
    /// </summary>
    public static class TermReader
    {
        internal static TermReaderSettings globalSettings = new();
        internal static TermReaderState? state = null;
        private static readonly object readLock = new();

        /// <summary>
        /// The global reader settings
        /// </summary>
        public static TermReaderSettings GlobalReaderSettings =>
            globalSettings;

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static (ConsoleKeyInfo result, bool provided) ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            TermReaderTools.isWaitingForInput = true;
            bool result = SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, Timeout);
            TermReaderTools.isWaitingForInput = false;
            return (!result ? default : ConsoleWrapper.ReadKey(Intercept), result);
        }

        /// <summary>
        /// Reads the next key from the console input stream
        /// </summary>
        public static ConsoleKeyInfo ReadKey() =>
            ReadKey(true);

        /// <summary>
        /// Reads the next key from the console input stream
        /// </summary>
        /// <param name="intercept">Whether to intercept the key pressed or to just show the actual key to the console</param>
        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            TermReaderTools.isWaitingForInput = true;
            SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
            TermReaderTools.isWaitingForInput = false;
            return ConsoleWrapper.ReadKey(intercept);
        }

        /// <summary>
        /// Reads a pointer (blocking)
        /// </summary>
        /// <returns>A <see cref="PointerEventContext"/> instance that describes the last mouse event.</returns>
        public static PointerEventContext ReadPointer()
        {
            PointerEventContext? ctx = null;
            while (ctx is null)
                ctx = PointerListener.ReadPointerNow();
            return ctx;
        }

        /// <summary>
        /// Reads either a pointer or a key (blocking)
        /// </summary>
        public static (PointerEventContext?, ConsoleKeyInfo) ReadPointerOrKey()
        {
            bool looping = true;
            PointerEventContext? ctx = null;
            ConsoleKeyInfo cki = default;
            while (looping)
            {
                SpinWait.SpinUntil(() => PointerListener.InputAvailable);
                if (PointerListener.PointerAvailable)
                {
                    // Mouse input received.
                    ctx = ReadPointer();
                    if (ctx.ButtonPress != PointerButtonPress.Moved)
                        looping = false;
                }
                else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
                {
                    cki = ReadKey();
                    looping = false;
                }
            }
            return (ctx, cki);
        }

        /// <summary>
        /// Invalidates the input
        /// </summary>
        public static void InvalidateInput()
        {
            while (ConsoleWrapper.KeyAvailable)
                ReadKey(true);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(bool interruptible = true) =>
            ReadPassword(GlobalReaderSettings.PasswordMaskChar, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, bool interruptible = true)
        {
            var settings = new TermReaderSettings(GlobalReaderSettings)
            {
                PasswordMaskChar = mask
            };
            return Read("", "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(TermReaderSettings settings, bool interruptible = true) =>
            ReadPassword(settings.PasswordMaskChar, settings, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, TermReaderSettings settings, bool interruptible = true)
        {
            settings.PasswordMaskChar = mask;
            return Read("", "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(string inputPrompt, bool interruptible = true) =>
            ReadPassword(GlobalReaderSettings.PasswordMaskChar, inputPrompt, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, string inputPrompt, bool interruptible = true)
        {
            var settings = new TermReaderSettings(GlobalReaderSettings)
            {
                PasswordMaskChar = mask
            };
            return Read(() => inputPrompt, "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(string inputPrompt, TermReaderSettings settings, bool interruptible = true) =>
            ReadPassword(settings.PasswordMaskChar, inputPrompt, settings, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, string inputPrompt, TermReaderSettings settings, bool interruptible = true)
        {
            settings.PasswordMaskChar = mask;
            return Read(() => inputPrompt, "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(Func<string> inputPrompt, bool interruptible = true) =>
            ReadPassword(GlobalReaderSettings.PasswordMaskChar, inputPrompt, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, Func<string> inputPrompt, bool interruptible = true)
        {
            var settings = new TermReaderSettings(GlobalReaderSettings)
            {
                PasswordMaskChar = mask
            };
            return Read(inputPrompt, "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(Func<string> inputPrompt, TermReaderSettings settings, bool interruptible = true) =>
            ReadPassword(settings.PasswordMaskChar, inputPrompt, settings, interruptible);

        /// <summary>
        /// Reads the input with password character masking
        /// </summary>
        /// <param name="mask">Password mask character</param>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string ReadPassword(char mask, Func<string> inputPrompt, TermReaderSettings settings, bool interruptible = true)
        {
            settings.PasswordMaskChar = mask;
            return Read(inputPrompt, "", settings, true, false, interruptible);
        }

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(bool interruptible = true) => 
            Read("", "", GlobalReaderSettings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(TermReaderSettings settings, bool interruptible = true) => 
            Read("", "", settings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, bool interruptible = true) => 
            Read(inputPrompt, "", GlobalReaderSettings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, TermReaderSettings settings, bool interruptible = true) => 
            Read(inputPrompt, "", settings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The input to be read</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to wrap overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, string defaultValue, bool password = false, bool oneLineWrap = false, bool interruptible = true) =>
            Read(inputPrompt, defaultValue, GlobalReaderSettings, password, oneLineWrap, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="settings">Settings containing reader-related settings</param>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to wrap overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(string inputPrompt, string defaultValue, TermReaderSettings settings, bool password = false, bool oneLineWrap = false, bool interruptible = true) =>
            Read(() => inputPrompt, defaultValue, settings, password, oneLineWrap, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(Func<string> inputPrompt, bool interruptible = true) => 
            Read(inputPrompt, "", GlobalReaderSettings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="settings">Settigns containing reader-related settings</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(Func<string> inputPrompt, TermReaderSettings settings, bool interruptible = true) => 
            Read(inputPrompt, "", settings, false, false, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to wrap overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(Func<string> inputPrompt, string defaultValue, bool password = false, bool oneLineWrap = false, bool interruptible = true) =>
            Read(inputPrompt, defaultValue, GlobalReaderSettings, password, oneLineWrap, interruptible);

        /// <summary>
        /// Reads the input
        /// </summary>
        /// <param name="settings">Settings containing reader-related settings</param>
        /// <param name="inputPrompt">The dynamic input function to prompt the user</param>
        /// <param name="defaultValue">Default value to use if no input is provided</param>
        /// <param name="password">Whether the password mode is enabled</param>
        /// <param name="oneLineWrap">Whether to wrap overflown text as one line</param>
        /// <param name="interruptible">Whether the prompt is interruptible or not</param>
        public static string Read(Func<string> inputPrompt, string defaultValue, TermReaderSettings settings, bool password = false, bool oneLineWrap = false, bool interruptible = true)
        {
            lock (readLock)
            {
                // Wait until the previous input is complete
                TermReaderTools.WaitForInput();

                // Initialize everything
                string input = defaultValue;
                var struckKey = new ConsoleKeyInfo();
                var readState = new TermReaderState
                {
                    settings = settings
                };
                state = readState;

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
                    TextWriterColor.WriteForReader(readState.InputPromptText, settings, false);
                    readState.writingPrompt = false;

                    // Save current state of input
                    readState.inputPromptLeft = ConsoleWrapper.CursorLeft;
                    readState.inputPromptTop = ConsoleWrapper.CursorTop;
                    readState.currentCursorPosLeft = readState.inputPromptLeft;
                    readState.currentCursorPosTop = readState.inputPromptTop;
                    readState.passwordMode = password;
                    readState.oneLineWrap = oneLineWrap;
                    readState.currentHistoryPos = HistoryTools.GetHistoryEntries(readState.settings.HistoryName).Length;
                    ConsoleWrapper.TreatCtrlCAsInput = settings.TreatCtrlCAsInput;

                    // Get input
                    ConsoleWrapper.CursorVisible = true;
                    while (!BindingsTools.IsTerminate(struckKey))
                    {
                        // Write placeholder if needed
                        if (readState.CurrentText.Length == 0 && settings.PlaceholderText.Length != 0)
                        {
                            ConsoleWrapper.SetCursorPosition(readState.inputPromptLeft, readState.inputPromptTop);
                            TextWriterColor.WriteForReaderColor(settings.PlaceholderText, settings, false, new Color(ConsoleColors.Grey));
                            ConsoleWrapper.SetCursorPosition(readState.inputPromptLeft, readState.inputPromptTop);
                        }

                        // Get a key
                        TermReaderTools.isWaitingForInput = true;
                        struckKey = TermReaderTools.GetInput(interruptible);
                        ConsoleWrapper.CursorVisible = false;
                        TermReaderTools.isWaitingForInput = false;

                        // Install necessary values
                        readState.pressedKey = struckKey;

                        // Handle it
                        BindingsTools.Execute(readState);
                        TermReaderTools.Refresh();
                        PositioningTools.Commit(readState);

                        // Write the bell character if invalid
                        if (readState.OperationWasInvalid)
                            ConsoleWrapper.BeepSeq();
                        readState.operationWasInvalid = false;

                        // Cursor is visible
                        ConsoleWrapper.CursorVisible = true;
                    }

                    // Seek to the end of the text and write a new line
                    if (!readState.OneLineWrap)
                    {
                        PositioningTools.SeekTo(readState.CurrentText.Length, ref readState);
                        PositioningTools.Commit(readState);
                    }
                    TextWriterRaw.Write();

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
                        if (!string.IsNullOrWhiteSpace(input))
                            HistoryTools.Append(state.Settings.HistoryName, input);
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteColor($"Input reader has failed: {ex.Message}", ConsoleColors.Red);
                    TextWriterColor.WriteColor(ex.StackTrace, ConsoleColors.Red);
                }
                finally
                {
                    // Reset the suggestions
                    settings.Suggestions = (_, _, _) => [];

                    // Reset the CTRL + C state and the cursor visibility state
                    ConsoleWrapper.TreatCtrlCAsInput = ctrlCAsInput;
                    ConsoleWrapper.CursorVisible = initialVisible;
                }
                state = null;
                return input;
            }
        }

        static TermReader()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
