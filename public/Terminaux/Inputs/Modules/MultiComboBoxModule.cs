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
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Multi combo box module
    /// </summary>
    public class MultiComboBoxModule : InputModule
    {
        /// <summary>
        /// Choices to render
        /// </summary>
        public InputChoiceCategoryInfo[] Choices { get; set; } = [];

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            int[] indexes = Value is not null ? (int[])Value : [];
            string valueString = $"{indexes.Length} items selected";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($" ▼ {valueString}", width);
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
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                Value = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(Choices, Description, new InfoBoxSettings()
                {
                    Title = Name,
                    ForegroundColor = Foreground,
                    BackgroundColor = Background,
                });
            }
            else
            {
                List<int> selectedChoices = [];
                InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(Choices)];
                int currentSelection = choices.Any((ici) => ici.ChoiceDefault) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : 0;

                // Make selected choices from the ChoiceDefaultSelected value.
                InfoBoxTools.VerifyDisabled(ref currentSelection, choices);
                selectedChoices = choices.Any((ici) => ici.ChoiceDefaultSelected) ? choices.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx).ToList() : [];
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
                    var comboSelections = new Selection(Choices)
                    {
                        Left = inputPopoverPos.X + 1,
                        Top = inputPopoverPos.Y + 1,
                        Width = inputPopoverSize.Width - 2,
                        Height = finalPopoverHeight,
                        CurrentSelections = [.. selectedChoices],
                        CurrentSelection = currentSelection,
                        Settings = new()
                        {
                            OptionColor = TransformationTools.GetDarkBackground(Foreground),
                            SelectedOptionColor = Foreground,
                            BackgroundColor = Background,
                        }
                    };
                    TextWriterRaw.WriteRaw(
                        comboSelectionBoxFrame.Render() +
                        comboSelections.Render()
                    );

                    // Prompt user for choice selection
                    InputEventInfo data = Input.ReadPointerOrKey();
                    if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
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
            }
            Provided = true;
        }
    }
}
