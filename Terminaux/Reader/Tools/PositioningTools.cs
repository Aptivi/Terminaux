
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

using Terminaux.Reader;
using Terminaux.Reader.Bindings;

namespace Terminaux.Reader.Tools
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

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft++;
                if (state.CurrentCursorPosLeft >= ConsoleTools.ActionWindowWidth() - state.settings.RightMargin)
                {
                    // Reached to the end! Wrap down!
                    state.currentCursorPosLeft = state.settings.LeftMargin;
                    if (state.currentCursorPosTop < ConsoleTools.ActionBufferHeight())
                        state.currentCursorPosTop++;
                }

                // Check to see if we're at the end of the buffer in append mode
                if (state.currentCursorPosTop >= ConsoleTools.ActionBufferHeight() && isAppend)
                {
                    // We can't increase the top position since we're at the end of buffer, so we need to set the
                    // input prompt top position to be minus one. If we can't do that again because it went before the
                    // first column in the buffer, there's nothing we can do about this.
                    if (state.InputPromptTop > 0)
                        state.inputPromptTop -= state.currentCursorPosTop - ConsoleTools.ActionBufferHeight() + 1;
                    ConsoleTools.ActionWriteLine();
                    state.currentCursorPosTop--;
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

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft--;
                if (state.CurrentCursorPosLeft < state.settings.LeftMargin)
                {
                    // Reached to the beginning! Wrap up!
                    state.currentCursorPosLeft = ConsoleTools.ActionWindowWidth() - 1 - state.settings.RightMargin;
                    if (state.currentCursorPosTop > 0)
                        state.currentCursorPosTop--;
                }
            }
        }

        internal static void SeekTo(int steps, ref TermReaderState state) =>
            SeekTo(state.currentTextPos, steps, ref state);

        internal static void SeekTo(int fromPos, int steps, ref TermReaderState state)
        {
            GoBack(fromPos, ref state);
            GoForward(steps, ref state);
        }

        internal static void GoForwardOneLineWrapAware(ref TermReaderState state, string[] incompleteSentences) =>
            GoForwardOneLineWrapAware(1, ref state, incompleteSentences);

        internal static void GoForwardOneLineWrapAware(int steps, ref TermReaderState state, string[] incompleteSentences)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos++;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft++;
                if (state.CurrentCursorPosLeft >= ConsoleTools.ActionWindowWidth() - state.settings.RightMargin)
                {
                    // Reached to the end! Go back to the prompt position.
                    state.currentCursorPosLeft = state.InputPromptLeft + 1;

                    // Refresh the entire prompt
                    int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                    string renderedText = BaseBinding.GetOneLineWrappedSentenceToRender(incompleteSentences, state);
                    ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                }
            }
        }

        internal static void GoBackOneLineWrapAware(ref TermReaderState state, string[] incompleteSentences) =>
            GoBackOneLineWrapAware(1, ref state, incompleteSentences);

        internal static void GoBackOneLineWrapAware(int steps, ref TermReaderState state, string[] incompleteSentences)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            for (int i = 0; i < steps; i++)
            {
                state.currentTextPos--;

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;

                state.currentCursorPosLeft--;
                if (state.CurrentCursorPosLeft < state.inputPromptLeft + 1 && state.CurrentText.Length != 0)
                {
                    // Reached to the beginning! Go back to the furthest position, plus the extra character being printed.
                    state.currentCursorPosLeft = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - 1;

                    // Refresh the entire prompt
                    int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                    string renderedText = BaseBinding.GetOneLineWrappedSentenceToRender(incompleteSentences, state);
                    ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                }
            }
        }

        internal static void SeekToOneLineWrapAware(int steps, ref TermReaderState state, string[] incompleteSentences) =>
            SeekToOneLineWrapAware(state.currentTextPos, steps, ref state, incompleteSentences);

        internal static void SeekToOneLineWrapAware(int fromPos, int steps, ref TermReaderState state, string[] incompleteSentences)
        {
            GoBackOneLineWrapAware(fromPos, ref state, incompleteSentences);
            GoForwardOneLineWrapAware(steps, ref state, incompleteSentences);
        }

        internal static void HandleTopChangeForInput(ref TermReaderState state)
        {
            int promptLeft = state.InputPromptLeft;
            int promptTop = state.InputPromptTop;
            int promptTopOld = state.InputPromptTop;

            int counted = promptLeft;
            int heightOffset = 1;
            for (int i = promptLeft; i < state.CurrentText.Length + promptLeft; i++)
            {
                if (counted >= ConsoleTools.ActionWindowWidth() - state.settings.RightMargin)
                {
                    // Reached to the end! Wrap down!
                    if (promptTop >= ConsoleTools.ActionBufferHeight() - heightOffset)
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
