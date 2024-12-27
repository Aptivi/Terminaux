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

using System.Collections.Generic;
using System.Linq;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Input choice tools
    /// </summary>
    public static class InputChoiceTools
    {
        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="Answers">Set of working titles.</param>
        public static InputChoiceInfo[] GetInputChoices(string[] Answers) =>
            GetInputChoices(Answers.Select((answer, idx) => ($"{idx + 1}", answer)).ToArray());

        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="Answers">Set of answers and working titles for each answer in one tuple.</param>
        public static InputChoiceInfo[] GetInputChoices((string, string)[] Answers)
        {
            // Variables
            var finalChoices = new List<InputChoiceInfo>();

            // Now, populate choice information from the arrays
            for (int i = 0; i < Answers.Length; i++)
            {
                string answer = string.IsNullOrEmpty(Answers[i].Item1) ? $"[{i + 1}]" : Answers[i].Item1;
                string title = string.IsNullOrEmpty(Answers[i].Item2) ? $"Untitled answer #{i + 1}" : Answers[i].Item2;
                finalChoices.Add(new InputChoiceInfo(answer, title));
            }
            return [.. finalChoices];
        }
    }
}
