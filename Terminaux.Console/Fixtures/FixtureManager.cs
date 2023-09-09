
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

using System;
using System.Linq;
using Terminaux.ConsoleDemo.Fixtures.Cases;

namespace Terminaux.ConsoleDemo.Fixtures
{
    internal static class FixtureManager
    {
        internal static IFixture[] fixtures =
        {
            // Reader
            new Prompt(),
            new PromptInterruptible(),
            new PromptWithDefault(),
            new PromptWithPlaceholder(),
            new PromptWithOneLineWrap(),
            new PromptPassword(),
            new PromptPasswordHidden(),
            new PromptPasswordWithPlaceholder(),
            new PromptPasswordWithPlaceholderHidden(),
            new PromptLooped(),
            new PromptLoopedMargin(),
            new PromptLoopedManualWrite(),
            new PromptLoopedCompletion(),
            new PromptLoopedCompletionOneLineWrap(),
            new PromptLoopedCompletionMultiLinePrompt(),
            new PromptLoopedCompletionMultiLinePromptWithOneLine(),
            new PromptLoopedCtrlCAsInput(),

            // Writer
            new Print(),
            new PrintF(),
            new PrintFiglet(),
            new PrintFigletF(),
            new PrintSep(),
            new PrintSepF(),
            new PrintSepColor(),
            new PrintSepColorF(),
            new PrintWithNewLines(),
            new PrintWithNulls(),
            new TestDictWriterChar(),
            new TestDictWriterInt(),
            new TestDictWriterStr(),
            new TestListWriterChar(),
            new TestListWriterInt(),
            new TestListWriterStr(),
            new TestTable(),

            // Input
            new TestInputSelection(),
            new TestInputSelectionLarge(),
            new TestInputSelectionMultiple(),
            new TestInputSelectionLargeMultiple(),
            new TestInputChoice(),
            new TestInputFiglet(),
            new KeyInfo(),

            // Color
            new ColorTest(),
            new ColorTrueTest(),
            new ColorWheelTest(),
            new ColorWithResetTest(),

            // Interactive TUI
            new CliInfoPaneTest(),
            new CliInfoPaneTestRefreshing(),
            new CliInfoPaneSlowTest(),
            new CliInfoPaneSlowTestRefreshing(),
            new CliDoublePaneTest(),
            new CliDoublePaneSlowTest(),
        };

        internal static IFixture GetFixtureFromName(string name)
        {
            if (DoesFixtureExist(name))
            {
                var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name).ToArray();
                return detectedFixtures[0];
            }
            else
                throw new Exception("Fixture doesn't exist.");
        }

        internal static bool DoesFixtureExist(string name)
        {
            var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name);
            return detectedFixtures.Any();
        }
    }
}
