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
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Selection;
using Textify.Data;
using Textify.NameGen;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.Input
{
    internal class TestInputSelectionHuge : IFixture
    {
        public string FixtureID => "TestInputSelectionHuge";
        public void RunFixture()
        {
            DataInitializer.Initialize();
            var choices = new List<InputChoiceInfo>();
            var names = NameGenerator.FindFirstNames("");
            for (int i = 0; i < names.Length; i++)
                choices.Add(new InputChoiceInfo($"{i + 1}", names[i]));
            InputChoiceInfo[] choicesArray = [.. choices];
            SelectionStyle.PromptSelection("Select a choice.", choicesArray, SelectionStyleSettings.GlobalSettings);
        }
    }
}
