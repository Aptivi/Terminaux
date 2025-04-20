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
using System.Linq;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Combo box (selection) module
    /// </summary>
    public class ComboBoxModule : InputModule
    {
        /// <summary>
        /// Choices to render
        /// </summary>
        public InputChoiceInfo[] Choices { get; set; } = [];

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            InputChoiceInfo? choice = Value is not null ? Choices[(int)Value] : null;
            string valueString = choice?.ChoiceName ?? "";
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
                Value = InfoBoxSelectionColor.WriteInfoBoxSelection(Name, Choices, Description);
            }
            else
            {
                int currentSelection = Choices.Any((ici) => ici.ChoiceDefault) ? Choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx : Value is not null ? (int)Value : 0;
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
                    var comboSelections = new Selection()
                    {
                        Left = inputPopoverPos.X + 1,
                        Top = inputPopoverPos.Y + 1,
                        Width = inputPopoverSize.Width - 2,
                        Height = finalPopoverHeight,
                        CurrentSelection = currentSelection,
                        AltChoicePos = Choices.Length,
                        Selections = [
                            new InputChoiceCategoryInfo("Main category", [
                                new InputChoiceGroupInfo("Main group", Choices)
                            ])
                        ],
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
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var key = Input.ReadKey();
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                goingUp = true;
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = Choices.Length - 1;
                                break;
                            case ConsoleKey.DownArrow:
                                currentSelection++;
                                if (currentSelection > Choices.Length - 1)
                                    currentSelection = 0;
                                break;
                            case ConsoleKey.Home:
                                currentSelection = 0;
                                break;
                            case ConsoleKey.End:
                                currentSelection = Choices.Length - 1;
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
                                    if (currentSelection > Choices.Length - 1)
                                        currentSelection = Choices.Length - 1;
                                }
                                break;
                            case ConsoleKey.Tab:
                                var selectedInstance = Choices[currentSelection];
                                string choiceName = selectedInstance.ChoiceName;
                                string choiceTitle = selectedInstance.ChoiceTitle;
                                string choiceDesc = selectedInstance.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                }
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
                    if (currentSelection >= 0)
                    {
                        while (Choices[currentSelection].ChoiceDisabled)
                        {
                            if (goingUp)
                            {
                                currentSelection--;
                                if (currentSelection < 0)
                                    currentSelection = Choices.Length - 1;
                            }
                            else
                            {
                                currentSelection++;
                                if (currentSelection > Choices.Length - 1)
                                    currentSelection = 0;
                            }
                        }
                    }
                }
                if (!cancel)
                    Value = currentSelection;
            }
        }
    }
}
