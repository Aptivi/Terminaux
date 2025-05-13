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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        internal static PointerEventContext? context = null;
        private static PointerButton draggingButton = PointerButton.None;
        private static bool enableMovementEvents;
        private static int clickTier = 1;
        private static PointerEventContext? tieredContext = null;
        private static bool enableMouse;
        private static readonly Stopwatch inputTimeout = new();
        private static readonly IntPtr stdHandle = PlatformHelper.IsOnWindows() ? NativeMethods.GetStdHandle(-10) : IntPtr.Zero;
        private static readonly Queue<InputEventInfo> mouseEventQueue = [];
        private static readonly Queue<InputEventInfo> keyboardEventQueue = [];
        private static readonly Queue<InputEventInfo> positionEventQueue = [];

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
                if (!value)
                {
                    enableMouse = value;
                    DisableMouseSupport();
                }
                else
                {
                    EnableMouseSupport();
                    enableMouse = value;
                }
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
        /// <param name="eventType">Event types to wait for (None implies all events)</param>
        public static InputEventInfo ReadPointerOrKey(InputEventType eventType = InputEventType.Mouse | InputEventType.Keyboard)
        {
            InputEventInfo input = new();
            SpinWait.SpinUntil(() =>
            {
                input = ReadPointerOrKeyNoBlock(eventType);
                return input.EventType != InputEventType.None;
            });
            return input;
        }

        /// <summary>
        /// Reads either a pointer or a key (non-blocking)
        /// </summary>
        /// <param name="eventType">Event types to wait for (None implies all events)</param>
        public static InputEventInfo ReadPointerOrKeyNoBlock(InputEventType eventType = InputEventType.Mouse | InputEventType.Keyboard)
        {
            // Enqueue the events
            var genericEvent = new InputEventInfo();
            EnqueueEvents();

            // Return the enqueued event itself
            switch (eventType)
            {
                case InputEventType.None:
                    if (mouseEventQueue.Count > 0)
                        return mouseEventQueue.Dequeue();
                    if (keyboardEventQueue.Count > 0)
                        return keyboardEventQueue.Dequeue();
                    if (positionEventQueue.Count > 0)
                        return positionEventQueue.Dequeue();
                    return genericEvent;
                default:
                    if (mouseEventQueue.Count > 0 && eventType.HasFlag(InputEventType.Mouse))
                        return mouseEventQueue.Dequeue();
                    if (keyboardEventQueue.Count > 0 && eventType.HasFlag(InputEventType.Keyboard))
                        return keyboardEventQueue.Dequeue();
                    if (positionEventQueue.Count > 0 && eventType.HasFlag(InputEventType.Position))
                        return positionEventQueue.Dequeue();
                    return genericEvent;
            }
        }

        /// <summary>
        /// Reads the next key from the console input stream
        /// </summary>
        public static ConsoleKeyInfo ReadKey()
        {
            TermReaderTools.isWaitingForInput = true;
            InputEventInfo data = new();
            SpinWait.SpinUntil(() =>
            {
                data = ReadPointerOrKeyNoBlock();
                return data.EventType == InputEventType.Keyboard;
            });
            TermReaderTools.isWaitingForInput = false;
            return data.ConsoleKeyInfo ?? default;
        }

        /// <summary>
        /// Reads the next key from the console input stream with the timeout
        /// </summary>
        /// <param name="Timeout">Timeout</param>
        public static (ConsoleKeyInfo result, bool provided) ReadKeyTimeout(TimeSpan Timeout)
        {
            TermReaderTools.isWaitingForInput = true;
            InputEventInfo data = new();
            bool result = SpinWait.SpinUntil(() =>
            {
                data = ReadPointerOrKeyNoBlock();
                return data.EventType == InputEventType.Keyboard;
            }, Timeout);
            TermReaderTools.isWaitingForInput = false;
            return (!result ? default : data.ConsoleKeyInfo ?? default, result);
        }

        /// <summary>
        /// Invalidates the input
        /// </summary>
        public static void InvalidateInput()
        {
            SpinWait.SpinUntil(() =>
            {
                var data = ReadPointerOrKeyNoBlock();
                return data.EventType == InputEventType.None;
            });
        }

        private static void EnqueueEvents()
        {
            PointerEventContext? ctx = null;
            ConsoleKeyInfo? cki = null;
            List<InputEventInfo> eventInfos = [];
            while (true)
            {
                if (PlatformHelper.IsOnWindows())
                {
                    // Set the appropriate modes
                    bool bail = false;
                    uint numRead = 0;
                    INPUT_RECORD[] record = [new INPUT_RECORD()];
                    PeekConsoleInput(stdHandle, record, 1, ref numRead);

                    // Check for event number
                    if (numRead == 0)
                        break;

                    // Check the event type
                    switch (record[0].EventType)
                    {
                        case INPUT_RECORD.MOUSE_EVENT:
                            // Get the coordinates and event arguments
                            ReadConsoleInput(stdHandle, record, 1, ref numRead);
                            var @event = record[0].MouseEvent;
                            var coord = @event.dwMousePosition;
                            ConsoleLogger.Debug($"Coord: {coord.X}, {coord.Y}, {@event.dwButtonState}, {@event.dwControlKeyState}, {@event.dwEventFlags}");

                            // Now, translate them to something Terminaux understands
                            (PointerButton button, PointerButtonPress press, PointerModifiers mods) = ProcessPointerEventWin(@event);
                            if (!EnableMovementEvents && press == PointerButtonPress.Moved)
                                break;
                            ctx = GenerateContext(coord.X, coord.Y, button, press, mods);
                            eventInfos.Add(new(ctx, null, null));
                            context = ctx;
                            bail = true;
                            break;
                        default:
                            if (Console.KeyAvailable)
                            {
                                // Read a key
                                cki = Console.ReadKey(true);
                                eventInfos.Add(new(null, cki, null));
                                bail = true;
                            }
                            else
                            {
                                // Dismiss any foreign events, such as window focus event
                                GetNumberOfConsoleInputEvents(stdHandle, ref numRead);
                                while (numRead != 0)
                                {
                                    ReadConsoleInput(stdHandle, record, 1, ref numRead);
                                    GetNumberOfConsoleInputEvents(stdHandle, ref numRead);
                                }
                            }
                            break;
                    }
                    if (bail)
                        break;

                }
                else
                {
                    // Check for raw, since cooked mode doesn't support mouse.
                    if (!ConsoleMode.IsRaw)
                    {
                        // Use the standard ReadKey function
                        if (Console.KeyAvailable)
                        {
                            cki = Console.ReadKey(true);
                            eventInfos.Add(new(null, cki, null));
                        }
                        break;
                    }
                    else
                    {
                        byte[] charRead = [];
                        lock (Console.In)
                        {
                            unsafe
                            {
                                byte* chars = stackalloc byte[1024];
                                int result = NativeMethods.read(0, chars, 1024);
                                if (result != -1)
                                {
                                    charRead = new byte[result];
                                    for (int i = 0; i < result; i++)
                                        charRead[i] = chars[i];
                                }
                                else
                                    break;
                            }
                        }

                        // Check the array
                        if (charRead.Length == 0)
                            break;

                        // Make a tokenizer instance and return a group of events to install
                        char[] tokenChars = Encoding.UTF8.GetChars(charRead);
                        var tokenizer = new InputPosixTokenizer(tokenChars);
                        var parsedEvents = tokenizer.Parse();
                        eventInfos.AddRange(parsedEvents);
                    }
                }
            }

            // Add all resultant event info instances
            foreach (var inputEvent in eventInfos)
            {
                switch (inputEvent.EventType)
                {
                    case InputEventType.Mouse:
                        mouseEventQueue.Enqueue(inputEvent);
                        break;
                    case InputEventType.Keyboard:
                        keyboardEventQueue.Enqueue(inputEvent);
                        break;
                    case InputEventType.Position:
                        positionEventQueue.Enqueue(inputEvent);
                        break;
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

        internal static (PointerButton button, PointerButtonPress press, PointerModifiers mods) ProcessPointerEventPosix(PosixButtonState state, PosixButtonModifierState modState)
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

        internal static PointerEventContext GenerateContext(int x, int y, PointerButton button, PointerButtonPress press, PointerModifiers mods)
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

        private static void DisableMouseSupport()
        {
            if (!EnableMouse)
                return;
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
            {
                TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?1000l{VtSequenceBasicChars.EscapeChar}[?1006l{(EnableMovementEvents ? $"{VtSequenceBasicChars.EscapeChar}[?1003l" : "")}");
                if (ConsoleMode.IsRaw)
                    ConsoleMode.DisableRaw();
            }
        }

        private static void EnableMouseSupport()
        {
            if (EnableMouse)
                return;
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
            {
                if (!ConsoleMode.IsRaw)
                    ConsoleMode.EnableRaw();
                TextWriterRaw.WriteRaw($"{VtSequenceBasicChars.EscapeChar}[?1000h{VtSequenceBasicChars.EscapeChar}[?1006h{(EnableMovementEvents ? $"{VtSequenceBasicChars.EscapeChar}[?1003h" : "")}");
            }
        }
    }
}
