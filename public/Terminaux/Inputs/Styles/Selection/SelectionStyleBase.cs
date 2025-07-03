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

using System;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles.Selection
{
    internal static class SelectionStyleBase
    {
        internal static Keybinding[] ShowBindingsMultiple =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_CONFIRM"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SELECT"), ConsoleKey.Spacebar),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SIDEBAR"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_HELP"), ConsoleKey.K),
        ];

        internal static Keybinding[] ShowBindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SELECT"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SIDEBAR"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_HELP"), ConsoleKey.K),
        ];

        internal static Keybinding[] Bindings =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_CONFIRMSELECTIONS"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_CANCELSELECTIONS"), ConsoleKey.Escape),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GOUP"), ConsoleKey.UpArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GODOWN"), ConsoleKey.DownArrow),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_FIRSTELEMENT"), ConsoleKey.Home),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_LASTELEMENT"), ConsoleKey.End),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_PREVPAGE"), ConsoleKey.PageUp),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_NEXTPAGE"), ConsoleKey.PageDown),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SEARCH"), ConsoleKey.F),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GOUPSIDEBAR"), ConsoleKey.E, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GODOWNSIDEBAR"), ConsoleKey.D, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GOUPQUESTION"), ConsoleKey.E),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GODOWNQUESTION"), ConsoleKey.D),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SHOWCOUNT"), ConsoleKey.P),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SHOWITEMINFO"), ConsoleKey.Tab),
        ];

        internal static Keybinding[] BindingsMultiple =>
        [
            .. Bindings,
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SELECTONECHOICE"), ConsoleKey.Spacebar),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SELECTALLITEMSSAMEGROUPSAMECATEGORY"), ConsoleKey.A),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SELECTALLITEMSALLGROUPSSAMECATEGORIES"), ConsoleKey.A, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SELECTALLITEMSALLGROUPSALLCATEGORIES"), ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Control),
        ];

        internal static Keybinding[] BindingsMouse =>
        [
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GOUP"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_GODOWN"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_SELECT"), PointerButton.Left, PointerButtonPress.Released),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_SHOWITEMINFO"), PointerButton.Right, PointerButtonPress.Released),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_UPDATEPOSITION"), PointerButton.None, PointerButtonPress.Moved),
        ];

        internal static int[] PromptSelection(string question, InputChoiceCategoryInfo[] answers, InputChoiceCategoryInfo[] altAnswers, SelectionStyleSettings settings, bool kiosk, bool multiple)
        {
            settings ??= SelectionStyleSettings.GlobalSettings;

            // Make a new TUI instance
            var tui = new SelectionStyleTui(question, answers, altAnswers, settings, kiosk, multiple);
            TextualUITools.RunTui(tui);
            return tui.GetResultingChoices();
        }

        static SelectionStyleBase()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
