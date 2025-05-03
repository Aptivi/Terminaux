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

using Aptivestigate.CrashHandler;
using Aptivestigate.Logging;
using Aptivestigate.Serilog;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Console.Fixtures;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console
{
    internal class Entry
    {
        static void Main(string[] args)
        {
            // Initialize logging and crash logging
            if (args.Contains("-verbose"))
            {
                ConsoleLogger.AbstractLogger = new SerilogLogger(new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(LogTools.GenerateLogFilePath(out _)));
                ConsoleLogger.EnableLogging = true;
            }
            CrashTools.InstallCrashHandler();

            // Run the resize listener
            ConsoleResizeHandler.StartResizeListener();

            // Initialize sequences for Windows
            ConsoleMisc.InitializeSequences();

            // Prepare the fixtures
            var fixtures = FixtureManager.fixtures.OrderBy((fixture) => fixture.Category).ToArray();
            var fixtureNames = fixtures.Select((fixture) => fixture.GetType().Name).ToArray();
            List<InputChoiceCategoryInfo> altChoices =
            [
                new("Additional options",
                [
                    new("Meta",
                    [
                        new($"{fixtureNames.Length + 1}", "Exit")
                    ])
                ])
            ];
            List<InputChoiceGroupInfo> groups = [];
            List<InputChoiceInfo> groupItems = [];
            FixtureCategory lastCategory = FixtureCategory.Unapplicable;
            for (int i = 0; i < fixtures.Length; i++)
            {
                var fixture = fixtures[i];
                string fixtureName = fixtureNames[i];
                if (lastCategory != fixture.Category && lastCategory != FixtureCategory.Unapplicable)
                {
                    groups.Add(new($"{lastCategory}", [.. groupItems]));
                    groupItems.Clear();
                }
                lastCategory = fixture.Category;
                groupItems.Add(new($"{i + 1}", fixtureName));
            }
            groups.Add(new($"{lastCategory}", [.. groupItems]));
            List<InputChoiceCategoryInfo> choices =
            [
                new("Test fixtures", [.. groups])
            ];

            // Prompt for fixtures
            while (true)
            {
                int selected = SelectionStyle.PromptSelection("Choose a fixture", [.. choices], [.. altChoices], true);
                if (selected == fixtureNames.Length + 1)
                    break;

                // Get the fixture from the selection and run it
                var chosenFixture = fixtures[selected - 1];
                string chosenFixtureName = fixtureNames[selected - 1];
                TextWriterColor.Write($"Fixture to be tested: {chosenFixtureName}\n");
                chosenFixture.RunFixture();
                Input.ReadKey();
            }
            ConsoleClearing.ResetAll();
            ConsoleWrapper.CursorVisible = true;
        }
    }
}
