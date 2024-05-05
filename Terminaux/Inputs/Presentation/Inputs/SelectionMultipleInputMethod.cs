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

using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Selection multiple input method (multiple selection, uses informational box)
    /// </summary>
    public class SelectionMultipleInputMethod : IInputMethod<int[]>, IInputMethod
    {
        private int[] _value = [];

        /// <summary>
        /// The input 
        /// </summary>
        public int[] Input =>
            _value;

        /// <summary>
        /// The display name for the input 
        /// </summary>
        public string DisplayInput =>
            $"Selected {Input.Length} options";

        object? IInputMethod<object>.Input =>
            Input;

        /// <summary>
        /// Prompts the user using the multiple selection style
        /// </summary>
        /// <typeparam name="TChoices">Choice type to use to specify the list of choices</typeparam>
        /// <param name="question">A question to ask the user</param>
        /// <param name="choices">List of choices</param>
        /// <exception cref="TerminauxException"></exception>
        public void PromptInput<TChoices>(string question, TChoices[]? choices = null)
        {
            if (choices is null || choices.Length == 0)
                throw new TerminauxException("Choices are not specified");
            if (choices is not InputChoiceInfo[] inputChoices)
                throw new TerminauxException("Choice list is invalid");

            // Now, open the infobox
            _value = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(inputChoices, question);
        }
    }
}
