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

using System;
using static Terminaux.Base.Extensions.Native.NativeMethods;

namespace Terminaux.Base.Extensions.Native
{
    internal static partial class NativeMapping
    {
        internal static (char character, ConsoleKey consoleKey) TranslateKeysym(TermKeySym keySym) =>
            keySym switch
            {
                TermKeySym.TERMKEY_SYM_BACKSPACE => ('\u0008', ConsoleKey.Backspace),
                TermKeySym.TERMKEY_SYM_TAB => ('\u0009', ConsoleKey.Tab),
                TermKeySym.TERMKEY_SYM_ENTER => ('\r', ConsoleKey.Enter),
                TermKeySym.TERMKEY_SYM_ESCAPE => ('\u001b', ConsoleKey.Escape),
                TermKeySym.TERMKEY_SYM_SPACE => (' ', ConsoleKey.Spacebar),
                TermKeySym.TERMKEY_SYM_DEL => ('\0', ConsoleKey.Delete),
                TermKeySym.TERMKEY_SYM_UP => ('\0', ConsoleKey.UpArrow),
                TermKeySym.TERMKEY_SYM_DOWN => ('\0', ConsoleKey.DownArrow),
                TermKeySym.TERMKEY_SYM_LEFT => ('\0', ConsoleKey.LeftArrow),
                TermKeySym.TERMKEY_SYM_RIGHT => ('\0', ConsoleKey.RightArrow),
                TermKeySym.TERMKEY_SYM_BEGIN => ('\0', ConsoleKey.Home),
                TermKeySym.TERMKEY_SYM_INSERT => ('\0', ConsoleKey.Insert),
                TermKeySym.TERMKEY_SYM_DELETE => ('\0', ConsoleKey.Delete),
                TermKeySym.TERMKEY_SYM_PAGEUP => ('\0', ConsoleKey.PageUp),
                TermKeySym.TERMKEY_SYM_PAGEDOWN => ('\0', ConsoleKey.PageDown),
                TermKeySym.TERMKEY_SYM_HOME => ('\0', ConsoleKey.Home),
                TermKeySym.TERMKEY_SYM_END => ('\0', ConsoleKey.End),
                TermKeySym.TERMKEY_SYM_CANCEL => ('\u001b', ConsoleKey.Escape),
                TermKeySym.TERMKEY_SYM_CLEAR => ('\0', ConsoleKey.Clear),
                TermKeySym.TERMKEY_SYM_KP0 => ('0', ConsoleKey.NumPad0),
                TermKeySym.TERMKEY_SYM_KP1 => ('1', ConsoleKey.NumPad1),
                TermKeySym.TERMKEY_SYM_KP2 => ('2', ConsoleKey.NumPad2),
                TermKeySym.TERMKEY_SYM_KP3 => ('3', ConsoleKey.NumPad3),
                TermKeySym.TERMKEY_SYM_KP4 => ('4', ConsoleKey.NumPad4),
                TermKeySym.TERMKEY_SYM_KP5 => ('5', ConsoleKey.NumPad5),
                TermKeySym.TERMKEY_SYM_KP6 => ('6', ConsoleKey.NumPad6),
                TermKeySym.TERMKEY_SYM_KP7 => ('7', ConsoleKey.NumPad7),
                TermKeySym.TERMKEY_SYM_KP8 => ('8', ConsoleKey.NumPad8),
                TermKeySym.TERMKEY_SYM_KP9 => ('9', ConsoleKey.NumPad9),
                TermKeySym.TERMKEY_SYM_KPENTER => ('\r', ConsoleKey.Enter),
                TermKeySym.TERMKEY_SYM_KPPLUS => ('+', ConsoleKey.Add),
                TermKeySym.TERMKEY_SYM_KPMINUS => ('-', ConsoleKey.Subtract),
                TermKeySym.TERMKEY_SYM_KPMULT => ('*', ConsoleKey.Multiply),
                TermKeySym.TERMKEY_SYM_KPDIV => ('/', ConsoleKey.Divide),
                TermKeySym.TERMKEY_SYM_KPCOMMA => (',', ConsoleKey.OemComma),
                TermKeySym.TERMKEY_SYM_KPPERIOD => ('.', ConsoleKey.Decimal),
                TermKeySym.TERMKEY_SYM_KPEQUALS => ('=', ConsoleKey.OemPlus),
                _ => ('\0', 0),
            };

        internal static ConsoleKey TranslateFunction(int number) =>
            number switch
            {
                1 => ConsoleKey.F1,
                2 => ConsoleKey.F2,
                3 => ConsoleKey.F3,
                4 => ConsoleKey.F4,
                5 => ConsoleKey.F5,
                6 => ConsoleKey.F6,
                7 => ConsoleKey.F7,
                8 => ConsoleKey.F8,
                9 => ConsoleKey.F9,
                10 => ConsoleKey.F10,
                11 => ConsoleKey.F11,
                12 => ConsoleKey.F12,
                13 => ConsoleKey.F13,
                14 => ConsoleKey.F14,
                15 => ConsoleKey.F15,
                16 => ConsoleKey.F16,
                17 => ConsoleKey.F17,
                18 => ConsoleKey.F18,
                19 => ConsoleKey.F19,
                20 => ConsoleKey.F20,
                21 => ConsoleKey.F21,
                22 => ConsoleKey.F22,
                23 => ConsoleKey.F23,
                24 => ConsoleKey.F24,
                _ => 0,
            };
    }
}
