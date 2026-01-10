//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using System;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs;
using Terminaux.Sequences.Builder.Types;

namespace Terminaux.Console.Fixtures.Cases.Screens
{
    internal class TestScreenWasRefreshed : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Screen;

        public void RunFixture()
        {
            // Show the screen measurement dimensions
            var dimensionsScreen = new Screen()
            {
                CycleFrequency = 500,
            };
            try
            {
                // First, clear the screen
                ConsoleColoring.LoadBack();

                // Then, show text with dimensions
                var dimensionsScreenPart = new ScreenPart();
                dimensionsScreenPart.AddDynamicText(() =>
                {
                    return
                        CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight) +
                        $"Was refreshed: {dimensionsScreen.RefreshWasDone} ";
                });
                dimensionsScreen.AddBufferedPart("Test", dimensionsScreenPart);
                ScreenTools.SetCurrent(dimensionsScreen);
                ScreenTools.SetCurrentCyclic(dimensionsScreen);
                ScreenTools.StartCyclicScreen();
                Input.ReadKey();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"Screen failed to render: {ex.Message}");
            }
            finally
            {
                ScreenTools.StopCyclicScreen();
                ScreenTools.UnsetCurrent(dimensionsScreen);
                ConsoleColoring.LoadBack();
            }
        }
    }
}
