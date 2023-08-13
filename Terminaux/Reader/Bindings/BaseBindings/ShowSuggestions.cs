
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
using System.Text;
using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class ShowSuggestions : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override ConsoleKeyInfo[] BoundKeys { get; } =
        {
            new ConsoleKeyInfo('s', ConsoleKey.S, false, true, false),
        };

        /// <inheritdoc/>
        public override bool IsExit => false;

        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            string[] suggestions = state.settings.suggestions(state.CurrentText.ToString(), state.CurrentTextPos, state.settings.suggestionsDelims);
            if (suggestions.Length > 1)
            {
                // Write a new line
                ConsoleTools.ActionWriteLine();

                // Check to see if we can display 15 characters
                int max = suggestions.Max((suggestion) => suggestion.Length);
                bool isLine = max > 15;
                if (isLine)
                {
                    // Write each suggestion on their own line
                    foreach (string suggestion in suggestions)
                        ConsoleTools.ActionWriteLineString(suggestion, state.Settings);
                }
                else
                {
                    // Write suggestions using 5 columns
                    var line = new StringBuilder();
                    int columns = 5;
                    int maxChars = 15;
                    int maxLineLength = maxChars * columns;
                    for (int i = 0; i < suggestions.Length; i++)
                    {
                        // Write a suggestion
                        line.Append(suggestions[i]);
                        int fillTimes = maxChars - (line.Length % maxChars);
                        line.Append(' ', fillTimes);
                        
                        // Check to see if we've reached the limit
                        if (line.Length >= maxLineLength)
                        {
                            ConsoleTools.ActionWriteLineString(line.ToString(), state.Settings);
                            line.Clear();
                        }
                    }

                    // Flush for the last time
                    if (line.Length > 0)
                    {
                        ConsoleTools.ActionWriteLineString(line.ToString(), state.Settings);
                        line.Clear();
                    }

                    // Re-draw
                    state.inputPromptTop = ConsoleTools.ActionCursorTop();
                    state.currentCursorPosTop = ConsoleTools.ActionCursorTop();
                    new Refresh().DoAction(state);
                }
            }
            else if (suggestions.Length == 1)
                new PreviousSuggestion().DoAction(state);
        }
    }
}
