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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Line chart renderable
    /// </summary>
    public class LineChart : GraphicalCyclicWriter
    {
        private ChartElement[] elements = [];
        private bool showcase = false;
        private bool run = false;
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
        /// Whether to render this chart as a run chart
        /// </summary>
        public bool RunChart
        {
            get => run;
            set => run = value;
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
        /// Whether to render the bars upside down or not
        /// </summary>
        public bool UpsideDown { get; set; }

        /// <summary>
        /// Renders a line chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Showcase variables
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double median = shownElements.Average((element) => element.Value);
            var showcase = new ValueShowcase()
            {
                Left = Left,
                Top = Top,
                Width = Width / 4,
                Height = Height,
                UseColors = UseColors,
                Elements = !RunChart ? Elements : [.. Elements.Union([new ChartElement()
                {
                    Name = "Average Run",
                    Value = median,
                    Color = ConsoleColors.Fuchsia,
                }])],
            };
            int showcaseLength = 0;

            // Fill the line chart with the elements first
            StringBuilder lineChart = new();
            if (Showcase)
            {
                showcaseLength = showcase.Length;
                lineChart.Append(showcase.Render());
            }

            // Some variables
            double maxValue = shownElements.Max((element) => element.Value);
            var shownElementHeights = shownElements.Select((ce) => (ce, (int)(ce.Value * Height / maxValue))).ToArray();
            double lineWidth = (double)(Width - (showcaseLength + 3)) / shownElements.Length / 2;
            int medianPosition = (int)(median * Height / maxValue);
            int inverseMedianPosition = Height - medianPosition;
            var elementsWithRun = shownElements;

            // Show the actual chart
            for (int e = 0; e < shownElementHeights.Length; e++)
            {
                // Get the element and the height
                var elementTuple = shownElementHeights[e];
                ChartElement element = elementTuple.ce;
                int height = elementTuple.Item2;
                int inverseHeight = Height - height;

                // Get the next element and the next height
                var nextElementTuple = e + 1 < shownElementHeights.Length ? shownElementHeights[e + 1] : (null, shownElementHeights[shownElementHeights.Length - 1].Item2);
                int nextHeight = nextElementTuple.Item2;
                int inverseNextHeight = Height - nextHeight;

                // Build the line renderer
                int lineWidthInt = (int)lineWidth * 2;
                int startPosY = Top + (UpsideDown ? height : inverseHeight);
                int endPosY = Top + (UpsideDown ? nextHeight : inverseNextHeight);
                if (startPosY == Top + Height)
                    startPosY--;
                if (endPosY == Top + Height)
                    endPosY--;
                var line = new Line()
                {
                    Color = element.Color,
                    DoubleWidth = false,
                    StartPos = new(Left + showcaseLength + (lineWidthInt * e), startPosY),
                    EndPos = new(Left + showcaseLength + (lineWidthInt * (e + 1)), endPosY),
                };
                lineChart.Append(line.Render());
            }

            // If we need to draw the run, print it
            if (run)
            {
                var runLine = new Line()
                {
                    Color = ConsoleColors.Fuchsia,
                    DoubleWidth = false,
                    StartPos = new(Left + showcaseLength, Top + (UpsideDown ? medianPosition : inverseMedianPosition)),
                    EndPos = new(Left + showcaseLength + Width, Top + (UpsideDown ? medianPosition : inverseMedianPosition)),
                };
                lineChart.Append(runLine.Render());
            }

            // Return the result
            return lineChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the line chart renderer
        /// </summary>
        public LineChart()
        { }
    }
}
