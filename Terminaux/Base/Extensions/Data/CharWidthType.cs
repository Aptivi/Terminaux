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

namespace Terminaux.Base.Extensions.Data
{
    /// <summary>
    /// Character width type
    /// </summary>
    public enum CharWidthType
    {
        /// <summary>
        /// Private characters
        /// </summary>
        Private,
        /// <summary>
        /// Non-printing characters
        /// </summary>
        NonPrinting,
        /// <summary>
        /// Combining characters
        /// </summary>
        Combining,
        /// <summary>
        /// Double width characters
        /// </summary>
        DoubleWidth,
        /// <summary>
        /// Ambiguous characters
        /// </summary>
        Ambiguous,
        /// <summary>
        /// Emoji characters
        /// </summary>
        Emoji,
        /// <summary>
        /// Unassigned characters
        /// </summary>
        Unassigned
    }
}
