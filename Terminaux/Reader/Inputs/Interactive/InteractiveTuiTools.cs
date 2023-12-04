
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.General;

namespace Terminaux.Reader.Inputs.Interactive
{
    /// <summary>
    /// Tools for the interactive TUI implementation
    /// </summary>
    public static class InteractiveTuiTools
    {

        private static int _lastFirstPanePos = 0;
        private static int _lastSecondPanePos = 0;
        private static string _finalInfoRendered = "";
        private static bool _refreshSelection = true;
        private static readonly object _interactiveTuiLock = new();

        /// <summary>
        /// Opens the interactive TUI
        /// </summary>
        /// <param name="interactiveTui">The inherited class instance of the interactive TUI</param>
        /// <exception cref="TerminauxException"></exception>
        public static void OpenInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            lock (_interactiveTuiLock)
            {
                if (interactiveTui is null)
                    throw new TerminauxException("Please provide a base Interactive TUI class and try again.");

                // First, check to see if the interactive TUI has no data source
                if (interactiveTui.PrimaryDataSource is null && interactiveTui.SecondaryDataSource is null ||
                    CountElements(interactiveTui.PrimaryDataSource) == 0 && CountElements(interactiveTui.SecondaryDataSource) == 0 && !interactiveTui.AcceptsEmptyData)
                {
                    TextWriterColor.Write("The interactive TUI {0} doesn't contain any data source. This program can't continue.", interactiveTui.GetType().Name);
                    TextWriterColor.Write();
                    TextWriterColor.Write("Press any key to exit this program...");
                    Input.DetectKeypress();
                    return;
                }

                // Now, run the application
                bool notifyCrash = false;
                string crashReason = "";
                _refreshSelection = true;
                try
                {
                    // Loop until the user requests to exit
                    while (!interactiveTui.isExiting)
                    {
                        // Check the selection
                        interactiveTui.LastOnOverflow();
                        CheckSelectionForUnderflow(interactiveTui);

                        // Draw the boxes
                        DrawInteractiveTui(interactiveTui);

                        // Draw the first pane
                        if (_refreshSelection || !interactiveTui.FastRefresh)
                            DrawInteractiveTuiItems(interactiveTui, 1);
                        else
                            DrawInteractiveTuiItemsDelta(interactiveTui, 1, _lastFirstPanePos, InteractiveTuiStatus.FirstPaneCurrentSelection);

                        // Draw the second pane
                        if (interactiveTui.SecondPaneInteractable)
                        {
                            if (_refreshSelection || !interactiveTui.FastRefresh)
                                DrawInteractiveTuiItems(interactiveTui, 2);
                            else
                                DrawInteractiveTuiItemsDelta(interactiveTui, 2, _lastSecondPanePos, InteractiveTuiStatus.SecondPaneCurrentSelection);
                        }
                        else
                        {
                            DrawInformationOnSecondPane(interactiveTui);
                        }
                        _refreshSelection = false;
                        DrawStatus(interactiveTui);

                        // Wait for user input
                        RespondToUserInput(interactiveTui);
                    }
                }
                catch (Exception ex)
                {
                    notifyCrash = true;
                    crashReason = ConsoleExtensions.FormatString("The interactive TUI, {0}, has crashed for the following reason:", interactiveTui.GetType().Name) + $" {ex.Message}";
                }

                // Clear the console to clean up
                ColorTools.LoadBack();

                // If there is a crash, notify the user about it
                if (notifyCrash)
                {
                    notifyCrash = false;
                    TextWriterColor.WriteColor(crashReason, true, ConsoleColors.Red);
                    TextWriterColor.Write();
                    TextWriterColor.Write("Press any key to exit this program...");
                    Input.DetectKeypress();
                }

                // Reset some static variables
                InteractiveTuiStatus.RedrawRequired = true;
                InteractiveTuiStatus.CurrentPane = 1;
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
                InteractiveTuiStatus.Status = "";
            }
        }

        /// <summary>
        /// Initiates the selection movement
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        /// <param name="pos">Position to move the pane selection to</param>
        public static void SelectionMovement(BaseInteractiveTui interactiveTui, int pos)
        {
            // Check the position
            var data = InteractiveTuiStatus.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            int elements = CountElements(data);
            if (pos < 1)
                pos = 1;
            if (pos > elements)
                pos = elements;

            // Now, process the movement
            int itemsPerPage = ConsoleWrapper.WindowHeight - 4;
            _lastFirstPanePos = InteractiveTuiStatus.FirstPaneCurrentSelection;
            _lastSecondPanePos = InteractiveTuiStatus.SecondPaneCurrentSelection;
            if (InteractiveTuiStatus.CurrentPane == 2)
                InteractiveTuiStatus.SecondPaneCurrentSelection = pos;
            else
                InteractiveTuiStatus.FirstPaneCurrentSelection = pos;

            // Check if we need delta or full re-population
            if (pos % itemsPerPage == 0)
                _refreshSelection = true;
            if ((pos - 1) % itemsPerPage == 0)
                _refreshSelection = true;
            if (pos == 1)
                _refreshSelection = true;
            if (pos == elements)
                _refreshSelection = true;
            if (ConsoleResizeListener.WasResized())
            {
                _refreshSelection = true;
                InteractiveTuiStatus.RedrawRequired = true;
            }
        }

        /// <summary>
        /// Forces the refresh
        /// </summary>
        public static void ForceRefreshSelection() =>
            _refreshSelection = true;

        internal static int CountElements(IEnumerable enumerable)
        {
            int dataCount = 0;
            foreach (var item in enumerable)
                // IEnumerable from System.Collections doesn't implement Count() or Length, hence the array, List<>, Dictionary<>,
                // and other collections have either Count or Length. This is an ugly hack that we should live with.
                dataCount++;
            return dataCount;
        }

        internal static object GetElementFromIndex(IEnumerable enumerable, int index)
        {
            // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
            object dataObject = null;
            int steppedItems = 0;
            foreach (var item in enumerable)
            {
                steppedItems++;
                if (steppedItems == index + 1)
                {
                    // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                    dataObject = item;
                    break;
                }
            }
            return dataObject;
        }

        private static void DrawInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");

            // Prepare the console
            ConsoleWrapper.CursorVisible = false;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Redraw the entire TUI screen
            if (InteractiveTuiStatus.RedrawRequired)
            {
                _refreshSelection = true;
                ColorTools.LoadBack(InteractiveTuiStatus.BackgroundColor);

                // Make a separator that separates the two panes to make it look like Total Commander or Midnight Commander. We need information in the upper and the
                // lower part of the console, so we need to render the entire program to look like this: (just a concept mockup)
                //
                //       |  vvvvvvvvvvvvvvvvvvvv  (SeparatorHalfConsoleWidthInterior)
                //       | v                    v (SeparatorHalfConsoleWidth)
                // H: 0  |
                // H: 1  | a--------------------+c---------------------+ < ----> (SeparatorMinimumHeight)
                // H: 2  | |b                   ||d                    |  < ----> (SeparatorMinimumHeightInterior)
                // H: 3  | |                    ||                     |  <
                // H: 4  | |                    ||                     |  <
                // H: 5  | |                    ||                     |  <
                // H: 6  | |                    ||                     |  <
                // H: 7  | |                    ||                     |  <
                // H: 8  | |                    ||                     |  < ----> (SeparatorMaximumHeightInterior)
                // H: 9  | +--------------------++---------------------+ < ----> (SeparatorMaximumHeight)
                // H: 10 |
                //       | where a is the dimension for the first pane upper left corner           (0, SeparatorMinimumHeight                                     (usually 1))
                //       |   and b is the dimension for the first pane interior upper left corner  (1, SeparatorMinimumHeightInterior                             (usually 2))
                //       |   and c is the dimension for the second pane upper left corner          (SeparatorHalfConsoleWidth, SeparatorMinimumHeight             (usually 1))
                //       |   and d is the dimension for the second pane interior upper left corner (SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior (usually 2))

                // First, the horizontal and vertical separators
                var finalForeColorFirstPane = InteractiveTuiStatus.CurrentPane == 1 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
                var finalForeColorSecondPane = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
                BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorFirstPane, InteractiveTuiStatus.PaneBackgroundColor);
                BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor);

                // Render the key bindings
                ConsoleWrapper.CursorLeft = 0;
                var finalBindings = new List<InteractiveTuiBinding>(interactiveTui.Bindings)
                {
                    new InteractiveTuiBinding("Exit", ConsoleKey.Escape, null),
                    new InteractiveTuiBinding("Keybindings", ConsoleKey.K, null),
                };
                foreach (InteractiveTuiBinding binding in finalBindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $" {binding.BindingKeyName} {binding.BindingName}  ";
                    bool canDraw = renderedBinding.Length + ConsoleWrapper.CursorLeft < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        TextWriterWhereColor.WriteWhereColorBack($" {binding.BindingKeyName} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, InteractiveTuiStatus.KeyBindingOptionColor, InteractiveTuiStatus.OptionBackgroundColor);
                        TextWriterWhereColor.WriteWhereColorBack($"{binding.BindingName}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, InteractiveTuiStatus.OptionForegroundColor, InteractiveTuiStatus.BackgroundColor);
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        TextWriterWhereColor.WriteWhereColorBack($" K ", ConsoleWrapper.WindowWidth - 3, ConsoleWrapper.WindowHeight - 1, InteractiveTuiStatus.KeyBindingOptionColor, InteractiveTuiStatus.OptionBackgroundColor);
                        break;
                    }
                }

                // Don't require redraw
                InteractiveTuiStatus.RedrawRequired = false;
            }
        }

        private static void DrawInteractiveTuiItems(BaseInteractiveTui interactiveTui, int paneNum)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            Debug.Assert(!interactiveTui.SecondPaneInteractable && paneNum == 1 || interactiveTui.SecondPaneInteractable,
                "tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Get some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

            // Render the pane right away
            int answersPerPage = SeparatorMaximumHeightInterior;
            int paneCurrentSelection = paneNum == 2 ? InteractiveTuiStatus.SecondPaneCurrentSelection : InteractiveTuiStatus.FirstPaneCurrentSelection;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            for (int i = 0; i <= answersPerPage - 1; i++)
            {
                // Populate the first pane
                string finalEntry = "";
                int finalIndex = i + startIndex;
                if (finalIndex <= dataCount - 1)
                {
                    // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                    object dataObject = null;
                    int steppedItems = 0;
                    foreach (var item in data)
                    {
                        steppedItems++;
                        if (steppedItems == startIndex + i + 1)
                        {
                            // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                            dataObject = item;
                            break;
                        }
                    }

                    // Here, we're now doing our job
                    finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                }

                // Render an entry
                var finalForeColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemForeColor : InteractiveTuiStatus.PaneItemForeColor;
                var finalBackColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemBackColor : InteractiveTuiStatus.PaneItemBackColor;
                int left = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                TextWriterWhereColor.WriteWhereColorBack(finalEntry + new string(' ', SeparatorHalfConsoleWidthInterior - finalEntry.Length - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 0 : 1)), left, top, finalForeColor, finalBackColor);
                ColorTools.SetConsoleColor(InteractiveTuiStatus.PaneItemBackColor, true);
            }

            // Render the vertical bar
            int actualLeft = paneNum == 2 ? (SeparatorHalfConsoleWidthInterior * 2) + (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 2 : 1) : SeparatorHalfConsoleWidthInterior - 1;
            ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)paneCurrentSelection / dataCount), actualLeft, 1, 2, 2, false);
        }

        private static void DrawInteractiveTuiItemsDelta(BaseInteractiveTui interactiveTui, int paneNum, int lastSelection, int currentSelection)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            Debug.Assert(!interactiveTui.SecondPaneInteractable && paneNum == 1 || interactiveTui.SecondPaneInteractable,
                "tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Get some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

            // Render the pane right away
            int answersPerPage = SeparatorMaximumHeightInterior;
            int paneCurrentSelection = paneNum == 2 ? InteractiveTuiStatus.SecondPaneCurrentSelection : InteractiveTuiStatus.FirstPaneCurrentSelection;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            int[] indexes = [lastSelection, currentSelection];
            for (int i = 0; i < indexes.Length; i++)
            {
                // Populate the first pane with changes
                int index = indexes[i] - 1 < 0 ? 0 : indexes[i] - 1;
                string finalEntry = "";
                if (index <= dataCount - 1)
                {
                    // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                    object dataObject = null;
                    int steppedItems = 0;
                    foreach (var item in data)
                    {
                        steppedItems++;
                        if (steppedItems == index + 1)
                        {
                            // We found the item that we need! Assign it to dataObject so GetEntryFromItem() can formulate a string.
                            dataObject = item;
                            break;
                        }
                    }

                    // Here, we're now doing our job
                    finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                }

                // Render an entry
                var finalForeColor = index == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemForeColor : InteractiveTuiStatus.PaneItemForeColor;
                var finalBackColor = index == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemBackColor : InteractiveTuiStatus.PaneItemBackColor;
                int left = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                int top = SeparatorMinimumHeightInterior + index - startIndex;
                if (top > 0)
                    TextWriterWhereColor.WriteWhereColorBack(finalEntry + new string(' ', SeparatorHalfConsoleWidthInterior - finalEntry.Length - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 0 : 1)), left, top, finalForeColor, finalBackColor);
                ColorTools.SetConsoleColor(InteractiveTuiStatus.PaneItemBackColor, true);
            }

            // Render the vertical bar
            int actualLeft = paneNum == 2 ? (SeparatorHalfConsoleWidthInterior * 2) + (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 2 : 1) : SeparatorHalfConsoleWidthInterior - 1;
            ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)paneCurrentSelection / dataCount), actualLeft, 1, 2, 2, false);
            _refreshSelection = false;
        }

        private static void DrawInformationOnSecondPane(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");

            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            Debug.Assert(!interactiveTui.SecondPaneInteractable,
                "tried to render information the secondary pane on an interactive TUI that allows interaction from two panes, messing the selection rendering up there.");

            // Populate some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Populate some colors
            var ForegroundColor = InteractiveTuiStatus.ForegroundColor;
            var PaneItemBackColor = InteractiveTuiStatus.PaneItemBackColor;

            // Now, do the job!
            string finalInfoRendered;
            try
            {
                // Populate data source and its count
                int paneCurrentSelection = InteractiveTuiStatus.CurrentPane == 2 ?
                                           InteractiveTuiStatus.SecondPaneCurrentSelection :
                                           InteractiveTuiStatus.FirstPaneCurrentSelection;
                var data = InteractiveTuiStatus.CurrentPane == 2 ?
                           interactiveTui.SecondaryDataSource :
                           interactiveTui.PrimaryDataSource;
                int dataCount = CountElements(data);

                // Populate selected data
                if (dataCount > 0)
                {
                    object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);
                    Debug.Assert(selectedData is not null,
                        "attempted to render info about null data");
                    finalInfoRendered = interactiveTui.GetInfoFromItem(selectedData);
                }
                else
                    finalInfoRendered = "No info.";
            }
            catch
            {
                finalInfoRendered = "Failed to get information.";
            }

            // Now, write info
            var finalForeColorSecondPane = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
            BorderColor.WriteBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor);
            _finalInfoRendered = finalInfoRendered;
            string[] finalInfoStrings = TextTools.GetWrappedSentences(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            for (int infoIndex = 0; infoIndex < finalInfoStrings.Length; infoIndex++)
            {
                // Check to see if the info is overpopulated
                if (infoIndex >= SeparatorMaximumHeightInterior - 1)
                {
                    string truncated = "Shift+I = more info";
                    TextWriterWhereColor.WriteWhereColorBack(truncated + new string(' ', SeparatorHalfConsoleWidthInterior - truncated.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex, ForegroundColor, PaneItemBackColor);
                    break;
                }

                // Now, render the info
                string finalInfo = finalInfoStrings[infoIndex];
                TextWriterWhereColor.WriteWhereColorBack(finalInfo + new string(' ', SeparatorHalfConsoleWidthInterior - finalInfo.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex, ForegroundColor, PaneItemBackColor);
            }
        }

        private static void DrawStatus(BaseInteractiveTui interactiveTui)
        {
            // Populate some necessary variables
            int paneCurrentSelection = InteractiveTuiStatus.CurrentPane == 2 ?
                                       InteractiveTuiStatus.SecondPaneCurrentSelection :
                                       InteractiveTuiStatus.FirstPaneCurrentSelection;
            var data = InteractiveTuiStatus.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);
            interactiveTui.RenderStatus(selectedData);

            // Now, write info
            TextWriterWhereColor.WriteWhereColorBack(InteractiveTuiStatus.Status.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, InteractiveTuiStatus.ForegroundColor, InteractiveTuiStatus.BackgroundColor);
            ConsoleExtensions.ClearLineToRight();
        }

        private static void RespondToUserInput(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to respond to user input on null");

            // Populate some necessary variables
            int paneCurrentSelection = InteractiveTuiStatus.CurrentPane == 2 ?
                                       InteractiveTuiStatus.SecondPaneCurrentSelection :
                                       InteractiveTuiStatus.FirstPaneCurrentSelection;
            var data = InteractiveTuiStatus.CurrentPane == 2 ?
                       interactiveTui.SecondaryDataSource :
                       interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

            // Populate selected data
            object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);

            // Wait for key
            try
            {
                ConsoleKeyInfo pressedKey;
                if (interactiveTui.RefreshInterval == 0 || interactiveTui.SecondPaneInteractable)
                    pressedKey = Input.DetectKeypress();
                else
                    pressedKey = Input.ReadKeyTimeout(true, TimeSpan.FromMilliseconds(interactiveTui.RefreshInterval));

                // Handle the key
                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (InteractiveTuiStatus.CurrentPane == 2)
                            SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection - 1);
                        else
                            SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        if (InteractiveTuiStatus.CurrentPane == 2)
                            SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection + 1);
                        else
                            SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection + 1);
                        break;
                    case ConsoleKey.PageUp:
                        SelectionMovement(interactiveTui, 1);
                        break;
                    case ConsoleKey.PageDown:
                        SelectionMovement(interactiveTui, dataCount);
                        break;
                    case ConsoleKey.I:
                        if (pressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) && !string.IsNullOrEmpty(_finalInfoRendered))
                        {
                            // User needs more information in the infobox
                            InfoBoxColor.WriteInfoBoxColorBack(_finalInfoRendered, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                            InteractiveTuiStatus.RedrawRequired = true;
                        }
                        break;
                    case ConsoleKey.K:
                        // User needs an infobox that shows all available keys
                        string section = "Available keys";
                        var bindings = interactiveTui.Bindings;
                        int maxBindingLength = bindings
                            .Max((itb) => $"[{itb.BindingKeyName}]".Length);
                        string[] bindingRepresentations = bindings
                            .Select((itb) => $"{$"[{itb.BindingKeyName}]" + new string(' ', maxBindingLength - $"[{itb.BindingKeyName}]".Length) + $" | {itb.BindingName}"}")
                            .ToArray();
                        InfoBoxColor.WriteInfoBoxColorBack(
                            $"{section}{ConsolePlatform.NewLine}" +
                            $"{new string('=', section.Length)}{ConsolePlatform.NewLine}{ConsolePlatform.NewLine}" +
                            $"{string.Join("\n", bindingRepresentations)}"
                        , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                        InteractiveTuiStatus.RedrawRequired = true;
                        break;
                    case ConsoleKey.Escape:
                        // User needs to exit
                        interactiveTui.HandleExit();
                        interactiveTui.isExiting = true;
                        break;
                    default:
                        var implementedBindings = interactiveTui.Bindings.Where((binding) => binding.BindingKeyName == pressedKey.Key);
                        foreach (var implementedBinding in implementedBindings)
                            implementedBinding.BindingAction.Invoke(selectedData, paneCurrentSelection - 1);
                        break;
                }
            }
            catch (TerminauxContinuableException)
            {
                Debug.Write("Refreshing...");
            }
        }

        private static void CheckSelectionForUnderflow(BaseInteractiveTui interactiveTui)
        {
            if (InteractiveTuiStatus.FirstPaneCurrentSelection <= 0 && CountElements(interactiveTui.PrimaryDataSource) > 0)
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection <= 0 && CountElements(interactiveTui.SecondaryDataSource) > 0)
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
        }
    }
}
