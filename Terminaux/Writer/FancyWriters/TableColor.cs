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
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTablePlain(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions>? CellOptions = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTablePlain(Headers, Rows, Margin, SeparateRows, CellOptions));
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
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Safe threshold from left</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions>? CellOptions = null)
        {
            try
            {
                var sep = new Color(ConsoleColors.Silver);
                var header = new Color(ConsoleColors.White);
                var value = new Color(ConsoleColors.DarkCyan);
                var back = ColorTools.currentBackgroundColor;
                TextWriterRaw.WriteRaw(RenderTable(Headers, Rows, Margin, sep, header, value, back, SeparateRows, CellOptions));
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
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions>? CellOptions = null)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderTable(Headers, Rows, Margin, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, SeparateRows, CellOptions));
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
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        public static string RenderTablePlain(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions>? CellOptions = null) =>
            RenderTable(Headers, Rows, Margin, ColorTools.GetGray(), ColorTools.GetGray(), ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, SeparateRows, CellOptions);

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        public static string RenderTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions>? CellOptions = null) =>
            RenderTable(Headers, Rows, Margin, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, true, SeparateRows, CellOptions);

        /// <summary>
        /// Renders a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="SeparatorForegroundColor">A separator foreground color that will be changed to.</param>
        /// <param name="HeaderForegroundColor">A header foreground color that will be changed to.</param>
        /// <param name="ValueForegroundColor">A value foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="useColor">Whether to use the colors or not</param>
        internal static string RenderTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool useColor, bool SeparateRows = true, List<CellOptions>? CellOptions = null)
        {
            try
            {
                int width = ConsoleWrapper.WindowWidth;
                var table = new StringBuilder();
                int ColumnCapacity = (int)Math.Round(width / (double)Headers.Length);
                var ColumnPositions = new List<int>();
                int RepeatTimes;
                int line = 1;

                // Populate the positions
                table.AppendLine();
                for (int ColumnPosition = Margin; ColumnCapacity >= 0 ? ColumnPosition <= width : ColumnPosition >= width; ColumnPosition += ColumnCapacity)
                {
                    if (ColumnPosition < width)
                    {
                        ColumnPositions.Add(ColumnPosition);
                        if (ColumnPositions.Count == 1)
                            ColumnPosition = 0;
                    }
                    else
                    {
                        break;
                    }
                }

                // Write the headers
                var headerBuilder = new StringBuilder();
                if (useColor)
                {
                    table.Append(
                        ColorTools.RenderSetConsoleColor(HeaderForegroundColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                for (int HeaderIndex = 0; HeaderIndex < Headers.Length; HeaderIndex++)
                {
                    string Header = Headers[HeaderIndex];
                    int ColumnPosition = ColumnPositions[HeaderIndex];
                    Header ??= "";
                    string renderedHeader = Header.Truncate(ColumnCapacity - 3 - Margin);
                    if (HeaderIndex == 0)
                        headerBuilder.Append(new string(' ', ColumnPosition));
                    headerBuilder.Append(renderedHeader);
                    if (HeaderIndex < Headers.Length - 1)
                        headerBuilder.Append(new string(' ', ColumnPositions[HeaderIndex + 1] - headerBuilder.Length));
                }
                table.AppendLine(headerBuilder.ToString());
                line++;

                // Write the closing minus sign.
                RepeatTimes = width - Margin * 2;
                if (useColor)
                {
                    table.Append(
                        ColorTools.RenderSetConsoleColor(SeparatorForegroundColor) +
                        ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                if (Margin > 0)
                    table.Append(new string(' ', Margin));
                table.AppendLine(new string('═', RepeatTimes));
                line++;

                // Write the rows
                int rowValues =  Rows.GetLength(0);
                for (int rowIndex = 0; rowIndex < rowValues; rowIndex++)
                {
                    var rowBuilder = new StringBuilder();
                    int columnValues = Rows.GetLength(1);
                    for (int columnIndex = 0; columnIndex < columnValues; columnIndex++)
                    {
                        var columnBuilder = new StringBuilder();
                        var ColoredCell = false;
                        var CellColor = ColorTools.currentForegroundColor;
                        var CellBackgroundColor = ColorTools.currentBackgroundColor;
                        string RowValue = Rows[rowIndex, columnIndex];
                        int ColumnPosition = ColumnPositions[columnIndex];
                        RowValue ??= "";

                        // Get the cell options and set them as necessary
                        if (CellOptions is not null)
                        {
                            foreach (CellOptions CellOption in CellOptions)
                            {
                                if (CellOption.ColumnIndex == columnIndex & CellOption.RowIndex == rowIndex)
                                {
                                    ColoredCell = CellOption.ColoredCell;
                                    CellColor = CellOption.CellColor;
                                    CellBackgroundColor = CellOption.CellBackgroundColor;
                                }
                            }
                        }

                        // Now, write the cell value
                        string FinalRowValue = RowValue.Truncate(ColumnCapacity - 3 - Margin);
                        if (useColor)
                        {
                            if (ColoredCell)
                                rowBuilder.Append(
                                    ColorTools.RenderSetConsoleColor(CellColor) +
                                    CellBackgroundColor.VTSequenceBackground
                                );
                            else
                                rowBuilder.Append(
                                    ColorTools.RenderSetConsoleColor(ValueForegroundColor) +
                                    ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                                );
                        }
                        if (columnIndex == 0)
                            columnBuilder.Append(new string(' ', ColumnPosition));
                        columnBuilder.Append(FinalRowValue);
                        if (columnIndex < Headers.Length - 1)
                            columnBuilder.Append(new string(' ', ColumnCapacity - columnBuilder.Length));
                        rowBuilder.Append(columnBuilder.ToString());
                    }
                    table.AppendLine(rowBuilder.ToString());
                    line++;

                    // Separate the rows optionally
                    if (SeparateRows)
                    {
                        // Write the closing minus sign.
                        if (useColor)
                        {
                            table.Append(
                                ColorTools.RenderSetConsoleColor(SeparatorForegroundColor) +
                                ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                            );
                        }
                        RepeatTimes = width - Margin * 2;
                        if (Margin > 0)
                            table.Append(new string(' ', Margin));
                        table.AppendLine(new string('═', RepeatTimes));
                        line++;
                    }
                }

                // Write the resulting buffer
                if (useColor)
                {
                    table.Append(
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentForegroundColor) +
                        ColorTools.RenderSetConsoleColor(ColorTools.CurrentBackgroundColor, true)
                    );
                }
                return table.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
            return "";
        }

        static TableColor()
        {
            ConsoleChecker.CheckConsole();
        }
    }
}
