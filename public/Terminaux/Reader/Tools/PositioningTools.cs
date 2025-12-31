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

using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Terminaux.Reader.Tools
{
    /// <summary>
    /// Positioning tools for the console input reader
    /// </summary>
    public static class PositioningTools
    {
        internal static bool needsCommit = false;

        /// <summary>
        /// Goes to the beginning of the text.
        /// </summary>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoLeftmost(ref TermReaderState state)
        {
            if (state.OneLineWrap)
                GoLeftmostOneLineWrapAware(ref state);
            else
                GoLeftmostOneLineWrapDisabled(ref state);
        }

        /// <summary>
        /// Goes to the end of the text.
        /// </summary>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoRightmost(ref TermReaderState state)
        {
            GoLeftmost(ref state);
            if (state.OneLineWrap)
                GoRightmostOneLineWrapAware(ref state);
            else
                GoRightmostOneLineWrapDisabled(ref state);
        }

        /// <summary>
        /// Goes one step closer to the end of the text.
        /// </summary>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoForward(ref TermReaderState state)
        {
            if (state.OneLineWrap)
                GoForwardOneLineWrapAware(ref state);
            else
                GoForwardOneLineWrapDisabled(ref state);
        }

        /// <summary>
        /// Goes to the number of <paramref name="steps"/> closer to the end of the text.
        /// </summary>
        /// <param name="steps">The number of steps to go closer to the end of the text</param>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoForward(int steps, ref TermReaderState state)
        {
            if (state.OneLineWrap)
                GoForwardOneLineWrapAware(steps, ref state);
            else
                GoForwardOneLineWrapDisabled(steps, ref state);
        }

        /// <summary>
        /// Goes one step closer to the beginning of the text.
        /// </summary>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoBack(ref TermReaderState state)
        {
            if (state.OneLineWrap)
                GoBackOneLineWrapAware(ref state);
            else
                GoBackOneLineWrapDisabled(ref state);
        }

        /// <summary>
        /// Goes to the number of <paramref name="steps"/> closer to the beginning of the text.
        /// </summary>
        /// <param name="steps">The number of steps to go closer to the beginning of the text</param>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void GoBack(int steps, ref TermReaderState state)
        {
            if (state.OneLineWrap)
                GoBackOneLineWrapAware(steps, ref state);
            else
                GoBackOneLineWrapDisabled(steps, ref state);
        }

        /// <summary>
        /// Seeks to the selected text position number, <paramref name="pos"/>.
        /// </summary>
        /// <param name="pos">The text position number (zero-based)</param>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void SeekTo(int pos, ref TermReaderState state)
        {
            if (state.OneLineWrap)
                SeekToOneLineWrapAware(pos, ref state);
            else
                SeekToOneLineWrapDisabled(pos, ref state);
        }

        /// <summary>
        /// Seeks to the selected text position number, <paramref name="pos"/>.
        /// </summary>
        /// <param name="fromPos">The text position number offset (zero-based)</param>
        /// <param name="pos">The text position number (zero-based)</param>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You need to commit this change with the <see cref="Commit(TermReaderState)"/> function, except if this is the last thing to do.
        /// </remarks>
        public static void SeekTo(int fromPos, int pos, ref TermReaderState state)
        {
            if (state.OneLineWrap)
                SeekToOneLineWrapAware(fromPos, pos, ref state);
            else
                SeekToOneLineWrapDisabled(fromPos, pos, ref state);
        }

        /// <summary>
        /// Commits the positional changes by changing the cursor dimensions to the current position according to the reader state
        /// </summary>
        /// <param name="state">State of the terminal reader in its present state</param>
        /// <remarks>
        /// You don't need to call this function most of the time, except if you want to show the cursor changes or if you want to write directly to that position.
        /// </remarks>
        public static void Commit(TermReaderState state)
        {
            if (needsCommit)
                ConsoleWrapper.SetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            needsCommit = false;
        }

        #region Actual functions
        internal static void GoLeftmostOneLineWrapDisabled(ref TermReaderState state) =>
            GoBackOneLineWrapDisabled(state.currentTextPos, ref state);

        internal static void GoRightmostOneLineWrapDisabled(ref TermReaderState state) =>
            GoForwardOneLineWrapDisabled(state.currentText.Length - state.currentTextPos, ref state);

        internal static void GoForwardOneLineWrapDisabled(ref TermReaderState state) =>
            GoForwardOneLineWrapDisabled(1, ref state);

        internal static void GoForwardOneLineWrapDisabled(int steps, ref TermReaderState state)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            int height = ConsoleWrapper.BufferHeight;
            var sentences = ConsoleMisc.GetWrappedSentences(state.currentText.ToString(), state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            var oldSentences = ConsoleMisc.GetWrappedSentences(state.oldText, state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            for (int i = 0; i < steps; i++)
            {
                int prevWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1 < 0 ? 0 : state.currentTextPos - 1);
                int cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos);
                int oldWidth = ConsoleChar.EstimateCellWidth(TermReaderTools.GetLineFromCurrentPos(oldSentences, state));
                state.currentTextPos++;
                if (state.currentTextPos + 1 <= state.currentText.Length && char.IsSurrogatePair(state.currentText[state.currentTextPos - 1], state.currentText[state.currentTextPos]))
                {
                    state.currentTextPos++;
                    i++;
                }

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;
                if (cellWidth == 0)
                    continue;

                // In case CJK and other full-width characters are entered
                int newWidth = ConsoleChar.EstimateCellWidth(TermReaderTools.GetLineFromCurrentPos(sentences, state));
                int nextWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos);
                state.currentCursorPosLeft +=
                    ((prevWidth == 2 && cellWidth == 1) || (nextWidth == 2 && cellWidth == 1)) &&
                    nextWidth == 2 && state.CurrentCursorPosLeft == state.LeftMargin && oldWidth == state.MaximumInputPositionLeft ? 0 : cellWidth;

                // Check to see if we need to wrap
                if (state.CurrentCursorPosLeft > state.MaximumInputPositionLeft ||
                    (state.CurrentCursorPosLeft >= newWidth && state.CurrentCursorPosLeft >= state.MaximumInputPositionLeft && ((cellWidth == 2 || cellWidth == 1) && nextWidth == 2)))
                {
                    // Reached to the end! Wrap down!
                    state.currentCursorPosLeft = state.settings.LeftMargin;
                    if (state.currentCursorPosTop < height - 1)
                        state.currentCursorPosTop++;
                }
            }
            needsCommit = true;
        }

        internal static void GoBackOneLineWrapDisabled(ref TermReaderState state) =>
            GoBackOneLineWrapDisabled(1, ref state);

        internal static void GoBackOneLineWrapDisabled(int steps, ref TermReaderState state)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            int width = ConsoleWrapper.WindowWidth;
            var sentences = ConsoleMisc.GetWrappedSentences(state.currentText.ToString(), state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            var oldSentences = ConsoleMisc.GetWrappedSentences(state.oldText, state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            for (int i = 0; i < steps; i++)
            {
                if (state.currentTextPos == 0)
                    return;

                int prevWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1 < 0 ? 0 : state.currentTextPos - 1);
                int cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1);
                int oldWidth = ConsoleChar.EstimateCellWidth(TermReaderTools.GetLineFromCurrentPos(oldSentences, state));
                state.currentTextPos--;
                if (state.currentTextPos - 1 >= 0 && state.currentTextPos < state.currentText.Length && char.IsSurrogatePair(state.currentText[state.currentTextPos - 1], state.currentText[state.currentTextPos]))
                {
                    cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1);
                    state.currentTextPos--;
                    i++;
                }

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;
                if (cellWidth == 0)
                    continue;

                // In case CJK and other full-width characters are entered
                int nextWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos + 1);
                if (!(
                    ((prevWidth == 2 && cellWidth == 1) || (nextWidth == 2 && cellWidth == 1)) &&
                    nextWidth == 2 && state.CurrentCursorPosLeft == state.LeftMargin && oldWidth == state.MaximumInputPositionLeft
                   ))
                    state.currentCursorPosLeft -= cellWidth > 0 ? cellWidth : ConsoleChar.EstimateCellWidth(state.currentText.ToString(), 0);

                // Check to see if we need to wrap
                if (state.CurrentCursorPosLeft < state.settings.LeftMargin)
                {
                    // Reached to the beginning! Wrap up!
                    state.currentCursorPosLeft = width - cellWidth - state.settings.RightMargin;
                    if (state.currentCursorPosTop > 0 && state.CurrentTextPos < TermReaderTools.GetMaximumInputLength(state))
                        state.currentCursorPosTop--;

                    // Now, check to see if we have a character that would overflow
                    string line = TermReaderTools.GetLineFromCurrentPos(sentences, state);
                    int lineWidth = ConsoleChar.EstimateCellWidth(line);
                    if (lineWidth == state.MaximumInputPositionLeft - state.LeftMargin - (state.CurrentCursorPosTop == state.InputPromptTop ? ConsoleChar.EstimateCellWidth(state.InputPromptLastLine) : 0))
                        state.currentCursorPosLeft--;
                }
            }
            needsCommit = true;
        }

        internal static void GoLeftmostOneLineWrapAware(ref TermReaderState state) =>
            GoBackOneLineWrapAware(state.currentTextPos, ref state);

        internal static void GoRightmostOneLineWrapAware(ref TermReaderState state) =>
            GoForwardOneLineWrapAware(state.currentText.Length - state.currentTextPos, ref state);

        internal static void GoForwardOneLineWrapAware(ref TermReaderState state) =>
            GoForwardOneLineWrapAware(1, ref state);

        internal static void GoForwardOneLineWrapAware(int steps, ref TermReaderState state)
        {
            if (steps > state.currentText.Length - state.currentTextPos)
                steps = state.currentText.Length - state.currentTextPos;

            var oldSentences = ConsoleMisc.GetWrappedSentences(state.oldText, state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            for (int i = 0; i < steps; i++)
            {
                int prevWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1 < 0 ? 0 : state.currentTextPos - 1);
                int cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos);
                int oldWidth = ConsoleChar.EstimateCellWidth(TermReaderTools.GetLineFromCurrentPos(oldSentences, state));
                state.currentTextPos++;
                if (state.currentTextPos + 1 <= state.currentText.Length && char.IsSurrogatePair(state.currentText[state.currentTextPos - 1], state.currentText[state.currentTextPos]))
                {
                    state.currentTextPos++;
                    i++;
                }

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;
                if (cellWidth == 0)
                    continue;

                // In case CJK and other full-width characters are entered
                int nextWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos);
                state.currentCursorPosLeft +=
                    ((prevWidth == 2 && cellWidth == 1) || (nextWidth == 2 && cellWidth == 1)) &&
                    nextWidth == 2 && state.CurrentCursorPosLeft == state.LeftMargin && oldWidth == state.MaximumInputPositionLeft ? 0 : cellWidth;
                if (state.CurrentCursorPosLeft >= state.LongestSentenceLengthFromLeft)
                {
                    // Reached to the end! Go back to the prompt position.
                    state.currentCursorPosLeft = state.InputPromptLeft + cellWidth;
                    state.RefreshRequired = true;
                }
            }
            needsCommit = true;
        }

        internal static void GoBackOneLineWrapAware(ref TermReaderState state) =>
            GoBackOneLineWrapAware(1, ref state);

        internal static void GoBackOneLineWrapAware(int steps, ref TermReaderState state)
        {
            if (steps > state.currentTextPos)
                steps = state.currentTextPos;

            int width = ConsoleWrapper.WindowWidth;
            var sentences = ConsoleMisc.GetWrappedSentences(state.currentText.ToString(), state.LongestSentenceLengthFromLeftForFirstLine, 0);
            var oldSentences = ConsoleMisc.GetWrappedSentences(state.oldText, state.LongestSentenceLengthFromLeft - state.settings.LeftMargin, state.InputPromptLastLineLength);
            for (int i = 0; i < steps; i++)
            {
                if (state.currentTextPos == 0)
                    return;

                int prevWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1 < 0 ? 0 : state.currentTextPos - 1);
                int cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1);
                int oldWidth = ConsoleChar.EstimateCellWidth(TermReaderTools.GetLineFromCurrentPos(oldSentences, state));
                state.currentTextPos--;
                if (state.currentTextPos - 1 >= 0 && state.currentTextPos < state.currentText.Length && char.IsSurrogatePair(state.currentText[state.currentTextPos - 1], state.currentText[state.currentTextPos]))
                {
                    cellWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos - 1);
                    state.currentTextPos--;
                    i++;
                }

                // If the character is unrenderable, continue the loop
                if (state.PasswordMode && char.IsControl(state.settings.PasswordMaskChar))
                    continue;
                if (cellWidth == 0)
                    continue;

                // In case CJK and other full-width characters are entered
                int nextWidth = ConsoleChar.EstimateCellWidth(state.currentText.ToString(), state.currentTextPos + 1);
                if (!(
                    ((prevWidth == 2 && cellWidth == 1) || (nextWidth == 2 && cellWidth == 1)) &&
                    nextWidth == 2 && state.CurrentCursorPosLeft == state.LeftMargin && oldWidth == state.MaximumInputPositionLeft
                   ))
                    state.currentCursorPosLeft -= cellWidth > 0 ? cellWidth : ConsoleChar.EstimateCellWidth(state.currentText.ToString(), 0);

                // Check to see if we need to wrap
                if (state.CurrentCursorPosLeft <= state.inputPromptLeft && state.CurrentTextPos != 0)
                {
                    // Reached to the beginning! Go back to the furthest position, plus the extra character being printed.
                    state.currentCursorPosLeft = width - state.settings.RightMargin - 1;
                    string line = TermReaderTools.GetLineFromCurrentPos(sentences, state);
                    int lineWidth = ConsoleChar.EstimateCellWidth(line);
                    state.currentCursorPosLeft -= state.LongestSentenceLengthFromLeftForFirstLine - lineWidth;
                    state.RefreshRequired = true;
                }
            }
            needsCommit = true;
        }

        internal static void SeekToOneLineWrapDisabled(int steps, ref TermReaderState state) =>
            SeekToOneLineWrapDisabled(state.currentTextPos, steps, ref state);

        internal static void SeekToOneLineWrapDisabled(int fromPos, int steps, ref TermReaderState state)
        {
            GoBackOneLineWrapDisabled(fromPos, ref state);
            GoForwardOneLineWrapDisabled(steps, ref state);
        }

        internal static void SeekToOneLineWrapAware(int steps, ref TermReaderState state) =>
            SeekToOneLineWrapAware(state.currentTextPos, steps, ref state);

        internal static void SeekToOneLineWrapAware(int fromPos, int steps, ref TermReaderState state)
        {
            GoBackOneLineWrapAware(fromPos, ref state);
            GoForwardOneLineWrapAware(steps, ref state);
        }
        #endregion
    }
}
