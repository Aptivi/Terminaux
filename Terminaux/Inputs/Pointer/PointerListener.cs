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

using SpecProbe.Platform;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Terminaux.Inputs.Pointer
{
    /// <summary>
    /// Pointer listener for handling mouse events
    /// </summary>
    public static class PointerListener
    {
        private static bool listening = false;
        private static Thread? pointerListener;

        /// <summary>
        /// Raised when console mouse event occurs
        /// </summary>
        public static event EventHandler<PointerEventContext> MouseEvent = delegate { };

        /// <summary>
        /// Starts listening to mouse events
        /// </summary>
        public static void StartListening()
        {
            if (listening)
                return;
            listening = true;

            // Now, start the listener by calling platform-specific initialization code
            if (PlatformHelper.IsOnWindows())
                StartListenerWindows();
            else
                StartListenerLinux();
        }

        /// <summary>
        /// Stops listening to mouse events
        /// </summary>
        public static void StopListening()
        {
            if (!listening)
                return;
            listening = false;

            // Now, stop the listener by calling platform-specific finalization code
            if (PlatformHelper.IsOnWindows())
                StopListenerWindows();
            else
                StopListenerLinux();
        }

        #region Linux-specific
        private static void StartListenerLinux()
        {
            // Test ioctl
            uint numRead = 0;
            int result = ioctl(0, 0x541b, ref numRead);
            if (result == -1)
                throw new TerminauxException("Failed to start the listener.");

            // Set DEC locator mode to standard mode
            Process.Start("stty", "-echo -icanon");
            Console.Write("\u001b[?1003h");

            // Make a new thread for Linux listener
            pointerListener = new(() =>
            {
                while (listening)
                {
                    uint numRead = 0;
                    bool error = false;

                    // Functions to help get output
                    static int Peek(ref uint numRead, ref bool error)
                    {
                        int result = ioctl(0, 0x541b, ref numRead);
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
                        int _ = Peek(ref numRead, ref error);
                        if (numRead != 6)
                            return 0;
                        byte* chars = stackalloc byte[6];
                        int result = read(0, chars, 6);
                        if (result == -1)
                        {
                            // Some failure occurred. Stop listening.
                            StopListening();
                            error = true;
                            return -1;
                        }
                        charRead = [chars[0], chars[1], chars[2], chars[3], chars[4], chars[5]];
                        return result;
                    }
                    byte[] chars = [];
                    byte button = 0;
                    byte x = 0;
                    byte y = 0;
                    int readChar = Read(ref chars, ref error);
                    if (error)
                        break;
                    if (readChar == 0)
                        continue;
                    if (chars[0] == '\u001b')
                    {
                        // Now, read the button, X, and Y positions
                        button = chars[3];
                        x = (byte)(chars[4] - 32);
                        y = (byte)(chars[5] - 32);

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
                            PointerButtonPress.Moved;
                        PointerButton buttonPtr =
                            state == PosixButtonState.Left ? PointerButton.Left :
                            state == PosixButtonState.Middle ? PointerButton.Middle :
                            state == PosixButtonState.Right ? PointerButton.Right :
                            state == PosixButtonState.WheelDown ? PointerButton.WheelDown :
                            state == PosixButtonState.WheelUp ? PointerButton.WheelUp :
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
                        var ctx = new PointerEventContext(buttonPtr, press, mods, x, y);
                        MouseEvent.Invoke("Terminaux", ctx);
                    }
                }
            })
            {
                Name = "Terminaux Pointer Listener for Linux",
                IsBackground = true,
            };
            pointerListener.Start();
        }

        private static void StopListenerLinux()
        {
            Console.Write("\u001b[?1003l");
            Process.Start("stty", "echo");
            pointerListener = null;
        }

        [DllImport("libc", SetLastError = true)]
        private static extern int ioctl(int fd, ulong request, ref uint argp);

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
                            // Get the coordinates and event arguments
                            var @event = record[0].MouseEvent;
                            var coord = @event.dwMousePosition;
                            Debug.WriteLine($"Coord: {coord.X}, {coord.Y}, {@event.dwButtonState}, {@event.dwControlKeyState}, {@event.dwEventFlags}");

                            // Now, translate them to something Terminaux understands
                            PointerButtonPress press =
                                @event.dwButtonState != ButtonState.None && @event.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Clicked :
                                @event.dwButtonState != ButtonState.None && @event.dwEventFlags == EventFlags.DoubleClicked ? PointerButtonPress.Clicked :
                                @event.dwButtonState == ButtonState.None && @event.dwEventFlags == EventFlags.Clicked ? PointerButtonPress.Released :
                                PointerButtonPress.Moved;
                            PointerButton button =
                                @event.dwButtonState == ButtonState.Left ? PointerButton.Left :
                                @event.dwButtonState == ButtonState.Middle ? PointerButton.Middle :
                                @event.dwButtonState == ButtonState.Right ? PointerButton.Right :
                                (uint)@event.dwButtonState >= 4000000000 ? PointerButton.WheelDown :
                                (uint)@event.dwButtonState >= 7000000 ? PointerButton.WheelUp :
                                PointerButton.None;
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
                            var ctx = new PointerEventContext(button, press, mods, coord.X, coord.Y);
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
    }
}
