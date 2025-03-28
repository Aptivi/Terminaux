﻿//
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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public class SpinnerSelectorTui : TextualUI
    {
        private string spinner = nameof(BuiltinSpinners.Dots);
        private int selectedSpinner = SpinnerSelector.DetermineSpinnerIndex(nameof(BuiltinSpinners.Dots));
        private bool cancel = false;
        private readonly int selectedSpinnerFallback = SpinnerSelector.DetermineSpinnerIndex(nameof(BuiltinSpinners.Dots));

        /// <inheritdoc/>
        public override string Render()
        {
            // Some initial variables to populate spinner spinners
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            string spinnerName = SpinnerSelector.spinners.Contains(spinner) ? spinner : nameof(BuiltinSpinners.Dots);
            selectedSpinner = SpinnerSelector.DetermineSpinnerIndex(nameof(BuiltinSpinners.Dots));

            // Now, clear the console and let the user select a spinner spinner while displaying a small text in the middle
            // of the console
            var screen = new Screen()
            {
                CycleFrequency = 50,
            };
            var selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
            var spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
            var buffer = new StringBuilder();

            // Write the selected spinner name and the keybindings
            var spinnerInfo = new AlignedText($"{spinnerName} - [[{selectedSpinner + 1}/{SpinnerSelector.spinners.Length}]]")
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
                KeybindingList = SpinnerSelector.bindings,
                WriteHelpKeyInfo = false,
            };
            buffer.Append(spinnerInfo.Render());
            buffer.Append(RendererTools.RenderRenderable(spinnerKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));

            // Write the rendered content using the selected spinner
            if (spinnerObject is Spinner spinnerRenderable)
            {
                var spinnerDisplay = new AlignedText()
                {
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                    Text = spinnerRenderable.Render()
                };
                buffer.Append(spinnerDisplay.Render());
            }
            return buffer.ToString();
        }

        internal Spinner GetResultingSpinner()
        {
            int selectedSpinner = SpinnerSelector.DetermineSpinnerIndex(spinner);
            var selectedSpinnerPropertyInfo = !cancel ? SpinnerSelector.builtinSpinners[selectedSpinner] : null;
            var fallbackSpinnerPropertyInfo = SpinnerSelector.builtinSpinners[selectedSpinnerFallback];
            var spinnerObject =
                selectedSpinnerPropertyInfo?.GetGetMethod()?.Invoke(null, null) ??
                fallbackSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null) ??
                BuiltinSpinners.Dots;
            return (Spinner)spinnerObject;
        }

        private void Previous(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedSpinner--;
            if (selectedSpinner < 0)
                selectedSpinner = SpinnerSelector.spinners.Length - 1;
            spinner = SpinnerSelector.spinners[selectedSpinner];
            ui.RequireRefresh();
        }

        private void Next(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedSpinner++;
            if (selectedSpinner > SpinnerSelector.spinners.Length - 1)
                selectedSpinner = 0;
            spinner = SpinnerSelector.spinners[selectedSpinner];
            ui.RequireRefresh();
        }

        private void Exit(TextualUI ui, bool cancel)
        {
            this.cancel = cancel;
            TextualUITools.ExitTui(ui);
        }

        private void Help(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            Keybinding[] allBindings = [.. SpinnerSelector.bindings, .. SpinnerSelector.additionalBindings];
            KeybindingTools.ShowKeybindingInfobox(allBindings);
            ui.RequireRefresh();
        }

        private void Select(TextualUI ui, bool write)
        {
            if (write)
            {
                string promptedSpinnerName = InfoBoxInputColor.WriteInfoBoxInput("Write the spinner name. It'll be converted to lowercase.").ToLower();
                if (!SpinnerSelector.spinners.Contains(promptedSpinnerName))
                    InfoBoxModalColor.WriteInfoBoxModal("The spinner doesn't exist.");
                else
                    spinner = promptedSpinnerName;
            }
            else
            {
                InputChoiceInfo[] spinnerSelections = [.. InputChoiceTools.GetInputChoices(SpinnerSelector.spinners.Select((spinner, num) => ($"{num}", spinner)).ToArray())];
                selectedSpinner = InfoBoxSelectionColor.WriteInfoBoxSelection("Spinner selection", spinnerSelections, "Select a spinner spinner from the list below");
                spinner = SpinnerSelector.spinners[selectedSpinner];
            }
            selectedSpinner = SpinnerSelector.DetermineSpinnerIndex(spinner);
            ui.RequireRefresh();
        }

        internal SpinnerSelectorTui(string name)
        {
            spinner = name;

            // Keyboard bindings
            Keybindings.Add((SpinnerSelector.bindings[0], Previous));
            Keybindings.Add((SpinnerSelector.bindings[1], Next));
            Keybindings.Add((SpinnerSelector.bindings[2], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((SpinnerSelector.bindings[3], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((SpinnerSelector.bindings[4], Help));
            Keybindings.Add((SpinnerSelector.additionalBindings[0], (ui, _, _) => Select(ui, false)));
            Keybindings.Add((SpinnerSelector.additionalBindings[1], (ui, _, _) => Select(ui, true)));

            // Mouse bindings
            Keybindings.Add((SpinnerSelector.additionalBindings[2], Previous));
            Keybindings.Add((SpinnerSelector.additionalBindings[3], Next));
        }
    }
}
