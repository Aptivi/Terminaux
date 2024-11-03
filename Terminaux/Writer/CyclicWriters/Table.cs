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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Terminaux.Writer.MiscWriters.Tools;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Table renderable
    /// </summary>
    public class Table : IStaticRenderable
    {
        private string[,]? rows;
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool header = false;
        private Color separatorColor = ColorTools.CurrentForegroundColor;
        private Color headerColor = ColorTools.CurrentForegroundColor;
        private Color valueColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private List<CellOptions> settings = [];
        private BorderSettings borderSettings = new();
        private bool customColor = false;

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Top position
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
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
        }

        /// <summary>
        /// Table separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set
            {
                separatorColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Table header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set
            {
                headerColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Table value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set
            {
                valueColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                customColor = true;
            }
        }

        /// <summary>
        /// Cell settings to use
        /// </summary>
        public List<CellOptions> Settings
        {
            get => settings;
            set
            {
                settings = value;
                customColor = true;
            }
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
        /// Renders an table text
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return RenderTable(
                Rows, Left, Top, InteriorWidth, InteriorHeight, Header, SeparatorColor, HeaderColor, ValueColor, BackgroundColor, customColor, Settings, BorderSettings);
        }

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
                    string spaces = new(' ', maxCellWidth - 1);
                    text = text.Truncate(maxCellWidth - 4);
                    var alignment = TextAlignment.Left;
                    if (CellOptions is not null && CellOptions.Count > 0)
                    {
                        var options = CellOptions.FirstOrDefault((co) => co.ColumnIndex == x && co.RowIndex == y);
                        if (options is not null && options.ColoredCell)
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
                    if (useColor)
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWhereColorBack(spaces, positionsValues.Item1, positionsValues.Item2, finalColor, finalBackgroundColor) +
                            TextWriterWhereColor.RenderWhereColorBack(text, alignmentPosX, positionsValues.Item2, finalColor, finalBackgroundColor)
                        );
                    }
                    else
                    {
                        tableBuilder.Append(
                            TextWriterWhereColor.RenderWhere(spaces, positionsValues.Item1, positionsValues.Item2) +
                            TextWriterWhereColor.RenderWhere(text, alignmentPosX, positionsValues.Item2)
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
