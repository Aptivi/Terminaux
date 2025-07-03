//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Selection multiple input method (multiple selection, uses informational box)
    /// </summary>
    public class SelectionMultipleInputMethod : BaseInputMethod<int[]>, IInputMethod<int[]>, IInputMethod
    {
        private int[] _value = [];

        /// <inheritdoc/>
        public override string DisplayInput =>
            Input.Length > 0 && Choices is not null ? string.Join(", ", Choices.Select((ici) => ici.ChoiceName).Where((_, idx) => Input.Contains(idx))) : "   ";

        /// <inheritdoc/>
        public override int[] Input =>
            _value;

        object? IInputMethod<object>.Input =>
            Input;

        /// <inheritdoc/>
        public override void PromptInput()
        {
            if (Choices is null || Choices.Length == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_EXCEPTION_NEEDSCHOICES"));
            if (Question is null || string.IsNullOrEmpty(Question))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_EXCEPTION_NEEDSQUESTION"));

            // Now, open the infobox
            _value = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(Choices, Question);
            Provided = true;
        }

        /// <inheritdoc/>
        public override bool Process() =>
            Provided;
    }
}
