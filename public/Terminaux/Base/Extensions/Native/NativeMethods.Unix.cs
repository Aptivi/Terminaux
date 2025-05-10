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
using System.Runtime.InteropServices;

namespace Terminaux.Base.Extensions.Native
{
    internal static partial class NativeMethods
    {
        private const int STDIN_FD = 0;
        private const int T_TCSANOW = 0;
        private const int F_GETFL = 3;
        private const int F_SETFL = 4;

        private static Termios orig;

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

        [Flags]
        internal enum TermKeyFlag
        {
            TERMKEY_FLAG_NOINTERPRET = 1 << 0,
            TERMKEY_FLAG_CONVERTKP = 1 << 1,
            TERMKEY_FLAG_RAW = 1 << 2,
            TERMKEY_FLAG_UTF8 = 1 << 3,
        }

        internal enum TermKeyResult
        {
            TERMKEY_RES_NONE = 0,
            TERMKEY_RES_KEY = 1,
            TERMKEY_RES_EOF = 2,
            TERMKEY_RES_AGAIN = 3,
        }

        [Flags]
        internal enum TermKeyKeyMod
        {
            SHIFT = 1 << 0,
            ALT = 1 << 1,
            CTRL = 1 << 2,
        }

        internal enum TermKeyType
        {
            TERMKEY_TYPE_UNICODE,
            TERMKEY_TYPE_FUNCTION,
            TERMKEY_TYPE_KEYSYM,
            TERMKEY_TYPE_MOUSE,
            TERMKEY_TYPE_POSITION,
            TERMKEY_TYPE_MODEREPORT,
            TERMKEY_TYPE_DCS,
            TERMKEY_TYPE_OSC,
            TERMKEY_TYPE_UNKNOWN_CSI = -1
        }

        internal enum TermKeySym
        {
            TERMKEY_SYM_UNKNOWN = -1,
            TERMKEY_SYM_NONE = 0,
            TERMKEY_SYM_BACKSPACE,
            TERMKEY_SYM_TAB,
            TERMKEY_SYM_ENTER,
            TERMKEY_SYM_ESCAPE,
            TERMKEY_SYM_SPACE,
            TERMKEY_SYM_DEL,
            TERMKEY_SYM_UP,
            TERMKEY_SYM_DOWN,
            TERMKEY_SYM_LEFT,
            TERMKEY_SYM_RIGHT,
            TERMKEY_SYM_BEGIN,
            TERMKEY_SYM_FIND,
            TERMKEY_SYM_INSERT,
            TERMKEY_SYM_DELETE,
            TERMKEY_SYM_SELECT,
            TERMKEY_SYM_PAGEUP,
            TERMKEY_SYM_PAGEDOWN,
            TERMKEY_SYM_HOME,
            TERMKEY_SYM_END,
            TERMKEY_SYM_CANCEL,
            TERMKEY_SYM_CLEAR,
            TERMKEY_SYM_CLOSE,
            TERMKEY_SYM_COMMAND,
            TERMKEY_SYM_COPY,
            TERMKEY_SYM_EXIT,
            TERMKEY_SYM_HELP,
            TERMKEY_SYM_MARK,
            TERMKEY_SYM_MESSAGE,
            TERMKEY_SYM_MOVE,
            TERMKEY_SYM_OPEN,
            TERMKEY_SYM_OPTIONS,
            TERMKEY_SYM_PRINT,
            TERMKEY_SYM_REDO,
            TERMKEY_SYM_REFERENCE,
            TERMKEY_SYM_REFRESH,
            TERMKEY_SYM_REPLACE,
            TERMKEY_SYM_RESTART,
            TERMKEY_SYM_RESUME,
            TERMKEY_SYM_SAVE,
            TERMKEY_SYM_SUSPEND,
            TERMKEY_SYM_UNDO,
            TERMKEY_SYM_KP0,
            TERMKEY_SYM_KP1,
            TERMKEY_SYM_KP2,
            TERMKEY_SYM_KP3,
            TERMKEY_SYM_KP4,
            TERMKEY_SYM_KP5,
            TERMKEY_SYM_KP6,
            TERMKEY_SYM_KP7,
            TERMKEY_SYM_KP8,
            TERMKEY_SYM_KP9,
            TERMKEY_SYM_KPENTER,
            TERMKEY_SYM_KPPLUS,
            TERMKEY_SYM_KPMINUS,
            TERMKEY_SYM_KPMULT,
            TERMKEY_SYM_KPDIV,
            TERMKEY_SYM_KPCOMMA,
            TERMKEY_SYM_KPPERIOD,
            TERMKEY_SYM_KPEQUALS,
            TERMKEY_N_SYMS
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Termios
        {
            public uint c_iflag;
            public uint c_oflag;
            public uint c_cflag;
            public uint c_lflag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] c_cc;
            public uint c_ispeed;
            public uint c_ospeed;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TermKeyKeyCodeUnion
        {
            [FieldOffset(0)]
            public long codepoint;
            [FieldOffset(0)]
            public int number;
            [FieldOffset(0)]
            public TermKeySym sym;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct TermKeyKey
        {
            public TermKeyType type;
            public TermKeyKeyCodeUnion code;
            public int modifiers;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
            public string utf8;

        }

        [DllImport("libc", SetLastError = true)]
        internal static extern unsafe int read(int fd, void* buf, uint count);

        [DllImport("libc", SetLastError = true)]
        internal static extern int tcgetattr(int fd, out Termios termios_p);

        [DllImport("libc", SetLastError = true)]
        internal static extern int tcsetattr(int fd, int optional_actions, ref Termios termios_p);

        [DllImport("libc", SetLastError = true)]
        internal static extern int fcntl(int fd, int cmd, int arg);

        [DllImport("libc", SetLastError = true)]
        internal static extern int isatty(int fd);

        [DllImport("termkey", SetLastError = true)]
        internal static extern IntPtr termkey_new(int fd, TermKeyFlag flags);

        [DllImport("termkey", SetLastError = true)]
        internal static extern void termkey_destroy(IntPtr tk);

        [DllImport("termkey", SetLastError = true)]
        internal static extern TermKeyResult termkey_push_bytes(IntPtr tk, IntPtr buffer, UIntPtr length);

        [DllImport("termkey", SetLastError = true)]
        internal static extern TermKeyResult termkey_getkey(IntPtr tk, out TermKeyKey key);

        internal static void RawSet(bool enable)
        {
            if (enable)
            {
                tcgetattr(STDIN_FD, out orig);
                Termios newTermios = orig;
                newTermios.c_iflag &= ~(0x1u | 0x200u | 0x400u);
                newTermios.c_lflag &= ~(0x8u | 0x100u | 0x80u);
                if (!PlatformHelper.IsOnMacOS())
                {
                    newTermios.c_cc[6] = 0;
                    newTermios.c_cc[5] = 1;
                }
                else
                {
                    newTermios.c_cc[16] = 0;
                    newTermios.c_cc[17] = 1;
                }
            }
            else
            {
                if (tcsetattr(STDIN_FD, T_TCSANOW, ref orig) != 0)
                    throw new TerminauxInternalException($"Can't restore new termios attributes for raw mode: {Marshal.GetLastWin32Error()}");
            }
            NonblockSet(enable);
        }

        internal static void NonblockSet(bool enable)
        {
            if (PlatformHelper.IsOnWindows())
                return;

            int nonBlock = PlatformHelper.IsOnMacOS() ? 0x4 : 0x800;
            if (enable)
            {
                int flags = fcntl(STDIN_FD, F_GETFL, 0);
                if (flags == -1)
                    throw new TerminauxInternalException($"Can't get file descriptor flags: {Marshal.GetLastWin32Error()}");
                if (fcntl(STDIN_FD, F_SETFL, flags | nonBlock) == -1)
                    throw new TerminauxInternalException($"Can't set file descriptor flag to non-blocking read: {Marshal.GetLastWin32Error()}");
            }
            else
            {
                int flags = fcntl(STDIN_FD, F_GETFL, 0);
                if (flags == -1)
                    throw new TerminauxInternalException($"Can't get file descriptor flags: {Marshal.GetLastWin32Error()}");
                flags &= ~nonBlock;
                if (fcntl(STDIN_FD, F_SETFL, flags) == -1)
                    throw new TerminauxInternalException($"Can't set file descriptor flag to blocking read: {Marshal.GetLastWin32Error()}");
            }
        }
    }
}
