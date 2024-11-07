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

using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Nerd Fonts renderer
    /// </summary>
    public class NerdFonts : IStaticRenderable
    {
        private readonly string nerdfonts = "";

        /// <summary>
        /// Renders a Nerd Fonts glyph
        /// </summary>
        /// <returns>A string representation of the Nerd Fonts glyph</returns>
        public string Render() =>
            nerdfonts;

        /// <summary>
        /// Makes a new nerdfonts instance
        /// </summary>
        /// <param name="type">Nerd Fonts type</param>
        /// <param name="charName">Character name</param>
        public NerdFonts(NerdFontsTypes type, string charName) =>
            nerdfonts = NerdFontsTools.GetNerdFontChar(type, charName);
    }
}
