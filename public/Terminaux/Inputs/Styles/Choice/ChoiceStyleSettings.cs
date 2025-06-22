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

namespace Terminaux.Inputs.Styles.Choice
{
    /// <summary>
    /// Settings for the choice input style, <see cref="ChoiceStyle"/>
    /// </summary>
    public class ChoiceStyleSettings
    {
        private static readonly ChoiceStyleSettings globalSettings = new();

        /// <summary>
        /// Global choice style settings
        /// </summary>
        public static ChoiceStyleSettings GlobalSettings =>
            globalSettings;

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
        public Color QuestionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Question);

        /// <summary>
        /// The input color
        /// </summary>
        public Color InputColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Input);

        /// <summary>
        /// The option color
        /// </summary>
        public Color OptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Option);

        /// <summary>
        /// The alternative option color
        /// </summary>
        public Color AltOptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption);

        /// <summary>
        /// The disabled option color
        /// </summary>
        public Color DisabledOptionColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.DisabledOption);
    }
}
