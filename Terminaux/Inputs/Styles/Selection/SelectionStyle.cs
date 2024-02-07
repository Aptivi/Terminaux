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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;
using Textify.General;

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
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, bool kiosk = false) =>
            PromptSelection(Question, AnswersStr, [], "", [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, AnswersStr, AnswersTitles, "", [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, bool kiosk = false) =>
            PromptSelection(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, string[] AlternateAnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(AnswersStr, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswersStr, AlternateAnswersTitles), kiosk);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], [], [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, string[] AnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, Answers, AnswersTitles, [], [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, bool kiosk = false) =>
            PromptSelection(Question, Answers, AnswersTitles, AlternateAnswers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="AlternateAnswersTitles">Working titles for each alternate answer. It must be the same amount as the alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, string[] AlternateAnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(Answers, AnswersTitles), InputChoiceTools.GetInputChoices(AlternateAnswers, AlternateAnswersTitles), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, List<InputChoiceInfo> Answers, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, List<InputChoiceInfo> Answers, List<InputChoiceInfo> AltAnswers, bool kiosk = false) =>
            PromptSelection(Question, Answers.ToArray(), [.. AltAnswers], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, bool kiosk = false) =>
            PromptSelection(Question, Answers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, bool kiosk = false)
        {
            // Variables
            Color questionColor = new(ConsoleColors.Yellow);
            Color progressColor = new(ConsoleColors.Yellow);
            Color inputColor = new(ConsoleColors.White);
            Color optionColor = new(ConsoleColors.DarkYellow);
            Color altOptionColor = new(ConsoleColors.Yellow);
            Color selectedOptionColor = new(ConsoleColors.Cyan);
            Color separatorColor = new(ConsoleColors.Gray);
            Color textColor = new(ConsoleColors.Gray);
            int HighlightedAnswer = 1;
            List<InputChoiceInfo> AllAnswers = new(Answers);
            AllAnswers.AddRange(AltAnswers);
            int AnswerTitleLeft = AllAnswers.Max(x => $"  {x.ChoiceName}) ".Length);

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
                    // Make a screen part
                    var screenPart = new ScreenPart();
                    int startIndex = 0;
                    int endIndex = 0;
                    var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                    screenPart.Position(0, 0);
                    screenPart.AddDynamicText(() =>
                    {
                        // Make pages based on console window height
                        int listStartPosition = TextTools.GetWrappedSentences(Question, ConsoleWrapper.WindowWidth).Length;
                        int listEndPosition = ConsoleWrapper.WindowHeight - listStartPosition;
                        int answersPerPage = listEndPosition - 5;
                        int pages = AllAnswers.Count / answersPerPage;
                        if (AllAnswers.Count % answersPerPage == 0)
                            pages--;
                        var selectionBuilder = new StringBuilder(
                            $"{ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)}" +
                            ConsoleClearing.GetClearWholeScreenSequence()
                        );

                        // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                        // causing confusion. Pages, again, are one-based.
                        int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                        startIndex = answersPerPage * currentPage;
                        endIndex = answersPerPage * (currentPage + 1) - 1;

                        // Write the question.
                        selectionBuilder.Append(
                            $"{questionColor.VTSequenceForeground}" +
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

                                // Get the option
                                string AnswerOption = $"{(selected ? ">" : " ")} {AnswerInstance}) {AnswerTitle}";
                                int answerTitleMaxLeft = ConsoleWrapper.WindowWidth;
                                if (AnswerTitleLeft < answerTitleMaxLeft)
                                {
                                    string renderedChoice = $"{(selected ? ">" : " ")} {AnswerInstance.ChoiceName}) ";
                                    int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                                    AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}" + $"{clear}";
                                }
                                var AnswerColor =
                                    selected ?
                                    selectedOptionColor :
                                    AltAnswer ? altOptionColor : optionColor;
                                AnswerOption = $"{AnswerColor.VTSequenceForeground}{AnswerOption}";
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
                            $"{ColorTools.GetGray().VTSequenceForeground}" +
                            $"{new string('=', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 2)}" +
                            $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                            $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                            $"{new Color(ConsoleColors.White).VTSequenceForeground}" +
                            descFinal
                        );

                        // Render keybindings and page and answer number
                        bool isExtendable = !string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription);
                        string bindingsRender = $"[ENTER: select]";
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
                            $"{ColorTools.GetGray().VTSequenceForeground}" +
                            bindingsRender +
                            $"{CsiSequences.GenerateCsiCursorPosition(numbersLeft + 1, descSepArea + 1)}" +
                            $"{ColorTools.GetGray().VTSequenceForeground}" +
                            numberRender
                        );

                        // Render the vertical slider.
                        selectionBuilder.Append(
                            SliderVerticalColor.RenderVerticalSlider(HighlightedAnswer, AllAnswers.Count, ConsoleWrapper.WindowWidth - 2, listStartPosition, listStartPosition + 1, 4, progressColor, progressColor, false)
                        );
                        return selectionBuilder.ToString();
                    });
                    selectionScreen.AddBufferedPart("Selection - single", screenPart);

                    // Wait for an answer
                    ScreenTools.Render();
                    Answer = Input.DetectKeypress();

                    // Check the answer
                    switch (Answer.Key)
                    {
                        case ConsoleKey.UpArrow:
                            HighlightedAnswer -= 1;
                            if (HighlightedAnswer == 0)
                                HighlightedAnswer = AllAnswers.Count;
                            break;
                        case ConsoleKey.DownArrow:
                            HighlightedAnswer += 1;
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
                                InfoBoxColor.WriteInfoBox($"[{choiceName}] {choiceTitle}", choiceDesc);
                            break;
                    }

                    // Reset, in case selection changed
                    selectionScreen.RemoveBufferedParts();
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

    }
}
