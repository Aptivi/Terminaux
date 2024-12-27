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
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

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
            if (maxHeight >= ConsoleWrapper.WindowHeight - 3)
                maxHeight = ConsoleWrapper.WindowHeight - 4;
            int maxRenderWidth = ConsoleWrapper.WindowWidth - 6;
            int borderX = ConsoleWrapper.WindowWidth / 2 - maxWidth / 2 - 1;
            int borderY = ConsoleWrapper.WindowHeight / 2 - maxHeight / 2 - 1;
            return (maxWidth, maxHeight, maxRenderWidth, borderX, borderY);
        }

        internal static (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY, int selectionBoxPosX, int selectionBoxPosY, int leftPos, int maxSelectionWidth, int left, int selectionReservedHeight) GetDimensions(InputChoiceInfo[] selections, string[] splitFinalLines)
        {
            int selectionChoices = selections.Length > 10 ? 10 : selections.Length;
            int selectionReservedHeight = 4 + selectionChoices;
            (int maxWidth, int maxHeight, int maxRenderWidth, int borderX, int borderY) = GetDimensions(splitFinalLines, selectionReservedHeight);

            // Fill in some selection properties
            int selectionBoxPosX = borderX + 2;
            int selectionBoxPosY = borderY + maxHeight - selectionReservedHeight + 3;
            int leftPos = selectionBoxPosX + 1;
            int maxSelectionWidth = selections.Max((ici) => ConsoleChar.EstimateCellWidth($"  {ici.ChoiceName})  {ici.ChoiceTitle}")) + 4;
            maxSelectionWidth = maxSelectionWidth > maxWidth - 4 ? maxSelectionWidth : maxWidth - 4;
            maxSelectionWidth = maxSelectionWidth >= ConsoleWrapper.WindowWidth - 8 ? ConsoleWrapper.WindowWidth - 8 : maxSelectionWidth;
            int diff = maxSelectionWidth != maxWidth - 4 ? maxSelectionWidth - maxWidth + 2 : 0;
            maxWidth = maxSelectionWidth + 4;
            borderX -= (int)Math.Round(diff / 2d);
            selectionBoxPosX -= (int)Math.Round(diff / 2d);
            int left = maxWidth - 2;
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
            InputChoiceInfo[] choices, string title, string text, BorderSettings settings, Color InfoBoxColor, Color BackgroundColor, bool useColor, ref int increment, int currIdx, bool drawBar, params object[] vars
        )
        {
            // Deal with the lines to actually fit text in the infobox
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY, _, _, _, _, _, selectionReservedHeight) = GetDimensions(choices, splitFinalLines);
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

            // Fill the info box with text inside it
            var boxBuffer = new StringBuilder();
            var border = new Border()
            {
                Left = borderX,
                Top = borderY,
                InteriorWidth = maxWidth,
                InteriorHeight = maxHeight,
                Color = InfoBoxColor,
                TextColor = InfoBoxColor,
                BackgroundColor = BackgroundColor,
                Settings = settings,
            };
            if (!string.IsNullOrEmpty(title))
                border.Title = (writeBinding && maxWidth >= buttonsWidth + 2 ? title.Truncate(maxWidth - buttonsWidth - 9) : title).FormatString(vars);
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
                Top = borderY,
                Line = currIdx,
                Height = maxHeight,
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
                boxBuffer.Append(ContainerTools.RenderRenderable(slider, new(left, 3)));
            }

            // Render a keybinding that points to the help page
            if (writeBinding && maxWidth >= buttonsWidth + 2)
                boxBuffer.Append(TextWriterWhereColor.RenderWhereColorBack(buttons, left - buttonsWidth - 1, borderY, InfoBoxColor, BackgroundColor));
            return boxBuffer.ToString();
        }
    }
}
