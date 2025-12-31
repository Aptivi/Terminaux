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

using System.Linq;
using Terminaux.Reader.Tools;
using Textify.General;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class NextSuggestion : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override bool IsBindingOverridable => true;

        /// <inheritdoc/>
        public override bool ResetSuggestionsTextPos =>
            false;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // Get suggestions
            bool setTextPos = state.currentSuggestionsTextPos == -1;
            string[] suggestions = state.settings.suggestions(state.CurrentText.ToString(), !setTextPos ? state.currentSuggestionsTextPos : state.CurrentTextPos, state.settings.suggestionsDelims);
            if (suggestions.Length > 0)
            {
                state.currentSuggestionsPos++;
                if (state.currentSuggestionsPos >= suggestions.Length)
                    state.currentSuggestionsPos = 0;

                // Get a suggestion
                string suggestion = suggestions[state.CurrentSuggestionsPos];

                // If there is no suggestion, bail
                if (!ConditionalTools.ShouldNot(string.IsNullOrEmpty(suggestion), state))
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
                        if (pos == state.CurrentTextPos || pos == state.currentSuggestionsTextPos)
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
                    {
                        if (setTextPos)
                            state.currentSuggestionsTextPos = state.CurrentTextPos;
                        break;
                    }
                }
                TermReaderTools.WipeAll(ref state);
                state.CurrentText.Append(string.Join(" ", splitText));
                PositioningTools.GoRightmost(ref state);
                state.RefreshRequired = true;
            }
            else
            {
                BindingsTools.fallbackBinding.DoAction(state);
            }
        }
    }
}
