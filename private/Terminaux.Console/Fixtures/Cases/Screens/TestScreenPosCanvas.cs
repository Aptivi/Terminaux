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
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using System.Collections.Generic;
using Terminaux.Base.Structures;
using System.Linq;
using Colorimetry.Data;

namespace Terminaux.Console.Fixtures.Cases.Screens
{
    internal class TestScreenPosCanvas : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Screen;

        public void RunFixture()
        {
            // Show the screen position canvas with ability to add a point
            var canvasScreen = new Screen();
            List<CellOptions> pixels = [];
            Coordinate position = new(ConsoleWrapper.WindowWidth / 4 - 1, ConsoleWrapper.WindowHeight / 2 - 1);
            int posX = position.X;
            int posY = position.Y;
            try
            {
                // First, clear the screen
                ConsoleColoring.LoadBack();

                // Second, show the pixels
                var pixelsScreenPart = new ScreenPart();
                pixelsScreenPart.AddDynamicText(() =>
                {
                    var pointCanvas = new Canvas()
                    {
                        Transparent = true,
                        Left = 0,
                        Top = 0,
                        DoubleWidth = true,
                        Width = ConsoleWrapper.WindowWidth / 2,
                        Height = ConsoleWrapper.WindowHeight - 1,
                        Pixels = [.. pixels]
                    };
                    return pointCanvas.Render();
                });
                canvasScreen.AddBufferedPart("Pixels", pixelsScreenPart);

                // Third, show the cursor
                var cursorScreenPart = new ScreenPart();
                pixelsScreenPart.AddDynamicText(() =>
                {
                    var cursorCanvas = new Canvas()
                    {
                        Transparent = true,
                        Left = 0,
                        Top = 0,
                        DoubleWidth = true,
                        Width = ConsoleWrapper.WindowWidth / 2,
                        Height = ConsoleWrapper.WindowHeight - 1,
                        Pixels = [new CellOptions(position.X + 1, position.Y + 1)
                        {
                            CellColor = ConsoleColors.Lime
                        }]
                    };
                    return cursorCanvas.Render();
                });
                canvasScreen.AddBufferedPart("Cursor", cursorScreenPart);

                // Then, show text with dimensions
                var dimensionsScreenPart = new ScreenPart();
                dimensionsScreenPart.AddDynamicText(() =>
                {
                    return
                        CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight) +
                        $"pos: {position}";
                });
                canvasScreen.AddBufferedPart("Dimensions", dimensionsScreenPart);
                ScreenTools.SetCurrent(canvasScreen);

                // Main loop to handle user input
                bool bail = false;
                while (!bail)
                {
                    ScreenTools.Render();
                    var key = Input.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            posY--;
                            if (posY < 0)
                                posY = 0;
                            canvasScreen.RequireRefresh();
                            break;
                        case ConsoleKey.DownArrow:
                            posY++;
                            if (posY > ConsoleWrapper.WindowHeight - 2)
                                posY = ConsoleWrapper.WindowHeight - 2;
                            canvasScreen.RequireRefresh();
                            break;
                        case ConsoleKey.LeftArrow:
                            posX--;
                            if (posX < 0)
                                posX = 0;
                            canvasScreen.RequireRefresh();
                            break;
                        case ConsoleKey.RightArrow:
                            posX++;
                            if (posX > ConsoleWrapper.WindowWidth / 2 - 1)
                                posX = ConsoleWrapper.WindowWidth / 2 - 1;
                            canvasScreen.RequireRefresh();
                            break;
                        case ConsoleKey.Spacebar:
                            var pixel = pixels.SingleOrDefault((co) => co.ColumnIndex == posX && co.RowIndex == posY);
                            if (pixel is not null)
                                pixels.Remove(pixel);
                            else
                                pixels.Add(new(posX + 1, posY + 1)
                                {
                                    CellColor = ConsoleColors.White,
                                });
                            break;
                        case ConsoleKey.Q:
                            bail = true;
                            break;
                    }
                    position = new(posX, posY);
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal($"Screen failed to render: {ex.Message}");
            }
            finally
            {
                ScreenTools.UnsetCurrent(canvasScreen);
                ConsoleColoring.LoadBack();
            }
        }
    }
}
