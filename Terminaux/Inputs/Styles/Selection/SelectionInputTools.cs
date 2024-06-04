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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Inputs.Styles.Selection
{
    /// <summary>
    /// Selection style input tools
    /// </summary>
    public static class SelectionInputTools
    {
        /// <summary>
        /// Renders the selections to the console
        /// </summary>
        /// <param name="selections">The selection choices</param>
        /// <param name="left">Left position of the selection box (upper left corner, zero-based)</param>
        /// <param name="top">Top position of the selection box (upper left corner, zero-based)</param>
        /// <param name="currentSelection">Zero-based current choice selection</param>
        /// <param name="selectionChoices">How many choices to render to the selection box? One-based.</param>
        /// <param name="width">Width of the selection box (zero-based)</param>
        /// <param name="sliderInside">Whether to render the slider inside the box or outside the box</param>
        /// <param name="altChoicePos">Alternate choice position (zero-based)</param>
        /// <param name="swapSelectedColors">Whether to swap the selected choice colors or not</param>
        /// <param name="foregroundColor">Foreground color of the unselected item</param>
        /// <param name="backgroundColor">Background color of the unselected item</param>
        /// <param name="selectedForegroundColor">Foreground color of the selected item</param>
        /// <param name="selectedBackgroundColor">Background color of the selected item</param>
        /// <param name="altForegroundColor">Foreground color of the unselected alternate item</param>
        /// <param name="altBackgroundColor">Background color of the unselected alternate item</param>
        /// <param name="altSelectedForegroundColor">Foreground color of the selected alternate item</param>
        /// <param name="altSelectedBackgroundColor">Background color of the selected alternate item</param>
        /// <param name="disabledForegroundColor">Foreground color of the disabled item</param>
        /// <param name="disabledBackgroundColor">Background color of the disabled item</param>
        /// <returns>A string that you can use with the raw console writer or with the screen part renderer</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string RenderSelections(InputChoiceInfo[] selections, int left, int top, int currentSelection, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            // Check for values
            if (selections is null || selections.Length == 0)
                throw new TerminauxException("Choices are not specified");
            if (left < 0 || left > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Left position is out of range");
            if (top < 0 || top > ConsoleWrapper.WindowHeight)
                throw new TerminauxException("Top position is out of range");
            if (currentSelection < 0 || currentSelection >= selections.Length)
                throw new TerminauxException("Current selection is out of range");
            if (selectionChoices <= 0)
                throw new TerminauxException("Selection choice number is out of range");
            if (width < 0 || width > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Selection width is out of range");
            if (altChoicePos < 0 || altChoicePos > selections.Length)
                altChoicePos = selections.Length;

            // Rely on the internal function
            return RenderSelectionsInternal(selections, left, top, currentSelection, null, selectionChoices, width, sliderInside, altChoicePos, swapSelectedColors, foregroundColor, backgroundColor, selectedForegroundColor, selectedBackgroundColor, altForegroundColor, altBackgroundColor, altSelectedForegroundColor, altSelectedBackgroundColor, disabledForegroundColor, disabledBackgroundColor);
        }

        /// <summary>
        /// Renders the selections to the console
        /// </summary>
        /// <param name="selections">The selection choices</param>
        /// <param name="left">Left position of the selection box (upper left corner, zero-based)</param>
        /// <param name="top">Top position of the selection box (upper left corner, zero-based)</param>
        /// <param name="currentSelection">Zero-based current choice selection</param>
        /// <param name="currentSelections">List of zero-based current selection numbers (for multiple choice selection)</param>
        /// <param name="selectionChoices">How many choices to render to the selection box? One-based.</param>
        /// <param name="width">Width of the selection box (zero-based)</param>
        /// <param name="sliderInside">Whether to render the slider inside the box or outside the box</param>
        /// <param name="altChoicePos">Alternate choice position (zero-based)</param>
        /// <param name="swapSelectedColors">Whether to swap the selected choice colors or not</param>
        /// <param name="foregroundColor">Foreground color of the unselected item</param>
        /// <param name="backgroundColor">Background color of the unselected item</param>
        /// <param name="selectedForegroundColor">Foreground color of the selected item</param>
        /// <param name="selectedBackgroundColor">Background color of the selected item</param>
        /// <param name="altForegroundColor">Foreground color of the unselected alternate item</param>
        /// <param name="altBackgroundColor">Background color of the unselected alternate item</param>
        /// <param name="altSelectedForegroundColor">Foreground color of the selected alternate item</param>
        /// <param name="altSelectedBackgroundColor">Background color of the selected alternate item</param>
        /// <param name="disabledForegroundColor">Foreground color of the disabled item</param>
        /// <param name="disabledBackgroundColor">Background color of the disabled item</param>
        /// <returns>A string that you can use with the raw console writer or with the screen part renderer</returns>
        /// <exception cref="TerminauxException"></exception>
        public static string RenderSelections(InputChoiceInfo[] selections, int left, int top, int currentSelection, int[]? currentSelections, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            // Check for values
            if (selections is null || selections.Length == 0)
                throw new TerminauxException("Choices are not specified");
            if (left < 0 || left > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Left position is out of range");
            if (top < 0 || top > ConsoleWrapper.WindowHeight)
                throw new TerminauxException("Top position is out of range");
            if (currentSelection < 0 || currentSelection >= selections.Length)
                throw new TerminauxException("Current selection is out of range");
            if (currentSelections is null)
                throw new TerminauxException("Current selections are not specified");
            if (selectionChoices <= 0)
                throw new TerminauxException("Selection choice number is out of range");
            if (width < 0 || width > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Selection width is out of range");
            if (altChoicePos < 0 || altChoicePos > selections.Length)
                altChoicePos = selections.Length;

            // Rely on the internal function
            return RenderSelectionsInternal(selections, left, top, currentSelection, currentSelections, selectionChoices, width, sliderInside, altChoicePos, swapSelectedColors, foregroundColor, backgroundColor, selectedForegroundColor, selectedBackgroundColor, altForegroundColor, altBackgroundColor, altSelectedForegroundColor, altSelectedBackgroundColor, disabledForegroundColor, disabledBackgroundColor);
        }

        internal static string RenderSelectionsInternal(InputChoiceInfo[] selections, int left, int top, int currentSelection, int[]? currentSelections, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            // Determine if multiple or single
            bool isMultiple = currentSelections is not null;
            if ((currentSelection < 0 || currentSelection >= selections.Length) && !isMultiple)
                throw new TerminauxInternalException("Can't determine if the selection input is single or multiple");
            if (altChoicePos < 0 || altChoicePos > selections.Length)
                altChoicePos = selections.Length;

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

            // Now, render the selections
            StringBuilder buffer = new();
            int AnswerTitleLeft = selections.Max(x => (isMultiple ? $"  [ ] {x.ChoiceName}) " : $"  {x.ChoiceName}) ").Length);
            int currentPage = currentSelection / selectionChoices;
            int startIndex = selectionChoices * currentPage;
            for (int i = 0; i <= selectionChoices - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                int leftPos = left + (sliderInside ? 1 : 0);
                int optionTop = top + finalIndex - startIndex;
                if (finalIndex >= selections.Length)
                {
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(leftPos + 1, optionTop + 1) +
                        new string(' ', width)
                    );
                    continue;
                }
                bool selected = finalIndex == currentSelection;
                var choice = selections[finalIndex];
                string AnswerTitle = choice.ChoiceTitle ?? "";
                bool disabled = choice.ChoiceDisabled;

                // Get the option
                string modifiers = $"{(selected ? ">" : disabled ? "X" : " ")}{(isMultiple ? $" [{(currentSelections.Contains(finalIndex) ? "*" : " ")}]" : "")}";
                string AnswerOption = $"{modifiers} {choice}) {AnswerTitle}";
                if (AnswerTitleLeft < width)
                {
                    string renderedChoice = $"{modifiers} {choice.ChoiceName}) ";
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
                    buffer.Append(
                        ColorTools.RenderSetConsoleColor(finalForeColor) +
                        ColorTools.RenderSetConsoleColor(finalBackColor, true) +
                        TextWriterWhereColor.RenderWhere(AnswerOption + new string(' ', width - AnswerOption.Length), leftPos, optionTop, finalForeColor, finalBackColor) +
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                else
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWhere(AnswerOption + new string(' ', width - AnswerOption.Length), leftPos, optionTop)
                    );
                }
            }

            // Render the vertical bar
            if (selections.Length > selectionChoices && selectionChoices >= 4)
            {
                int finalWidth = sliderInside ? left + width + 1 : left + width;
                if (useColor)
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack("↑", finalWidth, top, foregroundColor, backgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("↓", finalWidth, top + selectionChoices - 1, foregroundColor, backgroundColor) +
                        SliderVerticalColor.RenderVerticalSlider(currentSelection + 1, selections.Length, finalWidth - 1, top, selectionChoices - 2, foregroundColor, backgroundColor, false)
                    );
                }
                else
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWhere("↑", finalWidth, top) +
                        TextWriterWhereColor.RenderWhere("↓", finalWidth, top + selectionChoices - 1) +
                        SliderVerticalColor.RenderVerticalSliderPlain(currentSelection + 1, selections.Length, finalWidth - 1, top, selectionChoices - 2, false)
                    );
                }
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
    }
}
