﻿//
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
using Terminaux.Inputs.Pointer;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Input
{
    internal class MouseInfo : IFixture
    {
        public void RunFixture()
        {
            bool looping = true;
            TextWriterColor.Write("Move your mouse around here or click anywhere. Press HOME to go back, M to enable/disable movement events, I to invert the scrolling Y axis, or L to invert the left/right mouse buttons.");
            PointerListener.MouseEvent += MouseEvent;
            PointerListener.StartListening();
            while (looping)
            {
                SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable);
                var key = TermReader.ReadKey();
                if (key.Key == ConsoleKey.Home)
                    looping = false;
                else if (key.Key == ConsoleKey.M)
                    PointerListener.EnableMovementEvents = !PointerListener.EnableMovementEvents;
                else if (key.Key == ConsoleKey.I)
                    PointerListener.InvertScrollYAxis = !PointerListener.InvertScrollYAxis;
                else if (key.Key == ConsoleKey.L)
                    PointerListener.SwapLeftRightButtons = !PointerListener.SwapLeftRightButtons;
            }
            PointerListener.MouseEvent -= MouseEvent;
            PointerListener.StopListening();
        }

        private void MouseEvent(object sender, PointerEventContext e)
        {
            TextWriterColor.WriteColor($"{e.Coordinates.x}/{e.Coordinates.y} [{e.Button}, {e.ButtonPress}, {e.Modifiers}] [Dragging: {e.Dragging}, Tier: {e.ClickTier}]",
                e.ButtonPress == PointerButtonPress.Clicked ? ConsoleColors.Green :
                e.ButtonPress == PointerButtonPress.Released ? ConsoleColors.Purple :
                e.ButtonPress == PointerButtonPress.Scrolled ? ConsoleColors.Red :
                e.Dragging ? ConsoleColors.Yellow :
                ConsoleColors.Teal);
        }
    }
}
