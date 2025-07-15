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
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Multi combo box module
    /// </summary>
    public class MultiComboBoxModule : InputModule
    {
        private int selectedChoice = -1;
        private Selection selection = new();
        private InputChoiceCategoryInfo[] choices = [];

        /// <summary>
        /// Choices to render
        /// </summary>
        public InputChoiceCategoryInfo[] Choices
        {
            get => choices;
            set
            {
                choices = value;
                selection = new Selection(Choices);
                selectedChoice = -1;
            }
        }

        /// <inheritdoc/>
        public override int ExtraPopoverHeight { get; protected set; } = 10;

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            int[] indexes = Value is not null ? (int[])Value : [];
            string valueString = $"{indexes.Length} items selected";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($"▼ {valueString}", width);
            valueString = wrappedValue[0];

            // Determine how many underscores we need to render
            int valueWidth = ConsoleChar.EstimateCellWidth(valueString);
            int diffWidth = width - valueWidth;
            string underscores = new('_', diffWidth);

            // Render the text box contents now
            string textBox =
                ColorTools.RenderSetConsoleColor(Foreground) +
                ColorTools.RenderSetConsoleColor(Background, true) +
                valueString +
                ColorTools.RenderSetConsoleColor(BlankForeground) +
                underscores +
                ColorTools.RenderRevertForeground() +
                ColorTools.RenderRevertBackground();
            return textBox;
        }

        /// <inheritdoc/>
        public override void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default)
        {
            // Determine selected choices
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(Choices)];
            int[] indexes = Value is not null ? (int[])Value : SelectionInputTools.SelectDefaults(choices);
            int currentSelection = selectedChoice < 0 ? SelectionInputTools.GetDefaultChoice(choices) : selectedChoice;
            selectedChoice = currentSelection;

            // Check to see if we need a popover
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                Value = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(indexes, currentSelection, Choices, Description, new InfoBoxSettings()
                {
                    Title = Name,
                    ForegroundColor = Foreground,
                    BackgroundColor = Background,
                });
            }
            else
            {
                List<int> selectedChoices = [.. indexes];

                // Make selected choices from the ChoiceDefaultSelected value.
                InfoBoxTools.VerifyDisabled(ref currentSelection, choices);
                int finalPopoverHeight = inputPopoverSize.Height > ConsoleWrapper.WindowHeight - inputPopoverPos.Y - 2 ? ConsoleWrapper.WindowHeight - inputPopoverPos.Y - 2 : inputPopoverSize.Height;
                bool goingUp = false;
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the popover. A box will appear under the selection, just like combo-boxes.
                    var comboSelectionBoxFrame = new BoxFrame()
                    {
                        Left = inputPopoverPos.X,
                        Top = inputPopoverPos.Y,
                        Width = inputPopoverSize.Width - 2,
                        Height = finalPopoverHeight,
                        FrameColor = Foreground,
                        BackgroundColor = Background,
                    };
                    selection.Left = inputPopoverPos.X + 1;
                    selection.Top = inputPopoverPos.Y + 1;
                    selection.Width = inputPopoverSize.Width - 2;
                    selection.Height = finalPopoverHeight;
                    selection.CurrentSelections = [.. selectedChoices];
                    selection.CurrentSelection = currentSelection;
                    selection.SwapSelectedColors = true;
                    selection.Settings = new()
                    {
                        OptionColor = Foreground,
                        SelectedOptionColor = Foreground,
                        BackgroundColor = Background,
                    };
                    TextWriterRaw.WriteRaw(
                        comboSelectionBoxFrame.Render() +
                        selection.Render()
                    );

                    // Prompt user for choice selection
                    InputEventInfo data = Input.ReadPointerOrKey();
                    var related = selection.GetRelatedHeights();
                    int selectionBoxPosX = inputPopoverPos.X + 1;
                    int selectionBoxPosY = inputPopoverPos.Y + 1;
                    int maxSelectionWidth = inputPopoverSize.Width - 2;
                    int arrowSelectLeft = selectionBoxPosX + maxSelectionWidth;
                    int selectionReservedHeight = finalPopoverHeight - 1;
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Make hitboxes for arrow presses
                        var arrowSelectUpHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY), new Action<PointerEventContext>((_) => SelectionGoUp(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowSelectDownHitbox = new PointerHitbox(new(arrowSelectLeft, selectionBoxPosY + selectionReservedHeight), new Action<PointerEventContext>((_) => SelectionGoDown(ref currentSelection, choices))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                        // Mouse input received.
                        ChoiceHitboxType hitboxType = ChoiceHitboxType.Choice;
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (IsMouseWithinInputBox(selectionBoxPosX, selectionBoxPosY, maxSelectionWidth, selectionReservedHeight, mouse))
                                {
                                    goingUp = true;
                                    SelectionGoUp(ref currentSelection, choices);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinInputBox(selectionBoxPosX, selectionBoxPosY, maxSelectionWidth, selectionReservedHeight, mouse))
                                    SelectionGoDown(ref currentSelection, choices);
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowSelectUpHitbox.IsPointerWithin(mouse) || arrowSelectDownHitbox.IsPointerWithin(mouse)) && related.Count > selectionReservedHeight)
                                {
                                    arrowSelectUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowSelectDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else
                                {
                                    int oldIndex = currentSelection;
                                    if (InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, Choices, selectionBoxPosX - 1, selectionBoxPosY, maxSelectionWidth, finalPopoverHeight, out hitboxType, ref currentSelection, false))
                                    {
                                        switch (hitboxType)
                                        {
                                            case ChoiceHitboxType.Category:
                                                InfoBoxTools.ProcessSelectionRequest(2, currentSelection + 1, Choices, ref selectedChoices);
                                                currentSelection = oldIndex;
                                                break;
                                            case ChoiceHitboxType.Group:
                                                InfoBoxTools.ProcessSelectionRequest(1, currentSelection + 1, Choices, ref selectedChoices);
                                                currentSelection = oldIndex;
                                                break;
                                            case ChoiceHitboxType.Choice:
                                                InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
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
                                if (!InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, Choices, selectionBoxPosX - 1, selectionBoxPosY, maxSelectionWidth, finalPopoverHeight, out hitboxType, ref currentSelection))
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
                                InfoBoxTools.UpdateSelectedIndexWithMousePos(mouse, Choices, selectionBoxPosX - 1, selectionBoxPosY, maxSelectionWidth, finalPopoverHeight, out hitboxType, ref currentSelection);
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = choices.Length - 1;
                                break;
                            case ConsoleKey.DownArrow:
                                currentSelection++;
                                if (currentSelection > choices.Length - 1)
                                    currentSelection = 0;
                                break;
                            case ConsoleKey.Home:
                                currentSelection = 0;
                                break;
                            case ConsoleKey.End:
                                currentSelection = choices.Length - 1;
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int currentPageMove = (currentSelection - 1) / finalPopoverHeight;
                                    int startIndexMove = finalPopoverHeight * currentPageMove;
                                    currentSelection = startIndexMove;
                                    if (currentSelection < 0)
                                        currentSelection = 0;
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int currentPageMove = currentSelection / finalPopoverHeight;
                                    int startIndexMove = finalPopoverHeight * (currentPageMove + 1);
                                    currentSelection = startIndexMove;
                                    if (currentSelection > choices.Length - 1)
                                        currentSelection = choices.Length - 1;
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
                                        Title = $"[{choiceName}] {choiceTitle}",
                                        ForegroundColor = Foreground,
                                        BackgroundColor = Background,
                                    });
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                if (!selectedChoices.Remove(currentSelection))
                                    selectedChoices.Add(currentSelection);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                        }
                    }

                    // Verify that the current position is not a disabled choice
                    InfoBoxTools.VerifyDisabled(ref currentSelection, choices, goingUp);
                }
                if (!cancel)
                    Value = selectedChoices.ToArray();
                selectedChoice = currentSelection;
            }
            Provided = true;
        }

        private static bool IsMouseWithinInputBox(int selectionBoxPosX, int selectionBoxPosY, int maxSelectionWidth, int reservedHeight, PointerEventContext mouse)
        {
            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (selectionBoxPosX, selectionBoxPosY), (selectionBoxPosX + maxSelectionWidth, selectionBoxPosY + reservedHeight));
        }

        private static void SelectionGoUp(ref int currentSelection, InputChoiceInfo[] selections)
        {
            currentSelection--;
            if (currentSelection < 0)
                currentSelection = selections.Length - 1;
        }

        private static void SelectionGoDown(ref int currentSelection, InputChoiceInfo[] selections)
        {
            currentSelection++;
            if (currentSelection > selections.Length - 1)
                currentSelection = 0;
        }
    }
}
