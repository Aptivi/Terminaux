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
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Textify.General;

namespace Terminaux.Inputs.Styles.Choice
{
    /// <summary>
    /// Choice style for input module
    /// </summary>
    public static class ChoiceStyle
    {
        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers and working titles for each answer in one tuple.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers and working titles for each answer in one tuple.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AltAnswers, new(ConsoleColors.Yellow), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, Color questionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, Color questionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), questionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, Color questionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, Color questionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AltAnswers, questionColor, new(ConsoleColors.White), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, Color questionColor, Color inputColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, Color questionColor, Color inputColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), questionColor, inputColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, Color questionColor, Color inputColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, Color questionColor, Color inputColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AltAnswers, questionColor, inputColor, new(ConsoleColors.Olive), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, Color questionColor, Color inputColor, Color optionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, Color questionColor, Color inputColor, Color optionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), questionColor, inputColor, optionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, Color questionColor, Color inputColor, Color optionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, Color questionColor, Color inputColor, Color optionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AltAnswers, questionColor, inputColor, optionColor, new(ConsoleColors.Yellow), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, altOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), questionColor, inputColor, optionColor, altOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, altOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, AltAnswers, questionColor, inputColor, optionColor, altOptionColor, new(ConsoleColors.Grey), OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="disabledOptionColor">The disabled option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, Color disabledOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, altOptionColor, disabledOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="disabledOptionColor">The disabled option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, Color disabledOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), questionColor, inputColor, optionColor, altOptionColor, disabledOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="disabledOptionColor">The disabled option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, Color disabledOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false) =>
            PromptChoice(Question, Answers, [], questionColor, inputColor, optionColor, altOptionColor, disabledOptionColor, OutputType, PressEnter);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="questionColor">The question color</param>
        /// <param name="inputColor">The input color</param>
        /// <param name="optionColor">The option color</param>
        /// <param name="altOptionColor">The alt option color</param>
        /// <param name="disabledOptionColor">The disabled option color</param>
        /// <param name="OutputType">Output type of choices</param>
        /// <param name="PressEnter">When enabled, allows the input to consist of multiple characters</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, Color questionColor, Color inputColor, Color optionColor, Color altOptionColor, Color disabledOptionColor, ChoiceOutputType OutputType = ChoiceOutputType.OneLine, bool PressEnter = false)
        {
            // Check for answer count
            if (Answers.Length == 0 && AltAnswers.Length == 0)
                throw new TerminauxException("Can't show choice with no answers and no alternative answers.");

            // We need not to run the selection style when everything is disabled
            bool allDisabled = Answers.All((ici) => ici.ChoiceDisabled) && AltAnswers.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException("The choice style requires that there is at least one choice enabled.");

            // Main loop
            while (true)
            {
                string answer;

                // Check to see if the answers consist of single or multiple characters, and exit if the mode is not appropriate with an error message.
                Dictionary<string, bool> answers = Answers.ToDictionary((ici) => ici.ChoiceName, (ici) => ici.ChoiceDisabled);
                Dictionary<string, bool> altAnswers = AltAnswers.ToDictionary((ici) => ici.ChoiceName, (ici) => ici.ChoiceDisabled);
                string[] answerNames = [.. answers.Keys];
                string[] altAnswerNames = [.. altAnswers.Keys];
                if (!PressEnter && (answerNames.Any((answer) => answer.Length > 1) || altAnswerNames.Any((answer) => answer.Length > 1)))
                {
                    TextWriterColor.Write("Can't provide answers with more than one character in single-character mode.");
                    return "";
                }

                // Ask a question
                switch (OutputType)
                {
                    case ChoiceOutputType.OneLine:
                        {
                            string answersPlace = altAnswers.Count > 0 ? " <{0}/{1}> " : " <{0}> ";
                            TextWriterColor.WriteColor(Question, false, questionColor);
                            TextWriterColor.WriteColor(answersPlace, false, inputColor, string.Join("/", answerNames), string.Join("/", altAnswerNames));
                            break;
                        }
                    case ChoiceOutputType.TwoLines:
                        {
                            string answersPlace = altAnswers.Count > 0 ? "<{0}/{1}> " : "<{0}> ";
                            TextWriterColor.WriteColor(Question, true, questionColor);
                            TextWriterColor.WriteColor(answersPlace, false, inputColor, string.Join("/", answerNames), string.Join("/", altAnswerNames));
                            break;
                        }
                    case ChoiceOutputType.Modern:
                        {
                            TextWriterColor.WriteColor(Question + CharManager.NewLine, true, questionColor);
                            if (answers.Count > 0)
                                PrintChoices(Answers, optionColor, disabledOptionColor);
                            if (altAnswers.Count > 0)
                            {
                                if (answers.Count > 0)
                                    TextWriterRaw.Write();
                                PrintChoices(AltAnswers, altOptionColor, disabledOptionColor);
                            }
                            TextWriterColor.WriteColor(CharManager.NewLine + ">> ", false, inputColor);
                            break;
                        }
                    case ChoiceOutputType.Table:
                        {
                            var ChoiceHeader = new[] { "Possible answers", "Answer description" };
                            var ChoiceData = new string[Answers.Length + AltAnswers.Length, 2];
                            TextWriterColor.WriteColor(Question, true, questionColor);
                            for (int AnswerIndex = 0; AnswerIndex <= Answers.Length - 1; AnswerIndex++)
                            {
                                ChoiceData[AnswerIndex, 0] = Answers[AnswerIndex].ChoiceName;
                                ChoiceData[AnswerIndex, 1] = Answers[AnswerIndex].ChoiceTitle ?? "";
                            }
                            for (int AnswerIndex = 0; AnswerIndex <= AltAnswers.Length - 1; AnswerIndex++)
                            {
                                ChoiceData[Answers.Length - 1 + AnswerIndex, 0] = AltAnswers[AnswerIndex].ChoiceName;
                                ChoiceData[Answers.Length - 1 + AnswerIndex, 1] = AltAnswers[AnswerIndex].ChoiceTitle ?? "";
                            }
                            TableColor.WriteTable(ChoiceHeader, ChoiceData, 2);
                            TextWriterColor.WriteColor(CharManager.NewLine + ">> ", false, inputColor);
                            break;
                        }
                }

                // Wait for an answer
                if (PressEnter)
                {
                    answer = TermReader.Read();
                }
                else
                {
                    answer = Convert.ToString(TermReader.ReadKey().KeyChar);
                    TextWriterRaw.WritePlain(answer);
                }

                // Check if answer is correct.
                if (answerNames.Contains(answer) && !answers[answer] || altAnswerNames.Contains(answer) && !altAnswers[answer])
                    return answer;
                else
                    TextWriterColor.Write("Wrong answer. Please try again.");
            }
        }

        private static void PrintChoices(InputChoiceInfo[] answerInfos, Color answerColor, Color disabledOptionColor)
        {
            int AnswerTitleLeft = answerInfos.Max(x => $" {x.ChoiceName}) ".Length);
            for (int AnswerIndex = 0; AnswerIndex <= answerInfos.Length - 1; AnswerIndex++)
            {
                var AnswerInstance = answerInfos[AnswerIndex];
                string AnswerTitle = AnswerInstance.ChoiceTitle ?? "";
                string AnswerOption = $" {AnswerInstance.ChoiceName}) {AnswerTitle}";
                if (AnswerTitleLeft < ConsoleWrapper.WindowWidth)
                {
                    int blankRepeats = AnswerTitleLeft - $" {AnswerInstance.ChoiceName}) ".Length;
                    AnswerOption = $" {AnswerInstance.ChoiceName}) " + new string(' ', blankRepeats) + $"{AnswerTitle}";
                }
                TextWriterColor.WriteColor(AnswerOption, true, AnswerInstance.ChoiceDisabled ? disabledOptionColor : answerColor);
            }
        }

        static ChoiceStyle()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
