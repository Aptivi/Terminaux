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

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Terminaux.Base.TermInfo.Parsing.Parameters;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

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
                throw new TerminauxException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_PARAMONNONSTRING").FormatString(ValueType, Value?.GetType().Name ?? "<null>"));

            // Now, process the sequence with the provided arguments
            string processed = ParameterProcessor.ProcessSequenceParams(valueDesc, args);
            return processed;
        }

        /// <summary>
        /// Renders the sequence with delay support
        /// </summary>
        /// <param name="args">Arguments to pass to the sequence processor</param>
        /// <exception cref="TerminauxException"></exception>
        public void RenderSequence(params object?[]? args)
        {
            // Process the sequence
            string processed = ProcessSequence(args) ?? "";

            // Find the delay indicators
            List<(int, string)> parts = [];
            StringBuilder partBuilder = new();
            int delay = 0;
            for (int i = 0; i < processed.Length; i++)
            {
                // Check to see if we're at the delay indicator
                char ch = processed[i];
                if (ch == '$')
                {
                    char next = i + 1 < processed.Length ? processed[i + 1] : '\0';
                    if (next == '<')
                    {
                        StringBuilder delayBuilder = new();
                        i++;
                        while (true)
                        {
                            if (++i == processed.Length)
                                throw new TerminauxException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_DELAYDESIGNATORENDED"));

                            // Add the digits or flags, as long as they're valid
                            next = processed[i];
                            if (next == '>')
                                break;
                            if (!char.IsNumber(next) && next != '*' && next != '/')
                                throw new TerminauxException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_DELAYDESIGNATORINVALID"));

                            // Ignore the flags
                            if (next == '*' || next == '/')
                                continue;

                            // Add the digit
                            delayBuilder.Append(next);
                        }

                        // Convert to integer and add the result, clearing it in the process.
                        if (!int.TryParse(delayBuilder.ToString(), out delay))
                            throw new TerminauxException(LanguageTools.GetLocalized("T_CT_PARSING_EXCEPTION_DELAYVALUENOTANUMBER"));
                        parts.Add((delay, partBuilder.ToString()));
                        partBuilder.Clear();
                        delay = 0;
                        continue;
                    }
                }

                // Now, add the character as it is
                partBuilder.Append(ch);
            }
            if (partBuilder.Length > 0)
                parts.Add((delay, partBuilder.ToString()));

            // Now, render the sequence with delays
            foreach ((int seqDelay, string sequence) in parts)
            {
                TextWriterRaw.WriteRaw(sequence);
                if (seqDelay > 0)
                    Thread.Sleep(seqDelay);
            }
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
