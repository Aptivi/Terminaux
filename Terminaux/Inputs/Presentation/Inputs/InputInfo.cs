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

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Input information for the presentation
    /// </summary>
    public class InputInfo
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
        public IInputMethod InputMethod { get; }

        /// <summary>
        /// Input method for the resulting input (interface)
        /// </summary>
        /// <typeparam name="T">Target type for this input method</typeparam>
        public IInputMethod<T> GetInputMethodInterface<T>() =>
            (IInputMethod<T>)InputMethod;

        /// <summary>
        /// Input method for the resulting input (generic)
        /// </summary>
        /// <typeparam name="T">Type of the available input methods</typeparam>
        public T GetInputMethod<T>() where T : IInputMethod =>
            (T)InputMethod;

        /// <summary>
        /// Makes a new input info instance
        /// </summary>
        /// <param name="inputName">Input name for the input list</param>
        /// <param name="inputDescription">Input description for the input list</param>
        /// <param name="inputMethod">Input method for the resulting input</param>
        /// <param name="required">Whether this input is required or not</param>
        /// <exception cref="TerminauxException"></exception>
        public InputInfo(string inputName, string inputDescription, IInputMethod inputMethod, bool required = false)
        {
            InputName = !string.IsNullOrEmpty(inputName) ? inputName :
                throw new TerminauxException("Input name is not specified");
            InputDescription = inputDescription;
            InputMethod = inputMethod ??
                throw new TerminauxException(nameof(inputMethod));
            InputRequired = required;
        }
    }
}
