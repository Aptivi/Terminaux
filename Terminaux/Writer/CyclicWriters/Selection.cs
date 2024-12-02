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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Selection cyclic renderer
    /// </summary>
    public class Selection : IStaticRenderable
    {
        private InputChoiceCategoryInfo[] selections = [];
        private int left;
        private int top;
        private int currentSelection;
        private int[]? currentSelections;
        private int height;
        private int width;
        private bool sliderInside = false;
        private int altChoicePos = -1;
        private bool swapSelectedColors = true;
        private Color? foregroundColor = null;
        private Color? backgroundColor = null;
        private Color? selectedForegroundColor = null;
        private Color? selectedBackgroundColor = null;
        private Color? altForegroundColor = null;
        private Color? altBackgroundColor = null;
        private Color? altSelectedForegroundColor = null;
        private Color? altSelectedBackgroundColor = null;
        private Color? disabledForegroundColor = null;
        private Color? disabledBackgroundColor = null;

        /// <summary>
        /// Renders a selection
        /// </summary>
        /// <returns>A string representation of the selection</returns>
        public string Render()
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(selections);
            bool isMultiple = currentSelections is not null;
            if ((currentSelection < 0 || currentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxInternalException("Can't determine if the selection input is single or multiple");
            if (altChoicePos < 0 || altChoicePos > choices.Count)
                altChoicePos = choices.Count;

            // Check for colors
            bool useColor =
                foregroundColor is not null || backgroundColor is not null ||
                selectedForegroundColor is not null || selectedBackgroundColor is not null ||
                altForegroundColor is not null || altBackgroundColor is not null ||
                altSelectedForegroundColor is not null || altSelectedBackgroundColor is not null ||
                disabledForegroundColor is not null || disabledBackgroundColor is not null;
            foregroundColor ??= ColorTools.CurrentForegroundColor;
            backgroundColor ??= ColorTools.CurrentBackgroundColor;
            selectedForegroundColor ??= ColorTools.CurrentForegroundColor;
            selectedBackgroundColor ??= ColorTools.CurrentBackgroundColor;
            altForegroundColor ??= ColorTools.CurrentForegroundColor;
            altBackgroundColor ??= ColorTools.CurrentBackgroundColor;
            altSelectedForegroundColor ??= ColorTools.CurrentForegroundColor;
            altSelectedBackgroundColor ??= ColorTools.CurrentBackgroundColor;
            disabledForegroundColor ??= ColorTools.CurrentForegroundColor;
            disabledBackgroundColor ??= ColorTools.CurrentBackgroundColor;

            // Now, render the choices
            StringBuilder buffer = new();
            StringBuilder choiceText = new();
            string prefix = isMultiple ? "  [ ] " : "  ";
            int AnswerTitleLeft = choices.Max(x => (selections.Length > 1 ? $"  {prefix}{x.ChoiceName}) " : $"{prefix}{x.ChoiceName}) ").Length);
            int leftPos = left + (sliderInside ? 1 : 0);
            List<int> selectionHeights = [];
            int processedHeight = 0;
            int processedChoices = 0;
            var tristates = SelectionInputTools.GetCategoryTristates(selections, choices, currentSelections);
            int relatedIdx = -1;
            for (int categoryIdx = 0; categoryIdx < selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = selections[categoryIdx];
                var tristate = tristates[categoryIdx];
                if (selections.Length > 1)
                {
                    string modifiers = $"{(isMultiple ? tristate == SelectionTristate.Selected ? "[*] " : tristate == SelectionTristate.FiftyFifty ? "[/] " : "[ ] " : "")}";
                    string finalRendered = $"{modifiers}{category.Name}".Truncate(width);
                    choiceText.AppendLine(
                        ColorTools.RenderSetConsoleColor(ConsoleColorData.Silver.Color) +
                        ColorTools.RenderSetConsoleColor(backgroundColor, true) +
                        finalRendered + new string(' ', width - ConsoleChar.EstimateCellWidth(finalRendered))
                    );
                    processedHeight++;
                }

                var groupTristates = SelectionInputTools.GetGroupTristates(category.Groups, choices, currentSelections);
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    var groupTristate = groupTristates[groupIdx];
                    if (category.Groups.Length > 1)
                    {
                        string modifiers = $"{(isMultiple ? groupTristate == SelectionTristate.Selected ? "[*] " : groupTristate == SelectionTristate.FiftyFifty ? "[/] " : "[ ] " : "")}";
                        string finalRendered = $"  {modifiers}{group.Name}".Truncate(width);
                        choiceText.AppendLine(
                            ColorTools.RenderSetConsoleColor(ConsoleColorData.Grey.Color) +
                            ColorTools.RenderSetConsoleColor(backgroundColor, true) +
                            finalRendered + new string(' ', width - ConsoleChar.EstimateCellWidth(finalRendered))
                        );
                        processedHeight++;
                    }
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        relatedIdx++;
                        bool selected = processedChoices == currentSelection;
                        var choice = group.Choices[i];
                        string AnswerTitle = choice.ChoiceTitle ?? "";
                        bool disabled = choice.ChoiceDisabled;

                        // Get the option
                        string modifiers = $"{(selected ? ">" : disabled ? "X" : " ")}{(isMultiple ? $" [{(currentSelections.Contains(relatedIdx) ? "*" : " ")}]" : "")}";
                        string AnswerOption = selections.Length > 1 ? $"  {modifiers} {choice.ChoiceName}) {AnswerTitle}" : $"{modifiers} {choice.ChoiceName}) {AnswerTitle}";
                        if (AnswerTitleLeft < width)
                        {
                            string renderedChoice = selections.Length > 1 ? $"  {modifiers} {choice.ChoiceName}) " : $"{modifiers} {choice.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}";
                        }
                        AnswerOption = AnswerOption.Truncate(width - 4);

                        // Render an entry
                        bool isAlt = i > altChoicePos;
                        var finalForeColor =
                            choice.ChoiceDisabled ? disabledForegroundColor :
                            selected ?
                                (isAlt ?
                                    (swapSelectedColors ? altSelectedBackgroundColor : altSelectedForegroundColor) :
                                    (swapSelectedColors ? selectedBackgroundColor : selectedForegroundColor)
                                ) :
                                (isAlt ? altForegroundColor : foregroundColor);
                        var finalBackColor =
                            choice.ChoiceDisabled ? disabledBackgroundColor :
                            selected ?
                                (isAlt ?
                                    (swapSelectedColors ? altSelectedForegroundColor : altSelectedBackgroundColor) :
                                    (swapSelectedColors ? selectedForegroundColor : selectedBackgroundColor)
                                ) :
                                (isAlt ? altBackgroundColor : backgroundColor);
                        if (useColor)
                        {
                            choiceText.AppendLine(
                                ColorTools.RenderSetConsoleColor(finalForeColor) +
                                ColorTools.RenderSetConsoleColor(finalBackColor, true) +
                                AnswerOption + new string(' ', width - AnswerOption.Length)
                            );
                        }
                        else
                        {
                            choiceText.AppendLine(
                                AnswerOption + new string(' ', width - AnswerOption.Length)
                            );
                        }
                        processedHeight++;
                        processedChoices++;
                        selectionHeights.Add(processedHeight);
                    }
                }
            }

            // Render the choices
            int selectionHeight = selectionHeights[currentSelection];
            int currentPage = (selectionHeight - 1) / height;
            int startIndex = height * currentPage;
            var choiceTextLines = choiceText.ToString().SplitNewLines();
            bool wiped = false;
            for (int i = 0; i <= height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                int optionTop = top + finalIndex - startIndex;
                if (finalIndex >= selectionHeights[selectionHeights.Count - 1])
                {
                    if (useColor && !wiped)
                    {
                        wiped = true;
                        buffer.Append(
                            ColorTools.RenderRevertForeground() +
                            ColorTools.RenderRevertBackground()
                        );
                    }
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(leftPos + 1, optionTop + 1) +
                        new string(' ', width)
                    );
                }
                else
                {
                    string line = choiceTextLines[finalIndex];
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(leftPos + 1, optionTop + 1) +
                        line
                    );
                }
            }

            // Render the vertical bar
            if (choices.Count > height && height >= 4)
            {
                int finalWidth = sliderInside ? left + width + 1 : left + width;
                var slider = new Slider(currentSelection + 1, 0, choices.Count)
                {
                    Vertical = true,
                    Height = height - 2,
                    SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                };
                if (useColor)
                {
                    slider.SliderActiveForegroundColor = foregroundColor;
                    slider.SliderForegroundColor = TransformationTools.GetDarkBackground(foregroundColor);
                    slider.SliderBackgroundColor = backgroundColor;
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack("▲", finalWidth, top, foregroundColor, backgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▼", finalWidth, top + height - 1, foregroundColor, backgroundColor)
                    );
                }
                else
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWhere("▲", finalWidth, top) +
                        TextWriterWhereColor.RenderWhere("▼", finalWidth, top + height - 1)
                    );
                }
                buffer.Append(
                    ContainerTools.RenderRenderable(slider, new(finalWidth, top + 1))
                );
            }

            // Render the final result
            if (useColor)
            {
                buffer.Append(
                    ColorTools.RenderRevertForeground() +
                    ColorTools.RenderRevertBackground()
                );
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        public Selection()
        { }
    }
}
