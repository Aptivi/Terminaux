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

using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Bar chart renderable
    /// </summary>
    public class BarChart : IStaticRenderable
    {
        private ChartElement[] elements = [];
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private bool showcase = false;

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
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Show the element list
        /// </summary>
        public bool Showcase
        {
            get => showcase;
            set => showcase = value;
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
        public string Render()
        {
            return TextWriterWhereColor.RenderWhere(
                RenderBarChart(
                    elements, InteriorWidth, Showcase), Left, Top);
        }

        internal static string RenderBarChart(ChartElement[] elements, int InteriorWidth, bool showcase = false)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            int nameLength = elements.Max((element) => ConsoleChar.EstimateCellWidth(element.Name));
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;
            int wholeLength = InteriorWidth - showcaseLength;
            int maxNumLength = elements.Max((element) => $" {element.Value}".Length);
            int chartLength = wholeLength - maxNumLength;
            double maxValue = elements.Max((element) => element.Value);

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
                    int nameWidth = ConsoleChar.EstimateCellWidth(name);
                    int spaces = nameLength - nameWidth;
                    barChart.Append(
                        ColorTools.RenderSetConsoleColor(color) +
                        name.Truncate(nameLength) +
                        new string(' ', spaces) +
                        ColorTools.RenderResetForeground() +
                        " ┃ "
                    );
                }

                // Render the element and its value
                int length = (int)(value * chartLength / maxValue);
                barChart.AppendLine(
                    ColorTools.RenderSetConsoleColor(color, true) +
                    new string(' ', length) +
                    ColorTools.RenderSetConsoleColor(color) +
                    ColorTools.RenderResetBackground() +
                    $" {value}" +
                    ColorTools.RenderResetForeground()
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
