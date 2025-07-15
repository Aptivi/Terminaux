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

using Terminaux.Base;

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
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, (string, string)[] Answers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, [], kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, [], SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answer categories.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceCategoryInfo[] Answers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, [], SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answer categories.</param>
        /// <param name="AltAnswers">Set of alternate answer categories.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceCategoryInfo[] Answers, InputChoiceCategoryInfo[] AltAnswers, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, (string, string)[] Answers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), settings ?? SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null) =>
            PromptSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk, currentSelection, currentSelected);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null)
        {
            InputChoiceCategoryInfo[] answersCategory =
            [
                new InputChoiceCategoryInfo(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_GENERALCHOICES"),
                [
                    new InputChoiceGroupInfo(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_AVAILABLECHOICES"), Answers)
                ])
            ];
            InputChoiceCategoryInfo[] altAnswersCategory = AltAnswers.Length == 0 ? [] :
            [
                new InputChoiceCategoryInfo(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_ALTERNATIVECHOICES"),
                [
                    new InputChoiceGroupInfo(LanguageTools.GetLocalized("T_INPUT_STYLES_SELECTION_AVAILABLECHOICES"), AltAnswers)
                ])
            ];
            return PromptSelection(Question, answersCategory, altAnswersCategory, settings, kiosk, currentSelection, currentSelected);
        }

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answer categories.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceCategoryInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null)
        {
            currentSelection ??= SelectionInputTools.GetDefaultChoice(Answers) + 1;
            currentSelected ??= SelectionInputTools.GetDefaultChoice(Answers) + 1;
            var answers = SelectionStyleBase.PromptSelection(Question, Answers, [], settings, kiosk, false, null, currentSelection, currentSelected);
            if (answers.Length == 0)
                return -1;
            return answers[0] + 1;
        }

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answer categories.</param>
        /// <param name="AltAnswers">Set of alternate answer categories.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <param name="currentSelected">Current selected choice (for radio buttons)</param>
        /// <returns>A one-based selection choice number, or -1 if the user has cancelled the input</returns>
        public static int PromptSelection(string Question, InputChoiceCategoryInfo[] Answers, InputChoiceCategoryInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false, int? currentSelection = null, int? currentSelected = null)
        {
            currentSelection ??= SelectionInputTools.GetDefaultChoice([.. Answers, .. AltAnswers]) + 1;
            currentSelected ??= SelectionInputTools.GetDefaultChoice([.. Answers, .. AltAnswers]) + 1;
            var answers = SelectionStyleBase.PromptSelection(Question, Answers, AltAnswers, settings, kiosk, false, null, currentSelection, currentSelected);
            if (answers.Length == 0)
                return -1;
            return answers[0] + 1;
        }
    }
}
