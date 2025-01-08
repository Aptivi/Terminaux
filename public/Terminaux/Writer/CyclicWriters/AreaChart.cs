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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Area chart renderable
    /// </summary>
    public class AreaChart : IStaticRenderable
    {
        private (string, ChartElement[])[] elements = [];
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool showcase = false;
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
        /// Whether to render the bars upside down or not
        /// </summary>
        public bool UpsideDown { get; set; }

        /// <summary>
        /// Renders a area chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return TextWriterWhereColor.RenderWhere(
                RenderAreaChart(
                    elements, InteriorWidth, InteriorHeight, Showcase, UseColors, UpsideDown), Left, Top);
        }

        internal static string RenderAreaChart((string, ChartElement[])[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool useColor = true, bool upsideDown = false)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            int wholeLength = InteriorHeight - 1;

            // Normalize the list of elements
            List<(string, ChartElement[])> elementList = [];
            int maxElements = elements.Max((tuple) => tuple.Item2.Length);
            foreach (var element in elements)
            {
                var specificElements = element.Item2.ToList();

                // Set the element color
                Color elementColor = specificElements.Count == 0 ? ColorTools.GetRandomColor() : specificElements[0].Color;
                for (int i = 0; i < specificElements.Count; i++)
                    specificElements[i].Color = elementColor;

                // Check the element count
                if (specificElements.Count < maxElements)
                    for (int i = 0; i < maxElements - specificElements.Count; i++)
                        specificElements.Add(new());

                // Add the resultant elements
                elementList.Add((element.Item1, [.. specificElements]));
            }
            elements = [.. elementList];

            // Get the lengths
            int nameLength = elements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Item1));
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double lineWidth = (double)(InteriorWidth - (showcaseLength + 3)) / elements.Length / 2;

            // Fill the line chart with the showcase first
            StringBuilder areaChart = new();
            for (int i = 0; i < InteriorHeight; i++)
            {
                // If showcase is on, show names and values.
                int processedWidth = 0;
                if (showcase && i < elements.Length)
                {
                    var element = elements[i];
                    int nameWidth = ConsoleChar.EstimateCellWidth(element.Item1);
                    int spaces = showcaseLength - (" ■ ".Length + nameWidth);
                    spaces = spaces < 0 ? 0 : spaces;
                    areaChart.Append(
                        (useColor ? ColorTools.RenderSetConsoleColor(element.Item2[0].Color) : "") +
                        " ■ " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Item1.Truncate(nameLength - 4) +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        new string(' ', spaces) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else if (showcase)
                {
                    areaChart.Append(
                        new string(' ', showcaseLength) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else
                {
                    areaChart.Append(
                        " ┃ "
                    );
                    processedWidth += 3;
                }

                // Render all elements
                int elementHeightIdx = 0;
                int elementIdx = 0;
                while (processedWidth < InteriorWidth)
                {
                    var elementTuple = elements[elementIdx];
                    double maxValue = elementTuple.Item2.Max((element) => element.Value);
                    var shownElementHeights = elementTuple.Item2.Select((ce) => (ce, (int)(ce.Value * wholeLength / maxValue))).OrderByDescending((ce) => ce.Item2).ToArray();
                    int inverse = upsideDown ? i : InteriorHeight - i;

                    // Get the current and the next element
                    var elementHeightTuple = shownElementHeights[elementHeightIdx];
                    var nextElementTuple = elementIdx + 1 < shownElementHeights.Length ? shownElementHeights[elementHeightIdx + 1] : default;
                    ChartElement? element = elementHeightTuple.ce;
                    int height = elementHeightTuple.Item2;
                    ChartElement? nextElement = nextElementTuple.ce;
                    int nextHeight = nextElementTuple.Item2;

                    double threshold = nextElement is null ? 0 : (nextHeight - height) / lineWidth;
                    for (int w = 0; w < (int)lineWidth; w++)
                    {
                        int finalHeight = (int)Math.Round(height + (threshold * w));
                        var color =
                            inverse <= finalHeight ? element.Color :
                            (useColor ? ColorTools.CurrentBackgroundColor : "");
                        double value = element.Value;

                        // Render the element and its value
                        areaChart.Append(
                            (useColor ? ColorTools.RenderSetConsoleColor(color, true) : "") +
                            "  " +
                            (useColor ? ColorTools.RenderResetBackground() : "")
                        );
                        processedWidth += 2;
                        if (nextElement is null)
                            break;
                    }

                    // Next element height, or next element, or bail
                    elementHeightIdx++;
                    if (elementHeightIdx >= shownElementHeights.Length)
                    {
                        elementIdx++;
                        elementHeightIdx = 0;
                    }
                    if (elementIdx >= elements.Length)
                        break;
                }

                if (i < InteriorHeight - 1)
                    areaChart.AppendLine();
            }

            // Return the result
            return areaChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the area chart renderer
        /// </summary>
        public AreaChart()
        { }
    }
}
