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

using Magico.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry.Transformation;
using Terminaux.Inputs.Interactive.Selectors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Simple;

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
        public static void OpenInteractiveTui<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            lock (_interactiveTuiLock)
            {
                if (!VerifyInteractiveTui(interactiveTui))
                    return;

                // Check the selection
                LastOnOverflow(interactiveTui);
                FirstOnUnderflow(interactiveTui);

                // Make the screen
                var tui = new InteractiveSelectorTui<TPrimary, TSecondary>(interactiveTui)
                {
                    RefreshDelay = interactiveTui.RefreshInterval,
                    extraHelpPages = interactiveTui.HelpPages,
                };
                TextualUITools.RunTui(tui);
            }
        }

        /// <summary>
        /// Initiates the selection movement
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        /// <param name="pos">Position to move the pane selection to</param>
        public static void SelectionMovement<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, int pos) =>
            SelectionMovement(interactiveTui, pos, interactiveTui.CurrentPane);

        /// <summary>
        /// Initiates the selection movement
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        /// <param name="pos">Position to move the pane selection to</param>
        /// <param name="paneNum">Pane number to process</param>
        public static void SelectionMovement<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, int pos, int paneNum)
        {
            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Check the position
            int elements = paneNum == 2 ? interactiveTui.SecondaryDataSource.Count() : interactiveTui.PrimaryDataSource.Count();
            if (pos < 1)
                pos = 1;
            if (pos > elements)
                pos = elements;

            // Now, process the movement
            interactiveTui.CurrentInfoLine = 0;
            if (paneNum == 2)
                interactiveTui.SecondPaneCurrentSelection = pos;
            else
                interactiveTui.FirstPaneCurrentSelection = pos;

            // Check the positioning
            FirstOnUnderflow(interactiveTui);
            LastOnOverflow(interactiveTui);
        }

        /// <summary>
        /// Switches between two panes
        /// </summary>
        /// <param name="interactiveTui">Interactive TUI to deal with</param>
        public static void SwitchSides<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            if (!interactiveTui.SecondPaneInteractable)
                return;
            interactiveTui.CurrentPane++;
            if (interactiveTui.CurrentPane > 2)
                interactiveTui.CurrentPane = 1;
        }

        /// <summary>
        /// Goes down to the last element upon overflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public static void LastOnOverflow<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            int primaryCount = interactiveTui.PrimaryDataSource.Length();
            int secondaryCount = interactiveTui.SecondaryDataSource.Length();
            if (interactiveTui.FirstPaneCurrentSelection > primaryCount)
                interactiveTui.FirstPaneCurrentSelection = primaryCount;
            if (interactiveTui.SecondPaneCurrentSelection > secondaryCount)
                interactiveTui.SecondPaneCurrentSelection = secondaryCount;
        }

        /// <summary>
        /// Goes up to the first element upon underflow (caused by remove operation, ...). This applies to the first and the second pane.
        /// </summary>
        public static void FirstOnUnderflow<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            int primaryCount = interactiveTui.PrimaryDataSource.Length();
            int secondaryCount = interactiveTui.SecondaryDataSource.Length();
            if (interactiveTui.FirstPaneCurrentSelection <= 0 && primaryCount > 0)
                interactiveTui.FirstPaneCurrentSelection = 1;
            if (interactiveTui.SecondPaneCurrentSelection <= 0 && secondaryCount > 0)
                interactiveTui.SecondPaneCurrentSelection = 1;
        }

        internal static string RenderInteractiveTui<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
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
            var elements = new StringBuilder();

            // First, the horizontal and vertical separators
            var finalForeColorFirstPane = interactiveTui.CurrentPane == 1 ? interactiveTui.Settings.PaneSelectedSeparatorColor : interactiveTui.Settings.PaneSeparatorColor;
            var finalForeColorSecondPane = interactiveTui.CurrentPane == 2 ? interactiveTui.Settings.PaneSelectedSeparatorColor : interactiveTui.Settings.PaneSeparatorColor;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            var firstPane = new Border()
            {
                Left = 0,
                Top = SeparatorMinimumHeight,
                Width = SeparatorHalfConsoleWidthInterior,
                Height = SeparatorMaximumHeightInterior,
                Settings = interactiveTui.Settings.InfoBoxSettings.BorderSettings,
                Color = finalForeColorFirstPane,
                BackgroundColor = interactiveTui.Settings.PaneBackgroundColor,
            };
            var secondPane = new Border()
            {
                Left = SeparatorHalfConsoleWidth,
                Top = SeparatorMinimumHeight,
                Width = SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0),
                Height = SeparatorMaximumHeightInterior,
                Settings = interactiveTui.Settings.InfoBoxSettings.BorderSettings,
                Color = finalForeColorSecondPane,
                BackgroundColor = interactiveTui.Settings.PaneBackgroundColor,
            };
            elements.Append(firstPane.Render());
            elements.Append(secondPane.Render());

            // Populate appropriate bindings, depending on the SecondPaneInteractable value, and render them
            var finalBindings = GetAllBindings(interactiveTui);
            var builtIns = finalBindings.Where((itb) => !interactiveTui.Bindings.Contains(itb)).ToArray();
            var keybindingsRenderable = new Keybindings()
            {
                KeybindingList = [.. interactiveTui.Bindings],
                BuiltinKeybindings = builtIns,
                BuiltinColor = interactiveTui.Settings.KeyBindingBuiltinColor,
                BuiltinForegroundColor = interactiveTui.Settings.KeyBindingBuiltinForegroundColor,
                BuiltinBackgroundColor = interactiveTui.Settings.KeyBindingBuiltinBackgroundColor,
                OptionColor = interactiveTui.Settings.KeyBindingOptionColor,
                OptionForegroundColor = interactiveTui.Settings.OptionForegroundColor,
                OptionBackgroundColor = interactiveTui.Settings.OptionBackgroundColor,
                BackgroundColor = interactiveTui.Settings.BackgroundColor,
                Width = ConsoleWrapper.WindowWidth - 1,
                WriteHelpKeyInfo = false,
            };
            elements.Append(RendererTools.RenderRenderable(keybindingsRenderable, new(0, ConsoleWrapper.WindowHeight - 1)));

            // Return the result
            return elements.ToString();
        }

        internal static string RenderInteractiveTuiItems<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, int paneNum)
        {
            // Check to make sure that we're not rendering the second pane on the first-pane-only interactive TUI
            if (!interactiveTui.SecondPaneInteractable && paneNum > 1)
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_RENDERSECONDONLYONEPANE"));

            // Check the pane number
            if (paneNum < 1)
                paneNum = 1;
            if (paneNum > 2)
                paneNum = 2;

            // Get how many data instances are there in the chosen data source
            var dataPrimary = interactiveTui.PrimaryDataSource;
            var dataSecondary = interactiveTui.SecondaryDataSource;
            int dataCount = paneNum == 2 ? dataSecondary.Length() : dataPrimary.Length();

            // Render the pane right away
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            int answersPerPage = SeparatorMaximumHeightInterior;
            int paneCurrentSelection = paneNum == 2 ? interactiveTui.SecondPaneCurrentSelection : interactiveTui.FirstPaneCurrentSelection;
            int currentPage = (paneCurrentSelection - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;
            var builder = new StringBuilder();
            for (int i = 0; i <= answersPerPage - 1; i++)
            {
                // Populate the first pane
                string finalEntry = "";
                int finalIndex = i + startIndex;
                object? dataObject = null;
                if (finalIndex <= dataCount - 1)
                {
                    dataObject = paneNum == 2 ? dataSecondary.GetElementFromIndex(startIndex + i) : dataPrimary.GetElementFromIndex(startIndex + i);
                    if (dataObject is null)
                        continue;

                    // Render an entry
                    var finalForeColor = finalIndex == paneCurrentSelection - 1 ? interactiveTui.Settings.PaneSelectedItemForeColor : interactiveTui.Settings.PaneItemForeColor;
                    var finalBackColor = finalIndex == paneCurrentSelection - 1 ? interactiveTui.Settings.PaneSelectedItemBackColor : interactiveTui.Settings.PaneItemBackColor;
                    int leftPos = paneNum == 2 ? SeparatorHalfConsoleWidth + 1 : 1;
                    int top = SeparatorMinimumHeightInterior + finalIndex - startIndex;
                    finalEntry = (paneNum == 2 ? interactiveTui.GetEntryFromItemSecondary((TSecondary)dataObject) : interactiveTui.GetEntryFromItem((TPrimary)dataObject)).Truncate(SeparatorHalfConsoleWidthInterior - 1);
                    int width = ConsoleChar.EstimateCellWidth(finalEntry);
                    string text =
                        $"{CsiSequences.GenerateCsiCursorPosition(leftPos + 1, top + 1)}" +
                        $"{ConsoleColoring.RenderSetConsoleColor(finalForeColor, false, true)}" +
                        $"{ConsoleColoring.RenderSetConsoleColor(finalBackColor, true)}" +
                        finalEntry +
                        $"{ConsoleColoring.RenderSetConsoleColor(finalForeColor, false, true)}" +
                        $"{ConsoleColoring.RenderSetConsoleColor(finalBackColor, true)}" +
                        new string(' ', SeparatorHalfConsoleWidthInterior - width - (ConsoleWrapper.WindowWidth % 2 != 0 && paneNum == 2 ? -1 : 0)) +
                        $"{ConsoleColoring.RenderSetConsoleColor(interactiveTui.Settings.PaneItemBackColor, true)}";
                    builder.Append(text);
                }
                else
                    break;
            }

            // Render the vertical bar
            var finalForeColorFirstPane = interactiveTui.CurrentPane == 1 ? interactiveTui.Settings.PaneSelectedSeparatorColor : interactiveTui.Settings.PaneSeparatorColor;
            var finalForeColorSecondPane = interactiveTui.CurrentPane == 2 ? interactiveTui.Settings.PaneSelectedSeparatorColor : interactiveTui.Settings.PaneSeparatorColor;
            int left = paneNum == 2 ? SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 3 : 2) : SeparatorHalfConsoleWidthInterior;
            if (dataCount > SeparatorMaximumHeightInterior)
            {
                var finalColor = paneNum == 2 ? finalForeColorSecondPane : finalForeColorFirstPane;
                var dataSlider = new Slider(paneCurrentSelection, 0, dataCount)
                {
                    Vertical = true,
                    Height = ConsoleWrapper.WindowHeight - 6,
                    SliderActiveForegroundColor = finalColor,
                    SliderForegroundColor = TransformationTools.GetDarkBackground(finalColor),
                    SliderBackgroundColor = interactiveTui.Settings.BackgroundColor,
                    SliderVerticalActiveTrackChar = interactiveTui.Settings.InfoBoxSettings.BorderSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = interactiveTui.Settings.InfoBoxSettings.BorderSettings.BorderRightFrameChar,
                };
                builder.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left + 1, 2, finalColor, interactiveTui.Settings.PaneBackgroundColor));
                builder.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left + 1, SeparatorMaximumHeightInterior + 1, finalColor, interactiveTui.Settings.PaneBackgroundColor));
                builder.Append(RendererTools.RenderRenderable(dataSlider, new(left + 1, 3)));
            }
            return builder.ToString();
        }

        internal static string RenderInformationOnSecondPane<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            // Check to make sure that we're not rendering the information pane on the both-panes interactive TUI
            if (interactiveTui.SecondPaneInteractable)
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_RENDERINFOTWOPANES"));

            // Populate some colors
            var ForegroundColor = interactiveTui.Settings.ForegroundColor;
            var PaneItemBackColor = interactiveTui.Settings.PaneItemBackColor;

            // Now, write info
            string finalInfoRendered = RenderFinalInfo(interactiveTui);
            var finalForeColorSecondPane = interactiveTui.CurrentPane == 2 ? interactiveTui.Settings.PaneSelectedSeparatorColor : interactiveTui.Settings.PaneSeparatorColor;
            int SeparatorHalfConsoleWidth = ConsoleWrapper.WindowWidth / 2;
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMinimumHeightInterior = 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            var builder = new StringBuilder();

            // Render a border
            var border = new Border()
            {
                Left = SeparatorHalfConsoleWidth,
                Top = SeparatorMinimumHeight,
                Width = SeparatorHalfConsoleWidthInterior + (ConsoleWrapper.WindowWidth % 2 != 0 ? 1 : 0),
                Height = SeparatorMaximumHeightInterior,
                Settings = interactiveTui.Settings.InfoBoxSettings.BorderSettings,
                Color = finalForeColorSecondPane,
                BackgroundColor = interactiveTui.Settings.PaneBackgroundColor
            };
            builder.Append(border.Render());

            // Split the information string
            string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            var bounded = new BoundedText()
            {
                Left = SeparatorHalfConsoleWidth + 1,
                Top = SeparatorMinimumHeightInterior,
                Line = interactiveTui.CurrentInfoLine,
                Height = SeparatorMaximumHeightInterior,
                Width = SeparatorHalfConsoleWidthInterior,
                ForegroundColor = ForegroundColor,
                BackgroundColor = PaneItemBackColor,
                Text = finalInfoRendered
            };
            builder.Append(bounded.Render());

            // Render the vertical bar
            int left = SeparatorHalfConsoleWidthInterior * 2 + (ConsoleWrapper.WindowWidth % 2 != 0 ? 3 : 2);
            if (finalInfoStrings.Length > SeparatorMaximumHeightInterior)
            {
                var dataSlider = new Slider((int)((double)interactiveTui.CurrentInfoLine / (finalInfoStrings.Length - SeparatorMaximumHeightInterior) * finalInfoStrings.Length), 0, finalInfoStrings.Length)
                {
                    Vertical = true,
                    Height = ConsoleWrapper.WindowHeight - 6,
                    SliderActiveForegroundColor = finalForeColorSecondPane,
                    SliderForegroundColor = TransformationTools.GetDarkBackground(finalForeColorSecondPane),
                    SliderBackgroundColor = interactiveTui.Settings.BackgroundColor,
                    SliderVerticalActiveTrackChar = interactiveTui.Settings.InfoBoxSettings.BorderSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = interactiveTui.Settings.InfoBoxSettings.BorderSettings.BorderRightFrameChar,
                };
                builder.Append(TextWriterWhereColor.RenderWhereColorBack("[W|S|SHIFT + I]", ConsoleWrapper.WindowWidth - "[W|S|SHIFT + I]".Length - 2, SeparatorMaximumHeightInterior + 2, finalForeColorSecondPane, interactiveTui.Settings.PaneBackgroundColor));
                builder.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left + 1, 2, finalForeColorSecondPane, interactiveTui.Settings.PaneBackgroundColor));
                builder.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left + 1, SeparatorMaximumHeightInterior + 1, finalForeColorSecondPane, interactiveTui.Settings.PaneBackgroundColor));
                builder.Append(RendererTools.RenderRenderable(dataSlider, new(left + 1, 3)));
            }
            return builder.ToString();
        }

        internal static string RenderStatus<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            // Populate some necessary variables
            if (interactiveTui.CurrentPane == 2)
            {
                var data = interactiveTui.SecondaryDataSource;
                int length = data.Length();
                if (length > 0 && interactiveTui.SecondPaneCurrentSelection > 0 && interactiveTui.SecondPaneCurrentSelection <= length)
                {
                    TSecondary? selectedData = (TSecondary?)data.GetElementFromIndex(interactiveTui.SecondPaneCurrentSelection - 1);
                    interactiveTui.Status = selectedData is not null ? interactiveTui.GetStatusFromItemSecondary(selectedData) : "No status.";
                }
                else
                    interactiveTui.Status = "No status.";
            }
            else
            {
                var data = interactiveTui.PrimaryDataSource;
                int length = data.Length();
                if (length > 0 && interactiveTui.FirstPaneCurrentSelection > 0 && interactiveTui.FirstPaneCurrentSelection <= length)
                {
                    TPrimary? selectedData = (TPrimary?)data.GetElementFromIndex(interactiveTui.FirstPaneCurrentSelection - 1);
                    interactiveTui.Status = selectedData is not null ? interactiveTui.GetStatusFromItem(selectedData) : "No status.";
                }
                else
                    interactiveTui.Status = "No status.";
            }

            // Now, write info
            string truncatedStatus = interactiveTui.Status.Truncate(ConsoleWrapper.WindowWidth);
            int statusWidth = ConsoleChar.EstimateCellWidth(truncatedStatus);
            var builder = new StringBuilder();
            builder.Append(TextWriterWhereColor.RenderWhereColorBack(truncatedStatus, 0, 0, interactiveTui.Settings.ForegroundColor, interactiveTui.Settings.BackgroundColor));
            if (statusWidth < ConsoleWrapper.WindowWidth)
                builder.Append(ConsoleClearing.GetClearLineToRightSequence());
            return builder.ToString();
        }

        internal static string RenderFinalInfo<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            string finalInfoRendered;
            try
            {
                // Populate data source and its count
                var dataPrimary = interactiveTui.PrimaryDataSource;
                var dataSecondary = interactiveTui.SecondaryDataSource;
                int dataCount = interactiveTui.CurrentPane == 2 ? dataSecondary.Length() : dataPrimary.Length();

                // Populate selected data
                if (interactiveTui.CurrentPane == 2)
                {
                    if (dataCount > 0 && interactiveTui.SecondPaneCurrentSelection > 0 && interactiveTui.SecondPaneCurrentSelection <= dataCount)
                    {
                        TSecondary selectedData = (TSecondary)(dataSecondary.GetElementFromIndex(interactiveTui.SecondPaneCurrentSelection - 1) ??
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_RENDERINFONULLDATA")));
                        finalInfoRendered = interactiveTui.GetInfoFromItemSecondary(selectedData);
                    }
                    else
                        finalInfoRendered = LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_BASE_NOINFO");
                }
                else
                {
                    if (dataCount > 0 && interactiveTui.FirstPaneCurrentSelection > 0 && interactiveTui.FirstPaneCurrentSelection <= dataCount)
                    {
                        TPrimary selectedData = (TPrimary)(dataPrimary.GetElementFromIndex(interactiveTui.FirstPaneCurrentSelection - 1) ??
                            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_RENDERINFONULLDATA")));
                        finalInfoRendered = interactiveTui.GetInfoFromItem(selectedData);
                    }
                    else
                        finalInfoRendered = LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_BASE_NOINFO");
                }
            }
            catch
            {
                finalInfoRendered = LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_INFOFAILED");
            }
            return finalInfoRendered;
        }

        internal static bool InfoScrollUp<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, int factor = 1)
        {
            // Get the wrapped info string
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            string finalInfoRendered = RenderFinalInfo(interactiveTui);
            string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            if (interactiveTui.CurrentInfoLine == 0)
                return false;

            // Now, ascend
            if (factor < 1)
                factor = 1;
            interactiveTui.CurrentInfoLine -= factor;
            if (interactiveTui.CurrentInfoLine < 0)
                interactiveTui.CurrentInfoLine = factor == 1 ? finalInfoStrings.Length - SeparatorMaximumHeightInterior : 0;
            return true;
        }

        internal static bool InfoScrollDown<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, int factor = 1)
        {
            // Get the wrapped info string
            int SeparatorHalfConsoleWidthInterior = ConsoleWrapper.WindowWidth / 2 - 2;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;
            string finalInfoRendered = RenderFinalInfo(interactiveTui);
            string[] finalInfoStrings = ConsoleMisc.GetWrappedSentencesByWords(finalInfoRendered, SeparatorHalfConsoleWidthInterior);
            if (interactiveTui.CurrentInfoLine == finalInfoStrings.Length - 1)
                return false;

            // Now, descend
            if (factor < 1)
                factor = 1;
            interactiveTui.CurrentInfoLine += factor;
            if (interactiveTui.CurrentInfoLine > finalInfoStrings.Length - SeparatorMaximumHeightInterior)
                interactiveTui.CurrentInfoLine = factor == 1 ? 0 : finalInfoStrings.Length - SeparatorMaximumHeightInterior;
            return true;
        }

        internal static InteractiveTuiBinding<TPrimary, TSecondary>[] GetAllBindings<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui, bool full = false)
        {
            // Populate appropriate bindings, depending on the SecondPaneInteractable value
            List<InteractiveTuiBinding<TPrimary, TSecondary>> finalBindings =
            [
                new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_KEYBINDINGS"), ConsoleKey.K, null),
                new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_EXIT"), ConsoleKey.Escape, null),
            ];

            // Populate switch as needed
            if (interactiveTui.SecondPaneInteractable)
                finalBindings.Add(
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_SWITCH"), ConsoleKey.Tab, null)
                );

            // Now, check to see if we need to add additional base bindings
            if (full)
            {
                finalBindings.AddRange(
                [
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP2"), ConsoleKey.UpArrow, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN2"), ConsoleKey.DownArrow, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOFIRST2"), ConsoleKey.Home, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOLAST2"), ConsoleKey.End, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOPREVPAGE2"), ConsoleKey.PageUp, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GONEXTPAGE2"), ConsoleKey.PageDown, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_SEARCH"), ConsoleKey.F, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_GOUPINFO"), ConsoleKey.W, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_IS_COMMON_KEYBINDING_GODOWNINFO"), ConsoleKey.S, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_IS_SELECTOR_KEYBINDING_READMORE"), ConsoleKey.I, ConsoleModifiers.Shift, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(Input.InvertScrollYAxis ? LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN1") : LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP1"), PointerButton.WheelUp, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(Input.InvertScrollYAxis ? LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GOUP1") : LanguageTools.GetLocalized("T_INPUT_COMMON_KEYBINDING_GODOWN1"), PointerButton.WheelDown, null),
                    new InteractiveTuiBinding<TPrimary, TSecondary>(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_KEYBINDING_DOACTION"), PointerButton.Left, PointerButtonPress.Released, null),
                ]);
            }

            // Now, add the custom bindings
            if (interactiveTui.Bindings is not null && interactiveTui.Bindings.Count > 0)
                finalBindings.AddRange(interactiveTui.Bindings);

            // Filter the bindings based on the binding type
            return [.. finalBindings];
        }

        private static string GetBindingKeyShortcut<TPrimary, TSecondary>(InteractiveTuiBinding<TPrimary, TSecondary> bind, bool mark = true)
        {
            if (bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingKeyModifiers != 0 ? $"{bind.BindingKeyModifiers} + " : "")}{bind.BindingKeyName}{markEnd}";
        }

        private static string GetBindingMouseShortcut<TPrimary, TSecondary>(InteractiveTuiBinding<TPrimary, TSecondary> bind, bool mark = true)
        {
            if (!bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingPointerModifiers != 0 ? $"{bind.BindingPointerModifiers} + " : "")}{bind.BindingPointerButton}{(bind.BindingPointerButtonPress != 0 ? $" {bind.BindingPointerButtonPress}" : "")}{markEnd}";
        }

        private static bool VerifyInteractiveTui<TPrimary, TSecondary>(BaseInteractiveTui<TPrimary, TSecondary> interactiveTui)
        {
            if (interactiveTui is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_NEEDSBASECLASS"));

            // First, check to see if the interactive TUI has no data source
            if ((interactiveTui.PrimaryDataSource is null || interactiveTui.SecondaryDataSource is null ||
                (interactiveTui.PrimaryDataSource.Length() == 0 && interactiveTui.SecondaryDataSource.Length() == 0)) && !interactiveTui.AcceptsEmptyData)
            {
                InfoBoxModalColor.WriteInfoBoxModal(
                    LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_NODATASOURCE") + "\n\n" +
                    LanguageTools.GetLocalized("T_COMMON_CANTCONTINUE") + "\n" +
                    LanguageTools.GetLocalized("T_COMMON_ANYKEY"),
                    interactiveTui.Settings.InfoBoxSettings, interactiveTui.GetType().Name);
                return false;
            }

            // Then, check for conflicts in the key bindings
            var bindings = GetAllBindings(interactiveTui, true);
            List<InteractiveTuiBinding<TPrimary, TSecondary>> conflicts = [];
            foreach (var binding in bindings)
            {
                var keyBindings = bindings.Where((keyBinding) => !binding.BindingUsesMouse && !keyBinding.BindingUsesMouse && keyBinding.BindingKeyName == binding.BindingKeyName && keyBinding.BindingKeyModifiers == binding.BindingKeyModifiers);
                var mouseBindings = bindings.Where((keyBinding) => binding.BindingUsesMouse && keyBinding.BindingUsesMouse && keyBinding.BindingPointerButton == binding.BindingPointerButton && keyBinding.BindingPointerButtonPress == binding.BindingPointerButtonPress && keyBinding.BindingPointerModifiers == binding.BindingPointerModifiers);
                if (keyBindings.Count() > 1 || mouseBindings.Count() > 1)
                    conflicts.Add(binding);
            }
            if (conflicts.Count > 0)
            {
                InfoBoxModalColor.WriteInfoBoxModal(
                    LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_CONFLICTBINDINGS1") + "\n\n" +
                    LanguageTools.GetLocalized("T_INPUT_INTERACTIVE_EXCEPTION_CONFLICTBINDINGS2") + "\n" +
                    "  - {1}" + "\n\n" +
                    LanguageTools.GetLocalized("T_COMMON_CANTCONTINUE") + "\n" +
                    LanguageTools.GetLocalized("T_COMMON_ANYKEY"), interactiveTui.Settings.InfoBoxSettings, interactiveTui.GetType().Name, string.Join("\n  - ", conflicts.Select((binding) => $"[{GetBindingKeyShortcut(binding, false)}{GetBindingMouseShortcut(binding, false)}] {binding.BindingName}")));
                return false;
            }
            return true;
        }
    }
}
