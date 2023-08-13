
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

using Terminaux.Reader.Bindings.BaseBindings;

namespace Terminaux.Reader.Tools
{
    internal static class PositioningTools
    {
        internal static void GoForward(ref TermReaderState state) =>
            GoForward(1, ref state);

        internal static void GoForward(int steps, ref TermReaderState state)
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

        internal static void GoForwardOneLineWrapAware(ref TermReaderState state) =>
            GoForwardOneLineWrapAware(1, ref state);

        internal static void GoForwardOneLineWrapAware(int steps, ref TermReaderState state)
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
                    new Refresh().DoAction(state);
                }
            }
        }

        internal static void GoBackOneLineWrapAware(ref TermReaderState state) =>
            GoBackOneLineWrapAware(1, ref state);

        internal static void GoBackOneLineWrapAware(int steps, ref TermReaderState state)
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
                    new Refresh().DoAction(state);
                }
            }
        }

        internal static void SeekToOneLineWrapAware(int steps, ref TermReaderState state) =>
            SeekToOneLineWrapAware(state.currentTextPos, steps, ref state);

        internal static void SeekToOneLineWrapAware(int fromPos, int steps, ref TermReaderState state)
        {
            GoBackOneLineWrapAware(fromPos, ref state);
            GoForwardOneLineWrapAware(steps, ref state);
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
