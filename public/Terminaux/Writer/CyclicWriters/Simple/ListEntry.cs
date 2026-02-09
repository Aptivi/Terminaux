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
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// ListEntry renderable
    /// </summary>
    public class ListEntry : SimpleCyclicWriter
    {
        private Color keyColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry);
        private Color valueColor = ThemeColorsTools.GetColor(ThemeColorType.ListValue);
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
        public override string Render()
        {
            // First, get the spaces count to indent
            if (Indentation < 0)
                Indentation = 0;
            string spaces = new(' ', Indentation * 2);

            // Then, write the list entry
            var listBuilder = new StringBuilder();
            listBuilder.Append(
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(KeyColor) : "")}" +
                $"{spaces}{(Indicator ? "- " : "")}{Entry}: " +
                $"{(UseColors ? ConsoleColoring.RenderSetConsoleColor(ValueColor) : "")}" +
                Value +
                $"{(UseColors ? ConsoleColoring.RenderRevertForeground() : "")}"
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
