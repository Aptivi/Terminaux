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
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.TestFixtures.Tools
{
    /// <summary>
    /// Fixture selector initializer
    /// </summary>
    public static class FixtureSelector
    {
        /// <summary>
        /// Opens the fixture selector
        /// </summary>
        /// <param name="fixtures">List of fixtures to test (must specify initial parameters for parameterized fixtures)</param>
        public static void OpenFixtureSelector(Fixture[]? fixtures)
        {
            // Check if we have fixtures or not
            if (fixtures is null || fixtures.Length == 0)
                throw new TerminauxException("No fixtures here. Insert one.");

            // Now, make choices out of them and present it to the user as a selection choice
            int[] statuses = new int[fixtures.Length];
            while (true)
            {
                var choices = FlattenFixturesIntoChoices(fixtures, statuses);

                // Let the user select a test fixture
                int selectedIndex = SelectionStyle.PromptSelection("Choose a test fixture.", choices, [new InputChoiceInfo("Exit", "Exits the test fixture selector")]);
                if (selectedIndex == -1 || selectedIndex == choices.Length + 1)
                    break;

                // Determine what fixture we're going to run, and run it
                var fixture = fixtures[selectedIndex - 1];
                ConsoleLogger.Debug("Running fixture {0}...", selectedIndex);
                bool result = FixtureRunner.RunGeneralTest(fixture, out var exc, fixture.initialParameters);
                if (result)
                {
                    TextWriterColor.WriteColor("This test has succeeded!", ConsoleColors.Lime);
                    if (fixture.GetType() == typeof(FixtureConditional) || fixture.GetType().BaseType == typeof(FixtureConditional))
                        TextWriterColor.WriteColor("Expected value matches actual value.", ConsoleColors.Lime);
                }
                else
                {
                    TextWriterColor.WriteColor("This test has failed.", ConsoleColors.Red);
                    TextWriterColor.WriteColor("Message: " + exc?.Message ?? "Unknown error", ConsoleColors.Red);
                    if (fixture.GetType() == typeof(FixtureConditional) || fixture.GetType().BaseType == typeof(FixtureConditional))
                        TextWriterColor.WriteColor("Expected value doesn't match actual value.", ConsoleColors.Red);
                }
                statuses[selectedIndex - 1] = result ? 1 : 2;
                Input.ReadKey();
            }
        }

        private static InputChoiceInfo[] FlattenFixturesIntoChoices(Fixture[] fixtures, int[] statuses)
        {
            List<InputChoiceInfo> choices = [];
            for (int i = 0; i < fixtures.Length; i++)
            {
                Fixture fixture = fixtures[i];
                string status = statuses[i] == 1 ? "[*]" : statuses[i] == 2 ? "[X]" : "[ ]";
                string name = status + " " + fixture.Name;
                string description = fixture.Description;
                var choiceInfo = new InputChoiceInfo(name, description);
                choices.Add(choiceInfo);
            }
            return [.. choices];
        }
    }
}
