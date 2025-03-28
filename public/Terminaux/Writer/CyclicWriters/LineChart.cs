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

using System;
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Line chart renderable
    /// </summary>
    public class LineChart : IStaticRenderable
    {
        private ChartElement[] elements = [];
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool showcase = false;
        private bool run = false;
        private bool useColors = true;

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
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
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
        public string Render()
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            var shownElementHeights = shownElements.Select((ce) => (ce, (int)(ce.Value * InteriorHeight / maxValue))).ToArray();
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double lineWidth = (double)(InteriorWidth - (showcaseLength + 3)) / shownElements.Length / 2;
            int median = (int)shownElements.Average((element) => element.Value);
            int medianPosition = (int)(median * InteriorHeight / maxValue);
            int inverseMedianPosition = InteriorHeight - medianPosition;

            // Fill the line chart with the showcase first
            StringBuilder lineChart = new();
            if (Showcase)
            {
                for (int i = 0; i < shownElements.Length; i++)
                {
                    // Get the element showcase position and write it there
                    bool canShow = InteriorHeight > i;
                    if (!canShow)
                        break;
                    Coordinate coord = new(Left, Top + i);
                    var element = shownElements[i];

                    // Now, write it at the selected position
                    lineChart.Append(
                        ConsolePositioning.RenderChangePosition(coord.X, coord.Y) +
                        (UseColors ? ColorTools.RenderSetConsoleColor(element.Color) : "") +
                        " ■ " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        element.Value
                    );
                }

                // Show the separator
                for (int h = 0; h < InteriorHeight; h++)
                {
                    Coordinate separatorCoord = new(Left + nameLength, Top + h);
                    lineChart.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ┃ "
                    );
                }
            }

            // Show the actual chart
            for (int e = 0; e < shownElementHeights.Length; e++)
            {
                // Get the element and the height
                var elementTuple = shownElementHeights[e];
                ChartElement element = elementTuple.ce;
                int height = elementTuple.Item2;
                int inverseHeight = InteriorHeight - height;

                // Get the next element and the next height
                var nextElementTuple = e + 1 < shownElementHeights.Length ? shownElementHeights[e + 1] : (null, shownElementHeights[shownElementHeights.Length - 1].Item2);
                int nextHeight = nextElementTuple.Item2;
                int inverseNextHeight = InteriorHeight - nextHeight;

                // Build the line renderer
                int lineWidthInt = (int)lineWidth * 2;
                int startPosY = Top + (UpsideDown ? height : inverseHeight);
                int endPosY = Top + (UpsideDown ? nextHeight : inverseNextHeight);
                if (startPosY == Top + InteriorHeight)
                    startPosY--;
                if (endPosY == Top + InteriorHeight)
                    endPosY--;
                var line = new Line()
                {
                    Color = element.Color,
                    DoubleWidth = false,
                    StartPos = new(Left + showcaseLength + lineWidthInt * e, startPosY),
                    EndPos = new(Left + showcaseLength + lineWidthInt * (e + 1), endPosY),
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
                    EndPos = new(Left + showcaseLength + InteriorWidth, Top + (UpsideDown ? medianPosition : inverseMedianPosition)),
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
