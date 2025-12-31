//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using System.Runtime.InteropServices;

namespace Terminaux.Base.Extensions.Native
{
    internal static partial class NativeMethods
    {
        // Library names
        private const string winKernel = "kernel32.dll";
        private const string winUser = "user32.dll";

        internal const int WM_GETICON = 0x007F;

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

        [DllImport(winKernel, SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        [DllImport(winKernel, SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr handle, out uint mode);

        [DllImport(winKernel, SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int handle);

        [DllImport(winKernel, CharSet = CharSet.Unicode)]
        internal static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

        [DllImport(winKernel, CharSet = CharSet.Unicode)]
        internal static extern bool PeekConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, ref uint lpNumberOfEventsRead);

        [DllImport(winKernel, SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport(winUser, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}
