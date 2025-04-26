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

using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
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
                ColorTools.LoadBack();

                // Then, show the resizable red, green, and blue boxes
                var redBox = new ScreenPart();
                var greenBox = new ScreenPart();
                var blueBox = new ScreenPart();
                bool redState = true;
                bool greenState = true;
                bool blueState = true;
                bool bail = false;
                redBox.AddDynamicText(() =>
                {
                    var redPos = GetBoxPos(1);
                    var box = new Box()
                    {
                        Left = redPos.start.X,
                        Top = redPos.start.Y,
                        Width = redPos.size.Width,
                        Height = redPos.size.Height,
                        Color = ConsoleColors.Red,
                    };
                    return box.Render();
                });
                stickScreen.AddBufferedPart("Red Box", redBox);
                greenBox.AddDynamicText(() =>
                {
                    var greenPos = GetBoxPos(2);
                    var box = new Box()
                    {
                        Left = greenPos.start.X,
                        Top = greenPos.start.Y,
                        Width = greenPos.size.Width,
                        Height = greenPos.size.Height,
                        Color = ConsoleColors.Lime,
                    };
                    return box.Render();
                });
                stickScreen.AddBufferedPart("Green Box", greenBox);
                blueBox.AddDynamicText(() =>
                {
                    var bluePos = GetBoxPos(3);
                    var box = new Box()
                    {
                        Left = bluePos.start.X,
                        Top = bluePos.start.Y,
                        Width = bluePos.size.Width,
                        Height = bluePos.size.Height,
                        Color = ConsoleColors.Blue,
                    };
                    return box.Render();
                });
                stickScreen.AddBufferedPart("Blue Box", blueBox);

                // Press info screen part
                var pressInfo = new ScreenPart();
                stickScreen.AddBufferedPart("Press info", pressInfo);

                // Now, render the boxes in an infinite loop until the user exits
                ScreenTools.SetCurrent(stickScreen);
                while (!bail)
                {
                    // Get the box visibility
                    redBox.Visible = redState;
                    greenBox.Visible = greenState;
                    blueBox.Visible = blueState;

                    // Render them
                    ScreenTools.Render();
                    var key = Input.ReadPointerOrKey();
                    if (key.Item1 is null)
                    {
                        switch (key.Item2.Key)
                        {
                            case ConsoleKey.Q:
                                bail = true;
                                break;
                        }
                    }
                    else
                    {
                        var redPos = GetBoxPos(1);
                        var greenPos = GetBoxPos(2);
                        var bluePos = GetBoxPos(3);
                        var redHitbox = new PointerHitbox(redPos.start, redPos.size, (pec) => RenderHitboxPos(pec, ConsoleColors.Red, 3));
                        var greenHitbox = new PointerHitbox(greenPos.start, greenPos.size, (pec) => RenderHitboxPos(pec, ConsoleColors.Lime, 2));
                        var blueHitbox = new PointerHitbox(bluePos.start, bluePos.size, (pec) => RenderHitboxPos(pec, ConsoleColors.Blue));
                        pressInfo.Clear();
                        pressInfo.AddDynamicText(() =>
                        {
                            string redHitboxInfo = (string?)redHitbox.ProcessPointer(key.Item1, out bool redInRange) ?? "";
                            string greenHitboxInfo = (string?)greenHitbox.ProcessPointer(key.Item1, out bool greenInRange) ?? "";
                            string blueHitboxInfo = (string?)blueHitbox.ProcessPointer(key.Item1, out bool blueInRange) ?? "";
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
                ColorTools.LoadBack();
            }
            Input.EnableMouse = false;
        }

        private (Coordinate start, Size size) GetBoxPos(int factor)
        {
            factor -= 1;
            int consoleHalf = ConsoleWrapper.WindowWidth / 2;
            int startX = consoleHalf - (consoleHalf * 3 / 4) + (factor * 4);
            int endX = consoleHalf + (consoleHalf / 4) + (factor * 4);
            int y = factor * 2;
            int height = ConsoleWrapper.WindowHeight - y - (7 - (factor * 2));
            int width = endX - startX;
            return (new(startX, y), new(width, height));
        }

        private string? RenderHitboxPos(PointerEventContext context, Color color, int offset = 1)
        {
            int finalY = ConsoleWrapper.WindowHeight - offset;
            return TextWriterWhereColor.RenderWhereColor($"Box pos in range: {context.Coordinates}" + ConsoleClearing.GetClearLineToRightSequence(), 4, finalY, color);
        }
    }
}
