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
    /// Input choice group info
    /// </summary>
    public class InputChoiceGroupInfo
    {
        private readonly string groupName = "";
        private readonly InputChoiceInfo[] choices = [];

        /// <summary>
        /// Input choice group name
        /// </summary>
        public string Name =>
            groupName;

        /// <summary>
        /// Input choices for this group
        /// </summary>
        public InputChoiceInfo[] Choices =>
            choices;

        /// <summary>
        /// Makes a new instance of input choice group info
        /// </summary>
        /// <param name="groupName">Group info</param>
        /// <param name="choices">Choices in this group</param>
        public InputChoiceGroupInfo(string groupName, InputChoiceInfo[] choices)
        {
            this.groupName = groupName;
            this.choices = choices;
        }
    }
}
