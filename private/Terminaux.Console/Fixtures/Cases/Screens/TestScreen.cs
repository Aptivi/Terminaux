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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs;
using Terminaux.Sequences.Builder.Types;

namespace Terminaux.Console.Fixtures.Cases.Screens
{
    internal class TestScreen : IFixture
    {
        public void RunFixture()
        {
            // Show the screen measurement sticks
            var stickScreen = new Screen();
            try
            {
                // First, clear the screen
                ColorTools.LoadBack();

                // Then, show the resizable sticks
                var stickScreenPart = new ScreenPart();
                stickScreenPart.Position(0, 1);
                stickScreenPart.BackgroundColor(new Color(ConsoleColors.Silver));
                stickScreenPart.AddDynamicText(GenerateWidthStick);
                stickScreenPart.AddDynamicText(GenerateHeightStick);
                stickScreenPart.AddDynamicText(() => new Color(ConsoleColors.White).VTSequenceForeground);
                stickScreenPart.AddDynamicText(() => new Color(ConsoleColors.Black).VTSequenceBackground);
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.Render();
                Input.ReadKey();
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

        private static string GenerateWidthStick() =>
            new(' ', ConsoleWrapper.WindowWidth);

        private static string GenerateHeightStick()
        {
            var stick = new StringBuilder();
            for (int i = 0; i < ConsoleWrapper.WindowHeight; i++)
            {
                stick.Append(CsiSequences.GenerateCsiCursorPosition(2, i));
                stick.Append(' ');
            }
            return stick.ToString();
        }
    }
}
