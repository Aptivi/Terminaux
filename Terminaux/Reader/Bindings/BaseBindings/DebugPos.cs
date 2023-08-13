
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class DebugPos : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\0', ConsoleKey.D, true, true, true)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Show debug background positions
            Console.BackgroundColor = ConsoleColor.Green;
            ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);
            Console.BackgroundColor = ConsoleColor.Red;
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences and debug the positions
            int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
            string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
            renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
            ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            if (state.OneLineWrap)
                PositioningTools.SeekToOneLineWrapAware(renderedText.Length, ref state);
            else
                PositioningTools.SeekTo(renderedText.Length, ref state);
            Console.BackgroundColor = ConsoleColor.Blue;
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);

            // Verify seek to 0
            if (state.OneLineWrap)
                PositioningTools.SeekToOneLineWrapAware(0, ref state);
            else
                PositioningTools.SeekTo(0, ref state);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);

            // Verify going forward 5 times
            if (state.OneLineWrap)
                PositioningTools.GoForwardOneLineWrapAware(5, ref state);
            else
                PositioningTools.GoForward(5, ref state);
            Console.BackgroundColor = ConsoleColor.Magenta;
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);

            // Verify going backward 3 times
            if (state.OneLineWrap)
                PositioningTools.GoBackOneLineWrapAware(3, ref state);
            else
                PositioningTools.GoBack(3, ref state);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleTools.ActionWriteString(" ", state.settings);
            Thread.Sleep(1000);

            // Now, reset everything
            ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            Console.ResetColor();
            if (state.OneLineWrap)
            {
                ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                PositioningTools.SeekToOneLineWrapAware(renderedText.Length, ref state);
            }
            else
            {
                ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleTools.ActionWriteString(renderedText + " ", state.settings);
                PositioningTools.SeekTo(renderedText.Length, ref state);
            }
            ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
