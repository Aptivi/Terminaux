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

using Figletize;
using System;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Inputs.Styles
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
            bool cancel = false;
            var screen = new Screen();
            try
            {
                bool bail = false;
                string text = "Test";

                // Make a buffer that represents the TUI
                var screenPart = new ScreenPart();
                screenPart.AddDynamicText(() =>
                {
                    var buffer = new StringBuilder();
                    ConsoleWrapper.CursorVisible = false;
                    ConsoleWrapper.Clear();

                    // Write the text using the selected figlet font
                    var figletFont = FigletTools.GetFigletFont(fontName);
                    buffer.Append(CenteredFigletTextColor.RenderCenteredFiglet(figletFont, text));

                    // Write the selected font name and the keybindings
                    buffer.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 4, fontName));
                    buffer.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 2, "[ESC] Cancel | [ENTER] Submit | [<-|->] Select | [H] Help"));
                    return buffer.ToString();
                });

                // Now, make the interactive TUI resizable.
                screen.AddBufferedPart("Figlet selector", screenPart);
                ScreenTools.SetCurrent(screen);
                while (!bail)
                {
                    // Render
                    ScreenTools.Render();

                    // Wait for input
                    var key = TermReader.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            bail = true;
                            break;
                        case ConsoleKey.Escape:
                            bail = true;
                            cancel = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            selectedFont--;
                            if (selectedFont < 0)
                                selectedFont = fonts.Length - 1;
                            fontName = fonts[selectedFont];
                            break;
                        case ConsoleKey.RightArrow:
                            selectedFont++;
                            if (selectedFont > fonts.Length - 1)
                                selectedFont = 0;
                            fontName = fonts[selectedFont];
                            break;
                        case ConsoleKey.S:
                            string promptedFontName = InfoBoxInputColor.WriteInfoBoxInput("Write the font name. It'll be converted to lowercase.").ToLower();
                            if (!fonts.Contains(promptedFontName))
                                InfoBoxColor.WriteInfoBox("The font doesn't exist.");
                            else
                                fontName = promptedFontName;
                            break;
                        case ConsoleKey.C:
                            screen.RemoveBufferedPart("Figlet selector");
                            ShowChars(screen, fontName);
                            screen.AddBufferedPart("Figlet selector", screenPart);
                            break;
                        case ConsoleKey.H:
                            InfoBoxColor.WriteInfoBox("Available keybindings",
                                $$"""
                                [ENTER]              | Accept font
                                [ESC]                | Exit
                                [H]                  | Help page
                                [S]                  | Write font name
                                [C]                  | Shows the individual characters
                                """
                            );
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox("Figlet selector failed: " + ex.Message);
            }
            finally
            {
                ScreenTools.UnsetCurrent(screen);
                ConsoleWrapper.Clear();
            }
            return cancel ? font : fontName;
        }

        private static void ShowChars(Screen screen, string fontName)
        {
            try
            {
                // Capital letters are from range 65 to 90, small letters are from range 97 to 122, and numbers are
                // from range 48 to 57.
                bool bail = false;
                int[] chars = Enumerable.Range(65, 90 - 65 + 1)
                    .Union(Enumerable.Range(97, 122 - 97 + 1))
                    .Union(Enumerable.Range(48, 57 - 48 + 1))
                    .ToArray();
                int index = 0;

                // Make a buffer that represents the TUI
                var screenPart = new ScreenPart();
                screenPart.AddDynamicText(() =>
                {
                    var buffer = new StringBuilder();
                    ConsoleWrapper.CursorVisible = false;
                    ConsoleWrapper.Clear();

                    // Write the text using the selected figlet font
                    var figletFont = FigletTools.GetFigletFont(fontName);
                    buffer.Append(CenteredFigletTextColor.RenderCenteredFiglet(figletFont, ((char)chars[index]).ToString()));

                    // Write the selected font name and the keybindings
                    buffer.Append(CenteredTextColor.RenderCentered(ConsoleWrapper.WindowHeight - 2, "[ENTER] Go back | [<-|->] Select character"));
                    return buffer.ToString();
                });

                // Now, make the interactive TUI resizable.
                screen.AddBufferedPart("Figlet selector - show characters", screenPart);
                ScreenTools.SetCurrent(screen);
                while (!bail)
                {
                    // Render
                    ScreenTools.Render();

                    // Wait for input
                    var key = TermReader.ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            bail = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            index--;
                            if (index < 0)
                                index = chars.Length - 1;
                            break;
                        case ConsoleKey.RightArrow:
                            index++;
                            if (index > chars.Length - 1)
                                index = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBox("Figlet selector failed: " + ex.Message);
            }
            finally
            {
                if (screen.CheckBufferedPart("Figlet selector - show characters"))
                    screen.RemoveBufferedPart("Figlet selector - show characters");
                ConsoleWrapper.Clear();
            }
        }
    }
}