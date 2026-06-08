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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terminaux.Base;
using Terminaux.Inputs.Styles.Infobox;
using Textify.Tools;

namespace Terminaux.Inputs.Styles
{
    /// <summary>
    /// Input choice tools
    /// </summary>
    public static class InputChoiceTools
    {
        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="Answers">Set of working titles.</param>
        public static InputChoiceInfo[] GetInputChoices(string[] Answers) =>
            GetInputChoices(Answers.Select((answer, idx) => ($"{idx + 1}", answer)).ToArray());

        /// <summary>
        /// Gets the input choices
        /// </summary>
        /// <param name="Answers">Set of answers and working titles for each answer in one tuple.</param>
        public static InputChoiceInfo[] GetInputChoices((string, string)[] Answers)
        {
            // Variables
            var finalChoices = new List<InputChoiceInfo>();

            // Now, populate choice information from the arrays
            for (int i = 0; i < Answers.Length; i++)
            {
                string answer = string.IsNullOrEmpty(Answers[i].Item1) ? $"{i + 1}" : Answers[i].Item1;
                string title = string.IsNullOrEmpty(Answers[i].Item2) ? LanguageTools.GetLocalized("T_INPUT_STYLES_CHOICE_UNTITLEDANSWER") + $" #{i + 1}" : Answers[i].Item2;
                ConsoleLogger.Debug("Adding choice \"{0}\": \"{1}\"", answer, title);
                finalChoices.Add(new InputChoiceInfo(answer, title));
            }
            return [.. finalChoices];
        }

        internal static int GetEntryIdxFromSearchPrompt(InputModule[] modules, out (string choiceName, string choiceTitle, bool choiceDisabled, int itemIdx)[] resultEntries)
        {
            // Convert input module instances to input choice info
            var entriesString = modules.Select((entry) => new InputChoiceInfo(entry.Name, "", entry.Description)).ToArray();
            return GetEntryIdxFromSearchPrompt(entriesString, out resultEntries);
        }

        internal static int GetEntryIdxFromSearchPrompt(InputChoiceInfo[] allAnswers, out (string choiceName, string choiceTitle, bool choiceDisabled, int itemIdx)[] resultEntries)
        {
            // Prompt the user for search term
            resultEntries = [];
            var entriesString = allAnswers.Select((entry) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled)).ToArray();
            string keyword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("T_INPUT_COMMON_SEARCHPROMPT"));
            if (!RegexTools.IsValidRegex(keyword))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_INVALIDQUERY"));
                return -1;
            }

            // Get the result entries
            var regex = new Regex(keyword);
            resultEntries = entriesString
                .Select((entry, idx) => (entry.ChoiceName, entry.ChoiceTitle, entry.ChoiceDisabled, itemIdx: idx))
                .Where((entry) => (regex.IsMatch(entry.ChoiceName) || regex.IsMatch(entry.ChoiceTitle)) && !entry.ChoiceDisabled).ToArray();

            // Act, depending on the result entries
            int idx = 0;
            if (resultEntries.Length > 1)
            {
                var choices = resultEntries.Select((tuple) => new InputChoiceInfo(tuple.choiceName, tuple.choiceTitle)).ToArray();
                idx = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("T_INPUT_COMMON_ENTRYPROMPT"));
                if (idx < 0)
                    return -1;
            }
            else if (resultEntries.Length == 1)
                idx = 0;
            else
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("T_INPUT_COMMON_NOITEMS"));
                return -1;
            }
            return idx;
        }
    }
}
