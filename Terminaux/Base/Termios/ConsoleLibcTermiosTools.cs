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

using System.Runtime.InteropServices;
using speed_t = System.UInt32;

namespace Terminaux.Base.Termios
{
    internal unsafe class ConsoleLibcTermiosTools
    {
        private const string libc = "libc";

        [DllImport(libc)]
        internal static extern speed_t cfgetospeed(ConsoleLibcTermios* p);
        [DllImport(libc)]
        internal static extern speed_t cfgetispeed(ConsoleLibcTermios* p);
        [DllImport(libc, SetLastError = true)]
        internal static extern int cfsetospeed(ConsoleLibcTermios* p, speed_t speed);
        [DllImport(libc, SetLastError = true)]
        internal static extern int cfsetispeed(ConsoleLibcTermios* p, speed_t speed);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcgetattr(int fd, ConsoleLibcTermios* p);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcsetattr(int fd, uint optional_actions, ConsoleLibcTermios* p);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcsendbreak(int fd, int duration);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcdrain(int fd);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcflush(int fd, int queue_selector);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcflow(int fd, int action);
        [DllImport(libc, SetLastError = true)]
        internal static extern int tcgetsid(int fd);

        internal const byte VINTR = 0;
        internal const byte VQUIT = 1;
        internal const byte VERASE = 2;
        internal const byte VKILL = 3;
        internal const byte VEOF = 4;
        internal const int VTIME = 5;
        internal const int VMIN = 6;
        internal const byte VSWTC = 7;
        internal const byte VSTART = 8;
        internal const byte VSTOP = 9;
        internal const byte VSUSP = 10;
        internal const byte VEOL = 11;
        internal const byte VREPRINT = 12;
        internal const byte VDISCARD = 13;
        internal const byte VWERASE = 14;
        internal const byte VLNEXT = 15;
        internal const byte VEOL2 = 16;

        internal const uint IGNBRK = 1;

        internal const uint TCOOFF = 0;
        internal const uint TCOON = 1;
        internal const uint TCIOFF = 2;
        internal const uint TCION = 3;

        internal const uint TCIFLUSH = 0;
        internal const uint TCOFLUSH = 1;
        internal const uint TCIOFLUSH = 2;
        internal const uint TCSANOW = 0;
        internal const uint TCSADRAIN = 1;
        internal const uint TCSAFLUSH = 2;

        internal const uint BRKINT = 2;
        internal const uint IGNPAR = 4;
        internal const uint PARMRK = 8;
        internal const uint INPCK = 16;
        internal const uint ISTRIP = 32;
        internal const uint INLCR = 64;
        internal const uint IGNCR = 128;
        internal const uint ICRNL = 256;
        internal const uint IUCLC = 512;
        internal const uint IXON = 1024;
        internal const uint IXANY = 2048;
        internal const uint IXOFF = 4096;
        internal const uint IMAXBEL = 8192;
        internal const uint IUTF8 = 16384;
        internal const uint OPOST = 1;
        internal const uint OLCUC = 2;
        internal const uint ONLCR = 4;
        internal const uint OCRNL = 8;
        internal const uint ONOCR = 16;
        internal const uint ONLRET = 32;
        internal const uint OFILL = 64;
        internal const uint OFDEL = 128;
        internal const uint NLDLY = 256;
        internal const uint NL0 = 0;
        internal const uint NL1 = 256;
        internal const uint CRDLY = 1536;
        internal const uint CR0 = 0;
        internal const uint CR1 = 512;
        internal const uint CR2 = 1024;
        internal const uint CR3 = 1536;
        internal const uint TABDLY = 6144;
        internal const uint TAB0 = 0;
        internal const uint TAB1 = 2048;
        internal const uint TAB2 = 4096;
        internal const uint TAB3 = 6144;
        internal const uint BSDLY = 8192;
        internal const uint BS0 = 0;
        internal const uint BS1 = 8192;
        internal const uint FFDLY = 32768;
        internal const uint FF0 = 0;
        internal const uint FF1 = 32768;
        internal const uint VTDLY = 16384;
        internal const uint VT0 = 0;
        internal const uint VT1 = 16384;
        internal const uint B0 = 0;
        internal const uint B50 = 1;
        internal const uint B75 = 2;
        internal const uint B110 = 3;
        internal const uint B134 = 4;
        internal const uint B150 = 5;
        internal const uint B200 = 6;
        internal const uint B300 = 7;
        internal const uint B600 = 8;
        internal const uint B1200 = 9;
        internal const uint B1800 = 10;
        internal const uint B2400 = 11;
        internal const uint B4800 = 12;
        internal const uint B9600 = 13;
        internal const uint B19200 = 14;
        internal const uint B38400 = 15;
        internal const uint B57600 = 4097;
        internal const uint B115200 = 4098;
        internal const uint B230400 = 4099;
        internal const uint B460800 = 4100;
        internal const uint B500000 = 4101;
        internal const uint B576000 = 4102;
        internal const uint B921600 = 4103;
        internal const uint B1000000 = 4104;
        internal const uint B1152000 = 4105;
        internal const uint B1500000 = 4106;
        internal const uint B2000000 = 4107;
        internal const uint B2500000 = 4108;
        internal const uint B3000000 = 4109;
        internal const uint B3500000 = 4110;
        internal const uint B4000000 = 4111;
        internal const uint CSIZE = 48;
        internal const uint CS5 = 0;
        internal const uint CS6 = 16;
        internal const uint CS7 = 32;
        internal const uint CS8 = 48;
        internal const uint CSTOPB = 64;
        internal const uint CREAD = 128;
        internal const uint PARENB = 256;
        internal const uint PARODD = 512;
        internal const uint HUPCL = 1024;
        internal const uint CLOCAL = 2048;
        internal const uint ISIG = 1;
        internal const uint ICANON = 2;
        internal const uint ECHO = 8;
        internal const uint ECHOE = 16;
        internal const uint ECHOK = 32;
        internal const uint ECHONL = 64;
        internal const uint NOFLSH = 128;
        internal const uint TOSTOP = 256;
        internal const uint IEXTEN = 32768;
        internal const uint EXTA = 14;
        internal const uint EXTB = 15;
        internal const uint CBAUD = 4111;
        internal const uint CBAUDEX = 4096;
        internal const uint CIBAUD = 269418496;
        internal const uint CMSPAR = 1073741824;
        internal const uint CRTSCTS = 2147483648;
        internal const uint XCASE = 4;
        internal const uint ECHOCTL = 512;
        internal const uint ECHOPRT = 1024;
        internal const uint ECHOKE = 2048;
        internal const uint FLUSHO = 4096;
        internal const uint PENDIN = 16384;
        internal const uint EXTPROC = 65536;
        internal const uint XTABS = 6144;
    }
}
