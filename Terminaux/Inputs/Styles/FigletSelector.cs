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

using Textify.Data.Figlet;
using System;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Figlet selector
    /// </summary>
    public static class FigletSelector
    {
        internal readonly static string[] fonts = [.. FigletTools.GetFigletFonts().Keys];
        internal readonly static Keybinding[] bindings =
        [
            new("Previous", ConsoleKey.LeftArrow),
            new("Next", ConsoleKey.RightArrow),
            new("Submit", ConsoleKey.Enter),
            new("Cancel", ConsoleKey.Escape),
            new("Help", ConsoleKey.H),
        ];
        internal readonly static Keybinding[] additionalBindings =
        [
            new("Select", ConsoleKey.S),
            new("Manual Select", ConsoleKey.S, ConsoleModifiers.Shift),
            new("Character Showcase", ConsoleKey.C),
            new("Previous", PointerButton.WheelUp),
            new("Next", PointerButton.WheelDown),
        ];
        internal readonly static Keybinding[] charSelectBindings =
        [
            new("Previous", ConsoleKey.LeftArrow),
            new("Next", ConsoleKey.RightArrow),
            new("Done", ConsoleKey.Enter),
        ];

        /// <summary>
        /// Prompts the user for a figlet font
        /// </summary>
        /// <returns>Selected figlet font</returns>
        public static string PromptForFiglet() =>
            PromptForFiglet("small");

        /// <summary>
        /// Prompts the user for a figlet font
        /// </summary>
        /// <param name="font">Initial font to select</param>
        /// <returns>Selected figlet font</returns>
        public static string PromptForFiglet(string font)
        {
            var figletSelectorTui = new FigletSelectorTui(font);
            TextualUITools.RunTui(figletSelectorTui);
            return figletSelectorTui.GetResultingFigletName();
        }

        internal static int DetermineFigletIndex(string name)
        {
            int selectedFont = 0;
            for (selectedFont = 0; selectedFont < fonts.Length; selectedFont++)
            {
                string queriedFont = fonts[selectedFont];
                if (queriedFont == name)
                    break;
            }
            return selectedFont;
        }

        static FigletSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
