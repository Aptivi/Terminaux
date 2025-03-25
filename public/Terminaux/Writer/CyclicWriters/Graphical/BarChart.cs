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

using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Bar chart renderable
    /// </summary>
    public class BarChart : GraphicalCyclicWriter
    {
        private ChartElement[] elements = [];
        private bool showcase = false;
        private bool useColors = true;

        /// <summary>
        /// Show the element list
        /// </summary>
        public bool Showcase
        {
            get => showcase;
            set => showcase = value;
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
        /// Chart elements
        /// </summary>
        public ChartElement[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Renders a bar chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Some variables
            int maxNameLength = Width / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;
            int wholeLength = Width - showcaseLength;

            // Fill the bar chart with the elements first
            StringBuilder barChart = new();
            if (Showcase)
            {
                for (int i = 0; i < shownElements.Length; i++)
                {
                    // Get the element showcase position and write it there
                    bool canShow = Height > i;
                    if (!canShow)
                        break;
                    Coordinate coord = new(Left, Top + i);
                    var element = shownElements[i];

                    // Now, write it at the selected position
                    barChart.Append(
                        ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                        (UseColors ? ColorTools.RenderSetConsoleColor(element.Color) : "") +
                        " ■ " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        element.Value +
                        " ┃ "
                    );
                }
            }

            // Show the actual bar
            for (int e = 0; e < shownElements.Length; e++)
            {
                bool canShow = Height > e;
                if (!canShow)
                    break;
                var element = shownElements[e];
                var color = element.Color;
                double value = element.Value;

                // Render the element and its value
                Coordinate coord = new(Left + showcaseLength, Top + e);
                int length = (int)(value * wholeLength / maxValue);
                barChart.Append(
                    ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                    (UseColors ? ColorTools.RenderSetConsoleColor(color, true) : "") +
                    new string(' ', length) +
                    (UseColors ? ColorTools.RenderResetBackground() : "") +
                    (UseColors ? ColorTools.RenderResetForeground() : "")
                );
            }

            // Return the result
            return barChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the bar chart renderer
        /// </summary>
        public BarChart()
        { }
    }
}
