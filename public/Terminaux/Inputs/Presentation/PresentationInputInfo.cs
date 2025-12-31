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

using System;
using Terminaux.Base;

namespace Terminaux.Inputs.Presentation
{
    /// <summary>
    /// Input information for the presentation
    /// </summary>
    public class PresentationInputInfo
    {
        /// <summary>
        /// Input name for the input list
        /// </summary>
        public string InputName { get; }

        /// <summary>
        /// Input description for the input list
        /// </summary>
        public string InputDescription { get; }

        /// <summary>
        /// Whether this input is required or not
        /// </summary>
        public bool InputRequired { get; }

        /// <summary>
        /// Input method for the resulting input
        /// </summary>
        public InputModule InputMethod { get; }

        /// <summary>
        /// Value processing function
        /// </summary>
        public Func<object?, bool> ProcessFunction { get; set; } = (value) => value is not null;

        /// <summary>
        /// Input method for the resulting input
        /// </summary>
        /// <typeparam name="T">Type of the available input methods</typeparam>
        public T GetInputMethod<T>() where T : InputModule =>
            (T)InputMethod;

        /// <summary>
        /// Makes a new input info instance
        /// </summary>
        /// <param name="inputName">Input name for the input list</param>
        /// <param name="inputDescription">Input description for the input list</param>
        /// <param name="inputMethod">Input method for the resulting input</param>
        /// <param name="required">Whether this input is required or not</param>
        /// <exception cref="TerminauxException"></exception>
        public PresentationInputInfo(string inputName, string inputDescription, InputModule inputMethod, bool required = false)
        {
            InputName = !string.IsNullOrEmpty(inputName) ? inputName :
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_PRESENTATION_EXCEPTION_NOINPUTNAME"));
            InputDescription = inputDescription;
            InputMethod = inputMethod ??
                throw new TerminauxException(nameof(inputMethod));
            InputRequired = required;
        }
    }
}
