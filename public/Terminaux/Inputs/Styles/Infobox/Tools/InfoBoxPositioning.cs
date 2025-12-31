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

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    /// <summary>
    /// Positioning class for infoboxes
    /// </summary>
    public class InfoBoxPositioning
    {
        private static readonly InfoBoxPositioning globalSettings = new();

        /// <summary>
        /// Global infobox positioning settings
        /// </summary>
        public static InfoBoxPositioning GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Whether to automatically fit the elements or not
        /// </summary>
        public bool Autofit { get; set; } = true;

        /// <summary>
        /// Left position of the informational box (<see cref="Autofit"/> must be turned off)
        /// </summary>
        public int Left { get; set; } = 0;

        /// <summary>
        /// Top position of the informational box (<see cref="Autofit"/> must be turned off)
        /// </summary>
        public int Top { get; set; } = 0;

        /// <summary>
        /// Width of the informational box (<see cref="Autofit"/> must be turned off)
        /// </summary>
        public int Width { get; set; } = 50;

        /// <summary>
        /// Height of the informational box (<see cref="Autofit"/> must be turned off, does not include extra height)
        /// </summary>
        public int Height { get; set; } = 5;

        /// <summary>
        /// Reserved height for elements that will be placed after the information text
        /// </summary>
        public int ExtraHeight { get; set; } = 0;

        /// <summary>
        /// Creates a new instance of infobox positioning class with default settings
        /// </summary>
        public InfoBoxPositioning()
        { }

        /// <summary>
        /// Creates a new instance of infobox positioning class with copied settings
        /// </summary>
        /// <param name="positioning">Positioning settings to copy from</param>
        public InfoBoxPositioning(InfoBoxPositioning positioning)
        {
            Autofit = positioning.Autofit;
            Left = positioning.Left;
            Top = positioning.Top;
            Width = positioning.Width;
            Height = positioning.Height;
            ExtraHeight = positioning.ExtraHeight;
        }
    }
}
