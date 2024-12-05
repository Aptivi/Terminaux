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

using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Terminaux.Inputs.Styles.Choice
{
    /// <summary>
    /// Settings for the choice input style, <see cref="ChoiceStyle"/>
    /// </summary>
    public class ChoiceStyleSettings
    {
        /// <summary>
        /// Output type of choices
        /// </summary>
        public ChoiceOutputType OutputType { get; set; } = ChoiceOutputType.OneLine;

        /// <summary>
        /// When enabled, allows the input to consist of multiple characters
        /// </summary>
        public bool PressEnter { get; set; } = false;

        /// <summary>
        /// The question color
        /// </summary>
        public Color QuestionColor { get; set; } = ConsoleColors.Yellow;

        /// <summary>
        /// The input color
        /// </summary>
        public Color InputColor { get; set; } = ConsoleColors.White;

        /// <summary>
        /// The option color
        /// </summary>
        public Color OptionColor { get; set; } = ConsoleColors.Olive;

        /// <summary>
        /// The alternative option color
        /// </summary>
        public Color AltOptionColor { get; set; } = ConsoleColors.Yellow;

        /// <summary>
        /// The disabled option color
        /// </summary>
        public Color DisabledOptionColor { get; set; } = ConsoleColors.Grey;
    }
}
