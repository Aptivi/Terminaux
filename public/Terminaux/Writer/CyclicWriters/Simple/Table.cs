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

using System.Collections.Generic;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical.Rulers;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Table renderable
    /// </summary>
    public class Table : SimpleCyclicWriter
    {
        private TableCellOptions[,]? rows;
        private bool header = false;
        private Color separatorColor = ThemeColorsTools.GetColor(ThemeColorType.TableSeparator);
        private Color headerColor = ThemeColorsTools.GetColor(ThemeColorType.TableHeader);
        private Color valueColor = ThemeColorsTools.GetColor(ThemeColorType.TableValue);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private BorderSettings borderSettings = new();
        private bool useColors = true;

        /// <summary>
        /// List of rows and columns
        /// </summary>
        public TableCellOptions[,] Rows
        {
            get => rows ?? new TableCellOptions[,] {{}};
            set => rows = value;
        }

        /// <summary>
        /// Whether to enable table headers
        /// </summary>
        public bool Header
        {
            get => header;
            set => header = value;
        }

        /// <summary>
        /// Table separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set => separatorColor = value;
        }

        /// <summary>
        /// Table header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set => headerColor = value;
        }

        /// <summary>
        /// Table value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set => valueColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Border settings to use
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
        }

        /// <summary>
        /// Title of the table
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Maximum width of the table (0 to automatically determine based on content)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Maximum height of the table (0 to automatically determine based on content)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Renders a table
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Create a border which the table will be drawn on
            var tableBuilder = new StringBuilder();
            var tableBorderSettings = BorderSettings ?? new();

            // Get widths of columns according to width
            int columnsCount = Rows.GetLength(1);
            int initialRowsCount = Rows.GetLength(0);
            int rowsCount = Height > 0 ? Height : initialRowsCount;
            int[] columnWidths = new int[columnsCount];
            if (Width <= 0)
            {
                // Automatically fit based on content
                for (int c = 0; c < columnsCount; c++)
                {
                    int maximumCellWidth = 0;
                    for (int r = 0; r < rowsCount; r++)
                    {
                        int valueWidth = r > initialRowsCount ? 0 : ConsoleChar.EstimateCellWidth(Rows[r, c].Value);
                        if (valueWidth > maximumCellWidth)
                            maximumCellWidth = valueWidth;
                    }
                    columnWidths[c] = maximumCellWidth;
                }
            }
            else
            {
                // Determine the width of columns based on given width
                int maximumCellWidth = (Width - 2) / columnsCount;
                if (maximumCellWidth < 0)
                    maximumCellWidth = 0;
                for (int c = 0; c < columnsCount; c++)
                    columnWidths[c] = maximumCellWidth;
            }

            // Get the total width
            int totalWidth = 0;
            for (int c = 0; c < columnsCount; c++)
            {
                int columnWidth = columnWidths[c];
                totalWidth += columnWidth;
                if (c < columnsCount - 1 && columnWidth > 0)
                    totalWidth++;
            }
            if (totalWidth <= 0)
                return "";

            // Check to see if we need a title
            if (!string.IsNullOrEmpty(Title))
            {
                int alignment = TextWriterTools.DetermineTextAlignment(Title, totalWidth, TextAlignment.Middle);
                int titleWidth = ConsoleChar.EstimateCellWidth(Title);
                string finalTitle = Title.PadLeft(alignment + titleWidth);
                if (UseColors)
                {
                    tableBuilder.Append(
                        ConsoleColoring.RenderSetConsoleColor(HeaderColor) +
                        ConsoleColoring.RenderSetConsoleColor(BackgroundColor, true)
                    );
                }
                tableBuilder.Append(finalTitle + "\n");
            }

            // Create a table top edge
            if (UseColors)
            {
                tableBuilder.Append(
                    ConsoleColoring.RenderSetConsoleColor(SeparatorColor)
                );
            }
            tableBuilder.Append(
                tableBorderSettings.BorderUpperLeftCornerEnabled ? tableBorderSettings.BorderUpperLeftCornerChar : " "
            );
            for (int c = 0; c < columnsCount; c++)
            {
                int columnWidth = columnWidths[c];
                if (columnWidth > 0)
                {
                    tableBuilder.Append(
                        new string(tableBorderSettings.BorderUpperFrameEnabled ? tableBorderSettings.BorderUpperFrameChar : ' ', columnWidth) +
                        (tableBorderSettings.BorderTopVerticalIntersectionEnabled ? tableBorderSettings.BorderTopVerticalIntersectionChar : ' ')
                    );
                }
            }
            tableBuilder.Append(
                (tableBorderSettings.BorderUpperRightCornerEnabled ? tableBorderSettings.BorderUpperRightCornerChar : " ") +
                "\n"
            );

            // Create an actual table
            for (int y = 0; y < rowsCount; y++)
            {
                bool isHeader = y == 0 && Header;

                // Render left frame
                if (UseColors)
                {
                    tableBuilder.Append(
                        ConsoleColoring.RenderSetConsoleColor(SeparatorColor)
                    );
                }
                tableBuilder.Append(
                    tableBorderSettings.BorderLeftFrameEnabled ? tableBorderSettings.BorderLeftFrameChar : " "
                );

                // Render columns
                for (int x = 0; x < columnsCount; x++)
                {
                    // Check to see if we have this row
                    int columnWidth = columnWidths[x];
                    if (columnWidth <= 0)
                        continue;
                    Color finalColor = isHeader ? HeaderColor : ValueColor;
                    Color finalBackgroundColor = BackgroundColor;
                    string finalValue = new(' ', columnWidth);
                    if (y < initialRowsCount)
                    {
                        // Get the initial values
                        var rowOption = Rows[y, x];
                        rowOption.RowNumber = y;
                        rowOption.ColumnNumber = x;
                        string text = (rowOption.Value ?? "").Truncate(columnWidth);

                        // Process them according to both the cell width and the cell options
                        var rowAlignment = rowOption.TextSettings.Alignment;
                        int alignment = TextWriterTools.DetermineTextAlignment(text, columnWidth, rowAlignment);
                        int contentWidth = ConsoleChar.EstimateCellWidth(text);
                        finalValue =
                            rowAlignment == TextAlignment.Right ? text.PadLeft(columnWidth) :
                            rowAlignment == TextAlignment.Middle ? text.PadLeft(alignment + contentWidth) :
                            text.PadRight(columnWidth);
                        if (rowOption.ColoredCell)
                        {
                            finalColor = rowOption.CellColor;
                            finalBackgroundColor = rowOption.CellBackgroundColor;
                        }
                    }

                    // Write them
                    if (UseColors)
                    {
                        tableBuilder.Append(
                            ConsoleColoring.RenderSetConsoleColor(finalColor) +
                            ConsoleColoring.RenderSetConsoleColor(finalBackgroundColor, true)
                        );
                    }
                    tableBuilder.Append(finalValue);
                    if (UseColors)
                    {
                        tableBuilder.Append(
                            ConsoleColoring.RenderSetConsoleColor(SeparatorColor)
                        );
                    }
                    tableBuilder.Append(
                        x == columnsCount ?
                        (tableBorderSettings.BorderRightFrameEnabled ? tableBorderSettings.BorderRightFrameChar : " ") :
                        (tableBorderSettings.BorderVerticalIntersectionEnabled ? tableBorderSettings.BorderVerticalIntersectionChar : " ")
                    );
                }
                tableBuilder.AppendLine();

                // Check to see if we're done from the headers
                if (isHeader)
                {
                    if (UseColors)
                    {
                        tableBuilder.Append(
                            ConsoleColoring.RenderSetConsoleColor(SeparatorColor)
                        );
                    }
                    tableBuilder.Append(
                        tableBorderSettings.BorderLeftHorizontalIntersectionEnabled ? tableBorderSettings.BorderLeftHorizontalIntersectionChar : " "
                    );
                    for (int c = 0; c < columnsCount; c++)
                    {
                        int columnWidth = columnWidths[c];
                        if (columnWidth > 0)
                        {
                            tableBuilder.Append(
                                new string(tableBorderSettings.BorderUpperFrameEnabled ? tableBorderSettings.BorderUpperFrameChar : ' ', columnWidth) +
                                (tableBorderSettings.BorderWholeIntersectionEnabled ? tableBorderSettings.BorderWholeIntersectionChar : ' ')
                            );
                        }
                    }
                    tableBuilder.Append(
                        (tableBorderSettings.BorderRightHorizontalIntersectionEnabled ? tableBorderSettings.BorderRightHorizontalIntersectionChar : " ") +
                        "\n"
                    );
                }
            }

            // Create a table bottom edge
            if (UseColors)
            {
                tableBuilder.Append(
                    ConsoleColoring.RenderSetConsoleColor(SeparatorColor)
                );
            }
            tableBuilder.Append(
                tableBorderSettings.BorderLowerLeftCornerEnabled ? tableBorderSettings.BorderLowerLeftCornerChar : " "
            );
            for (int c = 0; c < columnsCount; c++)
            {
                int columnWidth = columnWidths[c];
                if (columnWidth > 0)
                {
                    tableBuilder.Append(
                        new string(tableBorderSettings.BorderLowerFrameEnabled ? tableBorderSettings.BorderLowerFrameChar : ' ', columnWidth) +
                        (tableBorderSettings.BorderBottomVerticalIntersectionEnabled ? tableBorderSettings.BorderBottomVerticalIntersectionChar : ' ')
                    );
                }
            }
            tableBuilder.Append(
                tableBorderSettings.BorderLowerRightCornerEnabled ? tableBorderSettings.BorderLowerRightCornerChar : " "
            );

            // Final results are here
            return tableBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the table renderer
        /// </summary>
        public Table()
        { }
    }
}
