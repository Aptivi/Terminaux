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

using System.Diagnostics;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Table and canvas cell options
    /// </summary>
    [DebuggerDisplay("Pos: {ColumnNumber},{RowNumber}, Color: {ColoredCell} [FG: {CellColor}, BG: {CellBackgroundColor}]")]
    public class CellOptions
    {

        /// <summary>
        /// The column, or row value, number
        /// </summary>
        public int ColumnNumber { get; private set; }
        /// <summary>
        /// The row number
        /// </summary>
        public int RowNumber { get; private set; }
        /// <summary>
        /// The column, or row value, index
        /// </summary>
        public int ColumnIndex =>
            ColumnNumber - 1;
        /// <summary>
        /// The row index
        /// </summary>
        public int RowIndex =>
            RowNumber - 1;
        /// <summary>
        /// Whether to color the cell
        /// </summary>
        public bool ColoredCell { get; set; }
        /// <summary>
        /// The custom cell color
        /// </summary>
        public Color CellColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
        /// <summary>
        /// The custom background cell color
        /// </summary>
        public Color CellBackgroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Background);
        /// <summary>
        /// Text settings for this cell
        /// </summary>
        public TextSettings TextSettings { get; set; } = new();

        /// <summary>
        /// Makes a new instance of the cell options class
        /// </summary>
        /// <param name="ColumnNumber">The column number</param>
        /// <param name="RowNumber">The row number</param>
        public CellOptions(int ColumnNumber, int RowNumber)
        {
            this.ColumnNumber = ColumnNumber;
            this.RowNumber = RowNumber;
        }

    }
}
