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

using System;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using System.Threading;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Inputs.Styles;
using System.Linq;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestProgressBarSpinnerSelectCjkAlt : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Prompt user to select a spinner
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            InputChoiceInfo[] spinnerNames = builtinSpinners.Select((pi, idx) => new InputChoiceInfo($"{idx + 1}", pi.Name)).ToArray();
            int spinnerIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(spinnerNames, "Select a spinner");
            if (spinnerIdx == -1)
                return;
            var selectedSpinnerPropertyInfo = builtinSpinners[spinnerIdx];
            var selectedSpinner = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
            if (selectedSpinner is not Spinner spinner)
                return;

            // Show a screen with a progress bar at the end
            var stickScreen = new Screen()
            {
                CycleFrequency = 50,
            };
            var progressBar = new ProgressBar(
                "這是測試進度欄，其中包含滾動選取框和您選擇的旋轉器。您可以透過查看最左邊的區域來觀察您選擇的旋轉器。", 0, 500, spinner)
            {
                Width = ConsoleWrapper.WindowWidth - 8,
            };
            try
            {
                // First, clear the screen
                ColorTools.LoadBack();

                // Then, show the progress bar
                var stickScreenPart = new ScreenPart();
                stickScreenPart.AddDynamicText(() => RendererTools.RenderRenderable(progressBar, new(4, ConsoleWrapper.WindowHeight - 1)));
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.SetCurrentCyclic(stickScreen);
                ScreenTools.StartCyclicScreen();

                // Finally, increment the progress bar until it's full
                for (int progress = 0; progress < 500; progress++)
                {
                    progressBar.Position = progress;
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"Screen failed to render: {ex.Message}");
            }
            finally
            {
                ScreenTools.StopCyclicScreen();
                ScreenTools.UnsetCurrent(stickScreen);
                ColorTools.LoadBack();
            }
        }
    }
}
