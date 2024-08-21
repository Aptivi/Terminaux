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

using System;
using System.Threading;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class MouseInfo : IFixture
    {
        public void RunFixture()
        {
            bool looping = true;
            TextWriterColor.Write("Move your mouse around here or click anywhere. Press HOME to go back, M to enable/disable movement events, I to invert the scrolling Y axis, or L to invert the left/right mouse buttons.");
            Input.EnableMouse = true;
            while (looping)
            {
                var key = TermReader.ReadPointerOrKey();
                if (key.Item1 is null)
                {
                    if (key.Item2.Key == ConsoleKey.Home)
                        looping = false;
                    else if (key.Item2.Key == ConsoleKey.M)
                        Input.EnableMovementEvents = !Input.EnableMovementEvents;
                    else if (key.Item2.Key == ConsoleKey.I)
                        Input.InvertScrollYAxis = !Input.InvertScrollYAxis;
                    else if (key.Item2.Key == ConsoleKey.L)
                        Input.SwapLeftRightButtons = !Input.SwapLeftRightButtons;
                }
                else
                {
                    TextWriterColor.WriteColor($"{key.Item1.Coordinates.x}/{key.Item1.Coordinates.y} [{key.Item1.Button}, {key.Item1.ButtonPress}, {key.Item1.Modifiers}] [Dragging: {key.Item1.Dragging}, Tier: {key.Item1.ClickTier}]",
                        key.Item1.ButtonPress == PointerButtonPress.Clicked ? ConsoleColors.Green :
                        key.Item1.ButtonPress == PointerButtonPress.Released ? ConsoleColors.Purple :
                        key.Item1.ButtonPress == PointerButtonPress.Scrolled ? ConsoleColors.Red :
                        key.Item1.Dragging ? ConsoleColors.Yellow :
                        ConsoleColors.Teal);
                }
            }
            Input.EnableMouse = false;
        }
    }
}
