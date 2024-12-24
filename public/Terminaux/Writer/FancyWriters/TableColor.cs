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
using Terminaux.Base.Checks;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Table writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
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
            Table.RenderTable(Rows, left, top, width, height, enableHeader, ColorTools.GetGray(), ConsoleColors.Yellow, ColorTools.GetGray(), ColorTools.currentBackgroundColor, true, CellOptions, tableBorderSettings);

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
            Table.RenderTable(Rows, left, top, width, height, enableHeader, SeparatorForegroundColor, HeaderForegroundColor, ValueForegroundColor, BackgroundColor, true, CellOptions, tableBorderSettings);

        static TableColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
