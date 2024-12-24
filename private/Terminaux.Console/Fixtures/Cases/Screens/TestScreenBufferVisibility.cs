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
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters;

namespace Terminaux.Console.Fixtures.Cases.Screens
{
    internal class TestScreenBufferVisibility : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Screen;

        public void RunFixture()
        {
            var stickScreen = new Screen();
            try
            {
                // First, clear the screen
                ColorTools.LoadBack();

                // Then, show the resizable red, green, and blue boxes
                var boxes = new ScreenPart();
                bool redState = true;
                bool greenState = true;
                bool blueState = true;
                bool bail = false;
                boxes.AddDynamicText(() =>
                {
                    int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                    int startX = consoleHalf - (consoleHalf * 3 / 4);
                    int endX = consoleHalf + (consoleHalf / 4);
                    int y = 2;
                    int height = ConsoleWrapper.WindowHeight - y - 7;
                    int width = endX - startX;
                    var box = new Box()
                    {
                        Left = startX,
                        Top = y,
                        InteriorWidth = width,
                        InteriorHeight = height,
                        Color = ConsoleColors.Red,
                    };
                    return box.Render();
                });
                boxes.AddDynamicText(() =>
                {
                    int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                    int startX = consoleHalf - (consoleHalf * 3 / 4) + 4;
                    int endX = consoleHalf + (consoleHalf / 4) + 4;
                    int y = 4;
                    int height = ConsoleWrapper.WindowHeight - y - 5;
                    int width = endX - startX;
                    var box = new Box()
                    {
                        Left = startX,
                        Top = y,
                        InteriorWidth = width,
                        InteriorHeight = height,
                        Color = ConsoleColors.Lime,
                    };
                    return box.Render();
                });
                boxes.AddDynamicText(() =>
                {
                    int consoleHalf = ConsoleWrapper.WindowWidth / 2;
                    int startX = consoleHalf - (consoleHalf * 3 / 4) + 8;
                    int endX = consoleHalf + (consoleHalf / 4) + 8;
                    int y = 6;
                    int height = ConsoleWrapper.WindowHeight - y - 3;
                    int width = endX - startX;
                    var box = new Box()
                    {
                        Left = startX,
                        Top = y,
                        InteriorWidth = width,
                        InteriorHeight = height,
                        Color = ConsoleColors.Blue,
                    };
                    return box.Render();
                });
                stickScreen.AddBufferedPart("Boxes", boxes);

                // Now, render the boxes in an infinite loop until the user exits
                ScreenTools.SetCurrent(stickScreen);
                while (!bail)
                {
                    // Get the box visibility
                    boxes.SetBufferVisibility(0, redState);
                    boxes.SetBufferVisibility(1, greenState);
                    boxes.SetBufferVisibility(2, blueState);

                    // Render them
                    ScreenTools.Render();
                    var key = Input.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.R:
                            redState = !redState;
                            stickScreen.RequireRefresh();
                            break;
                        case ConsoleKey.G:
                            greenState = !greenState;
                            stickScreen.RequireRefresh();
                            break;
                        case ConsoleKey.B:
                            blueState = !blueState;
                            stickScreen.RequireRefresh();
                            break;
                        case ConsoleKey.Q:
                            bail = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"Screen failed to render: {ex.Message}");
            }
            finally
            {
                ScreenTools.UnsetCurrent(stickScreen);
                ColorTools.LoadBack();
            }
        }
    }
}
