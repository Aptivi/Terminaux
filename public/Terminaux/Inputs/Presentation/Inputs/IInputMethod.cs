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

using Terminaux.Inputs.Styles;

namespace Terminaux.Inputs.Presentation.Inputs
{
    /// <summary>
    /// Presentation input method element
    /// </summary>
    public interface IInputMethod<TInput>
    {
        /// <summary>
        /// Resulting input entered by the user
        /// </summary>
        TInput? Input { get; }
    }

    /// <summary>
    /// Presentation input method element
    /// </summary>
    public interface IInputMethod : IInputMethod<object>
    {
        /// <summary>
        /// Whether the input has been provided
        /// </summary>
        bool Provided { get; }

        /// <summary>
        /// Input display for input list
        /// </summary>
        string DisplayInput { get; }

        /// <summary>
        /// The question to ask the user
        /// </summary>
        string? Question { get; }

        /// <summary>
        /// The choices
        /// </summary>
        InputChoiceInfo[]? Choices { get; }

        /// <summary>
        /// Prompts the user to enter the input
        /// </summary>
        void PromptInput();

        /// <summary>
        /// Processes the input after prompt
        /// </summary>
        /// <returns>True if the input is provided and processed correctly; false otherwise</returns>
        bool Process();
    }
}
