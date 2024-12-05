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

using System;
using System.Linq;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Interactive;
using System.Reflection;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public static class SpinnerSelector
    {
        internal readonly static PropertyInfo[] builtinSpinners = typeof(BuiltinSpinners).GetProperties();
        internal readonly static string[] spinners = builtinSpinners.Select((pi) => pi.Name).ToArray();
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
            new("Previous", PointerButton.WheelUp, PointerButtonPress.Scrolled),
            new("Next", PointerButton.WheelDown, PointerButtonPress.Scrolled),
        ];

        /// <summary>
        /// Prompts the user for a spinner spinner
        /// </summary>
        /// <returns>Selected spinner spinner</returns>
        public static Spinner PromptForSpinner() =>
            PromptForSpinner(nameof(BuiltinSpinners.Dots));

        /// <summary>
        /// Prompts the user for a spinner spinner
        /// </summary>
        /// <param name="spinner">Initial spinner to select</param>
        /// <returns>Selected spinner spinner</returns>
        public static Spinner PromptForSpinner(string spinner)
        {
            var spinnerSelectorTui = new SpinnerSelectorTui(spinner);
            TextualUITools.RunTui(spinnerSelectorTui);
            return spinnerSelectorTui.GetResultingSpinner();
        }

        internal static int DetermineSpinnerIndex(string name)
        {
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            string[] spinners = builtinSpinners.Select((pi) => pi.Name).ToArray();
            int selectedSpinner = 0;
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
