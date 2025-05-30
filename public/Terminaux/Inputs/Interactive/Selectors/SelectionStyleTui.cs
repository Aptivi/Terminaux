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
using Terminaux.Colors;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.Tools;

namespace Terminaux.Inputs.Interactive.Selectors
{
    internal class SelectionStyleTui : TextualUI
    {
        private int highlightedAnswer = 1;
        private int selectedAnswer = 1;
        private int questionLine;
        private int showcaseLine;
        private bool showCount;
        private bool sidebar;
        private List<int> selectedAnswers = [];
        private List<(string text, Color fore, Color back, bool force, ChoiceHitboxType type, int related)> choiceText = [];
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
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;

            // Get choice numbers and some positions
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            var selectionBuilder = new StringBuilder();

            // Prepare the border
            var border = new BoxFrame()
            {
                Left = 2,
                Top = 1,
                Width = interiorWidth,
                Height = answersPerPage + listStartPosition - 1,
                FrameColor = settings.SeparatorColor,
                BackgroundColor = settings.BackgroundColor,
                Rulers = sentenceLineCount > 0 ? [new(totalHeight, RulerOrientation.Horizontal)] : [],
            };
            selectionBuilder.Append(border.Render());

            // Write the question in a bordered box.
            if (sentenceLineCount > 0)
            {
                var boundedQuestion = new BoundedText()
                {
                    Left = 3,
                    Top = 2,
                    Width = interiorWidth,
                    Height = totalHeight,
                    ForegroundColor = settings.QuestionColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = questionLine,
                    Text = question,
                };
                selectionBuilder.Append(boundedQuestion.Render());
            }

            // Populate the answers
            var selections = new Selection(categories)
            {
                Left = 3,
                Top = listStartPosition + 1,
                ShowRadioButtons = settings.RadioButtons,
                CurrentSelection = highlightedAnswer - 1,
                SelectedChoice = selectedAnswer - 1,
                CurrentSelections = multiple ? [.. selectedAnswers] : null,
                Height = answersPerPage,
                Width = interiorWidth,
                Settings = settings,
            };
            selectionBuilder.Append(selections.Render());
            choiceText = selections.GetChoiceParameters().choiceText;

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
            if (sidebarEnabled)
            {
                string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
                string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
                var boundedSidebar = new BoundedText()
                {
                    Left = interiorWidth + 6,
                    Top = 2,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage + totalHeight + 1,
                    ForegroundColor = settings.TextColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = showcaseLine,
                    Text = finalSidebarText,
                };
                var sidebarBorder = new Border()
                {
                    Left = interiorWidth + 5,
                    Top = 1,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage + totalHeight + 1,
                    Color = settings.SeparatorColor,
                    BackgroundColor = settings.BackgroundColor,
                };
                selectionBuilder.Append(
                    sidebarBorder.Render() +
                    boundedSidebar.Render()
                );
                if (lines.Length > answersPerPage + totalHeight + 1)
                {
                    var dataSlider = new Slider(showcaseLine, 0, lines.Length - answersPerPage + totalHeight - 2)
                    {
                        Vertical = true,
                        Height = answersPerPage + totalHeight - 1,
                        SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderActiveForegroundColor = settings.SeparatorColor,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(settings.SeparatorColor),
                        SliderBackgroundColor = settings.BackgroundColor,
                    };
                    selectionBuilder.Append(
                        TextWriterWhereColor.RenderWhere("▲", interiorWidth + 3 + sidebarWidth, 2) +
                        TextWriterWhereColor.RenderWhere("▼", interiorWidth + 3 + sidebarWidth, listStartPosition + answersPerPage) +
                        RendererTools.RenderRenderable(dataSlider, new(interiorWidth + 3 + sidebarWidth, 3))
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
            {
                if (settings.RadioButtons)
                    selectedAnswers.Add(selectedAnswer - 1);
                else
                    selectedAnswers.Add(highlightedAnswer - 1);
            }
            TextualUITools.ExitTui(ui);
        }

        private void GoUp()
        {
            highlightedAnswer--;
            if (highlightedAnswer < 1)
                highlightedAnswer = 1;
            Update(true);
        }

        private void GoDown()
        {
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
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            int currentPage = SelectionInputTools.DetermineCurrentPage(categories, answersPerPage, highlightedAnswer) - 1;
            highlightedAnswer -= currentPage != -1 ? choiceNums[currentPage] : choiceNums[0];
            if (highlightedAnswer < 1)
                highlightedAnswer = 1;
            Update(true);
        }

        private void NextPage(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
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
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            if (!sidebarEnabled)
                return;
            showcaseLine--;
            if (showcaseLine < 0)
                showcaseLine = 0;
        }

        private void ShowcaseGoDown()
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            if (!sidebarEnabled)
                return;

            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
            if (lines.Length <= answersPerPage)
                return;
            showcaseLine++;
            if (showcaseLine > lines.Length - answersPerPage - totalHeight - 1)
                showcaseLine = lines.Length - answersPerPage - totalHeight - 1;
        }

        private void QuestionGoUp(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            if (sentenceLineCount <= 5)
                return;
            questionLine--;
            if (questionLine < 0)
                questionLine = 0;
        }

        private void QuestionGoDown(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
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
            if (mouse is not null)
            {
                if (!UpdateSelectedIndexWithMousePos(mouse, out ChoiceHitboxType hitboxType) || hitboxType != ChoiceHitboxType.Choice)
                    return;
            }
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string choiceDesc = highlightedAnswerChoiceInfo.ChoiceDescription;
            if (!string.IsNullOrWhiteSpace(choiceDesc))
            {
                string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
                InfoBoxModalColor.WriteInfoBoxModal(finalSidebarText, new InfoBoxSettings()
                {
                    Title = "Item info"
                });
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

        private void ProcessSelect()
        {
            if (settings.RadioButtons)
                selectedAnswer = highlightedAnswer;
        }

        private void ProcessSelectAll(int selectionMode) =>
            InfoBoxTools.ProcessSelectionRequest(selectionMode, highlightedAnswer, categories, ref selectedAnswers);

        private void ProcessLeftClick(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (mouse is null)
                return;

            // Get some essential variables
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);

            // Make hitboxes for arrow presses
            var arrowUpHitbox = new PointerHitbox(new(interiorWidth + 3, listStartPosition + 1), new Action<PointerEventContext>((_) => GoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var arrowDownHitbox = new PointerHitbox(new(interiorWidth + 3, listStartPosition + answersPerPage), new Action<PointerEventContext>((_) => GoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowUpHitbox = new PointerHitbox(new(interiorWidth + 3 + sidebarWidth, 2), new Action<PointerEventContext>((_) => ShowcaseGoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowDownHitbox = new PointerHitbox(new(interiorWidth + 3 + sidebarWidth, listStartPosition + answersPerPage), new Action<PointerEventContext>((_) => ShowcaseGoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && choiceText.Count > answersPerPage)
            {
                arrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    arrowDownHitbox.ProcessPointer(mouse, out done);
                showcaseLine = 0;
            }
            else if ((sidebarArrowUpHitbox.IsPointerWithin(mouse) || sidebarArrowDownHitbox.IsPointerWithin(mouse)) && lines.Length > answersPerPage + totalHeight + 1 && sidebarEnabled)
            {
                sidebarArrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    sidebarArrowDownHitbox.ProcessPointer(mouse, out done);
            }
            else
            {
                int oldIndex = highlightedAnswer;
                if (UpdateSelectedIndexWithMousePos(mouse, out ChoiceHitboxType hitboxType))
                {
                    if (!multiple)
                    {
                        if (!settings.RadioButtons)
                            Exit(ui, false);
                        else
                            selectedAnswer = highlightedAnswer;
                    }
                    else
                    {
                        switch (hitboxType)
                        {
                            case ChoiceHitboxType.Category:
                                ProcessSelectAll(2);
                                highlightedAnswer = oldIndex;
                                break;
                            case ChoiceHitboxType.Group:
                                ProcessSelectAll(1);
                                highlightedAnswer = oldIndex;
                                break;
                            case ChoiceHitboxType.Choice:
                                ModifyChoice(ui, key, mouse);
                                if (highlightedAnswer != oldIndex)
                                    showcaseLine = 0;
                                break;
                        }
                    }
                }
            }
        }

        private void ProcessMouseWheel(PointerEventContext? mouse, bool goingUp = false)
        {
            if (mouse is null)
                return;
            if (mouse.ButtonPress != PointerButtonPress.Scrolled)
                return;

            // Get some essential variables
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer - 1];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);

            // Check to see if we're scrolling the mouse wheel or not
            if (mouse.Coordinates.x < 3 || mouse.Coordinates.x >= interiorWidth + 3 ||
                mouse.Coordinates.y < listStartPosition + 1 || mouse.Coordinates.y >= listStartPosition + answersPerPage)
            {
                // It's possible that the user may be scrolling in the sidebar, but check the coordinates
                if (!sidebarEnabled)
                    return;
                if (mouse.Coordinates.x < interiorWidth + 6 || mouse.Coordinates.x >= interiorWidth + sidebarWidth + 3 ||
                    mouse.Coordinates.y < 2 || mouse.Coordinates.y >= listStartPosition + answersPerPage)
                    return;

                // Check the lines
                if (lines.Length <= answersPerPage)
                    return;

                // Now, scroll the showcase if possible
                if (goingUp)
                    ShowcaseGoUp();
                else
                    ShowcaseGoDown();
            }
            else
            {
                if (goingUp)
                    GoUp();
                else
                    GoDown();
            }
        }

        private bool UpdateSelectedIndexWithMousePos(PointerEventContext? mouse, out ChoiceHitboxType hitboxType)
        {
            hitboxType = ChoiceHitboxType.Choice;
            if (mouse is null)
                return false;

            // Make pages based on console window height
            int wholeWidth = ConsoleWrapper.WindowWidth - 6;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth;
            int sentenceLineCount = ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = ConsoleChar.EstimateCellWidth(question) > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 4;

            // Determine the hitbox types
            var selections = new Selection(categories)
            {
                Left = 3,
                Top = listStartPosition + 1,
                ShowRadioButtons = settings.RadioButtons,
                CurrentSelection = highlightedAnswer - 1,
                SelectedChoice = selectedAnswer - 1,
                CurrentSelections = multiple ? [.. selectedAnswers] : null,
                Height = answersPerPage,
                Width = interiorWidth,
                Settings = settings,
            };

            // Now, translate coordinates to the selected index and get its hitbox
            if (mouse.Coordinates.x <= 2 || mouse.Coordinates.x > interiorWidth + 2)
                return false;
            if (mouse.Coordinates.y < listStartPosition + 1 || mouse.Coordinates.y > listStartPosition + answersPerPage)
                return false;
            int listIndex = mouse.Coordinates.y - listStartPosition - 1;
            if (!selections.CanGenerateSelectionHitbox(listIndex, out var hitbox))
                return false;

            // Depending on the hitbox parameter, we need to act accordingly
            var highlightedAnswerChoiceInfo = allAnswers[hitbox.related - 1];
            if (highlightedAnswerChoiceInfo.ChoiceDisabled && hitbox.type == ChoiceHitboxType.Choice)
                return false;
            if (!highlightedAnswerChoiceInfo.ChoiceDisabled || hitbox.type != ChoiceHitboxType.Choice)
                highlightedAnswer = hitbox.related;
            hitboxType = hitbox.type;
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
            if (selectedAnswer > allAnswers.Count)
                selectedAnswer = 1;
            selectedAnswer = allAnswers.Any((ici) => ici.ChoiceDefault) ? allAnswers.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx + 1 : 1;

            // Install keybindings
            Keybindings.Add((SelectionStyleBase.bindings[0], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((SelectionStyleBase.bindings[1], (_, _, _) => ProcessSelect()));
            Keybindings.Add((SelectionStyleBase.bindings[2], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((SelectionStyleBase.bindings[3], (_, _, _) => GoUp()));
            Keybindings.Add((SelectionStyleBase.bindings[4], (_, _, _) => GoDown()));
            Keybindings.Add((SelectionStyleBase.bindings[5], GoFirst));
            Keybindings.Add((SelectionStyleBase.bindings[6], GoLast));
            Keybindings.Add((SelectionStyleBase.bindings[7], PreviousPage));
            Keybindings.Add((SelectionStyleBase.bindings[8], NextPage));
            Keybindings.Add((SelectionStyleBase.bindings[9], SearchPrompt));
            Keybindings.Add((SelectionStyleBase.bindings[10], (_, _, _) => ShowcaseGoUp()));
            Keybindings.Add((SelectionStyleBase.bindings[11], (_, _, _) => ShowcaseGoDown()));
            Keybindings.Add((SelectionStyleBase.bindings[12], QuestionGoUp));
            Keybindings.Add((SelectionStyleBase.bindings[13], QuestionGoDown));
            Keybindings.Add((SelectionStyleBase.bindings[14], ShowCount));
            Keybindings.Add((SelectionStyleBase.bindings[15], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.showBindings[1], ShowSidebar));
            Keybindings.Add((SelectionStyleBase.showBindings[3], Help));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[0], (_, _, mouse) => ProcessMouseWheel(mouse, true)));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[1], (_, _, mouse) => ProcessMouseWheel(mouse)));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[2], ProcessLeftClick));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[3], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.bindingsMouse[4], (_, _, mouse) => UpdateSelectedIndexWithMousePos(mouse, out _)));

            // Install mode-dependent keybindings
            if (multiple)
            {
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[16], ModifyChoice));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[17], (_, _, _) => ProcessSelectAll(1)));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[18], (_, _, _) => ProcessSelectAll(2)));
                Keybindings.Add((SelectionStyleBase.bindingsMultiple[19], (_, _, _) => ProcessSelectAll(3)));
            }
        }
    }
}
