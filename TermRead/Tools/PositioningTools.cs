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

using TermRead.Reader;

namespace TermRead.Tools
{
    internal static class PositioningTools
    {
        internal static void GoForward(ref TermReaderState state) =>
            GoForward(1, false, ref state);

        internal static void GoForward(int steps, ref TermReaderState state) =>
            GoForward(steps, false, ref state);

        internal static void GoForward(int steps, bool isAppend, ref TermReaderState state)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos++;
                state.currentCursorPosLeft++;
                if (state.CurrentCursorPosLeft >= ConsoleWrapper.WindowWidth)
                {
                    // Reached to the end! Wrap down!
                    state.currentCursorPosLeft = 0;
                    if (state.currentCursorPosTop < ConsoleWrapper.BufferHeight)
                        state.currentCursorPosTop++;
                    else if (isAppend)
                    {
                        // We can't increase the top position since we're at the end of buffer, so we need to set the
                        // input prompt top position to be minus one. If we can't do that again because it went before the
                        // first column in the buffer, there's nothing we can do about this.
                        if (state.InputPromptTop > 0)
                            state.inputPromptTop--;
                    }
                }
            }
        }

        internal static void GoBack(ref TermReaderState state) =>
            GoBack(1, ref state);

        internal static void GoBack(int steps, ref TermReaderState state)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos--;
                state.currentCursorPosLeft--;
                if (state.CurrentCursorPosLeft < 0)
                {
                    // Reached to the beginning! Wrap up!
                    state.currentCursorPosLeft = ConsoleWrapper.WindowWidth - 1;
                    if (state.currentCursorPosTop > 0)
                        state.currentCursorPosTop--;
                }
            }
        }

        internal static void SeekTo(int steps, ref TermReaderState state)
        {
            GoBack(state.currentTextPos, ref state);
            GoForward(steps, ref state);
        }
    }
}
