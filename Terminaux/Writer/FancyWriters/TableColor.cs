﻿
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using Terminaux.Base;
using Terminaux.Colors;
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
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            WriteTable(Headers, Rows, Margin, new Color(ConsoleColors.DarkGray), new Color(ConsoleColors.White), new Color(ConsoleColors.Gray), new Color(ConsoleColors.Black), SeparateRows, CellOptions);

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
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, ConsoleColors SeparatorForegroundColor, ConsoleColors HeaderForegroundColor, ConsoleColors ValueForegroundColor, ConsoleColors BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            WriteTable(Headers, Rows, Margin, new Color(SeparatorForegroundColor), new Color(HeaderForegroundColor), new Color(ValueForegroundColor), new Color(BackgroundColor), SeparateRows, CellOptions);

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
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, Color SeparatorForegroundColor, Color HeaderForegroundColor, Color ValueForegroundColor, Color BackgroundColor, bool SeparateRows = true, List<CellOptions> CellOptions = null)
        {
            int ColumnCapacity = (int)Math.Round(Console.WindowWidth / (double)Headers.Length);
            var ColumnPositions = new List<int>();
            int RepeatTimes;

            // Populate the positions
            TextWriterColor.Write();
            for (int ColumnPosition = Margin; ColumnCapacity >= 0 ? ColumnPosition <= Console.WindowWidth : ColumnPosition >= Console.WindowWidth; ColumnPosition += ColumnCapacity)
            {
                if (!(ColumnPosition >= Console.WindowWidth))
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
            for (int HeaderIndex = 0; HeaderIndex <= Headers.Length - 1; HeaderIndex++)
            {
                string Header = Headers[HeaderIndex];
                int ColumnPosition = ColumnPositions[HeaderIndex];
                Header ??= "";
                TextWriterWhereColor.WriteWhere(Header.Truncate(ColumnCapacity - 3 - Margin), ColumnPosition, Console.CursorTop, false, HeaderForegroundColor, BackgroundColor);
            }
            TextWriterColor.Write();

            // Write the closing minus sign.
            RepeatTimes = Console.WindowWidth - Console.CursorLeft - Margin * 2;
            if (Margin > 0)
                TextWriterColor.Write(new string(' ', Margin), false, SeparatorForegroundColor, BackgroundColor);
            TextWriterColor.Write(new string('═', RepeatTimes), true, SeparatorForegroundColor, BackgroundColor);

            // Write the rows
            for (int RowIndex = 0; RowIndex <= Rows.GetLength(0) - 1; RowIndex++)
            {
                for (int RowValueIndex = 0; RowValueIndex <= Rows.GetLength(1) - 1; RowValueIndex++)
                {
                    var ColoredCell = false;
                    var CellColor = new Color(ConsoleColors.Gray);
                    var CellBackgroundColor = new Color(ConsoleColors.Black);
                    string RowValue = Rows[RowIndex, RowValueIndex];
                    int ColumnPosition = ColumnPositions[RowValueIndex];
                    RowValue ??= "";

                    // Get the cell options and set them as necessary
                    if (CellOptions is not null)
                    {
                        foreach (CellOptions CellOption in CellOptions)
                        {
                            if (CellOption.ColumnIndex == RowValueIndex & CellOption.RowIndex == RowIndex)
                            {
                                ColoredCell = CellOption.ColoredCell;
                                CellColor = CellOption.CellColor;
                                CellBackgroundColor = CellOption.CellBackgroundColor;
                            }
                        }
                    }

                    // Now, write the cell value
                    string FinalRowValue = RowValue.Truncate(ColumnCapacity - 3 - Margin);
                    if (ColoredCell)
                        TextWriterWhereColor.WriteWhere(FinalRowValue, ColumnPosition, Console.CursorTop, false, CellColor, CellBackgroundColor);
                    else
                        TextWriterWhereColor.WriteWhere(FinalRowValue, ColumnPosition, Console.CursorTop, false, ValueForegroundColor, BackgroundColor);
                }
                TextWriterColor.Write();

                // Separate the rows optionally
                if (SeparateRows)
                {
                    // Write the closing minus sign.
                    RepeatTimes = Console.WindowWidth - Console.CursorLeft - Margin * 2;
                    if (Margin > 0)
                        TextWriterColor.Write(new string(' ', Margin), false, SeparatorForegroundColor, BackgroundColor);
                    TextWriterColor.Write(new string('=', RepeatTimes), true, SeparatorForegroundColor, BackgroundColor);
                }
            }
        }

    }
}
