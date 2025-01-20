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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI settings
    /// </summary>
    public class InteractiveTuiSettings
    {
        private static readonly InteractiveTuiSettings globalSettings = new();
        private Color backgroundColor = ConsoleColors.Black;
        private Color foregroundColor = ConsoleColors.Yellow;
        private Color paneBackgroundColor = ConsoleColors.Black;
        private Color paneSeparatorColor = ConsoleColors.Green;
        private Color paneSelectedSeparatorColor = ConsoleColors.Lime;
        private Color paneSelectedItemForeColor = ConsoleColors.Black;
        private Color paneSelectedItemBackColor = ConsoleColors.Olive;
        private Color paneItemForeColor = ConsoleColors.Olive;
        private Color paneItemBackColor = ConsoleColors.Black;
        private Color optionBackgroundColor = ConsoleColors.Olive;
        private Color keyBindingOptionColor = ConsoleColors.Black;
        private Color optionForegroundColor = ConsoleColors.Yellow;
        private Color keyBindingBuiltinBackgroundColor = ConsoleColors.Green;
        private Color keyBindingBuiltinColor = ConsoleColors.Black;
        private Color keyBindingBuiltinForegroundColor = ConsoleColors.Lime;
        private Color boxBackgroundColor = ConsoleColors.Black;
        private Color boxForegroundColor = ConsoleColors.Yellow;
        private BorderSettings borderSettings = BorderSettings.GlobalSettings;

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
        /// Border settings to use when rendering the interactive TUI
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
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
            BoxBackgroundColor = settings.BoxBackgroundColor;
            BoxForegroundColor = settings.BoxForegroundColor;
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
            BorderSettings = settings.BorderSettings;
        }
    }
}
