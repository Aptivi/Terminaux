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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Console.Fixtures;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console
{
    internal class Entry
    {
        static void Main()
        {
            // Run the resize listener
            ConsoleResizeHandler.StartResizeListener();

            // Initialize sequences for Windows
            ConsolePositioning.InitializeSequences();

            // Prepare the fixtures
            var fixtureNames = FixtureManager.GetFixtureNames();
            List<InputChoiceInfo> choices = [];
            List<InputChoiceInfo> altChoices =
            [
                new($"{fixtureNames.Length + 1}", "Exit")
            ];
            for (int i = 0; i < fixtureNames.Length; i++)
            {
                string fixtureName = fixtureNames[i];
                choices.Add(new($"{i + 1}", fixtureName));
            }

            // Prompt for fixtures
            while (true)
            {
                int selected = SelectionStyle.PromptSelection("Choose a fixture", [.. choices], [.. altChoices], true);
                if (selected == fixtureNames.Length + 1)
                    break;

                // Get the fixture name from the selection and run it
                string chosenFixture = fixtureNames[selected - 1];
                TextWriterColor.Write($"Fixture to be tested: {chosenFixture}\n");
                FixtureManager.GetFixtureFromName(chosenFixture).RunFixture();
                Input.ReadKey();
            }
            ConsoleClearing.ResetAll();
            ConsoleWrapper.CursorVisible = true;
        }
    }
}
