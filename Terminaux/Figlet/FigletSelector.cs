
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Figgle;
using System;
using System.Linq;
using Terminaux.Reader.Inputs;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Figlet
{
    /// <summary>
    /// Figlet selector
    /// </summary>
    public static class FigletSelector
    {
        /// <summary>
        /// Prompts the user for a figlet font
        /// </summary>
        /// <returns>Selected figlet font</returns>
        public static string PromptForFiglet()
        {
            // Some initial variables to populate figlet fonts
            string fontName = "";
            string[] fonts = FigletTools.FigletFonts.Keys.ToArray();

            // Now, clear the console and let the user select a figlet font while displaying a small text in the middle
            // of the console
            bool bail = false;
            string text = "Test";
            int selectedFont = 0;
            while (!bail)
            {
                Console.CursorVisible = false;
                Console.Clear();

                // Write the text using the selected figlet font
                fontName = fonts[selectedFont];
                var figletFont = FigletTools.GetFigletFont(fontName);
                CenteredFigletTextColor.WriteCenteredFiglet(figletFont, text);

                // Write the selected font name and the keybindings
                CenteredTextColor.WriteCentered(Console.WindowHeight - 4, fontName);
                CenteredTextColor.WriteCentered(Console.WindowHeight - 2, "[ENTER] Select | [<-|->] Select");

                // Wait for input
                var key = Input.DetectKeypress().Key;
                switch (key)
                {
                    case ConsoleKey.Enter:
                        bail = true;
                        break;
                    case ConsoleKey.LeftArrow:
                        selectedFont--;
                        if (selectedFont < 0)
                            selectedFont = fonts.Length - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        selectedFont++;
                        if (selectedFont > fonts.Length - 1)
                            selectedFont = 0;
                        break;
                }
            }

            Console.Clear();
            return fontName;
        }
    }
}
