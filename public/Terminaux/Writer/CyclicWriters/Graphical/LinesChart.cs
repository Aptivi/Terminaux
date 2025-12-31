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
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Lines chart renderable
    /// </summary>
    public class LinesChart : GraphicalCyclicWriter
    {
        private (string, ChartElement[])[] elements = [];
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
        public (string, ChartElement[])[] Elements
        {
            get => elements;
            set => elements = value;
        }

        /// <summary>
        /// Whether to render the lines upside down or not
        /// </summary>
        public bool UpsideDown { get; set; }

        /// <summary>
        /// Renders a lines chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Populate a list of names as elements for showcase
            var namesAsElements = elements.Select((tuple) => new ChartElement()
            {
                Name = tuple.Item1,
                Hidden = tuple.Item2.Any((ce) => ce.Hidden),
                Value = tuple.Item2.Max((ce) => ce.Value),
                Color = tuple.Item2[0].Color,
            }).ToArray();

            // Showcase variables
            var showcase = new ValueShowcase()
            {
                Left = Left,
                Top = Top,
                Width = Width / 4,
                Height = Height,
                UseColors = UseColors,
                Elements = namesAsElements,
            };
            int showcaseLength = 0;

            // Fill the lines chart with the elements first
            StringBuilder linesChart = new();
            if (Showcase)
            {
                showcaseLength = showcase.Length;
                linesChart.Append(showcase.Render());
            }

            // Some variables
            var shownElements = namesAsElements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            double lineWidth = (double)(Width - (showcaseLength + 3)) / shownElements.Length / 2;

            // Show the actual chart
            foreach (var element in elements)
            {
                // Get the sub-element heights for lines charts
                var subElements = element.Item2;
                var shownElementHeights = subElements.Select((ce) => (ce, (int)(ce.Value * Height / maxValue))).ToArray();

                // For each sub-element, make appropriate filled area
                for (int e = 0; e < shownElementHeights.Length; e++)
                {
                    // Get the element and the height
                    var elementTuple = shownElementHeights[e];
                    ChartElement ce = elementTuple.ce;
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
                        Color = element.Item2[0].Color,
                        DoubleWidth = false,
                        StartPos = new(Left + showcaseLength + (lineWidthInt * e), startPosY),
                        EndPos = new(Left + showcaseLength + (lineWidthInt * (e + 1)), endPosY),
                    };
                    linesChart.Append(line.Render());
                }
            }
            
            // Return the result
            return linesChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the lines chart renderer
        /// </summary>
        public LinesChart()
        { }
    }
}
