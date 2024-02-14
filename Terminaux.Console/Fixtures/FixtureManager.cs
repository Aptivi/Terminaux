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

using System.Linq;
using Terminaux.Base;
using Terminaux.ConsoleDemo.Fixtures.Cases.Colors;
using Terminaux.ConsoleDemo.Fixtures.Cases.Consoles;
using Terminaux.ConsoleDemo.Fixtures.Cases.Input;
using Terminaux.ConsoleDemo.Fixtures.Cases.Presentations;
using Terminaux.ConsoleDemo.Fixtures.Cases.Reader;
using Terminaux.ConsoleDemo.Fixtures.Cases.Screens;
using Terminaux.ConsoleDemo.Fixtures.Cases.Tui;
using Terminaux.ConsoleDemo.Fixtures.Cases.Writer;

namespace Terminaux.ConsoleDemo.Fixtures
{
    internal static class FixtureManager
    {
        internal static IFixture[] fixtures =
        [
            // Reader
            new Prompt(),
            new PromptInterruptible(),
            new PromptWithDefault(),
            new PromptWithPromptText(),
            new PromptWithOneLineWrap(),
            new PromptWithPlaceholderText(),
            new PromptPassword(),
            new PromptPasswordHidden(),
            new PromptPasswordWithPromptText(),
            new PromptPasswordWithPromptTextHidden(),
            new PromptPasswordCustomMask(),
            new PromptLooped(),
            new PromptLoopedHistories(),
            new PromptLoopedMargin(),
            new PromptLoopedManualWrite(),
            new PromptLoopedCompletion(),
            new PromptLoopedCompletionOneLineWrap(),
            new PromptLoopedCompletionMultiLinePrompt(),
            new PromptLoopedCompletionMultiLinePromptWithOneLine(),
            new PromptLoopedCtrlCAsInput(),
            new PromptHighlighted(),

            // Writer
            new Print(),
            new PrintF(),
            new PrintFiglet(),
            new PrintFigletF(),
            new PrintFigletDefault(),
            new PrintFigletCentered(),
            new PrintFigletCenteredF(),
            new PrintFigletCenteredPositional(),
            new PrintFigletCenteredPositionalF(),
            new PrintCentered(),
            new PrintSep(),
            new PrintSepF(),
            new PrintSepColor(),
            new PrintSepColorF(),
            new PrintWithNewLines(),
            new PrintWithNulls(),
            new PrintTemplate(),
            new PrintLineHandle(),
            new PrintPowerLine(),
            new TestDictWriterChar(),
            new TestDictWriterInt(),
            new TestDictWriterStr(),
            new TestDictWriterLarge(),
            new TestDictWriterLargeWrap(),
            new TestDictWriterHuge(),
            new TestDictWriterHugeWrap(),
            new TestListWriterChar(),
            new TestListWriterInt(),
            new TestListWriterStr(),
            new TestListWriterLarge(),
            new TestListWriterLargeWrap(),
            new TestListWriterHuge(),
            new TestListWriterHugeWrap(),
            new TestNonGenericListWriterInt(),
            new TestTable(),
            new BorderText(),

            // Input
            new TestInputSelection(),
            new TestInputSelectionLarge(),
            new TestInputSelectionHuge(),
            new TestInputSelectionScroll(),
            new TestInputSelectionDisabled(),
            new TestInputSelectionScrollDisabled(),
            new TestInputSelectionMultiple(),
            new TestInputSelectionLargeMultiple(),
            new TestInputSelectionHugeMultiple(),
            new TestInputSelectionMultipleScroll(),
            new TestInputSelectionMultipleDisabled(),
            new TestInputSelectionMultipleDisabledScroll(),
            new TestInputChoice(),
            new TestInputChoiceNoAlt(),
            new TestInputChoiceNoMain(),
            new TestInputChoiceDisabled(),
            new TestInputFiglet(),
            new TestInputInfoBoxButtons(),
            new TestInputInfoBoxMultiSelection(),
            new TestInputInfoBoxSelection(),
            new TestInputInfoBoxSelectionDisabled(),
            new TestInputInfoBoxSelectionDisabledMultiple(),
            new TestInputInfoBoxSelectionLarge(),
            new TestInputInfoBoxSelectionLargeMultiple(),
            new TestInputInfoBoxSelectionHuge(),
            new TestInputInfoBoxSelectionHugeMultiple(),
            new TestInputInfoBoxInput(),
            new TestInputInfoBoxColoredInput(),
            new TestInputInfoBoxProgress(),
            new ColorSelectorTest(),
            new ColorSelectorNoPaletteTest(),
            new KeyInfo(),

            // Color
            new ColorTest(),
            new ColorTrueTest(),
            new ColorWithResetTest(),
            new ColorRandomTest(),
            new ColorRandom256Test(),
            new ColorRandom16Test(),
            new ColorBackTest(),
            new ColorBackTestDry(),
            new ColorTransparencyTest(),

            // Interactive TUI
            new CliInfoPaneTest(),
            new CliInfoPaneTestRefreshing(),
            new CliInfoPaneHugeTest(),
            new CliInfoPaneSlowTest(),
            new CliInfoPaneSlowTestRefreshing(),
            new CliDoublePaneTest(),
            new CliDoublePaneSlowTest(),
            
            // Screen
            new TestScreen(),

            // Presentation
            new TestPresentation(),
            new TestPresentationKiosk(),
            new TestPresentationRequired(),
            new TestPresentationKioskRequired(),

            // Console
            new ConsoleSizeCheck(),
            new ConsoleCheck(),
            new TestCursorStyles(),
            new TestFormatting(),
            new ClearKeepPosition(),
        ];

        internal static IFixture GetFixtureFromName(string name)
        {
            if (DoesFixtureExist(name))
            {
                var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name).ToArray();
                return detectedFixtures[0];
            }
            else
                throw new TerminauxException(
                    "Fixture doesn't exist. Available fixtures:\n" +
                    "  - " + string.Join("\n  - ", GetFixtureNames())
                );
        }

        internal static bool DoesFixtureExist(string name)
        {
            var detectedFixtures = fixtures.Where((fixture) => fixture.FixtureID == name);
            return detectedFixtures.Any();
        }

        internal static string[] GetFixtureNames() =>
            fixtures.Select((fixture) => fixture.FixtureID).ToArray();
    }
}
