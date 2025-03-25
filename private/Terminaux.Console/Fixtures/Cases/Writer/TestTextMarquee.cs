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
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestTextMarquee : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Show a screen with a counter in the middle
            var stickScreen = new Screen()
            {
                CycleFrequency = 50,
            };
            var marquee = new TextMarquee(
                "This is the test text marquee that's adjusted to your console width with the margin of 4 from both the " +
                "left and the right side, and is intentionally long to make the text scroll just like how music players " +
                "work.")
            {
                Width = ConsoleWrapper.WindowWidth - 8,
            };
            try
            {
                // First, clear the screen
                ColorTools.LoadBack();

                // Then, show the counter
                var stickScreenPart = new ScreenPart();
                stickScreenPart.AddDynamicText(() => RendererTools.RenderRenderable(marquee, new(4, ConsoleWrapper.WindowHeight / 2)));
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.SetCurrentCyclic(stickScreen);
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
                ScreenTools.UnsetCurrent(stickScreen);
                ColorTools.LoadBack();
            }
        }
    }
}
