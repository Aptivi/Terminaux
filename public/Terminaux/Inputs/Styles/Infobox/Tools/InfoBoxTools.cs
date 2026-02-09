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

using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Transformation;
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

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    internal static class InfoBoxTools
    {
        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensions(string[] splitFinalLines, int extraHeight = 0, int extraWidth = 0)
        {
            int windowWidth = ConsoleWrapper.WindowWidth;
            int windowHeight = ConsoleWrapper.WindowHeight;
            int maxWidth = splitFinalLines.Length > 0 ? splitFinalLines.Max(ConsoleChar.EstimateCellWidth) : 0;
            if (maxWidth < 50 && extraHeight > 0)
                maxWidth = 50;
            maxWidth += extraWidth;
            if (maxWidth > windowWidth - 4)
                maxWidth = windowWidth - 4;
            int maxHeight = splitFinalLines.Length + extraHeight;
            if (maxHeight >= windowHeight - 3)
                maxHeight = windowHeight - 4;
            int maxRenderWidth = windowWidth - 6;
            int borderX = windowWidth / 2 - maxWidth / 2 - 1;
            int borderY = windowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }
        
        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensions(int width, int height, int left, int top, int extraHeight = 0, int extraWidth = 0)
        {
            int windowWidth = ConsoleWrapper.WindowWidth;
            int windowHeight = ConsoleWrapper.WindowHeight;
            int maxWidth = width;
            if (maxWidth < 50 && extraHeight > 0)
                maxWidth = 50;
            maxWidth += extraWidth;
            if (maxWidth > windowWidth - 4)
                maxWidth = windowWidth - 4;
            int maxHeight = height + extraHeight;
            if (maxHeight >= windowHeight - 3)
                maxHeight = windowHeight - 4;
            int maxRenderWidth = windowWidth - 6;
            int borderX = left;
            int borderY = top;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
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
            int buttonsWidth = writeBinding ? ConsoleChar.EstimateCellWidth(buttons) : -2;
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, maxWidth, vars);

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
            };
            if (!string.IsNullOrEmpty(title))
                border.Title = (writeBinding && maxWidth >= buttonsWidth + 2 ? title.Truncate(maxWidth - buttonsWidth) : title).FormatString(vars);
            boxBuffer.Append(
                (useColor ? InfoBoxColor.VTSequenceForeground() : "") +
                (useColor ? ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true) : "") +
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
                Width = maxWidth,
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
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▲", left, borderY + 1, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("▼", left, borderY + maxHeight - maxHeightOffset, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(RendererTools.RenderRenderable(slider, new(left, borderY + 2)));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttonsWidth + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttonsWidth - 1, borderY, InfoBoxColor, BackgroundColor));
            return boxBuffer.ToString();
        }

        internal static void VerifyDisabled(ref int currentSelection, InputChoiceCategoryInfo[] selections, bool goingUp = false)
        {
            InputChoiceInfo[] choices = [.. SelectionInputTools.GetChoicesFromCategories(selections)];
            VerifyDisabled(ref currentSelection, choices, goingUp);
        }

        internal static void VerifyDisabled(ref int currentSelection, InputChoiceInfo[] selections, bool goingUp = false)
        {
            int minSelectionIdx = 0;
            int maxSelectionIdx = selections.Length - 1;
            if (currentSelection < minSelectionIdx || currentSelection > maxSelectionIdx)
                currentSelection = minSelectionIdx;
            if (currentSelection >= minSelectionIdx)
            {
                while (selections[currentSelection - minSelectionIdx].ChoiceDisabled)
                {
                    if (goingUp)
                    {
                        currentSelection--;
                        if (currentSelection < minSelectionIdx)
                        {
                            currentSelection = minSelectionIdx;
                            goingUp = !goingUp;
                        }
                    }
                    else
                    {
                        currentSelection++;
                        if (currentSelection > maxSelectionIdx)
                        {
                            currentSelection = maxSelectionIdx;
                            goingUp = !goingUp;
                        }
                    }
                }
            }
        }

        internal static bool UpdateSelectedIndexWithMousePos(PointerEventContext mouse, InputChoiceCategoryInfo[] selections, InfoBox infoBox, out ChoiceHitboxType hitboxType, ref int currentSelection, bool checkPos = true)
        {
            hitboxType = ChoiceHitboxType.Choice;
            if (mouse is null)
                return false;

            // Get necessary variables
            var selectionsRendered = new Selections(selections);
            var related = selectionsRendered.GetRelatedHeights();
            int selectionChoices = related.Count > 10 ? 10 : related.Count;
            var (maxWidth, _, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;
            int selectionBoxPosX = borderX + 2;
            int selectionBoxPosY = borderY + maxTextHeight + 2;
            int maxSelectionWidth = maxWidth - 4;
            return UpdateSelectedIndexWithMousePos(mouse, selections, selectionBoxPosX, selectionBoxPosY, maxSelectionWidth, selectionChoices, out hitboxType, ref currentSelection, checkPos);
        }

        internal static bool UpdateSelectedIndexWithMousePos(PointerEventContext mouse, InputChoiceCategoryInfo[] selections, int selectionBoxPosX, int selectionBoxPosY, int maxSelectionWidth, int selectionChoices, out ChoiceHitboxType hitboxType, ref int currentSelection, bool checkPos = true)
        {
            hitboxType = ChoiceHitboxType.Choice;
            if (mouse is null)
                return false;

            // Make a temporary selection renderer
            var selectionsRendered = new Selections(selections)
            {
                Left = selectionBoxPosX,
                Top = selectionBoxPosY,
                Height = selectionChoices,
                Width = maxSelectionWidth,
                CurrentSelection = currentSelection
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
            if (checkPos)
                VerifyDisabled(ref currentSelection, selections);
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

        internal static bool IsMouseWithinText(InfoBox infoBox, PointerEventContext mouse)
        {
            var (maxWidth, _, _, borderX, borderY, maxTextHeight, _) = infoBox.Dimensions;

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (borderX + 1, borderY + 1), (borderX + maxWidth, borderY + maxTextHeight));
        }

        internal static void GoUp(ref int currIdx, int level = 1)
        {
            if (level < 1)
                level = 1;
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        internal static void GoDown(ref int currIdx, InfoBox infoBox, int level = 1)
        {
            var (_, _, _, _, _, maxTextHeight, linesLength) = infoBox.Dimensions;
            if (level < 1)
                level = 1;
            currIdx += level;
            if (currIdx > linesLength - maxTextHeight)
                currIdx = linesLength - maxTextHeight;
            if (currIdx < 0)
                currIdx = 0;
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
