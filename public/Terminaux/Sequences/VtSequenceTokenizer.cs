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
using System.Text;
using Terminaux.Sequences.Builder;

namespace Terminaux.Sequences
{
    internal class VtSequenceTokenizer
    {
        private readonly char[] charRead;

        internal VtSequenceInfo[] Parse(VtSequenceType type = VtSequenceType.All)
        {
            if (type == VtSequenceType.None)
                return [];
            List<VtSequenceInfo> sequenceInfos = [];

            // First, check to see if we have ESC sequence characters
            bool hasSequences = false;
            for (int pos = 0; pos < charRead.Length; pos++)
            {
                hasSequences =
                    VtSequenceTokenTools.CheckChar(charRead, pos,
                    [
                        VtSequenceBasicChars.EscapeChar,
                        VtSequenceBasicChars.CsiChar,
                        VtSequenceBasicChars.OSCChar,
                        VtSequenceBasicChars.APCChar,
                        VtSequenceBasicChars.DCSChar,
                        VtSequenceBasicChars.PMChar,
                    ]);
                if (hasSequences)
                    break;
            }
            if (!hasSequences)
                return [];

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
                    if (type.HasFlag(VtSequenceType.Csi))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse OSC sequences
                if (TryParseOscSeqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.Osc))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse ESC sequences
                if (TryParseEscSeqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.Esc))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse APC sequences
                if (TryParseApcSeqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.Apc))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse DCS sequences
                if (TryParseDcsSeqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.Dcs))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse PM sequences
                if (TryParsePmSeqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.Pm))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }

                // Parse C1 sequences
                if (TryParseC1Seqs(pos, out seq, out bytesToAdd))
                {
                    // Add this sequence
                    if (type.HasFlag(VtSequenceType.C1))
                        sequenceInfos.Add(seq);
                    pos += bytesToAdd - 1;
                    continue;
                }
            }

            return [.. sequenceInfos];
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

            // Now, check to see if the parameter is numeric or not, and assign a type
            var sequenceType =
                char.IsDigit(param) ? VtSequenceStartType.Numeric :
                char.IsLetter(param) ? VtSequenceStartType.Alphabetic :
                VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the parameters first
            var parameters = new StringBuilder();
            var intermediates = new StringBuilder();
            char ending = '\0';
            int increment = 0;
            bool result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            while (result)
            {
                if (param >= 0x30 && param <= 0x3F)
                {
                    parameters.Append(param);
                    increment++;
                }
                else if (param >= 0x20 && param <= 0x2F)
                {
                    intermediates.Append(param);
                    increment++;
                }
                else if (param >= 0x40 && param <= 0x7E)
                {
                    ending = param;
                    increment++;
                    break;
                }
                result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            }
            if (!result)
                return false;

            // Build the sequence now
            char[] finalChars = new char[advance + increment - 1];
            Array.Copy(charRead, idx, finalChars, 0, advance + increment - 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Csi, sequenceType, offset == 1 ? $"{VtSequenceBasicChars.EscapeChar}[" : $"{VtSequenceBasicChars.CsiChar}", parameters.ToString(), intermediates.ToString(), finalSeq, ending, idx);
            advance += increment - 1;
            return true;
        }

        private bool TryParseOscSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC>] or <OSC> sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || (prefix != VtSequenceBasicChars.EscapeChar && prefix != VtSequenceBasicChars.OSCChar))
                return false;
            advance++;

            // If it's ESC, we need to check the ']' prefix.
            int offset = prefix == VtSequenceBasicChars.EscapeChar ? 1 : 0;
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset, out char param) || param != ']')
                return false;
            advance++;

            // Check the parameter character
            if (offset == 1)
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset + 1, out _))
                    return false;
                advance++;
            }

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the parameter
            var parameters = new StringBuilder();
            char ending = '\0';
            int increment = 0;
            bool result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            while (result)
            {
                if (param == VtSequenceBasicChars.BellChar || param == VtSequenceBasicChars.StChar)
                {
                    ending = param;
                    increment++;
                    break;
                }
                else
                {
                    parameters.Append(param);
                    increment++;
                }
                result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            }
            if (!result)
                return false;

            // Build the sequence now
            char[] finalChars = new char[advance + increment - 1];
            Array.Copy(charRead, idx, finalChars, 0, advance + increment - 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Osc, sequenceType, offset == 1 ? $"{VtSequenceBasicChars.EscapeChar}]" : $"{VtSequenceBasicChars.OSCChar}", parameters.ToString(), "", finalSeq, ending, idx);
            advance += increment - 1;
            return true;
        }

        private bool TryParseEscSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC> sequences, and advance one byte.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || prefix != VtSequenceBasicChars.EscapeChar)
                return false;
            advance++;

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the sequence
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + 1, out char param))
                return false;
            advance++;

            // Those sequences may need an additional letter, so parse it, too.
            bool found = false;
            if (VtSequenceTokenTools.CheckChar(param, [' ', '#', '%', '(', ')', '*', '+', '-', ',', '/']))
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + 2, out char argParam))
                    return false;
                advance++;

                if (param == ' ')
                {
                    // Check the parameter after the ' ' parameter
                    if (!VtSequenceTokenTools.CheckChar(argParam, ['F', 'G', 'L', 'M', 'N']))
                        return false;
                    found = true;
                }
                else if (param == '#')
                {
                    // Check the parameter after the '#' parameter
                    if (!VtSequenceTokenTools.CheckChar(argParam, ['3', '4', '5', '6', '8']))
                        return false;
                    found = true;
                }
                else if (param == '%')
                {
                    // Check the parameter after the '%' parameter
                    if (!VtSequenceTokenTools.CheckChar(argParam, ['@', 'G']))
                        return false;
                    found = true;
                }
            }

            // Check for those sequences, too
            if (!found && !VtSequenceTokenTools.CheckChar(param, ['6', '7', '8', '9', '=', '>', 'F', 'c', 'l', 'm', 'n', 'o', '}', '|', '~']))
                return false;

            // Build the sequence now
            char[] finalChars = new char[idx + advance - sequenceStart + 1];
            Array.Copy(charRead, idx, finalChars, 0, idx + advance - sequenceStart + 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Esc, sequenceType, $"{VtSequenceBasicChars.EscapeChar}", "", "", finalSeq, '\0', idx);
            return true;
        }

        private bool TryParseApcSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC>_ or <APC> sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || (prefix != VtSequenceBasicChars.EscapeChar && prefix != VtSequenceBasicChars.APCChar))
                return false;
            advance++;

            // If it's ESC, we need to check the '_' prefix.
            int offset = prefix == VtSequenceBasicChars.EscapeChar ? 1 : 0;
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset, out char param) || param != '_')
                return false;
            advance++;

            // Check the parameter character
            if (offset == 1)
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset + 1, out _))
                    return false;
                advance++;
            }

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the parameter
            var parameters = new StringBuilder();
            char ending = '\0';
            int increment = 0;
            bool result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            while (result)
            {
                if (param == VtSequenceBasicChars.StChar)
                {
                    ending = param;
                    increment++;
                    break;
                }
                else
                {
                    parameters.Append(param);
                    increment++;
                }
                result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            }
            if (!result)
                return false;

            // Build the sequence now
            char[] finalChars = new char[advance + increment - 1];
            Array.Copy(charRead, idx, finalChars, 0, advance + increment - 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Apc, sequenceType, offset == 1 ? $"{VtSequenceBasicChars.EscapeChar}]" : $"{VtSequenceBasicChars.APCChar}", parameters.ToString(), "", finalSeq, ending, idx);
            advance += increment - 1;
            return true;
        }

        private bool TryParseDcsSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC>P or <DCS> sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || (prefix != VtSequenceBasicChars.EscapeChar && prefix != VtSequenceBasicChars.DCSChar))
                return false;
            advance++;

            // If it's ESC, we need to check the 'P' prefix.
            int offset = prefix == VtSequenceBasicChars.EscapeChar ? 1 : 0;
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset, out char param) || param != 'P')
                return false;
            advance++;

            // Check the parameter character
            if (offset == 1)
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset + 1, out _))
                    return false;
                advance++;
            }

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the parameter
            var parameters = new StringBuilder();
            char ending = '\0';
            int increment = 0;
            bool result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            while (result)
            {
                if (param == VtSequenceBasicChars.StChar)
                {
                    ending = param;
                    increment++;
                    break;
                }
                else
                {
                    parameters.Append(param);
                    increment++;
                }
                result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            }
            if (!result)
                return false;

            // Build the sequence now
            char[] finalChars = new char[advance + increment - 1];
            Array.Copy(charRead, idx, finalChars, 0, advance + increment - 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Dcs, sequenceType, offset == 1 ? $"{VtSequenceBasicChars.EscapeChar}]" : $"{VtSequenceBasicChars.DCSChar}", parameters.ToString(), "", finalSeq, ending, idx);
            advance += increment - 1;
            return true;
        }

        private bool TryParsePmSeqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC>^ or <PM> sequences, and advance two bytes to indicate that we've seen
            // the two characters before the prefix.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || (prefix != VtSequenceBasicChars.EscapeChar && prefix != VtSequenceBasicChars.PMChar))
                return false;
            advance++;

            // If it's ESC, we need to check the '^' prefix.
            int offset = prefix == VtSequenceBasicChars.EscapeChar ? 1 : 0;
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset, out char param) || param != '^')
                return false;
            advance++;

            // Check the parameter character
            if (offset == 1)
            {
                if (!VtSequenceTokenTools.TryGetChar(charRead, idx + offset + 1, out _))
                    return false;
                advance++;
            }

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the parameter
            var parameters = new StringBuilder();
            char ending = '\0';
            int increment = 0;
            bool result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            while (result)
            {
                if (param == VtSequenceBasicChars.StChar)
                {
                    ending = param;
                    increment++;
                    break;
                }
                else
                {
                    parameters.Append(param);
                    increment++;
                }
                result = VtSequenceTokenTools.TryGetChar(charRead, sequenceStart + increment - 1, out param);
            }
            if (!result)
                return false;

            // Build the sequence now
            char[] finalChars = new char[advance + increment - 1];
            Array.Copy(charRead, idx, finalChars, 0, advance + increment - 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.Pm, sequenceType, offset == 1 ? $"{VtSequenceBasicChars.EscapeChar}]" : $"{VtSequenceBasicChars.PMChar}", parameters.ToString(), "", finalSeq, ending, idx);
            advance += increment - 1;
            return true;
        }

        private bool TryParseC1Seqs(int idx, out VtSequenceInfo seq, out int advance)
        {
            // Set initial values
            seq = new();
            advance = 0;

            // Check to see if we have <ESC> sequences, and advance one byte.
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx, out char prefix) || prefix != VtSequenceBasicChars.EscapeChar)
                return false;
            advance++;

            // Parse until the end
            var sequenceType = VtSequenceStartType.Special;
            int sequenceStart = idx + advance;

            // Parse the sequence
            if (!VtSequenceTokenTools.TryGetChar(charRead, idx + 1, out char param))
                return false;
            advance++;

            // Check for the sequence
            if (!VtSequenceTokenTools.CheckChar(param, ['D', 'E', 'H', 'M', 'N', 'O', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_']))
                return false;

            // Build the sequence now
            char[] finalChars = new char[idx + advance - sequenceStart + 1];
            Array.Copy(charRead, idx, finalChars, 0, idx + advance - sequenceStart + 1);
            string finalSeq = new(finalChars);

            // Make a VT sequence instance
            seq = new VtSequenceInfo(VtSequenceType.C1, sequenceType, $"{VtSequenceBasicChars.EscapeChar}", "", "", finalSeq, '\0', idx);
            return true;
        }

        internal VtSequenceTokenizer(char[] charRead)
        {
            this.charRead = charRead;
        }
    }
}
