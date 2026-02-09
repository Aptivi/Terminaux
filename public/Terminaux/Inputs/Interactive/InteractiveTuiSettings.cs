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
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Themes.Colors;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI settings
    /// </summary>
    public class InteractiveTuiSettings
    {
        private static readonly InteractiveTuiSettings globalSettings = new();
        private Color? backgroundColor;
        private Color? foregroundColor;
        private Color? paneBackgroundColor;
        private Color? paneSeparatorColor;
        private Color? paneSelectedSeparatorColor;
        private Color? paneSelectedItemForeColor;
        private Color? paneSelectedItemBackColor;
        private Color? paneItemForeColor;
        private Color? paneItemBackColor;
        private Color? optionBackgroundColor;
        private Color? keyBindingOptionColor;
        private Color? optionForegroundColor;
        private Color? keyBindingBuiltinBackgroundColor;
        private Color? keyBindingBuiltinColor;
        private Color? keyBindingBuiltinForegroundColor;
        private Color? boxBackgroundColor;
        private Color? boxForegroundColor;
        private InfoBoxSettings infoBoxSettings = new(InfoBoxSettings.GlobalSettings);

        /// <summary>
        /// Global interactive TUI settings
        /// </summary>
        public static InteractiveTuiSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Infobox settings to use when rendering the interactive TUI
        /// </summary>
        public InfoBoxSettings InfoBoxSettings
        {
            get => infoBoxSettings;
            set => infoBoxSettings = value;
        }

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiBackground);
            set => SetBackgroundColor(value);
        }

        /// <summary>
        /// Sets the background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetBackgroundColor(Color? color) =>
            backgroundColor = color;

        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiForeground);
            set => SetForegroundColor(value);
        }

        /// <summary>
        /// Sets the foreground color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetForegroundColor(Color? color) =>
            foregroundColor = color;

        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public Color PaneBackgroundColor
        {
            get => paneBackgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneBackground);
            set => SetPaneBackgroundColor(value);
        }

        /// <summary>
        /// Sets the pane background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneBackgroundColor(Color? color) =>
            paneBackgroundColor = color;

        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public Color PaneSeparatorColor
        {
            get => paneSeparatorColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator);
            set => SetPaneSeparatorColor(value);
        }

        /// <summary>
        /// Sets the pane separator color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneSeparatorColor(Color? color) =>
            paneSeparatorColor = color;

        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public Color PaneSelectedSeparatorColor
        {
            get => paneSelectedSeparatorColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator);
            set => SetPaneSelectedSeparatorColor(value);
        }

        /// <summary>
        /// Sets the pane selected separator color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneSelectedSeparatorColor(Color? color) =>
            paneSelectedSeparatorColor = color;

        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public Color PaneSelectedItemForeColor
        {
            get => paneSelectedItemForeColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemFore);
            set => SetPaneSelectedItemForeColor(value);
        }

        /// <summary>
        /// Sets the pane selected item foreground color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneSelectedItemForeColor(Color? color) =>
            paneSelectedItemForeColor = color;

        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public Color PaneSelectedItemBackColor
        {
            get => paneSelectedItemBackColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemBack);
            set => SetPaneSelectedItemBackColor(value);
        }

        /// <summary>
        /// Sets the pane selected item background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneSelectedItemBackColor(Color? color) =>
            paneSelectedItemBackColor = color;

        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public Color PaneItemForeColor
        {
            get => paneItemForeColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemFore);
            set => SetPaneItemForeColor(value);
        }

        /// <summary>
        /// Sets the pane item foreground color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneItemForeColor(Color? color) =>
            paneItemForeColor = color;

        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public Color PaneItemBackColor
        {
            get => paneItemBackColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemBack);
            set => SetPaneItemBackColor(value);
        }

        /// <summary>
        /// Sets the pane item background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetPaneItemBackColor(Color? color) =>
            paneItemBackColor = color;

        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public Color OptionBackgroundColor
        {
            get => optionBackgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground);
            set => SetOptionBackgroundColor(value);
        }

        /// <summary>
        /// Sets the option background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetOptionBackgroundColor(Color? color) =>
            optionBackgroundColor = color;

        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public Color KeyBindingOptionColor
        {
            get => keyBindingOptionColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption);
            set => SetKeyBindingOptionColor(value);
        }

        /// <summary>
        /// Sets the key binding in option color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetKeyBindingOptionColor(Color? color) =>
            keyBindingOptionColor = color;

        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public Color OptionForegroundColor
        {
            get => optionForegroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground);
            set => SetOptionForegroundColor(value);
        }

        /// <summary>
        /// Sets the option foreground color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetOptionForegroundColor(Color? color) =>
            optionForegroundColor = color;

        /// <summary>
        /// Interactive TUI built-in key binding background color
        /// </summary>
        public Color KeyBindingBuiltinBackgroundColor
        {
            get => keyBindingBuiltinBackgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground);
            set => SetKeyBindingBuiltinBackgroundColor(value);
        }

        /// <summary>
        /// Sets the built-in key binding background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetKeyBindingBuiltinBackgroundColor(Color? color) =>
            keyBindingBuiltinBackgroundColor = color;

        /// <summary>
        /// Interactive TUI built-in key binding foreground color in the background color
        /// </summary>
        public Color KeyBindingBuiltinColor
        {
            get => keyBindingBuiltinColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin);
            set => SetKeyBindingBuiltinColor(value);
        }

        /// <summary>
        /// Sets the built-in key binding foreground color in the background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetKeyBindingBuiltinColor(Color? color) =>
            keyBindingBuiltinColor = color;

        /// <summary>
        /// Interactive TUI built-in key binding foreground color outside the background color
        /// </summary>
        public Color KeyBindingBuiltinForegroundColor
        {
            get => keyBindingBuiltinForegroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground);
            set => SetKeyBindingBuiltinForegroundColor(value);
        }

        /// <summary>
        /// Sets the built-in key binding foreground color outside the background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetKeyBindingBuiltinForegroundColor(Color? color) =>
            keyBindingBuiltinForegroundColor = color;

        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public Color BoxBackgroundColor
        {
            get => boxBackgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground);
            set => SetBoxBackgroundColor(value);
        }

        /// <summary>
        /// Sets the box background color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetBoxBackgroundColor(Color? color) =>
            boxBackgroundColor = color;

        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public Color BoxForegroundColor
        {
            get => boxForegroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.TuiBoxForeground);
            set => SetBoxForegroundColor(value);
        }

        /// <summary>
        /// Sets the box foreground color of the interactive TUI
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetBoxForegroundColor(Color? color) =>
            boxForegroundColor = color;

        /// <summary>
        /// Makes a new instance of the interactive TUI settings
        /// </summary>
        public InteractiveTuiSettings() :
            this(globalSettings)
        { }

        /// <summary>
        /// Makes a new instance of the interactive TUI settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public InteractiveTuiSettings(InteractiveTuiSettings settings)
        {
            if (settings is null)
                return;

            InfoBoxSettings = settings.InfoBoxSettings;
            SetBackgroundColor(settings.backgroundColor);
            SetForegroundColor(settings.foregroundColor);
            SetKeyBindingOptionColor(settings.keyBindingOptionColor);
            SetKeyBindingBuiltinColor(settings.keyBindingBuiltinColor);
            SetKeyBindingBuiltinForegroundColor(settings.keyBindingBuiltinForegroundColor);
            SetKeyBindingBuiltinBackgroundColor(settings.keyBindingBuiltinBackgroundColor);
            SetOptionForegroundColor(settings.optionForegroundColor);
            SetOptionBackgroundColor(settings.optionBackgroundColor);
            SetPaneItemForeColor(settings.paneItemForeColor);
            SetPaneItemBackColor(settings.paneItemBackColor);
            SetPaneSeparatorColor(settings.paneSeparatorColor);
            SetPaneSelectedItemForeColor(settings.paneSelectedItemForeColor);
            SetPaneSelectedItemBackColor(settings.paneSelectedItemBackColor);
            SetPaneSelectedSeparatorColor(settings.paneSelectedSeparatorColor);
            SetPaneBackgroundColor(settings.paneBackgroundColor);
            SetBoxBackgroundColor(settings.boxBackgroundColor);
            SetBoxForegroundColor(settings.boxForegroundColor);
        }
    }
}
