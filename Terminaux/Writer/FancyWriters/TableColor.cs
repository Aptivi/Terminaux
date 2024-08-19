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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Checks;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Table writer with color support
    /// </summary>
    public static class TableColor
    {
        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTablePlain(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTablePlain(Rows, left, top, width, height, enableHeader, CellOptions));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null)
        {
            try
            {
                var sep = new Color(ConsoleColors.Grey);
                var header = new Color(ConsoleColors.White);
                var value = new Color(ConsoleColors.Silver);
                var back = ColorTools.currentBackgroundColor;
                TextWriterRaw.WriteRaw(RenderTable(Rows, left, top, width, height, enableHeader, sep, header, value, back, CellOptions));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, List<CellOptions>? CellOptions = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTable(Rows, left, top, width, height, enableHeader, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, CellOptions));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static string RenderTablePlain(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null) =>
            RenderTable(Rows, left, top, width, height, enableHeader, ColorTools.GetGray(), ColorTools.GetGray(), ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, CellOptions);

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static string RenderTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, List<CellOptions>? CellOptions = null) =>
            RenderTable(Rows, left, top, width, height, enableHeader, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, true, CellOptions);

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool useColor, List<CellOptions>? CellOptions = null)
        {
            // Create a border which the table will be drawn on
            var tableBuilder = new StringBuilder();
            tableBuilder.Append(
                BorderColor.RenderBorder(left, top, width, height, SeparatorForegroundColor, BackgroundColor) +
                ColorTools.RenderSetConsoleColor(SeparatorForegroundColor) +
                ColorTools.RenderSetConsoleColor(BackgroundColor, true)
            );

            // Determine the positions
            int columnsCount = Rows.GetLength(1);
            int rowsCount = Rows.GetLength(0);
            (int, int)[,] positions = new (int, int)[columnsCount, rowsCount];
            int maxCellWidth = width / columnsCount;
            for (int x = 0; x < columnsCount; x++)
            {
                for (int y = 0; y < rowsCount; y++)
                {
                    int finalPosX = left + maxCellWidth * x + 1;
                    int finalPosY = top + y + 1;
                    if (enableHeader && y > 0)
                        finalPosY++;
                    positions[x, y] = (finalPosX, finalPosY);
                }
            }

            // TODO: Make them customizable
            // Create a header separator if we need a header
            if (enableHeader)
            {
                char begin = '╠';
                char middle = '═';
                char end = '╣';
                int headerBorderPosX = left;
                int headerBorderPosY = top + 2;
                tableBuilder.Append(
                    CsiSequences.GenerateCsiCursorPosition(headerBorderPosX + 1, headerBorderPosY + 1) +
                    begin + new string(middle, width) + end
                );
            }

            // Create a row separator
            char beginVertical = '╦';
            char middleVertical = '║';
            char endVertical = '╩';
            char intersect = '╬';
            for (int x = 1; x < columnsCount; x++)
            {
                // Try to get the positions for the separator
                var positionsSeparator = ((int, int))positions.GetValue(x, 0);

                // Build the separator
                for (int y = 0; y < height + 2; y++)
                {
                    char finalChar =
                        y == 0 ? beginVertical :
                        y == height + 1 ? endVertical :
                        y == 2 && enableHeader ? intersect :
                        middleVertical;
                    tableBuilder.Append(
                        CsiSequences.GenerateCsiCursorPosition(positionsSeparator.Item1 + 1, y + top + 1) +
                        finalChar
                    );
                }
            }

            // Final results are here
            return tableBuilder.ToString();
        }

        static TableColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
