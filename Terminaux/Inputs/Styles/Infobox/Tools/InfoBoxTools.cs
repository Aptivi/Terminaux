//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Textify.General;

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    internal static class InfoBoxTools
    {
        internal static string[] GetFinalLines(string text, params object[] vars)
        {
            // Deal with the lines to actually fit text in the infobox
            string finalInfoRendered = TextTools.FormatString(text, vars);
            string[] splitLines = finalInfoRendered.ToString().SplitNewLines();
            List<string> splitFinalLines = [];
            foreach (var line in splitLines)
            {
                var lineSentences = ConsoleMisc.GetWrappedSentencesByWords(line, ConsoleWrapper.WindowWidth - 4);
                foreach (var lineSentence in lineSentences)
                    splitFinalLines.Add(lineSentence);
            }

            // Trim the new lines until we reach a full line
            for (int i = splitFinalLines.Count - 1; i >= 0; i--)
            {
                string line = splitFinalLines[i];
                if (!string.IsNullOrWhiteSpace(line))
                    break;
                splitFinalLines.RemoveAt(i);
            }
            return [.. splitFinalLines];
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensions(string[] splitFinalLines)
        {
            int maxWidth = splitFinalLines.Max(ConsoleChar.EstimateCellWidth);
            if (maxWidth >= ConsoleWrapper.WindowWidth)
                maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) GetDimensionsInput(string[] splitFinalLines)
        {
            int maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length + 5;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int selectionBoxPosX, int selectionBoxPosY, int leftPos, int maxSelectionWidth, int left, int selectionReservedHeight) GetDimensionsSelection(InputChoiceInfo[] selections, string[] splitFinalLines)
        {
            int selectionChoices = selections.Length > 10 ? 10 : selections.Length;
            int selectionReservedHeight = 4 + selectionChoices;
            int maxWidth = ConsoleWrapper.WindowWidth - 4;
            int maxHeight = splitFinalLines.Length + selectionReservedHeight;
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;

            // Fill in some selection properties
            int selectionBoxPosX = borderX + 4;
            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 3;
            int leftPos = selectionBoxPosX + 1;
            int maxSelectionWidth = maxWidth - selectionBoxPosX * 2 + 2;
            int left = maxWidth - 2;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY, selectionBoxPosX, selectionBoxPosY, leftPos, maxSelectionWidth, left, selectionReservedHeight);
        }

        internal static string RenderText(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensions(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderTextInput(
            int maxHeightOffset, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, bool writeBinding, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = GetDimensionsInput(splitFinalLines);
            return RenderText(maxWidth, maxHeight, borderX, borderY, maxHeightOffset, title, text, settings, InfoBoxColor, BackgroundColor, useColor, ref increment, currIdx, drawBar, writeBinding, vars);
        }

        internal static string RenderTextSelection(
            InputChoiceInfo[] choices, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensionsSelection(choices, splitFinalLines);
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
            string[] splitFinalLines = GetFinalLines(text, vars);

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            string border =
                !string.IsNullOrEmpty(title) ?
                BorderColor.RenderBorderPlain(writeBinding && maxWidth >= buttonsWidth + 2 ? title.Truncate(maxWidth - buttonsWidth - 9) : title, borderX, borderY, maxWidth, maxHeight, settings) :
                BorderColor.RenderBorderPlain(borderX, borderY, maxWidth, maxHeight, settings);
            boxBuffer.Append(
                $"{(useColor ? InfoBoxColor.VTSequenceForeground : "")}" +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(BackgroundColor, true) : "")}" +
                $"{border}"
            );

            // Render text inside it
            ConsoleWrapper.CursorVisible = false;
            int linesMade = 0;
            for (int i = currIdx; i < splitFinalLines.Length; i++)
            {
                var line = splitFinalLines[i];
                if (linesMade % (maxHeight - maxHeightOffset) == 0 && linesMade > 0)
                {
                    // Reached the end of the box. Bail.
                    increment = linesMade;
                    break;
                }
                boxBuffer.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(borderX + 2, borderY + 1 + linesMade % maxHeight + 1)}" +
                    $"{line}"
                );
                linesMade++;
            }

            // Render the vertical bar
            int left = maxWidth + borderX + 1;
            if (splitFinalLines.Length > maxHeight - maxHeightOffset && drawBar)
            {
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↑", left, 2, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack("↓", left, maxHeight - maxHeightOffset + 1, InfoBoxColor, BackgroundColor));
                boxBuffer.Append(SliderVerticalColor.RenderVerticalSlider((int)((double)currIdx / (splitFinalLines.Length - (maxHeight - maxHeightOffset)) * splitFinalLines.Length), splitFinalLines.Length, left - 1, 2, maxHeight - maxHeightOffset - 2, InfoBoxColor, BackgroundColor, BackgroundColor, false));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttonsWidth + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttonsWidth - 1, borderY, InfoBoxColor, BackgroundColor));
            return boxBuffer.ToString();
        }
    }
}
