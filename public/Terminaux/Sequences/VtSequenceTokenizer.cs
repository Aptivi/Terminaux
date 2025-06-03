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

using System.Collections.Generic;
using Terminaux.Sequences.Builder;

namespace Terminaux.Sequences
{
    internal class VtSequenceTokenizer
    {
        private readonly char[] charRead;

        internal VtSequenceInfo[] Parse()
        {
            List<VtSequenceInfo> eventInfos = [];

            // Run a main loop
            for (int pos = 0; pos < charRead.Length; pos++)
            {
                // A VT sequence either starts with ESC or one of the following characters:
                //   - \x9B: CSI
                //   - \x9D: OSC
                //   - \x9F: APC
                //   - \x90: DCS
                //   - \x9E: PM
                // Let's start with CSI
                if (TryParseCsiSeqs(pos, out var seq, out int bytesToAdd))
                {
                    // Add this sequence
                    eventInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }
            }

            return [.. eventInfos];
        }

        private bool TryParseCsiSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC>[ or <CSI> sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || (prefix != VtSequenceBasicChars.EscapeChar && prefix != VtSequenceBasicChars.CsiChar))
                return false;
            advance++;

            // If it's ESC, we need to check the '[' prefix.
            int offset = prefix == VtSequenceBasicChars.EscapeChar ? 1 : 0;
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset, out char param) || param != '[')
                return false;
            advance++;

            // Check the parameter character
            if (offset == 1)
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset + 1, out param))
                    return false;
                advance++;
            }

            // Helper function to help with the digits
            void AdvanceDigits(ref int advance)
            {
                while (char.IsDigit(param))
                {
                    if (!VtSequenceTokenTools.TryGetChar(charRead, idx + advance, out param))
                        return;
                    if (char.IsDigit(param))
                        advance++;
                }
            }

            // Now, check to see if the parameter is numeric or not
            if (char.IsDigit(param))
            {
                // We have a digit from 0 to 9! Now, check the parameters, since we could have 10, 100, ...
                bool bailNumber = false;
                while (!bailNumber)
                {
                    AdvanceDigits(ref advance);
                    if (param != ';')
                        break;
                }
            }
            else
            {
                // We have non-digit character, so we need to check further

            }

            // Parse the sequence now
            return false;
        }

        internal VtSequenceTokenizer(char[] charRead)
        {
            this.charRead = charRead;
        }
    }
}
