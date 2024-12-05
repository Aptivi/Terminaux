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

using System;
using System.Collections;
using System.Linq;

namespace Terminaux.Extensions.Enumerations
{
    /// <summary>
    /// Enumerable extensions. This is taken from Magico, but will be updated accordingly.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Counts the elements found in this enumerable
        /// </summary>
        /// <param name="enumerable">The target enumerable to count its elements</param>
        /// <returns>A number of elements</returns>
        public static int Length(this IEnumerable enumerable)
        {
            // First, focus on the known types
            if (enumerable is Array arrayEnumerable)
            {
                // It's an array!
                return arrayEnumerable.Length;
            }
            else if (enumerable is IList listEnumerable)
            {
                // It's a list!
                return listEnumerable.Count;
            }
            else if (enumerable is IDictionary dictionaryEnumerable)
            {
                // It's a dictionary!
                return dictionaryEnumerable.Count;
            }
            else if (enumerable is ICollection collectionEnumerable)
            {
                // It's a collection!
                return collectionEnumerable.Count;
            }

            // We're in the unknown IEnumerable.
            var generic = enumerable.OfType<object>();
            return generic.Count();
        }

        /// <summary>
        /// Gets an element from the index
        /// </summary>
        /// <param name="enumerable">The target enumerable to get an element</param>
        /// <param name="index">Zero-based index number of an element</param>
        /// <returns>An element from this enumerable</returns>
        public static object? GetElementFromIndex(this IEnumerable enumerable, int index)
        {
            if (index < 0)
                return null;

            // First, focus on the known types
            if (enumerable is Array arrayEnumerable)
            {
                // It's an array!
                return arrayEnumerable.GetValue(index);
            }
            else if (enumerable is IList listEnumerable)
            {
                // It's a list!
                return listEnumerable[index];
            }
            else if (enumerable is IDictionary dictionaryEnumerable)
            {
                // It's a dictionary!
                var keys = new object[dictionaryEnumerable.Count];
                dictionaryEnumerable.Keys.CopyTo(keys, 0);
                var key = keys[index];
                return dictionaryEnumerable[key];
            }
            else if (enumerable is ICollection collectionEnumerable)
            {
                var collection = collectionEnumerable.OfType<object>();
                return collection.ElementAt(index);
            }

            // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
            var generic = enumerable.OfType<object>();
            return generic.ElementAt(index);
        }
    }
}
