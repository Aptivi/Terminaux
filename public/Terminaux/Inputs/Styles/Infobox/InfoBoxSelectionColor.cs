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
using Terminaux.Writer.CyclicWriters;
using Selections = Terminaux.Writer.CyclicWriters.Selection;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with selection and color support
    /// </summary>
    public static class InfoBoxSelectionColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Goes one choice up (next choice)", ConsoleKey.UpArrow),
            new Keybinding("Goes one choice down (previous choice)", ConsoleKey.DownArrow),
            new Keybinding("Goes to the first choice", ConsoleKey.Home),
            new Keybinding("Goes to the last choice", ConsoleKey.End),
            new Keybinding("Goes to the previous page of choices", ConsoleKey.PageUp),
            new Keybinding("Goes to the next page of choices", ConsoleKey.PageDown),
            new Keybinding("Shows more info in an infobox", ConsoleKey.Tab),
            new Keybinding("Searches for a choice", ConsoleKey.F),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Submits the value", ConsoleKey.Enter),
            new Keybinding("Closes without submitting the value", ConsoleKey.Escape),
        ];

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionPlain("", selections, text, settings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColor(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, BorderSettings.GlobalSettings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxSelectionColor">InfoBoxSelection color</param>
        /// <param name="BackgroundColor">InfoBoxSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                "", selections, text, settings, InfoBoxSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionPlain(title, selections, text, BorderSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionPlain(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                title, selections, text, settings, ColorTools.currentForegroundColor, ColorTools.currentBackgroundColor, false, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColor(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, BorderSettings.GlobalSettings, InfoBoxTitledSelectionColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(title, selections, text, settings, new Color(ConsoleColors.Silver), ColorTools.currentBackgroundColor, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionColor, Color BackgroundColor, params object[] vars) =>
            WriteInfoBoxSelectionColorBack(
                title, selections, text, settings, InfoBoxTitledSelectionColor, BackgroundColor, true, vars);

        /// <summary>
        /// Writes the info box plainly
        /// </summary>
        /// <param name="title">Title to be written</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Border settings to use</param>
        /// <param name="InfoBoxTitledSelectionColor">InfoBoxTitledSelection color</param>
        /// <param name="BackgroundColor">InfoBoxTitledSelection background color</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="useColor">Whether to use color or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        internal static int WriteInfoBoxSelectionColorBack(string title, InputChoiceInfo[] selections, string text, BorderSettings settings, Color InfoBoxTitledSelectionColor, Color BackgroundColor, bool useColor, params object[] vars)
        {
            int selectedChoice = -1;
            int AnswerTitleLeft = selections.Max(x => $"  {x.ChoiceName}) ".Length);

            // First, verify that we have selections
            if (selections is null || selections.Length == 0)
                return selectedChoice;

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
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionColor), infoBoxScreenPart);
            try
            {
                // Modify the current selection according to the default
                int currentSelection = selections.Any((ici) => ici.ChoiceDefault) ? selections.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;
                int selectionChoices = selections.Length > 10 ? 10 : selections.Length;

                // Edge case: We need to check to see if the current highlight is disabled
                while (selections[currentSelection].ChoiceDisabled)
                {
                    currentSelection++;
                    if (currentSelection > selections.Length - 1)
                        currentSelection = 0;
                }

                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY, selectionBoxPosX, selectionBoxPosY, _, maxSelectionWidth, _, _) = InfoBoxTools.GetDimensions(selections, splitFinalLines);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(selections, title, text, settings, InfoBoxTitledSelectionColor, BackgroundColor, useColor, ref increment, currIdx, true, vars)
                    );

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY - 1,
                        InteriorWidth = maxSelectionWidth,
                        InteriorHeight = selectionChoices,
                        Settings = settings,
                        Color = InfoBoxTitledSelectionColor,
                    };
                    boxBuffer.Append(border.Render());

                    // Now, render the selections
                    var selectionsRendered = new Selections(selections)
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY,
                        CurrentSelection = currentSelection,
                        Height = selectionChoices,
                        Width = maxSelectionWidth,
                        SliderInside = true,
                        AltChoicePos = selections.Length,
                        SwapSelectedColors = true,
                        Settings = new()
                        {
                            OptionColor = InfoBoxTitledSelectionColor,
                            SelectedOptionColor = InfoBoxTitledSelectionColor,
                            BackgroundColor = BackgroundColor,
                        }
                    };
                    boxBuffer.Append(
                        selectionsRendered.Render()
                    );

                    // Return the buffer
                    return boxBuffer.ToString();
                });

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
                    var (maxWidth, maxHeight, _, borderX, borderY, _, selectionBoxPosY, leftPos, maxSelectionWidth, _, selectionReservedHeight) = InfoBoxTools.GetDimensions(selections, splitFinalLines);
                    maxHeight -= selectionReservedHeight;
                    if (Input.MouseInputAvailable)
                    {
                        bool UpdatePositionBasedOnMouse(PointerEventContext mouse)
                        {
                            // Make pages based on console window height
                            int currentPage = currentSelection / selectionChoices;
                            int startIndex = selectionChoices * currentPage;

                            // Now, translate coordinates to the selected index
                            if (!PointerTools.PointerWithinRange(mouse,
                                    (leftPos, selectionBoxPosY),
                                    (maxSelectionWidth, selectionBoxPosY + selectionChoices)))
                                return false;
                            int listIndex = mouse.Coordinates.y - selectionBoxPosY;
                            listIndex = startIndex + listIndex;
                            listIndex = listIndex >= selections.Length ? selections.Length - 1 : listIndex;
                            currentSelection = listIndex;
                            return true;
                        }

                        bool DetermineTextArrowPressed(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= maxHeight)
                                return false;
                            int arrowLeft = maxWidth + borderX + 1;
                            int arrowTop = 2;
                            int arrowBottom = maxHeight + 1;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (arrowLeft, arrowTop),
                                    (arrowLeft, arrowBottom));
                        }

                        void UpdatePositionBasedOnTextArrowPress(PointerEventContext mouse)
                        {
                            if (splitFinalLines.Length <= maxHeight)
                                return;
                            int arrowLeft = maxWidth + borderX + 1;
                            int arrowTop = 2;
                            int arrowBottom = maxHeight + 1;
                            if (mouse.Coordinates.x == arrowLeft)
                            {
                                if (mouse.Coordinates.y == arrowTop)
                                {
                                    currIdx -= 1;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                                else if (mouse.Coordinates.y == arrowBottom)
                                {
                                    currIdx += 1;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                            }
                        }

                        bool DetermineSelectionArrowPressed(PointerEventContext mouse)
                        {
                            // Make pages based on console window height
                            int currentPage = currentSelection / selectionChoices;
                            int startIndex = selectionChoices * currentPage;

                            // Now, translate coordinates to the selected index
                            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                            var (_, _, _, _, _, _, selectionBoxPosY, _, _, left, _) = InfoBoxTools.GetDimensions(selections, splitFinalLines);
                            if (selections.Length <= selectionChoices)
                                return false;
                            return
                                mouse.Coordinates.x == left &&
                                (mouse.Coordinates.y == selectionBoxPosY || mouse.Coordinates.y == ConsoleWrapper.WindowHeight - selectionChoices);
                        }

                        void UpdatePositionBasedOnSelectionArrowPress(PointerEventContext mouse)
                        {
                            // Make pages based on console window height
                            int currentPage = currentSelection / selectionChoices;
                            int startIndex = selectionChoices * currentPage;

                            // Now, translate coordinates to the selected index
                            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                            var (_, _, _, _, _, _, selectionBoxPosY, _, _, left, _) = InfoBoxTools.GetDimensions(selections, splitFinalLines);
                            if (selections.Length <= selectionChoices)
                                return;
                            if (mouse.Coordinates.x == left)
                            {
                                if (mouse.Coordinates.y == selectionBoxPosY)
                                {
                                    goingUp = true;
                                    currentSelection--;
                                    if (currentSelection < 0)
                                        currentSelection = 0;
                                }
                                else if (mouse.Coordinates.y == ConsoleWrapper.WindowHeight - selectionChoices)
                                {
                                    currentSelection++;
                                    if (currentSelection > selections.Length)
                                        currentSelection = selections.Length;
                                }
                            }
                        }

                        bool DetermineButtonsPressed(PointerEventContext mouse)
                        {
                            string buttons = InfoBoxTools.GetButtons(settings);
                            int buttonsWidth = ConsoleChar.EstimateCellWidth(buttons);
                            int buttonsLeftMin = maxWidth + borderX - buttonsWidth;
                            int buttonsLeftMax = buttonsLeftMin + buttonsWidth;
                            int buttonsTop = borderY;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (buttonsLeftMin, buttonsTop),
                                    (buttonsLeftMax, buttonsTop));
                        }

                        void DoActionBasedOnButtonPress(PointerEventContext mouse)
                        {
                            string buttons = InfoBoxTools.GetButtons(settings);
                            int buttonsWidth = ConsoleChar.EstimateCellWidth(buttons);
                            int buttonLeftHelpMin = maxWidth + borderX - buttonsWidth;
                            int buttonLeftHelpMax = buttonLeftHelpMin + 2;
                            int buttonLeftCloseMin = buttonLeftHelpMin + 3;
                            int buttonLeftCloseMax = buttonLeftHelpMin + buttonsWidth;
                            int buttonsTop = borderY;
                            if (mouse.Coordinates.y == buttonsTop)
                            {
                                if (PointerTools.PointerWithinRange(mouse, (buttonLeftHelpMin, buttonsTop), (buttonLeftHelpMax, buttonsTop)))
                                    KeybindingTools.ShowKeybindingInfobox(keybindings);
                                else if (PointerTools.PointerWithinRange(mouse, (buttonLeftCloseMin, buttonsTop), (buttonLeftCloseMax, buttonsTop)))
                                {
                                    bail = true;
                                    cancel = true;
                                }
                            }
                        }

                        // Mouse input received.
                        var mouse = Input.ReadPointer();
                        if (mouse is null)
                            continue;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                {
                                    currIdx -= 3;
                                    if (currIdx < 0)
                                        currIdx = 0;
                                }
                                else
                                {
                                    goingUp = true;
                                    currentSelection--;
                                    if (currentSelection < 0)
                                        currentSelection = selections.Length - 1;
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (mouse.Modifiers == PointerModifiers.Shift)
                                {
                                    currIdx += 3;
                                    if (currIdx > splitFinalLines.Length - maxHeight)
                                        currIdx = splitFinalLines.Length - maxHeight;
                                }
                                else
                                {
                                    currentSelection++;
                                    if (currentSelection > selections.Length - 1)
                                        currentSelection = 0;
                                }
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineSelectionArrowPressed(mouse))
                                    UpdatePositionBasedOnSelectionArrowPress(mouse);
                                else if (DetermineTextArrowPressed(mouse))
                                    UpdatePositionBasedOnTextArrowPress(mouse);
                                else if (DetermineButtonsPressed(mouse))
                                    DoActionBasedOnButtonPress(mouse);
                                else
                                {
                                    UpdatePositionBasedOnMouse(mouse);
                                    bail = true;
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
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
                                UpdatePositionBasedOnMouse(mouse);
                                break;
                        }
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = selections.Length - 1;
                                break;
                            case ConsoleKey.DownArrow:
                                currentSelection++;
                                if (currentSelection > selections.Length - 1)
                                    currentSelection = 0;
                                break;
                            case ConsoleKey.Home:
                                currentSelection = 0;
                                break;
                            case ConsoleKey.End:
                                currentSelection = selections.Length - 1;
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int currentPageMove = (currentSelection - 1) / selectionChoices;
                                    int startIndexMove = selectionChoices * currentPageMove;
                                    currentSelection = startIndexMove;
                                    if (currentSelection < 0)
                                        currentSelection = 0;
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / selectionChoices;
                                    int startIndexMove = selectionChoices * (currentPageMove + 1);
                                    currentSelection = startIndexMove;
                                    if (currentSelection > selections.Length - 1)
                                        currentSelection = selections.Length - 1;
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
                                    int answer = WriteInfoBoxSelection(choices, "Select one of the entries:");
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal("No item found.");
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.E:
                                currIdx -= maxHeight;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.D:
                                currIdx += increment;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
                                break;
                            case ConsoleKey.W:
                                currIdx -= 1;
                                if (currIdx < 0)
                                    currIdx = 0;
                                break;
                            case ConsoleKey.S:
                                currIdx += 1;
                                if (currIdx > splitFinalLines.Length - maxHeight)
                                    currIdx = splitFinalLines.Length - maxHeight;
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
                    if (currentSelection >= 0)
                    {
                        while (selections[currentSelection].ChoiceDisabled)
                        {
                            if (goingUp)
                            {
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = selections.Length - 1;
                            }
                            else
                            {
                                currentSelection++;
                                if (currentSelection > selections.Length - 1)
                                    currentSelection = 0;
                            }
                        }
                    }
                }
                if (!cancel)
                    selectedChoice = currentSelection;
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

            // Return the selected choice, or -1
            return selectedChoice;
        }

        static InfoBoxSelectionColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
