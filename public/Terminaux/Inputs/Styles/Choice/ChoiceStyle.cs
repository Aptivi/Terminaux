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
using Terminaux.Base;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;
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
        /// <param name="settings">Choice style settings</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, ChoiceStyleSettings? settings = null) =>
            PromptChoice(Question, Answers, [], settings);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers and working titles for each answer in one tuple.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="settings">Choice style settings</param>
        public static string PromptChoice(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, ChoiceStyleSettings? settings = null) =>
            PromptChoice(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), settings);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Choice style settings</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, ChoiceStyleSettings? settings = null) =>
            PromptChoice(Question, Answers, [], settings);

        /// <summary>
        /// Prompts user for choice
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Choice style settings</param>
        public static string PromptChoice(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, ChoiceStyleSettings? settings = null)
        {
            // Check for answer count
            if (Answers.Length == 0 && AltAnswers.Length == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_STYLES_CHOICE_EXCEPTION_NOANSWERS"));

            // We need not to run the selection style when everything is disabled
            bool allDisabled = Answers.All((ici) => ici.ChoiceDisabled) && AltAnswers.All((ici) => ici.ChoiceDisabled);
            if (allDisabled)
                throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_STYLES_CHOICE_EXCEPTION_NEEDSATLEASTONEITEM"));

            // Check settings and assign them if not found
            settings ??= new();

            // Main loop
            while (true)
            {
                string answer;

                // Check to see if the answers consist of single or multiple characters, and exit if the mode is not appropriate with an error message.
                Dictionary<string, bool> answers = Answers.ToDictionary((ici) => ici.ChoiceName, (ici) => ici.ChoiceDisabled);
                Dictionary<string, bool> altAnswers = AltAnswers.ToDictionary((ici) => ici.ChoiceName, (ici) => ici.ChoiceDisabled);
                string[] answerNames = [.. answers.Keys];
                string[] altAnswerNames = [.. altAnswers.Keys];
                if (!settings.PressEnter && (answerNames.Any((answer) => answer.Length > 1) || altAnswerNames.Any((answer) => answer.Length > 1)))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_INPUT_STYLES_CHOICE_EXCEPTION_ONECHARINVALIDCHOICES"));

                // Ask a question
                switch (settings.OutputType)
                {
                    case ChoiceOutputType.OneLine:
                        {
                            string answersPlace = altAnswers.Count > 0 ? " <{0}/{1}> " : " <{0}> ";
                            TextWriterColor.WriteColor(Question, false, settings.QuestionColor);
                            TextWriterColor.WriteColor(answersPlace, false, settings.InputColor, string.Join("/", answerNames), string.Join("/", altAnswerNames));
                            break;
                        }
                    case ChoiceOutputType.TwoLines:
                        {
                            string answersPlace = altAnswers.Count > 0 ? "<{0}/{1}> " : "<{0}> ";
                            TextWriterColor.WriteColor(Question, true, settings.QuestionColor);
                            TextWriterColor.WriteColor(answersPlace, false, settings.InputColor, string.Join("/", answerNames), string.Join("/", altAnswerNames));
                            break;
                        }
                    case ChoiceOutputType.Modern:
                        {
                            var selection = new PassiveSelection([.. Answers, .. AltAnswers])
                            {
                                AltChoicePos = Answers.Length,
                                Settings = new()
                                {
                                    OptionColor = settings.OptionColor,
                                    AltOptionColor = settings.AltOptionColor,
                                    DisabledOptionColor = settings.DisabledOptionColor,
                                }
                            };
                            TextWriterColor.WriteColor(Question + CharManager.NewLine, true, settings.QuestionColor);
                            TextWriterRaw.WritePlain(selection.Render());
                            TextWriterColor.WriteColor(">> ", false, settings.InputColor);
                            break;
                        }
                }

                // Wait for an answer
                if (settings.PressEnter)
                    answer = TermReader.Read();
                else
                {
                    answer = Convert.ToString(Input.ReadKey().KeyChar);
                    TextWriterRaw.WritePlain(answer);
                }

                // Check if answer is correct.
                if (answerNames.Contains(answer) && !answers[answer] || altAnswerNames.Contains(answer) && !altAnswers[answer])
                    return answer;
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("T_INPUT_STYLES_CHOICE_WRONGANSWER"));
            }
        }
    }
}
