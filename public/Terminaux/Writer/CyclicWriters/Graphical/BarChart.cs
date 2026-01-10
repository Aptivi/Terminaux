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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
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
        /// Whether to render the bars backwards (right to left) or not (left to right)
        /// </summary>
        public bool Backwards { get; set; }

        /// <summary>
        /// Renders a bar chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Showcase variables
            var showcase = new ValueShowcase()
            {
                Left = Left,
                Top = Top,
                Width = Width / 4,
                Height = Height,
                UseColors = UseColors,
                Elements = Elements,
            };
            int showcaseLength = 0;

            // Fill the bar chart with the elements first
            StringBuilder barChart = new();
            if (Showcase)
            {
                showcaseLength = showcase.Length;
                barChart.Append(showcase.Render());
            }

            // Show the actual bar
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int wholeLength = Width - showcaseLength;
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
                int diff = Backwards ? wholeLength - length : 0;
                ConsoleLogger.Debug("Rendering bar chart: ({0} + {1}, {2}) len: {3}", coord.X, diff, coord.Y, length);
                barChart.Append(
                    ConsolePositioning.RenderChangePosition(coord.X + diff, coord.Y) +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(color, true) : "") +
                    new string(' ', length) +
                    (UseColors ? ConsoleColoring.RenderResetBackground() : "") +
                    (UseColors ? ConsoleColoring.RenderResetForeground() : "")
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
