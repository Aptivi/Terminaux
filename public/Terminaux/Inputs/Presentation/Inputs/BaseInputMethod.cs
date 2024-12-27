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

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Base input method
    /// </summary>
    public class BaseInputMethod<T> : IInputMethod<T>
    {
        private bool provided;
        private string? question;

        /// <inheritdoc/>
        public virtual InputChoiceInfo[]? Choices { get; set; }

        /// <summary>
        /// The display name for the input 
        /// </summary>
        public virtual string DisplayInput =>
            Input is not null && Input.ToString() != Input.GetType().ToString() ? Input.ToString() :
            Input is not null ? "..." :
            "   ";

        /// <summary>
        /// The input 
        /// </summary>
        public virtual T? Input =>
            default;

        /// <inheritdoc/>
        public virtual bool Provided
        {
            get => provided;
            set
            {
                if (!provided)
                    provided = value;
            }
        }

        /// <inheritdoc/>
        public virtual string? Question
        {
            get => question;
            set => question ??= value;
        }

        /// <inheritdoc/>
        public virtual bool Process() =>
            false;

        /// <summary>
        /// Prompts the user using the masked text input style
        /// </summary>
        /// <exception cref="TerminauxException"></exception>
        public virtual void PromptInput() =>
            throw new TerminauxInternalException("Not implemented yet");
    }
}
