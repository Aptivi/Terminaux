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

#if !NET5_0
using System.Collections.Generic;
using System.Linq;
using Terminaux.TermInfo.Extensions;

namespace Terminaux.TermInfo.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(
            this IEnumerable<TFirst> source, IEnumerable<TSecond> first)
        {
            return source.Zip(first, (first, second) => (first, second));
        }
    }
}
#endif