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
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Value showcase renderable
    /// </summary>
    public class ValueShowcase : GraphicalCyclicWriter
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
        /// Calculated length of the showcase panel
        /// </summary>
        public int Length
        {
            get
            {
                // Some variables
                var shownElements = elements.Where((ce) => !ce.Hidden).Take(Height).ToArray();
                double maxValue = shownElements.Max((element) => element.Value);

                // Get the showcase length
                StringBuilder showcase = new();
                int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value:0.##}".Length);
                nameLength = nameLength > Width ? Width : nameLength;
                return nameLength + (ShowSeparator ? 2 : 0);
            }
        }

        /// <summary>
        /// Renders a showcase panel
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Some variables
            var shownElements = elements.Where((ce) => !ce.Hidden).Take(Height).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);

            // Fill the showcase panel with the elements first
            StringBuilder showcase = new();
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value:0.##}".Length);
            nameLength = nameLength > Width ? Width : nameLength;
            for (int i = 0; i < shownElements.Length; i++)
            {
                // Get the element showcase position and write it there
                bool canShow = Height > i;
                if (!canShow)
                    break;
                Coordinate coord = new(Left, Top + i);
                var element = shownElements[i];

                // Now, write it at the selected position
                showcase.Append(
                    ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(element.Color) : "") +
                    " ■ " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                    element.Name.Truncate(nameLength - 4 - $"{maxValue:0.##}".Length) + "  " +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                    $"{element.Value:0.##}"
                );
            }

            // Show the separator
            if (ShowSeparator)
            {
                showcase.Append(UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "");
                for (int h = 0; h < Height; h++)
                {
                    Coordinate separatorCoord = new(Left + nameLength, Top + h);
                    showcase.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ▐"
                    );
                }
            }

            // Return the result
            return showcase.ToString();
        }

        /// <summary>
        /// Makes a new instance of the value showcase renderer
        /// </summary>
        public ValueShowcase()
        { }
    }
}
