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
using Terminaux.Writer.ConsoleWriters;

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

        /// <summary>
        /// Processes the sequence and returns the processed sequence
        /// </summary>
        /// <param name="args">Arguments to pass to the sequence</param>
        /// <returns>A processed sequence that can be used with <see cref="TextWriterRaw.WriteRaw(string, object[])"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public string? ProcessSequence(params object?[]? args)
        {
            // We can't process anything that is not a string
            if (Value is null)
                return null;
            if (ValueType != TermInfoValueType.String || Value is not string || this is not TermInfoValueDesc<string> valueDesc)
                throw new TerminauxException($"Can't process parameter on non-string value type. Enum is {ValueType} and type is {Value?.GetType().Name ?? "<null>"}.");

            // Now, process the sequence with the provided arguments
            string processed = ParameterProcessor.ProcessSequenceParams(valueDesc, args);
            return processed;
        }

        internal TermInfoValueDesc(T? value, string name, TermInfoValueType valueType, ParameterInfo[]? parameters)
        {
            this.value = value;
            this.name = name;
            this.valueType = valueType;
            this.parameters = parameters;
        }
    }
}
