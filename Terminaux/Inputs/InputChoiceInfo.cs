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

using System.Diagnostics;

namespace Terminaux.Inputs
{
    /// <summary>
    /// Choice information for input
    /// </summary>
    [DebuggerDisplay("{ChoiceName} - {ChoiceTitle} - D:{ChoiceDefault} S:{ChoiceDefaultSelected} X:{ChoiceDisabled}")]
    public class InputChoiceInfo
    {
        /// <summary>
        /// Choice name
        /// </summary>
        public string ChoiceName { get; }

        /// <summary>
        /// Choice title
        /// </summary>
        public string ChoiceTitle { get; }

        /// <summary>
        /// Choice description
        /// </summary>
        public string ChoiceDescription { get; }

        /// <summary>
        /// Whether this choice is the default choice or not. If multiple choices in the same choice list have <see cref="ChoiceDefault"/>
        /// set to true, most of the tools will automatically select the first default, ignoring the rest.
        /// </summary>
        public bool ChoiceDefault { get; }

        /// <summary>
        /// Whether this choice is the selected choice by default or not.
        /// </summary>
        public bool ChoiceDefaultSelected { get; }

        /// <summary>
        /// Whether this choice is disabled or not.
        /// </summary>
        public bool ChoiceDisabled { get; }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        public InputChoiceInfo(string choiceName, string choiceTitle)
            : this(choiceName, choiceTitle, "", false, false)
        { }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        /// <param name="choiceDescription">Choice description</param>
        public InputChoiceInfo(string choiceName, string choiceTitle, string choiceDescription)
            : this(choiceName, choiceTitle, choiceDescription, false, false)
        { }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        /// <param name="choiceDescription">Choice description</param>
        /// <param name="choiceDefault">Whether this choice is the default choice or not</param>
        public InputChoiceInfo(string choiceName, string choiceTitle, string choiceDescription, bool choiceDefault)
            : this(choiceName, choiceTitle, choiceDescription, choiceDefault, false, false)
        { }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        /// <param name="choiceDescription">Choice description</param>
        /// <param name="choiceDefault">Whether this choice is the default choice or not</param>
        /// <param name="choiceDefaultSelected">Whether this choice is the selected choice by default</param>
        public InputChoiceInfo(string choiceName, string choiceTitle, string choiceDescription, bool choiceDefault, bool choiceDefaultSelected)
            : this(choiceName, choiceTitle, choiceDescription, choiceDefault, choiceDefaultSelected, false)
        { }

        /// <summary>
        /// Makes a new instance of choice information
        /// </summary>
        /// <param name="choiceName">Choice name</param>
        /// <param name="choiceTitle">Choice title</param>
        /// <param name="choiceDescription">Choice description</param>
        /// <param name="choiceDefault">Whether this choice is the default choice or not</param>
        /// <param name="choiceDefaultSelected">Whether this choice is the selected choice by default</param>
        /// <param name="choiceDisabled">Whether this choice is disabled or not</param>
        public InputChoiceInfo(string choiceName, string choiceTitle, string choiceDescription, bool choiceDefault, bool choiceDefaultSelected, bool choiceDisabled)
        {
            ChoiceName = choiceName;
            ChoiceTitle = choiceTitle;
            ChoiceDescription = choiceDescription;
            ChoiceDisabled = choiceDisabled;
            ChoiceDefault = !ChoiceDisabled && choiceDefault;
            ChoiceDefaultSelected = choiceDefaultSelected;
        }
    }
}
