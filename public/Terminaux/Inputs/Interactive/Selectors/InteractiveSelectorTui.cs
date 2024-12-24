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

using Magico.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    internal class InteractiveSelectorTui<TPrimary, TSecondary> : TextualUI
    {
        private readonly BaseInteractiveTui<TPrimary, TSecondary> selectorTui;
        private IEnumerable<TPrimary> dataPrimary;
        private IEnumerable<TSecondary> dataSecondary;
        private int dataCount;
        private int paneCurrentSelection;

        public override string Render()
        {
            var builder = new StringBuilder();

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
                    InteractiveTuiTools.InfoScrollUp(selectorTui);
                else
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection - 1);
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
                    InteractiveTuiTools.InfoScrollDown(selectorTui);
                else
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection + 1);
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
            int answersPerPage = SeparatorMaximumHeightInterior;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            InteractiveTuiTools.SelectionMovement(selectorTui, startIndex);
        }

        private void NextPage()
        {
            int paneCurrentSelection = selectorTui.CurrentPane == 2 ? selectorTui.SecondPaneCurrentSelection : selectorTui.FirstPaneCurrentSelection;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            int answersPerPage = SeparatorMaximumHeightInterior;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * (currentPage + 1) + 1;
            InteractiveTuiTools.SelectionMovement(selectorTui, startIndex);
        }

        private void More()
        {
            string finalInfoRendered = InteractiveTuiTools.RenderFinalInfo(selectorTui);
            if (!string.IsNullOrEmpty(finalInfoRendered))
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered, selectorTui.Settings.BorderSettings, selectorTui.Settings.BoxForegroundColor, selectorTui.Settings.BoxBackgroundColor);
        }

        private void ListBindings()
        {
            var bindings = InteractiveTuiTools.GetAllBindings(selectorTui, true);
            InfoBoxModalColor.WriteInfoBoxModalColorBack(
                "Available keys",
                KeybindingTools.RenderKeybindingHelpText(bindings)
            , selectorTui.Settings.BorderSettings, selectorTui.Settings.BoxForegroundColor, selectorTui.Settings.BoxBackgroundColor);
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
            bool refresh = false;
            int oldPane = selectorTui.CurrentPane;
            if (selectorTui.SecondPaneInteractable)
            {
                if (mouse.Coordinates.x >= 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidthInterior - 1)
                {
                    if (selectorTui.CurrentPane != 1)
                    {
                        selectorTui.CurrentPane = 1;
                        refresh = true;
                    }
                }
                else if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth + 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior)
                {
                    if (selectorTui.CurrentPane != 2)
                    {
                        selectorTui.CurrentPane = 2;
                        refresh = true;
                    }
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
            if (refresh)
            {
                dataPrimary = selectorTui.PrimaryDataSource;
                dataSecondary = selectorTui.SecondaryDataSource;
                dataCount = selectorTui.CurrentPane == 2 ? dataSecondary.Length() : dataPrimary.Length();
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
            if (listIndex + 1 > dataCount)
                return;
            listIndex = listIndex > dataCount ? dataCount : listIndex;
            if (listIndex + 1 != paneCurrentSelection || selectorTui.CurrentPane != oldPane)
            {
                if (listIndex + 1 != paneCurrentSelection)
                    InteractiveTuiTools.SelectionMovement(selectorTui, listIndex + 1);
            }
        }

        private bool DetermineArrowPressed(PointerEventContext mouse)
        {
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            if (dataCount <= SeparatorMaximumHeightInterior && selectorTui.SecondPaneInteractable)
                return false;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int leftPaneArrowLeft = SeparatorHalfConsoleWidthInterior + 1;
            int rightPaneArrowLeft = SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 4 : 3);
            int paneArrowTop = 2;
            int paneArrowBottom = SeparatorMaximumHeightInterior + 1;
            return
                PointerTools.PointerWithinPoint(mouse, (leftPaneArrowLeft, paneArrowTop)) ||
                PointerTools.PointerWithinPoint(mouse, (leftPaneArrowLeft, paneArrowBottom)) ||
                PointerTools.PointerWithinPoint(mouse, (rightPaneArrowLeft, paneArrowTop)) ||
                PointerTools.PointerWithinPoint(mouse, (rightPaneArrowLeft, paneArrowBottom));
        }

        private void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
        {
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            if (dataCount <= SeparatorMaximumHeightInterior && selectorTui.SecondPaneInteractable)
                return;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int leftPaneArrowLeft = SeparatorHalfConsoleWidthInterior + 1;
            int rightPaneArrowLeft = SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 4 : 3);
            int paneArrowTop = 2;
            int paneArrowBottom = SeparatorMaximumHeightInterior + 1;
            if (mouse.Coordinates.y == paneArrowTop)
            {
                if (mouse.Coordinates.x == leftPaneArrowLeft)
                {
                    selectorTui.CurrentPane = 1;
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection - 1);
                }
                else if (mouse.Coordinates.x == rightPaneArrowLeft)
                {
                    if (selectorTui.SecondPaneInteractable)
                    {
                        selectorTui.CurrentPane = 2;
                        InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.SecondPaneCurrentSelection - 1);
                    }
                    else
                        InteractiveTuiTools.InfoScrollUp(selectorTui);
                }
            }
            else if (mouse.Coordinates.y == paneArrowBottom)
            {
                if (mouse.Coordinates.x == leftPaneArrowLeft)
                {
                    selectorTui.CurrentPane = 1;
                    InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.FirstPaneCurrentSelection + 1);
                }
                else if (mouse.Coordinates.x == rightPaneArrowLeft)
                {
                    if (selectorTui.SecondPaneInteractable)
                    {
                        selectorTui.CurrentPane = 2;
                        InteractiveTuiTools.SelectionMovement(selectorTui, selectorTui.SecondPaneCurrentSelection + 1);
                    }
                    else
                        InteractiveTuiTools.InfoScrollDown(selectorTui);
                }
            }
        }

        private void Act(ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (mouse is not null)
            {
                if (DetermineArrowPressed(mouse))
                {
                    UpdatePositionBasedOnArrowPress(mouse);
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
            TPrimary? selectedData = (TPrimary?)dataPrimary.GetElementFromIndex(selectorTui.FirstPaneCurrentSelection - 1);
            TSecondary? selectedDataSecondary = (TSecondary?)dataSecondary.GetElementFromIndex(selectorTui.SecondPaneCurrentSelection - 1);
            object? finalData = selectorTui.CurrentPane == 2 ? selectedDataSecondary : selectedData;
            foreach (var implementedBinding in implementedBindings)
            {
                var binding = implementedBinding.BindingAction;
                if (binding is null || (finalData is null && !implementedBinding.BindingCanRunWithoutItems))
                    continue;
                binding.Invoke(selectedData, selectorTui.FirstPaneCurrentSelection - 1, selectedDataSecondary, selectorTui.SecondPaneCurrentSelection - 1);
            }
        }

        private void LaunchFinder(TextualUI ui)
        {
            if (selectorTui.CurrentPane == 2 && !dataSecondary.Any())
                return;
            if (selectorTui.CurrentPane == 1 && !dataPrimary.Any())
                return;
            var entriesString =
                (selectorTui.CurrentPane == 2 ?
                 dataSecondary.Select(selectorTui.GetEntryFromItemSecondary) :
                 dataPrimary.Select(selectorTui.GetEntryFromItem)).ToArray();
            string keyword = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write a search term (supports regular expressions)", selectorTui.Settings.BorderSettings, selectorTui.Settings.BoxForegroundColor, selectorTui.Settings.BoxBackgroundColor);
            if (!RegexTools.IsValidRegex(keyword))
            {
                InfoBoxModalColor.WriteInfoBoxModal("Your query is not a valid regular expression.");
                ui.RequireRefresh();
                return;
            }
            var regex = new Regex(keyword);
            var resultEntries = entriesString
                .Select((entry, idx) => ($"{idx + 1}", entry))
                .Where((tuple) => regex.IsMatch(tuple.entry)).ToArray();
            if (resultEntries.Length > 0)
            {
                var choices = InputChoiceTools.GetInputChoices(resultEntries);
                int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Select one of the entries:", selectorTui.Settings.BorderSettings, selectorTui.Settings.BoxForegroundColor, selectorTui.Settings.BoxBackgroundColor);
                if (answer < 0)
                    return;
                var resultIdx = int.Parse(resultEntries[answer].Item1);
                InteractiveTuiTools.SelectionMovement(selectorTui, resultIdx);
            }
            else
                InfoBoxModalColor.WriteInfoBoxModalColorBack("No item found.", selectorTui.Settings.BorderSettings, selectorTui.Settings.BoxForegroundColor, selectorTui.Settings.BoxBackgroundColor);
        }

        private void Exit(TextualUI ui)
        {
            selectorTui.HandleExit();
            TextualUITools.ExitTui(ui);
        }

        internal InteractiveSelectorTui(BaseInteractiveTui<TPrimary, TSecondary>? selectorTui)
        {
            this.selectorTui = selectorTui ??
                throw new TerminauxException("Selector is not specified");
            dataPrimary = selectorTui.PrimaryDataSource;
            dataSecondary = selectorTui.SecondaryDataSource;

            // Base bindings
            Keybindings.Add((new Keybinding("Go one element up", ConsoleKey.UpArrow), (_, _, _) => GoUp()));
            Keybindings.Add((new Keybinding("Go one element down", ConsoleKey.DownArrow), (_, _, _) => GoDown()));
            Keybindings.Add((new Keybinding("Go one element up", PointerButton.WheelUp, PointerButtonPress.Scrolled), (_, _, mouse) => GoUpDeterministic(mouse)));
            Keybindings.Add((new Keybinding("Go one element down", PointerButton.WheelDown, PointerButtonPress.Scrolled), (_, _, mouse) => GoDownDeterministic(mouse)));
            Keybindings.Add((new Keybinding("Go to the first element", ConsoleKey.Home), (_, _, _) => First()));
            Keybindings.Add((new Keybinding("Go to the last element", ConsoleKey.End), (_, _, _) => Last()));
            Keybindings.Add((new Keybinding("Go to the previous page", ConsoleKey.PageUp), (_, _, _) => PreviousPage()));
            Keybindings.Add((new Keybinding("Go to the next page", ConsoleKey.PageDown), (_, _, _) => NextPage()));
            Keybindings.Add((new Keybinding("Read more...", ConsoleKey.I, ConsoleModifiers.Shift), (_, _, _) => More()));
            Keybindings.Add((new Keybinding("List keybindings", ConsoleKey.K), (_, _, _) => ListBindings()));
            Keybindings.Add((new Keybinding("Move around", PointerButton.None, PointerButtonPress.Moved), (_, _, mouse) => UpdateSelectionBasedOnMouse(mouse)));
            Keybindings.Add((new Keybinding("Search for an element", ConsoleKey.F), (ui, _, _) => LaunchFinder(ui)));
            Keybindings.Add((new Keybinding("Exits the interactive TUI", ConsoleKey.Escape), (ui, _, _) => Exit(ui)));

            // Informational selector TUI
            if (!selectorTui.SecondPaneInteractable)
            {
                Keybindings.Add((new Keybinding("Go one line up (informational)", ConsoleKey.W), (_, _, _) => InteractiveTuiTools.InfoScrollUp(selectorTui)));
                Keybindings.Add((new Keybinding("Go one line down (informational)", ConsoleKey.S), (_, _, _) => InteractiveTuiTools.InfoScrollDown(selectorTui)));
            }
            else
                Keybindings.Add((new Keybinding("Switch", ConsoleKey.Tab), (_, _, _) => InteractiveTuiTools.SwitchSides(selectorTui)));

            // Dynamic fallback
            Fallback = (_, key, mouse) => Act(key, mouse);
        }
    }
}
