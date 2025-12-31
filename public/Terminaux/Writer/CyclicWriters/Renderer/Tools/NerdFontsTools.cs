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

using System.Collections.Generic;
using Terminaux.Base;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
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
        public static string GetNerdFontChar(NerdFontsTypes type, string charName)
        {
            var nerdEntries = VerifyEntries(type);
            if (!nerdEntries.TryGetValue(charName, out string character))
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_NERDFONTS_EXCEPTION_CHARINVALID"), charName, type.ToString());
            if (!char.TryParse(character, out char c))
            {
                // We may have surrogate pair based Nerd Fonts character, so we need to process it.
                if (character.Length == 2 && char.IsSurrogatePair(character[0], character[1]))
                    return character;
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_NERDFONTS_EXCEPTION_CHARREPRESENTATIONINVALID"), character, charName, type.ToString());
            }
            return $"{c}";
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_NERDFONTS_EXCEPTION_TYPEINVALID"), type);
            if (nerdEntries.Count == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_TOOLS_NERDFONTS_EXCEPTION_TYPENOCHARS"), type.ToString());
            return nerdEntries;
        }
    }
}
