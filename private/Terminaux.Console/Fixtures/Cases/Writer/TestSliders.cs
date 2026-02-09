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
using Terminaux.Inputs.Styles.Infobox;
using System.Threading;
using Terminaux.Writer.CyclicWriters.Renderer;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Base.Extensions;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestSliders : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Show a screen with a slider bar at the end
            var stickScreen = new Screen()
            {
                CycleFrequency = 50,
            };

            // Make a collection of renderables
            var container = new Container();
            var slider1 = new Slider(0, 0, 100)
            {
                Width = 40,
                SliderActiveForegroundColor = ConsoleColors.Red,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Red),
            };
            var slider2 = new Slider(0, 0, 10)
            {
                Width = 40,
                SliderActiveForegroundColor = ConsoleColors.Lime,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Lime),
            };
            var slider3 = new Slider(0, 0, 4)
            {
                Width = 40,
                SliderActiveForegroundColor = ConsoleColors.Blue,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Blue),
            };
            var slider4 = new Slider(0, 0, 100)
            {
                Height = 10,
                Vertical = true,
                SliderActiveForegroundColor = ConsoleColors.Fuchsia,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Fuchsia),
            };
            var slider5 = new Slider(0, 0, 10)
            {
                Height = 10,
                Vertical = true,
                SliderActiveForegroundColor = ConsoleColors.Yellow,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Yellow),
            };
            var slider6 = new Slider(0, 0, 4)
            {
                Height = 10,
                Vertical = true,
                SliderActiveForegroundColor = ConsoleColors.Aqua,
                SliderForegroundColor = TransformationTools.GetDarkBackground(ConsoleColors.Aqua),
            };
            container.AddRenderable("Slider bar 1", slider1);
            container.SetRenderablePosition("Slider bar 1", new(4, ConsoleWrapper.WindowHeight - 3));
            container.AddRenderable("Slider bar 2", slider2);
            container.SetRenderablePosition("Slider bar 2", new(4, ConsoleWrapper.WindowHeight - 2));
            container.AddRenderable("Slider bar 3", slider3);
            container.SetRenderablePosition("Slider bar 3", new(4, ConsoleWrapper.WindowHeight - 1));
            container.AddRenderable("Slider bar 4", slider4);
            container.SetRenderablePosition("Slider bar 4", new(4, 2));
            container.AddRenderable("Slider bar 5", slider5);
            container.SetRenderablePosition("Slider bar 5", new(6, 2));
            container.AddRenderable("Slider bar 6", slider6);
            container.SetRenderablePosition("Slider bar 6", new(8, 2));

            // Render them all
            try
            {
                // First, clear the screen
                ConsoleColoring.LoadBack();

                // Then, show the slider bar
                var stickScreenPart = new ScreenPart();
                stickScreenPart.AddDynamicText(() => ContainerTools.RenderContainer(container));
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.SetCurrentCyclic(stickScreen);
                ScreenTools.StartCyclicScreen();

                // Finally, increment the slider bar until it's full
                for (int sliderPos1 = 0, sliderPos2 = 0, sliderPos3 = 0; sliderPos1 < 100; sliderPos1++, sliderPos2++, sliderPos3++)
                {
                    if (sliderPos2 == 10)
                        sliderPos2 = 0;
                    if (sliderPos3 == 4)
                        sliderPos3 = 0;
                    slider1.Position = sliderPos1;
                    slider2.Position = sliderPos2;
                    slider3.Position = sliderPos3;
                    slider4.Position = sliderPos1;
                    slider5.Position = sliderPos2;
                    slider6.Position = sliderPos3;
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
                ConsoleColoring.LoadBack();
            }
        }
    }
}
