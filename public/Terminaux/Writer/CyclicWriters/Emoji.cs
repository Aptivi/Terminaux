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

using Textify.Data.Unicode;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Emoji renderer
    /// </summary>
    public class Emoji : IStaticRenderable
    {
        private readonly string emoji = "";

        /// <summary>
        /// Renders an emoji
        /// </summary>
        /// <returns>A string representation of the emoji</returns>
        public string Render() =>
            emoji;

        /// <summary>
        /// Makes a new emoji instance
        /// </summary>
        /// <param name="emoji">Emoji from an enumeration</param>
        public Emoji(EmojiEnum emoji) =>
            this.emoji = EmojiManager.GetEmojiFromEnum(emoji).Sequence;
    }
}
