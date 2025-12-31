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

using System.Text;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// ListEntry renderable
    /// </summary>
    public class ListEntry : IStaticRenderable
    {
        private Color keyColor = ConsoleColors.Yellow;
        private Color valueColor = ConsoleColors.Olive;
        private bool useColors = true;

        /// <summary>
        /// List key name
        /// </summary>
        public string Entry { get; set; } = "";

        /// <summary>
        /// List value as a string
        /// </summary>
        public string Value { get; set; } = "";

        /// <summary>
        /// List key color
        /// </summary>
        public Color KeyColor
        {
            get => keyColor;
            set => keyColor = value;
        }

        /// <summary>
        /// List value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set => valueColor = value;
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
        /// Whether to enable the list entry indicator
        /// </summary>
        public bool Indicator { get; set; } = true;

        /// <summary>
        /// Specifies a zero-based level of indentation
        /// </summary>
        public int Indentation { get; set; }

        /// <summary>
        /// Renders a list entry
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render() =>
            RenderListEntry(Entry, Value, KeyColor, ValueColor, Indentation, UseColors, Indicator);

        internal static string RenderListEntry(string entry, string value, Color ListKeyColor, Color ListValueColor, int indent = 0, bool useColor = true, bool writeIndicator = true)
        {
            // First, get the spaces count to indent
            if (indent < 0)
                indent = 0;
            string spaces = new(' ', indent * 2);

            // Then, write the list entry
            var listBuilder = new StringBuilder();
            listBuilder.Append(
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ListKeyColor) : "")}" +
                $"{spaces}{(writeIndicator ? "- " : "")}{entry}: " +
                $"{(useColor ? ColorTools.RenderSetConsoleColor(ListValueColor) : "")}" +
                value +
                $"{(useColor ? ColorTools.RenderRevertForeground() : "")}"
            );
            return listBuilder.ToString();
        }

        /// <summary>
        /// Makes a new instance of the list entry renderer
        /// </summary>
        public ListEntry()
        { }
    }
}
