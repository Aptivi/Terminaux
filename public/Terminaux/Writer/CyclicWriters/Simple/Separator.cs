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

using System.Text;
using Colorimetry;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Separator cyclic renderer
    /// </summary>
    public class Separator : SimpleCyclicWriter
    {
        private bool useColors = true;
        private Color fgColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        private Color bgColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private BorderSettings settings = new(BorderSettings.GlobalSettings);

        /// <summary>
        /// Text to render
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// Whether to extend to console window width or not on non-dumb consoles
        /// </summary>
        public bool Extend { get; set; } = true;

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => fgColor;
            set => fgColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => bgColor;
            set => bgColor = value;
        }

        /// <summary>
        /// Ruler style settings
        /// </summary>
        public BorderSettings Settings
        {
            get => settings;
            set => settings = value;
        }

        /// <summary>
        /// Renders a spinner
        /// </summary>
        /// <returns>A string representation of the spinner</returns>
        public override string Render()
        {
            var separator = new StringBuilder();
            bool canPosition = !ConsoleWrapper.IsDumb && Extend;

            // Print the suffix and the text
            if (!string.IsNullOrWhiteSpace(Text))
            {
                // Render the text accordingly
                Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 8) : Text;
                if (UseColors)
                {
                    separator.Append(
                        ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                separator.Append($"{Settings.BorderHorizontalIntersectionChar}{Settings.BorderRightHorizontalIntersectionChar} {Text} ");
            }

            // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
            int RepeatTimes = 0;
            if (canPosition)
            {
                int width = ConsoleChar.EstimateCellWidth(separator.ToString());
                RepeatTimes = ConsoleWrapper.WindowWidth - width - 1;
            }

            // Write the closing minus sign.
            if (RepeatTimes > 0)
            {
                separator.Append(string.IsNullOrEmpty(Text) ? Settings.BorderHorizontalIntersectionChar : Settings.BorderLeftHorizontalIntersectionChar);
                if (UseColors)
                {
                    separator.Append(
                        ConsoleColoring.RenderSetConsoleColor(ForegroundColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                separator.Append(new string(Settings.BorderHorizontalIntersectionChar, RepeatTimes));
            }

            // Return the resulting separator
            if (UseColors)
            {
                separator.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return separator.ToString();
        }
    }
}
