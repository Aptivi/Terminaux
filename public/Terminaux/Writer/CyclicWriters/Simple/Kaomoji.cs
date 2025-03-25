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

using Textify.Data.Unicode;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Kaomoji renderer
    /// </summary>
    public class Kaomoji : SimpleCyclicWriter
    {
        private readonly string kaomoji = "";

        /// <summary>
        /// Renders kaomoji
        /// </summary>
        /// <returns>A string representation of the kaomoji</returns>
        public override string Render() =>
            kaomoji;

        /// <summary>
        /// Makes a new kaomoji instance
        /// </summary>
        /// <param name="category">Kaomoji category</param>
        /// <param name="subcategory">Kaomoji subcategory</param>
        /// <param name="idx">Kaomoji index</param>
        public Kaomoji(KaomojiCategory category, KaomojiSubcategory subcategory, int idx) =>
            kaomoji = KaomojiManager.GetKaomoji(category, subcategory, idx);
    }
}
