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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters;
using Textify.Tools;

namespace Terminaux.Inputs.Styles.Selection
{
    internal static class SelectionStyleBase
    {
        internal static int[] PromptSelection(string Question, InputChoiceCategoryInfo[] Answers, InputChoiceCategoryInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk, bool multiple)
        {
            settings ??= SelectionStyleSettings.GlobalSettings;

            // Variables
            Color questionColor = settings.QuestionColor;
            Color sliderColor = settings.SliderColor;
            Color inputColor = settings.InputColor;
            Color optionColor = settings.OptionColor;
            Color altOptionColor = settings.AltOptionColor;
            Color selectedOptionColor = settings.SelectedOptionColor;
            Color separatorColor = settings.SeparatorColor;
            Color textColor = settings.TextColor;
            Color disabledOptionColor = settings.DisabledOptionColor;
            int HighlightedAnswer = 1;
            int showcaseLine = 0;
            InputChoiceCategoryInfo[] categories = [.. Answers, .. AltAnswers];
            List<InputChoiceInfo> AllAnswers = SelectionInputTools.GetChoicesFromCategories(categories);
            List<int> SelectedAnswers = [];
            bool allDisabled = AllAnswers.All((ici) => ici.ChoiceDisabled);
            bool showCategories = categories.Length > 0;
            int AnswerTitleLeft = AllAnswers.Max(x => $"{(showCategories ? $"      {x.ChoiceName}) " : $"  {x.ChoiceName}) ")}".Length);

            // We need to not to run the selection style when everything is disabled
            if (allDisabled)
                throw new TerminauxException("The selection style requires that there is at least one choice enabled.");

            // Make selected choices from the ChoiceDefaultSelected value.
            SelectedAnswers = AllAnswers.Any((ici) => ici.ChoiceDefaultSelected) ? AllAnswers.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx + 1).ToList() : [];

            // Before we proceed, we need to check the highlighted answer number
            if (HighlightedAnswer > AllAnswers.Count)
                HighlightedAnswer = 1;
            HighlightedAnswer = AllAnswers.Any((ici) => ici.ChoiceDefault) ? AllAnswers.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx + 1 : 1;

            // First alt answer index
            int altAnswersFirstIdx = Answers.Length;
            bool initialVisible = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = false;

            // Build list of bindings for help
            Keybinding[] showBindings = multiple ?
            [
                new("Confirm", ConsoleKey.Enter),
                new("Select", ConsoleKey.Spacebar),
                new("Sidebar", ConsoleKey.S),
                new("Help", ConsoleKey.K),
            ] :
            [
                new("Select", ConsoleKey.Enter),
                new("Sidebar", ConsoleKey.S),
                new("Help", ConsoleKey.K),
            ];
            Keybinding[] bindings = multiple ?
            [
                new("Confirms the selections", ConsoleKey.Enter),
                new("Selects or deselects a choice", ConsoleKey.Spacebar),
                new("Goes one element up", ConsoleKey.UpArrow),
                new("Goes one element down", ConsoleKey.DownArrow),
                new("Goes to the first element", ConsoleKey.Home),
                new("Goes to the last element", ConsoleKey.End),
                new("Goes to the previous page", ConsoleKey.PageUp),
                new("Goes to the next page", ConsoleKey.PageDown),
                new("Selects all the elements in the same group and category", ConsoleKey.A),
                new("Selects all the elements in all groups in the same category", ConsoleKey.A, ConsoleModifiers.Shift),
                new("Selects all the elements in all groups in all categories", ConsoleKey.A, ConsoleModifiers.Shift | ConsoleModifiers.Control),
                new("Searches for an element", ConsoleKey.F),
                new("Go up in a sidebar", ConsoleKey.E),
                new("Go down in a sidebar", ConsoleKey.D),
                new("Show page and choice count", ConsoleKey.P),
            ] :
            [
                new("Confirms a selection", ConsoleKey.Enter),
                new("Goes one element up", ConsoleKey.UpArrow),
                new("Goes one element down", ConsoleKey.DownArrow),
                new("Goes to the first element", ConsoleKey.Home),
                new("Goes to the last element", ConsoleKey.End),
                new("Goes to the previous page", ConsoleKey.PageUp),
                new("Goes to the next page", ConsoleKey.PageDown),
                new("Searches for an element", ConsoleKey.F),
                new("Go up in a sidebar", ConsoleKey.E),
                new("Go down in a sidebar", ConsoleKey.D),
                new("Show page and choice count", ConsoleKey.P),
            ];

            // Make a screen
            var selectionScreen = new Screen();
            bool bail = false;
            bool sidebar = false;
            bool showCount = false;
            ScreenTools.SetCurrent(selectionScreen);

            // Query the enabled answers
            try
            {
                while (!bail)
                {
                    // Check to see if the guide is open
                    int sidebarWidth = sidebar ? (ConsoleWrapper.WindowWidth - 6) / 4 : 0;
                    int interiorWidth = ConsoleWrapper.WindowWidth - 6 - sidebarWidth;

                    // Edge case: We need to check to see if the current highlight is disabled
                    while (AllAnswers[HighlightedAnswer - 1].ChoiceDisabled)
                    {
                        HighlightedAnswer++;
                        if (HighlightedAnswer > AllAnswers.Count)
                            HighlightedAnswer = 1;
                    }

                    // Make a screen part
                    var screenPart = new ScreenPart();
                    int startIndex = 0;
                    int endIndex = 0;
                    var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                    string finalSidebarText = $"[{highlightedAnswer.ChoiceName}] {highlightedAnswer.ChoiceTitle}\n\n{highlightedAnswer.ChoiceDescription}";
                    screenPart.Position(0, 0);
                    screenPart.AddDynamicText(() =>
                    {
                        sidebarWidth = sidebar ? (ConsoleWrapper.WindowWidth - 6) / 4 : 0;
                        interiorWidth = ConsoleWrapper.WindowWidth - 6 - sidebarWidth;

                        // Make pages based on console window height
                        int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                        int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                        int answersPerPage = listEndPosition - 5;
                        int pages = AllAnswers.Count / answersPerPage;
                        if (AllAnswers.Count % answersPerPage == 0)
                            pages--;
                        var selectionBuilder = new StringBuilder();

                        // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                        // causing confusion. Pages, again, are one-based.
                        int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                        startIndex = answersPerPage * currentPage;
                        endIndex = answersPerPage * (currentPage + 1) - 1;

                        // Write the question.
                        selectionBuilder.Append(
                            $"{ColorTools.RenderSetConsoleColor(questionColor)}" +
                            $"{Question}"
                        );

                        // Populate the answers
                        selectionBuilder.Append(
                            BorderColor.RenderBorder(2, listStartPosition + 1, interiorWidth, answersPerPage, optionColor) +
                            SelectionInputTools.RenderSelections(categories, 3, listStartPosition + 2, HighlightedAnswer - 1, multiple ? [.. SelectedAnswers] : null, answersPerPage, interiorWidth, false, SelectionInputTools.GetChoicesFromCategories(Answers).Count, false, optionColor, selectedForegroundColor: selectedOptionColor, altForegroundColor: altOptionColor, altSelectedForegroundColor: selectedOptionColor, disabledForegroundColor: disabledOptionColor)
                        );

                        // Write description hint
                        int descHintAreaX = interiorWidth - 3;
                        int descHintAreaY = ConsoleWrapper.WindowHeight - 3;
                        var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                        bool showHint = !string.IsNullOrWhiteSpace(highlightedAnswer.ChoiceDescription);
                        if (showHint || showCount)
                        {
                            selectionBuilder.Append(
                                TextWriterWhereColor.RenderWhereColor((showCount ? $"[{currentPage + 1}/{pages} | {HighlightedAnswer}/{AllAnswers.Count}]" : "") + (showHint ? "[TAB]" : ""), descHintAreaX, descHintAreaY, optionColor)
                            );
                        }

                        // Render a sidebar
                        if (sidebar)
                        {
                            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
                            var boundedSidebar = new BoundedText(finalSidebarText)
                            {
                                Left = interiorWidth + 6,
                                Top = listStartPosition + 1,
                                Width = sidebarWidth - 3,
                                Height = answersPerPage,
                                ForegroundColor = textColor,
                                Line = showcaseLine,
                            };
                            selectionBuilder.Append(
                                BorderColor.RenderBorder(interiorWidth + 5, listStartPosition + 1, sidebarWidth - 3, answersPerPage, textColor) +
                                boundedSidebar.Render()
                            );
                            if (lines.Length > answersPerPage)
                            {
                                selectionBuilder.Append(
                                    TextWriterWhereColor.RenderWhere("↑", interiorWidth + 3 + sidebarWidth, listStartPosition + 2) +
                                    TextWriterWhereColor.RenderWhere("↓", interiorWidth + 3 + sidebarWidth, listStartPosition + answersPerPage + 1) +
                                    SliderVerticalColor.RenderVerticalSlider(showcaseLine, lines.Length, interiorWidth + 2 + sidebarWidth, listStartPosition + 2, answersPerPage - 2, textColor, false)
                                );
                            }
                        }

                        // Render keybindings
                        int descArea = ConsoleWrapper.WindowHeight - 1;
                        selectionBuilder.Append(
                            KeybindingsWriter.RenderKeybindings(showBindings, 0, descArea)
                        );
                        return selectionBuilder.ToString();
                    });
                    selectionScreen.AddBufferedPart("Selection - Variant", screenPart);

                    // Wait for an answer
                    ScreenTools.Render();
                    SpinWait.SpinUntil(() => Input.InputAvailable);
                    bool goingUp = false;
                    if (Input.MouseInputAvailable)
                    {
                        bool UpdateSelectedIndexWithMousePos(PointerEventContext mouse)
                        {
                            if (mouse.Coordinates.x <= 2 || mouse.Coordinates.x >= ConsoleWrapper.WindowWidth - 1)
                                return false;

                            // Make pages based on console window height
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 7;
                            int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                            startIndex = answersPerPage * currentPage;
                            endIndex = answersPerPage * (currentPage + 1) - 1;

                            // Now, translate coordinates to the selected index
                            if (mouse.Coordinates.y <= listStartPosition + 1 || mouse.Coordinates.y >= listEndPosition - 4)
                                return false;
                            int listIndex = mouse.Coordinates.y - (listStartPosition + 1);
                            listIndex = startIndex + listIndex;
                            listIndex = listIndex > AllAnswers.Count ? AllAnswers.Count : listIndex;
                            HighlightedAnswer = listIndex;
                            return true;
                        }

                        bool DetermineArrowPressed(PointerEventContext mouse)
                        {
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            if (AllAnswers.Count <= answersPerPage)
                                return false;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (interiorWidth + 3, listStartPosition + 2),
                                    (interiorWidth + 3, listStartPosition + 1 + answersPerPage));
                        }

                        bool DetermineSidebarArrowPressed(PointerEventContext mouse)
                        {
                            if (!sidebar)
                                return false;
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            if (AllAnswers.Count <= answersPerPage)
                                return false;
                            return
                                PointerTools.PointerWithinRange(mouse,
                                    (ConsoleWrapper.WindowWidth - 3, listStartPosition + 2),
                                    (ConsoleWrapper.WindowWidth - 3, listStartPosition + 1 + answersPerPage));
                        }

                        void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
                        {
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            if (AllAnswers.Count <= answersPerPage)
                                return;
                            if (mouse.Coordinates.x == interiorWidth + 3)
                            {
                                if (mouse.Coordinates.y == listStartPosition + 2)
                                {
                                    goingUp = true;
                                    HighlightedAnswer--;
                                    if (HighlightedAnswer == 0)
                                        HighlightedAnswer = 1;
                                }
                                else if (mouse.Coordinates.y == listStartPosition + 1 + answersPerPage)
                                {
                                    HighlightedAnswer++;
                                    if (HighlightedAnswer > AllAnswers.Count)
                                        HighlightedAnswer = AllAnswers.Count;
                                }
                            }
                        }

                        void UpdateSidebarPositionBasedOnArrowPress(PointerEventContext mouse)
                        {
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            if (AllAnswers.Count <= answersPerPage)
                                return;
                            if (mouse.Coordinates.x == ConsoleWrapper.WindowWidth - 3)
                            {
                                if (mouse.Coordinates.y == listStartPosition + 2)
                                {
                                    showcaseLine--;
                                    if (showcaseLine < 0)
                                        showcaseLine = 0;
                                }
                                else if (mouse.Coordinates.y == listStartPosition + 1 + answersPerPage)
                                {
                                    string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
                                    if (lines.Length <= answersPerPage)
                                        return;
                                    showcaseLine++;
                                    if (showcaseLine > lines.Length - answersPerPage)
                                        showcaseLine = lines.Length - answersPerPage;
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
                                goingUp = true;
                                HighlightedAnswer--;
                                if (HighlightedAnswer == 0)
                                    HighlightedAnswer = AllAnswers.Count;
                                showcaseLine = 0;
                                break;
                            case PointerButton.WheelDown:
                                HighlightedAnswer++;
                                if (HighlightedAnswer > AllAnswers.Count)
                                    HighlightedAnswer = 1;
                                showcaseLine = 0;
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineArrowPressed(mouse))
                                {
                                    UpdatePositionBasedOnArrowPress(mouse);
                                    showcaseLine = 0;
                                }
                                else if (DetermineSidebarArrowPressed(mouse))
                                    UpdateSidebarPositionBasedOnArrowPress(mouse);
                                else
                                {
                                    showcaseLine = 0;
                                    if (UpdateSelectedIndexWithMousePos(mouse))
                                    {
                                        if (!multiple)
                                        {
                                            SelectedAnswers.Add(HighlightedAnswer - 1);
                                            bail = true;
                                        }
                                        else
                                        {
                                            if (!SelectedAnswers.Remove(HighlightedAnswer - 1))
                                                SelectedAnswers.Add(HighlightedAnswer - 1);
                                        }
                                    }
                                }
                                break;
                            case PointerButton.Right:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                string choiceName = highlightedAnswer.ChoiceName;
                                string choiceTitle = highlightedAnswer.ChoiceTitle;
                                string choiceDesc = highlightedAnswer.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal("Item info", finalSidebarText);
                                    selectionScreen.RequireRefresh();
                                }
                                break;
                            case PointerButton.None:
                                if (mouse.ButtonPress != PointerButtonPress.Moved)
                                    break;
                                UpdateSelectedIndexWithMousePos(mouse);
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
                                HighlightedAnswer--;
                                if (HighlightedAnswer == 0)
                                    HighlightedAnswer = AllAnswers.Count;
                                showcaseLine = 0;
                                break;
                            case ConsoleKey.DownArrow:
                                HighlightedAnswer++;
                                if (HighlightedAnswer > AllAnswers.Count)
                                    HighlightedAnswer = 1;
                                showcaseLine = 0;
                                break;
                            case ConsoleKey.Home:
                                HighlightedAnswer = 1;
                                showcaseLine = 0;
                                break;
                            case ConsoleKey.End:
                                HighlightedAnswer = AllAnswers.Count;
                                showcaseLine = 0;
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                {
                                    int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                                    int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                                    int answersPerPage = listEndPosition - 5;
                                    var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
                                    int currentPage = HighlightedAnswer / answersPerPage;
                                    HighlightedAnswer -= choiceNums[currentPage];
                                    if (HighlightedAnswer < 1)
                                        HighlightedAnswer = 1;
                                    showcaseLine = 0;
                                }
                                break;
                            case ConsoleKey.PageDown:
                                {
                                    int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                                    int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                                    int answersPerPage = listEndPosition - 5;
                                    var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
                                    int currentPage = HighlightedAnswer / answersPerPage;
                                    HighlightedAnswer += choiceNums[currentPage];
                                    if (HighlightedAnswer > AllAnswers.Count)
                                        HighlightedAnswer = AllAnswers.Count;
                                    showcaseLine = 0;
                                }
                                break;
                            case ConsoleKey.Spacebar:
                                if (multiple)
                                {
                                    if (!SelectedAnswers.Remove(HighlightedAnswer - 1))
                                        SelectedAnswers.Add(HighlightedAnswer - 1);
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (!multiple)
                                    SelectedAnswers.Add(HighlightedAnswer - 1);
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                if (kiosk)
                                    break;
                                bail = true;
                                SelectedAnswers.Clear();
                                break;
                            case ConsoleKey.Tab:
                                string choiceName = highlightedAnswer.ChoiceName;
                                string choiceTitle = highlightedAnswer.ChoiceTitle;
                                string choiceDesc = highlightedAnswer.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal("Item info", finalSidebarText);
                                    selectionScreen.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.S:
                                sidebar = !sidebar;
                                selectionScreen.RequireRefresh();
                                break;
                            case ConsoleKey.A:
                                if (!multiple)
                                    break;
                                int selectionMode =
                                    key.Modifiers.HasFlag(ConsoleModifiers.Shift) && key.Modifiers.HasFlag(ConsoleModifiers.Control) ? 3 :
                                    key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? 2 : 1;
                                ProcessSelectionRequest(selectionMode, HighlightedAnswer, categories, ref SelectedAnswers);
                                showcaseLine = 0;
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (AllAnswers.Count <= 0)
                                    break;
                                var entriesString = AllAnswers.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (supports regular expressions)");
                                if (!RegexTools.IsValidRegex(keyword))
                                {
                                    InfoBoxModalColor.WriteInfoBoxModal("Your query is not a valid regular expression.");
                                    ScreenTools.CurrentScreen?.RequireRefresh();
                                    break;
                                }
                                var regex = new Regex(keyword);
                                var resultEntries = entriesString
                                    .Where((entry) => regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle))
                                    .Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
                                if (resultEntries.Length > 0)
                                {
                                    var choices = InputChoiceTools.GetInputChoices(resultEntries);
                                    int answer = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Select one of the entries:");
                                    if (answer < 0)
                                        break;
                                    var resultIdx = int.Parse(resultEntries[answer].Item1);
                                    HighlightedAnswer = resultIdx;
                                    showcaseLine = 0;
                                }
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal("No item found.");
                                selectionScreen.RequireRefresh();
                                break;
                            case ConsoleKey.K:
                                // Add base bindings
                                bool isExtendable = !string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription);

                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(bindings);
                                break;
                            case ConsoleKey.E:
                                // Remove one from the line count
                                showcaseLine--;
                                if (showcaseLine < 0)
                                    showcaseLine = 0;
                                break;
                            case ConsoleKey.D:
                                // Add one to the line count
                                {
                                    int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                                    int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                                    int answersPerPage = listEndPosition - 5;
                                    string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
                                    if (lines.Length <= answersPerPage)
                                        break;
                                    showcaseLine++;
                                    if (showcaseLine > lines.Length - answersPerPage)
                                        showcaseLine = lines.Length - answersPerPage;
                                }
                                break;
                            case ConsoleKey.P:
                                showCount = !showCount;
                                break;
                        }
                    }

                    // Verify that the current position is not a disabled choice
                    if (HighlightedAnswer > 0)
                    {
                        while (AllAnswers[HighlightedAnswer - 1].ChoiceDisabled)
                        {
                            if (goingUp)
                            {
                                HighlightedAnswer--;
                                if (HighlightedAnswer == 0)
                                    HighlightedAnswer = AllAnswers.Count;
                            }
                            else
                            {
                                HighlightedAnswer++;
                                if (HighlightedAnswer > AllAnswers.Count)
                                    HighlightedAnswer = 1;
                            }
                        }
                    }

                    // Reset, in case selection changed
                    selectionScreen.RemoveBufferedPart(screenPart.Id);
                }
                ConsoleWrapper.CursorVisible = initialVisible;
                ColorTools.LoadBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to initialize the selection input:" + $" {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                InfoBoxModalColor.WriteInfoBoxModal("Failed to initialize the selection input:" + $" {ex.Message}");
            }
            ScreenTools.UnsetCurrent(selectionScreen);
            SelectedAnswers.Sort();
            return [.. SelectedAnswers];
        }

        private static void ProcessSelectionRequest(int mode, int choiceNum, InputChoiceCategoryInfo[] categories, ref List<int> SelectedAnswers)
        {
            List<InputChoiceInfo> allAnswers = SelectionInputTools.GetChoicesFromCategories(categories);
            var choiceGroup = SelectionInputTools.GetCategoryGroupFrom(choiceNum, categories);
            var enabledAnswers = allAnswers.Select((ici, idx) => (ici, idx)).Where((ici) => !ici.ici.ChoiceDisabled).Select((tuple) => tuple.idx).ToArray();
            switch (mode)
            {
                case 1:
                    {
                        var category = choiceGroup.Item1;
                        var group = choiceGroup.Item2;
                        var choices = group.Choices;
                        List<int> indexes = [];
                        for (int i = 0; i < allAnswers.Count; i++)
                        {
                            var answer = allAnswers[i];
                            foreach (var choice in choices)
                            {
                                if (choice == answer && !choice.ChoiceDisabled)
                                    indexes.Add(i);
                            }
                        }

                        bool clear = DetermineClear(indexes, SelectedAnswers);
                        if (clear)
                        {
                            foreach (int index in indexes)
                                SelectedAnswers.Remove(index);
                        }
                        else
                        {
                            foreach (int index in indexes)
                            {
                                if (!SelectedAnswers.Contains(index))
                                    SelectedAnswers.Add(index);
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        var category = choiceGroup.Item1;
                        var groups = category.Groups;
                        List<int> indexes = [];
                        foreach (var group in groups)
                        {
                            var choices = group.Choices;
                            for (int i = 0; i < allAnswers.Count; i++)
                            {
                                var answer = allAnswers[i];
                                foreach (var choice in choices)
                                {
                                    if (choice == answer && !choice.ChoiceDisabled)
                                        indexes.Add(i);
                                }
                            }
                        }

                        bool clear = DetermineClear(indexes, SelectedAnswers);
                        if (clear)
                        {
                            foreach (int index in indexes)
                                SelectedAnswers.Remove(index);
                        }
                        else
                        {
                            foreach (int index in indexes)
                            {
                                if (!SelectedAnswers.Contains(index))
                                    SelectedAnswers.Add(index);
                            }
                        }
                    }
                    break;
                case 3:
                    bool unselect = SelectedAnswers.Count == enabledAnswers.Count();
                    if (unselect)
                        SelectedAnswers.Clear();
                    else if (SelectedAnswers.Count == 0)
                        SelectedAnswers.AddRange(enabledAnswers);
                    else
                    {
                        // We need to use Except here to avoid wasting CPU cycles, since we could be dealing with huge data.
                        var unselected = enabledAnswers.Except(SelectedAnswers);
                        SelectedAnswers.AddRange(unselected);
                    }
                    break;
            }
        }

        private static bool DetermineClear(List<int> indexes, List<int> selectedAnswers)
        {
            int found = 0;
            foreach (int selectedAnswer in selectedAnswers)
            {
                if (indexes.Contains(selectedAnswer))
                    found++;
            }
            return found == indexes.Count;
        }

        static SelectionStyleBase()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
