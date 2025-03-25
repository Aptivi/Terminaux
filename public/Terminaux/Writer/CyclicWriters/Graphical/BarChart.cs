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
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Bar chart renderable
    /// </summary>
    // TODO: Make it actually work as a graphical renderable!
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
            return TextWriterWhereColor.RenderWhere(
                RenderBarChart(
                    elements, Width, Showcase, UseColors), Left, Top);
        }

        internal static string RenderBarChart(ChartElement[] elements, int InteriorWidth, bool showcase = false, bool useColor = true)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;
            int wholeLength = InteriorWidth - showcaseLength - (showcase ? 3 : 0);

            // Fill the bar chart with the elements first
            StringBuilder barChart = new();
            foreach (var element in elements)
            {
                var color = element.Color;
                string name = element.Name;
                bool hidden = element.Hidden;
                double value = element.Value;
                if (hidden)
                    continue;

                // Render the showcase
                if (showcase)
                {
                    int nameWidth = ConsoleChar.EstimateCellWidth(element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length));
                    int spaces = showcaseLength - (" ■ ".Length + nameWidth + 2 + $"{element.Value}".Length);
                    spaces = spaces < 0 ? 0 : spaces;
                    barChart.Append(
                        (useColor ? ColorTools.RenderSetConsoleColor(element.Color) : "") +
                        " ■ " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        element.Value +
                        new string(' ', spaces) +
                        " ┃ "
                    );
                }

                // Render the element and its value
                int length = (int)(value * wholeLength / maxValue);
                barChart.AppendLine(
                    (useColor ? ColorTools.RenderSetConsoleColor(color, true) : "") +
                    new string(' ', length) +
                    (useColor ? ColorTools.RenderResetBackground() : "") +
                    (useColor ? ColorTools.RenderResetForeground() : "")
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
