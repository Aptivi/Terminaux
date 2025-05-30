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

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    /// <summary>
    /// Infobox settings
    /// </summary>
    public class InfoBoxSettings
    {
        private static readonly InfoBoxSettings globalSettings = new();
        internal bool useColors = true;
        internal string title = "";
        internal BorderSettings borderSettings = BorderSettings.GlobalSettings;
        internal Color foregroundColor = new(ConsoleColors.Silver);
        internal Color backgroundColor = ColorTools.currentBackgroundColor;

        /// <summary>
        /// Global infobox settings
        /// </summary>
        public static InfoBoxSettings GlobalSettings =>
            globalSettings;

        /// <summary>
        /// Title of the informational box 
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        /// <summary>
        /// Border settings for the informational box 
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
        }

        /// <summary>
        /// Foreground color of the infobox 
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color of the infobox 
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use the colors or not 
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Use the radio buttons when dealing with selection infoboxes (single-choice)
        /// </summary>
        public bool RadioButtons { get; set; }

        /// <summary>
        /// Makes a new instance of the infobox settings
        /// </summary>
        public InfoBoxSettings()
        { }

        /// <summary>
        /// Makes a new instance of the infobox settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public InfoBoxSettings(InfoBoxSettings settings)
        {
            title = settings.title;
            borderSettings = settings.borderSettings;
            foregroundColor = settings.foregroundColor;
            backgroundColor = settings.backgroundColor;
            useColors = settings.useColors;
            RadioButtons = settings.RadioButtons;
        }
    }
}
