//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using SpecProbe.Software.Platform;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Extensions.Native;
using Terminaux.Inputs.Pointer;
using Terminaux.Reader;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;
using static Terminaux.Base.Extensions.Native.NativeMethods;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Input management tools
    /// </summary>
    public static class Input
    {
        private const uint ENABLE_MOUSE_INPUT = 0x0010;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;

        private static PointerEventContext? context = null;
        private static PointerButton draggingButton = PointerButton.None;
        private static bool enableMovementEvents;
        private static int clickTier = 1;
        private static PointerEventContext? tieredContext = null;
        private static bool enableMouse;
        private static byte[]? cachedMouseEvent = null;
        private static readonly Stopwatch inputTimeout = new();
        private static readonly IntPtr stdHandle = PlatformHelper.IsOnWindows() ? NativeMethods.GetStdHandle(-10) : IntPtr.Zero;

        /// <summary>
        /// Checks to see whether any pending keyboard events are here
        /// </summary>
        public static bool KeyboardInputAvailable =>
            ConsoleWrapper.KeyAvailable;

        /// <summary>
        /// Checks to see whether any pending mouse events are here
        /// </summary>
        public static bool MouseInputAvailable =>
            IsMouseAvailable();

        /// <summary>
        /// Checks to see whether the pointer is active or not
        /// </summary>
        public static bool PointerActive
        {
            get
            {
                bool active = inputTimeout.ElapsedMilliseconds <= DoubleClickTimeout.TotalMilliseconds && inputTimeout.IsRunning;
                if (!active)
                    inputTimeout.Reset();
                return active;
            }
        }

        /// <summary>
        /// Checks to see whether any pending mouse or keyboard events are here
        /// </summary>
        public static bool InputAvailable =>
            MouseInputAvailable || KeyboardInputAvailable;

        /// <summary>
        /// Specifies the time in milliseconds whether the double click times out
        /// </summary>
        public static TimeSpan DoubleClickTimeout { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Whether to enable mouse support.
        /// </summary>
        public static bool EnableMouse
        {
            get => enableMouse;
            set
            {
                enableMouse = value;
                if (!enableMouse)
                    DisableMouseSupport();
                else
                    EnableMouseSupport();
            }
        }

        /// <summary>
        /// Whether to invert the Y axis for scrolling or not.
        /// </summary>
        /// <remarks>
        /// If this option is enabled, scrolling up emits the scroll down event, and scrolling down emits the scroll up event.
        /// Otherwise, scrolling up emits the scroll up event, and scrolling down emits the scroll down event.
        /// </remarks>
        public static bool InvertScrollYAxis { get; set; }

        /// <summary>
        /// Whether to invert the left and the right mouse buttons for left-handed people or not.
        /// </summary>
        /// <remarks>
        /// This is suitable for left-handed people (i.e. you use your mouse in your left hand). If this option
        /// is enabled, clicking on the left button of your mouse emits the right click event, and clicking on the
        /// right button of your mouse emits the left click event. Otherwise, clicking on the left button of your
        /// mouse emits the left click event, and clicking on the right button of your mouse emits the right click
        /// event.
        /// </remarks>
        public static bool SwapLeftRightButtons { get; set; }

        /// <summary>
        /// Checks to see whether the movement events are enabled or not
        /// </summary>
        public static bool EnableMovementEvents
        {
            get => enableMovementEvents;
            set
            {
                enableMovementEvents = value;
                TextWriterRaw.WriteRaw(enableMovementEvents ? $"{VtSequenceBasicChars.EscapeChar}[?1003h" : $"{VtSequenceBasicChars.EscapeChar}[?1003l");
            }
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
                SpinWait.SpinUntil(() => InputAvailable);
                if (MouseInputAvailable)
                {
                    // Mouse input received.
                    ctx = ReadPointer();
                    looping = false;
                }
                else if (KeyboardInputAvailable)
                {
                    cki = ReadKey();
                    looping = false;
                }
            }
            return (ctx, cki);
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
            SpinWait.SpinUntil(() => KeyboardInputAvailable);
            TermReaderTools.isWaitingForInput = false;
            return ConsoleWrapper.ReadKey(intercept);
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Intercept">Whether to intercept an input</param>
        /// <param name="Timeout">Timeout</param>
        public static (ConsoleKeyInfo result, bool provided) ReadKeyTimeout(bool Intercept, TimeSpan Timeout)
        {
            TermReaderTools.isWaitingForInput = true;
            bool result = SpinWait.SpinUntil(() => KeyboardInputAvailable, Timeout);
            TermReaderTools.isWaitingForInput = false;
            return (!result ? default : ConsoleWrapper.ReadKey(Intercept), result);
        }

        // Developers: From this point on, you must be very careful in what you're doing here, because the
        // Linux mouse input detection logic reads from the stdin stream that the appropriate DEC VT Locator
        // sequence prints out when every mouse movement is detected. The below code can be tweaked, but any
        // mistake and the pointer detection logic is broken, so be extra careful when adding, modifying, or
        // tweaking the code below to avoid bugs.
        #region Sensitive code points
        // HACK: We've managed to eliminate unwanted "feedback" on mouse events, but, now, we're being blocked when reading keyboard!
        /// <summary>
        /// Reads a pointer (blocking)
        /// </summary>
        /// <returns>A <see cref="PointerEventContext"/> instance that describes the last mouse event.</returns>
        public static PointerEventContext? ReadPointer()
        {
            // Check for mouse state
            PointerEventContext? ctx = null;
            if (!EnableMouse)
                return ctx;
            if (PlatformHelper.IsOnWindows())
            {
                // Set the appropriate modes
                bool isMouse = false;
                while (!isMouse)
                {
                    uint numRead = 0;
                    INPUT_RECORD[] record = [new INPUT_RECORD()];
                    ReadConsoleInput(stdHandle, record, 1, ref numRead);

                    // Now, filter all events except the mouse ones
                    switch (record[0].EventType)
                    {
                        case INPUT_RECORD.MOUSE_EVENT:
                            // Get the coordinates and event arguments
                            isMouse = true;
                            var @event = record[0].MouseEvent;
                            var coord = @event.dwMousePosition;
                            ConsoleLogger.Debug($"Coord: {coord.X}, {coord.Y}, {@event.dwButtonState}, {@event.dwControlKeyState}, {@event.dwEventFlags}");

                            // Now, translate them to something Terminaux understands
                            (PointerButton button, PointerButtonPress press, PointerModifiers mods) = ProcessPointerEventWin(@event);
                            if (!EnableMovementEvents && press == PointerButtonPress.Moved)
                                break;
                            ctx = GenerateContext(coord.X, coord.Y, button, press, mods);
                            context = ctx;
                            break;
                    }
                }
                return ctx;
            }
            else
            {
                NativeMethods.RawSet(true);
                try
                {
                    // Get cached buffer
                    byte[] chars = cachedMouseEvent ?? new byte[6];
                    if (cachedMouseEvent is null && ReadMouseSequence(ref chars) != 6)
                        return null;
                    cachedMouseEvent = null;

                    // Now, read the button, X, and Y positions
                    byte button = chars[3];
                    byte x = (byte)(chars[4] - 32);
                    byte y = (byte)(chars[5] - 32);
                    x -= 1;
                    y -= 1;

                    // Get the button states and change them as necessary
                    PosixButtonState state = (PosixButtonState)(button & 0b11);
                    PosixButtonModifierState modState = (PosixButtonModifierState)(button & 0b11100);
                    if (button >= 64 && button < 96)
                        state = PosixButtonState.Movement;
                    if (button >= 96 && button % 2 == 0)
                        state = PosixButtonState.WheelUp;
                    else if (button >= 97)
                        state = PosixButtonState.WheelDown;
                    ConsoleLogger.Debug($"[{button}: {state} {modState}] X={x} Y={y}");

                    // Now, translate them to something Terminaux understands
                    (PointerButton buttonPtr, PointerButtonPress press, PointerModifiers mods) = ProcessPointerEventPosix(state, modState);
                    if (!EnableMovementEvents && press == PointerButtonPress.Moved)
                        return null;
                    ctx = GenerateContext(x, y, buttonPtr, press, mods);
                    context = ctx;
                    return ctx;
                }
                finally
                {
                    NativeMethods.RawSet(false);
                }
            }
        }

        /// <summary>
        /// Invalidates the input
        /// </summary>
        public static void InvalidateInput()
        {
            while (ConsoleWrapper.KeyAvailable)
                ReadKey(true);
        }

        #region Private functions
        private static bool IsMouseAvailable()
        {
            if (!EnableMouse)
                return false;
            if (PlatformHelper.IsOnWindows())
            {
                // Make a record and read the input for mouse event
                uint numRead = 0;
                INPUT_RECORD[] record = [new INPUT_RECORD()];
                if (!PeekConsoleInput(stdHandle, record, 1, ref numRead))
                    return false;

                // Now, filter all events except the mouse ones
                return record[0].EventType switch
                {
                    INPUT_RECORD.MOUSE_EVENT => true,
                    _ => false,
                };
            }
            else
            {
                NativeMethods.RawSet(true);
                try
                {
                    if (cachedMouseEvent != null)
                        return true;

                    // Fill the chars array
                    byte[] chars = [];
                    int readBytes = ReadMouseSequence(ref chars);
                    if (readBytes != 6)
                        return false;
                    cachedMouseEvent = chars;
                    return true;
                }
                finally
                {
                    NativeMethods.RawSet(false);
                }
            }
        }

        private static void ProcessDragging(ref PointerButtonPress press, ref PointerButton button, out bool dragging)
        {
            bool resultDragging = false;
            if (EnableMovementEvents)
            {
                if (press == PointerButtonPress.Clicked)
                    draggingButton = button;
                else if (press == PointerButtonPress.Moved && draggingButton != PointerButton.None)
                {
                    button = draggingButton;
                    resultDragging = true;
                }
                else if (press == PointerButtonPress.Released)
                {
                    button = draggingButton;
                    draggingButton = PointerButton.None;
                }
            }
            else
            {
                if (press == PointerButtonPress.Clicked)
                    draggingButton = button;
                else if (press == PointerButtonPress.Released)
                {
                    button = draggingButton;
                    draggingButton = PointerButton.None;
                }
            }
            dragging = resultDragging;
        }

        private static (PointerButton button, PointerButtonPress press, PointerModifiers mods) ProcessPointerEventWin(MOUSE_EVENT_RECORD eventRecord)
        {
            // Determine the button
            PointerButton button =
                eventRecord.dwButtonState == ButtonState.Left ? (SwapLeftRightButtons ? PointerButton.Right : PointerButton.Left) :
                eventRecord.dwButtonState == ButtonState.Middle ? PointerButton.Middle :
                eventRecord.dwButtonState == ButtonState.Right ? (SwapLeftRightButtons ? PointerButton.Left : PointerButton.Right) :
                PointerButton.None;
            button = (eventRecord.dwEventFlags == EventFlags.WheelScrolled || eventRecord.dwEventFlags == EventFlags.HorizontalWheelScrolled) ?
                ((int)eventRecord.dwButtonState >> 16 < 0 ?
                    (InvertScrollYAxis ? PointerButton.WheelUp : PointerButton.WheelDown) :
                    (InvertScrollYAxis ? PointerButton.WheelDown : PointerButton.WheelUp)) :
                button;

            // Determine the button press
            PointerButtonPress press =
                eventRecord.dwButtonState != ButtonState.None && eventRecord.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Clicked :
                eventRecord.dwButtonState != ButtonState.None && eventRecord.dwEventFlags == EventFlags.DoubleClicked ? PointerButtonPress.Clicked :
                eventRecord.dwButtonState == ButtonState.None && eventRecord.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Released :
                button == PointerButton.WheelDown || button == PointerButton.WheelUp ? PointerButtonPress.Scrolled :
                PointerButtonPress.Moved;

            // Determine the modifiers
            PointerModifiers mods =
                (eventRecord.dwControlKeyState & ControlKeyState.RightAltPressed) != 0 ? PointerModifiers.Alt :
                (eventRecord.dwControlKeyState & ControlKeyState.LeftAltPressed) != 0 ? PointerModifiers.Alt :
                PointerModifiers.None;
            mods |=
                (eventRecord.dwControlKeyState & ControlKeyState.RightCtrlPressed) != 0 ? PointerModifiers.Ctrl :
                (eventRecord.dwControlKeyState & ControlKeyState.LeftCtrlPressed) != 0 ? PointerModifiers.Ctrl :
                PointerModifiers.None;
            mods |=
                (eventRecord.dwControlKeyState & ControlKeyState.ShiftPressed) != 0 ? PointerModifiers.Shift :
                PointerModifiers.None;

            // Return the results
            return (button, press, mods);
        }

        private static (PointerButton button, PointerButtonPress press, PointerModifiers mods) ProcessPointerEventPosix(PosixButtonState state, PosixButtonModifierState modState)
        {
            // Determine the button press
            PointerButtonPress press =
                state == PosixButtonState.Left || state == PosixButtonState.Middle || state == PosixButtonState.Right ? PointerButtonPress.Clicked :
                state == PosixButtonState.Released ? PointerButtonPress.Released :
                state == PosixButtonState.WheelDown || state == PosixButtonState.WheelUp ? PointerButtonPress.Scrolled :
                PointerButtonPress.Moved;

            // Determine the button
            PointerButton button =
                state == PosixButtonState.Left ? (SwapLeftRightButtons ? PointerButton.Right : PointerButton.Left) :
                state == PosixButtonState.Middle ? PointerButton.Middle :
                state == PosixButtonState.Right ? (SwapLeftRightButtons ? PointerButton.Left : PointerButton.Right) :
                state == PosixButtonState.WheelDown ? (InvertScrollYAxis ? PointerButton.WheelUp : PointerButton.WheelDown) :
                state == PosixButtonState.WheelUp ? (InvertScrollYAxis ? PointerButton.WheelDown : PointerButton.WheelUp) :
                PointerButton.None;

            // Determine the modifiers
            PointerModifiers mods =
                (modState & PosixButtonModifierState.Alt) != 0 ? PointerModifiers.Alt :
                PointerModifiers.None;
            mods |=
                (modState & PosixButtonModifierState.Control) != 0 ? PointerModifiers.Ctrl :
                PointerModifiers.None;
            mods |=
                (modState & PosixButtonModifierState.Shift) != 0 ? PointerModifiers.Shift :
                PointerModifiers.None;

            // Return the results
            return (button, press, mods);
        }

        private static PointerEventContext GenerateContext(int x, int y, PointerButton button, PointerButtonPress press, PointerModifiers mods)
        {
            // Process dragging
            ProcessDragging(ref press, ref button, out bool dragging);

            // Process double-clicks and other tiered clicks
            if (tieredContext is not null &&
                tieredContext.Button == button &&
                tieredContext.Modifiers == mods &&
                press == PointerButtonPress.Released &&
                tieredContext.Coordinates == (x, y) &&
                inputTimeout.Elapsed <= DoubleClickTimeout)
            {
                clickTier++;
                inputTimeout.Restart();
            }
            else if (press == PointerButtonPress.Released || inputTimeout.Elapsed > DoubleClickTimeout)
            {
                if (press == PointerButtonPress.Released)
                    tieredContext = context;
                else
                    tieredContext = null;
                clickTier = 1;
                inputTimeout.Restart();
            }
            int finalTier = press == PointerButtonPress.Released ? clickTier : 0;

            // Add the results
            return new PointerEventContext(button, press, mods, dragging, x, y, finalTier);
        }

        private static unsafe int ReadMouseSequence(ref byte[] charRead)
        {
            byte* chars = stackalloc byte[6];
            int result = NativeMethods.read(0, chars, 6);
            if (result == 6 && chars[0] == VtSequenceBasicChars.EscapeChar && chars[1] == '[' && chars[2] == 'M')
            {
                charRead = [chars[0], chars[1], chars[2], chars[3], chars[4], chars[5]];
                return 6;
            }
            return -1;
        }

        private static void DisableMouseSupport()
        {
            if (PlatformHelper.IsOnWindows())
            {
                // Set the appropriate modes
                uint mode = ConsoleMisc.GetMode(stdHandle);
                if ((mode & ENABLE_QUICK_EDIT_MODE) == 0)
                    mode |= ENABLE_QUICK_EDIT_MODE;
                if ((mode & ENABLE_MOUSE_INPUT) != 0)
                    mode &= ~ENABLE_MOUSE_INPUT;
                NativeMethods.SetConsoleMode(stdHandle, mode);
            }
            else
                TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?1000l{(EnableMovementEvents ? $"{VtSequenceBasicChars.EscapeChar}[?1003l" : "")}");
        }

        private static void EnableMouseSupport()
        {
            if (PlatformHelper.IsOnWindows())
            {
                // Set the appropriate modes
                uint mode = ConsoleMisc.GetMode(stdHandle);
                if ((mode & ENABLE_QUICK_EDIT_MODE) != 0)
                    mode &= ~ENABLE_QUICK_EDIT_MODE;
                if ((mode & ENABLE_MOUSE_INPUT) == 0)
                    mode |= ENABLE_MOUSE_INPUT;
                NativeMethods.SetConsoleMode(stdHandle, mode);
            }
            else
                TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?1000h{(EnableMovementEvents ? $"{VtSequenceBasicChars.EscapeChar}[?1003h" : "")}");
        }
        #endregion
        #endregion
    }
}
