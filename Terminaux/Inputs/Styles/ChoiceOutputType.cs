
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace Terminaux.Reader.Inputs.Styles
{
    /// <summary>
    /// The enumeration for the choice command output type
    /// </summary>
    public enum ChoiceOutputType
    {
        /// <summary>
        /// A question and a set of answers in one line
        /// </summary>
        OneLine,
        /// <summary>
        /// A question in a line and a set of answers in another line
        /// </summary>
        TwoLines,
        /// <summary>
        /// The modern way of listing choices
        /// </summary>
        Modern,
        /// <summary>
        /// The table of choices
        /// </summary>
        Table
    }
}
