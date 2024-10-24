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

using Textify.Data.Figlet;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters.Tools;
using Terminaux.Writer.MiscWriters;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Figlet selector
    /// </summary>
    public static class FigletSelector
    {
        private readonly static Keybinding[] bindings =
        [
            new("Previous", ConsoleKey.LeftArrow),
            new("Next", ConsoleKey.RightArrow),
            new("Submit", ConsoleKey.Enter),
            new("Cancel", ConsoleKey.Escape),
            new("Help", ConsoleKey.H),
        ];
        private readonly static Keybinding[] additionalBindings =
        [
            new("Select", ConsoleKey.S),
            new("Manual Select", ConsoleKey.S, ConsoleModifiers.Shift),
            new("Character Showcase", ConsoleKey.C),
            new("Previous", PointerButton.WheelUp),
            new("Next", PointerButton.WheelDown),
        ];
        private readonly static Keybinding[] charSelectBindings =
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
            // Some initial variables to populate figlet fonts
            string[] fonts = [.. FigletTools.GetFigletFonts().Keys];
            var figletSelections = InputChoiceTools.GetInputChoices(fonts.Select((font, num) => ($"{num}", font)).ToArray()).ToArray();
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

                    // Write the text using the selected figlet font
                    var figletFont = FigletTools.GetFigletFont(fontName);
                    buffer.Append(AlignedFigletTextColor.RenderAligned(figletFont, text, TextAlignment.Middle));

                    // Write the selected font name and the keybindings
                    buffer.Append(AlignedTextColor.RenderAligned(1, $"{fontName} - [{selectedFont + 1}/{fonts.Length}]", TextAlignment.Middle));
                    buffer.Append(KeybindingsWriter.RenderKeybindings(bindings, 0, ConsoleWrapper.WindowHeight - 1));
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
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    if (Input.MouseInputAvailable)
                    {
                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                selectedFont--;
                                if (selectedFont < 0)
                                    selectedFont = fonts.Length - 1;
                                fontName = fonts[selectedFont];
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelDown:
                                selectedFont++;
                                if (selectedFont > fonts.Length - 1)
                                    selectedFont = 0;
                                fontName = fonts[selectedFont];
                                screen.RequireRefresh();
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
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
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.RightArrow:
                                selectedFont++;
                                if (selectedFont > fonts.Length - 1)
                                    selectedFont = 0;
                                fontName = fonts[selectedFont];
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.S:
                                bool write = key.Modifiers.HasFlag(ConsoleModifiers.Shift);
                                if (write)
                                {
                                    string promptedFontName = InfoBoxInputColor.WriteInfoBoxInput("Write the font name. It'll be converted to lowercase.").ToLower();
                                    if (!fonts.Contains(promptedFontName))
                                        InfoBoxModalColor.WriteInfoBoxModal("The font doesn't exist.");
                                    else
                                        fontName = promptedFontName;
                                }
                                else
                                {
                                    selectedFont = InfoBoxSelectionColor.WriteInfoBoxSelection("Font selection", figletSelections, "Select a figlet font from the list below");
                                    fontName = fonts[selectedFont];
                                }
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.C:
                                ColorTools.LoadBack();
                                screen.RemoveBufferedPart(screenPart.Id);
                                ShowChars(screen, fontName);
                                screen.AddBufferedPart("Figlet selector", screenPart);
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.H:
                                Keybinding[] allBindings = [.. bindings, .. additionalBindings];
                                KeybindingsWriter.ShowKeybindingInfobox(allBindings);
                                screen.RequireRefresh();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal("Figlet selector failed: " + ex.Message);
            }
            finally
            {
                ScreenTools.UnsetCurrent(screen);
                ColorTools.LoadBack();
            }
            return cancel ? font : fontName;
        }

        private static void ShowChars(Screen screen, string fontName)
        {
            var screenPart = new ScreenPart();
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
                screenPart.AddDynamicText(() =>
                {
                    var buffer = new StringBuilder();

                    // Write the text using the selected figlet font
                    string character = ((char)chars[index]).ToString();
                    var figletFont = FigletTools.GetFigletFont(fontName);
                    buffer.Append(AlignedFigletTextColor.RenderAligned(figletFont, character, TextAlignment.Middle));

                    // Write the selected character name and the keybindings
                    buffer.Append(AlignedTextColor.RenderAligned(1, $"{character} - [{index + 1}/{chars.Length}]", TextAlignment.Middle));
                    buffer.Append(KeybindingsWriter.RenderKeybindings(charSelectBindings, 0, ConsoleWrapper.WindowHeight - 1));
                    return buffer.ToString();
                });

                // Now, make the interactive TUI resizable.
                screen.AddBufferedPart("Figlet selector - show characters", screenPart);
                while (!bail)
                {
                    // Render
                    ScreenTools.Render();

                    // Wait for input
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    if (Input.MouseInputAvailable)
                    {
                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                index--;
                                if (index < 0)
                                    index = chars.Length - 1;
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelDown:
                                index++;
                                if (index > chars.Length - 1)
                                    index = 0;
                                screen.RequireRefresh();
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        // Keyboard input received.
                        var key = Input.ReadKey().Key;
                        switch (key)
                        {
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.LeftArrow:
                                index--;
                                if (index < 0)
                                    index = chars.Length - 1;
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.RightArrow:
                                index++;
                                if (index > chars.Length - 1)
                                    index = 0;
                                screen.RequireRefresh();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal("Figlet selector failed: " + ex.Message);
            }
            finally
            {
                if (screen.CheckBufferedPart(screenPart.Id))
                    screen.RemoveBufferedPart(screenPart.Id);
            }
        }

        static FigletSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
