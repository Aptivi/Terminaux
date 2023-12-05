
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

using Figletize;
using System;
using System.Linq;
using Terminaux.Base;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox;
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
        public static string PromptForFiglet() =>
            PromptForFiglet("small");

        /// <summary>
        /// Prompts the user for a figlet font
        /// </summary>
        /// <param name="font">Initial font to select</param>
        /// <returns>Selected figlet font</returns>
        public static string PromptForFiglet(string font)
        {
            // Some initial variables to populate figlet fonts
            string[] fonts = [.. FigletTools.GetFigletFonts().Keys];
            string fontName = fonts.Contains(font) ? font : "small";

            // Determine the font index
            int selectedFont;
            for (selectedFont = 0; selectedFont < fonts.Length; selectedFont++)
            {
                string queriedFont = fonts[selectedFont];
                if (queriedFont == fontName)
                    break;
            }

            // Now, clear the console and let the user select a figlet font while displaying a small text in the middle
            // of the console
            bool bail = false;
            string text = "Test";
            bool rerender = true;
            while (!bail)
            {
                if (rerender)
                {
                    rerender = false;
                    ConsoleWrapper.CursorVisible = false;
                    ConsoleWrapper.Clear();
                }

                // Write the text using the selected figlet font
                var figletFont = FigletTools.GetFigletFont(fontName);
                CenteredFigletTextColor.WriteCenteredFiglet(figletFont, text);

                // Write the selected font name and the keybindings
                CenteredTextColor.WriteCentered(ConsoleWrapper.WindowHeight - 4, fontName);
                CenteredTextColor.WriteCentered(ConsoleWrapper.WindowHeight - 2, "[ENTER] Select | [<-|->] Select | [S] Write font name");

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
                        fontName = fonts[selectedFont];
                        rerender = true;
                        break;
                    case ConsoleKey.RightArrow:
                        selectedFont++;
                        if (selectedFont > fonts.Length - 1)
                            selectedFont = 0;
                        fontName = fonts[selectedFont];
                        rerender = true;
                        break;
                    case ConsoleKey.S:
                        string promptedFontName = InfoBoxInputColor.WriteInfoBoxInput("Write the font name. It'll be converted to lowercase.").ToLower();
                        if (!fonts.Contains(promptedFontName))
                            InfoBoxColor.WriteInfoBox("The font doesn't exist.");
                        else
                            fontName = promptedFontName;
                        rerender = true;
                        break;
                }
            }

            ConsoleWrapper.Clear();
            return fontName;
        }
    }
}
