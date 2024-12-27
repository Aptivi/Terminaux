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
        public static int PromptSelection(string Question, InputChoiceInfo[] Answers, InputChoiceInfo[] AltAnswers, SelectionStyleSettings settings, bool kiosk = false) =>
            SelectionStyleBase.PromptSelection(Question, Answers, AltAnswers, settings, kiosk, false)[0] + 1;
    }
}
