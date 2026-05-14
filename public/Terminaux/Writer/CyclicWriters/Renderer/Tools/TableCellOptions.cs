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

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// Table and canvas cell options
    /// </summary>
    [DebuggerDisplay("Text: {Value}, Color: {ColoredCell} [FG: {CellColor}, BG: {CellBackgroundColor}]")]
    public class TableCellOptions : CellOptions
    {
        /// <summary>
        /// Value to display in this cell
        /// </summary>
        public string Value { get; set; }
        
        /// <summary>
        /// Makes a new instance of the cell options class
        /// </summary>
        /// <param name="value">Cell value</param>
        public TableCellOptions(string value) :
            base(0, 0)
        {
            Value = value;
        }
    }
}
