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

using Terminaux.TermInfo;

namespace Terminaux.TermInfo
{
    /// <summary>
    /// Represents a parsed terminfo description.
    /// </summary>
    public sealed partial class TermInfoDesc
    {
        private readonly string[] _names;
        private readonly bool?[] _booleans;
        private readonly int?[] _nums;
        private readonly string?[] _strings;

        /// <summary>
        /// Gets the names of the parsed terminfo description.
        /// </summary>
        public string[] Names => _names;

        /// <summary>
        /// Gets the extended capabilities.
        /// </summary>
        public ExtendedCapabilities Extended { get; }

        internal TermInfoDesc(
            string[] names, bool?[] booleans, int?[] nums,
            string?[] strings, ExtendedCapabilities? extended = null)
        {
            _names = names;
            _booleans = booleans;
            _nums = nums;
            _strings = strings;

            Extended = extended ?? new ExtendedCapabilities();
        }

        /// <summary>
        /// Gets a specific boolean terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public bool? GetBoolean(TermInfoCaps.Boolean value)
        {
            var index = (int)value;
            if (index >= _booleans.Length)
            {
                return null;
            }

            return _booleans[index];
        }

        /// <summary>
        /// Gets a specific numeric terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public int? GetNum(TermInfoCaps.Num value)
        {
            var index = (int)value;
            if (index >= _nums.Length)
            {
                return null;
            }

            var result = _nums[index];
            if (result == null || result == -1)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Gets a specific string terminfo capability value.
        /// </summary>
        /// <param name="value">The capability to get the value for.</param>
        /// <returns>The terminfo capability value.</returns>
        public string? GetString(TermInfoCaps.String value)
        {
            var index = (int)value;
            if (index >= _strings.Length)
            {
                return null;
            }

            var result = _strings[index];
            return result;
        }
    }
}
