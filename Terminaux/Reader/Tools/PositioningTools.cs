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

using Terminaux.Reader;
using Terminaux.Reader.Bindings;

namespace Terminaux.Tools
{
    internal static class PositioningTools
    {
        internal static void GoForward(ref TerminauxerState state) =>
            GoForward(1, false, ref state);

        internal static void GoForward(int steps, ref TerminauxerState state) =>
            GoForward(steps, false, ref state);

        internal static void GoForward(int steps, bool isAppend, ref TerminauxerState state)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos++;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(TerminauxerSettings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft++;
                if (state.CurrentCursorPosLeft >= ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin)
                {
                    // Reached to the end! Wrap down!
                    state.currentCursorPosLeft = TerminauxerSettings.LeftMargin;
                    if (state.currentCursorPosTop < ConsoleWrapperTools.ActionBufferHeight())
                        state.currentCursorPosTop++;
                }

                // Check to see if we're at the end of the buffer in append mode
                if (state.currentCursorPosTop >= ConsoleWrapperTools.ActionBufferHeight() && isAppend)
                {
                    // We can't increase the top position since we're at the end of buffer, so we need to set the
                    // input prompt top position to be minus one. If we can't do that again because it went before the
                    // first column in the buffer, there's nothing we can do about this.
                    if (state.InputPromptTop > 0)
                        state.inputPromptTop -= state.currentCursorPosTop - ConsoleWrapperTools.ActionBufferHeight() + 1;
                    ConsoleWrapperTools.ActionWriteLine();
                    state.currentCursorPosTop--;
                }
            }
        }

        internal static void GoBack(ref TerminauxerState state) =>
            GoBack(1, ref state);

        internal static void GoBack(int steps, ref TerminauxerState state)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos--;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(TerminauxerSettings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft--;
                if (state.CurrentCursorPosLeft < TerminauxerSettings.LeftMargin)
                {
                    // Reached to the beginning! Wrap up!
                    state.currentCursorPosLeft = ConsoleWrapperTools.ActionWindowWidth() - 1 - TerminauxerSettings.RightMargin;
                    if (state.currentCursorPosTop > 0)
                        state.currentCursorPosTop--;
                }
            }
        }

        internal static void SeekTo(int steps, ref TerminauxerState state) =>
            SeekTo(state.currentTextPos, steps, ref state);

        internal static void SeekTo(int fromPos, int steps, ref TerminauxerState state)
        {
            GoBack(fromPos, ref state);
            GoForward(steps, ref state);
        }

        internal static void GoForwardOneLineWrapAware(ref TerminauxerState state, string[] incompleteSentences) =>
            GoForwardOneLineWrapAware(1, ref state, incompleteSentences);

        internal static void GoForwardOneLineWrapAware(int steps, ref TerminauxerState state, string[] incompleteSentences)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos++;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(TerminauxerSettings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft++;
                if (state.CurrentCursorPosLeft >= ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin)
                {
                    // Reached to the end! Go back to the prompt position.
                    state.currentCursorPosLeft = state.InputPromptLeft + 1;

                    // Refresh the entire prompt
                    int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin - state.inputPromptLeft - 1;
                    string renderedText = BaseBinding.GetOneLineWrappedSentenceToRender(incompleteSentences, state);
                    ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleWrapperTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
                }
            }
        }

        internal static void GoBackOneLineWrapAware(ref TerminauxerState state, string[] incompleteSentences) =>
            GoBackOneLineWrapAware(1, ref state, incompleteSentences);

        internal static void GoBackOneLineWrapAware(int steps, ref TerminauxerState state, string[] incompleteSentences)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos--;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(TerminauxerSettings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft--;
                if (state.CurrentCursorPosLeft < state.inputPromptLeft + 1 && state.CurrentText.Length != 0)
                {
                    // Reached to the beginning! Go back to the furthest position, plus the extra character being printed.
                    state.currentCursorPosLeft = ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin - 1;

                    // Refresh the entire prompt
                    int longestSentenceLength = ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin - state.inputPromptLeft - 1;
                    string renderedText = BaseBinding.GetOneLineWrappedSentenceToRender(incompleteSentences, state);
                    ConsoleWrapperTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleWrapperTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length));
                }
            }
        }

        internal static void SeekToOneLineWrapAware(int steps, ref TerminauxerState state, string[] incompleteSentences) =>
            SeekToOneLineWrapAware(state.currentTextPos, steps, ref state, incompleteSentences);

        internal static void SeekToOneLineWrapAware(int fromPos, int steps, ref TerminauxerState state, string[] incompleteSentences)
        {
            GoBackOneLineWrapAware(fromPos, ref state, incompleteSentences);
            GoForwardOneLineWrapAware(steps, ref state, incompleteSentences);
        }

        internal static void HandleTopChangeForInput(ref TerminauxerState state)
        {
            int promptLeft = state.InputPromptLeft;
            int promptTop = state.InputPromptTop;
            int promptTopOld = state.InputPromptTop;

            int counted = promptLeft;
            int heightOffset = 1;
            for (int i = promptLeft; i < state.CurrentText.Length + promptLeft; i++)
            {
                if (counted >= ConsoleWrapperTools.ActionWindowWidth() - TerminauxerSettings.RightMargin)
                {
                    // Reached to the end! Wrap down!
                    if (promptTop >= ConsoleWrapperTools.ActionBufferHeight() - heightOffset)
                    {
                        heightOffset++;
                        promptTop--;
                        counted = 0;
                        continue;
                    }
                }
                counted++;
            }
            state.inputPromptTop = promptTop;
            state.currentCursorPosTop -= promptTopOld - promptTop;
        }
    }
}
