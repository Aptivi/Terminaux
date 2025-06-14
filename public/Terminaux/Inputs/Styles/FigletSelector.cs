//
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

using Textify.Data.Figlet;
using System;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Base;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Figlet selector
    /// </summary>
    public static class FigletSelector
    {
        internal readonly static string[] fonts = [.. FigletTools.GetFigletFonts().Keys];

        internal static Keybinding[] Bindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SUBMIT"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_CANCEL"), ConsoleKey.Escape),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_HELP"), ConsoleKey.H),
        ];

        internal static Keybinding[] AdditionalBindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SELECT"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_MANUALSELECT"), ConsoleKey.S, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_FIGLETSELECTOR_KEYBINDING_SHOWCASE"), ConsoleKey.C),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
        ];

        internal static Keybinding[] CharSelectBindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_DONE"), ConsoleKey.Enter),
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
            ConsoleLogger.Debug("Initial font: {0}", font);
            var figletSelectorTui = new FigletSelectorTui(font);
            TextualUITools.RunTui(figletSelectorTui);
            var result = figletSelectorTui.GetResultingFigletName();
            ConsoleLogger.Debug("Result font: {0}", font);
            return result;
        }

        internal static int DetermineFigletIndex(string name)
        {
            int selectedFont;
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
