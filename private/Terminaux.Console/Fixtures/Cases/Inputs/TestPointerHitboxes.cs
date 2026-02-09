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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class TestPointerHitboxes : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            Input.EnableMouse = true;
            var stickScreen = new Screen();
            try
            {
                // First, clear the screen
                ConsoleColoring.LoadBack();

                // Then, show the resizable red, green, and blue boxes
                var redBoxPart = new ScreenPart();
                var greenBoxPart = new ScreenPart();
                var blueBoxPart = new ScreenPart();
                bool redState = true;
                bool greenState = true;
                bool blueState = true;
                bool bail = false;
                redBoxPart.AddDynamicText(() =>
                {
                    var redBox = GenBox(1, ConsoleColors.Red);
                    return redBox.Render();
                });
                stickScreen.AddBufferedPart("Red Box", redBoxPart);
                greenBoxPart.AddDynamicText(() =>
                {
                    var greenBox = GenBox(2, ConsoleColors.Lime);
                    return greenBox.Render();
                });
                stickScreen.AddBufferedPart("Green Box", greenBoxPart);
                blueBoxPart.AddDynamicText(() =>
                {
                    var blueBox = GenBox(3, ConsoleColors.Blue);
                    return blueBox.Render();
                });
                stickScreen.AddBufferedPart("Blue Box", blueBoxPart);

                // Press info screen part
                var pressInfo = new ScreenPart();
                stickScreen.AddBufferedPart("Press info", pressInfo);

                // Now, render the boxes in an infinite loop until the user exits
                ScreenTools.SetCurrent(stickScreen);
                while (!bail)
                {
                    // Get the box visibility
                    redBoxPart.Visible = redState;
                    greenBoxPart.Visible = greenState;
                    blueBoxPart.Visible = blueState;

                    // Render them
                    ScreenTools.Render();
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var mouse = data.PointerEventContext;
                    if (data.ConsoleKeyInfo is ConsoleKeyInfo cki)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Q:
                                bail = true;
                                break;
                        }
                    }
                    else if (mouse is not null)
                    {
                        var redBox = GenBox(1, ConsoleColors.Red);
                        var greenBox = GenBox(2, ConsoleColors.Lime);
                        var blueBox = GenBox(3, ConsoleColors.Blue);
                        var redHitbox = redBox.GenerateHitbox((pec) => RenderHitboxPos(pec, ConsoleColors.Red, 3));
                        var greenHitbox = greenBox.GenerateHitbox((pec) => RenderHitboxPos(pec, ConsoleColors.Lime, 2));
                        var blueHitbox = blueBox.GenerateHitbox((pec) => RenderHitboxPos(pec, ConsoleColors.Blue));
                        redHitbox.Button = greenHitbox.Button = blueHitbox.Button = PointerButton.Left;
                        redHitbox.ButtonPress = greenHitbox.ButtonPress = blueHitbox.ButtonPress = PointerButtonPress.Released;
                        pressInfo.Clear();
                        pressInfo.AddDynamicText(() =>
                        {
                            string redHitboxInfo = (string?)redHitbox.ProcessPointer(mouse, out bool redInRange) ?? "";
                            string greenHitboxInfo = (string?)greenHitbox.ProcessPointer(mouse, out bool greenInRange) ?? "";
                            string blueHitboxInfo = (string?)blueHitbox.ProcessPointer(mouse, out bool blueInRange) ?? "";
                            string rangeSpecifierRed = redInRange ? "[Y]" : "[N]";
                            string rangeSpecifierGreen = greenInRange ? "[Y]" : "[N]";
                            string rangeSpecifierBlue = blueInRange ? "[Y]" : "[N]";

                            // Make a string builder that contains press info
                            var pressInfo = new StringBuilder();
                            pressInfo.Append(
                                TextWriterWhereColor.RenderWhereColor(rangeSpecifierRed, 0, ConsoleWrapper.WindowHeight - 3, ConsoleColors.Red) +
                                redHitboxInfo +
                                TextWriterWhereColor.RenderWhereColor(rangeSpecifierGreen, 0, ConsoleWrapper.WindowHeight - 2, ConsoleColors.Lime) +
                                greenHitboxInfo +
                                TextWriterWhereColor.RenderWhereColor(rangeSpecifierBlue, 0, ConsoleWrapper.WindowHeight - 1, ConsoleColors.Blue) +
                                blueHitboxInfo
                            );
                            return pressInfo.ToString();
                        });
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
                ConsoleColoring.LoadBack();
            }
            Input.EnableMouse = false;
        }

        private Box GenBox(int factor, Color color)
        {
            factor -= 1;
            int consoleHalf = ConsoleWrapper.WindowWidth / 2;
            int startX = consoleHalf - (consoleHalf * 3 / 4) + (factor * 4);
            int endX = consoleHalf + (consoleHalf / 4) + (factor * 4);
            int y = factor * 2;
            int height = ConsoleWrapper.WindowHeight - y - (7 - (factor * 2));
            int width = endX - startX;
            var box = new Box()
            {
                Left = startX,
                Top = y,
                Width = width,
                Height = height,
                Color = color
            };
            return box;
        }

        private string? RenderHitboxPos(PointerEventContext context, Color color, int offset = 1)
        {
            int finalY = ConsoleWrapper.WindowHeight - offset;
            return TextWriterWhereColor.RenderWhereColor($"Box pos in range: {context.Coordinates}" + ConsoleClearing.GetClearLineToRightSequence(), 4, finalY, color);
        }
    }
}
