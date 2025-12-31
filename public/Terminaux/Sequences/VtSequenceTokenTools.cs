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
using System.Collections.Generic;

namespace Terminaux.Sequences
{
    internal static class VtSequenceTokenTools
    {
        internal static int NumberizeArray(List<char> numbers)
        {
            int num = 0;
            for (int i = 0; i < numbers.Count; i++)
                num += (int)(MapDigitNum(numbers[i]) * Math.Pow(10, numbers.Count - (i + 1)));
            return num;
        }

        internal static int MapDigitNum(char digit) =>
            digit switch
            {
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                _ => 0,
            };

        internal static bool CheckChar(char[] charRead, int idx, char[] expected)
        {
            if (idx > charRead.Length)
                return false;
            char actual = charRead[idx];
            return CheckChar(actual, expected);
        }

        internal static bool CheckChar(char character, char[] expected)
        {
            // Check character one by one
            for (int i = 0; i < expected.Length; i++)
            {
                char exp = expected[i];
                if (character == exp)
                    return true;
            }
            return false;
        }

        internal static bool CheckChars(char[] charRead, int idx, char[] expected)
        {
            if (idx + expected.Length > charRead.Length)
                return false;

            // Check character one by one
            for (int i = 0; i < expected.Length; i++)
            {
                char exp = expected[i];
                char actual = charRead[idx + i];
                if (actual != exp)
                    return false;
            }

            return true;
        }

        internal static bool TryGetChar(char[] charRead, int idx, out char character)
        {
            character = '\0';
            if (idx < 0 || idx >= charRead.Length)
                return false;
            character = charRead[idx];
            return true;
        }

        internal static bool CharInRange(char ch, char start, char end) =>
            (uint)(ch - start) <= (uint)(end - start);
    }
}
