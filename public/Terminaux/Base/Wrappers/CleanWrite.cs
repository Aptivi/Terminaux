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

using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.Words.Profanity;
using Textify.General;

namespace Terminaux.Base.Wrappers
{
    internal class CleanWrite : BaseConsoleWrapper
    {
        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        public override void Write(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                text = FilterProfanity(text);
                base.Write(text);
            }
        }

        /// <summary>
        /// Writes text to console (stdout)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public override void Write(string text, params object[] args)
        {
            string formatted = text.FormatString(args);
            Write(formatted);
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public override void WriteLine(string text)
        {
            Write(text);
            WriteLine();
        }

        /// <summary>
        /// Writes text to console (stdout) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public override void WriteLine(string text, params object[] args)
        {
            Write(text, args);
            WriteLine();
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        public override void WriteError(string text)
        {
            lock (TextWriterRaw.WriteLock)
            {
                text = FilterProfanity(text);
                base.WriteError(text);
            }
        }

        /// <summary>
        /// Writes text to console (stderr)
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public override void WriteError(string text, params object[] args)
        {
            string formatted = text.FormatString(args);
            WriteError(formatted);
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        public override void WriteErrorLine(string text)
        {
            WriteError(text);
            WriteErrorLine();
        }

        /// <summary>
        /// Writes text to console (stderr) with line terminator
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="args">The arguments to evaluate</param>
        public override void WriteErrorLine(string text, params object[] args)
        {
            WriteError(text, args);
            WriteErrorLine();
        }

        private string FilterProfanity(string text)
        {
            var split = VtSequenceTools.SplitVTSequences(text);
            string[] splitFiltered = [.. split];
            for (int i = 0; i < split.Length; i++)
            {
                splitFiltered[i] = ProfanityManager.FilterProfanities(split[i]);
                if (split[i] != splitFiltered[i])
                    text = text.Replace(split[i], splitFiltered[i]);
            }
            return text;
        }
    }
}
