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

using Magico.Enumeration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters;
using Textify.General;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Tools for the interactive TUI implementation
    /// </summary>
    public static class InteractiveTuiTools
    {
        private static readonly object _interactiveTuiLock = new();

        /// <summary>
        /// Opens the interactive TUI
        /// </summary>
        /// <param name="interactiveTui">The inherited class instance of the interactive TUI</param>
        /// <exception cref="TerminauxException"></exception>
        public static void OpenInteractiveTui<T>(BaseInteractiveTui<T> interactiveTui)
        {
            lock (_interactiveTuiLock)
            {
                if (!VerifyInteractiveTui(interactiveTui))
                    return;

                // Make the screen
                var screen = new Screen();
                ScreenTools.SetCurrent(screen);
                interactiveTui.screen = screen;

                // Now, run the application
                bool notifyCrash = false;
                string crashReason = "";
                try
                {
                    // Loop until the user requests to exit
                    while (!interactiveTui.isExiting)
                    {
                        // Check the selection
                        LastOnOverflow(interactiveTui);
                        FirstOnUnderflow(interactiveTui);

                        // Draw the boxes
                        DrawInteractiveTui(interactiveTui);

                        // Draw the first pane
                        DrawInteractiveTuiItems(interactiveTui, 1);

                        // Draw the second pane
                        if (interactiveTui.SecondPaneInteractable)
                            DrawInteractiveTuiItems(interactiveTui, 2);
                        else
                            DrawInformationOnSecondPane(interactiveTui);
                        DrawStatus(interactiveTui);

                        // Wait for user input
                        ScreenTools.Render(screen);
                        RespondToUserInput(interactiveTui);
                    }
                }
                catch (Exception ex)
                {
                    notifyCrash = true;
                    crashReason = TextTools.FormatString("The interactive TUI, {0}, has crashed for the following reason:", interactiveTui.GetType().Name) + $" {ex.Message}";
                }
                ScreenTools.UnsetCurrent(screen);

                // Clear the console to clean up
                ColorTools.LoadBack();

                // If there is a crash, notify the user about it
                if (notifyCrash)
                {
                    notifyCrash = false;
                    InfoBoxColor.WriteInfoBoxColorBack(crashReason + "\n" + "Press any key to continue...", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                }

                // Reset some static variables
                InteractiveTuiStatus.CurrentPane = 1;
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
                InteractiveTuiStatus.CurrentInfoLine = 0;
                InteractiveTuiStatus.Status = "";
            }
        }

        /// <summary>
        /// Initiates the selection movement
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        /// <param name="pos">Position to move the pane selection to</param>
        public static void SelectionMovement<T>(BaseInteractiveTui<T> interactiveTui, int pos)
        {
            // Check the position
            var data = interactiveTui.DataSource;
            int elements = data.Length();
            if (pos < 1)
                pos = 1;
            if (pos > elements)
                pos = elements;

            // Now, process the movement
            InteractiveTuiStatus.CurrentInfoLine = 0;
            if (InteractiveTuiStatus.CurrentPane == 2)
                InteractiveTuiStatus.SecondPaneCurrentSelection = pos;
            else
                InteractiveTuiStatus.FirstPaneCurrentSelection = pos;
        }

        /// <summary>
        /// Switches between two panes
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        public static void SwitchSides<T>(BaseInteractiveTui<T> interactiveTui)
        {
            if (!interactiveTui.SecondPaneInteractable)
                return;
            InteractiveTuiStatus.CurrentPane++;
            if (InteractiveTuiStatus.CurrentPane > 2)
                InteractiveTuiStatus.CurrentPane = 1;
        }

        /// <summary>
        /// Goes down to the last element upon overflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public static void LastOnOverflow<T>(BaseInteractiveTui<T> interactiveTui)
        {
            int primaryCount = interactiveTui.PrimaryDataSource.Length();
            int secondaryCount = interactiveTui.SecondaryDataSource.Length();
            if (InteractiveTuiStatus.FirstPaneCurrentSelection > primaryCount)
                InteractiveTuiStatus.FirstPaneCurrentSelection = primaryCount;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection > secondaryCount)
                InteractiveTuiStatus.SecondPaneCurrentSelection = secondaryCount;
        }

        /// <summary>
        /// Goes up to the first element upon underflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public static void FirstOnUnderflow<T>(BaseInteractiveTui<T> interactiveTui)
        {
            int primaryCount = interactiveTui.PrimaryDataSource.Length();
            int secondaryCount = interactiveTui.SecondaryDataSource.Length();
            if (InteractiveTuiStatus.FirstPaneCurrentSelection <= 0 && primaryCount > 0)
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection <= 0 && secondaryCount > 0)
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
        }

        private static void DrawInteractiveTui<T>(BaseInteractiveTui<T> interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            if (interactiveTui is null)
                throw new TerminauxInternalException("Attempted to render TUI items on null");

            // Remove the old screen part
            string partName = $"Interactive TUI - Main - {interactiveTui.GetType().Name}";
            if (interactiveTui.trackedParts.TryGetValue(partName, out var oldPart))
            {
                interactiveTui.screen?.RemoveBufferedPart(oldPart.Id);
                interactiveTui.trackedParts.Remove(partName);
            }

            // Make a screen part
            var part = new ScreenPart();

            // Prepare the console
            ConsoleWrapper.CursorVisible = false;

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
            part.AddDynamicText(new(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(BorderColor.RenderBorder(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior, finalForeColorFirstPane, InteractiveTuiStatus.PaneBackgroundColor));
                builder.Append(BorderColor.RenderBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor));
                return builder.ToString();
            }));

            // Populate appropriate bindings, depending on the SecondPaneInteractable value, and render them
            var finalBindings = GetAllBindings(interactiveTui);
            var builtIns = finalBindings.Where((itb) => !interactiveTui.Bindings.Contains(itb)).ToArray();
            part.AddDynamicText(() => KeybindingsWriter.RenderKeybindings(interactiveTui.Bindings, builtIns, InteractiveTuiStatus.KeyBindingBuiltinColor, InteractiveTuiStatus.KeyBindingBuiltinForegroundColor, InteractiveTuiStatus.KeyBindingBuiltinBackgroundColor, InteractiveTuiStatus.KeyBindingOptionColor, InteractiveTuiStatus.OptionForegroundColor, InteractiveTuiStatus.OptionBackgroundColor, InteractiveTuiStatus.BackgroundColor, 0, ConsoleWrapper.WindowHeight - 1, 0));

            // We've added the necessary buffer. Now, add that to the buffered part list
            interactiveTui.screen?.AddBufferedPart(partName, part);
            interactiveTui.trackedParts.Add(partName, part);
        }

        private static void DrawInteractiveTuiItems<T>(BaseInteractiveTui<T> interactiveTui, int paneNum)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            if (interactiveTui is null)
                throw new TerminauxInternalException("Attempted to render TUI items on null");
            if (interactiveTui.screen is null)
                throw new TerminauxInternalException("Attempted to render TUI items on no screen");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            if (!interactiveTui.SecondPaneInteractable && paneNum > 1)
                throw new TerminauxInternalException("Tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Remove the old screen part
            string partName = $"Interactive TUI - Items [{paneNum}] - {interactiveTui.GetType().Name}";
            if (interactiveTui.trackedParts.TryGetValue(partName, out var oldPart))
            {
                interactiveTui.screen?.RemoveBufferedPart(oldPart.Id);
                interactiveTui.trackedParts.Remove(partName);
            }

            // Make a screen part
            var part = new ScreenPart();

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data instances are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = data.Length();

            // Render the pane right away
            part.AddDynamicText(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                int answersPerPage = SeparatorMaximumHeightInterior;
                int paneCurrentSelection = paneNum == 2 ? InteractiveTuiStatus.SecondPaneCurrentSelection : InteractiveTuiStatus.FirstPaneCurrentSelection;
                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                var builder = new StringBuilder();
                for (int i = 0; i <= answersPerPage - 1; i++)
                {
                    // Populate the first pane
                    string finalEntry = "";
                    int finalIndex = i + startIndex;
                    T? dataObject = default;
                    if (finalIndex <= dataCount - 1)
                    {
                        dataObject = (T?)data.GetElementFromIndex(startIndex + i);
                        if (dataObject is null)
                            continue;

                        // Render an entry
                        var finalForeColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemForeColor : InteractiveTuiStatus.PaneItemForeColor;
                        var finalBackColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemBackColor : InteractiveTuiStatus.PaneItemBackColor;
                        int leftPos = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                        int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                        finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                        int width = ConsoleChar.EstimateCellWidth(finalEntry);
                        string text =
                            $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, top + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(finalForeColor, false, true)}" +
                            $"{ColorTools.RenderSetConsoleColor(finalBackColor, true)}" +
                            finalEntry +
                            new string(' ', SeparatorHalfConsoleWidthInterior - width - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? -1 : 0)) +
                            $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.PaneItemBackColor, true)}";
                        builder.Append(text);
                    }
                    else
                        break;
                }

                // Render the vertical bar
                var finalForeColorFirstPane = InteractiveTuiStatus.CurrentPane == 1 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
                var finalForeColorSecondPane = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
                int left = paneNum == 2 ? SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 3 : 2) : SeparatorHalfConsoleWidthInterior;
                if (dataCount > SeparatorMaximumHeightInterior)
                {
                    builder.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left + 1, 2, paneNum == 2 ? finalForeColorSecondPane : finalForeColorFirstPane, InteractiveTuiStatus.PaneBackgroundColor));
                    builder.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left + 1, SeparatorMaximumHeightInterior + 1, paneNum == 2 ? finalForeColorSecondPane : finalForeColorFirstPane, InteractiveTuiStatus.PaneBackgroundColor));
                    builder.Append(SliderVerticalColor.RenderVerticalSlider(paneCurrentSelection, dataCount, left, 2, ConsoleWrapper.WindowHeight - 6, paneNum == 2 ? finalForeColorSecondPane : finalForeColorFirstPane, InteractiveTuiStatus.PaneBackgroundColor, false));
                }
                return builder.ToString();
            });

            interactiveTui.screen?.AddBufferedPart(partName, part);
            interactiveTui.trackedParts.Add(partName, part);
        }

        private static void DrawInformationOnSecondPane<T>(BaseInteractiveTui<T> interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            if (interactiveTui is null)
                throw new TerminauxInternalException("Attempted to draw info on null");
            if (interactiveTui.screen is null)
                throw new TerminauxInternalException("Attempted to draw info on no screen");

            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            if (interactiveTui.SecondPaneInteractable)
                throw new TerminauxInternalException("Tried to render information the secondary pane on an interactive TUI that allows interaction from two panes, messing the selection rendering up there.");

            // Remove the old screen part
            string partName = $"Interactive TUI - Info (2nd pane) - {interactiveTui.GetType().Name}";
            if (interactiveTui.trackedParts.TryGetValue(partName, out var oldPart))
            {
                interactiveTui.screen?.RemoveBufferedPart(oldPart.Id);
                interactiveTui.trackedParts.Remove(partName);
            }

            // Make a screen part
            var part = new ScreenPart();

            // Populate some colors
            var ForegroundColor = InteractiveTuiStatus.ForegroundColor;
            var PaneItemBackColor = InteractiveTuiStatus.PaneItemBackColor;

            // Now, write info
            string finalInfoRendered = RenderFinalInfo(interactiveTui);
            var finalForeColorSecondPane = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
            part.AddDynamicText(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(BorderColor.RenderBorder(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor));

                // Split the information string
                string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
                int top = 0;
                for (int infoIndex = InteractiveTuiStatus.CurrentInfoLine; infoIndex < finalInfoStrings.Length; infoIndex++, top++)
                {
                    // Check to see if the info is overpopulated
                    if (top >= SeparatorMaximumHeightInterior)
                    {
                        builder.Append(TextWriterWhereColor.RenderWhereColorBack("[W|S|SHIFT + I]", ConsoleWrapper.WindowWidth - "[W|S|SHIFT + I]".Length - 2, SeparatorMaximumHeightInterior + 2, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor));
                        break;
                    }

                    // Now, render the info
                    string finalInfo = finalInfoStrings[infoIndex];
                    builder.Append(TextWriterWhereColor.RenderWhereColorBack(finalInfo, SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + top, ForegroundColor, PaneItemBackColor));
                }

                // Render the vertical bar
                int left = SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 3 : 2);
                if (finalInfoStrings.Length > SeparatorMaximumHeightInterior)
                {
                    builder.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left + 1, 2, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor));
                    builder.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left + 1, SeparatorMaximumHeightInterior + 1, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor));
                    builder.Append(SliderVerticalColor.RenderVerticalSlider((int)((double)InteractiveTuiStatus.CurrentInfoLine / (finalInfoStrings.Length - SeparatorMaximumHeightInterior) * finalInfoStrings.Length), finalInfoStrings.Length, left, 2, ConsoleWrapper.WindowHeight - 6, finalForeColorSecondPane, InteractiveTuiStatus.PaneBackgroundColor, false));
                }
                return builder.ToString();
            });

            interactiveTui.screen?.AddBufferedPart(partName, part);
            interactiveTui.trackedParts.Add(partName, part);
        }

        private static void DrawStatus<T>(BaseInteractiveTui<T> interactiveTui)
        {
            if (interactiveTui is null)
                throw new TerminauxInternalException("Attempted to draw status on null");
            if (interactiveTui.screen is null)
                throw new TerminauxInternalException("Attempted to draw status on no screen");

            // Remove the old screen part
            string partName = $"Interactive TUI - Status - {interactiveTui.GetType().Name}";
            if (interactiveTui.trackedParts.TryGetValue(partName, out var oldPart))
            {
                interactiveTui.screen?.RemoveBufferedPart(oldPart.Id);
                interactiveTui.trackedParts.Remove(partName);
            }

            // Make a screen part
            var part = new ScreenPart();

            // Populate some necessary variables
            int paneCurrentSelection = InteractiveTuiStatus.CurrentSelection;
            var data = interactiveTui.DataSource;
            T? selectedData = (T?)data.GetElementFromIndex(paneCurrentSelection - 1);
            InteractiveTuiStatus.Status = selectedData is not null ? interactiveTui.GetStatusFromItem(selectedData) : "No status.";

            // Now, write info
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(TextWriterWhereColor.RenderWhereColorBack(InteractiveTuiStatus.Status.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0, InteractiveTuiStatus.ForegroundColor, InteractiveTuiStatus.BackgroundColor));
                builder.Append(ConsoleClearing.GetClearLineToRightSequence());
                return builder.ToString();
            });

            interactiveTui.screen?.AddBufferedPart(partName, part);
            interactiveTui.trackedParts.Add(partName, part);
        }

        private static string RenderFinalInfo<T>(BaseInteractiveTui<T> interactiveTui)
        {
            string finalInfoRendered;
            try
            {
                // Populate data source and its count
                int paneCurrentSelection = InteractiveTuiStatus.CurrentSelection;
                var data = interactiveTui.DataSource;
                int dataCount = data.Length();

                // Populate selected data
                if (dataCount > 0)
                {
                    T selectedData = (T)(data.GetElementFromIndex(paneCurrentSelection - 1) ??
                        throw new TerminauxInternalException("Attempted to render info about null data"));
                    finalInfoRendered = interactiveTui.GetInfoFromItem(selectedData);
                }
                else
                {
                    finalInfoRendered = "No info.";
                }
            }
            catch
            {
                finalInfoRendered = "Failed to get information.";
            }
            return finalInfoRendered;
        }

        private static void RespondToUserInput<T>(BaseInteractiveTui<T> interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            if (interactiveTui is null)
                throw new TerminauxInternalException("Attempted to respond to user input on null");

            // Populate some necessary variables
            int paneCurrentSelection = InteractiveTuiStatus.CurrentSelection;
            var data = interactiveTui.DataSource;
            int dataCount = data.Length();

            // Populate selected data
            object? selectedData = data.GetElementFromIndex(paneCurrentSelection - 1);

            // Wait for key
            bool loopBail = false;
            bool timed = interactiveTui.RefreshInterval > 0 && !interactiveTui.SecondPaneInteractable;
            Stopwatch sw = new();
            if (timed)
                sw.Start();
            while (!loopBail)
            {
                SpinWait.SpinUntil(() => Input.InputAvailable || (timed && sw.ElapsedMilliseconds >= interactiveTui.RefreshInterval));
                bool timedOut = timed && sw.ElapsedMilliseconds >= interactiveTui.RefreshInterval;
                if (timedOut)
                {
                    sw.Restart();
                    loopBail = true;
                    continue;
                }
                if (Input.MouseInputAvailable)
                {
                    void UpdateSelectionBasedOnMouse(PointerEventContext mouse)
                    {
                        // First, check to see if the cursor has moved to the other side or not
                        int SeparatorMinimumHeight = 1;
                        int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                        if (mouse.Coordinates.y < SeparatorMinimumHeight || mouse.Coordinates.y > SeparatorMaximumHeightInterior + 2)
                            return;
                        int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                        int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                        bool refresh = false;
                        int oldPane = InteractiveTuiStatus.CurrentPane;
                        if (interactiveTui.SecondPaneInteractable)
                        {
                            if (mouse.Coordinates.x >= 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidthInterior - 1)
                            {
                                if (InteractiveTuiStatus.CurrentPane != 1)
                                {
                                    InteractiveTuiStatus.CurrentPane = 1;
                                    refresh = true;
                                }
                            }
                            else if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth + 1 && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior)
                            {
                                if (InteractiveTuiStatus.CurrentPane != 2)
                                {
                                    InteractiveTuiStatus.CurrentPane = 2;
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
                            data =
                                InteractiveTuiStatus.CurrentPane == 2 ?
                                interactiveTui.SecondaryDataSource :
                                interactiveTui.PrimaryDataSource;
                            dataCount = data.Length();
                        }

                        // Now, update the selection relative to the mouse pointer location
                        int SeparatorMinimumHeightInterior = 2;
                        int answersPerPage = SeparatorMaximumHeightInterior;
                        paneCurrentSelection = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.SecondPaneCurrentSelection : InteractiveTuiStatus.FirstPaneCurrentSelection;
                        int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                        int startIndex = answersPerPage * currentPage;
                        int endIndex = answersPerPage * (currentPage + 1) - 1;
                        if (mouse.Coordinates.y < SeparatorMinimumHeightInterior || mouse.Coordinates.y >= SeparatorMaximumHeightInterior + 2)
                            return;
                        int listIndex = mouse.Coordinates.y - SeparatorMinimumHeightInterior;
                        listIndex = startIndex + listIndex;
                        if (listIndex + 1 > dataCount)
                            return;
                        listIndex = listIndex > dataCount ? dataCount : listIndex;
                        if (listIndex + 1 != paneCurrentSelection || InteractiveTuiStatus.CurrentPane != oldPane)
                        {
                            if (listIndex + 1 != paneCurrentSelection)
                                SelectionMovement(interactiveTui, listIndex + 1);
                            loopBail = true;
                        }
                    }

                    bool DetermineArrowPressed(PointerEventContext mouse)
                    {
                        int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                        if (dataCount <= SeparatorMaximumHeightInterior && interactiveTui.SecondPaneInteractable)
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

                    void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
                    {
                        int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                        if (dataCount <= SeparatorMaximumHeightInterior && interactiveTui.SecondPaneInteractable)
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
                                InteractiveTuiStatus.CurrentPane = 1;
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection - 1);
                            }
                            else if (mouse.Coordinates.x == rightPaneArrowLeft)
                            {
                                if (interactiveTui.SecondPaneInteractable)
                                {
                                    InteractiveTuiStatus.CurrentPane = 2;
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection - 1);
                                }
                                else
                                    InfoScrollUp();
                            }
                        }
                        else if (mouse.Coordinates.y == paneArrowBottom)
                        {
                            if (mouse.Coordinates.x == leftPaneArrowLeft)
                            {
                                InteractiveTuiStatus.CurrentPane = 1;
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection + 1);
                            }
                            else if (mouse.Coordinates.x == rightPaneArrowLeft)
                            {
                                if (interactiveTui.SecondPaneInteractable)
                                {
                                    InteractiveTuiStatus.CurrentPane = 2;
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection + 1);
                                }
                                else
                                    InfoScrollDown(interactiveTui);
                            }
                        }
                    }

                    // Mouse input received.
                    var mouse = Input.ReadPointer();
                    bool processed = false;
                    switch (mouse.Button)
                    {
                        case PointerButton.WheelUp:
                            processed = true;
                            loopBail = true;
                            if (interactiveTui.SecondPaneInteractable)
                            {
                                if (InteractiveTuiStatus.CurrentPane == 2)
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection - 1);
                                else
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection - 1);
                            }
                            else
                            {
                                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                                if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior + 1)
                                    processed = InfoScrollUp();
                                else
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection - 1);
                            }
                            break;
                        case PointerButton.WheelDown:
                            processed = true;
                            loopBail = true;
                            if (interactiveTui.SecondPaneInteractable)
                            {
                                if (InteractiveTuiStatus.CurrentPane == 2)
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection + 1);
                                else
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection + 1);
                            }
                            else
                            {
                                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                                if (mouse.Coordinates.x >= SeparatorHalfConsoleWidth && mouse.Coordinates.x <= SeparatorHalfConsoleWidth + SeparatorHalfConsoleWidthInterior + 1)
                                    InfoScrollDown(interactiveTui);
                                else
                                    SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection + 1);
                            }
                            break;
                        case PointerButton.Left:
                            if (mouse.ButtonPress != PointerButtonPress.Released)
                                break;
                            processed = true;

                            // Check to see if the user pressed the up/down arrow button
                            if (DetermineArrowPressed(mouse))
                            {
                                UpdatePositionBasedOnArrowPress(mouse);
                                loopBail = true;
                            }
                            else
                            {
                                UpdateSelectionBasedOnMouse(mouse);

                                // First, check the bindings
                                var allBindings = interactiveTui.Bindings;
                                if (allBindings is null || allBindings.Length == 0)
                                    break;

                                // Now, get the implemented bindings from the pressed key
                                var implementedBindings = allBindings.Where((binding) =>
                                    binding.BindingKeyName == ConsoleKey.Enter);
                                if (implementedBindings.Any())
                                    loopBail = true;
                                foreach (var implementedBinding in implementedBindings)
                                {
                                    var binding = implementedBinding.BindingAction;
                                    if (binding is null || selectedData is null)
                                        continue;
                                    binding.Invoke(selectedData, paneCurrentSelection - 1);
                                }
                            }
                            break;
                        case PointerButton.None:
                            if (mouse.ButtonPress != PointerButtonPress.Moved)
                                break;
                            processed = true;
                            UpdateSelectionBasedOnMouse(mouse);
                            break;
                    }
                    if (!processed && !DetermineArrowPressed(mouse))
                    {
                        UpdateSelectionBasedOnMouse(mouse);

                        // First, check the bindings
                        var allBindings = interactiveTui.Bindings;
                        if (allBindings is null || allBindings.Length == 0)
                            break;

                        // Now, get the implemented bindings from the pressed key
                        var implementedBindings = allBindings.Where((binding) =>
                            binding.BindingPointerButton == mouse.Button && binding.BindingPointerButtonPress == mouse.ButtonPress && binding.BindingPointerModifiers == mouse.Modifiers);
                        foreach (var implementedBinding in implementedBindings)
                        {
                            var binding = implementedBinding.BindingAction;
                            if (binding is null || selectedData is null)
                                continue;
                            loopBail = true;
                            binding.Invoke(selectedData, paneCurrentSelection - 1);
                        }
                    }
                }
                else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                {
                    var key = Input.ReadKey();
                    bool processed = false;
                    loopBail = true;
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            processed = true;
                            if (InteractiveTuiStatus.CurrentPane == 2)
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection - 1);
                            else
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            processed = true;
                            if (InteractiveTuiStatus.CurrentPane == 2)
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.SecondPaneCurrentSelection + 1);
                            else
                                SelectionMovement(interactiveTui, InteractiveTuiStatus.FirstPaneCurrentSelection + 1);
                            break;
                        case ConsoleKey.Home:
                            processed = true;
                            SelectionMovement(interactiveTui, 1);
                            break;
                        case ConsoleKey.End:
                            processed = true;
                            SelectionMovement(interactiveTui, dataCount);
                            break;
                        case ConsoleKey.PageUp:
                            {
                                processed = true;
                                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                                int answersPerPage = SeparatorMaximumHeightInterior;
                                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                                int startIndex = answersPerPage * currentPage;
                                SelectionMovement(interactiveTui, startIndex);
                            }
                            break;
                        case ConsoleKey.PageDown:
                            {
                                processed = true;
                                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                                int answersPerPage = SeparatorMaximumHeightInterior;
                                int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                                int startIndex = answersPerPage * (currentPage + 1) + 1;
                                SelectionMovement(interactiveTui, startIndex);
                            }
                            break;
                        case ConsoleKey.W:
                            processed = InfoScrollUp();
                            break;
                        case ConsoleKey.S:
                            processed = InfoScrollDown(interactiveTui);
                            break;
                        case ConsoleKey.I:
                            {
                                string finalInfoRendered = RenderFinalInfo(interactiveTui);
                                if (key.Modifiers.HasFlag(ConsoleModifiers.Shift) && !string.IsNullOrEmpty(finalInfoRendered))
                                {
                                    // User needs more information in the infobox
                                    processed = true;
                                    InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                                }
                            }
                            break;
                        case ConsoleKey.K:
                            processed = true;
                            var bindings = GetAllBindings(interactiveTui, true);
                            InfoBoxColor.WriteInfoBoxColorBack(
                                "Available keys",
                                KeybindingsWriter.RenderKeybindingHelpText(bindings)
                            , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                            break;
                        case ConsoleKey.F:
                            // Search function
                            if (!data.Any())
                                break;
                            processed = true;
                            var entriesString = data.Select(interactiveTui.GetEntryFromItem).ToArray();
                            string keyword = InfoBoxInputColor.WriteInfoBoxInputColorBack("Write a search term (case insensitive)", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor).ToLower();
                            var resultEntries = entriesString.Select((entry, idx) => ($"{idx + 1}", entry)).Where((tuple) => tuple.entry.ToLower().Contains(keyword)).ToArray();
                            if (resultEntries.Length > 0)
                            {
                                var choices = InputChoiceTools.GetInputChoices(resultEntries);
                                int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Select one of the entries:");
                                if (answer < 0)
                                    break;
                                var resultIdx = int.Parse(resultEntries[answer].Item1);
                                SelectionMovement(interactiveTui, resultIdx);
                            }
                            else
                                InfoBoxColor.WriteInfoBoxColorBack("No item found.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                            break;
                        case ConsoleKey.Escape:
                            // User needs to exit
                            interactiveTui.HandleExit();
                            interactiveTui.isExiting = true;
                            processed = true;
                            break;
                        case ConsoleKey.Tab:
                            // User needs to switch sides
                            SwitchSides(interactiveTui);
                            processed = true;
                            break;
                    }
                    if (!processed)
                    {
                        // First, check the bindings
                        var allBindings = interactiveTui.Bindings;
                        if (allBindings is null || allBindings.Length == 0)
                            break;

                        // Now, get the implemented bindings from the pressed key
                        var implementedBindings = allBindings.Where((binding) =>
                            binding.BindingKeyName == key.Key && binding.BindingKeyModifiers == key.Modifiers);
                        foreach (var implementedBinding in implementedBindings)
                        {
                            var binding = implementedBinding.BindingAction;
                            if (binding is null || (selectedData is null && !implementedBinding.BindingCanRunWithoutItems))
                                continue;
                            binding.Invoke(selectedData, paneCurrentSelection - 1);
                        }
                    }
                }
            }
        }

        private static string GetBindingKeyShortcut(InteractiveTuiBinding bind, bool mark = true)
        {
            if (bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingKeyModifiers != 0 ? $"{bind.BindingKeyModifiers} + " : "")}{bind.BindingKeyName}{markEnd}";
        }

        private static string GetBindingMouseShortcut(InteractiveTuiBinding bind, bool mark = true)
        {
            if (!bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingPointerModifiers != 0 ? $"{bind.BindingPointerModifiers} + " : "")}{bind.BindingPointerButton}{(bind.BindingPointerButtonPress != 0 ? $" {bind.BindingPointerButtonPress}" : "")}{markEnd}";
        }

        private static InteractiveTuiBinding[] GetAllBindings<T>(BaseInteractiveTui<T> interactiveTui, bool full = false)
        {
            // Populate appropriate bindings, depending on the SecondPaneInteractable value
            List<InteractiveTuiBinding> finalBindings =
            [
                new InteractiveTuiBinding("Keybindings", ConsoleKey.K, null),
                new InteractiveTuiBinding("Exit", ConsoleKey.Escape, null),
            ];

            // Populate switch as needed
            if (interactiveTui.SecondPaneInteractable)
                finalBindings.Add(
                    new InteractiveTuiBinding("Switch", ConsoleKey.Tab, null)
                );

            // Now, check to see if we need to add additional base bindings
            if (full)
            {
                finalBindings.AddRange(
                [
                    new InteractiveTuiBinding("Go one element up", ConsoleKey.UpArrow, null),
                    new InteractiveTuiBinding("Go one element down", ConsoleKey.DownArrow, null),
                    new InteractiveTuiBinding("Go to the first element", ConsoleKey.Home, null),
                    new InteractiveTuiBinding("Go to the last element", ConsoleKey.End, null),
                    new InteractiveTuiBinding("Go to the previous page", ConsoleKey.PageUp, null),
                    new InteractiveTuiBinding("Go to the next page", ConsoleKey.PageDown, null),
                    new InteractiveTuiBinding("Search for an element", ConsoleKey.F, null),
                    new InteractiveTuiBinding("Go one line up (informational)", ConsoleKey.W, null),
                    new InteractiveTuiBinding("Go one line down (informational)", ConsoleKey.S, null),
                    new InteractiveTuiBinding("Read more...", ConsoleKey.I, ConsoleModifiers.Shift, null),
                    new InteractiveTuiBinding(Input.InvertScrollYAxis ? "Go one element down" : "Go one element up", PointerButton.WheelUp, null),
                    new InteractiveTuiBinding(Input.InvertScrollYAxis ? "Go one element up" : "Go one element down", PointerButton.WheelDown, null),
                    new InteractiveTuiBinding("Do an action on the selected item", PointerButton.Left, PointerButtonPress.Released, null),
                ]);
            }

            // Now, add the custom bindings
            if (interactiveTui.Bindings is not null && interactiveTui.Bindings.Length > 0)
                finalBindings.AddRange(interactiveTui.Bindings);

            // Filter the bindings based on the binding type
            return [.. finalBindings];
        }

        private static bool InfoScrollUp()
        {
            if (InteractiveTuiStatus.CurrentInfoLine == 0)
                return false;

            // Now, ascend
            InteractiveTuiStatus.CurrentInfoLine--;
            if (InteractiveTuiStatus.CurrentInfoLine < 0)
                InteractiveTuiStatus.CurrentInfoLine = 0;
            return true;
        }

        private static bool InfoScrollDown<T>(BaseInteractiveTui<T> interactiveTui)
        {
            // Get the wrapped info string
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            string finalInfoRendered = RenderFinalInfo(interactiveTui);
            string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            if (InteractiveTuiStatus.CurrentInfoLine == finalInfoStrings.Length - 1)
                return false;

            // Now, descend
            InteractiveTuiStatus.CurrentInfoLine++;
            if (InteractiveTuiStatus.CurrentInfoLine > finalInfoStrings.Length - SeparatorMaximumHeightInterior)
                InteractiveTuiStatus.CurrentInfoLine = finalInfoStrings.Length - SeparatorMaximumHeightInterior;
            return true;
        }

        private static bool VerifyInteractiveTui<T>(BaseInteractiveTui<T> interactiveTui)
        {
            if (interactiveTui is null)
                throw new TerminauxException("Please provide a base Interactive TUI class and try again.");

            // First, check to see if the interactive TUI has no data source
            if ((interactiveTui.PrimaryDataSource is null || interactiveTui.SecondaryDataSource is null ||
                (interactiveTui.PrimaryDataSource.Length() == 0 && interactiveTui.SecondaryDataSource.Length() == 0)) && !interactiveTui.AcceptsEmptyData)
            {
                InfoBoxColor.WriteInfoBoxColorBack("The interactive TUI {0} doesn't contain any data source. This program can't continue.\n" + "Press any key to continue...", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, interactiveTui.GetType().Name);
                return false;
            }

            // Then, check for conflicts in the key bindings
            var bindings = GetAllBindings(interactiveTui, true);
            List<InteractiveTuiBinding> conflicts = [];
            foreach (var binding in bindings)
            {
                var keyBindings = bindings.Where((keyBinding) => !binding.BindingUsesMouse && !keyBinding.BindingUsesMouse && keyBinding.BindingKeyName == binding.BindingKeyName && keyBinding.BindingKeyModifiers == binding.BindingKeyModifiers);
                var mouseBindings = bindings.Where((keyBinding) => binding.BindingUsesMouse && keyBinding.BindingUsesMouse && keyBinding.BindingPointerButton == binding.BindingPointerButton && keyBinding.BindingPointerButtonPress == binding.BindingPointerButtonPress && keyBinding.BindingPointerModifiers == binding.BindingPointerModifiers);
                if (keyBindings.Count() > 1 || mouseBindings.Count() > 1)
                    conflicts.Add(binding);
            }
            if (conflicts.Count > 0)
            {
                InfoBoxColor.WriteInfoBoxColorBack("The interactive TUI {0} has conflicting keyboard or mouse bindings.\n\nThe following keybindings or mouse bindings conflict:\n  - {1}\n\nThis program can't continue.\n" + "Press any key to continue...", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, interactiveTui.GetType().Name, string.Join("\n  - ", conflicts.Select((binding) => $"[{GetBindingKeyShortcut(binding, false)}{GetBindingMouseShortcut(binding, false)}] {binding.BindingName}")));
                return false;
            }
            return true;
        }

        static InteractiveTuiTools()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
