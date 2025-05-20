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
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Transformation;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Selections = Terminaux.Writer.CyclicWriters.Graphical.Selection;
using Textify.General;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Selection;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    internal static class InfoBoxTools
    {
        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensions(string[] splitFinalLines, int extraHeight = 0)
        {
            int maxWidth = splitFinalLines.Length > 0 ? splitFinalLines.Max(ConsoleChar.EstimateCellWidth) : 0;
            if (maxWidth < 30)
                maxWidth = 30;
            if (maxWidth > ConsoleWrapper.WindowWidth - 4)
                maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length + extraHeight;
            if (extraHeight > 0)
                maxHeight++;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int selectionBoxPosX, int selectionBoxPosY, int leftPos, int maxSelectionWidth, int left, int selectionReservedHeight) GetDimensions(InputChoiceCategoryInfo[] selections, string[] splitFinalLines)
        {
            var selectionsRendered = new Selections(selections)
            {
                Width = 42,
            };
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(selections)];
            var (choiceText, _) = selectionsRendered.GetChoiceParameters();
            int selectionChoices = choiceText.Count > 10 ? 10 : choiceText.Count;
            int selectionReservedHeight = 2 + selectionChoices;
            (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) = GetDimensions(splitFinalLines, selectionReservedHeight);

            // Fill in some selection properties
            int selectionBoxPosX = borderX + 2;
            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 2;
            int leftPos = selectionBoxPosX + 1;
            int maxSelectionWidth = choices.Max((ici) => ConsoleChar.EstimateCellWidth($"   {ici.ChoiceName})  {ici.ChoiceTitle}")) + 4;
            maxSelectionWidth = maxSelectionWidth > maxWidth - 4 ? maxSelectionWidth : maxWidth - 4;
            maxSelectionWidth = maxSelectionWidth >= ConsoleWrapper.WindowWidth - 8 ? ConsoleWrapper.WindowWidth - 8 : maxSelectionWidth;
            int diff = maxSelectionWidth != maxWidth - 4 ? maxSelectionWidth - maxWidth + 2 : 0;
            maxWidth = maxSelectionWidth + 4;
            borderX -= (int)Math.Round(diff / 2d);
            selectionBoxPosX -= (int)Math.Round(diff / 2d);
            int left = selectionBoxPosX + maxWidth - 3;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, selectionBoxPosX, selectionBoxPosY, leftPos, maxSelectionWidth, left, selectionReservedHeight);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int selectionBoxPosX, int selectionBoxPosY, int leftPos, int maxSelectionWidth, int left, int selectionReservedHeight) GetDimensions(InputModule[] modules, string[] splitFinalLines)
        {
            int selectionChoices = modules.Length > 10 ? 10 : modules.Length;
            int selectionReservedHeight = 2 + selectionChoices;
            (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) = GetDimensions(splitFinalLines, selectionReservedHeight);

            // Fill in some selection properties
            int selectionBoxPosX = borderX + 2;
            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 2;
            int leftPos = selectionBoxPosX + 1;
            int maxSelectionWidth = maxRenderWidth;
            int diff = maxSelectionWidth != maxWidth - 4 ? maxSelectionWidth - maxWidth + 2 : 0;
            maxWidth = maxSelectionWidth;
            maxSelectionWidth -= 4;
            borderX -= (int)Math.Round(diff / 2d);
            selectionBoxPosX -= (int)Math.Round(diff / 2d);
            int left = selectionBoxPosX + maxWidth - 3;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, selectionBoxPosX, selectionBoxPosY, leftPos, maxSelectionWidth, left, selectionReservedHeight);
        }

        internal static string RenderText(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines, maxHeightOffset);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderText(
            InputChoiceCategoryInfo[] choices, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensions(choices, splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, selectionReservedHeight, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, true, vars);
        }

        internal static string RenderText(
            InputModule[] modules, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensions(modules, splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, selectionReservedHeight, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, true, vars);
        }

        internal static string GetButtons(BorderSettings settings) =>
            $"{settings.BorderRightHorizontalIntersectionChar}K{settings.BorderLeftHorizontalIntersectionChar}" +
            $"{settings.BorderRightHorizontalIntersectionChar}X{settings.BorderLeftHorizontalIntersectionChar}";

        internal static string RenderText(
            int maxWidth, int maxHeight, int borderX, int borderY, int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string buttons = GetButtons(settings);
            int buttonsWidth = ConsoleChar.EstimateCellWidth(buttons);
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);

            // Finalize the offsets
            var titleSettings = new TextSettings()
            {
                TitleOffset = new(0, buttonsWidth + 2)
            };

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            var border = new Border()
            {
                Left = borderX,
                Top = borderY,
                Width = maxWidth,
                Height = maxHeight,
                Color = InfoBoxColor,
                TextColor = InfoBoxColor,
                BackgroundColor = BackgroundColor,
                Settings = settings,
                TextSettings = titleSettings,
                Rulers = maxHeightOffset > 0 ? [new RulerInfo(maxHeight - maxHeightOffset - 1, RulerOrientation.Horizontal)] : [],
            };
            if (!string.IsNullOrEmpty(title))
                border.Title = (writeBinding && maxWidth >= buttonsWidth + 2 ? title.Truncate(maxWidth - buttonsWidth) : title).FormatString(vars);
            boxBuffer.Append(
                (useColor ? InfoBoxColor.VTSequenceForeground : "") +
                (useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "") +
                border.Render()
            );

            // Render text inside it
            ConsoleWrapper.CursorVisible = false;
            var bounded = new BoundedText()
            {
                Left = borderX + 1,
                Top = borderY + 1,
                Line = currIdx,
                Height = maxHeight - maxHeightOffset,
                Width = maxWidth + 1,
                ForegroundColor = InfoBoxColor,
                BackgroundColor = BackgroundColor,
                Text = text.FormatString(vars)
            };
            boxBuffer.Append(
                bounded.Render()
            );
            increment = bounded.IncrementRate;

            // Render the vertical bar
            int left = maxWidth + borderX + 1;
            var slider = new Slider((int)((double)currIdx / (splitFinalLines.Length - (maxHeight - maxHeightOffset)) * splitFinalLines.Length), 0, splitFinalLines.Length)
            {
                Vertical = true,
                Height = maxHeight - maxHeightOffset - 2,
                SliderActiveForegroundColor = InfoBoxColor,
                SliderForegroundColor = TransformationTools.GetDarkBackground(InfoBoxColor),
                SliderBackgroundColor = BackgroundColor,
                SliderVerticalActiveTrackChar = settings.BorderRightFrameChar,
                SliderVerticalInactiveTrackChar = settings.BorderRightFrameChar,
            };
            if (splitFinalLines.Length > maxHeight - maxHeightOffset && drawBar)
            {
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left, 2, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left, maxHeight - maxHeightOffset + 1, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(RendererTools.RenderRenderable(slider, new(left, 3)));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttonsWidth + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttonsWidth - 1, borderY, InfoBoxColor, BackgroundColor));
            return boxBuffer.ToString();
        }

        internal static void VerifyDisabled(ref int currentSelection, InputChoiceInfo[] selections, bool goingUp = false)
        {
            if (currentSelection < 0 || currentSelection > selections.Length - 1)
                currentSelection = 0;
            if (currentSelection >= 0)
            {
                while (selections[currentSelection].ChoiceDisabled)
                {
                    if (goingUp)
                    {
                        currentSelection--;
                        if (currentSelection < 0)
                            currentSelection = selections.Length - 1;
                    }
                    else
                    {
                        currentSelection++;
                        if (currentSelection > selections.Length - 1)
                            currentSelection = 0;
                    }
                }
            }
        }

        internal static bool UpdateSelectedIndexWithMousePos(PointerEventContext mouse, InputChoiceCategoryInfo[] selections, string text, object[] vars, out ChoiceHitboxType hitboxType, ref int currentSelection)
        {
            hitboxType = ChoiceHitboxType.Choice;
            if (mouse is null)
                return false;

            // Get necessary variables
            var selectionsRendered = new Selections(selections)
            {
                CurrentSelection = currentSelection,
                Width = 42,
            };
            var (choiceText, _) = selectionsRendered.GetChoiceParameters();
            int selectionChoices = choiceText.Count > 10 ? 10 : choiceText.Count;
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, _, _, _, _, selectionBoxPosX, selectionBoxPosY, _, maxSelectionWidth, _, _) = GetDimensions(selections, splitFinalLines);

            // Determine the selection renderer
            selectionsRendered = new Selections(selections)
            {
                Left = selectionBoxPosX,
                Top = selectionBoxPosY,
                CurrentSelection = currentSelection,
                Height = selectionChoices,
                Width = maxSelectionWidth,
            };

            // Now, translate coordinates to the selected index
            if (mouse.Coordinates.x <= selectionBoxPosX || mouse.Coordinates.x > selectionBoxPosX + maxSelectionWidth)
                return false;
            if (mouse.Coordinates.y < selectionBoxPosY || mouse.Coordinates.y > selectionBoxPosY + selectionChoices - 1)
                return false;
            int listIndex = mouse.Coordinates.y - selectionBoxPosY;
            if (!selectionsRendered.CanGenerateSelectionHitbox(listIndex, out var hitbox))
                return false;

            // Depending on the hitbox parameter, we need to act accordingly
            List<InputChoiceInfo> allAnswers = SelectionInputTools.GetChoicesFromCategories(selections);
            var highlightedAnswerChoiceInfo = allAnswers[hitbox.related - 1];
            if (highlightedAnswerChoiceInfo.ChoiceDisabled && hitbox.type == ChoiceHitboxType.Choice)
                return false;
            if (!highlightedAnswerChoiceInfo.ChoiceDisabled || hitbox.type != ChoiceHitboxType.Choice)
                currentSelection = hitbox.related - 1;
            hitboxType = hitbox.type;
            return true;
        }

        internal static void ProcessSelectionRequest(int mode, int choiceNum, InputChoiceCategoryInfo[] categories, ref List<int> SelectedAnswers)
        {
            List<InputChoiceInfo> allAnswers = SelectionInputTools.GetChoicesFromCategories(categories);
            var choiceGroup = SelectionInputTools.GetCategoryGroupFrom(choiceNum, categories);
            var enabledAnswers = allAnswers.Select((ici, idx) => (ici, idx)).Where((ici) => !ici.ici.ChoiceDisabled).Select((tuple) => tuple.idx).ToArray();
            switch (mode)
            {
                case 1:
                    {
                        var category = choiceGroup.Item1;
                        var group = choiceGroup.Item2;
                        var choices = group.Choices;
                        List<int> indexes = [];
                        for (int i = 0; i < allAnswers.Count; i++)
                        {
                            var answer = allAnswers[i];
                            foreach (var choice in choices)
                            {
                                if (choice == answer && !choice.ChoiceDisabled)
                                    indexes.Add(i);
                            }
                        }

                        bool clear = DetermineClear(indexes, SelectedAnswers);
                        if (clear)
                        {
                            foreach (int index in indexes)
                                SelectedAnswers.Remove(index);
                        }
                        else
                        {
                            foreach (int index in indexes)
                            {
                                if (!SelectedAnswers.Contains(index))
                                    SelectedAnswers.Add(index);
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        var category = choiceGroup.Item1;
                        var groups = category.Groups;
                        List<int> indexes = [];
                        foreach (var group in groups)
                        {
                            var choices = group.Choices;
                            for (int i = 0; i < allAnswers.Count; i++)
                            {
                                var answer = allAnswers[i];
                                foreach (var choice in choices)
                                {
                                    if (choice == answer && !choice.ChoiceDisabled)
                                        indexes.Add(i);
                                }
                            }
                        }

                        bool clear = DetermineClear(indexes, SelectedAnswers);
                        if (clear)
                        {
                            foreach (int index in indexes)
                                SelectedAnswers.Remove(index);
                        }
                        else
                        {
                            foreach (int index in indexes)
                            {
                                if (!SelectedAnswers.Contains(index))
                                    SelectedAnswers.Add(index);
                            }
                        }
                    }
                    break;
                case 3:
                    bool unselect = SelectedAnswers.Count == enabledAnswers.Count();
                    if (unselect)
                        SelectedAnswers.Clear();
                    else if (SelectedAnswers.Count == 0)
                        SelectedAnswers.AddRange(enabledAnswers);
                    else
                    {
                        // We need to use Except here to avoid wasting CPU cycles, since we could be dealing with huge data.
                        var unselected = enabledAnswers.Except(SelectedAnswers);
                        SelectedAnswers.AddRange(unselected);
                    }
                    break;
            }
        }

        private static bool DetermineClear(List<int> indexes, List<int> selectedAnswers)
        {
            int found = 0;
            foreach (int selectedAnswer in selectedAnswers)
            {
                if (indexes.Contains(selectedAnswer))
                    found++;
            }
            return found == indexes.Count;
        }
    }
}
