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

using SpecProbe.Software.Platform;
using System.Runtime.InteropServices;

namespace Terminaux.Inputs.Native
{
    internal static class InputPosix
    {
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
        internal static extern unsafe int read(int fd, void* buf, uint count);

        [DllImport("libc", SetLastError = true)]
        internal static extern int ioctl(int fd, ulong request, ref uint argp);

        internal static ulong DetermineIoCtl()
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
    }
}
