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
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Inputs
{
    internal class MouseInfoX10 : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Input;

        public void RunFixture()
        {
            bool looping = true;
            TextWriterColor.Write("[X10 protocol] Move your mouse around here or click anywhere. Press HOME to go back, M to enable/disable movement events, I to invert the scrolling Y axis, or L to invert the left/right mouse buttons.");
            Input.PointerEncoding = PointerEncoding.X10;
            Input.EnableMouse = true;
            while (looping)
            {
                InputEventInfo data = Input.ReadPointerOrKey();
                var mouse = data.PointerEventContext;
                if (data.ConsoleKeyInfo is ConsoleKeyInfo cki)
                {
                    if (cki.Key == ConsoleKey.Home)
                        looping = false;
                    else if (cki.Key == ConsoleKey.M)
                        Input.EnableMovementEvents = !Input.EnableMovementEvents;
                    else if (cki.Key == ConsoleKey.I)
                        Input.InvertScrollYAxis = !Input.InvertScrollYAxis;
                    else if (cki.Key == ConsoleKey.L)
                        Input.SwapLeftRightButtons = !Input.SwapLeftRightButtons;
                }
                else if (mouse is not null)
                {
                    TextWriterColor.WriteColor($"{mouse.Coordinates.x}/{mouse.Coordinates.y} [{mouse.Button}, {mouse.ButtonPress}, {mouse.Modifiers}] [Dragging: {mouse.Dragging}, Tier: {mouse.ClickTier}]",
                        mouse.ButtonPress == PointerButtonPress.Clicked ? ConsoleColors.Green :
                        mouse.ButtonPress == PointerButtonPress.Released ? ConsoleColors.Purple :
                        mouse.ButtonPress == PointerButtonPress.Scrolled ? ConsoleColors.Red :
                        mouse.Dragging ? ConsoleColors.Yellow :
                        ConsoleColors.Teal);
                }
            }
            Input.EnableMouse = false;
        }
    }
}
