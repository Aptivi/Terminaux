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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.General;
using Textify.Sequences.Builder.Types;
using Textify.Sequences.Tools;

namespace Terminaux.Inputs.Interactive
{
    /// <summary>
    /// Tools for the interactive TUI implementation
    /// </summary>
    public static class InteractiveTuiTools
    {

        private static string _finalInfoRendered = "";
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
                BaseInteractiveTui.instances.Add(interactiveTui);

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
                bool notifyCrash = false;
                string crashReason = "";

                // Make the screen
                var screen = new Screen();
                ScreenTools.SetCurrent(screen);
                interactiveTui.screen = screen;

                // Now, run the application
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

                        // Reset, in case selection changed
                        screen.RemoveBufferedParts();
                    }
                }
                catch (Exception ex)
                {
                    notifyCrash = true;
                    crashReason = TextTools.FormatString("The interactive TUI, {0}, has crashed for the following reason:", interactiveTui.GetType().Name) + $" {ex.Message}";
                }
                finally
                {
                    BaseInteractiveTui.instances.Remove(interactiveTui);
                }
                ScreenTools.UnsetCurrent(screen);

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
            if (InteractiveTuiStatus.CurrentPane == 2)
                InteractiveTuiStatus.SecondPaneCurrentSelection = pos;
            else
                InteractiveTuiStatus.FirstPaneCurrentSelection = pos;
        }

        internal static int CountElements(IEnumerable enumerable)
        {
            // First, focus on the known types
            if (enumerable is Array arrayEnumerable)
            {
                // It's an array!
                return arrayEnumerable.Length;
            }
            else if (enumerable is IList listEnumerable)
            {
                // It's a list!
                return listEnumerable.Count;
            }
            else if (enumerable is IDictionary dictionaryEnumerable)
            {
                // It's a dictionary!
                return dictionaryEnumerable.Count;
            }
            else if (enumerable is ICollection collectionEnumerable)
            {
                // It's a collection!
                return collectionEnumerable.Count;
            }

            // We're in the unknown IEnumerable.
            var generic = enumerable.OfType<object>();
            return generic.Count();
        }

        internal static object GetElementFromIndex(IEnumerable enumerable, int index)
        {
            if (index < 0)
                return null;

            // First, focus on the known types
            if (enumerable is Array arrayEnumerable)
            {
                // It's an array!
                return arrayEnumerable.GetValue(index);
            }
            else if (enumerable is IList listEnumerable)
            {
                // It's a list!
                return listEnumerable[index];
            }
            else if (enumerable is IDictionary dictionaryEnumerable)
            {
                // It's a dictionary!
                var keys = new object[dictionaryEnumerable.Count];
                dictionaryEnumerable.Keys.CopyTo(keys, 0);
                var key = keys[index];
                return dictionaryEnumerable[key];
            }
            else if (enumerable is ICollection collectionEnumerable)
            {
                var collection = collectionEnumerable.OfType<object>();
                return collection.ElementAt(index);
            }

            // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
            var generic = enumerable.OfType<object>();
            return generic.ElementAt(index);
        }

        private static void DrawInteractiveTui(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");

            // Make a screen part
            var part = new ScreenPart();

            // Prepare the console
            ConsoleWrapper.CursorVisible = false;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Redraw the entire TUI screen
            part.BackgroundColor(InteractiveTuiStatus.BackgroundColor);
            part.AddText(CsiSequences.GenerateCsiEraseInDisplay(2));

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
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorFirstPane.VTSequenceForeground);
                builder.Append(InteractiveTuiStatus.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior, SeparatorMaximumHeightInterior));
                return builder.ToString();
            }));
            part.AddDynamicText(new(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorSecondPane.VTSequenceForeground);
                builder.Append(InteractiveTuiStatus.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior));
                return builder.ToString();
            }));

            // Populate appropriate bindings, depending on the SecondPaneInteractable value
            List<InteractiveTuiBinding> finalBindings;
            if (interactiveTui.Bindings is null || interactiveTui.Bindings.Count == 0)
                finalBindings =
                [
                    new InteractiveTuiBinding(/* Localizable */ "Exit", ConsoleKey.Escape, null)
                ];
            else
                finalBindings = new(interactiveTui.Bindings)
                {
                    new InteractiveTuiBinding(/* Localizable */ "Exit", ConsoleKey.Escape, null),
                    new InteractiveTuiBinding(/* Localizable */ "Keybindings", ConsoleKey.K, null),
                };
            if (interactiveTui.SecondPaneInteractable)
                finalBindings.Add(
                    new InteractiveTuiBinding(/* Localizable */ "Switch", ConsoleKey.Tab, null)
                );

            // Render the key bindings
            part.AddDynamicText(() =>
            {
                var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                foreach (InteractiveTuiBinding binding in finalBindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                    int actualLength = VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()).Length;
                    bool canDraw = renderedBinding.Length + actualLength < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        bindingsBuilder.Append(
                            $"{InteractiveTuiStatus.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.OptionBackgroundColor.VTSequenceBackground}" +
                            GetBindingKeyShortcut(binding, false) +
                            $"{InteractiveTuiStatus.OptionForegroundColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.BackgroundColor.VTSequenceBackground}" +
                            $" {binding.BindingName}  "
                        );
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        bindingsBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                            $"{InteractiveTuiStatus.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.OptionBackgroundColor.VTSequenceBackground}" +
                            " K "
                        );
                        break;
                    }
                }
                return bindingsBuilder.ToString();
            });

            interactiveTui.screen.AddBufferedPart($"Interactive TUI - Main - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawInteractiveTuiItems(BaseInteractiveTui interactiveTui, int paneNum)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");
            Debug.Assert(interactiveTui.screen is not null,
                "attempted to render TUI items on no screen");

            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            Debug.Assert(!interactiveTui.SecondPaneInteractable && paneNum == 1 || interactiveTui.SecondPaneInteractable,
                "tried to render interactive TUI items for the secondary pane on an interactive TUI that only allows interaction from one pane.");

            // Make a screen part
            var part = new ScreenPart();

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data are there in the chosen data source
            var data = paneNum == 2 ? interactiveTui.SecondaryDataSource : interactiveTui.PrimaryDataSource;
            int dataCount = CountElements(data);

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
                    object dataObject = null;
                    if (finalIndex <= dataCount - 1)
                    {
                        // Here, it's getting uglier as we don't have ElementAt() in IEnumerable, too!
                        dataObject = GetElementFromIndex(data, startIndex + i);

                        // Render an entry
                        var finalForeColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemForeColor : InteractiveTuiStatus.PaneItemForeColor;
                        var finalBackColor = finalIndex == paneCurrentSelection - 1 ? InteractiveTuiStatus.PaneSelectedItemBackColor : InteractiveTuiStatus.PaneItemBackColor;
                        int leftPos = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                        int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                        finalEntry = interactiveTui.GetEntryFromItem(dataObject).Truncate(SeparatorHalfConsoleWidthInterior - 4);
                        string text =
                            $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, top + 1)}" +
                            $"{finalForeColor.VTSequenceForeground}" +
                            $"{finalBackColor.VTSequenceBackground}" +
                            finalEntry +
                            new string(' ', SeparatorHalfConsoleWidthInterior - finalEntry.Length - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 0 : 1)) +
                            $"{InteractiveTuiStatus.PaneItemBackColor.VTSequenceBackground}";
                        builder.Append(text);
                    }
                }

                // Render the vertical bar
                int left = paneNum == 2 ? SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? 2 : 1) : SeparatorHalfConsoleWidthInterior - 1;
                builder.Append(ProgressBarVerticalColor.RenderVerticalProgress(100 * ((double)paneCurrentSelection / dataCount), left, 1, 2, 2, InteractiveTuiStatus.PaneSeparatorColor, InteractiveTuiStatus.PaneBackgroundColor, false));
                return builder.ToString();
            });

            interactiveTui.screen.AddBufferedPart($"Interactive TUI - Items - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawInformationOnSecondPane(BaseInteractiveTui interactiveTui)
        {
            // Check to make sure that we don't get nulls on interactiveTui
            Debug.Assert(interactiveTui is not null,
                "attempted to render TUI items on null");
            Debug.Assert(interactiveTui.screen is not null,
                "attempted to render TUI items on no screen");

            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            Debug.Assert(!interactiveTui.SecondPaneInteractable,
                "tried to render information the secondary pane on an interactive TUI that allows interaction from two panes, messing the selection rendering up there.");
            // Make a screen part
            var part = new ScreenPart();

            // Populate some positions
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
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
                {
                    finalInfoRendered = "No info.";
                }
            }
            catch
            {
                finalInfoRendered = "Failed to get information.";
            }

            // Now, write info
            var finalForeColorSecondPane = InteractiveTuiStatus.CurrentPane == 2 ? InteractiveTuiStatus.PaneSelectedSeparatorColor : InteractiveTuiStatus.PaneSeparatorColor;
            part.AddDynamicText(() =>
            {
                int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
                int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMinimumHeightInterior = 2;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
                var builder = new StringBuilder();
                builder.Append(finalForeColorSecondPane.VTSequenceForeground);
                builder.Append(InteractiveTuiStatus.PaneBackgroundColor.VTSequenceBackground);
                builder.Append(BorderColor.RenderBorderPlain(SeparatorHalfConsoleWidth, SeparatorMinimumHeight, SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0), SeparatorMaximumHeightInterior));

                _finalInfoRendered = finalInfoRendered;
                string[] finalInfoStrings = TextTools.GetWrappedSentences(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
                for (int infoIndex = 0; infoIndex < finalInfoStrings.Length; infoIndex++)
                {
                    // Check to see if the info is overpopulated
                    if (infoIndex >= SeparatorMaximumHeightInterior - 1)
                    {
                        string truncated = "Shift+I = more info";
                        builder.Append(ForegroundColor.VTSequenceForeground);
                        builder.Append(PaneItemBackColor.VTSequenceBackground);
                        builder.Append(TextWriterWhereColor.RenderWherePlain(truncated + new string(' ', SeparatorHalfConsoleWidthInterior - truncated.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex));
                        break;
                    }

                    // Now, render the info
                    string finalInfo = finalInfoStrings[infoIndex];
                    builder.Append(ForegroundColor.VTSequenceForeground);
                    builder.Append(PaneItemBackColor.VTSequenceBackground);
                    builder.Append(TextWriterWhereColor.RenderWherePlain(finalInfo + new string(' ', SeparatorHalfConsoleWidthInterior - finalInfo.Length), SeparatorHalfConsoleWidth + 1, SeparatorMinimumHeightInterior + infoIndex));
                }
                return builder.ToString();
            });

            interactiveTui.screen.AddBufferedPart($"Interactive TUI - Info (2nd pane) - {interactiveTui.GetType().Name}", part);
        }

        private static void DrawStatus(BaseInteractiveTui interactiveTui)
        {
            Debug.Assert(interactiveTui.screen is not null,
                "attempted to render TUI items on no screen");

            // Make a screen part
            var part = new ScreenPart();

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
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(InteractiveTuiStatus.ForegroundColor.VTSequenceForeground);
                builder.Append(InteractiveTuiStatus.BackgroundColor.VTSequenceBackground);
                builder.Append(TextWriterWhereColor.RenderWherePlain(InteractiveTuiStatus.Status.Truncate(ConsoleWrapper.WindowWidth - 3), 0, 0));
                builder.Append(ConsoleExtensions.GetClearLineToRightSequence());
                return builder.ToString();
            });
            interactiveTui.screen.AddBufferedPart($"Interactive TUI - Status - {interactiveTui.GetType().Name}", part);
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
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Populate selected data
            object selectedData = GetElementFromIndex(data, paneCurrentSelection - 1);

            // Wait for key
            ConsoleKeyInfo pressedKey;
            if (interactiveTui.RefreshInterval == 0 || interactiveTui.SecondPaneInteractable)
                pressedKey = Input.DetectKeypress();
            else
                pressedKey = Input.ReadKeyTimeout(true, TimeSpan.FromMilliseconds(interactiveTui.RefreshInterval)).result;

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
                case ConsoleKey.Home:
                    SelectionMovement(interactiveTui, 1);
                    break;
                case ConsoleKey.End:
                    SelectionMovement(interactiveTui, dataCount);
                    break;
                case ConsoleKey.PageUp:
                    {
                        int answersPerPage = SeparatorMaximumHeightInterior;
                        int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                        int startIndex = answersPerPage * currentPage;
                        SelectionMovement(interactiveTui, startIndex);
                    }
                    break;
                case ConsoleKey.PageDown:
                    {
                        int answersPerPage = SeparatorMaximumHeightInterior;
                        int currentPage = (paneCurrentSelection - 1) / answersPerPage;
                        int startIndex = answersPerPage * (currentPage + 1) + 1;
                        SelectionMovement(interactiveTui, startIndex);
                    }
                    break;
                case ConsoleKey.I:
                    if (pressedKey.Modifiers.HasFlag(ConsoleModifiers.Shift) && !string.IsNullOrEmpty(_finalInfoRendered))
                    {
                        // User needs more information in the infobox
                        InfoBoxColor.WriteInfoBoxColorBack(_finalInfoRendered, InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                    }
                    break;
                case ConsoleKey.K:
                    // First, check the bindings length
                    var bindings = interactiveTui.Bindings;
                    if (bindings is null || bindings.Count == 0)
                        break;

                    // User needs an infobox that shows all available keys
                    string section = "Available keys";
                    int maxBindingLength = bindings
                        .Max((itb) => GetBindingKeyShortcut(itb).Length);
                    string[] bindingRepresentations = bindings
                        .Select((itb) => $"{GetBindingKeyShortcut(itb) + new string(' ', maxBindingLength - GetBindingKeyShortcut(itb).Length) + $" | {itb.BindingName}"}")
                        .ToArray();
                    InfoBoxColor.WriteInfoBoxColorBack(
                        $"{section}{CharManager.NewLine}" +
                        $"{new string('=', section.Length)}{CharManager.NewLine}{CharManager.NewLine}" +
                        $"{string.Join("\n", bindingRepresentations)}"
                    , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                    break;
                case ConsoleKey.Escape:
                    // User needs to exit
                    interactiveTui.HandleExit();
                    interactiveTui.isExiting = true;
                    break;
                case ConsoleKey.Tab:
                    // User needs to switch sides
                    SwitchSides(interactiveTui);
                    break;
                default:
                    // First, check the bindings
                    var allBindings = interactiveTui.Bindings;
                    if (allBindings is null || allBindings.Count == 0)
                        break;

                    // Now, get the implemented bindings from the pressed key
                    var implementedBindings = allBindings.Where((binding) =>
                        binding.BindingKeyName == pressedKey.Key && binding.BindingKeyModifiers == pressedKey.Modifiers);
                    foreach (var implementedBinding in implementedBindings)
                        implementedBinding.BindingAction.Invoke(selectedData, paneCurrentSelection - 1);
                    break;
            }
        }

        private static void CheckSelectionForUnderflow(BaseInteractiveTui interactiveTui)
        {
            if (InteractiveTuiStatus.FirstPaneCurrentSelection <= 0 && CountElements(interactiveTui.PrimaryDataSource) > 0)
                InteractiveTuiStatus.FirstPaneCurrentSelection = 1;
            if (InteractiveTuiStatus.SecondPaneCurrentSelection <= 0 && CountElements(interactiveTui.SecondaryDataSource) > 0)
                InteractiveTuiStatus.SecondPaneCurrentSelection = 1;
        }

        private static string GetBindingKeyShortcut(InteractiveTuiBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingKeyModifiers != 0 ? $"{bind.BindingKeyModifiers} + " : "")}{bind.BindingKeyName}{markEnd}";
        }

        private static void SwitchSides(BaseInteractiveTui interactiveTui)
        {
            if (!interactiveTui.SecondPaneInteractable)
                return;
            InteractiveTuiStatus.CurrentPane++;
            if (InteractiveTuiStatus.CurrentPane > 2)
                InteractiveTuiStatus.CurrentPane = 1;
        }
    }
}
