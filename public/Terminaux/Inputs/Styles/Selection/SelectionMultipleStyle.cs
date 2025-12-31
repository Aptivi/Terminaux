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

using Terminaux.Base;

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
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, (string, string)[] Answers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, [], kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, [], kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceCategoryInfo[] Answers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, [], kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceCategoryInfo[] Answers, InputChoiceCategoryInfo[] AltAnswers, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, AltAnswers, SelectionStyleSettings.GlobalSettings, kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, (string, string)[] Answers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AlternateAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, (string, string)[] Answers, (string, string)[] AlternateAnswers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, InputChoiceTools.GetInputChoices(Answers), InputChoiceTools.GetInputChoices(AlternateAnswers), settings ?? SelectionStyleSettings.GlobalSettings, kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null) =>
            PromptMultipleSelection(Question, Answers, [], settings ?? SelectionStyleSettings.GlobalSettings, kiosk, initialChoices, currentSelection);

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null)
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
            return PromptMultipleSelection(Question, answersCategory, altAnswersCategory, settings, kiosk, initialChoices, currentSelection);
        }

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceCategoryInfo[] Answers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null)
        {
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(Answers)];
            initialChoices ??= SelectionInputTools.SelectDefaults(choices);
            currentSelection ??= SelectionInputTools.GetDefaultChoice(choices);
            return SelectionStyleBase.PromptSelection(Question, Answers, [], settings, kiosk, true, initialChoices, currentSelection, currentSelection);
        }

        /// <summary>
        /// Prompts user for selection
        /// </summary>
        /// <param name="Question">A question</param>
        /// <param name="Answers">Set of answers.</param>
        /// <param name="AltAnswers">Set of alternate answers.</param>
        /// <param name="settings">Selection settings</param>
        /// <param name="kiosk">Whether to prevent exiting or not</param>
        /// <param name="initialChoices">Zero-based index numbers of selected choices</param>
        /// <param name="currentSelection">Current selection (the choice that will be highlighted)</param>
        /// <returns>An array of zero-based choice indexes, or an empty array if the user has cancelled the input</returns>
        public static int[] PromptMultipleSelection(string Question, InputChoiceCategoryInfo[] Answers, InputChoiceCategoryInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false, int[]? initialChoices = null, int? currentSelection = null)
        {
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(Answers), .. SelectionInputTools.GetChoicesFromCategories(AltAnswers)];
            initialChoices ??= SelectionInputTools.SelectDefaults(choices);
            currentSelection ??= SelectionInputTools.GetDefaultChoice(choices);
            return SelectionStyleBase.PromptSelection(Question, Answers, AltAnswers, settings, kiosk, true, initialChoices, currentSelection, currentSelection);
        }
    }
}
