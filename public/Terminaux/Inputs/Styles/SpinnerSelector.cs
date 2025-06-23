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
using System.Linq;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Interactive;
using System.Reflection;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Base;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public static class SpinnerSelector
    {
        internal readonly static PropertyInfo[] builtinSpinners = typeof(BuiltinSpinners).GetProperties();
        internal readonly static string[] spinners = builtinSpinners.Select((pi) => pi.Name).ToArray();

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
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_PREV"), PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTORS_KEYBINDING_NEXT"), PointerButton.WheelDown, PointerButtonPress.Scrolled),
        ];

        /// <summary>
        /// Prompts the user for a spinner
        /// </summary>
        /// <returns>Selected spinner</returns>
        public static Spinner PromptForSpinner() =>
            PromptForSpinner(nameof(BuiltinSpinners.SpinMore));

        /// <summary>
        /// Prompts the user for a spinner
        /// </summary>
        /// <param name="spinner">Initial spinner to select</param>
        /// <returns>Selected spinner</returns>
        public static Spinner PromptForSpinner(string spinner)
        {
            ConsoleLogger.Debug("Initial spinner: {0}", spinner);
            var spinnerSelectorTui = new SpinnerSelectorTui(spinner)
            {
                RefreshDelay = 50
            };
            TextualUITools.RunTui(spinnerSelectorTui);
            var result = spinnerSelectorTui.GetResultingSpinner();
            ConsoleLogger.Debug("Result spinner: {0}", result);
            return spinnerSelectorTui.GetResultingSpinner();
        }

        internal static int DetermineSpinnerIndex(string name)
        {
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            string[] spinners = builtinSpinners.Select((pi) => pi.Name).ToArray();
            int selectedSpinner;
            for (selectedSpinner = 0; selectedSpinner < spinners.Length; selectedSpinner++)
            {
                string queriedSpinner = spinners[selectedSpinner];
                if (queriedSpinner == name)
                    break;
            }
            return selectedSpinner;
        }

        static SpinnerSelector()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
