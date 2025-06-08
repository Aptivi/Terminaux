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
using System.Runtime.InteropServices;
using Textify.General;

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
                    throw new TerminauxInternalException("Can't restore new termios attributes for raw mode: {0}".FormatString(Marshal.GetLastWin32Error()));
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
                    throw new TerminauxInternalException("Can't get file descriptor flags: {0}".FormatString(Marshal.GetLastWin32Error()));
                if (fcntl(STDIN_FD, F_SETFL, flags | nonBlock) == -1)
                    throw new TerminauxInternalException("Can't set file descriptor flag to non-blocking read: {0}".FormatString(Marshal.GetLastWin32Error()));
            }
            else
            {
                int flags = fcntl(STDIN_FD, F_GETFL, 0);
                if (flags == -1)
                    throw new TerminauxInternalException("Can't get file descriptor flags: {0}".FormatString(Marshal.GetLastWin32Error()));
                flags &= ~nonBlock;
                if (fcntl(STDIN_FD, F_SETFL, flags) == -1)
                    throw new TerminauxInternalException("Can't set file descriptor flag to blocking read: {0}".FormatString(Marshal.GetLastWin32Error()));
            }
        }
    }
}
