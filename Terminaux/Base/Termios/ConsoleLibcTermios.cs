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

using tcflag_t = System.UInt32;
using cc_t = System.Byte;
using speed_t = System.UInt32;

namespace Terminaux.Base.Termios
{
    internal unsafe struct ConsoleLibcTermios
    {
        internal const int NCCS = 32;

        internal tcflag_t c_iflag;
        internal tcflag_t c_oflag;
        internal tcflag_t c_cflag;
        internal tcflag_t c_lflag;
        internal cc_t c_line;
        internal fixed cc_t c_cc[NCCS];
        internal speed_t __c_ispeed;
        internal speed_t __c_ospeed;
    }
}
