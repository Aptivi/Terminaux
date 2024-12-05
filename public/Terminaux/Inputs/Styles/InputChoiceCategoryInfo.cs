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

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Input choice category info
    /// </summary>
    public class InputChoiceCategoryInfo
    {
        private readonly string categoryName = "";
        private readonly InputChoiceGroupInfo[] groups = [];

        /// <summary>
        /// Input choice category name
        /// </summary>
        public string Name =>
            categoryName;

        /// <summary>
        /// Input choice groups for this category
        /// </summary>
        public InputChoiceGroupInfo[] Groups =>
            groups;

        /// <summary>
        /// Makes a new instance of input choice category info
        /// </summary>
        /// <param name="categoryName">Category info</param>
        /// <param name="groups">Groups in this category</param>
        public InputChoiceCategoryInfo(string categoryName, InputChoiceGroupInfo[] groups)
        {
            this.categoryName = categoryName;
            this.groups = groups;
        }
    }
}
