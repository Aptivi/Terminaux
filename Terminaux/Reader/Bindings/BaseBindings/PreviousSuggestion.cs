
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
using System.Linq;
using Terminaux.Base;
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class PreviousSuggestion : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('\t', ConsoleKey.Tab, true, false, false)
        };

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Get suggestions
            string[] suggestions = state.settings.suggestions(state.CurrentText.ToString(), state.CurrentTextPos, state.settings.suggestionsDelims);
            if (suggestions.Length > 0)
            {
                TermReaderState.currentSuggestionsPos--;
                if (TermReaderState.currentSuggestionsPos < 0)
                    TermReaderState.currentSuggestionsPos = suggestions.Length - 1;
                if (TermReaderState.currentSuggestionsPos > suggestions.Length)
                    TermReaderState.currentSuggestionsPos = 0;

                // Get a suggestion
                string suggestion = suggestions[state.CurrentSuggestionsPos];
                int maxTimes = suggestions.Max((str) => str.Length);

                // Wipe out everything from the right
                state.CurrentText.Remove(state.CurrentTextPos, state.CurrentText.Length - state.CurrentTextPos);

                // Re-write the text and set the current cursor position as appropriate
                string renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();

                // In the case of one line wrap, get the list of sentences
                if (state.OneLineWrap)
                {
                    int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                    string[] incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                    renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                    ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                }
                else
                {
                    ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleTools.ActionWriteString(renderedText + new string(' ', state.CurrentText.Length - state.CurrentTextPos), state.settings);
                }
                ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);

                // Write the suggestion
                state.CurrentText.Append(suggestion);
                renderedText = state.PasswordMode ? new string(state.settings.PasswordMaskChar, state.currentText.ToString().Length) : state.currentText.ToString();
                if (state.OneLineWrap)
                {
                    int longestSentenceLength = ConsoleTools.ActionWindowWidth() - state.settings.RightMargin - state.inputPromptLeft - 1;
                    string[] incompleteSentences = ConsoleExtensions.GetWrappedSentences(renderedText, longestSentenceLength, 0);
                    renderedText = state.OneLineWrap ? GetOneLineWrappedSentenceToRender(incompleteSentences, state) : renderedText;
                    ConsoleTools.ActionSetCursorPosition(state.InputPromptLeft, state.InputPromptTop);
                    ConsoleTools.ActionWriteString(renderedText + new string(' ', longestSentenceLength - renderedText.Length), state.settings);
                }
                else
                    ConsoleTools.ActionWriteString(suggestion + new string(' ', maxTimes), state.settings);
                ConsoleTools.ActionSetCursorPosition(state.CurrentCursorPosLeft, state.CurrentCursorPosTop);
            }
        }
    }
}
