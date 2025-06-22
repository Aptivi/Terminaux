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
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI settings
    /// </summary>
    public class InteractiveTuiSettings
    {
        private static readonly InteractiveTuiSettings globalSettings = new();
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiBackground);
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiForeground);
        private Color paneBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneBackground);
        private Color paneSeparatorColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator);
        private Color paneSelectedSeparatorColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator);
        private Color paneSelectedItemForeColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemFore);
        private Color paneSelectedItemBackColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemBack);
        private Color paneItemForeColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemFore);
        private Color paneItemBackColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemBack);
        private Color optionBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground);
        private Color keyBindingOptionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption);
        private Color optionForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground);
        private Color keyBindingBuiltinBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground);
        private Color keyBindingBuiltinColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin);
        private Color keyBindingBuiltinForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground);
        private Color boxBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground);
        private Color boxForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiBoxForeground);
        private InfoBoxSettings infoBoxSettings = new(InfoBoxSettings.GlobalSettings);

        /// <summary>
        /// Global interactive TUI settings
        /// </summary>
        public static InteractiveTuiSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public Color PaneBackgroundColor
        {
            get => paneBackgroundColor;
            set => paneBackgroundColor = value;
        }

        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public Color PaneSeparatorColor
        {
            get => paneSeparatorColor;
            set => paneSeparatorColor = value;
        }

        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public Color PaneSelectedSeparatorColor
        {
            get => paneSelectedSeparatorColor;
            set => paneSelectedSeparatorColor = value;
        }

        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public Color PaneSelectedItemForeColor
        {
            get => paneSelectedItemForeColor;
            set => paneSelectedItemForeColor = value;
        }

        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public Color PaneSelectedItemBackColor
        {
            get => paneSelectedItemBackColor;
            set => paneSelectedItemBackColor = value;
        }

        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public Color PaneItemForeColor
        {
            get => paneItemForeColor;
            set => paneItemForeColor = value;
        }

        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public Color PaneItemBackColor
        {
            get => paneItemBackColor;
            set => paneItemBackColor = value;
        }

        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public Color OptionBackgroundColor
        {
            get => optionBackgroundColor;
            set => optionBackgroundColor = value;
        }

        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public Color KeyBindingOptionColor
        {
            get => keyBindingOptionColor;
            set => keyBindingOptionColor = value;
        }

        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public Color OptionForegroundColor
        {
            get => optionForegroundColor;
            set => optionForegroundColor = value;
        }

        /// <summary>
        /// Interactive TUI built-in key binding background color
        /// </summary>
        public Color KeyBindingBuiltinBackgroundColor
        {
            get => keyBindingBuiltinBackgroundColor;
            set => keyBindingBuiltinBackgroundColor = value;
        }

        /// <summary>
        /// Interactive TUI built-in key binding foreground color in the background color
        /// </summary>
        public Color KeyBindingBuiltinColor
        {
            get => keyBindingBuiltinColor;
            set => keyBindingBuiltinColor = value;
        }

        /// <summary>
        /// Interactive TUI built-in key binding foreground color outside the background color
        /// </summary>
        public Color KeyBindingBuiltinForegroundColor
        {
            get => keyBindingBuiltinForegroundColor;
            set => keyBindingBuiltinForegroundColor = value;
        }

        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public Color BoxBackgroundColor
        {
            get => boxBackgroundColor;
            set => boxBackgroundColor = value;
        }

        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public Color BoxForegroundColor
        {
            get => boxForegroundColor;
            set => boxForegroundColor = value;
        }

        /// <summary>
        /// Infobox settings to use when rendering the interactive TUI
        /// </summary>
        public InfoBoxSettings InfoBoxSettings
        {
            get => infoBoxSettings;
            set => infoBoxSettings = value;
        }

        /// <summary>
        /// Makes a new instance of the interactive TUI settings
        /// </summary>
        public InteractiveTuiSettings()
        { }

        /// <summary>
        /// Makes a new instance of the interactive TUI settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public InteractiveTuiSettings(InteractiveTuiSettings settings)
        {
            BackgroundColor = settings.BackgroundColor;
            ForegroundColor = settings.ForegroundColor;
            KeyBindingOptionColor = settings.KeyBindingOptionColor;
            KeyBindingBuiltinColor = settings.KeyBindingBuiltinColor;
            KeyBindingBuiltinForegroundColor = settings.KeyBindingBuiltinForegroundColor;
            KeyBindingBuiltinBackgroundColor = settings.KeyBindingBuiltinBackgroundColor;
            OptionForegroundColor = settings.OptionForegroundColor;
            OptionBackgroundColor = settings.OptionBackgroundColor;
            PaneItemForeColor = settings.PaneItemForeColor;
            PaneItemBackColor = settings.PaneItemBackColor;
            PaneSeparatorColor = settings.PaneSeparatorColor;
            PaneSelectedItemForeColor = settings.PaneSelectedItemForeColor;
            PaneSelectedItemBackColor = settings.PaneSelectedItemBackColor;
            PaneSelectedSeparatorColor = settings.PaneSelectedSeparatorColor;
            PaneBackgroundColor = settings.PaneBackgroundColor;
            BoxBackgroundColor = settings.BoxBackgroundColor;
            BoxForegroundColor = settings.BoxForegroundColor;
            InfoBoxSettings = settings.InfoBoxSettings;
        }
    }
}
