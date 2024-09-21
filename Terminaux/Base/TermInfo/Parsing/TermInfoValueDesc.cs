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

using System.Diagnostics;
using Terminaux.Base.TermInfo.Parsing.Parameters;

namespace Terminaux.Base.TermInfo.Parsing
{
    /// <summary>
    /// Description class of a terminal info value
    /// </summary>
    [DebuggerDisplay("[{ValueType}] {Name}: {Value}")]
    public class TermInfoValueDesc<T>
    {
        private readonly T? value;
        private readonly string name = "";
        private readonly TermInfoValueType valueType;
        private readonly ParameterInfo[]? parameters;

        /// <summary>
        /// A value. If there is none, it's <see langword="null"/>.
        /// </summary>
        public T? Value =>
            value;

        /// <summary>
        /// Name of this value.
        /// </summary>
        public string Name =>
            name;

        /// <summary>
        /// Type of this value
        /// </summary>
        public TermInfoValueType ValueType =>
            valueType;

        /// <summary>
        /// Parameter information for this value
        /// </summary>
        public ParameterInfo[]? Parameters =>
            parameters;

        internal TermInfoValueDesc(T? value, string name, TermInfoValueType valueType, ParameterInfo[]? parameters)
        {
            this.value = value;
            this.name = name;
            this.valueType = valueType;
            this.parameters = parameters;
        }
    }
}
