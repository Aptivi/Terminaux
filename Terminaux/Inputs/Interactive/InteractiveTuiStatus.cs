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

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Interactive TUI status
    /// </summary>
    public static class InteractiveTuiStatus
    {
        /// <summary>
        /// Current selection for the first pane
        /// </summary>
        public static int FirstPaneCurrentSelection { get; internal set; } = 1;
        /// <summary>
        /// Current selection for the second pane
        /// </summary>
        public static int SecondPaneCurrentSelection { get; internal set; } = 1;
        /// <summary>
        /// Current selection for the current pane
        /// </summary>
        public static int CurrentSelection =>
            CurrentPane == 2 ? SecondPaneCurrentSelection : FirstPaneCurrentSelection;
        /// <summary>
        /// Current status
        /// </summary>
        public static string Status { get; internal set; } = "";
        /// <summary>
        /// Current pane
        /// </summary>
        public static int CurrentPane { get; internal set; } = 1;
        /// <summary>
        /// Current info line index
        /// </summary>
        public static int CurrentInfoLine { get; internal set; } = 0;

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public static Color BackgroundColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public static Color ForegroundColor { get; set; } = ConsoleColors.Yellow;
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public static Color PaneBackgroundColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public static Color PaneSeparatorColor { get; set; } = ConsoleColors.Green;
        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public static Color PaneSelectedSeparatorColor { get; set; } = ConsoleColors.Lime;
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public static Color PaneSelectedItemForeColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public static Color PaneSelectedItemBackColor { get; set; } = ConsoleColors.Olive;
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public static Color PaneItemForeColor { get; set; } = ConsoleColors.Olive;
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public static Color PaneItemBackColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public static Color OptionBackgroundColor { get; set; } = ConsoleColors.Olive;
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public static Color KeyBindingOptionColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public static Color OptionForegroundColor { get; set; } = ConsoleColors.Yellow;
        /// <summary>
        /// Interactive TUI built-in key binding background color
        /// </summary>
        public static Color KeyBindingBuiltinBackgroundColor { get; set; } = ConsoleColors.Green;
        /// <summary>
        /// Interactive TUI built-in key binding foreground color in the background color
        /// </summary>
        public static Color KeyBindingBuiltinColor { get; set; } = ConsoleColors.Black;
        /// <summary>
        /// Interactive TUI built-in key binding foreground color outside the background color
        /// </summary>
        public static Color KeyBindingBuiltinForegroundColor { get; set; } = ConsoleColors.Lime;
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public static Color BoxBackgroundColor { get; set; } = ConsoleColors.Red;
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public static Color BoxForegroundColor { get; set; } = ConsoleColors.White;
    }
}
