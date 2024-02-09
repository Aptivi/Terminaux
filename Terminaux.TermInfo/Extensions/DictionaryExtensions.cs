//
// Terminaux.TermInfo  Copyright (C) 2023-2024  Aptivi
//
// This file is part of Terminaux.TermInfo
//
// Terminaux.TermInfo is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux.TermInfo is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;

namespace Terminaux.TermInfo.Extensions
{
    internal static class DictionaryExtensions
    {
        public static Dictionary<TKey, TElement> ToDictionarySafe<TSource, TKey, TElement>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> valueSelector)
            where TKey : notnull
        {
            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = valueSelector(item);

                dictionary[key] = value;
            }

            return dictionary;
        }
    }
}
