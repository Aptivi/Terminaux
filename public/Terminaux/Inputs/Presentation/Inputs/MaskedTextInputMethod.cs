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

using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Masked text input method
    /// </summary>
    public class MaskedTextInputMethod : BaseInputMethod<string>, IInputMethod<string>, IInputMethod
    {
        private string _value = "";

        /// <inheritdoc/>
        public override string DisplayInput =>
            !string.IsNullOrEmpty(Input) ? "***" : "   ";

        /// <inheritdoc/>
        public override string Input =>
            _value;

        object? IInputMethod<object>.Input =>
            Input;

        /// <inheritdoc/>
        public override void PromptInput()
        {
            if (Question is null || string.IsNullOrEmpty(Question))
                throw new TerminauxException("The question has not been provided.");

            // Now, open the infobox
            _value = InfoBoxInputPasswordColor.WriteInfoBoxInputPassword(Question);
            Provided = true;
        }

        /// <inheritdoc/>
        public override bool Process() =>
            Provided && !string.IsNullOrEmpty(Input);
    }
}
