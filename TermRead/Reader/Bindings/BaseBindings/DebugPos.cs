/*
 * MIT License
 *
 * Copyright (c) 2022-2023 Aptivi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */

using System;
using System.Threading;
using TermRead.Tools;

namespace TermRead.Reader.Bindings.BaseBindings
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
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);
            Console.BackgroundColor = ConsoleColor.Red;
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);

            // Re-write the text and set the current cursor position as appropriate
            string renderedText = state.PasswordMode ? new string(TermReaderSettings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

            // In the case of one line wrap, get the list of sentences and debug the positions
            int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TermReaderSettings.RightMargin - state.inputPromptLeft - 1;
            string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
            renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            if (state.OneLineWrap)
                PositioningTools.SeekToOneLineWrapAware(renderedText.Length, ref state, incompleteSentences);
            else
                PositioningTools.SeekTo(renderedText.Length, ref state);
            Console.BackgroundColor = ConsoleColor.Blue;
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);

            // Verify seek to 0
            if (state.OneLineWrap)
                PositioningTools.SeekToOneLineWrapAware(0, ref state, incompleteSentences);
            else
                PositioningTools.SeekTo(0, ref state);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);

            // Verify going forward 5 times
            if (state.OneLineWrap)
                PositioningTools.GoForwardOneLineWrapAware(5, ref state, incompleteSentences);
            else
                PositioningTools.GoForward(5, ref state);
            Console.BackgroundColor = ConsoleColor.Magenta;
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);

            // Verify going backward 3 times
            if (state.OneLineWrap)
                PositioningTools.GoBackOneLineWrapAware(3, ref state, incompleteSentences);
            else
                PositioningTools.GoBack(3, ref state);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            ConsoleWrapperTools.ActionWriteString(" ");
            Thread.Sleep(1000);

            // Now, reset everything
            ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
            Console.ResetColor();
            if (state.OneLineWrap)
            {
                ConsoleWrapperTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
                PositioningTools.SeekToOneLineWrapAware(renderedText.Length, ref state, incompleteSentences);
            }
            else
            {
                ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                ConsoleWrapperTools.ActionWriteString(renderedText + " ");
                PositioningTools.SeekTo(renderedText.Length, ref state);
            }
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
