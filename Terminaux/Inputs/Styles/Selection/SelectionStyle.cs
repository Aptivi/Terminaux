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
using Terminaux.Colors.Data;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Inputs.Styles.Selection
{
    /// <summary>
    /// Selection style for input module
    /// </summary>
    public static class SelectionStyle
    {
        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, (string, string)[] Answers, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, bool kiosk = false) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, bool kiosk = false) =>
            PromptSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, (string, string)[] Answers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false)
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
            AllAnswers.AddRange(AltAnswers);
            bool allDisabled = AllAnswers.All((ici) => ici.ChoiceDisabled);
            int AnswerTitleLeft = AllAnswers.Max(x => $"  {x.ChoiceName}) ".Length);

            // We need to not to run the selection style when everything is disabled
            if (allDisabled)
                throw new TerminauxException("The selection style requires that there is at least one choice enabled.");

            // Before we proceed, we need to check the highlighted answer number
            if (HighlightedAnswer > AllAnswers.Count)
                HighlightedAnswer = 1;
            HighlightedAnswer = AllAnswers.Any((ici) => ici.ChoiceDefault) ? AllAnswers.Select((ici, idx) => (idx, ici.ChoiceDefault)).Where((tuple) => tuple.ChoiceDefault).First().idx + 1 : 1;

            // First alt answer index
            int altAnswersFirstIdx = Answers.Length;
            bool initialVisible = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = false;

            // Make a screen
            var selectionScreen = new Screen();
            bool bail = false;
            ScreenTools.SetCurrent(selectionScreen);
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
                            SelectionInputTools.RenderSelections([.. AllAnswers], 0, listStartPosition + 1, HighlightedAnswer - 1, answersPerPage, ConsoleWrapper.WindowWidth, false, Answers.Length - 1, false, optionColor, selectedForegroundColor: selectedOptionColor, altForegroundColor: altOptionColor, altSelectedForegroundColor: selectedOptionColor, disabledForegroundColor: disabledOptionColor)
                        );

                        // Write description area
                        int descSepArea = ConsoleWrapper.WindowHeight - 3;
                        int descArea = ConsoleWrapper.WindowHeight - 2;
                        var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                        string descFinal = highlightedAnswer.ChoiceDescription is not null ? highlightedAnswer.ChoiceDescription : "";
                        if (highlightedAnswer.ChoiceDescription is not null)
                        {
                            var wrappedDescLines = ConsoleMisc.GetWrappedSentencesByWords(descFinal, ConsoleWrapper.WindowWidth).Take(2).ToArray();
                            wrappedDescLines[wrappedDescLines.Length - 1] = wrappedDescLines[wrappedDescLines.Length - 1].Truncate(ConsoleWrapper.WindowWidth - 3);
                            descFinal = string.Join("\n", wrappedDescLines);
                        }
                        selectionBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{ConsoleClearing.GetClearLineToRightSequence()}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 2)}" +
                            $"{ConsoleClearing.GetClearLineToRightSequence()}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.White))}" +
                            descFinal
                        );

                        // Render keybindings and page and answer number
                        bool isExtendable = !string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription);
                        string bindingsRender = $"[ENTER: select]";
                        string numberRender = $"[{currentPage + 1}/{pages + 1}]══[{HighlightedAnswer}/{AllAnswers.Count}]";

                        // Add info binding if extendable
                        if (isExtendable)
                            bindingsRender += $"{(isExtendable ? "══[TAB: info]" : "")}";
                        if (!kiosk)
                            bindingsRender += $"{(!kiosk ? "══[ESC: exit]" : "")}";

                        // Now, render the bindings and the page numbers
                        int bindingsLeft = 2;
                        int numbersLeft = ConsoleWrapper.WindowWidth - numberRender.Length - bindingsLeft;
                        selectionBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descSepArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                            $"{new string('═', ConsoleWrapper.WindowWidth)}"
                        );
                        if (bindingsRender.Length + numberRender.Length + 6 < ConsoleWrapper.WindowWidth)
                        {
                            selectionBuilder.Append(
                                $"{CsiSequences.GenerateCsiCursorPosition(bindingsLeft + 1, descSepArea + 1)}" +
                                $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                                bindingsRender +
                                $"{CsiSequences.GenerateCsiCursorPosition(numbersLeft + 1, descSepArea + 1)}" +
                                $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                                numberRender
                            );
                        }
                        return selectionBuilder.ToString();
                    });
                    selectionScreen.AddBufferedPart("Selection - single", screenPart);

                    // Wait for an answer
                    ScreenTools.Render();
                    SpinWait.SpinUntil(() => PointerListener.InputAvailable);
                    bool goingUp = false;
                    if (PointerListener.PointerAvailable)
                    {
                        bool UpdateSelectedIndexWithMousePos(PointerEventContext mouse)
                        {
                            if (mouse.Coordinates.x >= ConsoleWrapper.WindowWidth - 1)
                                return false;

                            // Make pages based on console window height
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                            startIndex = answersPerPage * currentPage;
                            endIndex = answersPerPage * (currentPage + 1) - 1;

                            // Now, translate coordinates to the selected index
                            if (mouse.Coordinates.y <= listStartPosition || mouse.Coordinates.y >= listEndPosition - 3)
                                return false;
                            int listIndex = mouse.Coordinates.y - listStartPosition;
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
                                mouse.Coordinates.x == ConsoleWrapper.WindowWidth - 1 &&
                                (mouse.Coordinates.y == listStartPosition + 1 || mouse.Coordinates.y == listStartPosition + answersPerPage);
                        }

                        void UpdatePositionBasedOnArrowPress(PointerEventContext mouse)
                        {
                            int listStartPosition = ConsoleMisc.GetWrappedSentencesByWords(Question, ConsoleWrapper.WindowWidth).Length;
                            int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                            int answersPerPage = listEndPosition - 5;
                            if (AllAnswers.Count <= answersPerPage)
                                return;
                            if (mouse.Coordinates.x == ConsoleWrapper.WindowWidth - 1)
                            {
                                if (mouse.Coordinates.y == listStartPosition + 1)
                                {
                                    goingUp = true;
                                    HighlightedAnswer--;
                                    if (HighlightedAnswer == 0)
                                        HighlightedAnswer = 1;
                                }
                                else if (mouse.Coordinates.y == listStartPosition + answersPerPage)
                                {
                                    HighlightedAnswer++;
                                    if (HighlightedAnswer > AllAnswers.Count)
                                        HighlightedAnswer = AllAnswers.Count;
                                }
                            }
                        }

                        // Mouse input received.
                        var mouse = TermReader.ReadPointer();
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
                                        ConsoleWrapper.CursorVisible = initialVisible;
                                        ColorTools.LoadBack();
                                        bail = true;
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
                                    InfoBoxColor.WriteInfoBox($"[{choiceName}] {choiceTitle}", choiceDesc);
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
                    else if (ConsoleWrapper.KeyAvailable && !PointerListener.PointerActive)
                    {
                        var key = TermReader.ReadKey();
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
                            case ConsoleKey.Enter:
                                ConsoleWrapper.CursorVisible = initialVisible;
                                ColorTools.LoadBack();
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                if (kiosk)
                                    break;
                                ConsoleWrapper.CursorVisible = initialVisible;
                                ColorTools.LoadBack();
                                bail = true;
                                HighlightedAnswer = -1;
                                break;
                            case ConsoleKey.Tab:
                                string choiceName = highlightedAnswer.ChoiceName;
                                string choiceTitle = highlightedAnswer.ChoiceTitle;
                                string choiceDesc = highlightedAnswer.ChoiceDescription;
                                if (!string.IsNullOrWhiteSpace(choiceDesc))
                                {
                                    InfoBoxColor.WriteInfoBox($"[{choiceName}] {choiceTitle}", choiceDesc);
                                    selectionScreen.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.F:
                                // Search function
                                if (AllAnswers.Count <= 0)
                                    break;
                                var entriesString = AllAnswers.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle)).ToArray();
                                string keyword = InfoBoxInputColor.WriteInfoBoxInput("Write a search term (case insensitive)").ToLower();
                                var resultEntries = entriesString.Select((entry, idx) => ($"{idx + 1}", entry)).Where((tuple) => tuple.entry.ChoiceName.ToLower().Contains(keyword) || tuple.entry.ChoiceTitle.ToLower().Contains(keyword)).Select((tuple) => (tuple.Item1, $"{tuple.entry.ChoiceName}) {tuple.entry.ChoiceTitle}")).ToArray();
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
                                    InfoBoxColor.WriteInfoBox("No item found.");
                                selectionScreen.RequireRefresh();
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                InfoBoxColor.WriteInfoBox("Available keybindings",
                                    $$"""
                                    [UP]        | Goes one element up
                                    [DOWN]      | Goes one element down
                                    [HOME]      | Goes to the first element
                                    [END]       | Goes to the last element
                                    [PAGE UP]   | Goes to the previous page
                                    [PAGE DOWN] | Goes to the next page
                                    [ENTER]     | Select a choice
                                    [ESC]       | Exits the selection
                                    [TAB]       | Shows more info in an infobox
                                    [F]         | Searches for an element
                                    """
                                );
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to initialize the selection input:" + $" {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                InfoBoxColor.WriteInfoBox("Failed to initialize the selection input:" + $" {ex.Message}");
            }
            ScreenTools.UnsetCurrent(selectionScreen);
            return HighlightedAnswer;
        }

        static SelectionStyle()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
