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
using System.Text.RegularExpressions;
using Magico.Enumeration;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    internal class InteractiveSelectorTui<TPrimary, TSecondary> : TextualUI
    {
        internal InteractiveTuiHelpPage[] extraHelpPages = [];
        private readonly BaseInteractiveTui<TPrimary, TSecondary> selectorTui;
        private int paneCurrentSelection;

        public override InteractiveTuiHelpPage[] HelpPages
        {
            get
            {
                string moreKeybindingsHelpPageBody = GetDynamicHelpPageBody();
                var moreKeybindingsHelpPage = new InteractiveTuiHelpPage()
                {
                    HelpTitle = "T_INPUT_COMMON_KEYBINDING_KEYBINDINGS",
                    HelpDescription = "T_WRITER_CYCLICWRITERS_TOOLS_KEYBINDING_AVAILABLE_KEYBINDINGS",
                    HelpBody = moreKeybindingsHelpPageBody
                };
                return [moreKeybindingsHelpPage, .. extraHelpPages];
            }
        }

        public override string Render()
        {
            var builder = new StringBuilder();

            // Check position
            InteractiveTuiTools.FirstOnUnderflow(selectorTui);
            InteractiveTuiTools.LastOnOverflow(selectorTui);

            // Draw the boxes
            builder.Append(InteractiveTuiTools.RenderInteractiveTui(selectorTui));

            // Draw the first pane
            builder.Append(InteractiveTuiTools.RenderInteractiveTuiItems(selectorTui, 1));

            // Draw the second pane
            if (selectorTui.SecondPaneInteractable)
                builder.Append(InteractiveTuiTools.RenderInteractiveTuiItems(selectorTui, 2));
            else
                builder.Append(InteractiveTuiTools.RenderInformationOnSecondPane(selectorTui));
            builder.Append(InteractiveTuiTools.RenderStatus(selectorTui));

            // Return the result
            return builder.ToString();
        }

        private void GoUp()
        {
            if (selectorTui.CurrentPane == 2)
                InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.SecondPaneCurrentSelection - 1);
            else
                InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection - 1);
        }

        private void GoDown()
        {
            if (selectorTui.CurrentPane == 2)
                InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.SecondPaneCurrentSelection + 1);
            else
                InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection + 1);
        }

        private void GoUpDeterministic(PointerEventContext? mouse)
        {
            if (mouse is null)
                return;
            if (selectorTui.SecondPaneInteractable)
                GoUp();
            else
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior + 1)
                    InteractiveTuiTools.InfoScrollUp(selectorTui, 3);
                else
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection - 3);
            }
        }

        private void GoDownDeterministic(PointerEventContext? mouse)
        {
            if (mouse is null)
                return;
            if (selectorTui.SecondPaneInteractable)
                GoDown();
            else
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior + 1)
                    InteractiveTuiTools.InfoScrollDown(selectorTui, 3);
                else
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection + 3);
            }
        }

        private void First()
        {
            InteractiveTuiTools.SelectionMovement(selectorTui, 1);
        }

        private void Last()
        {
            var dataPrimary = selectorTui.PrimaryDataSource;
            var dataSecondary = selectorTui.SecondaryDataSource;
            int dataCount = selectorTui.CurrentPane == 2 ? dataSecondary.Length() : dataPrimary.Length();
            InteractiveTuiTools.SelectionMovement(selectorTui, dataCount);
        }

        private void PreviousPage()
        {
            int paneCurrentSelection = selectorTui.CurrentPane == 2 ? selectorTui.SecondPaneCurrentSelection : selectorTui.FirstPaneCurrentSelection;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            int targetItemNum = paneCurrentSelection - SeparatorMaximumHeightInterior;
            InteractiveTuiTools.SelectionMovement(selectorTui, targetItemNum);
        }

        private void NextPage()
        {
            int paneCurrentSelection = selectorTui.CurrentPane == 2 ? selectorTui.SecondPaneCurrentSelection : selectorTui.FirstPaneCurrentSelection;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            int targetItemNum = paneCurrentSelection + SeparatorMaximumHeightInterior;
            InteractiveTuiTools.SelectionMovement(selectorTui, targetItemNum);
        }

        private void More()
        {
            string finalInfoRendered = InteractiveTuiTools.RenderFinalInfo(selectorTui);
            if (!string.IsNullOrEmpty(finalInfoRendered))
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered, selectorTui.Settings.InfoBoxSettings);
        }

        private void UpdateSelectionBasedOnMouse(PointerEventContext? mouse)
        {
            if (mouse is null)
                return;

            // First, check to see if the cursor has moved to the other side or not
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            if (mouse.Coordinates.y < SeparatorMinimumHeight || mouse.Coordinates.y > SeparatorMaximumHeightInterior + 2)
                return;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int oldPane = selectorTui.CurrentPane;
            if (selectorTui.SecondPaneInteractable)
            {
                if (mouse.Coordinates.x >= 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidthInterior - 1)
                {
                    if (selectorTui.CurrentPane != 1)
                        selectorTui.CurrentPane = 1;
                }
                else if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth + 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior)
                {
                    if (selectorTui.CurrentPane != 2)
                        selectorTui.CurrentPane = 2;
                }
                else
                    return;
            }
            else
            {
                if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth - 1)
                    return;
                if (mouse.Coordinates.x < 1)
                    return;
            }

            // Now, update the selection relative to the mouse pointer location
            int SeparatorMinimumHeightInterior = 2;
            int answersPerPage = SeparatorMaximumHeightInterior;
            paneCurrentSelection = selectorTui.CurrentPane == 2 ? selectorTui.SecondPaneCurrentSelection : selectorTui.FirstPaneCurrentSelection;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            if (mouse.Coordinates.y < SeparatorMinimumHeightInterior || mouse.Coordinates.y >= SeparatorMaximumHeightInterior + 2)
                return;
            int listIndex = mouse.Coordinates.y - SeparatorMinimumHeightInterior;
            listIndex = startIndex + listIndex;
            int dataCount = GetDataCount();
            if (listIndex + 1 > dataCount)
                return;
            listIndex = listIndex > dataCount ? dataCount : listIndex;
            if (listIndex + 1 != paneCurrentSelection || selectorTui.CurrentPane != oldPane)
            {
                if (listIndex + 1 != paneCurrentSelection)
                    InteractiveTuiTools.SelectionMovement(selectorTui, listIndex + 1);
            }

            // Check the tier
            if (mouse.ClickTier > 1)
            {
                // Go further and open the item
                var allBindings = selectorTui.Bindings;
                if (allBindings is null || allBindings.Count == 0)
                    return;
                var submitBindings = allBindings.Where((binding) => binding.BindingKeyName == ConsoleKey.Enter && binding.BindingKeyModifiers == 0);
                var dataPrimary = selectorTui.PrimaryDataSource;
                var dataSecondary = selectorTui.SecondaryDataSource;
                object? selectedData = dataPrimary.GetElementFromIndex(selectorTui.FirstPaneCurrentSelection - 1);
                object? selectedDataSecondary = dataSecondary.GetElementFromIndex(selectorTui.SecondPaneCurrentSelection - 1);
                TPrimary? selectedDataCasted = selectedData is not null ? (TPrimary?)selectedData : default;
                TSecondary? selectedDataSecondaryCasted = selectedDataSecondary is not null ? (TSecondary?)selectedDataSecondary : default;
                object? finalData = selectorTui.CurrentPane == 2 ? selectedDataSecondary : selectedData;
                foreach (var implementedBinding in submitBindings)
                {
                    var binding = implementedBinding.BindingAction;
                    if (binding is null || (finalData is null && !implementedBinding.BindingCanRunWithoutItems))
                        continue;
                    binding.Invoke(selectedDataCasted, selectorTui.FirstPaneCurrentSelection - 1, selectedDataSecondaryCasted, selectorTui.SecondPaneCurrentSelection - 1);
                }
            }
        }

        private void Act(ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (mouse is not null)
            {
                // First, determine the arrow positions
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                int leftPaneArrowLeft = SeparatorHalfConsoleWidthInterior + 1;
                int rightPaneArrowLeft = SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 4 : 3);
                int paneArrowTop = 2;
                int paneArrowBottom = SeparatorMaximumHeightInterior + 1;

                // Generate the arrow hitboxes
                var leftArrowUpHitbox = new PointerHitbox(new(leftPaneArrowLeft, paneArrowTop), (_) => GoUp()) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                var leftArrowDownHitbox = new PointerHitbox(new(leftPaneArrowLeft, paneArrowBottom), (_) => GoDown()) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                var rightArrowUpHitbox = new PointerHitbox(new(rightPaneArrowLeft, paneArrowTop), selectorTui.SecondPaneInteractable ? (_) => GoUp() : (_) => InteractiveTuiTools.InfoScrollUp(selectorTui)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                var rightArrowDownHitbox = new PointerHitbox(new(rightPaneArrowLeft, paneArrowBottom), selectorTui.SecondPaneInteractable ? (_) => GoDown() : (_) => InteractiveTuiTools.InfoScrollDown(selectorTui)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                // Test for pane to get the correct data count
                int paneNum = 1;
                if (rightArrowUpHitbox.IsPointerWithin(mouse) || rightArrowDownHitbox.IsPointerWithin(mouse))
                    paneNum = 2;
                if (selectorTui.SecondPaneInteractable)
                    selectorTui.CurrentPane = paneNum;
                int dataCount = GetDataCount();

                // Now, process the pointer
                string finalInfoRendered = InteractiveTuiTools.RenderFinalInfo(selectorTui);
                string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
                if (dataCount > SeparatorMaximumHeightInterior || (!selectorTui.SecondPaneInteractable && paneNum == 2 && finalInfoStrings.Length > SeparatorMaximumHeightInterior))
                {
                    leftArrowUpHitbox.ProcessPointer(mouse, out bool done);
                    if (done)
                        return;
                    leftArrowDownHitbox.ProcessPointer(mouse, out done);
                    if (done)
                        return;
                    rightArrowUpHitbox.ProcessPointer(mouse, out done);
                    if (done)
                        return;
                    rightArrowDownHitbox.ProcessPointer(mouse, out done);
                    if (done)
                        return;
                }
                UpdateSelectionBasedOnMouse(mouse);
            }

            // First, check the bindings
            var allBindings = selectorTui.Bindings;
            if (allBindings is null || allBindings.Count == 0)
                return;

            // Now, get the implemented bindings from the pressed key
            var implementedBindings = allBindings.Where((binding) =>
                (mouse is null && binding.BindingKeyName == key.Key && binding.BindingKeyModifiers == key.Modifiers) ||
                (mouse is not null && binding.BindingPointerButton == mouse.Button && binding.BindingPointerButtonPress == mouse.ButtonPress && binding.BindingPointerModifiers == mouse.Modifiers));
            var dataPrimary = selectorTui.PrimaryDataSource;
            var dataSecondary = selectorTui.SecondaryDataSource;
            object? selectedData = dataPrimary.GetElementFromIndex(selectorTui.FirstPaneCurrentSelection - 1);
            object? selectedDataSecondary = dataSecondary.GetElementFromIndex(selectorTui.SecondPaneCurrentSelection - 1);
            TPrimary? selectedDataCasted = selectedData is not null ? (TPrimary?)selectedData : default;
            TSecondary? selectedDataSecondaryCasted = selectedDataSecondary is not null ? (TSecondary?)selectedDataSecondary : default;
            object? finalData = selectorTui.CurrentPane == 2 ? selectedDataSecondary : selectedData;
            foreach (var implementedBinding in implementedBindings)
            {
                var binding = implementedBinding.BindingAction;
                if (binding is null || (finalData is null && !implementedBinding.BindingCanRunWithoutItems))
                    continue;
                binding.Invoke(selectedDataCasted, selectorTui.FirstPaneCurrentSelection - 1, selectedDataSecondaryCasted, selectorTui.SecondPaneCurrentSelection - 1);
            }
        }

        private void LaunchFinder()
        {
            if (selectorTui.CurrentPane == 2 && !selectorTui.SecondaryDataSource.Any())
                return;
            if (selectorTui.CurrentPane == 1 && !selectorTui.PrimaryDataSource.Any())
                return;
            var entriesString =
                (selectorTui.CurrentPane == 2 ?
                 selectorTui.SecondaryDataSource.Select(selectorTui.GetEntryFromItemSecondary) :
                 selectorTui.PrimaryDataSource.Select(selectorTui.GetEntryFromItem)).ToArray();
            string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"), selectorTui.Settings.InfoBoxSettings);
            if (!RegexTools.IsValidRegex(keyword))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                return;
            }
            var regex = new Regex(keyword);
            var resultEntries = entriesString
                .Select((entry, idx) => ($"{idx + 1}", entry))
                .Where((tuple) => regex.IsMatch(tuple.entry)).ToArray();
            if (resultEntries.Length > 1)
            {
                var choices = InputChoiceTools.GetInputChoices(resultEntries);
                int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"), selectorTui.Settings.InfoBoxSettings);
                if (answer < 0)
                    return;
                var resultIdx = int.Parse(resultEntries[answer].Item1);
                InteractiveTuiTools.SelectionMovement(selectorTui, resultIdx);
            }
            else if (resultEntries.Length == 1)
            {
                var resultIdx = int.Parse(resultEntries[0].Item1);
                InteractiveTuiTools.SelectionMovement(selectorTui, resultIdx);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"), selectorTui.Settings.InfoBoxSettings);
        }

        private void Exit(TextualUI ui)
        {
            selectorTui.HandleExit();
            TextualUITools.ExitTui(ui);
        }

        private int GetDataCount()
        {
            var dataPrimary = selectorTui.PrimaryDataSource;
            var dataSecondary = selectorTui.SecondaryDataSource;
            return selectorTui.CurrentPane == 2 ? dataSecondary.Length() : dataPrimary.Length();
        }

        private string GetDynamicHelpPageBody()
        {
            if (selectorTui is null)
                return "";

            var helpPageBody = new StringBuilder();

            // Write informational text prior to writing all keybindings for this TUI
            helpPageBody.AppendLine(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_HELPPAGE_BODY_INFO") + "\n");

            // Now, write all keybindings
            var uiBindings = KeybindingTools.ConvertFromTuiKeybindingsToKeybindings(selectorTui.Bindings);
            helpPageBody.Append(KeybindingTools.RenderKeybindingHelpText(uiBindings));

            // Return the final body
            return helpPageBody.ToString();
        }

        internal InteractiveSelectorTui(BaseInteractiveTui<TPrimary, TSecondary>? selectorTui)
        {
            this.selectorTui = selectorTui ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_EXCEPTION_NOSELECTOR"));

            // Base bindings
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP2"), ConsoleKey.UpArrow), (_, _, _) => GoUp()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN2"), ConsoleKey.DownArrow), (_, _, _) => GoDown()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP1"), PointerButton.WheelUp, PointerButtonPress.Scrolled), (_, _, mouse) => GoUpDeterministic(mouse)));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN1"), PointerButton.WheelDown, PointerButtonPress.Scrolled), (_, _, mouse) => GoDownDeterministic(mouse)));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOFIRST2"), ConsoleKey.Home), (_, _, _) => First()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOLAST2"), ConsoleKey.End), (_, _, _) => Last()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOPREVPAGE2"), ConsoleKey.PageUp), (_, _, _) => PreviousPage()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GONEXTPAGE2"), ConsoleKey.PageDown), (_, _, _) => NextPage()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_KEYBINDING_READMORE"), ConsoleKey.I, ConsoleModifiers.Shift), (_, _, _) => More()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_KEYBINDING_MOVEAROUND"), PointerButton.None, PointerButtonPress.Moved), (_, _, mouse) => UpdateSelectionBasedOnMouse(mouse)));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_SEARCH"), ConsoleKey.F), (_, _, _) => LaunchFinder()));
            Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_KEYBINDING_EXIT"), ConsoleKey.Escape), (ui, _, _) => Exit(ui)));

            // Informational selector TUI
            if (!selectorTui.SecondPaneInteractable)
            {
                Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_GOUPINFO"), ConsoleKey.W), (_, _, _) => InteractiveTuiTools.InfoScrollUp(selectorTui)));
                Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_GODOWNINFO"), ConsoleKey.S), (_, _, _) => InteractiveTuiTools.InfoScrollDown(selectorTui)));
            }
            else
                Keybindings.Add((new Keybinding(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_SWITCH"), ConsoleKey.Tab), (_, _, _) => InteractiveTuiTools.SwitchSides(selectorTui)));

            // Dynamic fallback
            Fallback = (_, key, mouse) => Act(key, mouse);
        }
    }
}
