//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Linq;
using System.Threading;
using Terminaux.Base.Structures;
using Terminaux.Sequences;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Base.Wrappers
{
    internal class FileWrite : BaseConsoleWrapper
    {

        internal string PathToWrite { get; set; } = "";
        internal bool FilterVT { get; set; }

        public override bool IsDumb => true;

        public override int CursorLeft { get => 0; set => throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED")); }

        public override int CursorTop { get => 0; set => throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED")); }

        public override Coordinate GetCursorPosition => new(CursorLeft, CursorTop);

        public override int WindowWidth => 0;

        public override int WindowHeight => 0;

        public override int BufferWidth => 0;

        public override int BufferHeight => 0;

        public override bool CursorVisible { set => throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED")); }

        public override bool KeyAvailable =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void Beep() =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void BeepCustom(int freq, int ms) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void BeepSeq() =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void Clear() =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void ClearLoadBack() =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override bool TreatCtrlCAsInput =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override ConsoleKeyInfo ReadKey(bool intercept = false) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetCursorPosition(int left, int top) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetCursorLeft(int left) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetCursorTop(int top) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetWindowDimensions(int width, int height) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetBufferDimensions(int width, int height) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetWindowWidth(int width) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetWindowHeight(int height) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetBufferWidth(int width) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        public override void SetBufferHeight(int height) =>
            throw new TerminauxException(LanguageTools.GetLocalized("T_EXCEPTION_NOTIMPLEMENTED"));

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void Write(char value) =>
            WritePlain($"{value}", false, []);

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text) =>
            WritePlain(text, false, []);

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void Write(string text, params object[] args) =>
            WritePlain(text, false, args);

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine() =>
            WritePlain("", true, []);

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text) =>
            WritePlain(text, true, []);

        /// <summary>
        /// Outputs text to file
        /// </summary>
        /// <inheritdoc/>
        public override void WriteLine(string text, params object[] args) =>
            WritePlain(text, true, args);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteError(char value) =>
            WritePlain($"{value}", false, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteError(string text) =>
            WritePlain(text, false, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteError(string text, params object[] args) =>
            WritePlain(text, false, args);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteErrorLine() =>
            WritePlain("", true, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteErrorLine(string text) =>
            WritePlain(text, true, []);

        /// <summary>
        /// Outputs text to the buffered string builder
        /// </summary>
        /// <inheritdoc/>
        public override void WriteErrorLine(string text, params object[] args) =>
            WritePlain(text, true, args);

        internal void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterRaw.WriteLock)
            {
                // If filtering, filter all VT sequences
                if (FilterVT)
                    Text = VtSequenceTools.FilterVTSequences(Text);

                // Open the stream
                StreamWriter fileWriter = new(PathToWrite, true) { AutoFlush = true };
                try
                {
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            fileWriter.WriteLine(Text, vars);
                        }
                        else
                        {
                            fileWriter.WriteLine(Text);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        fileWriter.Write(Text, vars);
                    }
                    else
                    {
                        fileWriter.Write(Text);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    ConsoleLogger.Error(ex, "There is a serious error when printing text. {0}", ex.Message);
                }
                fileWriter.Close();
            }
        }
    }
}
