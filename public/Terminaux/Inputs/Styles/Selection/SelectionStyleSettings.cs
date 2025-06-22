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

using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Themes.Colors;

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
        public Color QuestionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Question);

        /// <summary>
        /// Slider color
        /// </summary>
        public Color SliderColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Option);

        /// <summary>
        /// Input color
        /// </summary>
        public Color InputColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Input);

        /// <summary>
        /// Option color
        /// </summary>
        public Color OptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Option);

        /// <summary>
        /// Alternative option color
        /// </summary>
        public Color AltOptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption);

        /// <summary>
        /// Selected option color
        /// </summary>
        public Color SelectedOptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.SelectedOption);

        /// <summary>
        /// Separator color
        /// </summary>
        public Color SeparatorColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Separator);

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);

        /// <summary>
        /// Disabled option color
        /// </summary>
        public Color DisabledOptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.DisabledOption);

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor { get; set; } = ColorTools.CurrentBackgroundColor;

        /// <summary>
        /// Use the radio buttons when dealing with selection style (single-choice)
        /// </summary>
        public bool RadioButtons { get; set; }
    }
}
