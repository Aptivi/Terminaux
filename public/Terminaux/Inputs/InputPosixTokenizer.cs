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
using System.Collections.Generic;
using Terminaux.Sequences.Builder;

namespace Terminaux.Inputs
{
    internal class InputPosixTokenizer
    {
        private readonly byte[] charRead;

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> arrowKeys = new()
        {
            { 'A', (ConsoleKey.UpArrow, 0) },
            { 'x', (ConsoleKey.UpArrow, 0) },
            { 'a', (ConsoleKey.UpArrow, ConsoleModifiers.Shift) },
            { 'B', (ConsoleKey.DownArrow, 0) },
            { 'r', (ConsoleKey.DownArrow, 0) },
            { 'b', (ConsoleKey.DownArrow, ConsoleModifiers.Shift) },
            { 'C', (ConsoleKey.RightArrow, 0) },
            { 'v', (ConsoleKey.RightArrow, 0) },
            { 'c', (ConsoleKey.RightArrow, ConsoleModifiers.Shift) },
            { 'D', (ConsoleKey.LeftArrow, 0) },
            { 't', (ConsoleKey.LeftArrow, 0) },
            { 'd', (ConsoleKey.LeftArrow, ConsoleModifiers.Shift) },
            { 'H', (ConsoleKey.Home, 0) },
            { 'F', (ConsoleKey.End, 0) },
        };

        private readonly Dictionary<char, (ConsoleKey, ConsoleModifiers)> functionKeys = new()
        {
            { 'P', (ConsoleKey.F1, 0) },
            { 'Q', (ConsoleKey.F2, 0) },
            { 'R', (ConsoleKey.F3, 0) },
            { 'S', (ConsoleKey.F4, 0) },
            { 'T', (ConsoleKey.F5, 0) },
            { 'U', (ConsoleKey.F6, 0) },
            { 'V', (ConsoleKey.F7, 0) },
            { 'W', (ConsoleKey.F8, 0) },
            { 'X', (ConsoleKey.F9, 0) },
            { 'Y', (ConsoleKey.F10, 0) },
            { 'Z', (ConsoleKey.F11, 0) },
            { '[', (ConsoleKey.F12, 0) },
        };

        internal InputEventInfo[] Parse()
        {
            List<InputEventInfo> eventInfos = [];

            // Run a main loop
            for (int pos = 0; pos < charRead.Length; pos++)
            {
                // Get a character
                byte charValue = charRead[pos];

                // TODO: This is not done yet!
                // Check to see if we've pressed ESC
                if (charValue == (byte)VtSequenceBasicChars.EscapeChar)
                {
                    // We've just obtained the escape character. We need to perform checks.
                    if (TryGetChar(pos + 1, out char prefix))
                    {
                        // The prefix might not describe a sequence. Check it.
                        if (prefix == '[' || prefix == 'O')
                        {
                            // Check for function keys
                            if (prefix == 'O' && TryGetChar(pos + 2, out char parameter) && functionKeys.TryGetValue(parameter, out var key))
                            {
                                // It's one of the function keys! Parse it.
                                var cki = new ConsoleKeyInfo('\0', key.Item1, false, false, false);
                                eventInfos.Add(new(null, cki, null));
                                pos += 2;
                                continue;
                            }

                            // It describes a sequence! In this case, distinguish it from other sequence types.
                            if (TryGetChar(pos + 2, out parameter) && arrowKeys.TryGetValue(parameter, out key))
                            {
                                // It's one of the arrow keys! Parse it.
                                var cki = new ConsoleKeyInfo('\0', key.Item1, key.Item2.HasFlag(ConsoleModifiers.Shift), false, false);
                                eventInfos.Add(new(null, cki, null));
                                pos += 2;
                                continue;
                            }
                        }
                    }
                }
            }

            return [.. eventInfos];
        }

        private bool TryGetChar(int idx, out char character)
        {
            character = '\0';
            if (idx < 0 || idx >= charRead.Length)
                return false;
            character = (char)charRead[idx];
            return true;
        }

        internal InputPosixTokenizer(byte[] charRead)
        {
            this.charRead = charRead;
        }
    }
}
