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

using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Inputs.Styles.Choice
{
    /// <summary>
    /// Settings for the choice input style, <see cref="ChoiceStyle"/>
    /// </summary>
    public class ChoiceStyleSettings
    {
        private static readonly ChoiceStyleSettings globalSettings = new();
        private Color? questionColor;
        private Color? inputColor;
        private Color? optionColor;
        private Color? altOptionColor;
        private Color? disabledOptionColor;

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
        public Color QuestionColor
        {
            get => questionColor ?? ThemeColorsTools.GetColor(ThemeColorType.Question);
            set => SetQuestionColor(value);
        }

        /// <summary>
        /// Sets the question color of the choice style input
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetQuestionColor(Color? color) =>
            questionColor = color;

        /// <summary>
        /// The input color
        /// </summary>
        public Color InputColor
        {
            get => inputColor ?? ThemeColorsTools.GetColor(ThemeColorType.Input);
            set => SetInputColor(value);
        }

        /// <summary>
        /// Sets the input color of the choice style input
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetInputColor(Color? color) =>
            inputColor = color;

        /// <summary>
        /// The option color
        /// </summary>
        public Color OptionColor
        {
            get => optionColor ?? ThemeColorsTools.GetColor(ThemeColorType.Option);
            set => SetOptionColor(value);
        }

        /// <summary>
        /// Sets the option color of the choice style input
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetOptionColor(Color? color) =>
            optionColor = color;

        /// <summary>
        /// The alternative option color
        /// </summary>
        public Color AltOptionColor
        {
            get => altOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption);
            set => SetAltOptionColor(value);
        }

        /// <summary>
        /// Sets the alternative option color of the choice style input
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetAltOptionColor(Color? color) =>
            altOptionColor = color;

        /// <summary>
        /// The disabled option color
        /// </summary>
        public Color DisabledOptionColor
        {
            get => disabledOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.DisabledOption);
            set => SetDisabledOptionColor(value);
        }

        /// <summary>
        /// Sets the disabled option color of the choice style input
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetDisabledOptionColor(Color? color) =>
            disabledOptionColor = color;

        /// <summary>
        /// Makes a new instance of the choice style settings
        /// </summary>
        public ChoiceStyleSettings() :
            this(globalSettings)
        { }

        /// <summary>
        /// Makes a new instance of the choice style settings with the copied settings
        /// </summary>
        /// <param name="choiceStyleSettings">Settings to copy from</param>
        public ChoiceStyleSettings(ChoiceStyleSettings choiceStyleSettings)
        {
            if (choiceStyleSettings is null)
                return;

            OutputType = choiceStyleSettings.OutputType;
            PressEnter = choiceStyleSettings.PressEnter;
            SetQuestionColor(choiceStyleSettings.questionColor);
            SetInputColor(choiceStyleSettings.inputColor);
            SetOptionColor(choiceStyleSettings.optionColor);
            SetAltOptionColor(choiceStyleSettings.altOptionColor);
            SetDisabledOptionColor(choiceStyleSettings.disabledOptionColor);
        }
    }
}
