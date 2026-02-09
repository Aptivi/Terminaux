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
        private Color? foregroundColor;
        private Color? backgroundColor;
        private InfoBoxPositioning positioning = InfoBoxPositioning.GlobalSettings;
        private char passwordMaskChar = Input.PasswordMaskChar;

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
        /// Use popover for multi-input selections
        /// </summary>
        public bool UsePopover { get; set; } = true;

        /// <summary>
        /// Foreground color of the infobox 
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
            set => SetForegroundColor(value);
        }

        /// <summary>
        /// Sets the foreground color of the infobox
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetForegroundColor(Color? color) =>
            foregroundColor = color;

        /// <summary>
        /// Background color of the infobox 
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor ?? ThemeColorsTools.GetColor(ThemeColorType.Background);
            set => SetBackgroundColor(value);
        }

        /// <summary>
        /// Positioning settings
        /// </summary>
        public InfoBoxPositioning Positioning
        {
            get => positioning;
            set => positioning = value;
        }

        /// <summary>
        /// Sets the background color of the infobox
        /// </summary>
        /// <param name="color">Color to set. If null, inherits the color from the theme.</param>
        public void SetBackgroundColor(Color? color) =>
            backgroundColor = color;

        /// <summary>
        /// Password mask character
        /// </summary>
        public char PasswordMaskChar
        {
            get => passwordMaskChar;
            set => passwordMaskChar = value;
        }

        /// <summary>
        /// Makes a new instance of the infobox settings
        /// </summary>
        public InfoBoxSettings() :
            this(globalSettings)
        { }

        /// <summary>
        /// Makes a new instance of the infobox settings with the copied settings
        /// </summary>
        /// <param name="settings">Settings instance to copy settings from</param>
        public InfoBoxSettings(InfoBoxSettings settings)
        {
            if (settings is null)
                return;

            title = settings.title;
            borderSettings = settings.borderSettings;
            useColors = settings.useColors;
            RadioButtons = settings.RadioButtons;
            Positioning = settings.Positioning;
            PasswordMaskChar = settings.PasswordMaskChar;
            SetForegroundColor(settings.foregroundColor);
            SetBackgroundColor(settings.backgroundColor);
        }
    }
}
