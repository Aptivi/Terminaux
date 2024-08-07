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

using SpecProbe.Software.Platform;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer listener for handling mouse events
    /// </summary>
    public static class PointerListener
    {
        private static PointerEventContext? context = null;
        private static bool listening = false;
        private static bool active = false;
        private static PointerButton draggingButton = PointerButton.None;
        private static Thread? pointerListener;
        private static bool enableMovementEvents;
        private static int clickTier = 1;
        private static PointerEventContext? tieredContext = null;
        private static Stopwatch clickTierStopwatch = new();

        /// <summary>
        /// Raised when console mouse event occurs
        /// </summary>
        public static event EventHandler<PointerEventContext> MouseEvent = delegate { };

        /// <summary>
        /// Checks whether the pointer listener is listening to mouse events or not
        /// </summary>
        public static bool Listening =>
            listening;

        /// <summary>
        /// Checks to see whether any pending mouse or keyboard events are here
        /// </summary>
        public static bool InputAvailable =>
            PointerAvailable || (ConsoleWrapper.KeyAvailable && !PointerActive);

        /// <summary>
        /// Checks to see whether any pending mouse events are here
        /// </summary>
        public static bool PointerAvailable =>
            Listening && context is not null;

        /// <summary>
        /// Checks to see whether the pointer is active or not
        /// </summary>
        public static bool PointerActive =>
            Listening && active;

        /// <summary>
        /// Checks to see whether the movement events are enabled or not
        /// </summary>
        public static bool EnableMovementEvents
        {
            get => enableMovementEvents;
            set
            {
                enableMovementEvents = value;
                if (Listening)
                    TextWriterRaw.WriteRaw(enableMovementEvents ? "\u001b[?1003h" : "\u001b[?1003l");
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
        /// Specifies the time in milliseconds whether the double click times out
        /// </summary>
        public static TimeSpan DoubleClickTimeout { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Reads a pointer event and removes it from the context buffer
        /// </summary>
        /// <returns>A <see cref="PointerEventContext"/> instance that describes the last mouse event, or <see langword="null"/> if there are no more events to read.</returns>
        public static PointerEventContext? ReadPointerNow()
        {
            if (context is null)
                return null;
            lock (context)
            {
                if (!PointerAvailable)
                    return null;
                var ctx = context;
                context = null;
                return ctx;
            }
        }

        /// <summary>
        /// Starts listening to mouse events
        /// </summary>
        public static void StartListening()
        {
            if (listening)
                return;
            listening = true;
            active = false;
            context = null;
            tieredContext = null;
            clickTierStopwatch.Reset();

            // Now, start the listener by calling platform-specific initialization code
            if (PlatformHelper.IsOnWindows())
                StartListenerWindows();
            else
                StartListenerPosix();
        }

        /// <summary>
        /// Stops listening to mouse events
        /// </summary>
        public static void StopListening()
        {
            if (!listening)
                return;
            listening = false;
            active = false;
            context = null;
            tieredContext = null;

            // Now, stop the listener by calling platform-specific finalization code
            if (PlatformHelper.IsOnWindows())
                StopListenerWindows();
            else
                StopListenerPosix();
        }

        #region POSIX-specific
        private static ulong DetermineIoCtl()
        {
            if (PlatformHelper.IsOnMacOS())
            {
                // Return FreeBSD's FIONREAD ioctl according to this line, because Darwin is BSD.
                // http://fxr.watson.org/fxr/source/sys/filio.h#L49
                //    |  IOC_OUT  |   l   IOCPARM_MASK   |    g         |   n
                return 0x40000000 | ((4 & 0x1fff) << 16) | (('f') << 8) | (127);
            }

            // Return Linux's FIONREAD ioctl according to this line.
            // https://github.com/torvalds/linux/blob/cf87f46fd34d6c19283d9625a7822f20d90b64a4/include/uapi/asm-generic/ioctls.h#L46
            return 0x541b;
        }

        private static void StartListenerPosix()
        {
            // Test ioctl
            ulong ctl = DetermineIoCtl();
            uint numRead = 0;
            int result = ioctl(0, ctl, ref numRead);
            if (result == -1)
                throw new TerminauxException("Failed to start the listener.");

            // Set DEC locator mode to standard mode
            Process.Start("stty", "-echo -icanon min 1 time 0");
            TextWriterRaw.WriteRaw($"\u001b[?1000h{(EnableMovementEvents ? "\u001b[?1003h" : "")}");

            // Make a new thread for POSIX listener
            pointerListener = new(() =>
            {
                var sw = new Stopwatch();
                while (listening)
                {
                    uint numRead = 0;
                    bool error = false;

                    // Functions to help get output
                    int Peek(ref uint numRead, ref bool error)
                    {
                        int result = ioctl(0, ctl, ref numRead);
                        if (result == -1)
                        {
                            // Some failure occurred. Stop listening.
                            StopListening();
                            error = true;
                            return -1;
                        }
                        return result;
                    }
                    unsafe int Read(ref byte[] charRead, ref bool error)
                    {
                        int result = 0;
                        lock (Console.In)
                        {
                            int _ = Peek(ref numRead, ref error);
                            if (numRead > 0 && (numRead == 6 || numRead == 4))
                            {
                                byte* chars = stackalloc byte[(int)numRead];
                                result = read(0, chars, numRead);
                                if (result == -1)
                                {
                                    // Some failure occurred. Stop listening.
                                    StopListening();
                                    error = true;
                                    return -1;
                                }
                                if (numRead == 6)
                                {
                                    if (chars[0] == '\u001b' && chars[1] == '[' && chars[2] == 'M')
                                    {
                                        active = true;
                                        sw.Reset();
                                        charRead = [chars[0], chars[1], chars[2], chars[3], chars[4], chars[5]];
                                    }
                                }
                                else if (numRead == 4)
                                {
                                    if (chars[0] == '\0')
                                    {
                                        active = true;
                                        sw.Reset();
                                        charRead = [chars[0], chars[1], chars[2], chars[3]];
                                    }
                                }
                                while (ConsoleWrapper.KeyAvailable)
                                    ConsoleWrapper.ReadKey(true);
                            }
                            else if (active && sw.ElapsedMilliseconds > 250)
                            {
                                while (ConsoleWrapper.KeyAvailable)
                                    ConsoleWrapper.ReadKey(true);
                                sw.Reset();
                                active = false;
                            }
                        }
                        return result;
                    }

                    // Fill the chars array
                    byte[] chars = [];
                    byte button = 0;
                    byte x = 0;
                    byte y = 0;
                    int readChar = Read(ref chars, ref error);
                    if (error)
                        break;
                    if (readChar == 0 || chars.Length == 0)
                        continue;
                    if (chars[0] == '\u001b' || chars[0] == 91)
                    {
                        // Now, read the button, X, and Y positions
                        button = chars[0] == 91 ? chars[2] : chars[3];
                        x = chars[0] == 91 ? (byte)(chars[3] - 32) : (byte)(chars[4] - 32);
                        y = chars[0] == 91 ? (byte)(chars[4] - 32) : (byte)(chars[5] - 32);
                    }
                    else if (chars[0] == '\0')
                    {
                        // Now, read the button, X, and Y positions
                        button = chars[1];
                        x = (byte)(chars[2] - 32);
                        y = (byte)(chars[3] - 32);
                    }
                    else
                        continue;
                    x -= 1;
                    y -= 1;
                    sw.Start();

                    // Get the button states and change them as necessary
                    PosixButtonState state = (PosixButtonState)(button & 0b11);
                    PosixButtonModifierState modState = (PosixButtonModifierState)(button & 0b11100);
                    if (button >= 64 && button < 96)
                        state = PosixButtonState.Movement;
                    if (button >= 96 && button % 2 == 0)
                        state = PosixButtonState.WheelUp;
                    else if (button >= 97)
                        state = PosixButtonState.WheelDown;
                    Debug.WriteLine($"[{button}: {state} {modState}] X={x} Y={y}");

                    // Now, translate them to something Terminaux understands
                    PointerButtonPress press =
                        state == PosixButtonState.Left || state == PosixButtonState.Middle || state == PosixButtonState.Right ? PointerButtonPress.Clicked :
                        state == PosixButtonState.Released ? PointerButtonPress.Released :
                        state == PosixButtonState.WheelDown || state == PosixButtonState.WheelUp ? PointerButtonPress.Scrolled :
                        PointerButtonPress.Moved;
                    PointerButton buttonPtr =
                        state == PosixButtonState.Left ? (SwapLeftRightButtons ? PointerButton.Right : PointerButton.Left) :
                        state == PosixButtonState.Middle ? PointerButton.Middle :
                        state == PosixButtonState.Right ? (SwapLeftRightButtons ? PointerButton.Left : PointerButton.Right) :
                        state == PosixButtonState.WheelDown ? (InvertScrollYAxis ? PointerButton.WheelUp : PointerButton.WheelDown) :
                        state == PosixButtonState.WheelUp ? (InvertScrollYAxis ? PointerButton.WheelDown : PointerButton.WheelUp) :
                        PointerButton.None;
                    PointerModifiers mods =
                        (modState & PosixButtonModifierState.Alt) != 0 ? PointerModifiers.Alt :
                        PointerModifiers.None;
                    mods |=
                        (modState & PosixButtonModifierState.Control) != 0 ? PointerModifiers.Ctrl :
                        PointerModifiers.None;
                    mods |=
                        (modState & PosixButtonModifierState.Shift) != 0 ? PointerModifiers.Shift :
                        PointerModifiers.None;

                    // Process dragging
                    bool dragging = false;
                    if (!EnableMovementEvents && press == PointerButtonPress.Moved)
                        break;
                    else
                        ProcessDragging(ref press, ref buttonPtr, out dragging);

                    // Process double-clicks and other tiered clicks
                    if (tieredContext is not null && tieredContext.Button == buttonPtr && tieredContext.Modifiers == mods && press == PointerButtonPress.Released && tieredContext.Coordinates == (x, y) && clickTierStopwatch.Elapsed <= DoubleClickTimeout)
                    {
                        clickTier++;
                        clickTierStopwatch.Restart();
                    }
                    else if (press == PointerButtonPress.Released || clickTierStopwatch.Elapsed > DoubleClickTimeout)
                    {
                        if (press == PointerButtonPress.Released)
                            tieredContext = context;
                        else
                            tieredContext = null;
                        clickTier = 1;
                        clickTierStopwatch.Restart();
                    }
                    int finalTier = press == PointerButtonPress.Released ? clickTier : 0;

                    // Add the results
                    var ctx = new PointerEventContext(buttonPtr, press, mods, dragging, x, y, finalTier);
                    context = ctx;
                    MouseEvent.Invoke("Terminaux", ctx);
                }
            })
            {
                Name = "Terminaux Pointer Listener for POSIX",
                IsBackground = true,
            };
            pointerListener.Start();
        }

        private static void StopListenerPosix()
        {
            TextWriterRaw.WriteRaw($"\u001b[?1000l{(EnableMovementEvents ? "\u001b[?1003l" : "")}");
            Process.Start("stty", "echo");
            pointerListener = null;
        }

        [DllImport("libc", SetLastError = true)]
        private static extern unsafe int read(int fd, void* buf, uint count);

        internal enum PosixButtonState : uint
        {
            Left = 0x0000,
            Middle = 0x0001,
            Right = 0x0002,
            Released = 0x0003,
            Movement = 0x0004,
            WheelUp = 0x0005,
            WheelDown = 0x0006,
        }

        internal enum PosixButtonModifierState : uint
        {
            Shift = 0x0004,
            Alt = 0x0008,
            Control = 0x0010,
        }

        [DllImport("libc", SetLastError = true)]
        internal static extern int ioctl(int fd, ulong request, ref uint argp);
        #endregion

        #region Windows-specific
        private const uint ENABLE_MOUSE_INPUT = 0x0010;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;

        private static void StartListenerWindows()
        {
            // Set the appropriate modes
            IntPtr stdHandle = ConsolePositioning.GetStdHandle(-10);
            uint mode = ConsolePositioning.GetMode(stdHandle);
            if ((mode & ENABLE_QUICK_EDIT_MODE) != 0)
                mode &= ~ENABLE_QUICK_EDIT_MODE;
            if ((mode & ENABLE_MOUSE_INPUT) == 0)
                mode |= ENABLE_MOUSE_INPUT;
            ConsolePositioning.SetConsoleMode(stdHandle, mode);

            // Now, start a thread to handle event
            pointerListener = new(() =>
            {
                while (listening)
                {
                    // Make a record and read the input for mouse event
                    uint numRead = 0;
                    INPUT_RECORD[] record = [new INPUT_RECORD()];
                    PeekConsoleInput(stdHandle, record, 1, ref numRead);

                    // Now, filter all events except the mouse ones
                    switch (record[0].EventType)
                    {
                        case INPUT_RECORD.MOUSE_EVENT:
                            ReadConsoleInput(stdHandle, record, 1, ref numRead);

                            // Get the coordinates and event arguments
                            var @event = record[0].MouseEvent;
                            var coord = @event.dwMousePosition;
                            Debug.WriteLine($"Coord: {coord.X}, {coord.Y}, {@event.dwButtonState}, {@event.dwControlKeyState}, {@event.dwEventFlags}");

                            // Now, translate them to something Terminaux understands
                            PointerButton button =
                                @event.dwButtonState == ButtonState.Left ? (SwapLeftRightButtons ? PointerButton.Right : PointerButton.Left) :
                                @event.dwButtonState == ButtonState.Middle ? PointerButton.Middle :
                                @event.dwButtonState == ButtonState.Right ? (SwapLeftRightButtons ? PointerButton.Left : PointerButton.Right) :
                                (uint)@event.dwButtonState >= 4000000000 ? (InvertScrollYAxis ? PointerButton.WheelUp : PointerButton.WheelDown) :
                                (uint)@event.dwButtonState >= 7000000 ? (InvertScrollYAxis ? PointerButton.WheelDown : PointerButton.WheelUp) :
                                PointerButton.None;
                            PointerButtonPress press =
                                @event.dwButtonState != ButtonState.None && @event.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Clicked :
                                @event.dwButtonState != ButtonState.None && @event.dwEventFlags == EventFlags.DoubleClicked ? PointerButtonPress.Clicked :
                                @event.dwButtonState == ButtonState.None && @event.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Released :
                                button == PointerButton.WheelDown || button == PointerButton.WheelUp ? PointerButtonPress.Scrolled :
                                PointerButtonPress.Moved;
                            PointerModifiers mods =
                                (@event.dwControlKeyState & ControlKeyState.RightAltPressed) != 0 ? PointerModifiers.Alt :
                                (@event.dwControlKeyState & ControlKeyState.LeftAltPressed) != 0 ? PointerModifiers.Alt :
                                PointerModifiers.None;
                            mods |=
                                (@event.dwControlKeyState & ControlKeyState.RightCtrlPressed) != 0 ? PointerModifiers.Ctrl :
                                (@event.dwControlKeyState & ControlKeyState.LeftCtrlPressed) != 0 ? PointerModifiers.Ctrl :
                                PointerModifiers.None;
                            mods |=
                                (@event.dwControlKeyState & ControlKeyState.ShiftPressed) != 0 ? PointerModifiers.Shift :
                                PointerModifiers.None;

                            // Process dragging
                            bool dragging = false;
                            if (!EnableMovementEvents && press == PointerButtonPress.Moved)
                                break;
                            else
                                ProcessDragging(ref press, ref button, out dragging);

                            // Process double-clicks and other tiered clicks
                            if (tieredContext is not null && tieredContext.Button == button && tieredContext.Modifiers == mods && press == PointerButtonPress.Released && tieredContext.Coordinates == (coord.X, coord.Y) && clickTierStopwatch.Elapsed <= DoubleClickTimeout)
                            {
                                clickTier++;
                                clickTierStopwatch.Restart();
                            }
                            else if (press == PointerButtonPress.Released || clickTierStopwatch.Elapsed > DoubleClickTimeout)
                            {
                                if (press == PointerButtonPress.Released)
                                    tieredContext = context;
                                else
                                    tieredContext = null;
                                clickTier = 1;
                                clickTierStopwatch.Restart();
                            }
                            int finalTier = press == PointerButtonPress.Released ? clickTier : 0;

                            // Add the results
                            var ctx = new PointerEventContext(button, press, mods, dragging, coord.X, coord.Y, finalTier);
                            context = ctx;
                            MouseEvent.Invoke("Terminaux", ctx);
                            break;
                    }
                }
            })
            {
                Name = "Terminaux Pointer Listener for Windows",
                IsBackground = true,
            };
            pointerListener.Start();
        }

        private static void StopListenerWindows()
        {
            // Set the appropriate modes
            IntPtr stdHandle = ConsolePositioning.GetStdHandle(-10);
            uint mode = ConsolePositioning.GetMode(stdHandle);
            if ((mode & ENABLE_QUICK_EDIT_MODE) == 0)
                mode |= ENABLE_QUICK_EDIT_MODE;
            if ((mode & ENABLE_MOUSE_INPUT) != 0)
                mode &= ~ENABLE_MOUSE_INPUT;
            ConsolePositioning.SetConsoleMode(stdHandle, mode);
            pointerListener = null;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool PeekConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, ref uint lpcNumberOfEvents);

        internal struct COORD
        {
            internal short X;
            internal short Y;

            internal COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT_RECORD
        {
            internal const ushort MOUSE_EVENT = 0x0002;

            [FieldOffset(0)]
            internal ushort EventType;
            [FieldOffset(4)]
            internal MOUSE_EVENT_RECORD MouseEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSE_EVENT_RECORD
        {
            internal COORD dwMousePosition;

            internal ButtonState dwButtonState;
            internal ControlKeyState dwControlKeyState;
            internal EventFlags dwEventFlags;
        }

        internal enum ButtonState : uint
        {
            None = 0x0000,
            Left = 0x0001,
            Right = 0x0002,
            Middle = 0x0004,
            Fourth = 0x0008,
            Fifth = 0x0010,
        }

        internal enum ControlKeyState : uint
        {
            None = 0x0000,
            RightAltPressed = 0x0001,
            LeftAltPressed = 0x0002,
            RightCtrlPressed = 0x0004,
            LeftCtrlPressed = 0x0008,
            ShiftPressed = 0x0010,
            NumlockOn = 0x0020,
            ScrollLockOn = 0x0040,
            CapsLockOn = 0x0080,
            EnhancedKey = 0x0100,
        }

        internal enum EventFlags : uint
        {
            Clicked = 0x0000,
            Moved = 0x0001,
            DoubleClicked = 0x0002,
            WheelScrolled = 0x0004,
            HorizontalWheelScrolled = 0x0008,
        }
        #endregion

        #region Private functions
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
        #endregion

        static PointerListener()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
