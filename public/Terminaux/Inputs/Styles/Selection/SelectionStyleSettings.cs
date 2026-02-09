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

namespace Terminaux.Inputs.Styles.Selection
{
    /// <summary>
    /// Settings for the selection style
    /// </summary>
    public class SelectionStyleSettings
    {
        private Color? questionColor;
        private Color? sliderColor;
        private Color? inputColor;
        private Color? optionColor;
        private Color? altOptionColor;
        private Color? selectedOptionColor;
        private Color? separatorColor;
        private Color? textColor;
        private Color? disabledOptionColor;
        private Color? backgroundColor;
        private static readonly SelectionStyleSettings globalSettings = new();

        /// <summary>
        /// Global selection style settings
        /// </summary>
        public static SelectionStyleSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Use the radio buttons when dealing with selection style (single-choice)
        /// </summary>
        public bool RadioButtons { get; set; }

        /// <summary>
        /// Title of the selection style
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Question color
        /// </summary>
        public Color QuestionColor
        {
            get => questionColor ?? ThemeColorsTools.GetColor(ThemeColorType.Question);
            set => SetQuestionColor(value);
        }

        /// <summary>
        /// Sets the question color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetQuestionColor(Color? color) =>
            questionColor = color;

        /// <summary>
        /// Slider color
        /// </summary>
        public Color SliderColor
        {
            get => sliderColor ?? ThemeColorsTools.GetColor(ThemeColorType.Separator);
            set => SetSliderColor(value);
        }

        /// <summary>
        /// Sets the slider color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetSliderColor(Color? color) =>
            sliderColor = color;

        /// <summary>
        /// Input color
        /// </summary>
        public Color InputColor
        {
            get => inputColor ?? ThemeColorsTools.GetColor(ThemeColorType.Input);
            set => SetInputColor(value);
        }

        /// <summary>
        /// Sets the input color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetInputColor(Color? color) =>
            inputColor = color;

        /// <summary>
        /// Option color
        /// </summary>
        public Color OptionColor
        {
            get => optionColor ?? ThemeColorsTools.GetColor(ThemeColorType.Option);
            set => SetOptionColor(value);
        }

        /// <summary>
        /// Sets the option color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetOptionColor(Color? color) =>
            optionColor = color;

        /// <summary>
        /// Alternative option color
        /// </summary>
        public Color AltOptionColor
        {
            get => altOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption);
            set => SetAltOptionColor(value);
        }

        /// <summary>
        /// Sets the alternative option color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetAltOptionColor(Color? color) =>
            altOptionColor = color;

        /// <summary>
        /// Selected option color
        /// </summary>
        public Color SelectedOptionColor
        {
            get => selectedOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.SelectedOption);
            set => SetSelectedOptionColor(value);
        }

        /// <summary>
        /// Sets the selected option color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetSelectedOptionColor(Color? color) =>
            selectedOptionColor = color;

        /// <summary>
        /// Separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor ?? ThemeColorsTools.GetColor(ThemeColorType.Separator);
            set => SetSeparatorColor(value);
        }

        /// <summary>
        /// Sets the separator color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetSeparatorColor(Color? color) =>
            separatorColor = color;

        /// <summary>
        /// Text color
        /// </summary>
        public Color TextColor
        {
            get => textColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
            set => SetTextColor(value);
        }

        /// <summary>
        /// Sets the text color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetTextColor(Color? color) =>
            textColor = color;

        /// <summary>
        /// Disabled option color
        /// </summary>
        public Color DisabledOptionColor
        {
            get => disabledOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.DisabledOption);
            set => SetDisabledOptionColor(value);
        }

        /// <summary>
        /// Sets the disabled option color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetDisabledOptionColor(Color? color) =>
            disabledOptionColor = color;

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.Background);
            set => SetBackgroundColor(value);
        }

        /// <summary>
        /// Sets the background color of the selection style
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetBackgroundColor(Color? color) =>
            backgroundColor = color;

        /// <summary>
        /// Makes a new instance of the selection style settings
        /// </summary>
        public SelectionStyleSettings() :
            this(globalSettings)
        { }

        /// <summary>
        /// Makes a new instance of the selection style settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public SelectionStyleSettings(SelectionStyleSettings settings)
        {
            if (settings is null)
                return;

            RadioButtons = settings.RadioButtons;
            SetQuestionColor(settings.questionColor);
            SetSliderColor(settings.sliderColor);
            SetInputColor(settings.inputColor);
            SetOptionColor(settings.optionColor);
            SetAltOptionColor(settings.altOptionColor);
            SetSelectedOptionColor(settings.selectedOptionColor);
            SetSeparatorColor(settings.separatorColor);
            SetTextColor(settings.textColor);
            SetDisabledOptionColor(settings.disabledOptionColor);
            SetBackgroundColor(settings.BackgroundColor);
        }
    }
}
