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
using Terminaux.Colors;
using Selections = Terminaux.Writer.CyclicWriters.Selection;

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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
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
            if (selectionChoices <= 0)
                throw new TerminauxException("Selection choice number is out of range");
            if (width < 0 || width > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Selection width is out of range");
            if (altChoicePos < 0 || altChoicePos > selections.Length)
                altChoicePos = selections.Length;

            // Rely on the internal function
            return RenderSelectionsInternal(selections, left, top, currentSelection, currentSelections, selectionChoices, width, sliderInside, altChoicePos, swapSelectedColors, foregroundColor, backgroundColor, selectedForegroundColor, selectedBackgroundColor, altForegroundColor, altBackgroundColor, altSelectedForegroundColor, altSelectedBackgroundColor, disabledForegroundColor, disabledBackgroundColor);
        }

        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        internal static string RenderSelectionsInternal(InputChoiceInfo[] selections, int left, int top, int currentSelection, int[]? currentSelections, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            InputChoiceCategoryInfo[] selectionsCategory =
            [
                new InputChoiceCategoryInfo("General choices",
                [
                    new InputChoiceGroupInfo("Available choices", selections)
                ])
            ];
            return RenderSelectionsInternal(selectionsCategory, left, top, currentSelection, currentSelections, selectionChoices, width, sliderInside, altChoicePos, swapSelectedColors, foregroundColor, backgroundColor, selectedForegroundColor, selectedBackgroundColor, altForegroundColor, altBackgroundColor, altSelectedForegroundColor, altSelectedBackgroundColor, disabledForegroundColor, disabledBackgroundColor);
        }

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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderSelections(InputChoiceCategoryInfo[] selections, int left, int top, int currentSelection, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            // Check for values
            if (selections is null || selections.Length == 0)
                throw new TerminauxException("Choices are not specified");
            List<InputChoiceInfo> choices = GetChoicesFromCategories(selections);
            if (choices is null || choices.Count == 0)
                throw new TerminauxException("Choices are not specified");
            if (left < 0 || left > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Left position is out of range");
            if (top < 0 || top > ConsoleWrapper.WindowHeight)
                throw new TerminauxException("Top position is out of range");
            if (currentSelection < 0 || currentSelection >= choices.Count)
                throw new TerminauxException("Current selection is out of range");
            if (selectionChoices <= 0)
                throw new TerminauxException("Selection choice number is out of range");
            if (width < 0 || width > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Selection width is out of range");
            if (altChoicePos < 0 || altChoicePos > choices.Count)
                altChoicePos = choices.Count;

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
        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        public static string RenderSelections(InputChoiceCategoryInfo[] selections, int left, int top, int currentSelection, int[]? currentSelections, int selectionChoices, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            // Check for values
            if (selections is null || selections.Length == 0)
                throw new TerminauxException("Choices are not specified");
            List<InputChoiceInfo> choices = GetChoicesFromCategories(selections);
            if (choices is null || choices.Count == 0)
                throw new TerminauxException("Choices are not specified");
            if (left < 0 || left > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Left position is out of range");
            if (top < 0 || top > ConsoleWrapper.WindowHeight)
                throw new TerminauxException("Top position is out of range");
            if (currentSelection < 0 || currentSelection >= choices.Count)
                throw new TerminauxException("Current selection is out of range");
            if (selectionChoices <= 0)
                throw new TerminauxException("Selection choice number is out of range");
            if (width < 0 || width > ConsoleWrapper.WindowWidth)
                throw new TerminauxException("Selection width is out of range");
            if (altChoicePos < 0 || altChoicePos > choices.Count)
                altChoicePos = choices.Count;

            // Rely on the internal function
            return RenderSelectionsInternal(selections, left, top, currentSelection, currentSelections, selectionChoices, width, sliderInside, altChoicePos, swapSelectedColors, foregroundColor, backgroundColor, selectedForegroundColor, selectedBackgroundColor, altForegroundColor, altBackgroundColor, altSelectedForegroundColor, altSelectedBackgroundColor, disabledForegroundColor, disabledBackgroundColor);
        }

        [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
        internal static string RenderSelectionsInternal(InputChoiceCategoryInfo[] selections, int left, int top, int currentSelection, int[]? currentSelections, int height, int width, bool sliderInside = false, int altChoicePos = -1, bool swapSelectedColors = true, Color? foregroundColor = null, Color? backgroundColor = null, Color? selectedForegroundColor = null, Color? selectedBackgroundColor = null, Color? altForegroundColor = null, Color? altBackgroundColor = null, Color? altSelectedForegroundColor = null, Color? altSelectedBackgroundColor = null, Color? disabledForegroundColor = null, Color? disabledBackgroundColor = null)
        {
            return new Selections(selections)
            {
                Left = left,
                Top = top,
                CurrentSelection = currentSelection,
                CurrentSelections = currentSelections,
                Height = height,
                Width = width,
                SliderInside = sliderInside,
                AltChoicePos = altChoicePos,
                SwapSelectedColors = swapSelectedColors,
                Settings = new()
                {
                    BackgroundColor = backgroundColor ?? ColorTools.CurrentBackgroundColor,
                    OptionColor = foregroundColor ?? ColorTools.CurrentForegroundColor,
                    AltOptionColor = altForegroundColor ?? ColorTools.CurrentBackgroundColor,
                    SelectedOptionColor = selectedForegroundColor ?? ColorTools.CurrentBackgroundColor,
                    DisabledOptionColor = disabledForegroundColor ?? ColorTools.CurrentBackgroundColor,
                }
            }.Render();
        }

        internal static List<InputChoiceInfo> GetChoicesFromCategories(InputChoiceCategoryInfo[] categories)
        {
            List<InputChoiceInfo> choices = [];
            foreach (var category in categories)
                foreach (var group in category.Groups)
                    choices.AddRange(group.Choices);
            return choices;
        }

        internal static List<SelectionTristate> GetCategoryTristates(InputChoiceCategoryInfo[] categories, List<InputChoiceInfo> choices, int[]? currentSelections, ref int startIndex)
        {
            List<SelectionTristate> categoryTristates = [];
            foreach (var category in categories)
            {
                SelectionTristate tristate = SelectionTristate.Unselected;
                var tristates = GetGroupTristates(category.Groups, choices, currentSelections, ref startIndex);
                tristate =
                    tristates.All((ts) => ts == SelectionTristate.Selected) ? SelectionTristate.Selected :
                    tristates.All((ts) => ts == SelectionTristate.Unselected) ? SelectionTristate.Unselected :
                    SelectionTristate.FiftyFifty;
                categoryTristates.Add(tristate);
            }
            return categoryTristates;
        }

        internal static List<SelectionTristate> GetGroupTristates(InputChoiceGroupInfo[] groups, List<InputChoiceInfo> choices, int[]? currentSelections, ref int startIndex)
        {
            List<SelectionTristate> groupTristates = [];
            foreach (var group in groups)
            {
                SelectionTristate tristate = SelectionTristate.Unselected;
                var tristates = GetChoiceTristates(group.Choices, choices, currentSelections, ref startIndex);
                tristate =
                    tristates.All((ts) => ts == SelectionTristate.Selected) ? SelectionTristate.Selected :
                    tristates.All((ts) => ts == SelectionTristate.Unselected) ? SelectionTristate.Unselected :
                    SelectionTristate.FiftyFifty;
                groupTristates.Add(tristate);
            }
            return groupTristates;
        }

        internal static List<SelectionTristate> GetChoiceTristates(InputChoiceInfo[] groupChoices, List<InputChoiceInfo> choices, int[]? currentSelections, ref int startIndex)
        {
            List<SelectionTristate> choiceTristates = [];
            currentSelections ??= [];
            int endIndex = startIndex + groupChoices.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                SelectionTristate tristate = currentSelections.Contains(i) ? SelectionTristate.Selected : SelectionTristate.Unselected;
                choiceTristates.Add(tristate);
            }
            startIndex = endIndex;
            return choiceTristates;
        }

        internal static Dictionary<int, int> GetChoicePages(InputChoiceCategoryInfo[] categories, int height)
        {
            int page = 0;
            int choiceCount = 0;
            int offset = 0;
            Dictionary<int, int> choicePages = [];

            void ProcessPage()
            {
                if (choiceCount >= height - offset)
                {
                    choicePages.Add(page, choiceCount);
                    page++;
                    choiceCount = 0;
                    offset = 0;
                }
            }
            foreach (var category in categories)
            {
                if (categories.Length > 1)
                {
                    offset++;
                    ProcessPage();
                }

                foreach (var group in category.Groups)
                {
                    if (category.Groups.Length > 1)
                    {
                        offset++;
                        ProcessPage();
                    }
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        choiceCount++;
                        ProcessPage();
                    }
                }
            }
            if (choiceCount > 0)
                choicePages.Add(page, choiceCount);
            return choicePages;
        }

        internal static int DetermineCurrentPage(InputChoiceCategoryInfo[] categories, int height, int currentNum)
        {
            var pages = GetChoicePages(categories, height);
            int page = 0;
            int position = 0;
            foreach (var pageKvp in pages)
            {
                page = pageKvp.Key;
                position += pageKvp.Value;
                if (position >= currentNum)
                    break;
            }
            return page;
        }

        internal static (InputChoiceCategoryInfo, InputChoiceGroupInfo) GetCategoryGroupFrom(int choiceNum, InputChoiceCategoryInfo[] categories)
        {
            int choiceCount = 0;
            foreach (var category in categories)
            {
                foreach (var group in category.Groups)
                {
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        choiceCount++;
                        if (choiceCount == choiceNum)
                            return (category, group);
                    }
                }
            }
            throw new TerminauxInternalException("Can't get category and group in choice number {0}.", choiceNum);
        }
    }
}
