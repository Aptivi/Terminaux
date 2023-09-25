
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
using Terminaux.Sequences.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Reader.Inputs.Styles
{
    /// <summary>
    /// Selection style for input module
    /// </summary>
    public static class SelectionStyle
    {

        private static int savedPos = 1;

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, bool kiosk = false) => 
            PromptSelection(Question, AnswersStr, Array.Empty<string>(), "", Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, AnswersStr, AnswersTitles, "", Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswersStr">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string AnswersStr, string[] AnswersTitles, string AlternateAnswersStr, bool kiosk = false) =>
            PromptSelection(Question, AnswersStr, AnswersTitles, AlternateAnswersStr, Array.Empty<string>(), kiosk);

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
            PromptSelection(Question, Answers, Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, string[] AnswersTitles, bool kiosk = false) =>
            PromptSelection(Question, Answers, AnswersTitles, Array.Empty<string>(), Array.Empty<string>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers. They can be written like this: Y/N/C.</param>
        /// <param name="AnswersTitles">Working titles for each answer. It must be the same amount as the answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers. They can be written like this: Y/N/C.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, string[] Answers, string[] AnswersTitles, string[] AlternateAnswers, bool kiosk = false) =>
            PromptSelection(Question, Answers, AnswersTitles, AlternateAnswers, Array.Empty<string>(), kiosk);

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
            PromptSelection(Question, Answers, new List<InputChoiceInfo>(), kiosk);

        /// <summary>
        /// Prompts user for Selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        public static int PromptSelection(string Question, List<InputChoiceInfo> Answers, List<InputChoiceInfo> AltAnswers, bool kiosk = false)
        {
            // Variables
            Color questionColor = new(ConsoleColors.Yellow);
            Color inputColor = new(ConsoleColors.White);
            Color optionColor = new(ConsoleColors.DarkYellow);
            Color altOptionColor = new(ConsoleColors.Yellow);
            Color selectedOptionColor = new(ConsoleColors.Cyan);
            Color separatorColor = new(ConsoleColors.Gray);
            Color textColor = new(ConsoleColors.Gray);
            int HighlightedAnswer = savedPos;
            List<InputChoiceInfo> AllAnswers = new(Answers);
            AllAnswers.AddRange(AltAnswers);

            // Before we proceed, we need to check the highlighted answer number
            if (HighlightedAnswer > AllAnswers.Count)
                HighlightedAnswer = 1;

            // First alt answer index
            int altAnswersFirstIdx = Answers.Count;
            ConsoleKeyInfo Answer;
            ConsoleWrappers.ActionCursorVisible(false);
            ConsoleWrappers.ActionClear();

            // Ask a question
            TextWriterColor.Write(Question, true, questionColor);

            // Make pages based on console window height
            int listStartPosition = ConsoleWrappers.ActionCursorTop();
            int listEndPosition = ConsoleWrappers.ActionWindowHeight() - ConsoleWrappers.ActionCursorTop();
            int answersPerPage = listEndPosition - 5;
            int pages = AllAnswers.Count / answersPerPage;
            if (AllAnswers.Count % answersPerPage == 0)
                pages--;
            int lastPage = 1;
            bool refreshRequired = false;

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
                    ConsoleWrappers.ActionClear();
                    TextWriterColor.Write(Question, true, questionColor);
                }

                // Populate the answers
                int renderedAnswers = 1;
                for (int AnswerIndex = startIndex; AnswerIndex <= endIndex; AnswerIndex++)
                {
                    ConsoleWrappers.ActionSetCursorPosition(0, listStartPosition + renderedAnswers);
                    bool AltAnswer = AnswerIndex >= altAnswersFirstIdx;

                    // Check to see if we're out of bounds
                    if (AnswerIndex >= AllAnswers.Count)
                    {
                        // Write an empty entry that clears the line
                        TextWriterColor.Write($"{Color255.GetEsc()}[0K");
                    }
                    else
                    {
                        // Populate the answer
                        var AnswerInstance = AllAnswers[AnswerIndex];
                        string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";

                        // Get the option
                        string AnswerOption = $"  {AnswerInstance}) {AnswerTitle}";
                        int AnswerTitleLeft = AllAnswers.Max(x => $"  {x.ChoiceName}) ".Length);
                        int answerTitleMaxLeft = ConsoleWrappers.ActionWindowWidth();
                        if (AnswerTitleLeft < answerTitleMaxLeft)
                        {
                            string renderedChoice = $"  {AnswerInstance.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - renderedChoice.Length;
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}" + $"{Color255.GetEsc()}[0K";
                        }
                        var AnswerColor = AnswerIndex + 1 == HighlightedAnswer ?
                                          selectedOptionColor :
                                          AltAnswer ? altOptionColor : optionColor;
                        AnswerOption = $"{AnswerColor.VTSequenceForeground}{AnswerOption}";
                        TextWriterColor.Write(AnswerOption.Truncate(answerTitleMaxLeft - 3 + VtSequenceTools.MatchVTSequences(AnswerOption)
                        .Sum((mc) =>
                        {
                            int sum = 0;
                            foreach (Match item in mc)
                                sum += item.Length;
                            return sum;
                        })));
                    }
                    renderedAnswers++;
                }

                // If we need to write the vertical progress bar, do so. But, we need to refresh in case we're told to redraw on demand when
                // we're not switching pages yet.
                ProgressBarVerticalColor.WriteVerticalProgress(100 * ((double)HighlightedAnswer / AllAnswers.Count), ConsoleWrappers.ActionWindowWidth() - 2, listStartPosition, listStartPosition + 1, 4, false);

                // Write description area
                int descSepArea = ConsoleWrappers.ActionWindowHeight() - 3;
                int descArea = ConsoleWrappers.ActionWindowHeight() - 2;
                var highlightedAnswer = AllAnswers[HighlightedAnswer - 1];
                string descFinal = highlightedAnswer.ChoiceDescription is not null ? highlightedAnswer.ChoiceDescription.Truncate((ConsoleWrappers.ActionWindowWidth() * 2) - 3) : "";
                TextWriterWhereColor.WriteWhere(new string('=', ConsoleWrappers.ActionWindowWidth()), 0, descSepArea, separatorColor);
                TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrappers.ActionWindowWidth()), 0, descArea);
                TextWriterWhereColor.WriteWhere(new string(' ', ConsoleWrappers.ActionWindowWidth()), 0, descArea + 1);
                TextWriterWhereColor.WriteWhere(descFinal, 0, descArea, textColor);

                // Write keybindings and page and answer number
                string bindingsRender =
                    kiosk ?
                    $"[ENTER: select]==[TAB: info]" :
                    $"[ENTER: select]==[ESC: exit]==[TAB: info]";
                string numberRender = $"[{currentPage + 1}/{pages + 1}]==[{HighlightedAnswer}/{AllAnswers.Count}]";
                int bindingsLeft = 2;
                int numbersLeft = ConsoleWrappers.ActionWindowWidth() - numberRender.Length - bindingsLeft;
                TextWriterWhereColor.WriteWhere(bindingsRender, bindingsLeft, descSepArea, separatorColor);
                TextWriterWhereColor.WriteWhere(numberRender, numbersLeft, descSepArea, separatorColor);

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
                    case ConsoleKey.Enter:
                        TextWriterColor.Write();
                        savedPos = HighlightedAnswer;
                        return HighlightedAnswer;
                    case ConsoleKey.Escape:
                        if (kiosk)
                            break;
                        TextWriterColor.Write();
                        savedPos = HighlightedAnswer;
                        return -1;
                    case ConsoleKey.Tab:
                        if (string.IsNullOrEmpty(highlightedAnswer.ChoiceDescription))
                            break;
                        var infoRenderer = new StringBuilder();
                        infoRenderer.AppendFormat("{0}",
                            string.Join("\n", new[]
                            {
                                highlightedAnswer.ChoiceTitle,
                                new string('-', highlightedAnswer.ChoiceTitle.Length),
                                "",
                                highlightedAnswer.ChoiceDescription,
                            })
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
