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
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.MiscWriters;
using Terminaux.Writer.MiscWriters.Tools;
using Textify.Tools;

namespace Terminaux.Inputs.Styles.Selection
{
    internal static class SelectionStyleBase
    {
        internal static int[] PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk, bool multiple)
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
            List<InputChoiceInfo> AllAnswers = new(Answers);
            List<int> SelectedAnswers = [];
            AllAnswers.AddRange(AltAnswers);
            bool allDisabled = AllAnswers.All((ici) => ici.ChoiceDisabled);

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
                new("Help", ConsoleKey.K),
            ] :
            [
                new("Select", ConsoleKey.Enter),
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
                new("Selects all the elements", ConsoleKey.A),
                new("Searches for an element", ConsoleKey.F),
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
            ];

            // Make a screen
            var selectionScreen = new Screen();
            bool bail = false;
            ScreenTools.SetCurrent(selectionScreen);

            // Query the enabled answers
            var enabledAnswers = AllAnswers.Select((ici, idx) => (ici, idx)).Where((ici) => !ici.ici.ChoiceDisabled).Select((tuple) => tuple.idx).ToArray();
            try
            {
                while (!bail)
                {
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
                    screenPart.Position(0, 0);
                    screenPart.AddDynamicText(() =>
                    {
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
                            BorderColor.RenderBorder(2, listStartPosition + 1, ConsoleWrapper.WindowWidth - 6, answersPerPage, optionColor) +
                            SelectionInputTools.RenderSelections([.. AllAnswers], 3, listStartPosition + 2, HighlightedAnswer - 1, multiple ? [.. SelectedAnswers] : null, answersPerPage, ConsoleWrapper.WindowWidth - 6, false, Answers.Length - 1, false, optionColor, selectedForegroundColor: selectedOptionColor, altForegroundColor: altOptionColor, altSelectedForegroundColor: selectedOptionColor, disabledForegroundColor: disabledOptionColor)
                        );

                        // Write description hint
                        int descHintAreaX = ConsoleWrapper.WindowWidth - 9;
                        int descHintAreaY = ConsoleWrapper.WindowHeight - 3;
                        var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                        bool showHint = !string.IsNullOrWhiteSpace(highlightedAnswer.ChoiceDescription);
                        if (showHint)
                        {
                            selectionBuilder.Append(
                                TextWriterWhereColor.RenderWhereColor("[TAB]", descHintAreaX, descHintAreaY, optionColor)
                            );
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
                            if (mouse.Coordinates.x == ConsoleWrapper.WindowWidth - 3)
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
                                break;
                            case PointerButton.WheelDown:
                                HighlightedAnswer++;
                                if (HighlightedAnswer > AllAnswers.Count)
                                    HighlightedAnswer = 1;
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if (DetermineArrowPressed(mouse))
                                    UpdatePositionBasedOnArrowPress(mouse);
                                else
                                {
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
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
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
                                break;
                            case ConsoleKey.DownArrow:
                                HighlightedAnswer++;
                                if (HighlightedAnswer > AllAnswers.Count)
                                    HighlightedAnswer = 1;
                                break;
                            case ConsoleKey.Home:
                                HighlightedAnswer = 1;
                                break;
                            case ConsoleKey.End:
                                HighlightedAnswer = AllAnswers.Count;
                                break;
                            case ConsoleKey.PageUp:
                                goingUp = true;
                                HighlightedAnswer = startIndex > 0 ? startIndex : 1;
                                break;
                            case ConsoleKey.PageDown:
                                HighlightedAnswer = endIndex > AllAnswers.Count - 1 ? AllAnswers.Count : endIndex + 2;
                                HighlightedAnswer = endIndex == AllAnswers.Count - 1 ? endIndex + 1 : HighlightedAnswer;
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
                                    InfoBoxModalColor.WriteInfoBoxModal($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    selectionScreen.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.A:
                                if (!multiple)
                                    break;
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
                                }
                                else
                                    InfoBoxModalColor.WriteInfoBoxModal("No item found.");
                                selectionScreen.RequireRefresh();
                                break;
                            case ConsoleKey.K:
                                // Add base bindings
                                bool isExtendable = !string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription);

                                // Keys function
                                KeybindingsWriter.ShowKeybindingInfobox(bindings);
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

        static SelectionStyleBase()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
