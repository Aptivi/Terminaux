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

using Magico.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base.TermInfo.Parsing;
using Textify.General;

namespace Terminaux.Base.TermInfo
{
    /// <summary>
    /// Represents extended capabilities.
    /// </summary>
    public sealed class ExtendedCapabilities
    {
        private readonly Dictionary<string, TermInfoValueDesc<bool?>> _booleans;
        private readonly Dictionary<string, TermInfoValueDesc<int?>> _nums;
        private readonly Dictionary<string, TermInfoValueDesc<string?>> _strings;

        /// <summary>
        /// Gets the number of extended capabilities.
        /// </summary>
        public int Count { get; }

        internal ExtendedCapabilities(
            TermInfoValueDesc<bool?>[] booleans, TermInfoValueDesc<int?>[] nums, TermInfoValueDesc<string?>[] strings,
            string[] booleanNames, string[] numNames, string[] stringNames)
        {
            _booleans = booleanNames.Zip(booleans).ToDictionarySafe(x => x.First, x => x.Second);
            _nums = numNames.Zip(nums).ToDictionarySafe(x => x.First, x => x.Second);
            _strings = stringNames.Zip(strings).ToDictionarySafe(x => x.First, x => x.Second);

            Count = _booleans.Count + _nums.Count + _strings.Count;
        }

        /// <summary>
        /// Checks whether or not a specific extended capability exist.
        /// </summary>
        /// <param name="key">The extended capability key to check.</param>
        /// <returns><c>true</c> if the extended capability exist, otherwise <c>false</c>.</returns>
        public bool Exist(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            return _booleans.ContainsKey(key)
                || _nums.ContainsKey(key)
                || _strings.ContainsKey(key);
        }

        /// <summary>
        /// Gets the names for the specified capability kind.
        /// </summary>
        /// <param name="kind">The capability kind to get the names for.</param>
        /// <returns>The names for the specified capability kind.</returns>
        public List<string> GetNames(TermInfoValueType kind) =>
            kind switch
            {
                TermInfoValueType.Boolean => new List<string>(_booleans.Keys),
                TermInfoValueType.Integer => new List<string>(_nums.Keys),
                TermInfoValueType.String => new List<string>(_strings.Keys),
                _ => throw new NotSupportedException(LanguageTools.GetLocalized("T_CT_TERMINFO_EXCEPTION_CAPABILITYTYPE").FormatString(kind)),
            };

        /// <summary>
        /// Gets a extended boolean capability.
        /// </summary>
        /// <param name="key">The key of the extended boolean capability.</param>
        /// <returns>The value of the extended capability, or <c>null</c> if its missing.</returns>
        public TermInfoValueDesc<bool?>? GetBoolean(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            _booleans.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Gets a extended numeric capability.
        /// </summary>
        /// <param name="key">The key of the extended numeric capability.</param>
        /// <returns>The value of the extended capability, or <c>null</c> if its missing.</returns>
        public TermInfoValueDesc<int?>? GetNum(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            _nums.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Gets a extended string capability.
        /// </summary>
        /// <param name="key">The key of the extended string capability.</param>
        /// <returns>The value of the extended capability, or <c>null</c> if its missing.</returns>
        public TermInfoValueDesc<string?>? GetString(string key)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            _strings.TryGetValue(key, out var value);
            return value;
        }

        internal ExtendedCapabilities()
        {
            _booleans = [];
            _nums = [];
            _strings = [];
        }
    }
}
