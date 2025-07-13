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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Table renderable
    /// </summary>
    public class Table : GraphicalCyclicWriter
    {
        private string[,]? rows;
        private bool header = false;
        private Color separatorColor = ThemeColorsTools.GetColor(ThemeColorType.TableSeparator);
        private Color headerColor = ThemeColorsTools.GetColor(ThemeColorType.TableHeader);
        private Color valueColor = ThemeColorsTools.GetColor(ThemeColorType.TableValue);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private List<CellOptions> settings = [];
        private BorderSettings borderSettings = new();
        private bool useColors = true;

        /// <summary>
        /// List of rows and columns
        /// </summary>
        public string[,] Rows
        {
            get => rows ?? new string[,] {{}};
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
        /// Cell settings to use
        /// </summary>
        public List<CellOptions> Settings
        {
            get => settings;
            set => settings = value;
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
        /// Renders a table
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Create a border which the table will be drawn on
            var tableBuilder = new StringBuilder();
            var tableBorderSettings = BorderSettings ?? new();

            // Check to see if we need a title
            bool needsTitle = !string.IsNullOrEmpty(Title);
            int processedHeight = Height;
            int processedTop = Top;
            if (needsTitle)
            {
                processedHeight -= 2;
                int titleWidth = ConsoleChar.EstimateCellWidth(Title);
                tableBuilder.Append(
                    TextWriterWhereColor.RenderWhereColorBack(Title, Left + (Width / 2) - (titleWidth / 2), processedTop, HeaderColor, BackgroundColor)
                );
                processedTop += 2;
            }

            // Determine the positions
            int columnsCount = Rows.GetLength(1);
            int rowsCount = Rows.GetLength(0);
            (int, int)[,] positions = new (int, int)[columnsCount, rowsCount];
            int maxCellWidth = (Width + 2) / columnsCount;
            for (int x = 0; x < columnsCount; x++)
            {
                for (int y = 0; y < rowsCount; y++)
                {
                    int finalPosX = Left + maxCellWidth * x + 1;
                    int finalPosY = processedTop + y + 1;
                    if (Header && y > 0)
                        finalPosY++;
                    positions[x, y] = (finalPosX, finalPosY);
                }
            }

            // Use the final width to create the actual table
            var border = new Border()
            {
                Left = Left,
                Top = processedTop,
                Width = maxCellWidth * columnsCount - 1,
                Height = processedHeight,
                Settings = tableBorderSettings,
                UseColors = UseColors,
            };
            if (UseColors)
            {
                border.Color = SeparatorColor;
                border.BackgroundColor = BackgroundColor;
                tableBuilder.Append(
                    border.Render() +
                    ColorTools.RenderSetConsoleColor(SeparatorColor) +
                    ColorTools.RenderSetConsoleColor(BackgroundColor, true)
                );
            }
            else
            {
                tableBuilder.Append(
                    border.Render()
                );
            }

            // Create a header separator if we need a header
            if (Header)
            {
                char begin = tableBorderSettings.BorderLeftHorizontalIntersectionChar;
                char middle = tableBorderSettings.BorderHorizontalIntersectionChar;
                char end = tableBorderSettings.BorderRightHorizontalIntersectionChar;
                int headerBorderPosX = Left;
                int headerBorderPosY = processedTop + 2;
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
            int mode = 0;
            for (int x = 1; x < columnsCount; x++)
            {
                // Try to get the positions for the separator
                var positionsSeparator = ((int, int))positions.GetValue(x, 0);

                // Build the separator
                for (int y = 0; y < processedHeight + 2; y++)
                {
                    mode =
                        y == 0 ? 1 :
                        y == processedHeight + 1 ? 2 :
                        y == 2 && Header ? 3 :
                        0;
                    char finalChar =
                        mode == 1 ? beginVertical :
                        mode == 2 ? endVertical :
                        mode == 3 ? intersect :
                        middleVertical;
                    if ((mode == 0 && tableBorderSettings.BorderVerticalIntersectionEnabled) ||
                        (mode == 1 && tableBorderSettings.BorderTopVerticalIntersectionEnabled) ||
                        (mode == 2 && tableBorderSettings.BorderBottomVerticalIntersectionEnabled) ||
                        (mode == 3 && tableBorderSettings.BorderWholeIntersectionEnabled))
                    {
                        tableBuilder.Append(
                            CsiSequences.GenerateCsiCursorPosition(positionsSeparator.Item1, y + processedTop + 1) +
                            finalChar
                        );
                    }
                }
            }

            // Write values
            for (int y = 0; y < rowsCount; y++)
            {
                for (int x = 0; x < columnsCount; x++)
                {
                    // Get the initial values
                    var positionsValues = ((int, int))positions.GetValue(x, y);
                    if (positionsValues.Item2 > processedHeight + processedTop)
                        break;
                    string text = (string)Rows.GetValue(y, x) ?? "";
                    Color finalColor = y == 0 && Header ? HeaderColor : ValueColor;
                    Color finalBackgroundColor = BackgroundColor;

                    // Process them according to both the cell width and the cell options
                    string spaces = new(' ', maxCellWidth - 1);
                    text = text.Truncate(maxCellWidth - 1);
                    var alignment = TextAlignment.Left;
                    if (Settings is not null && Settings.Count > 0)
                    {
                        var options = Settings.FirstOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y);
                        if (options is not null)
                        {
                            if (options.ColoredCell)
                            {
                                finalColor = options.CellColor;
                                finalBackgroundColor = options.CellBackgroundColor;
                            }
                            alignment = options.TextSettings.Alignment;
                        }
                    }

                    // Adjust the X position according to the alignment
                    int alignmentPosX = TextWriterTools.DetermineTextAlignment(text, maxCellWidth - 1, alignment, positionsValues.Item1);

                    // Write them
                    if (UseColors)
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWhereColorBack(spaces, positionsValues.Item1, positionsValues.Item2, finalColor, finalBackgroundColor) +
                            TextWriterWhereColor.RenderWhereColorBack(text, alignmentPosX, positionsValues.Item2, finalColor, finalBackgroundColor)
                        );
                    }
                    else
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWherePlain(spaces, positionsValues.Item1, positionsValues.Item2) +
                            TextWriterWhereColor.RenderWherePlain(text, alignmentPosX, positionsValues.Item2)
                        );
                    }
                }
            }

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
