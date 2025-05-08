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
using Terminaux.Base.Checks;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles.Selection
{
    internal static class SelectionStyleBase
    {
        internal static Keybinding[] showBindingsMultiple =
        [
            new("Confirm", ConsoleKey.Enter),
            new("Select", ConsoleKey.Spacebar),
            new("Sidebar", ConsoleKey.S),
            new("More", ConsoleKey.Tab),
            new("Help", ConsoleKey.K),
        ];
        internal static Keybinding[] showBindings =
        [
            new("Select", ConsoleKey.Enter),
            new("Sidebar", ConsoleKey.S),
            new("More", ConsoleKey.Tab),
            new("Help", ConsoleKey.K),
        ];
        internal static Keybinding[] bindings =
        [
            new("Confirms selection(s)", ConsoleKey.Enter),
            new("Cancel selection(s)", ConsoleKey.Escape),
            new("Goes one element up", ConsoleKey.UpArrow),
            new("Goes one element down", ConsoleKey.DownArrow),
            new("Goes to the first element", ConsoleKey.Home),
            new("Goes to the last element", ConsoleKey.End),
            new("Goes to the previous page", ConsoleKey.PageUp),
            new("Goes to the next page", ConsoleKey.PageDown),
            new("Searches for an element", ConsoleKey.F),
            new("Go up in a sidebar", ConsoleKey.E, ConsoleModifiers.Shift),
            new("Go down in a sidebar", ConsoleKey.D, ConsoleModifiers.Shift),
            new("Go up in a question box", ConsoleKey.E),
            new("Go down in a question box", ConsoleKey.D),
            new("Show page and choice count", ConsoleKey.P),
            new("Show item info", ConsoleKey.Tab),
        ];
        internal static Keybinding[] bindingsMultiple =
        [
            .. bindings,
            new("Selects or deselects a choice", ConsoleKey.Spacebar),
            new("Selects all the elements in the same group and category", ConsoleKey.A),
            new("Selects all the elements in all groups in the same category", ConsoleKey.A, ConsoleModifiers.Shift),
            new("Selects all the elements in all groups in all categories", ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Control),
        ];
        internal static Keybinding[] bindingsMouse =
        [
            new("Goes one element or showcase line up", PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new("Goes one element or showcase line down", PointerButton.WheelDown, PointerButtonPress.Scrolled),
            new("Select", PointerButton.Left, PointerButtonPress.Released),
            new("Show item info", PointerButton.Right, PointerButtonPress.Released),
            new("Update position", PointerButton.None, PointerButtonPress.Moved),
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
