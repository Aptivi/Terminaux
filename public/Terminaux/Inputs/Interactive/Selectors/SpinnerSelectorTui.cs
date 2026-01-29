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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Pointer;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    /// <summary>
    /// Spinner selector
    /// </summary>
    public class SpinnerSelectorTui : TextualUI
    {
        private string spinner = nameof(BuiltinSpinners.SpinMore);
        private int selectedSpinner = DetermineSpinnerIndex(nameof(BuiltinSpinners.SpinMore));
        private bool cancel = false;
        private bool invalidate = true;
        private Spinner spinnerInstance = BuiltinSpinners.SpinMore;
        private readonly int selectedSpinnerFallback = DetermineSpinnerIndex(nameof(BuiltinSpinners.SpinMore));

        /// <inheritdoc/>
        public override string Render()
        {
            // Some initial variables to populate spinners
            var builtinSpinners = typeof(BuiltinSpinners).GetProperties();
            string spinnerName = SpinnerSelector.spinners.Contains(spinner) ? spinner : nameof(BuiltinSpinners.SpinMore);

            // Now, clear the console and let the user select a spinner while displaying a small text in the middle
            // of the console
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
                KeybindingList = SpinnerSelector.Bindings,
                WriteHelpKeyInfo = false,
            };
            buffer.Append(spinnerInfo.Render());
            buffer.Append(RendererTools.RenderRenderable(spinnerKeybindings, new(0, ConsoleWrapper.WindowHeight - 1)));

            // Determine whether to repopulate cached spinner instance or not
            if (invalidate)
            {
                invalidate = false;
                var selectedSpinnerPropertyInfo = builtinSpinners[selectedSpinner];
                var spinnerObject = selectedSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null);
                if (spinnerObject is Spinner finalSpinner)
                    spinnerInstance = finalSpinner;
            }

            // Write the rendered content using the selected spinner
            var spinnerDisplay = new AlignedText()
            {
                Top = ConsoleWrapper.WindowHeight / 2,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                },
                Text = spinnerInstance.Render()
            };
            buffer.Append(spinnerDisplay.Render());
            return buffer.ToString();
        }

        internal Spinner GetResultingSpinner()
        {
            int selectedSpinner = DetermineSpinnerIndex(spinner);
            var selectedSpinnerPropertyInfo = !cancel ? SpinnerSelector.builtinSpinners[selectedSpinner] : null;
            var fallbackSpinnerPropertyInfo = SpinnerSelector.builtinSpinners[selectedSpinnerFallback];
            var spinnerObject =
                selectedSpinnerPropertyInfo?.GetGetMethod()?.Invoke(null, null) ??
                fallbackSpinnerPropertyInfo.GetGetMethod()?.Invoke(null, null) ??
                BuiltinSpinners.SpinMore;
            return (Spinner)spinnerObject;
        }

        private void Previous(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedSpinner--;
            if (selectedSpinner < 0)
                selectedSpinner = SpinnerSelector.spinners.Length - 1;
            invalidate = true;
            spinner = SpinnerSelector.spinners[selectedSpinner];
            ui.RequireRefresh();
        }

        private void Next(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            selectedSpinner++;
            if (selectedSpinner > SpinnerSelector.spinners.Length - 1)
                selectedSpinner = 0;
            invalidate = true;
            spinner = SpinnerSelector.spinners[selectedSpinner];
            ui.RequireRefresh();
        }

        private void Exit(TextualUI ui, bool cancel)
        {
            this.cancel = cancel;
            TextualUITools.ExitTui(ui);
        }

        private void Select(TextualUI ui, bool write)
        {
            if (write)
            {
                string promptedSpinnerName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_IS_SPINNER_SPINNERNAMEPROMPT")).ToLower();
                if (!SpinnerSelector.spinners.Contains(promptedSpinnerName))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_IS_SPINNER_NOSPINNER"));
                    return;
                }
                else
                    spinner = promptedSpinnerName;
            }
            else
            {
                InputChoiceInfo[] spinnerSelections = [.. InputChoiceTools.GetInputChoices(SpinnerSelector.spinners.Select((spinner, num) => ($"{num}", spinner)).ToArray())];
                selectedSpinner = InfoBoxSelectionColor.WriteInfoBoxSelection(spinnerSelections, LanguageTools.GetLocalized("T_INPUT_IS_SPINNER_SPINNERSELECTPROMPT"), new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_IS_SPINNER_SPINNERSELECTPROMPTTITLE")
                });
                spinner = SpinnerSelector.spinners[selectedSpinner];
            }
            invalidate = true;
            selectedSpinner = DetermineSpinnerIndex(spinner);
            ui.RequireRefresh();
        }

        private static int DetermineSpinnerIndex(string name)
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

        internal SpinnerSelectorTui(string name)
        {
            spinner = name;
            selectedSpinner = DetermineSpinnerIndex(nameof(BuiltinSpinners.SpinMore));

            // Keyboard bindings
            Keybindings.Add((SpinnerSelector.Bindings[0], Previous));
            Keybindings.Add((SpinnerSelector.Bindings[1], Next));
            Keybindings.Add((SpinnerSelector.Bindings[2], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((SpinnerSelector.Bindings[3], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((SpinnerSelector.AdditionalBindings[0], (ui, _, _) => Select(ui, false)));
            Keybindings.Add((SpinnerSelector.AdditionalBindings[1], (ui, _, _) => Select(ui, true)));

            // Mouse bindings
            Keybindings.Add((SpinnerSelector.AdditionalBindings[2], Previous));
            Keybindings.Add((SpinnerSelector.AdditionalBindings[3], Next));
        }
    }
}
