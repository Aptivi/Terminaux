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
        public static int FirstPaneCurrentSelection { get; set; } = 1;
        /// <summary>
        /// Current selection for the second pane
        /// </summary>
        public static int SecondPaneCurrentSelection { get; set; } = 1;
        /// <summary>
        /// Current status
        /// </summary>
        public static string Status { get; set; } = "";
        /// <summary>
        /// Current pane
        /// </summary>
        public static int CurrentPane { get; set; } = 1;

        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public static Color BackgroundColor { get; set; } = "0";
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public static Color ForegroundColor { get; set; } = "11";
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public static Color PaneBackgroundColor { get; set; } = "0";
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public static Color PaneSeparatorColor { get; set; } = "2";
        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        public static Color PaneSelectedSeparatorColor { get; set; } = "10";
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        public static Color PaneSelectedItemForeColor { get; set; } = "0";
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        public static Color PaneSelectedItemBackColor { get; set; } = "3";
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        public static Color PaneItemForeColor { get; set; } = "3";
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        public static Color PaneItemBackColor { get; set; } = "0";
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public static Color OptionBackgroundColor { get; set; } = "3";
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        public static Color KeyBindingOptionColor { get; set; } = "0";
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public static Color OptionForegroundColor { get; set; } = "11";
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public static Color BoxBackgroundColor { get; set; } = "9";
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public static Color BoxForegroundColor { get; set; } = "15";
    }
}
