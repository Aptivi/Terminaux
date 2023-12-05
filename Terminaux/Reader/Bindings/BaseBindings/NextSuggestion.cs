
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
using Textify.General;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class NextSuggestion : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        [
            new ConsoleKeyInfo('\t', ConsoleKey.Tab, false, false, false)
        ];

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Get suggestions
            string[] suggestions = state.settings.suggestions(state.CurrentText.ToString(), state.CurrentTextPos, state.settings.suggestionsDelims);
            if (suggestions.Length > 0)
            {
                TermReaderState.currentSuggestionsPos++;
                if (TermReaderState.currentSuggestionsPos >= suggestions.Length)
                    TermReaderState.currentSuggestionsPos = 0;

                // Get a suggestion
                string suggestion = suggestions[state.CurrentSuggestionsPos];

                // If there is no suggestion, bail
                if (string.IsNullOrEmpty(suggestion))
                    return;
                int maxTimes = suggestions.Max((str) => str.Length);
                int oldLength = state.CurrentText.Length;

                // Wipe out everything from the right until the space is spotted
                string[] splitText = state.CurrentText.ToString().SplitEncloseDoubleQuotes();
                int pos = 0;
                for (int i = 0; i < splitText.Length; i++)
                {
                    string text = splitText[i] + " ";
                    bool bail = false;
                    for (int j = 0; j < text.Length; j++)
                    {
                        if (pos == state.CurrentTextPos)
                        {
                            if (j + 1 == text.Length)
                                splitText[i] += suggestion;
                            else
                                splitText[i] = splitText[i].Remove(j) + suggestion;
                            bail = true;
                            break;
                        }
                        pos++;
                    }
                    if (bail)
                        break;
                }
                state.CurrentText.Clear();
                state.CurrentText.Append(string.Join(" ", splitText));
                TermReaderTools.RefreshPrompt(ref state);
            }
            else
            {
                BindingsList.fallbackBinding.DoAction(state);
            }
        }
    }
}
