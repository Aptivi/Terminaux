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

using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;

namespace Terminaux.Graphics.NerdFonts
{
    /// <summary>
    /// Nerd Fonts tools
    /// </summary>
    public static partial class NerdFontsTools
    {
        /// <summary>
        /// Gets the Nerd Fonts character
        /// </summary>
        /// <param name="type">Nerd Fonts type</param>
        /// <param name="charName">Character name</param>
        /// <returns>Nerd Fonts character from <paramref name="charName"/> from the specified <paramref name="type"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static char GetNerdFontChar(NerdFontsTypes type, string charName)
        {
            var nerdEntries = VerifyEntries(type);
            if (!nerdEntries.TryGetValue(charName, out string character))
                throw new TerminauxException($"Invalid Nerd Fonts character name {charName} in type {type}");
            if (!char.TryParse(character, out char c))
                throw new TerminauxException($"Invalid Nerd Fonts character representation {character} from {charName} in type {type}");
            return c;
        }

        /// <summary>
        /// Gets the Nerd Fonts character names
        /// </summary>
        /// <param name="type">Nerd Fonts type</param>
        /// <returns>Nerd Fonts character name list from the specified <paramref name="type"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static string[] GetNerdFontCharNames(NerdFontsTypes type)
        {
            var nerdEntries = VerifyEntries(type);
            return [.. nerdEntries.Keys];
        }

        private static Dictionary<string, string> VerifyEntries(NerdFontsTypes type)
        {
            if (!nerdFontChars.TryGetValue(type, out var nerdEntries))
                throw new TerminauxException($"Invalid Nerd Fonts type {type}");
            if (nerdEntries.Count == 0)
                throw new TerminauxException($"Nerd Fonts type {type} has no characters");
            return nerdEntries;
        }
    }
}
