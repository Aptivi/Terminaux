//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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

using Terminaux.Reader.Tools;

namespace Terminaux.Reader.Bindings.BaseBindings
{
    internal class SubstituteWords : BaseBinding, IBinding
    {
        /// <inheritdoc/>
        public override void DoAction(TermReaderState state)
        {
            // First, check the boundaries
            if (!ConditionalTools.ShouldNot(state.CurrentText.Length <= 1, state))
                return;
            if (!ConditionalTools.ShouldNot(state.CurrentTextPos == state.CurrentText.Length, state))
                return;
            if (!ConditionalTools.ShouldNot(!char.IsWhiteSpace(state.CurrentText[state.CurrentTextPos]), state))
                return;

            // Then, get the two words
            string first, second;
            int firstWordStart = 0;
            int firstWordEnd = state.CurrentTextPos;
            int secondWordStart = state.CurrentTextPos;
            int secondWordEnd = state.CurrentText.Length;
            bool sawCharForFirstWord = false;
            bool sawCharForSecondWord = false;

            // Get the word index values in the first word
            for (int s = firstWordEnd; s > firstWordStart; s--)
            {
                if (char.IsWhiteSpace(state.CurrentText[s]) && sawCharForFirstWord)
                {
                    firstWordStart = s + 1;
                    break;
                }
                else if (!char.IsWhiteSpace(state.CurrentText[s]))
                    // We saw a character for the first time after n amount of spaces! Now, look for spaces.
                    sawCharForFirstWord = true;
            }

            // Get the word index values in the second word
            for (int s = secondWordStart; s < secondWordEnd; s++)
            {
                if (char.IsWhiteSpace(state.CurrentText[s]) && sawCharForSecondWord)
                {
                    secondWordEnd = s;
                    break;
                }
                else if (!char.IsWhiteSpace(state.CurrentText[s]))
                    // We saw a character for the first time after n amount of spaces! Now, look for spaces.
                    sawCharForSecondWord = true;
            }
            first = state.CurrentText.ToString().Substring(firstWordStart, firstWordEnd - firstWordStart);
            second = state.CurrentText.ToString().Substring(secondWordStart, secondWordEnd - secondWordStart);

            // Now, substitute them while trimming them
            string orig = first + second;
            int diff = secondWordEnd - firstWordStart;
            first = first.Trim();
            second = second.Trim();
            string final = second + " " + first;

            // Finally, go to the start of the second word by length (not by cell length).
            PositioningTools.GoBack(first.Length, ref state);
            state.CurrentText.Replace(orig, final, firstWordStart, diff);
            PositioningTools.GoForward(second.Length, ref state);
            state.RefreshRequired = true;
        }
    }
}
