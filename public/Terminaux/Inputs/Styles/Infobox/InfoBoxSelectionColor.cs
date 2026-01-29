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
using Terminaux.Writer.ConsoleWriters;
using System.Linq;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Textify.Tools;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Selections = Terminaux.Writer.CyclicWriters.Graphical.Selection;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Base.Structures;
using Terminaux.Inputs.Styles.Selection;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with selection and color support
    /// </summary>
    public static class InfoBoxSelectionColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_GOUP"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_GODOWN"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_FIRST"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_LAST"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_PREVPAGECHOICES"), ConsoleKey.PageUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_NEXTPAGECHOICES"), ConsoleKey.PageDown),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), ConsoleKey.Tab),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_SELECTION_COMMON_KEYBINDING_SEARCHCHOICES"), ConsoleKey.F),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_SINGULAR"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_KEYBINDING_CHANGEVALUERADIO"), ConsoleKey.Spacebar),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_SINGULAR"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MOREINFO"), PointerButton.Right),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELUPCHOICE"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELDOWNCHOICE"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, params object[] vars)
        {
            int defaultSelection = SelectionInputTools.GetDefaultChoice(selections);
            return WriteInfoBoxSelection(defaultSelection, defaultSelection, selections, text, InfoBoxSettings.GlobalSettings, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            int defaultSelection = SelectionInputTools.GetDefaultChoice(selections);
            return WriteInfoBoxSelection(defaultSelection, defaultSelection, selections, text, settings, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(int currentSelection, int currentSelected, InputChoiceInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelection(currentSelection, currentSelected, selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(int currentSelection, int currentSelected, InputChoiceInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            var category = new InputChoiceCategoryInfo[]
            {
                new("Selection infobox", [new("Available options", selections)])
            };
            return WriteInfoBoxSelection(currentSelection, currentSelected, category, text, settings, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceCategoryInfo[] selections, string text, params object[] vars)
        {
            int defaultSelection = SelectionInputTools.GetDefaultChoice(selections);
            return WriteInfoBoxSelection(defaultSelection, defaultSelection, selections, text, InfoBoxSettings.GlobalSettings, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(InputChoiceCategoryInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            int defaultSelection = SelectionInputTools.GetDefaultChoice(selections);
            return WriteInfoBoxSelection(defaultSelection, defaultSelection, selections, text, settings, vars);
        }

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <param name="selections">List of choices</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(int currentSelection, int currentSelected, InputChoiceCategoryInfo[] selections, string text, params object[] vars) =>
            WriteInfoBoxSelection(currentSelection, currentSelected, selections, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the single-choice selection info box
        /// </summary>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <param name="selections">List of choices</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <returns>Selected choice index (starting from zero), or -1 if exited, selection list is empty, or an error occurred</returns>
        public static int WriteInfoBoxSelection(int currentSelection, int currentSelected, InputChoiceCategoryInfo[] selections, string text, InfoBoxSettings settings, params object[] vars)
        {
            int selectedChoice = -1;
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(selections)];

            // First, verify that we have selections
            if (choices is null || choices.Length == 0)
                return selectedChoice;

            // We need not to run the selection style when everything is disabled
            bool allDisabled = choices.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException("The infobox selection style requires that there is at least one choice enabled.");

            // Prepare the screen
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxSelectionColor), infoBoxScreenPart);

            // Make a new infobox instance
            var selectionsRendered = new Selections(selections)
            {
                ShowRadioButtons = settings.RadioButtons,
                CurrentSelection = currentSelection,
                SelectedChoice = currentSelected,
                SwapSelectedColors = true,
                UseColors = settings.UseColors,
                Settings = new()
                {
                    OptionColor = settings.ForegroundColor,
                    SelectedOptionColor = settings.ForegroundColor,
                    BackgroundColor = settings.BackgroundColor,
                }
            };
            var related = selectionsRendered.GetRelatedHeights();
            int selectionChoices = related.Count > 10 ? 10 : related.Count;
            int selectionReservedHeight = 2 + selectionChoices;
            var infoBox = new InfoBox()
            {
                Settings = new(settings)
                {
                    Positioning = new(InfoBoxPositioning.GlobalSettings)
                },
                Text = text.FormatString(vars),
            };
            infoBox.Settings.Positioning.ExtraHeight = selectionReservedHeight;

            // Now, some logic to get the informational box ready
            try
            {
                // Edge case: We need to check to see if the current highlight is disabled
                InfoBoxTools.VerifyDisabled(ref currentSelection, choices);

                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Get the absolute infobox positioning dimensions according to the width of the selection
                    infoBox.Elements.RemoveRenderables();
                    infoBox.Settings.Positioning.ExtraWidth = 0;
                    var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
                    if (selectionsRendered.choiceTexts.Count == 0)
                        selectionsRendered.choiceTexts = selectionsRendered.GetChoiceParameters();
                    int maxChoiceTextsWidth = selectionsRendered.choiceTexts.Max((ct) => ConsoleChar.EstimateCellWidth(ct) + 4);
                    infoBox.Settings.Positioning.ExtraWidth = maxChoiceTextsWidth > maxWidth ? maxChoiceTextsWidth - maxWidth : 0;
                    (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
                    int selectionBoxPosX = borderX + 2;
                    int selectionBoxPosY = borderY + maxTextHeight + 1;
                    int maxSelectionWidth = maxWidth - 4;

                    // Buffer the selection box
                    var border = new Border()
                    {
                        Left = selectionBoxPosX,
                        Top = selectionBoxPosY,
                        Width = maxSelectionWidth,
                        Height = selectionChoices,
                        Settings = settings.BorderSettings,
                        UseColors = settings.UseColors,
                        Color = settings.ForegroundColor,
                        BackgroundColor = settings.BackgroundColor,
                    };
                    infoBox.Elements.AddRenderable("Selection box", border);

                    // Now, render the selections
                    selectionsRendered.Left = selectionBoxPosX + 1;
                    selectionsRendered.Top = selectionBoxPosY + 1;
                    selectionsRendered.Width = maxSelectionWidth;
                    selectionsRendered.Height = selectionChoices;
                    selectionsRendered.CurrentSelection = currentSelection;
                    selectionsRendered.SelectedChoice = currentSelected;
                    infoBox.Elements.AddRenderable("Rendered selections", selectionsRendered);
                    return infoBox.Render(ref increment, currIdx, true, true);
                });

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, maxTextHeight, linesLength) = infoBox.Dimensions;
                    int selectionBoxPosX = borderX + 2;
                    int selectionBoxPosY = borderY + maxTextHeight + 1;
                    int maxSelectionWidth = maxWidth - 4;
                    int arrowSelectLeft = selectionBoxPosX + maxWidth - 3;

                    // Handle input
                    bool goingUp = false;
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Get positions for arrows
                        int arrowLeft = maxWidth + borderX + 1;
                        int arrowTop = borderY + 1;
                        int arrowBottom = borderY + maxTextHeight;

                        // Get positions for infobox buttons
                        string infoboxButtons = InfoBoxTools.GetButtons(settings.BorderSettings);
                        int infoboxButtonsWidth = ConsoleChar.EstimateCellWidth(infoboxButtons);
                        int infoboxButtonLeftHelpMin = maxWidth + borderX - infoboxButtonsWidth;
                        int infoboxButtonLeftHelpMax = infoboxButtonLeftHelpMin + 2;
                        int infoboxButtonLeftCloseMin = infoboxButtonLeftHelpMin + 3;
                        int infoboxButtonLeftCloseMax = infoboxButtonLeftHelpMin + infoboxButtonsWidth;
                        int infoboxButtonsTop = borderY;

                        // Make hitboxes for arrow and button presses
                        var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => InfoBoxTools.GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => InfoBoxTools.GoDown(ref currIdx, infoBox))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowSelectUpHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY + 1), new Action<PointerEventContext>((_) => SelectionGoUp(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowSelectDownHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY + selectionChoices), new Action<PointerEventContext>((_) => SelectionGoDown(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var infoboxButtonHelpHitbox = new PointerHitbox(new(infoboxButtonLeftHelpMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftHelpMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => KeybindingTools.ShowKeybindingInfobox(Keybindings))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var infoboxButtonCloseHitbox = new PointerHitbox(new(infoboxButtonLeftCloseMin, infoboxButtonsTop), new Coordinate(infoboxButtonLeftCloseMax, infoboxButtonsTop), new Action<PointerEventContext>((_) => cancel = bail = true)) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                        // Mouse input received.
                        ChoiceHitboxType hitboxType = ChoiceHitboxType.Choice;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoUp(ref currIdx, 3);
                                else if (IsMouseWithinInputBox(selectionBoxPosX, selectionBoxPosY, maxSelectionWidth, selectionReservedHeight, mouse))
                                {
                                    goingUp = true;
                                    SelectionGoUp(ref currentSelection, choices, 3);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (InfoBoxTools.IsMouseWithinText(infoBox, mouse))
                                    InfoBoxTools.GoDown(ref currIdx, infoBox, 3);
                                else if (IsMouseWithinInputBox(selectionBoxPosX, selectionBoxPosY, maxSelectionWidth, selectionReservedHeight, mouse))
                                    SelectionGoDown(ref currentSelection, choices, 3);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && linesLength > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowSelectUpHitbox.IsPointerWithin(mouse) || arrowSelectDownHitbox.IsPointerWithin(mouse)) && related.Count > selectionChoices)
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
                                    if (!InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, infoBox, out hitboxType, ref currentSelection))
                                        break;
                                    if (hitboxType != ChoiceHitboxType.Choice)
                                        break;
                                    if (mouse.ClickTier > 1)
                                        bail = true;
                                    else if (settings.RadioButtons)
                                        currentSelected = currentSelection;
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (!InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, infoBox, out hitboxType, ref currentSelection))
                                    break;
                                if (hitboxType != ChoiceHitboxType.Choice)
                                    break;
                                var selectedInstance = choices[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}",
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, selections, infoBox, out hitboxType, ref currentSelection);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                SelectionGoUp(ref currentSelection, choices);
                                break;
                            case ConsoleKey.DownArrow:
                                SelectionGoDown(ref currentSelection, choices);
                                break;
                            case ConsoleKey.Home:
                                goingUp = true;
                                SelectionSet(ref currentSelection, choices, 0);
                                break;
                            case ConsoleKey.End:
                                SelectionSet(ref currentSelection, choices, choices.Length - 1);
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    // Use the rendered selection heights to go to the previous page
                                    int projectedSelection = currentSelection;
                                    int processedHeight = 0;
                                    var heights = selectionsRendered.GetSelectionHeights();
                                    for (int h = projectedSelection; h > 0 && processedHeight < selectionChoices; h--)
                                    {
                                        int height = heights[h];
                                        int prevHeight = h - 1 < heights.Count ? heights[h - 1] : 0;
                                        projectedSelection--;
                                        if (projectedSelection < 0)
                                            projectedSelection = 0;
                                        processedHeight += height - prevHeight;
                                    }
                                    SelectionSet(ref currentSelection, choices, projectedSelection);
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    // Use the rendered selection heights to go to the next page
                                    int projectedSelection = currentSelection;
                                    int processedHeight = 0;
                                    var heights = selectionsRendered.GetSelectionHeights();
                                    for (int h = projectedSelection; h < heights.Count && processedHeight < selectionChoices; h++)
                                    {
                                        int height = heights[h];
                                        int nextHeight = h + 1 < heights.Count ? heights[h + 1] : 0;
                                        projectedSelection++;
                                        if (currentSelection > selections.Length - 1)
                                            currentSelection = selections.Length - 1;
                                        processedHeight += nextHeight - height > 0 ? nextHeight - height : 1;
                                    }
                                    SelectionSet(ref currentSelection, choices, projectedSelection);
                                }
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = choices[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(choiceDesc, new InfoBoxSettings()
                                    {
                                        Title = $"[{choiceName}] {choiceTitle}"
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (selectionChoices <= 0)
                                    break;
                                var entriesString = choices.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"));
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                    break;
                                }
                                var regex = new Regex(keyword);
                                var resultEntries = entriesString
                                    .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled, idx))
                                    .Where((entry) => (regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)) && !entry.ChoiceDisabled).ToArray();
                                if (resultEntries.Length > 1)
                                {
                                    var resultChoices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.ChoiceName, tuple.ChoiceTitle)).ToArray();
                                    int answer = WriteInfoBoxSelection(resultChoices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"));
                                    if (answer < 0)
                                        break;
                                    currentSelection = resultEntries[answer].idx;
                                }
                                else if (resultEntries.Length == 1)
                                    currentSelection = resultEntries[0].idx;
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"));
                                ScreenTools.CurrentScreen?.RequireRefresh();
                                break;
                            case ConsoleKey.E:
                                InfoBoxTools.GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                InfoBoxTools.GoDown(ref currIdx, infoBox, increment);
                                break;
                            case ConsoleKey.W:
                                InfoBoxTools.GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                InfoBoxTools.GoDown(ref currIdx, infoBox);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Spacebar:
                                if (settings.RadioButtons)
                                    currentSelected = currentSelection;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(Keybindings);
                                break;

                        }
                    }

                    // Verify that the current position is not a disabled choice
                    InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
                }
                if (!cancel)
                {
                    if (settings.RadioButtons)
                        selectedChoice = currentSelected;
                    else
                        selectedChoice = currentSelection;
                }
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (settings.UseColors)
                {
                    TextWriterRaw.WriteRaw(
                        ConsoleColoring.RenderRevertForeground() +
                        ConsoleColoring.RenderRevertBackground()
                    );
                }
                TextWriterRaw.WriteRaw(infoBox.Erase());
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }

            // Return the selected choice, or -1
            return selectedChoice;
        }

        private static bool IsMouseWithinInputBox(int selectionBoxPosX, int selectionBoxPosY, int maxSelectionWidth, int reservedHeight, PointerEventContext mouse)
        {
            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (selectionBoxPosX + 1, selectionBoxPosY), (selectionBoxPosX + maxSelectionWidth, selectionBoxPosY + reservedHeight - 3));
        }

        private static void SelectionGoUp(ref int currentSelection, InputChoiceInfo[] selections, int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            currentSelection -= factor;
            if (currentSelection < 0)
                currentSelection = factor == 1 ? selections.Length - 1 : 0;
        }

        private static void SelectionGoDown(ref int currentSelection, InputChoiceInfo[] selections, int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            currentSelection += factor;
            if (currentSelection > selections.Length - 1)
                currentSelection = factor == 1 ? 0 : selections.Length - 1;
        }

        private static void SelectionSet(ref int currentSelection, InputChoiceInfo[] selections, int value)
        {
            currentSelection = value;
            if (currentSelection > selections.Length - 1)
                currentSelection = selections.Length - 1;
            if (currentSelection < 0)
                currentSelection = 0;
        }
    }
}
