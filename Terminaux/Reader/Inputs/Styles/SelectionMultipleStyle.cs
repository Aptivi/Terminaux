
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.Sequences.Builder.Types;
using Textify.Sequences.Tools;

namespace Terminaux.Reader.Inputs.Styles
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
            PromptMultipleSelection(Question, AnswersStr, Array.Empty<string>(), "", Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, "", Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, bool kiosk = false) =>
            PromptMultipleSelection(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, Array.Empty<string>(), kiosk);

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
            PromptMultipleSelection(Question, Answers, Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, Array.Empty<string>(), Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, AnswersTitles, AlternateAnswers, Array.Empty<string>(), kiosk);

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
        public static int[] PromptMultipleSelection(string Question, List<InputChoiceInfo> Answers, bool kiosk = false) =>
            PromptMultipleSelection(Question, Answers, [], kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int[] PromptMultipleSelection(string Question, List<InputChoiceInfo> Answers, List<InputChoiceInfo> AltAnswers, bool kiosk = false)
        {
            // Variables
            Color questionColor = new(ConsoleColors.Yellow);
            Color inputColor = new(ConsoleColors.White);
            Color optionColor = new(ConsoleColors.DarkYellow);
            Color altOptionColor = new(ConsoleColors.Yellow);
            Color selectedOptionColor = new(ConsoleColors.Cyan);
            Color separatorColor = new(ConsoleColors.Gray);
            Color textColor = new(ConsoleColors.Gray);
            int HighlightedAnswer = 1;
            List<InputChoiceInfo> AllAnswers = new(Answers);
            List<int> SelectedAnswers = [];
            AllAnswers.AddRange(AltAnswers);

            // Before we proceed, we need to check the highlighted answer number
            if (HighlightedAnswer > AllAnswers.Count)
                HighlightedAnswer = 1;

            // First alt answer index
            int altAnswersFirstIdx = Answers.Count;
            ConsoleKeyInfo Answer;
            bool origVisible = ConsoleWrapper.CursorVisible;
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Make pages based on console window height
            int listStartPosition = ConsoleExtensions.GetWrappedSentences(Question, ConsoleWrapper.WindowWidth).Length;
            int listEndPosition = ConsoleWrapper.WindowHeight - ConsoleWrapper.CursorTop;
            int answersPerPage = listEndPosition - 5;
            int pages = AllAnswers.Count / answersPerPage;
            if (AllAnswers.Count % answersPerPage == 0)
                pages--;
            int lastPage = 1;
            bool refreshRequired = false;
            var selectionBuilder = new StringBuilder();

            // Ask a question
            selectionBuilder.Append(
                $"{questionColor.VTSequenceForeground}" +
                $"{Question}"
            );

            while (true)
            {
                // The reason for subtracting the highlighted answer by one is that because while the highlighted answer number is one-based, the indexes are zero-based,
                // causing confusion. Pages, again, are one-based.
                int currentPage = (HighlightedAnswer - 1) / answersPerPage;
                int startIndex = answersPerPage * currentPage;
                int endIndex = (answersPerPage * (currentPage + 1)) - 1;

                // If the refresh is required, refresh the entire screen.
                if (refreshRequired)
                {
                    refreshRequired = false;
                    ConsoleWrapper.Clear();
                    selectionBuilder.Append(
                        $"{questionColor.VTSequenceForeground}" +
                        $"{Question}"
                    );
                }

                // Populate the answers
                int renderedAnswers = 1;
                for (int AnswerIndex = startIndex; AnswerIndex <= endIndex; AnswerIndex++)
                {
                    selectionBuilder.Append(CsiSequences.GenerateCsiCursorPosition(1, listStartPosition + renderedAnswers + 1));
                    bool AltAnswer = AnswerIndex >= altAnswersFirstIdx;

                    // Check to see if we're out of bounds
                    var clear = ConsoleExtensions.GetClearLineToRightSequence();
                    if (AnswerIndex >= AllAnswers.Count)
                    {
                        // Write an empty entry that clears the line
                        selectionBuilder.Append(clear);
                    }
                    else
                    {
                        // Populate the answer
                        var AnswerInstance = AllAnswers[AnswerIndex];
                        string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";
                        string answerIndicator = $"[{(SelectedAnswers.Contains(AnswerIndex + 1) ? "*" : " ")}]";

                        // Get the option
                        string AnswerOption = $"  {AnswerInstance}) {answerIndicator} {AnswerTitle}";
                        int AnswerTitleLeft = AllAnswers.Max(x => $"  {x.ChoiceName}) ".Length);
                        int answerTitleMaxLeft = ConsoleWrapper.WindowWidth;
                        if (AnswerTitleLeft < answerTitleMaxLeft)
                        {
                            string renderedChoice = $"  {AnswerInstance.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{answerIndicator} {AnswerTitle}" + $"\u001b[0K";
                        }
                        var AnswerColor = AnswerIndex + 1 == HighlightedAnswer ?
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
                string descFinal = highlightedAnswer.ChoiceDescription is not null ? highlightedAnswer.ChoiceDescription.Truncate((ConsoleWrapper.WindowWidth * 2) - 3) : "";
                selectionBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(1, descSepArea + 1)}" +
                    $"{separatorColor.VTSequenceForeground}" +
                    $"{new string('=', ConsoleWrapper.WindowWidth)}" +
                    $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                    $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                    $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 2)}" +
                    $"{new string(' ', ConsoleWrapper.WindowWidth)}" +
                    $"{CsiSequences.GenerateCsiCursorPosition(1, descArea + 1)}" +
                    $"{textColor.VTSequenceForeground}" +
                    descFinal
                );

                // Write keybindings and page and answer number
                string bindingsRender =
                    kiosk ?
                    $"[SPACE: (un)check]==[ENTER: submit]==[TAB: info]" :
                    $"[SPACE: (un)check]==[ENTER: submit]==[ESC: exit]==[TAB: info]";
                string numberRender = $"[{currentPage + 1}/{pages + 1}]==[{HighlightedAnswer}/{AllAnswers.Count}]";
                int bindingsLeft = 2;
                int numbersLeft = ConsoleWrapper.WindowWidth - numberRender.Length - bindingsLeft;
                selectionBuilder.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(bindingsLeft + 1, descSepArea + 1)}" +
                    $"{separatorColor.VTSequenceForeground}" +
                    bindingsRender +
                    $"{CsiSequences.GenerateCsiCursorPosition(numbersLeft + 1, descSepArea + 1)}" +
                    $"{separatorColor.VTSequenceForeground}" +
                    numberRender
                );
                TextWriterColor.WritePlain(selectionBuilder.ToString());
                selectionBuilder.Clear();

                // If we need to write the vertical progress bar, do so. But, we need to refresh in case we're told to redraw on demand when
                // we're not switching pages yet.
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)HighlightedAnswer / AllAnswers.Count), ConsoleWrapper.WindowWidth - 2, listStartPosition, listStartPosition + 1, 4, false);

                // Wait for an answer
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
                    case ConsoleKey.Spacebar:
                        if (SelectedAnswers.Contains(HighlightedAnswer))
                            SelectedAnswers.Remove(HighlightedAnswer);
                        else
                            SelectedAnswers.Add(HighlightedAnswer);
                        break;
                    case ConsoleKey.Enter:
                        ConsoleWrapper.CursorVisible = origVisible;
                        ConsoleWrapper.Clear();
                        return [.. SelectedAnswers];
                    case ConsoleKey.Escape:
                        if (kiosk)
                            break;
                        ConsoleWrapper.CursorVisible = origVisible;
                        ConsoleWrapper.Clear();
                        return Array.Empty<int>();
                    case ConsoleKey.Tab:
                        if (string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription))
                            break;
                        var infoRenderer = new StringBuilder();
                        infoRenderer.AppendFormat("{0}",
                            string.Join("\n",
                            [
                                highlightedAnswer.ChoiceTitle,
                                new string('-', highlightedAnswer.ChoiceTitle.Length),
                                "",
                                highlightedAnswer.ChoiceDescription,
                            ])
                        );
                        InfoBoxColor.WriteInfoBox(infoRenderer.ToString());
                        refreshRequired = true;
                        break;
                }

                // Update the last page
                lastPage = currentPage;
            }
        }
    }
}
