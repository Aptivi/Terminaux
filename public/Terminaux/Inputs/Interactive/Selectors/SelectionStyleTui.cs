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
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    internal class SelectionStyleTui : TextualUI
    {
        private int highlightedAnswer = 1;
        private int questionLine;
        private int showcaseLine;
        private bool showCount;
        private bool sidebar;
        private List<int> selectedAnswers = [];
        private readonly string question = "";
        private readonly InputChoiceCategoryInfo[] answers = [];
        private readonly SelectionStyleSettings settings = SelectionStyleSettings.GlobalSettings;
        private readonly bool kiosk;
        private readonly bool multiple;
        private readonly InputChoiceCategoryInfo[] categories = [];
        private readonly List<InputChoiceInfo> allAnswers = [];

        public override string Render()
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            int sidebarWidth = sidebar ? wholeWidth / 4 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;

            // Get choice numbers and some positions
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, wholeWidth).Length;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? sentenceLineCount > 5 ? 7 : sentenceLineCount + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 5;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            var selectionBuilder = new StringBuilder();

            // Write the question in a bordered box.
            if (sentenceLineCount > 0)
            {
                var boundedQuestion = new BoundedText()
                {
                    Left = 3,
                    Top = 1,
                    Width = wholeWidth,
                    Height = sentenceLineCount > 5 ? 5 : sentenceLineCount,
                    ForegroundColor = settings.QuestionColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = questionLine,
                    Text = question,
                };
                var questionText = new Border()
                {
                    Color = settings.SeparatorColor,
                    BackgroundColor = settings.BackgroundColor,
                    Left = 2,
                    Top = 1,
                    Width = wholeWidth,
                    Height = sentenceLineCount > 5 ? 5 : sentenceLineCount
                };
                selectionBuilder.Append(questionText.Render());
                selectionBuilder.Append(boundedQuestion.Render());
            }

            // Populate the answers
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(categories);
            var border = new BoxFrame()
            {
                Left = 2,
                Top = listStartPosition + 1,
                Width = interiorWidth,
                Height = answersPerPage,
                FrameColor = settings.SeparatorColor,
                BackgroundColor = settings.BackgroundColor,
                Settings = new()
                {
                    BorderRightFrameEnabled = choices.Count <= answersPerPage,
                }
            };
            var selections = new Selection(categories)
            {
                Left = 3,
                Top = listStartPosition + 2,
                CurrentSelection = highlightedAnswer - 1,
                CurrentSelections = multiple ? [.. selectedAnswers] : null,
                Height = answersPerPage,
                Width = interiorWidth,
                AltChoicePos = SelectionInputTools.GetChoicesFromCategories(answers).Count,
                Settings = settings,
            };
            selectionBuilder.Append(
                border.Render() +
                selections.Render()
            );

            // Write description hint
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            if (showCount)
            {
                int currentPage = SelectionInputTools.DetermineCurrentPage(categories, answersPerPage, highlightedAnswer);
                string renderedHint = (showCount ? $"[{(multiple ? $"{selectedAnswers.Count} | " : "")}{currentPage + 1}/{choiceNums.Count} | {highlightedAnswer}/{allAnswers.Count}]" : "");
                int descHintAreaX = interiorWidth - ConsoleChar.EstimateCellWidth(renderedHint) + 2;
                int descHintAreaY = ConsoleWrapper.WindowHeight - 3;
                selectionBuilder.Append(
                    TextWriterWhereColor.RenderWhereColor(renderedHint, descHintAreaX, descHintAreaY, settings.OptionColor)
                );
            }

            // Render a sidebar
            if (sidebar)
            {
                string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
                string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
                var boundedSidebar = new BoundedText()
                {
                    Left = interiorWidth + 6,
                    Top = listStartPosition + 1,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage,
                    ForegroundColor = settings.TextColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = showcaseLine,
                    Text = finalSidebarText,
                };
                var sidebarBorder = new Border()
                {
                    Left = interiorWidth + 5,
                    Top = listStartPosition + 1,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage,
                    Color = settings.SeparatorColor,
                    BackgroundColor = settings.BackgroundColor,
                };
                selectionBuilder.Append(
                    sidebarBorder.Render() +
                    boundedSidebar.Render()
                );
                if (lines.Length > answersPerPage)
                {
                    var dataSlider = new Slider(showcaseLine, 0, lines.Length)
                    {
                        Vertical = true,
                        Height = answersPerPage - 2,
                        SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderActiveForegroundColor = settings.SeparatorColor,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(settings.SeparatorColor),
                        SliderBackgroundColor = settings.BackgroundColor,
                    };
                    selectionBuilder.Append(
                        TextWriterWhereColor.RenderWhere("▲", interiorWidth + 3 + sidebarWidth, listStartPosition + 2) +
                        TextWriterWhereColor.RenderWhere("▼", interiorWidth + 3 + sidebarWidth, listStartPosition + answersPerPage + 1) +
                        RendererTools.RenderRenderable(dataSlider, new(interiorWidth + 3 + sidebarWidth, listStartPosition + 3))
                    );
                }
            }

            // Render keybindings
            var keybindingsRenderable = new Keybindings()
            {
                KeybindingList = multiple ? SelectionStyleBase.showBindingsMultiple : SelectionStyleBase.showBindings,
                Width = ConsoleWrapper.WindowWidth - 1,
                BackgroundColor = settings.BackgroundColor,
                WriteHelpKeyInfo = false,
            };
            selectionBuilder.Append(RendererTools.RenderRenderable(keybindingsRenderable, new(0, ConsoleWrapper.WindowHeight - 1)));
            return selectionBuilder.ToString();
        }

        internal int[] GetResultingChoices()
        {
            selectedAnswers.Sort();
            return [.. selectedAnswers];
        }

        private void Update(bool goingUp)
        {
            // Edge case: We need to check to see if the current highlight is disabled
            while (allAnswers[highlightedAnswer - 1].ChoiceDisabled)
            {
                if (goingUp)
                {
                    highlightedAnswer--;
                    if (highlightedAnswer == 0)
                        highlightedAnswer = allAnswers.Count;
                }
                else
                {
                    highlightedAnswer++;
                    if (highlightedAnswer > allAnswers.Count)
                        highlightedAnswer = 1;
                }
            }
            showcaseLine = 0;
        }

        private void Exit(TextualUI ui, bool cancel)
        {
            if (cancel)
            {
                if (kiosk)
                    return;
                selectedAnswers.Clear();
            }
            else if (!multiple)
                selectedAnswers.Add(highlightedAnswer - 1);
            TextualUITools.ExitTui(ui);
        }

        private void GoUp(PointerEventContext? mouse = null)
        {
            // Check the mouse first
            if (mouse is not null)
            {
                int wholeWidth = ConsoleWrapper.WindowWidth - 6;
                int sidebarWidth = sidebar ? wholeWidth / 4 : 0;
                int interiorWidth = wholeWidth - sidebarWidth;
                int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, wholeWidth).Length;
                int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? sentenceLineCount > 5 ? 7 : sentenceLineCount + 2 : 1;
                int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                int answersPerPage = listEndPosition - 5;

                // Check to see if we're scrolling the mouse wheel or not
                if (mouse.ButtonPress == PointerButtonPress.Scrolled &&
                    (mouse.Coordinates.x < 3 || mouse.Coordinates.x >= interiorWidth + 3 ||
                     mouse.Coordinates.y < listStartPosition + 2 || mouse.Coordinates.y >= listStartPosition + 2 + answersPerPage))
                    return;
            }

            // Now, we can go up
            highlightedAnswer--;
            if (highlightedAnswer < 1)
                highlightedAnswer = 1;
            Update(true);
        }

        private void GoDown(PointerEventContext? mouse = null)
        {
            // Check the mouse first
            if (mouse is not null)
            {
                int wholeWidth = ConsoleWrapper.WindowWidth - 6;
                int sidebarWidth = sidebar ? wholeWidth / 4 : 0;
                int interiorWidth = wholeWidth - sidebarWidth;
                int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, wholeWidth).Length;
                int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? sentenceLineCount > 5 ? 7 : sentenceLineCount + 2 : 1;
                int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                int answersPerPage = listEndPosition - 5;

                // Check to see if we're scrolling the mouse wheel or not
                if (mouse.ButtonPress == PointerButtonPress.Scrolled &&
                    (mouse.Coordinates.x < 3 || mouse.Coordinates.x >= interiorWidth + 3 ||
                     mouse.Coordinates.y < listStartPosition + 2 || mouse.Coordinates.y >= listStartPosition + 2 + answersPerPage))
                    return;
            }

            // Now, we can go down
            highlightedAnswer++;
            if (highlightedAnswer > allAnswers.Count)
                highlightedAnswer = allAnswers.Count;
            Update(false);
        }

        private void GoFirst(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            highlightedAnswer = 1;
            Update(true);
        }

        private void GoLast(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            highlightedAnswer = allAnswers.Count;
            Update(false);
        }

        private void PreviousPage(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 5;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            int currentPage = SelectionInputTools.DetermineCurrentPage(categories, answersPerPage, highlightedAnswer) - 1;
            highlightedAnswer -= currentPage != -1 ? choiceNums[currentPage] : choiceNums[0];
            if (highlightedAnswer < 1)
                highlightedAnswer = 1;
            Update(true);
        }

        private void NextPage(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 5;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            int currentPage = SelectionInputTools.DetermineCurrentPage(categories, answersPerPage, highlightedAnswer);
            highlightedAnswer += choiceNums[currentPage];
            if (highlightedAnswer > allAnswers.Count)
                highlightedAnswer = allAnswers.Count;
            Update(false);
        }

        private void SearchPrompt(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            // Prompt the user for search term
            var entriesString = allAnswers.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
            string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (supports regular expressions)");
            if (!RegexTools.IsValidRegex(keyword))
            {
                InfoBoxModalColor.WriteInfoBoxModal("Your query is not a valid regular expression.");
                ui.RequireRefresh();
                return;
            }

            // Get the result entries
            var regex = new Regex(keyword);
            var resultEntries = entriesString
                .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, itemNum: idx + 1))
                .Where((entry) => regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)).ToArray();

            // Act, depending on the result entries
            int idx = 0;
            if (resultEntries.Length > 1)
            {
                var choices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.ChoiceName, tuple.ChoiceTitle)).ToArray();
                idx = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, "Select one of the entries:");
                if (idx < 0)
                {
                    ui.RequireRefresh();
                    return;
                }
            }
            else if (resultEntries.Length == 1)
                idx = 0;
            else
                InfoBoxModalColor.WriteInfoBoxModal("No item found.");

            // Change the highlighted answer number
            var resultNum = idx >= resultEntries.Length ? highlightedAnswer : resultEntries[idx].itemNum;
            highlightedAnswer = resultNum;

            // Update the TUI
            Update(false);
            ui.RequireRefresh();
        }

        private void ShowcaseGoUp()
        {
            if (!sidebar)
                return;
            showcaseLine--;
            if (showcaseLine < 0)
                showcaseLine = 0;
        }

        private void ShowcaseGoDown()
        {
            if (!sidebar)
                return;
            int sidebarWidth = sidebar ? (ConsoleWrapper.WindowWidth - 6) / 4 : 0;
            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 5;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
            if (lines.Length <= answersPerPage)
                return;
            showcaseLine++;
            if (showcaseLine > lines.Length - answersPerPage)
                showcaseLine = lines.Length - answersPerPage;
        }

        private void QuestionGoUp(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, wholeWidth).Length;
            if (sentenceLineCount <= 5)
                return;
            questionLine--;
            if (questionLine < 0)
                questionLine = 0;
        }

        private void QuestionGoDown(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, wholeWidth).Length;
            if (sentenceLineCount <= 5)
                return;
            questionLine++;
            if (questionLine + 5 > sentenceLineCount)
                questionLine = sentenceLineCount - 5;
        }

        private void ShowCount(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse) =>
            showCount = !showCount;

        private void ShowItemInfo(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string choiceDesc = highlightedAnswerChoiceInfo.ChoiceDescription;
            if (!string.IsNullOrWhiteSpace(choiceDesc))
            {
                string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
                InfoBoxModalColor.WriteInfoBoxModal("Item info", finalSidebarText);
                ui.RequireRefresh();
            }
        }

        private void ShowSidebar(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            sidebar = !sidebar;
            ui.RequireRefresh();
        }

        private void Help(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            Keybinding[] allBindings = multiple ? SelectionStyleBase.bindingsMultiple : SelectionStyleBase.bindings;
            KeybindingTools.ShowKeybindingInfobox(allBindings);
            ui.RequireRefresh();
        }

        private void ModifyChoice(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (!selectedAnswers.Remove(highlightedAnswer - 1))
                selectedAnswers.Add(highlightedAnswer - 1);
        }

        private void ProcessSelectAll(int selectionMode) =>
            ProcessSelectionRequest(selectionMode, highlightedAnswer, categories, ref selectedAnswers);

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

        private void ProcessLeftClick(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (mouse is null)
                return;

            // Get some essential variables
            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 5;
            int sidebarWidth = sidebar ? (ConsoleWrapper.WindowWidth - 6) / 4 : 0;
            int interiorWidth = ConsoleWrapper.WindowWidth - 6 - sidebarWidth;

            // Make hitboxes for arrow presses
            var arrowUpHitbox = new PointerHitbox(new(interiorWidth + 3, listStartPosition + 4), new Action<PointerEventContext>((_) => GoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var arrowDownHitbox = new PointerHitbox(new(interiorWidth + 3, listStartPosition + 1 + answersPerPage), new Action<PointerEventContext>((_) => GoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowUpHitbox = new PointerHitbox(new(ConsoleWrapper.WindowWidth - 3, listStartPosition + 4), new Action<PointerEventContext>((_) => ShowcaseGoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowDownHitbox = new PointerHitbox(new(ConsoleWrapper.WindowWidth - 3, listStartPosition + 1 + answersPerPage), new Action<PointerEventContext>((_) => ShowcaseGoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && allAnswers.Count > answersPerPage)
            {
                arrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    arrowDownHitbox.ProcessPointer(mouse, out done);
                showcaseLine = 0;
            }
            else if ((sidebarArrowUpHitbox.IsPointerWithin(mouse) || sidebarArrowDownHitbox.IsPointerWithin(mouse)) && allAnswers.Count > answersPerPage && sidebar)
            {
                sidebarArrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    sidebarArrowDownHitbox.ProcessPointer(mouse, out done);
            }
            else
            {
                showcaseLine = 0;
                if (UpdateSelectedIndexWithMousePos(mouse))
                {
                    if (!multiple)
                        Exit(ui, false);
                    else
                        ModifyChoice(ui, key, mouse);
                }
            }
        }

        private bool UpdateSelectedIndexWithMousePos(PointerEventContext? mouse)
        {
            if (mouse is null)
                return false;
            if (mouse.Coordinates.x <= 2 || mouse.Coordinates.x >= ConsoleWrapper.WindowWidth - 1)
                return false;

            // Make pages based on console window height
            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 7;
            int currentPage = (highlightedAnswer - 1) / answersPerPage;
            int startIndex = answersPerPage * currentPage;

            // Now, translate coordinates to the selected index
            if (mouse.Coordinates.y <= listStartPosition + 4 || mouse.Coordinates.y >= listEndPosition - 4)
                return false;
            int listIndex = mouse.Coordinates.y - (listStartPosition + 4);
            listIndex = startIndex + listIndex;
            listIndex = listIndex > allAnswers.Count ? allAnswers.Count : listIndex;
            highlightedAnswer = listIndex;
            return true;
        }

        internal SelectionStyleTui(string question, InputChoiceCategoryInfo[] answers, InputChoiceCategoryInfo[] altAnswers, SelectionStyleSettings settings, bool kiosk, bool multiple)
        {
            // Check values
            categories = [.. answers, .. altAnswers];
            allAnswers = SelectionInputTools.GetChoicesFromCategories(categories);
            if (allAnswers.All((ici) => ici.ChoiceDisabled))
                throw new TerminauxException("The selection style requires that there is at least one choice enabled.");

            // Install values
            this.question = question;
            this.answers = answers;
            this.settings = settings ?? SelectionStyleSettings.GlobalSettings;
            this.kiosk = kiosk;
            this.multiple = multiple;

            // Make selected choices from the ChoiceDefaultSelected value.
            selectedAnswers = allAnswers.Any((ici) => ici.ChoiceDefaultSelected) ? allAnswers.Select((ici, idx) => (idx, ici.ChoiceDefaultSelected)).Where((tuple) => tuple.ChoiceDefaultSelected).Select((tuple) => tuple.idx + 1).ToList() : [];

            // Before we proceed, we need to check the highlighted answer number
            if (highlightedAnswer > allAnswers.Count)
                highlightedAnswer = 1;
            highlightedAnswer = allAnswers.Any((ici) => ici.ChoiceDefault) ? allAnswers.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx + 1 : 1;

            // Install keybindings
            Keybindings.Add((SelectionStyleBase.bindings[0], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((SelectionStyleBase.bindings[1], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((SelectionStyleBase.bindings[2], (_, _, _) => GoUp()));
            Keybindings.Add((SelectionStyleBase.bindings[3], (_, _, _) => GoDown()));
            Keybindings.Add((SelectionStyleBase.bindings[4], GoFirst));
            Keybindings.Add((SelectionStyleBase.bindings[5], GoLast));
            Keybindings.Add((SelectionStyleBase.bindings[6], PreviousPage));
            Keybindings.Add((SelectionStyleBase.bindings[7], NextPage));
            Keybindings.Add((SelectionStyleBase.bindings[8], SearchPrompt));
            Keybindings.Add((SelectionStyleBase.bindings[9], (_, _, _) => ShowcaseGoUp()));
            Keybindings.Add((SelectionStyleBase.bindings[10], (_, _, _) => ShowcaseGoDown()));
            Keybindings.Add((SelectionStyleBase.bindings[11], QuestionGoUp));
            Keybindings.Add((SelectionStyleBase.bindings[12], QuestionGoDown));
            Keybindings.Add((SelectionStyleBase.bindings[13], ShowCount));
            Keybindings.Add((SelectionStyleBase.bindings[14], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.showBindings[1], ShowSidebar));
            Keybindings.Add((SelectionStyleBase.showBindings[3], Help));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[0], (_, _, mouse) => GoUp(mouse)));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[1], (_, _, mouse) => GoDown(mouse)));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[2], ProcessLeftClick));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[3], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[4], (_, _, mouse) => UpdateSelectedIndexWithMousePos(mouse)));

            // Install mode-dependent keybindings
            if (multiple)
            {
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[15], ModifyChoice));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[16], (_, _, _) => ProcessSelectAll(1)));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[17], (_, _, _) => ProcessSelectAll(2)));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[18], (_, _, _) => ProcessSelectAll(3)));
            }
        }
    }
}
