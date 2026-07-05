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

namespace Terminaux.Base.Extensions.Data
{
    //
    // ========== CODE ASSISTED BY Claude Sonnet 5 Medium WITH HUMAN REVIEW
    //
    internal static class GraphemeCluster
    {
        private const char Zwj = '\u200D';
        private const char Vs15 = '\uFE0E';
        private const char Vs16 = '\uFE0F';
        private const char KeycapCombiner = '\u20E3';
        private const int ModifierRangeStart = 0x1F3FB;
        private const int ModifierRangeEnd = 0x1F3FF;
        private const int RegionalIndicatorStart = 0x1F1E6;
        private const int RegionalIndicatorEnd = 0x1F1FF;
        private const int TagStart = 0xE0020;
        private const int TagEnd = 0xE007E;
        private const int TagCancel = 0xE007F;

        public static int GetLength(string text, int pos)
        {
            if (pos >= text.Length)
                return 0;

            int idx = pos + ReadSegmentForward(text, pos);
            while (idx < text.Length && text[idx] == Zwj && idx + 1 < text.Length)
            {
                idx++;
                idx += ReadSegmentForward(text, idx);
            }
            return idx - pos;
        }

        public static int GetStart(string text, int index)
        {
            if (index <= 0)
                return 0;

            int pos = 0;
            while (pos < text.Length)
            {
                int len = GetLength(text, pos);
                if (len <= 0)
                    len = 1;

                if (index < pos + len)
                    return pos;

                pos += len;
            }
            return pos;
        }

        public static int GetLengthBackward(string text, int pos)
        {
            if (pos <= 0)
                return 0;
            int start = GetStart(text, pos - 1);
            return pos - start;
        }

        private static int ReadSegmentForward(string text, int pos)
        {
            if (pos >= text.Length)
                return 0;

            int len = (pos + 1 < text.Length && char.IsSurrogatePair(text[pos], text[pos + 1])) ? 2 : 1;
            int idx = pos + len;

            // Flags: two regional indicators paired together, nothing else attaches
            if (len == 2 && IsRegionalIndicatorPair(text, pos))
            {
                int clusterStart = GetRegionalIndicatorClusterStart(text, pos);
                if (clusterStart == pos)
                    return GetRegionalIndicatorClusterLength(text, clusterStart);
                return 2;
            }

            // Skin-tone modifier attaches directly to the base
            if (IsModifierPair(text, idx))
                idx += 2;

            // Tag sequence: base emoji + run of tag chars + cancel tag
            if (IsTagCharPair(text, idx))
            {
                while (IsTagCharPair(text, idx))
                    idx += 2;
                if (IsTagCancelPair(text, idx))
                    idx += 2;
            }

            // Variation selector, then optional keycap combiner
            if (idx < text.Length && (text[idx] == Vs15 || text[idx] == Vs16))
            {
                idx++;
                if (idx < text.Length && text[idx] == KeycapCombiner)
                    idx++;
            }
            else if (idx < text.Length && text[idx] == KeycapCombiner)
            {
                // Keycap without an explicit VS16 (rare but valid)
                idx++;
            }

            return idx - pos;
        }
        
        private static int GetRegionalIndicatorRunStart(string text, int pos)
        {
            int start = pos;
            while (start - 2 >= 0 && IsRegionalIndicatorPair(text, start - 2))
                start -= 2;
            return start;
        }

        private static int GetRegionalIndicatorClusterStart(string text, int posInRun)
        {
            int runStart = GetRegionalIndicatorRunStart(text, posInRun);
            int k = (posInRun - runStart) / 2;
            int pairIndex = k / 2;
            return runStart + pairIndex * 4;
        }

        private static int GetRegionalIndicatorClusterLength(string text, int clusterStart) =>
            (clusterStart + 2 < text.Length && IsRegionalIndicatorPair(text, clusterStart + 2)) ? 4 : 2;

        private static bool IsPairInRange(string text, int idx, int lo, int hi) =>
            idx + 1 < text.Length && char.IsSurrogatePair(text[idx], text[idx + 1]) &&
            char.ConvertToUtf32(text[idx], text[idx + 1]) is int cp && cp >= lo && cp <= hi;

        private static bool IsModifierPair(string text, int idx) =>
            IsPairInRange(text, idx, ModifierRangeStart, ModifierRangeEnd);

        private static bool IsRegionalIndicatorPair(string text, int idx) =>
            IsPairInRange(text, idx, RegionalIndicatorStart, RegionalIndicatorEnd);

        private static bool IsTagCharPair(string text, int idx) =>
            IsPairInRange(text, idx, TagStart, TagEnd);

        private static bool IsTagCancelPair(string text, int idx) =>
            IsPairInRange(text, idx, TagCancel, TagCancel);
    }
    //
    // ========== CODE ASSISTED BY Claude Sonnet 5 Medium WITH HUMAN REVIEW
    //
}
