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
using Terminaux.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class GoRight : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
            new ConsoleKeyInfo('\u0006', ConsoleKey.F, false, false, true)
        };

        /// <inheritdoc/>
        public override void DoAction(TerminauxerState state)
        {
            // If we're at the end of the text, bail.
            if (state.CurrentTextPos == state.CurrentText.Length)
                return;

            // Just set the position one character further than the input start position!
            if (state.OneLineWrap)
            {
                string renderedText = state.CurrentText.ToString();
                int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin - state.inputPromptLeft - 1;
                string[] incompleteSentences = GetWrappedSentences(renderedText, longestSentenceLength, 0);
                PositioningTools.GoForwardOneLineWrapAware(ref state, incompleteSentences);
            }
            else
                PositioningTools.GoForward(ref state);
            ConsoleWrapperTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
        }
    }
}
