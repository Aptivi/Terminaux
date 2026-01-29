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
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Base.Extensions;
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
        private int highlightedAnswer = 0;
        private int selectedAnswer = 0;
        private int questionLine;
        private int showcaseLine;
        private bool showCount;
        private bool sidebar;
        private List<int> selectedAnswers = [];
        private List<(ChoiceHitboxType type, int related)> relatedHeights = [];
        private readonly string question = "";
        private readonly Selection selection = new();
        private readonly SelectionStyleSettings settings = SelectionStyleSettings.GlobalSettings;
        private readonly bool kiosk;
        private readonly bool multiple;
        private readonly InputChoiceCategoryInfo[] categories = [];
        private readonly List<InputChoiceInfo> allAnswers = [];

        public override string Render()
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);

            // Get choice numbers and some positions
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;
            var choiceNums = SelectionInputTools.GetChoicePages(categories, answersPerPage);
            var selectionBuilder = new StringBuilder();

            // Prepare the border
            var border = new BoxFrame()
            {
                Left = 0,
                Top = 0,
                Text = settings.Title,
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
                var boundClear = new Box()
                {
                    Left = 1,
                    Top = 1,
                    Width = interiorWidth,
                    Height = totalHeight,
                    Color = settings.BackgroundColor,
                };
                var boundedQuestion = new BoundedText()
                {
                    Left = 1,
                    Top = 1,
                    Width = interiorWidth,
                    Height = totalHeight,
                    ForegroundColor = settings.QuestionColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = questionLine,
                    Text = question,
                };
                selectionBuilder.Append(boundClear.Render() + boundedQuestion.Render());
            }

            // Populate the answers
            selection.Top = listStartPosition;
            selection.Height = answersPerPage;
            selection.Width = interiorWidth;
            selection.ShowRadioButtons = settings.RadioButtons;
            selection.CurrentSelection = highlightedAnswer;
            selection.SelectedChoice = selectedAnswer;
            selection.CurrentSelections = multiple ? [.. selectedAnswers] : null;
            selection.Settings = settings;
            selectionBuilder.Append(selection.Render());
            relatedHeights = selection.GetRelatedHeights();

            // Write description hint
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer];
            if (showCount)
            {
                int currentPage = SelectionInputTools.DetermineCurrentPage(categories, answersPerPage, highlightedAnswer);
                string renderedHint = (showCount ? $"[{(multiple ? $"{selectedAnswers.Count} | " : "")}{currentPage + 1}/{choiceNums.Count} | {highlightedAnswer + 1}/{allAnswers.Count}]" : "");
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
                    Left = interiorWidth + 4,
                    Top = 1,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage + listStartPosition - 1,
                    ForegroundColor = settings.TextColor,
                    BackgroundColor = settings.BackgroundColor,
                    Line = showcaseLine,
                    Text = finalSidebarText,
                };
                var sidebarBorder = new Border()
                {
                    Left = interiorWidth + 3,
                    Top = 0,
                    Width = sidebarWidth - 3,
                    Height = answersPerPage + listStartPosition - 1,
                    Color = settings.SeparatorColor,
                    BackgroundColor = settings.BackgroundColor,
                };
                selectionBuilder.Append(
                    sidebarBorder.Render() +
                    boundedSidebar.Render()
                );
                if (lines.Length > answersPerPage + listStartPosition - 1)
                {
                    var dataSlider = new Slider(showcaseLine, 0, lines.Length - answersPerPage + totalHeight - 2)
                    {
                        Vertical = true,
                        Height = answersPerPage + totalHeight - 1,
                        SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                        SliderActiveForegroundColor = settings.SliderColor,
                        SliderForegroundColor = TransformationTools.GetDarkBackground(settings.SliderColor),
                        SliderBackgroundColor = settings.BackgroundColor,
                    };
                    selectionBuilder.Append(
                        TextWriterWhereColor.RenderWherePlain("▲", interiorWidth + 3 + sidebarWidth, 1) +
                        TextWriterWhereColor.RenderWherePlain("▼", interiorWidth + 3 + sidebarWidth, listStartPosition + answersPerPage - 1) +
                        RendererTools.RenderRenderable(dataSlider, new(interiorWidth + 3 + sidebarWidth, 2))
                    );
                }
            }

            // Render keybindings
            var keybindingsRenderable = new Keybindings()
            {
                KeybindingList = multiple ? SelectionStyleBase.ShowBindingsMultiple : SelectionStyleBase.ShowBindings,
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
            InfoBoxTools.VerifyDisabled(ref highlightedAnswer, categories, goingUp);
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
                    selectedAnswers.Add(selectedAnswer);
                else
                    selectedAnswers.Add(highlightedAnswer);
            }
            TextualUITools.ExitTui(ui);
        }

        private void GoUp(int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            highlightedAnswer -= factor;
            if (highlightedAnswer < 0)
                highlightedAnswer = factor == 1 ? allAnswers.Count - 1 : 0;
            Update(true);
        }

        private void GoDown(int factor = 1)
        {
            if (factor < 1)
                factor = 1;
            highlightedAnswer += factor;
            if (highlightedAnswer > allAnswers.Count - 1)
                highlightedAnswer = factor == 1 ? 0 : allAnswers.Count - 1;
            Update(false);
        }

        private void GoFirst(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            highlightedAnswer = 0;
            Update(true);
        }

        private void GoLast(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            highlightedAnswer = allAnswers.Count - 1;
            Update(false);
        }

        private void PreviousPage(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;

            // Use the rendered selection heights to go to the previous page
            var heights = selection.GetSelectionHeights();
            int processedHeight = 0;
            for (int h = highlightedAnswer; h > 0 && processedHeight < answersPerPage; h--)
            {
                int height = heights[h];
                int prevHeight = h - 1 < heights.Count ? heights[h - 1] : 0;
                highlightedAnswer--;
                if (highlightedAnswer < 0)
                    highlightedAnswer = 0;
                processedHeight += height - prevHeight;
            }
            Update(true);
        }

        private void NextPage(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;

            // Use the rendered selection heights to go to the next page
            var heights = selection.GetSelectionHeights();
            int processedHeight = 0;
            for (int h = highlightedAnswer; h < heights.Count && processedHeight < answersPerPage; h++)
            {
                int height = heights[h];
                int nextHeight = h + 1 < heights.Count ? heights[h + 1] : 0;
                highlightedAnswer++;
                if (highlightedAnswer > allAnswers.Count - 1)
                    highlightedAnswer = allAnswers.Count - 1;
                processedHeight += nextHeight - height > 0 ? nextHeight - height : 1;
            }
            Update(false);
        }

        private void SearchPrompt(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            // Prompt the user for search term
            var entriesString = allAnswers.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled)).ToArray();
            string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"));
            if (!RegexTools.IsValidRegex(keyword))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                return;
            }

            // Get the result entries
            var regex = new Regex(keyword);
            var resultEntries = entriesString
                .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled, itemIdx: idx))
                .Where((entry) => (regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)) && !entry.ChoiceDisabled).ToArray();

            // Act, depending on the result entries
            int idx = 0;
            if (resultEntries.Length > 1)
            {
                var choices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.ChoiceName, tuple.ChoiceTitle)).ToArray();
                idx = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"));
                if (idx < 0)
                    return;
            }
            else if (resultEntries.Length == 1)
                idx = 0;
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"));
                return;
            }

            // Change the highlighted answer number
            var resultNum = idx >= resultEntries.Length ? highlightedAnswer : resultEntries[idx].itemIdx;
            highlightedAnswer = resultNum;

            // Update the TUI
            Update(false);
        }

        private void ShowcaseGoUp(int factor = 1)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            if (!sidebarEnabled)
                return;
            if (factor < 1)
                factor = 1;
            showcaseLine -= factor;
            if (showcaseLine < 0)
                showcaseLine = 0;
        }

        private void ShowcaseGoDown(int factor = 1)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            if (!sidebarEnabled)
                return;

            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);
            if (lines.Length <= answersPerPage)
                return;
            if (factor < 1)
                factor = 1;
            showcaseLine += factor;
            if (showcaseLine > lines.Length - answersPerPage - totalHeight - 1)
                showcaseLine = lines.Length - answersPerPage - totalHeight - 1;
        }

        private void QuestionGoUp(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            if (sentenceLineCount <= 5)
                return;
            questionLine--;
            if (questionLine < 0)
                questionLine = 0;
        }

        private void QuestionGoDown(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
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
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer];
            string choiceDesc = highlightedAnswerChoiceInfo.ChoiceDescription;
            if (!string.IsNullOrWhiteSpace(choiceDesc))
            {
                string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
                InfoBoxModalColor.WriteInfoBoxModal(finalSidebarText, new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("T_INPUT_IS_SELECTION_ITEMINFO")
                });
                ui.RequireRefresh();
            }
        }

        private void ShowSidebar(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            sidebar = !sidebar;
            ui.RequireRefresh();
        }

        private void ModifyChoice(TextualUI ui, ConsoleKeyInfo key, PointerEventContext? mouse)
        {
            if (!selectedAnswers.Remove(highlightedAnswer))
                selectedAnswers.Add(highlightedAnswer);
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
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);

            // Make hitboxes for arrow presses
            var arrowUpHitbox = new PointerHitbox(new(interiorWidth + 1, listStartPosition), new Action<PointerEventContext>((_) => GoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var arrowDownHitbox = new PointerHitbox(new(interiorWidth + 1, listStartPosition + answersPerPage - 1), new Action<PointerEventContext>((_) => GoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowUpHitbox = new PointerHitbox(new(interiorWidth + 1 + sidebarWidth, 1), new Action<PointerEventContext>((_) => ShowcaseGoUp())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            var sidebarArrowDownHitbox = new PointerHitbox(new(interiorWidth + 1 + sidebarWidth, listStartPosition + answersPerPage - 1), new Action<PointerEventContext>((_) => ShowcaseGoDown())) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
            if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && relatedHeights.Count > answersPerPage)
            {
                arrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    arrowDownHitbox.ProcessPointer(mouse, out done);
                showcaseLine = 0;
            }
            else if ((sidebarArrowUpHitbox.IsPointerWithin(mouse) || sidebarArrowDownHitbox.IsPointerWithin(mouse)) && lines.Length > answersPerPage + listStartPosition - 1 && sidebarEnabled)
            {
                sidebarArrowUpHitbox.ProcessPointer(mouse, out bool done);
                if (!done)
                    sidebarArrowDownHitbox.ProcessPointer(mouse, out done);
            }
            else
            {
                int oldIndex = highlightedAnswer;
                if (UpdateSelectedIndexWithMousePos(mouse, out ChoiceHitboxType hitboxType, false))
                {
                    if (!multiple)
                    {
                        Update(false);
                        if (mouse.ClickTier > 1)
                            Exit(ui, false);
                        else if (settings.RadioButtons)
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
                                Update(false);
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
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;
            var highlightedAnswerChoiceInfo = allAnswers[highlightedAnswer];
            string finalSidebarText = $"[{highlightedAnswerChoiceInfo.ChoiceName}] {highlightedAnswerChoiceInfo.ChoiceTitle}\n\n{highlightedAnswerChoiceInfo.ChoiceDescription}";
            string[] lines = TextWriterTools.GetFinalLines(finalSidebarText, sidebarWidth - 3);

            // Check to see if we're scrolling the mouse wheel or not
            if (mouse.Coordinates.x < 1 || mouse.Coordinates.x >= interiorWidth + 1 ||
                mouse.Coordinates.y < listStartPosition || mouse.Coordinates.y >= listStartPosition + answersPerPage)
            {
                // It's possible that the user may be scrolling in the sidebar, but check the coordinates
                if (!sidebarEnabled)
                    return;
                if (mouse.Coordinates.x < interiorWidth + 4 || mouse.Coordinates.x >= interiorWidth + sidebarWidth + 1 ||
                    mouse.Coordinates.y < 1 || mouse.Coordinates.y >= listStartPosition + answersPerPage)
                    return;

                // Check the lines
                if (lines.Length <= answersPerPage)
                    return;

                // Now, scroll the showcase if possible
                if (goingUp)
                    ShowcaseGoUp(3);
                else
                    ShowcaseGoDown(3);
            }
            else
            {
                if (goingUp)
                    GoUp(3);
                else
                    GoDown(3);
            }
        }

        private bool UpdateSelectedIndexWithMousePos(PointerEventContext? mouse, out ChoiceHitboxType hitboxType, bool checkPos = true)
        {
            hitboxType = ChoiceHitboxType.Choice;
            if (mouse is null)
                return false;

            // Make pages based on console window height
            int wholeWidth = ConsoleWrapper.WindowWidth - 4;
            bool sidebarEnabled = sidebar && wholeWidth / 4 >= 15;
            int sidebarWidth = sidebarEnabled ? 30 : 0;
            int interiorWidth = wholeWidth - sidebarWidth + 2;
            int questionWidth = ConsoleChar.EstimateCellWidth(question);
            int sentenceLineCount = questionWidth > 0 ? ConsoleMisc.GetWrappedSentencesByWords(question, interiorWidth).Length : 0;
            int totalHeight = sentenceLineCount > 5 ? 5 : sentenceLineCount;
            int listStartPosition = questionWidth > 0 ? totalHeight + 2 : 1;
            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
            int answersPerPage = listEndPosition - 2;

            // Determine the hitbox types
            selection.Top = listStartPosition + 1;
            selection.Height = answersPerPage;
            selection.Width = interiorWidth;
            selection.ShowRadioButtons = settings.RadioButtons;
            selection.CurrentSelection = highlightedAnswer;
            selection.SelectedChoice = selectedAnswer;
            selection.CurrentSelections = multiple ? [.. selectedAnswers] : null;
            selection.Settings = settings;

            // Now, translate coordinates to the selected index and get its hitbox
            if (mouse.Coordinates.x < 1 || mouse.Coordinates.x > interiorWidth)
                return false;
            if (mouse.Coordinates.y < listStartPosition || mouse.Coordinates.y > listStartPosition + answersPerPage)
                return false;
            int listIndex = mouse.Coordinates.y - listStartPosition;
            if (!selection.CanGenerateSelectionHitbox(listIndex, out var hitbox))
                return false;

            // Depending on the hitbox parameter, we need to act accordingly
            var highlightedAnswerChoiceInfo = allAnswers[hitbox.related - 1];
            if (highlightedAnswerChoiceInfo.ChoiceDisabled && hitbox.type == ChoiceHitboxType.Choice)
                return false;
            if (!highlightedAnswerChoiceInfo.ChoiceDisabled || hitbox.type != ChoiceHitboxType.Choice)
                highlightedAnswer = hitbox.related - 1;
            if (checkPos)
                Update(false);
            hitboxType = hitbox.type;
            return true;
        }

        internal SelectionStyleTui(string question, InputChoiceCategoryInfo[] answers, InputChoiceCategoryInfo[] altAnswers, SelectionStyleSettings settings, bool kiosk, bool multiple, int[]? initialChoices, int? currentSelection, int? currentSelected)
        {
            // Check values
            initialChoices ??= [];
            selectedAnswers = [.. initialChoices];
            categories = [.. answers, .. altAnswers];
            allAnswers = SelectionInputTools.GetChoicesFromCategories(categories);
            currentSelection ??= SelectionInputTools.GetDefaultChoice(categories);
            currentSelected ??= SelectionInputTools.GetDefaultChoice(categories);
            highlightedAnswer = currentSelection ?? 0;
            selectedAnswer = currentSelected ?? 0;
            if (allAnswers.All((ici) => ici.ChoiceDisabled))
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_IS_SELECTION_EXCEPTION_NEEDSATLEASTONEITEM"));

            // Install values
            this.question = question;
            this.settings = settings ?? SelectionStyleSettings.GlobalSettings;
            this.kiosk = kiosk;
            this.multiple = multiple;

            // Before we proceed, we need to check the highlighted answer number
            if (highlightedAnswer > allAnswers.Count)
                highlightedAnswer = 0;
            if (selectedAnswer > allAnswers.Count)
                selectedAnswer = 0;

            // Set up the selection renderer instance
            selection = new Selection(categories)
            {
                Left = 1,
                ShowRadioButtons = this.settings.RadioButtons,
                CurrentSelection = highlightedAnswer,
                SelectedChoice = selectedAnswer,
                CurrentSelections = multiple ? [.. selectedAnswers] : null,
                Settings = this.settings,
            };

            // Install keybindings
            Keybindings.Add((SelectionStyleBase.Bindings[0], (ui, _, _) => Exit(ui, false)));
            Keybindings.Add((SelectionStyleBase.Bindings[1], (_, _, _) => ProcessSelect()));
            Keybindings.Add((SelectionStyleBase.Bindings[2], (ui, _, _) => Exit(ui, true)));
            Keybindings.Add((SelectionStyleBase.Bindings[3], (_, _, _) => GoUp()));
            Keybindings.Add((SelectionStyleBase.Bindings[4], (_, _, _) => GoDown()));
            Keybindings.Add((SelectionStyleBase.Bindings[5], GoFirst));
            Keybindings.Add((SelectionStyleBase.Bindings[6], GoLast));
            Keybindings.Add((SelectionStyleBase.Bindings[7], PreviousPage));
            Keybindings.Add((SelectionStyleBase.Bindings[8], NextPage));
            Keybindings.Add((SelectionStyleBase.Bindings[9], SearchPrompt));
            Keybindings.Add((SelectionStyleBase.Bindings[10], (_, _, _) => ShowcaseGoUp()));
            Keybindings.Add((SelectionStyleBase.Bindings[11], (_, _, _) => ShowcaseGoDown()));
            Keybindings.Add((SelectionStyleBase.Bindings[12], QuestionGoUp));
            Keybindings.Add((SelectionStyleBase.Bindings[13], QuestionGoDown));
            Keybindings.Add((SelectionStyleBase.Bindings[14], ShowCount));
            Keybindings.Add((SelectionStyleBase.Bindings[15], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.ShowBindings[1], ShowSidebar));
            Keybindings.Add((SelectionStyleBase.BindingsMouse[0], (_, _, mouse) => ProcessMouseWheel(mouse, true)));
            Keybindings.Add((SelectionStyleBase.BindingsMouse[1], (_, _, mouse) => ProcessMouseWheel(mouse)));
            Keybindings.Add((SelectionStyleBase.BindingsMouse[2], ProcessLeftClick));
            Keybindings.Add((SelectionStyleBase.BindingsMouse[3], ShowItemInfo));
            Keybindings.Add((SelectionStyleBase.BindingsMouse[4], (_, _, mouse) => UpdateSelectedIndexWithMousePos(mouse, out _)));

            // Install mode-dependent keybindings
            if (multiple)
            {
                Keybindings.RemoveAt(3);
                Keybindings.Add((SelectionStyleBase.BindingsMultiple[16], ModifyChoice));
                Keybindings.Add((SelectionStyleBase.BindingsMultiple[17], (_, _, _) => ProcessSelectAll(1)));
                Keybindings.Add((SelectionStyleBase.BindingsMultiple[18], (_, _, _) => ProcessSelectAll(2)));
                Keybindings.Add((SelectionStyleBase.BindingsMultiple[19], (_, _, _) => ProcessSelectAll(3)));
            }
        }
    }
}
