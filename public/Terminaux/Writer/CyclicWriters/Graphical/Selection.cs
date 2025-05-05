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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Selection cyclic renderer
    /// </summary>
    public class Selection : GraphicalCyclicWriter
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
        /// Current selection (zero-based)
        /// </summary>
        public int CurrentSelection { get; set; }

        /// <summary>
        /// Current selections (null for single selection)
        /// </summary>
        public int[]? CurrentSelections { get; set; }

        /// <summary>
        /// Whether to swap the selected colors or not
        /// </summary>
        public bool SwapSelectedColors { get; set; }

        /// <summary>
        /// Whether to show the radio buttons or not (ignored when <see cref="CurrentSelections"/> is not null)
        /// </summary>
        public bool ShowRadioButtons { get; set; }

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
        /// Selected option foreground color
        /// </summary>
        public Color SelectedForegroundColor =>
            Settings.SelectedOptionColor;

        /// <summary>
        /// Selected option background color
        /// </summary>
        public Color SelectedBackgroundColor =>
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
        /// Selected alternative option foreground color
        /// </summary>
        public Color AltSelectedForegroundColor =>
            Settings.SelectedOptionColor;

        /// <summary>
        /// Selected alternative option background color
        /// </summary>
        public Color AltSelectedBackgroundColor =>
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
        /// Whether to use ellipsis for selection entries or not
        /// </summary>
        public bool Ellipsis { get; set; } = true;

        /// <summary>
        /// Checks to see whether the generation of the selection hitboxes is possible
        /// </summary>
        /// <param name="hitboxIdx">Hitbox index</param>
        /// <param name="hitbox">Pointer hitbox instances that are built for each selection</param>
        /// <returns>True if generation is possible; false otherwise</returns>
        public bool CanGenerateSelectionHitbox(int hitboxIdx, out (PointerHitbox hitbox, ChoiceHitboxType type, int related) hitbox) =>
            CanGenerateSelectionHitbox(CurrentSelection, hitboxIdx, out hitbox);

        /// <summary>
        /// Checks to see whether the generation of the selection hitboxes is possible
        /// </summary>
        /// <param name="selectionIdx">Selection index from all choices</param>
        /// <param name="hitboxIdx">Hitbox index</param>
        /// <param name="hitbox">Pointer hitbox instances that are built for each selection</param>
        /// <returns>True if generation is possible; false otherwise</returns>
        public bool CanGenerateSelectionHitbox(int selectionIdx, int hitboxIdx, out (PointerHitbox hitbox, ChoiceHitboxType type, int related) hitbox)
        {
            // Get the choice parameters
            (_, List<int> selectionHeights) = GetChoiceParameters();
            hitbox = default;
            if (selectionHeights.Count == 0 || selectionIdx < 0 || selectionIdx >= selectionHeights.Count)
                return false;

            // Generate the hitboxes and verify the values
            var hitboxes = GenerateSelectionHitboxes(selectionIdx);
            if (hitboxes.Length == 0 || hitboxIdx < 0 || hitboxIdx >= hitboxes.Length)
                return false;

            // Return true once we get the hitbox
            hitbox = hitboxes[hitboxIdx];
            return true;
        }

        /// <summary>
        /// Generates the selection hitboxes
        /// </summary>
        /// <param name="hitboxIdx">Hitbox index</param>
        /// <returns>Pointer hitbox instances that are built for each selection</returns>
        public (PointerHitbox hitbox, ChoiceHitboxType type, int related) GenerateSelectionHitbox(int hitboxIdx) =>
            GenerateSelectionHitbox(CurrentSelection, hitboxIdx);

        /// <summary>
        /// Generates the selection hitboxes
        /// </summary>
        /// <param name="selectionIdx">Selection index from all choices</param>
        /// <param name="hitboxIdx">Hitbox index</param>
        /// <returns>Pointer hitbox instances that are built for each selection</returns>
        public (PointerHitbox hitbox, ChoiceHitboxType type, int related) GenerateSelectionHitbox(int selectionIdx, int hitboxIdx) =>
            GenerateSelectionHitboxes(selectionIdx)[hitboxIdx];

        /// <summary>
        /// Generates the selection hitboxes
        /// </summary>
        /// <returns>Pointer hitbox instances that are built for each selection</returns>
        public (PointerHitbox hitbox, ChoiceHitboxType type, int related)[] GenerateSelectionHitboxes() =>
            GenerateSelectionHitboxes(CurrentSelection);

        /// <summary>
        /// Generates the selection hitboxes
        /// </summary>
        /// <param name="selectionIdx">Selection index from all choices</param>
        /// <returns>Pointer hitbox instances that are built for each selection</returns>
        public (PointerHitbox hitbox, ChoiceHitboxType type, int related)[] GenerateSelectionHitboxes(int selectionIdx)
        {
            // Get the choice parameters
            (List<(string text, Color fore, Color back, bool force, ChoiceHitboxType type, int related)> choiceText, List<int> selectionHeights) = GetChoiceParameters();

            // Get the choice hitboxes
            List<(PointerHitbox hitbox, ChoiceHitboxType type, int related)> hitboxes = [];
            int selectionHeight = selectionHeights[selectionIdx];
            int currentPage = (selectionHeight - 1) / Height;
            int startIndex = Height * currentPage;
            for (int i = 0; i <= Height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                if (finalIndex >= choiceText.Count)
                    break;
                int optionTop = Top + finalIndex - startIndex;
                Coordinate start = new(Left, optionTop);
                Coordinate end = new(Left + Width, optionTop);
                (_, _, _, _, var type, int related) = choiceText[finalIndex];
                hitboxes.Add((new(start, end, null), type, related));
            }
            return [.. hitboxes];
        }

        /// <summary>
        /// Renders a selection
        /// </summary>
        /// <returns>A string representation of the selection</returns>
        public override string Render()
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException("Can't determine if the selection input is single or multiple");
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Get the choice parameters
            (List<(string text, Color fore, Color back, bool force, ChoiceHitboxType type, int related)> choiceText, List<int> selectionHeights) = GetChoiceParameters();

            // Render the choices
            int selectionHeight = selectionHeights[CurrentSelection];
            int currentPage = (selectionHeight - 1) / Height;
            int startIndex = Height * currentPage;
            bool wiped = false;
            StringBuilder buffer = new();
            for (int i = 0; i <= Height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                int optionTop = Top + finalIndex - startIndex;
                if (finalIndex >= selectionHeights[selectionHeights.Count - 1])
                {
                    if (UseColors && !wiped)
                    {
                        wiped = true;
                        buffer.Append(
                            ColorTools.RenderRevertForeground() +
                            ColorTools.RenderRevertBackground()
                        );
                    }
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(Left + 1, optionTop + 1) +
                        new string(' ', Width)
                    );
                }
                else
                {
                    var (text, fore, back, force, _, _) = choiceText[finalIndex];
                    if (UseColors || force)
                    {
                        buffer.Append(
                            ColorTools.RenderSetConsoleColor(fore) +
                            ColorTools.RenderSetConsoleColor(back, true)
                        );
                    }
                    string truncated = text.Truncate(Width, Ellipsis);
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(Left + 1, optionTop + 1) +
                        truncated + new string(' ', Width - ConsoleChar.EstimateCellWidth(truncated))
                    );
                }
            }

            // Render the vertical bar
            if (choices.Count > Height && Height >= 4)
            {
                int finalWidth = Left + Width;
                var slider = new Slider(CurrentSelection + 1, 0, choices.Count)
                {
                    Vertical = true,
                    Height = Height - 2,
                    SliderVerticalActiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                    SliderVerticalInactiveTrackChar = BorderSettings.GlobalSettings.BorderRightFrameChar,
                };
                if (UseColors)
                {
                    slider.SliderActiveForegroundColor = ForegroundColor;
                    slider.SliderForegroundColor = TransformationTools.GetDarkBackground(ForegroundColor);
                    slider.SliderBackgroundColor = BackgroundColor;
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack("▲", finalWidth, Top, ForegroundColor, BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▼", finalWidth, Top + Height - 1, ForegroundColor, BackgroundColor)
                    );
                }
                else
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWhere("▲", finalWidth, Top) +
                        TextWriterWhereColor.RenderWhere("▼", finalWidth, Top + Height - 1)
                    );
                }
                buffer.Append(
                    RendererTools.RenderRenderable(slider, new(finalWidth, Top + 1))
                );
            }

            // Render the final result
            if (UseColors)
            {
                buffer.Append(
                    ColorTools.RenderRevertForeground() +
                    ColorTools.RenderRevertBackground()
                );
            }
            return buffer.ToString();
        }

        internal (List<(string text, Color fore, Color back, bool force, ChoiceHitboxType type, int related)> choiceText, List<int> selectionHeights) GetChoiceParameters()
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException("Can't determine if the selection input is single or multiple");
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice parameters
            List<(string text, Color fore, Color back, bool force, ChoiceHitboxType type, int related)> choiceText = [];
            List<int> selectionHeights = [];
            int processedHeight = 0;
            int processedChoices = 0;
            int startIndexTristates = 0;
            int startIndexGroupTristates = 0;
            int relatedIdx = -1;
            var tristates = isMultiple ? SelectionInputTools.GetCategoryTristates(Selections, choices, CurrentSelections, ref startIndexTristates) : [];
            string prefix = isMultiple ? "  [ ] " : ShowRadioButtons ? "  ( ) " : "  ";
            int AnswerTitleLeft = choices.Max(x => ConsoleChar.EstimateCellWidth(Selections.Length > 1 ? $"  {prefix}{x.ChoiceName}) " : $"{prefix}{x.ChoiceName}) "));
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                var tristate = isMultiple ? tristates[categoryIdx] : SelectionTristate.Unselected;
                if (Selections.Length > 1)
                {
                    string modifiers = $"{(isMultiple ? tristate == SelectionTristate.Selected ? "[*] " : tristate == SelectionTristate.FiftyFifty ? "[/] " : "[ ] " : "")}";
                    string finalRendered = $"{modifiers}{category.Name}";
                    choiceText.Add((finalRendered, ConsoleColorData.Silver.Color, BackgroundColor, true, ChoiceHitboxType.Category, relatedIdx == -1 ? 1 : relatedIdx + 2));
                    processedHeight++;
                }

                var groupTristates = isMultiple ? SelectionInputTools.GetGroupTristates(category.Groups, choices, CurrentSelections, ref startIndexGroupTristates) : [];
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    var groupTristate = isMultiple ? groupTristates[groupIdx] : SelectionTristate.Unselected;
                    if (category.Groups.Length > 1)
                    {
                        string modifiers = $"{(isMultiple ? groupTristate == SelectionTristate.Selected ? "[*] " : groupTristate == SelectionTristate.FiftyFifty ? "[/] " : "[ ] " : "")}";
                        string finalRendered = $"  {modifiers}{group.Name}";
                        choiceText.Add((finalRendered, ConsoleColorData.Grey.Color, BackgroundColor, true, ChoiceHitboxType.Group, relatedIdx == -1 ? 1 : relatedIdx + 2));
                        processedHeight++;
                    }
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        relatedIdx++;
                        bool selected = processedChoices == CurrentSelection;
                        var choice = group.Choices[i];
                        string AnswerTitle = choice.ChoiceTitle ?? "";
                        bool disabled = choice.ChoiceDisabled;

                        // Get the option
                        string selectionIndicator = selected ? ">" : disabled ? "X" : " ";
                        string selectedIndicator =
                            isMultiple ? $" [{(CurrentSelections.Contains(relatedIdx) ? "*" : " ")}]" :
                            ShowRadioButtons ? $" ({(selected ? "*" : " ")})" : "";
                        string modifiers = $"{selectionIndicator}{selectedIndicator}";
                        string AnswerOption = Selections.Length > 1 ? $"  {modifiers} {choice.ChoiceName}) {AnswerTitle}" : $"{modifiers} {choice.ChoiceName}) {AnswerTitle}";
                        if (AnswerTitleLeft < Width)
                        {
                            string renderedChoice = Selections.Length > 1 ? $"  {modifiers} {choice.ChoiceName}) " : $"{modifiers} {choice.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - ConsoleChar.EstimateCellWidth(renderedChoice);
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}";
                        }

                        // Render an entry
                        bool isAlt = processedChoices + 1 > AltChoicePos;
                        var finalForeColor =
                            choice.ChoiceDisabled ? DisabledForegroundColor :
                            selected ?
                                isAlt ?
                                    SwapSelectedColors ? AltSelectedBackgroundColor : AltSelectedForegroundColor :
                                    SwapSelectedColors ? SelectedBackgroundColor : SelectedForegroundColor
                                 :
                                isAlt ? AltForegroundColor : ForegroundColor;
                        var finalBackColor =
                            choice.ChoiceDisabled ? DisabledBackgroundColor :
                            selected ?
                                isAlt ?
                                    SwapSelectedColors ? AltSelectedForegroundColor : AltSelectedBackgroundColor :
                                    SwapSelectedColors ? SelectedForegroundColor : SelectedBackgroundColor
                                 :
                                isAlt ? AltBackgroundColor : BackgroundColor;
                        choiceText.Add((AnswerOption, finalForeColor, finalBackColor, false, ChoiceHitboxType.Choice, relatedIdx + 1));
                        processedHeight++;
                        processedChoices++;
                        selectionHeights.Add(processedHeight);
                    }
                }
            }

            // Return the parameters
            return (choiceText, selectionHeights);
        }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        public Selection()
        { }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="categories">Categories</param>
        public Selection(InputChoiceCategoryInfo[] categories)
        {
            Selections = categories;
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            AltChoicePos = choices.Count;
        }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="groups">Groups</param>
        public Selection(InputChoiceGroupInfo[] groups) :
            this([new InputChoiceCategoryInfo("Selection category", groups)])
        { }

        /// <summary>
        /// Makes a new selection instance
        /// </summary>
        /// <param name="choices">Choices</param>
        public Selection(InputChoiceInfo[] choices) :
            this([new InputChoiceCategoryInfo("Selection category", [new("Selection group", choices)])])
        { }
    }
}
