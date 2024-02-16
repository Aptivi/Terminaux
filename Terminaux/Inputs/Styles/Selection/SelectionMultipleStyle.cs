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
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;
using Textify.General;

namespace Terminaux.Inputs.Styles.Selection
{
    /// <summary>
    /// Multiple selection style for input module
    /// </summary>
    public static class SelectionMultipleStyle
    {
        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, [], "", [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, "", [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(AnswersStr, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswersStr, AlternateAnswersTitles), kiosk);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, [], [], [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, [], [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, AlternateAnswers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, string[] AlternateAnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(Answers, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswers, AlternateAnswersTitles), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, [], "", [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, "", [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(AnswersStr, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswersStr, AlternateAnswersTitles), settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, [], [], [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, [], [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, AlternateAnswers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, string[] AlternateAnswersTitles, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(Answers, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswers, AlternateAnswersTitles), settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false)
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
            int AnswerTitleLeft = AllAnswers.Max(x => $"  {x.ChoiceName}) ".Length);

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
            ConsoleKeyInfo Answer;
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
                        int renderedAnswers = 1;
                        for (int AnswerIndex = startIndex; AnswerIndex <= endIndex; AnswerIndex++)
                        {
                            selectionBuilder.Append(CsiSequences.GenerateCsiCursorPosition(1, listStartPosition + renderedAnswers + 1));
                            bool AltAnswer = AnswerIndex >= altAnswersFirstIdx;

                            // Check to see if we're out of bounds
                            var clear = ConsoleClearing.GetClearLineToRightSequence();
                            if (AnswerIndex >= AllAnswers.Count)
                            {
                                // Write an empty entry that clears the line
                                selectionBuilder.Append(clear);
                            }
                            else
                            {
                                // Populate the answer
                                bool selected = AnswerIndex + 1 == HighlightedAnswer;
                                var AnswerInstance = AllAnswers[AnswerIndex];
                                string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";
                                string answerIndicator = $"[{(SelectedAnswers.Contains(AnswerIndex + 1) ? "*" : " ")}]";
                                bool disabled = AnswerInstance.ChoiceDisabled;

                                // Get the option
                                string AnswerOption = $"{(selected ? ">" : disabled ? "X" : " ")} {AnswerInstance}) {answerIndicator} {AnswerTitle}";
                                int answerTitleMaxLeft = ConsoleWrapper.WindowWidth;
                                if (AnswerTitleLeft < answerTitleMaxLeft)
                                {
                                    string renderedChoice = $"{(selected ? ">" : disabled ? "X" : " ")} {AnswerInstance.ChoiceName}) ";
                                    int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                                    AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{answerIndicator} {AnswerTitle}" + $"{ConsoleClearing.GetClearLineToRightSequence()}";
                                }
                                var AnswerColor =
                                    disabled ? disabledOptionColor :
                                    selected ? selectedOptionColor :
                                    AltAnswer ? altOptionColor :
                                    optionColor;
                                AnswerOption = $"{ColorTools.RenderSetConsoleColor(AnswerColor)}{AnswerOption}";
                                selectionBuilder.Append(AnswerOption.Truncate(answerTitleMaxLeft - 8 + VtSequenceTools.MatchVTSequences(AnswerOption).Sum((mc) =>
                                {
                                    int sum = 0;
                                    foreach (Match item in mc)
                                        sum += item.Length;
                                    return sum;
                                })));
                            }
                            renderedAnswers++;
                        }

                        // Write description area
                        int descSepArea = ConsoleWrapper.WindowHeight - 3;
                        int descArea = ConsoleWrapper.WindowHeight - 2;
                        var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                        string descFinal = highlightedAnswer.ChoiceDescription is not null ? highlightedAnswer.ChoiceDescription.Truncate(ConsoleWrapper.WindowWidth * 2 - 3) : "";
                        selectionBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descSepArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                            $"{new string('=', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 2)}" +
                            $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(new Color(ConsoleColors.White))}" +
                            descFinal
                        );

                        // Write keybindings and page and answer number
                        bool isExtendable = !string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription);
                        string bindingsRender = "[SPACE: (un)check]==[ENTER: select]";
                        string numberRender = $"[{currentPage + 1}/{pages + 1}]==[{HighlightedAnswer}/{AllAnswers.Count}]";

                        // Add info binding if extendable
                        if (isExtendable)
                            bindingsRender += $"{(isExtendable ? "==[TAB: info]" : "")}";
                        if (!kiosk)
                            bindingsRender += $"{(!kiosk ? "==[ESC: exit]" : "")}";

                        // Now, render the bindings and the page numbers
                        int bindingsLeft = 2;
                        int numbersLeft = ConsoleWrapper.WindowWidth - numberRender.Length - bindingsLeft;
                        selectionBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(bindingsLeft + 1, descSepArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                            bindingsRender +
                            $"{CsiSequences.GenerateCsiCursorPosition(numbersLeft + 1, descSepArea + 1)}" +
                            $"{ColorTools.RenderSetConsoleColor(ColorTools.GetGray())}" +
                            numberRender
                        );

                        // Render the vertical slider.
                        selectionBuilder.Append(
                            SliderVerticalColor.RenderVerticalSlider(HighlightedAnswer, AllAnswers.Count, ConsoleWrapper.WindowWidth - 2, listStartPosition, listStartPosition + 1, 4, sliderColor, sliderColor, false)
                        );
                        return selectionBuilder.ToString();
                    });
                    selectionScreen.AddBufferedPart("Selection - multiple", screenPart);

                    // Wait for an answer
                    ScreenTools.Render();
                    Answer = TermReader.ReadKey();

                    // Check the answer
                    bool goingUp = false;
                    switch (Answer.Key)
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
                            if (!SelectedAnswers.Remove(HighlightedAnswer))
                                SelectedAnswers.Add(HighlightedAnswer);
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
                            SelectedAnswers.Clear();
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
                    }

                    // Verify that the current position is not a disabled choice
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

                    // Reset, in case selection changed
                    selectionScreen.RemoveBufferedPart("Selection - multiple");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to initialize the selection input:" + $" {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                InfoBoxColor.WriteInfoBox("Failed to initialize the selection input:" + $" {ex.Message}");
            }
            ScreenTools.UnsetCurrent(selectionScreen);
            SelectedAnswers.Sort();
            return [.. SelectedAnswers];
        }

        static SelectionMultipleStyle()
        {
            ConsoleChecker.CheckConsole();
        }
    }
}
