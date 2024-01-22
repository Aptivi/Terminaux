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
using System.Threading;
using Terminaux.Base;
using Terminaux.Reader.Tools;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class DebugPos : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\0', ConsoleKey.D, true, true, true)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Show debug background positions
            Console.ForegroundColor = ConsoleColor.Black;

            // state.InputPromptLeft, state.InputPromptTop
            Console.BackgroundColor = ConsoleColor.Green;
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            TextWriterColor.WritePlain("I", state.settings);
            Thread.Sleep(1000);

            // state.CurrentCursorPosLeft, state.CurrentCursorPosTop
            Console.BackgroundColor = ConsoleColor.Red;
            PositioningTools.Commit(state);
            TextWriterColor.WritePlain("C", state.settings);
            Thread.Sleep(1000);

            // state.LeftMargin, state.InputPromptTop
            // Targets: state.MaximumInputPositionLeft, state.LongestSentenceLengthFromLeft
            Console.BackgroundColor = ConsoleColor.Yellow;
            ConsoleWrapper.SetCursorPosition(state.LeftMargin, state.InputPromptTop);
            TextWriterColor.WritePlain("|", state.settings);
            ConsoleWrapper.SetCursorPosition(state.MaximumInputPositionLeft, state.InputPromptTop);
            TextWriterColor.WritePlain("M", state.settings);
            ConsoleWrapper.SetCursorPosition(state.LongestSentenceLengthFromLeft >= ConsoleWrapper.WindowWidth ? state.LongestSentenceLengthFromLeft - 1 : state.LongestSentenceLengthFromLeft, state.InputPromptTop);
            TextWriterColor.WritePlain("L", state.settings);

            // 0, state.InputPromptTop
            // Targets: state.LongestSentenceLengthFromLeftForFirstLine, state.LongestSentenceLengthFromLeftForGeneralLine
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            ConsoleWrapper.SetCursorPosition(0, state.InputPromptTop);
            TextWriterColor.WritePlain("#", state.settings);
            ConsoleWrapper.SetCursorPosition(state.LongestSentenceLengthFromLeftForFirstLine, state.InputPromptTop);
            TextWriterColor.WritePlain("F", state.settings);
            ConsoleWrapper.SetCursorPosition(state.LongestSentenceLengthFromLeftForGeneralLine, state.InputPromptTop);
            TextWriterColor.WritePlain("G", state.settings);
            Thread.Sleep(1000);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences and debug the positions
            int longestSentenceLength = state.LongestSentenceLengthFromLeftForFirstLine;
            string[] incompleteSentences = TextTools.GetWrappedSentences(renderedText, longestSentenceLength, 0);
            renderedText = state.OneLineWrap ? TermReaderTools.GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            PositioningTools.SeekTo(renderedText.Length, ref state);
            Console.BackgroundColor = ConsoleColor.Blue;
            PositioningTools.Commit(state);
            TextWriterColor.WritePlain("S", state.settings);
            Thread.Sleep(1000);

            // Verify seek to 0
            PositioningTools.SeekTo(0, ref state);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            PositioningTools.Commit(state);
            TextWriterColor.WritePlain("Z", state.settings);
            Thread.Sleep(1000);

            // Verify going forward 5 times
            PositioningTools.GoForward(5, ref state);
            Console.BackgroundColor = ConsoleColor.Magenta;
            PositioningTools.Commit(state);
            TextWriterColor.WritePlain("5", state.settings);
            Thread.Sleep(1000);

            // Verify going backward 3 times
            PositioningTools.GoBack(3, ref state);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            PositioningTools.Commit(state);
            TextWriterColor.WritePlain("3", state.settings);
            Thread.Sleep(1000);

            // Now, reset everything
            ConsoleWrapper.SetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            Console.ResetColor();
            TermReaderTools.RefreshPrompt(ref state);
        }
    }
}
