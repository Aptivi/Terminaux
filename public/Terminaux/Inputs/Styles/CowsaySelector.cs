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
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Base;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Cowsay selector
    /// </summary>
    public static class CowsaySelector
    {
        internal static Keybinding[] Bindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), ConsoleKey.LeftArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), ConsoleKey.RightArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SUBMIT"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_CANCEL"), ConsoleKey.Escape),
        ];

        internal static Keybinding[] AdditionalBindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SELECT"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_MANUALSELECT"), ConsoleKey.S, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
        ];

        /// <summary>
        /// Prompts the user for a cowsay font
        /// </summary>
        /// <returns>Selected cowsay font</returns>
        public static CowName PromptForCowsay() =>
            PromptForCowsay(CowName.Default);

        /// <summary>
        /// Prompts the user for a cowsay font
        /// </summary>
        /// <param name="font">Initial font to select</param>
        /// <returns>Selected cowsay font</returns>
        public static CowName PromptForCowsay(CowName font)
        {
            ConsoleLogger.Debug("Initial font: {0}", font);
            var cowsaySelectorTui = new CowsaySelectorTui(font);
            TextualUITools.RunTui(cowsaySelectorTui);
            var result = cowsaySelectorTui.GetResultingCowsay();
            ConsoleLogger.Debug("Result font: {0}", font);
            return result;
        }
    }
}
