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
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public static class SpinnerSelector
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
            new("Previous", PointerButton.WheelUp),
            new("Next", PointerButton.WheelDown),
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
            // Some initial variables to populate spinner spinners
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            string[] spinners = builtinSpinners.Select((pi) => pi.Name).ToArray();
            var spinnerSelections = InputChoiceTools.GetInputChoices(spinners.Select((spinner, num) => ($"{num}", spinner)).ToArray()).ToArray();
            string spinnerName = spinners.Contains(spinner) ? spinner : nameof(BuiltinSpinners.Dots);

            // Determine the spinner index
            int selectedSpinner = DetermineSpinnerIndex(spinnerName);
            int selectedSpinnerFallback = DetermineSpinnerIndex(spinnerName);

            // Now, clear the console and let the user select a spinner spinner while displaying a small text in the middle
            // of the console
            bool cancel = false;
            var screen = new Screen()
            {
                CycleFrequency = 50,
            };
            var selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
            var spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
            try
            {
                bool bail = false;

                // Make a buffer that represents the TUI
                var screenPart = new ScreenPart();
                screenPart.AddDynamicText(() =>
                {
                    var buffer = new StringBuilder();

                    // Write the selected spinner name and the keybindings
                    var spinnerInfo = new AlignedText($"{spinnerName} - [{selectedSpinner + 1}/{spinners.Length}]")
                    {
                        Top = 1,
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle
                        }
                    };
                    var spinnerKeybindings = new Keybindings()
                    {
                        Width = ConsoleWrapper.WindowWidth - 1,
                        Top = ConsoleWrapper.WindowHeight - 1,
                        KeybindingList = bindings,
                    };
                    buffer.Append(spinnerInfo.Render());
                    buffer.Append(spinnerKeybindings.Render());

                    // Write the rendered content using the selected spinner
                    if (spinnerObject is Spinner spinner)
                    {
                        var spinnerDisplay = new AlignedText(spinner.Render())
                        {
                            Settings = new()
                            {
                                Alignment = TextAlignment.Middle
                            }
                        };
                        buffer.Append(spinnerDisplay.Render());
                    }
                    return buffer.ToString();
                });

                // Now, make the interactive TUI resizable.
                screen.AddBufferedPart("Spinner selector", screenPart);
                ScreenTools.SetCurrent(screen);
                ScreenTools.SetCurrentCyclic(screen);
                ScreenTools.StartCyclicScreen();
                while (!bail)
                {
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
                                selectedSpinner--;
                                if (selectedSpinner < 0)
                                    selectedSpinner = spinners.Length - 1;
                                spinnerName = spinners[selectedSpinner];
                                selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                                spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
                                screen.RequireRefresh();
                                break;
                            case PointerButton.WheelDown:
                                selectedSpinner++;
                                if (selectedSpinner > spinners.Length - 1)
                                    selectedSpinner = 0;
                                spinnerName = spinners[selectedSpinner];
                                selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                                spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
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
                                selectedSpinner--;
                                if (selectedSpinner < 0)
                                    selectedSpinner = spinners.Length - 1;
                                spinnerName = spinners[selectedSpinner];
                                selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                                spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.RightArrow:
                                selectedSpinner++;
                                if (selectedSpinner > spinners.Length - 1)
                                    selectedSpinner = 0;
                                spinnerName = spinners[selectedSpinner];
                                selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                                spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.S:
                                bool write = key.Modifiers.HasFlag(ConsoleModifiers.Shift);
                                if (write)
                                {
                                    string promptedSpinnerName = InfoBoxInputColor.WriteInfoBoxInput("Write the spinner name. It'll be converted to lowercase.").ToLower();
                                    if (!spinners.Contains(promptedSpinnerName))
                                        InfoBoxModalColor.WriteInfoBoxModal("The spinner doesn't exist.");
                                    else
                                        spinnerName = promptedSpinnerName;
                                }
                                else
                                {
                                    selectedSpinner = InfoBoxSelectionColor.WriteInfoBoxSelection("Spinner selection", spinnerSelections, "Select a spinner spinner from the list below");
                                    spinnerName = spinners[selectedSpinner];
                                }
                                selectedSpinner = DetermineSpinnerIndex(spinnerName);
                                selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                                spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
                                screen.RequireRefresh();
                                break;
                            case ConsoleKey.H:
                                Keybinding[] allBindings = [.. bindings, .. additionalBindings];
                                KeybindingTools.ShowKeybindingInfobox(allBindings);
                                screen.RequireRefresh();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal("Spinner selector failed: " + ex.Message);
            }
            finally
            {
                ScreenTools.UnsetCurrent(screen);
                ColorTools.LoadBack();
            }

            // Get the final spinner object
            var fallbackSpinnerPropertyInfo = builtinSpinners[selectedSpinnerFallback];
            var finalFallbackSpinnerObject = fallbackSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
            return
                spinnerObject is Spinner spinnerRenderer && !cancel ? spinnerRenderer :
                finalFallbackSpinnerObject is Spinner spinnerFallbackRenderer ? spinnerFallbackRenderer :
                BuiltinSpinners.Dots;
        }

        private static int DetermineSpinnerIndex(string name)
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
