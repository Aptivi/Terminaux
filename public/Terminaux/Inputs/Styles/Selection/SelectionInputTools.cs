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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;

namespace Terminaux.Inputs.Styles.Selection
{
    internal static class SelectionInputTools
    {
        internal static List<InputChoiceInfo> GetChoicesFromCategories(InputChoiceCategoryInfo[] categories)
        {
            List<InputChoiceInfo> choices = [];
            foreach (var category in categories)
                foreach (var group in category.Groups)
                    choices.AddRange(group.Choices);
            return choices;
        }

        internal static List<SelectionTristate> GetCategoryTristates(InputChoiceCategoryInfo[] categories, int[]? currentSelections, ref int startIndex)
        {
            List<SelectionTristate> categoryTristates = [];
            foreach (var category in categories)
            {
                SelectionTristate tristate = SelectionTristate.Unselected;
                var tristates = GetGroupTristates(category.Groups, currentSelections, ref startIndex);
                tristate =
                    tristates.All((ts) => ts == SelectionTristate.Selected) ? SelectionTristate.Selected :
                    tristates.All((ts) => ts == SelectionTristate.Unselected) ? SelectionTristate.Unselected :
                    SelectionTristate.FiftyFifty;
                categoryTristates.Add(tristate);
            }
            return categoryTristates;
        }

        internal static List<SelectionTristate> GetGroupTristates(InputChoiceGroupInfo[] groups, int[]? currentSelections, ref int startIndex)
        {
            List<SelectionTristate> groupTristates = [];
            foreach (var group in groups)
            {
                SelectionTristate tristate = SelectionTristate.Unselected;
                var tristates = GetChoiceTristates(group.Choices, currentSelections, ref startIndex);
                tristate =
                    tristates.All((ts) => ts == SelectionTristate.Selected) ? SelectionTristate.Selected :
                    tristates.All((ts) => ts == SelectionTristate.Unselected) ? SelectionTristate.Unselected :
                    SelectionTristate.FiftyFifty;
                groupTristates.Add(tristate);
            }
            return groupTristates;
        }

        internal static List<SelectionTristate> GetChoiceTristates(InputChoiceInfo[] groupChoices, int[]? currentSelections, ref int startIndex)
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
                if (position > currentNum)
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
                        if (choiceCount == choiceNum)
                            return (category, group);
                        choiceCount++;
                    }
                }
            }
            throw new TerminauxInternalException(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_TOOLS_EXCEPTION_CATEGORYGROUPERROR"), choiceNum);
        }

        internal static int[] SelectDefaults(InputChoiceCategoryInfo[] selections)
        {
            InputChoiceInfo[] choices = [.. GetChoicesFromCategories(selections)];
            return SelectDefaults(choices);
        }

        internal static int[] SelectDefaults(InputChoiceInfo[] choices) =>
            choices.Any((ici) => ici.ChoiceDefaultSelected) ?
            choices.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx).ToArray() :
            [];

        internal static int GetDefaultChoice(InputChoiceCategoryInfo[] selections)
        {
            InputChoiceInfo[] choices = [.. GetChoicesFromCategories(selections)];
            return GetDefaultChoice(choices);
        }

        internal static int GetDefaultChoice(InputChoiceInfo[] choices) =>
            choices.Any((ici) => ici.ChoiceDefault) ?
            choices.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx :
            0;
    }
}
