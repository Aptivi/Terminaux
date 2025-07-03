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

namespace Terminaux.Base.TermInfo.Parsing
{
    internal sealed class NullTerminatedIndexedString
    {
        private ReadOnlyMemory<char> _data;
        private ReadOnlyMemory<int> _indexes;

        public string[] GetStrings(int count, bool sliceData = false)
        {
            var result = new string[count];

            var last = 0;
            for (var i = 0; i < count; i++)
            {
                var start = _indexes.Span[i];
                if (start < 0)
                    continue;

                var end = FindNull(start);
                if (end != -1)
                {
                    result[i] = _data.Slice(start, end - start).ToString();
                    last = end + 1;
                }
                else
                {
                    throw new InvalidOperationException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_STRINGTABLE"));
                }
            }

            _indexes = _indexes.Slice(count);

            if (sliceData)
                _data = _data.Slice(last);

            return result;
        }

        private int FindNull(int start)
        {
            for (var i = start; i < _data.Length; i++)
                if (_data.Span[i] == '\0')
                    return i;
            return -1;
        }

        public NullTerminatedIndexedString(string data, int[] indexes)
        {
            _data = data.AsMemory();
            _indexes = indexes.AsMemory();
        }
    }
}
