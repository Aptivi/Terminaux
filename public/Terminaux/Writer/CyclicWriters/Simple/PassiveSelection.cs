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
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Passive selection cyclic renderer
    /// </summary>
    public class PassiveSelection : SimpleCyclicWriter
    {
        /// <summary>
        /// List of selection categories
        /// </summary>
        public InputChoiceCategoryInfo[] Selections { get; set; } = [];

        /// <summary>
        /// Alternative choice position (one-based)
        /// </summary>
        public int AltChoicePos { get; set; }

        /// <summary>
        /// Selection style settings
        /// </summary>
        public SelectionStyleSettings Settings { get; set; } = new();

        /// <summary>
        /// Option foreground color
        /// </summary>
        public Color ForegroundColor =>
            Settings.OptionColor;

        /// <summary>
        /// Option background color
        /// </summary>
        public Color BackgroundColor =>
            Settings.BackgroundColor;

        /// <summary>
        /// Alternative option foreground color
        /// </summary>
        public Color AltForegroundColor =>
            Settings.AltOptionColor;

        /// <summary>
        /// Alternative option background color
        /// </summary>
        public Color AltBackgroundColor =>
            Settings.BackgroundColor;

        /// <summary>
        /// Disabled option foreground color
        /// </summary>
        public Color DisabledForegroundColor =>
            Settings.DisabledOptionColor;

        /// <summary>
        /// Disabled option background color
        /// </summary>
        public Color DisabledBackgroundColor =>
            Settings.BackgroundColor;

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors { get; set; } = true;

        /// <summary>
        /// Renders a selection
        /// </summary>
        /// <returns>A string representation of the selection</returns>
        public override string Render()
        {
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Get the choice parameters
            List<(string text, Color fore, Color back, bool force)> choiceText = GetChoiceParameters();

            // Render the choices
            StringBuilder buffer = new();
            foreach (var (text, fore, back, force) in choiceText)
            {
                if (UseColors || force)
                {
                    buffer.Append(
                        ConsoleColoring.RenderSetConsoleColor(fore) +
                        ConsoleColoring.RenderSetConsoleColor(back, true)
                    );
                }
                buffer.AppendLine(text);
            }

            // Render the final result
            if (UseColors)
            {
                buffer.Append(
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return buffer.ToString();
        }

        internal List<(string text, Color fore, Color back, bool force)> GetChoiceParameters()
        {
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice parameters
            List<(string text, Color fore, Color back, bool force)> choiceText = [];
            int processedChoices = 0;
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                if (Selections.Length > 1)
                    choiceText.Add((category.Name, ConsoleColorData.Silver.Color, BackgroundColor, true));

                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    if (category.Groups.Length > 1)
                        choiceText.Add(($"  {group.Name}", ConsoleColorData.Grey.Color, BackgroundColor, true));
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        var choice = group.Choices[i];
                        string AnswerTitle = choice.ChoiceTitle ?? "";
                        bool disabled = choice.ChoiceDisabled;

                        // Get the option
                        string AnswerOption = Selections.Length > 1 ? $"    {choice.ChoiceName}) {AnswerTitle}" : $"  {choice.ChoiceName}) {AnswerTitle}";

                        // Render an entry
                        bool isAlt = processedChoices + 1 > AltChoicePos;
                        var finalForeColor =
                            disabled ? DisabledForegroundColor :
                            isAlt ? AltForegroundColor : ForegroundColor;
                        var finalBackColor =
                            disabled ? DisabledBackgroundColor :
                            isAlt ? AltBackgroundColor : BackgroundColor;
                        choiceText.Add((AnswerOption, finalForeColor, finalBackColor, false));
                        processedChoices++;
                    }
                }
            }

            // Return the parameters
            return choiceText;
        }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        public PassiveSelection()
        { }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="categories">Categories</param>
        public PassiveSelection(InputChoiceCategoryInfo[] categories)
        {
            Selections = categories;
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            AltChoicePos = choices.Count;
        }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="groups">Groups</param>
        public PassiveSelection(InputChoiceGroupInfo[] groups) :
            this([new InputChoiceCategoryInfo("Selection category", groups)])
        { }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="choices">Choices</param>
        public PassiveSelection(InputChoiceInfo[] choices) :
            this([new InputChoiceCategoryInfo("Selection category", [new("Selection group", choices)])])
        { }
    }
}
