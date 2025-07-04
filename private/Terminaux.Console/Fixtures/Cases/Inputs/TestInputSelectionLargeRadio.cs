﻿//
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

using System.Collections.Generic;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestInputSelectionLargeRadio : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;

            var choices = new List<InputChoiceInfo>();
            for (int i = 0; i < 1000; i++)
                choices.Add(new InputChoiceInfo($"{i + 1}", $"Number #{i + 1}"));
            InputChoiceInfo[] choicesArray = [.. choices];
            SelectionStyle.PromptSelection("Select a choice.", choicesArray, new SelectionStyleSettings()
            {
                RadioButtons = true,
            });
            Input.EnableMouse = false;
        }
    }
}
