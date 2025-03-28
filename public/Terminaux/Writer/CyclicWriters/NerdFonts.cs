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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Nerd Fonts renderer
    /// </summary>
    public class NerdFonts : IStaticRenderable
    {
        private readonly string nerdfonts = "";
        private bool useColors = true;
        private Color fgColor = ColorTools.CurrentForegroundColor;
        private Color bgColor = ColorTools.CurrentBackgroundColor;

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
        /// Renders a Nerd Fonts glyph
        /// </summary>
        /// <returns>A string representation of the Nerd Fonts glyph</returns>
        public string Render() =>
            (UseColors ? ColorTools.RenderSetConsoleColor(ForegroundColor) : "") +
            (UseColors ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "") +
            nerdfonts +
            (UseColors ? ColorTools.RenderResetBackground() : "") +
            (UseColors ? ColorTools.RenderResetForeground() : "");

        /// <summary>
        /// Makes a new Nerd Fonts instance
        /// </summary>
        /// <param name="type">Nerd Fonts type</param>
        /// <param name="charName">Character name</param>
        public NerdFonts(NerdFontsTypes type, string charName) =>
            nerdfonts = NerdFontsTools.GetNerdFontChar(type, charName);
    }
}
