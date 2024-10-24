﻿//
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
using Terminaux.Console.Fixtures.Cases.Colors;
using Terminaux.Console.Fixtures.Cases.Consoles;
using Terminaux.Console.Fixtures.Cases.Graphics;
using Terminaux.Console.Fixtures.Cases.Images;
using Terminaux.Console.Fixtures.Cases.Inputs;
using Terminaux.Console.Fixtures.Cases.Inputs.CJK;
using Terminaux.Console.Fixtures.Cases.Presentations;
using Terminaux.Console.Fixtures.Cases.Reader;
using Terminaux.Console.Fixtures.Cases.Screens;
using Terminaux.Console.Fixtures.Cases.Tui;
using Terminaux.Console.Fixtures.Cases.Writer;

namespace Terminaux.Console.Fixtures
{
    internal static class FixtureManager
    {
        internal static IFixture[] fixtures =
        [
            // Reader
            new Prompt(),
            new PromptInterruptible(),
            new PromptWithDefault(),
            new PromptWithDefaultWritten(),
            new PromptWithPromptText(),
            new PromptWithDynamicPromptText(),
            new PromptWithOneLineWrap(),
            new PromptWithPlaceholderText(),
            new PromptPassword(),
            new PromptPasswordHidden(),
            new PromptPasswordWithPromptText(),
            new PromptPasswordWithPromptTextHidden(),
            new PromptPasswordCustomMask(),
            new PromptLooped(),
            new PromptLoopedHistories(),
            new PromptLoopedHistoriesNamed(),
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
            new PrintFigletAlignLeft(),
            new PrintFigletAlignLeftF(),
            new PrintFigletAlignLeftPositional(),
            new PrintFigletAlignLeftPositionalF(),
            new PrintFigletAlignMiddle(),
            new PrintFigletAlignMiddleF(),
            new PrintFigletAlignMiddlePositional(),
            new PrintFigletAlignMiddlePositionalF(),
            new PrintFigletAlignRight(),
            new PrintFigletAlignRightF(),
            new PrintFigletAlignRightPositional(),
            new PrintFigletAlignRightPositionalF(),
            new PrintAlignLeft(),
            new PrintAlignLeftOff(),
            new PrintAlignLeftCjk(),
            new PrintAlignLeftOffCjk(),
            new PrintAlignMiddle(),
            new PrintAlignMiddleOff(),
            new PrintAlignMiddleCjk(),
            new PrintAlignMiddleOffCjk(),
            new PrintAlignRight(),
            new PrintAlignRightOff(),
            new PrintAlignRightCjk(),
            new PrintAlignRightOffCjk(),
            new PrintSep(),
            new PrintSepF(),
            new PrintSepColor(),
            new PrintSepColorF(),
            new PrintSepCjk(),
            new PrintSepFCjk(),
            new PrintSepColorCjk(),
            new PrintSepColorFCjk(),
            new PrintWithNewLines(),
            new PrintWithNulls(),
            new PrintTemplate(),
            new PrintTemplateFromCtor(),
            new PrintLineHandle(),
            new PrintPowerLine(),
            new PrintRainbow(),
            new PrintRainbowBack(),
            new PrintRainbowCjk(),
            new PrintRainbowBackCjk(),
            new PrintWrapped(),
            new PrintWrappedChar(),
            new PrintRtl(),
            new TestDictWriterChar(),
            new TestDictWriterCharWithStringifiers(),
            new TestDictWriterInt(),
            new TestDictWriterStr(),
            new TestDictWriterLarge(),
            new TestDictWriterLargeWrap(),
            new TestDictWriterHuge(),
            new TestDictWriterHugeWrap(),
            new TestListWriterChar(),
            new TestListWriterCharWithStringifiers(),
            new TestListWriterInt(),
            new TestListWriterStr(),
            new TestListWriterLarge(),
            new TestListWriterLargeWrap(),
            new TestListWriterHuge(),
            new TestListWriterHugeWrap(),
            new TestNonGenericListWriterInt(),
            new TestTable(),
            new TestTableCjk(),
            new BorderText(),
            new TestNonModalInfoBox(),
            new TestNonModalInfoBoxExcess(),

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
            new TestInputIcon(),
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
            new TestInputInfoBoxInputPassword(),
            new TestInputInfoBoxProgress(),
            new TestInputInfoBoxSlider(),
            new TestInputInfoBoxSliderMin(),
            new ColorSelectorTest(),
            new ColorSelectorNoPaletteTest(),
            new KeyInfo(),
            new MouseInfo(),
            new TestHexEditorInteractive(),
            new TestHexViewerInteractive(),
            new TestTextEditorInteractive(),
            new TestTextViewerInteractive(),

            // Input (CJK)
            new TestInputSelectionCjk(),
            new TestInputSelectionLargeCjk(),
            new TestInputSelectionHugeCjk(),
            new TestInputSelectionScrollCjk(),
            new TestInputSelectionDisabledCjk(),
            new TestInputSelectionScrollDisabledCjk(),
            new TestInputSelectionMultipleCjk(),
            new TestInputSelectionLargeMultipleCjk(),
            new TestInputSelectionHugeMultipleCjk(),
            new TestInputSelectionMultipleScrollCjk(),
            new TestInputSelectionMultipleDisabledCjk(),
            new TestInputSelectionMultipleDisabledScrollCjk(),
            new TestInputChoiceCjk(),
            new TestInputChoiceNoAltCjk(),
            new TestInputChoiceNoMainCjk(),
            new TestInputChoiceDisabledCjk(),
            new TestInputInfoBoxButtonsCjk(),
            new TestInputInfoBoxMultiSelectionCjk(),
            new TestInputInfoBoxSelectionCjk(),
            new TestInputInfoBoxSelectionDisabledCjk(),
            new TestInputInfoBoxSelectionDisabledMultipleCjk(),
            new TestInputInfoBoxSelectionLargeCjk(),
            new TestInputInfoBoxSelectionLargeMultipleCjk(),
            new TestInputInfoBoxSelectionHugeCjk(),
            new TestInputInfoBoxSelectionHugeMultipleCjk(),
            new TestInputInfoBoxInputCjk(),
            new TestInputInfoBoxColoredInputCjk(),
            new TestInputInfoBoxInputPasswordCjk(),
            new TestInputInfoBoxProgressCjk(),
            new TestHexEditorInteractiveCjk(),
            new TestHexViewerInteractiveCjk(),
            new TestTextEditorInteractiveCjk(),
            new TestTextViewerInteractiveCjk(),

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
            new ColorGradientTest(),

            // Interactive TUI
            new CliInfoPaneTest(),
            new CliInfoPaneTestCjk(),
            new CliInfoPaneTestRtl(),
            new CliInfoPaneTestNf(),
            new CliInfoPaneTestExcess(),
            new CliInfoPaneTestRefreshing(),
            new CliInfoPaneHugeTest(),
            new CliInfoPaneSlowTest(),
            new CliInfoPaneSlowTestRefreshing(),
            new CliDoublePaneTest(),
            new CliDoublePaneSlowTest(),
            
            // Screen
            new TestScreen(),
            new TestScreenPrintPos(),
            new TestScreenPartVisibility(),
            new TestScreenBufferVisibility(),

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
            new TermInfoPane(),
            new TermInfoTest(),
            new ConsoleConhostCheck(),
            new AlternateBuffers(),
            new Sizes(),
            new BeepTones(),
            new ProgressTest(),
            new ProgressFailTest(),
            new ProgressWarnFailTest(),
            new ProgressIndeterminateTest(),
            new ProgressPauseTest(),
            new ProgressFourTest(),
            new ProgressDecrementTest(),
            new TestCursorCoord(),

            // Image
            new RenderImage(),
            new RenderImageSvg(),
            new RenderImageBuiltin(),

            // Graphics
            new RenderLines(),
            new RenderLinesSmooth(),
            new RenderRectangle(),
            new RenderSquare(),
            new RenderTriangle(),
            new RenderTrapezoid(),
            new RenderParallelogram(),
            new RenderEllipsis(),
            new RenderCircle(),
        ];

        internal static IFixture GetFixtureFromName(string name)
        {
            if (DoesFixtureExist(name))
            {
                var detectedFixtures = fixtures.Where((fixture) => fixture.GetType().Name == name).ToArray();
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
            var detectedFixtures = fixtures.Where((fixture) => fixture.GetType().Name == name);
            return detectedFixtures.Any();
        }

        internal static string[] GetFixtureNames() =>
            fixtures.Select((fixture) => fixture.GetType().Name).ToArray();
    }
}
