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

using System.Linq;
using System.Text;
using Colorimetry.Data;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Value showcase renderable (horizontal)
    /// </summary>
    public class ValueShowcaseHorizontal : SimpleCyclicWriter
    {
        private ChartElement[] elements = [];
        private bool useColors = true;

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Chart elements
        /// </summary>
        public ChartElement[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Shows the separator
        /// </summary>
        public bool ShowSeparator { get; set; } = true;

        /// <summary>
        /// Maximum width of the showcase (0 to automatically determine based on content)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Maximum height of the showcase (0 to automatically determine based on content)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Whether to color the values or not
        /// </summary>
        public bool ColorValues { get; set; }

        /// <summary>
        /// Renders a showcase panel
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Check the height
            if (Height == 0)
                return "";

            // Render the showcase elements
            var showcase = new StringBuilder();
            int maxElementLength = Width / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).OrderByDescending((ce) => ce.Value).ToArray();
            int totalWidth = 0;
            int processedHeight = 0;
            for (int i = 0; i < shownElements.Length; i++)
            {
                var element = elements[i];
                var color = element.Color;
                double value = element.Value;
                string name = element.Name.Truncate(maxElementLength - 3 - $"  {value}".Length);
                int width = 3 + ConsoleChar.EstimateCellWidth(name) + $"  {value}".Length;
                string spaces = new(' ', 4);
                totalWidth += width + spaces.Length;

                // If the element would overflow, make a new line
                if (totalWidth > Width)
                {
                    processedHeight++;
                    if (processedHeight >= Height)
                        break;
                    showcase.AppendLine();
                    totalWidth = width + spaces.Length;
                }

                // Render the showcase element
                showcase.Append(
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(color) : "") +
                    " ■ " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                    name + "  " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ColorValues ? element.Color : ConsoleColors.Grey) : "") +
                    value +
                    spaces
                );
            }

            // Return the result
            return showcase.ToString();
        }

        /// <summary>
        /// Makes a new instance of the horizontal value showcase renderer
        /// </summary>
        public ValueShowcaseHorizontal()
        { }
    }
}
