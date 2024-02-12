﻿//
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

namespace Terminaux.Inputs.Styles.Selection
{
    /// <summary>
    /// Settings for the selection style
    /// </summary>
    public class SelectionStyleSettings
    {
        /// <summary>
        /// Global selection style settings
        /// </summary>
        public static SelectionStyleSettings GlobalSettings { get; } = new();

        /// <summary>
        /// Question color
        /// </summary>
        public Color QuestionColor { get; set; } = new(ConsoleColors.Yellow);
        /// <summary>
        /// Slider color
        /// </summary>
        public Color SliderColor { get; set; } = new(ConsoleColors.Yellow);
        /// <summary>
        /// Input color
        /// </summary>
        public Color InputColor { get; set; } = new(ConsoleColors.White);
        /// <summary>
        /// Option color
        /// </summary>
        public Color OptionColor { get; set; } = new(ConsoleColors.DarkYellow);
        /// <summary>
        /// Alternative option color
        /// </summary>
        public Color AltOptionColor { get; set; } = new(ConsoleColors.Yellow);
        /// <summary>
        /// Selected option color
        /// </summary>
        public Color SelectedOptionColor { get; set; } = new(ConsoleColors.Cyan);
        /// <summary>
        /// Separator color
        /// </summary>
        public Color SeparatorColor { get; set; } = new(ConsoleColors.Gray);
        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor { get; set; } = new(ConsoleColors.Gray);
    }
}
