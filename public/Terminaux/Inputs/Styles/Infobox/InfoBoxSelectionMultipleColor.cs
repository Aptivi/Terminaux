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

using System;
using System.Collections.Generic;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors;
using System.Text;
using System.Linq;
using Terminaux.Base.Buffered;
using System.Diagnostics;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using System.Threading;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Selections = Terminaux.Writer.CyclicWriters.Graphical.Selection;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with selection and color support
    /// </summary>
    public static class InfoBoxSelectionMultipleColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Goes one choice up (next choice)", ConsoleKey.UpArrow),
            new Keybinding("Goes one choice down (previous choice)", ConsoleKey.DownArrow),
            new Keybinding("Goes to the first choice", ConsoleKey.Home),
            new Keybinding("Goes to the last choice", ConsoleKey.End),
            new Keybinding("Goes to the previous page of choices", ConsoleKey.PageUp),
            new Keybinding("Goes to the next page of choices", ConsoleKey.PageDown),
            new Keybinding("Shows more info in an informational box", ConsoleKey.Tab),
            new Keybinding("Searches for a choice", ConsoleKey.F),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Selects or deselects all choices", ConsoleKey.A),
            new Keybinding("Selects or deselects a choice", ConsoleKey.Spacebar),
            new Keybinding("Submits the values", ConsoleKey.Enter),
            new Keybinding("Closes without submitting the values", ConsoleKey.Escape),
        ];

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiplePlain(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColor(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionMultipleColor">InfoBoxSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColorBack(InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                "", selections, text, settings, InfoBoxSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultiplePlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiplePlain(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                title, selections, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColor(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultiple(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        public static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionMultipleColorBack(
                title, selections, text, settings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionMultipleColor">InfoBoxTitledSelectionMultiple color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelectionMultiple background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>List of selected choice index (starting from zero), or an empty array if exited, selection list is empty, or an error occurred</returns>
        internal static int[] WriteInfoBoxSelectionMultipleColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionMultipleColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            List<int> selectedChoices = [];
            int AnswerTitleLeft = selections.Max(x => $"  [ ] {x.ChoiceName}) ".Length);

            // Make selected choices from the ChoiceDefaultSelected value.
            selectedChoices = selections.Any((ici) => ici.ChoiceDefaultSelected) ? selections.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx).ToList() : [];

            // Verify that we have selections
            if (selections is null || selections.Length == 0)
                return [.. selectedChoices];

            // We need not to run the selection style when everything is disabled
            bool allDisabled = selections.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException("The infobox selection style requires that there is at least one choice enabled.");

            // Now, some logic to get the informational box ready
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionMultipleColor), infoBoxScreenPart);
            try
            {
                // Modify the current selection according to the default
                int currentSelection = selections.Any((ici) => ici.ChoiceDefault) ? selections.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                int selectionChoices = selections.Length > 10 ? 10 : selections.Length;

                // Edge case: We need to check to see if the current highlight is disabled
                InfoBoxTools.VerifyDisabled(ref currentSelection, selections);

                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY, selectionBoxPosX, selectionBoxPosY, _, maxSelectionWidth, _, _) = InfoBoxTools.GetDimensions(selections, splitFinalLines);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(selections, title, text, settings, InfoBoxTitledSelectionMultipleColor, BackgroundColor, useColor, ref increment, currIdx, true, vars)
                    );

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY - 1,
                        Width = maxSelectionWidth,
                        Height = selectionChoices,
                        Settings = settings,
                        Color = InfoBoxTitledSelectionMultipleColor,
                    };
                    boxBuffer.Append(border.Render());

                    // Now, render the selections
                    var selectionsRendered = new Selections(selections)
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY,
                        CurrentSelection = currentSelection,
                        CurrentSelections = [.. selectedChoices],
                        Height = selectionChoices,
                        Width = maxSelectionWidth,
                        SliderInside = true,
                        AltChoicePos = selections.Length,
                        SwapSelectedColors = true,
                        Settings = new()
                        {
                            OptionColor = InfoBoxTitledSelectionMultipleColor,
                            SelectedOptionColor = InfoBoxTitledSelectionMultipleColor,
                            BackgroundColor = BackgroundColor,
                        }
                    };
                    boxBuffer.Append(
                        selectionsRendered.Render()
                    );

                    // Return the buffer
                    return boxBuffer.ToString();
                });

                // Query the enabled answers
                var enabledAnswers = selections.Select((ici, idx) => (ici, idx)).Where((ici) => !ici.ici.ChoiceDisabled).Select((tuple) => tuple.idx).ToArray();

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    bool goingUp = false;
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY, _, selectionBoxPosY, leftPos, maxSelectionWidth, arrowSelectLeft, selectionReservedHeight) = InfoBoxTools.GetDimensions(selections, splitFinalLines);
                    maxHeight -= selectionReservedHeight;

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = 2;
                    int arrowBottom = maxHeight + 1;

                    // Get positions for infobox buttons
                    string infoboxButtons = InfoBoxTools.GetButtons(settings);
                    int infoboxButtonsWidth = ConsoleChar.EstimateCellWidth(infoboxButtons);
                    int infoboxButtonLeftHelpMin = maxWidth + borderX - infoboxButtonsWidth;
                    int infoboxButtonLeftHelpMax = infoboxButtonLeftHelpMin + 2;
                    int infoboxButtonLeftCloseMin = infoboxButtonLeftHelpMin + 3;
                    int infoboxButtonLeftCloseMax = infoboxButtonLeftHelpMin + infoboxButtonsWidth;
                    int infoboxButtonsTop = borderY;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => GoDown(ref currIdx, text, vars))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSelectUpHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY), new Action<PointerEventContext>((_) => SelectionGoUp(ref currentSelection))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowSelectDownHitbox = new PointerHitbox(new(arrowSelectLeft, ConsoleWrapper.WindowHeight - selectionChoices), new Action<PointerEventContext>((_) => SelectionGoDown(ref currentSelection, selections))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    if (Input.MouseInputAvailable)
                    {
                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        ChoiceHitboxType hitboxType = ChoiceHitboxType.Choice;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                    GoUp(ref currIdx, 3);
                                else
                                {
                                    goingUp = true;
                                    SelectionGoUp(ref currentSelection);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                    GoDown(ref currIdx, text, vars, 3);
                                else
                                    SelectionGoDown(ref currentSelection, selections);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && splitFinalLines.Length > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowSelectUpHitbox.IsPointerWithin(mouse) || arrowSelectDownHitbox.IsPointerWithin(mouse)) && selections.Length > selectionChoices)
                                {
                                    arrowSelectUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSelectDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if (infoboxButtonHelpHitbox.IsPointerWithin(mouse))
                                    infoboxButtonHelpHitbox.ProcessPointer(mouse, out _);
                                else if (infoboxButtonCloseHitbox.IsPointerWithin(mouse))
                                    infoboxButtonCloseHitbox.ProcessPointer(mouse, out _);
                                else
                                {
                                    if (InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection))
                                    {
                                        switch (hitboxType)
                                        {
                                            case ChoiceHitboxType.Category:
                                                //ProcessSelectAll(2);
                                                break;
                                            case ChoiceHitboxType.Group:
                                                //ProcessSelectAll(1);
                                                break;
                                            case ChoiceHitboxType.Choice:
                                                if (!selectedChoices.Remove(currentSelection))
                                                    selectedChoices.Add(currentSelection);
                                                break;
                                        }
                                    }
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection);
                                var selectedInstance = selections[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, text, vars, out hitboxType, ref currentSelection);
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.Spacebar:
                                if (!selectedChoices.Remove(currentSelection))
                                    selectedChoices.Add(currentSelection);
                                break;
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                SelectionGoUp(ref currentSelection);
                                break;
                            case ConsoleKey.DownArrow:
                                SelectionGoDown(ref currentSelection, selections);
                                break;
                            case ConsoleKey.Home:
                                goingUp = true;
                                SelectionSet(ref currentSelection, selections, 0);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref currentSelection, selections, selections.Length - 1);
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int currentPageMove = (currentSelection - 1) / selectionChoices;
                                    int startIndexMove = selectionChoices * currentPageMove;
                                    SelectionSet(ref currentSelection, selections, startIndexMove);
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / selectionChoices;
                                    int startIndexMove = selectionChoices * (currentPageMove + 1);
                                    SelectionSet(ref currentSelection, selections, startIndexMove);
                                }
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = selections[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.A:
                                bool unselect = selectedChoices.Count == enabledAnswers.Count();
                                if (unselect)
                                    selectedChoices.Clear();
                                else if (selectedChoices.Count == 0)
                                    selectedChoices.AddRange(enabledAnswers);
                                else
                                {
                                    // We need to use Except here to avoid wasting CPU cycles, since we could be dealing with huge data.
                                    var unselected = enabledAnswers.Except(selectedChoices);
                                    selectedChoices.AddRange(unselected);
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = selections.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (supports regular expressions)");
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal("Your query is not a valid regular expression.");
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                    break;
                                }
                                var regex = new Regex(keyword);
                                var resultEntries = entriesString
                                    .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, idx))
                                    .Where((entry) => regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)).ToArray();
                                if (resultEntries.Length > 0)
                                {
                                    var choices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.ChoiceName, tuple.ChoiceTitle)).ToArray();
                                    int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Select one of the entries:");
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal("No item found.");
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.E:
                                GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                GoDown(ref currIdx, text, vars, increment);
                                break;
                            case ConsoleKey.W:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                GoDown(ref currIdx, text, vars);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(keybindings);
                                break;

                        }
                    }

                    // Verify that the current position is not a disabled choice
                    InfoBoxTools.VerifyDisabled(ref currentSelection, selections, goingUp);
                }
                if (cancel)
                    selectedChoices.Clear();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (useColor)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }

            // Return the selected choices
            selectedChoices.Sort();
            return [.. selectedChoices];
        }

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(ref int currIdx, string text, object[] vars, int level = 1)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, maxHeight, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines, 5);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void SelectionGoUp(ref int currentSelection)
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = 0;
        }

        private static void SelectionGoDown(ref int currentSelection, InputChoiceInfo[] selections)
        {
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = selections.Length - 1;
        }

        private static void SelectionSet(ref int currentSelection, InputChoiceInfo[] selections, int value)
        {
            currentSelection = value;
            if (currentSelection > selections.Length - 1)
                currentSelection = selections.Length - 1;
            if (currentSelection < 0)
                currentSelection = 0;
        }

        static InfoBoxSelectionMultipleColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
