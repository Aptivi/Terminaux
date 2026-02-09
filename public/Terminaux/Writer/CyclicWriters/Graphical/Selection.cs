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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Selection cyclic renderer
    /// </summary>
    public class Selection : GraphicalCyclicWriter
    {
        internal List<string> choiceTexts = [];
        private int selectedChoice;
        private bool cached;
        private InputChoiceCategoryInfo[] selections = [];

        /// <summary>
        /// List of selection categories
        /// </summary>
        public InputChoiceCategoryInfo[] Selections
        {
            get => selections;
            set
            {
                List<InputChoiceInfo> oldChoices = SelectionInputTools.GetChoicesFromCategories(Selections);
                List<InputChoiceInfo> newChoices = SelectionInputTools.GetChoicesFromCategories(value);
                if (!newChoices.SequenceEqual(oldChoices))
                    InvalidateCache();
                selections = value;
            }
        }

        /// <inheritdoc/>
        public override int Width
        {
            get => base.Width;
            set
            {
                if (Width != value)
                    InvalidateCache();
                base.Width = value;
            }
        }

        /// <summary>
        /// Alternative choice position (one-based)
        /// </summary>
        public int AltChoicePos { get; set; }

        /// <summary>
        /// Current selection (zero-based)
        /// </summary>
        /// <remarks>
        /// Use this property only if either <see cref="CurrentSelections"/> is not null or if <see cref="ShowRadioButtons"/> is not true. Otherwise,
        /// rely on the value of <see cref="SelectedChoice"/>
        /// </remarks>
        public int CurrentSelection { get; set; }

        /// <summary>
        /// Selected choice (zero-based, -1 if <see cref="ShowRadioButtons"/> is not true or if <see cref="CurrentSelections"/> is not null)
        /// </summary>
        /// <remarks>
        /// Use this property only if <see cref="CurrentSelections"/> is null and <see cref="ShowRadioButtons"/> is true. Otherwise,
        /// rely on the value of <see cref="CurrentSelection"/>
        /// </remarks>
        public int SelectedChoice
        {
            get => !ShowRadioButtons || CurrentSelections is not null ? -1 : selectedChoice;
            set => selectedChoice = value;
        }

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
        /// Slider foreground color
        /// </summary>
        public Color SliderColor =>
            Settings.SliderColor;

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
            List<int> selectionHeights = GetSelectionHeights();
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
            List<int> selectionHeights = GetSelectionHeights();
            List<(ChoiceHitboxType type, int related)> relatedHeights = GetRelatedHeights();

            // Get the choice hitboxes
            List<(PointerHitbox hitbox, ChoiceHitboxType type, int related)> hitboxes = [];
            int selectionHeight = selectionHeights[selectionIdx];
            int currentPage = (selectionHeight - 1) / Height;
            int startIndex = Height * currentPage;
            for (int i = 0; i <= Height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                if (finalIndex >= relatedHeights.Count)
                    break;
                int optionTop = Top + finalIndex - startIndex - 1;
                Coordinate start = new(Left, optionTop);
                Coordinate end = new(Left + Width, optionTop);
                (var type, int related) = relatedHeights[finalIndex];
                hitboxes.Add((new(start, end, null), type, related));
            }
            return [.. hitboxes];
        }

        /// <summary>
        /// Gets the hitbox index from the selection index
        /// </summary>
        /// <returns>Hitbox index that represents the selection index</returns>
        public int GetHitboxIndex() =>
            GetHitboxIndex(CurrentSelection);

        /// <summary>
        /// Gets the hitbox index from the selection index
        /// </summary>
        /// <param name="selectionIdx">Selection index from all choices</param>
        /// <returns>Hitbox index that represents the selection index</returns>
        public int GetHitboxIndex(int selectionIdx)
        {
            // Get the choice parameters
            List<int> selectionHeights = GetSelectionHeights();

            // Get the choice hitboxes
            int selectionHeight = selectionHeights[selectionIdx];
            int currentPage = (selectionHeight - 1) / Height;
            int startIndex = Height * currentPage;
            for (int i = 0; i <= Height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                if (finalIndex == selectionIdx)
                    return i;
            }
            return 0;
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
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_GRAPHICAL_SELECTION_EXCEPTION_SINGLEMULTIPLE"));
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Get the choice parameters
            List<int> selectionHeights = GetSelectionHeights();
            choiceTexts = GetChoiceParameters();
            var choiceStates = GetChoiceParametersStates(selectionHeights);

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
                            ConsoleColoring.RenderRevertForeground() +
                            ConsoleColoring.RenderRevertBackground()
                        );
                    }
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(Left + 1, optionTop + 1) +
                        new string(' ', Width)
                    );
                }
                else
                {
                    var text = choiceTexts[finalIndex];
                    var (textState, fore, back, force) = choiceStates[i];
                    if (UseColors || force)
                    {
                        buffer.Append(
                            ConsoleColoring.RenderSetConsoleColor(fore) +
                            ConsoleColoring.RenderSetConsoleColor(back, true)
                        );
                    }
                    var textBuilder = new StringBuilder(text);
                    textBuilder.Remove(0, textState.Length);
                    textBuilder.Insert(0, textState);
                    string truncated = textBuilder.ToString().Truncate(Width, Ellipsis);
                    buffer.Append(
                        CsiSequences.GenerateCsiCursorPosition(Left + 1, optionTop + 1) +
                        truncated + new string(' ', Width - ConsoleChar.EstimateCellWidth(truncated))
                    );
                }
            }

            // Render the vertical bar
            if (choiceTexts.Count > Height && Height >= 4)
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
                    slider.SliderActiveForegroundColor = SliderColor;
                    slider.SliderForegroundColor = TransformationTools.GetDarkBackground(SliderColor);
                    slider.SliderBackgroundColor = BackgroundColor;
                    buffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack("▲", finalWidth, Top, SliderColor, BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▼", finalWidth, Top + Height - 1, SliderColor, BackgroundColor)
                    );
                }
                else
                {
                    buffer.Append(
                        TextWriterWhereColor.RenderWherePlain("▲", finalWidth, Top) +
                        TextWriterWhereColor.RenderWherePlain("▼", finalWidth, Top + Height - 1)
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
                    ConsoleColoring.RenderRevertForeground() +
                    ConsoleColoring.RenderRevertBackground()
                );
            }
            return buffer.ToString();
        }

        internal List<int> GetSelectionHeights()
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_GRAPHICAL_SELECTION_EXCEPTION_SINGLEMULTIPLE"));
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice parameters
            List<int> selectionHeights = [];
            int processedHeight = 0;
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                if (Selections.Length > 1)
                    processedHeight++;
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    if (category.Groups.Length > 1)
                        processedHeight++;
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        processedHeight++;
                        selectionHeights.Add(processedHeight);
                    }
                }
            }

            // Return the parameters
            return selectionHeights;
        }

        internal List<(ChoiceHitboxType, int)> GetRelatedHeights()
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_GRAPHICAL_SELECTION_EXCEPTION_SINGLEMULTIPLE"));
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice parameters
            List<(ChoiceHitboxType, int)> relatedHeights = [];
            int relatedIdx = -1;
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                if (Selections.Length > 1)
                    relatedHeights.Add((ChoiceHitboxType.Category, relatedIdx == -1 ? 1 : relatedIdx + 2));
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    if (category.Groups.Length > 1)
                        relatedHeights.Add((ChoiceHitboxType.Group, relatedIdx == -1 ? 1 : relatedIdx + 2));
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        relatedIdx++;
                        relatedHeights.Add((ChoiceHitboxType.Choice, relatedIdx + 1));
                    }
                }
            }

            // Return the parameters
            return relatedHeights;
        }

        internal List<string> GetChoiceParameters()
        {
            if (cached)
                return choiceTexts;
            cached = true;

            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_GRAPHICAL_SELECTION_EXCEPTION_SINGLEMULTIPLE"));
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice text instances
            List<string> choiceText = [];
            int processedChoices = 0;
            int startIndexTristates = 0;
            int startIndexGroupTristates = 0;
            int relatedIdx = -1;
            var tristates = isMultiple ? SelectionInputTools.GetCategoryTristates(Selections, CurrentSelections, ref startIndexTristates) : [];
            string prefix = isMultiple ? "  [ ] " : ShowRadioButtons ? "  ( ) " : "  ";
            int AnswerTitleLeft = choices.Count > 5000 ? 0 : choices.Max(x => ConsoleChar.EstimateCellWidth(Selections.Length > 1 ? $"  {prefix}{x.ChoiceName}) " : $" {prefix}{x.ChoiceName}) "));
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                var tristate = isMultiple ? tristates[categoryIdx] : SelectionTristate.Unselected;
                if (Selections.Length > 1)
                {
                    string modifiers = isMultiple ? "[ ] " : "";
                    string finalRendered = $"{modifiers}{category.Name}";
                    choiceText.Add(finalRendered);
                }

                var groupTristates = isMultiple ? SelectionInputTools.GetGroupTristates(category.Groups, CurrentSelections, ref startIndexGroupTristates) : [];
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    var groupTristate = isMultiple ? groupTristates[groupIdx] : SelectionTristate.Unselected;
                    if (category.Groups.Length > 1)
                    {
                        string modifiers = isMultiple ? "[ ] " : "";
                        string finalRendered = $"  {modifiers}{group.Name}";
                        choiceText.Add(finalRendered);
                    }
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        relatedIdx++;
                        bool selected = processedChoices == CurrentSelection;
                        bool radioSelected = processedChoices == SelectedChoice;
                        var choice = group.Choices[i];
                        string AnswerTitle = choice.ChoiceTitle ?? "";
                        bool disabled = choice.ChoiceDisabled;

                        // Get the option
                        string selectedIndicator = isMultiple ? $"  [ ]" : ShowRadioButtons ? $"  ( )" : " ";
                        string AnswerOption = Selections.Length > 1 ? $"  {selectedIndicator} {choice.ChoiceName}) {AnswerTitle}" : $" {selectedIndicator} {choice.ChoiceName}) {AnswerTitle}";
                        if (choices.Count <= 5000 && AnswerTitleLeft < Width)
                        {
                            string renderedChoice = Selections.Length > 1 ? $"  {selectedIndicator} {choice.ChoiceName}) " : $" {selectedIndicator} {choice.ChoiceName}) ";
                            int blankRepeats = AnswerTitleLeft - ConsoleChar.EstimateCellWidth(renderedChoice);
                            AnswerOption = renderedChoice + new string(' ', blankRepeats) + $"{AnswerTitle}";
                        }

                        // Render an entry
                        choiceText.Add(AnswerOption);
                        processedChoices++;
                    }
                }
            }

            // Return the parameters
            choiceTexts = choiceText;
            return choiceText;
        }

        internal List<(string text, Color fore, Color back, bool force)> GetChoiceParametersStates(List<int> selectionHeights)
        {
            // Determine if multiple or single
            List<InputChoiceInfo> choices = SelectionInputTools.GetChoicesFromCategories(Selections);
            bool isMultiple = CurrentSelections is not null;
            if ((CurrentSelection < 0 || CurrentSelection >= choices.Count) && !isMultiple)
                throw new TerminauxException(LanguageTools.GetLocalized("T_WRITER_CYCLICWRITERS_GRAPHICAL_SELECTION_EXCEPTION_SINGLEMULTIPLE"));
            if (AltChoicePos <= 0 || AltChoicePos > choices.Count)
                AltChoicePos = choices.Count;

            // Now, get the choice parameters
            List<(string text, Color fore, Color back, bool force)> choiceText = [];
            List<(SelectionTristate tristate, ChoiceHitboxType type, bool radioSelected, bool multipleSelected, bool disabled, bool isAlt)> choiceParams = [];
            int processedChoices = 0;
            int startIndexTristates = 0;
            int startIndexGroupTristates = 0;
            int relatedIdx = -1;
            var tristates = isMultiple ? SelectionInputTools.GetCategoryTristates(Selections, CurrentSelections, ref startIndexTristates) : [];
            for (int categoryIdx = 0; categoryIdx < Selections.Length; categoryIdx++)
            {
                InputChoiceCategoryInfo? category = Selections[categoryIdx];
                var tristate = isMultiple ? tristates[categoryIdx] : SelectionTristate.Unselected;
                if (Selections.Length > 1)
                    choiceParams.Add((tristate, ChoiceHitboxType.Category, false, false, false, false));

                var groupTristates = isMultiple ? SelectionInputTools.GetGroupTristates(category.Groups, CurrentSelections, ref startIndexGroupTristates) : [];
                for (int groupIdx = 0; groupIdx < category.Groups.Length; groupIdx++)
                {
                    InputChoiceGroupInfo? group = category.Groups[groupIdx];
                    var groupTristate = isMultiple ? groupTristates[groupIdx] : SelectionTristate.Unselected;
                    if (category.Groups.Length > 1)
                        choiceParams.Add((groupTristate, ChoiceHitboxType.Group, false, false, false, false));
                    for (int i = 0; i < group.Choices.Length; i++)
                    {
                        relatedIdx++;
                        bool selected = processedChoices == CurrentSelection;
                        bool radioSelected = processedChoices == SelectedChoice;
                        var choice = group.Choices[i];
                        bool disabled = choice.ChoiceDisabled;

                        // Render an entry
                        bool isAlt = processedChoices + 1 > AltChoicePos;
                        choiceParams.Add((selected ? SelectionTristate.Selected : SelectionTristate.Unselected, ChoiceHitboxType.Choice, radioSelected, isMultiple && CurrentSelections.Contains(relatedIdx), disabled, isAlt));
                        processedChoices++;
                    }
                }
            }

            int selectionHeight = selectionHeights[CurrentSelection];
            int currentPage = (selectionHeight - 1) / Height;
            int startIndex = Height * currentPage;
            for (int i = 0; i <= Height - 1; i++)
            {
                // Populate the selection box
                int finalIndex = i + startIndex;
                if (finalIndex < selectionHeights[selectionHeights.Count - 1])
                {
                    var (tristate, type, radioSelected, multipleSelected, disabled, isAlt) = choiceParams[finalIndex];

                    // Check the type
                    if (type == ChoiceHitboxType.Category)
                    {
                        string modifiers =
                            isMultiple ?
                                tristate == SelectionTristate.Selected ? "[*] " :
                                tristate == SelectionTristate.FiftyFifty ? "[/] " : "[ ] "
                            : "";
                        choiceText.Add((modifiers, ConsoleColorData.Silver.Color, BackgroundColor, true));
                    }
                    else if (type == ChoiceHitboxType.Group)
                    {
                        string modifiers =
                            isMultiple ?
                                tristate == SelectionTristate.Selected ? "  [*] " :
                                tristate == SelectionTristate.FiftyFifty ? "  [/] " : "  [ ] " :
                            "  ";
                        choiceText.Add((modifiers, ConsoleColorData.Grey.Color, BackgroundColor, true));
                    }
                    else
                    {
                        // Get the option
                        bool selected = tristate == SelectionTristate.Selected;
                        string selectionIndicator = selected ? ">" : disabled ? "X" : " ";
                        string selectedIndicator =
                            isMultiple ? $" [{(multipleSelected ? "*" : " ")}]" :
                            ShowRadioButtons ? $" ({(radioSelected ? "*" : " ")})" : "";
                        string modifiers = Selections.Length > 1 ? $"  {selectionIndicator}{selectedIndicator}" : $" {selectionIndicator}{selectedIndicator}";

                        // Render an entry
                        var finalForeColor =
                            disabled ? DisabledForegroundColor :
                            selected ?
                                isAlt ?
                                    SwapSelectedColors ? AltSelectedBackgroundColor : AltSelectedForegroundColor :
                                    SwapSelectedColors ? SelectedBackgroundColor : SelectedForegroundColor
                                 :
                                isAlt ? AltForegroundColor : ForegroundColor;
                        var finalBackColor =
                            disabled ? DisabledBackgroundColor :
                            selected ?
                                isAlt ?
                                    SwapSelectedColors ? AltSelectedForegroundColor : AltSelectedBackgroundColor :
                                    SwapSelectedColors ? SelectedForegroundColor : SelectedBackgroundColor
                                 :
                                isAlt ? AltBackgroundColor : BackgroundColor;
                        choiceText.Add((modifiers, finalForeColor, finalBackColor, false));
                    }
                }
            }

            // Return the parameters
            return choiceText;
        }

        /// <summary>
        /// Forces refresh
        /// </summary>
        public void InvalidateCache() =>
            cached = false;

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
