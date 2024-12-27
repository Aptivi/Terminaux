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
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        public static void WriteTablePlain(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTablePlain(Rows, left, top, width, height, enableHeader, CellOptions, tableBorderSettings));
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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        public static void WriteTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null)
        {
            try
            {
                var sep = new Color(ConsoleColors.Grey);
                var header = new Color(ConsoleColors.Yellow);
                var value = new Color(ConsoleColors.Silver);
                var back = ColorTools.currentBackgroundColor;
                TextWriterRaw.WriteRaw(RenderTable(Rows, left, top, width, height, enableHeader, sep, header, value, back, CellOptions, tableBorderSettings));
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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        public static void WriteTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTable(Rows, left, top, width, height, enableHeader, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, CellOptions, tableBorderSettings));
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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        public static string RenderTablePlain(string[,] Rows, int left, int top, int width, int height, bool enableHeader, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null) =>
            RenderTable(Rows, left, top, width, height, enableHeader, ColorTools.GetGray(), ConsoleColors.Yellow, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, CellOptions, tableBorderSettings);

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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        public static string RenderTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null) =>
            RenderTable(Rows, left, top, width, height, enableHeader, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, true, CellOptions, tableBorderSettings);

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
        /// <param name="tableBorderSettings">Specifies the table border settings</param>
        internal static string RenderTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool useColor, List<CellOptions>? CellOptions = null, BorderSettings? tableBorderSettings = null)
        {
            // Create a border which the table will be drawn on
            var tableBuilder = new StringBuilder();
            tableBorderSettings ??= new();

            // Determine the positions
            int columnsCount = Rows.GetLength(1);
            int rowsCount = Rows.GetLength(0);
            (int, int)[,] positions = new (int, int)[columnsCount, rowsCount];
            int maxCellWidth = (width + 2) / columnsCount;
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

            // Use the final width to create the actual table
            if (useColor)
            {
                tableBuilder.Append(
                    BorderColor.RenderBorder(left, top, maxCellWidth * columnsCount - 1, height, tableBorderSettings, SeparatorForegroundColor, BackgroundColor) +
                    ColorTools.RenderSetConsoleColor(SeparatorForegroundColor) +
                    ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                );
            }
            else
            {
                tableBuilder.Append(
                    BorderColor.RenderBorderPlain(left, top, maxCellWidth * columnsCount - 1, height, tableBorderSettings)
                );
            }

            // Create a header separator if we need a header
            if (enableHeader)
            {
                char begin = tableBorderSettings.BorderLeftHorizontalIntersectionChar;
                char middle = tableBorderSettings.BorderHorizontalIntersectionChar;
                char end = tableBorderSettings.BorderRightHorizontalIntersectionChar;
                int headerBorderPosX = left;
                int headerBorderPosY = top + 2;
                tableBuilder.Append(
                    CsiSequences.GenerateCsiCursorPosition(headerBorderPosX + 1, headerBorderPosY + 1) +
                    begin + new string(middle, maxCellWidth * columnsCount - 1) + end
                );
            }

            // Create a row separator
            char beginVertical = tableBorderSettings.BorderTopVerticalIntersectionChar;
            char middleVertical = tableBorderSettings.BorderVerticalIntersectionChar;
            char endVertical = tableBorderSettings.BorderBottomVerticalIntersectionChar;
            char intersect = tableBorderSettings.BorderWholeIntersectionChar;
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
                        CsiSequences.GenerateCsiCursorPosition(positionsSeparator.Item1, y + top + 1) +
                        finalChar
                    );
                }
            }

            // Write values
            for (int y = 0; y < rowsCount; y++)
            {
                for (int x = 0; x < columnsCount; x++)
                {
                    // Get the initial values
                    var positionsValues = ((int, int))positions.GetValue(x, y);
                    if (positionsValues.Item2 > height + top)
                        break;
                    string text = (string)Rows.GetValue(y, x);
                    Color finalColor =
                        y == 0 && enableHeader ? HeaderForegroundColor :
                        ValueForegroundColor;
                    Color finalBackgroundColor = BackgroundColor;

                    // Process them according to both the cell width and the cell options
                    text = text.Truncate(maxCellWidth - 4);
                    text += new string(' ', maxCellWidth - ConsoleChar.EstimateCellWidth(text) - 1);
                    if (CellOptions is not null && CellOptions.Count > 0)
                    {
                        var options = CellOptions.FirstOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y);
                        if (options is not null && options.ColoredCell)
                        {
                            finalColor = options.CellColor;
                            finalBackgroundColor = options.CellBackgroundColor;
                        }
                    }

                    // Write them
                    if (useColor)
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWhereColorBack(text, positionsValues.Item1, positionsValues.Item2, finalColor, finalBackgroundColor)
                        );
                    }
                    else
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWhere(text, positionsValues.Item1, positionsValues.Item2)
                        );
                    }
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
