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
        /// Renders a line chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return TextWriterWhereColor.RenderWhere(
                RenderLineChart(
                    elements, InteriorWidth, InteriorHeight, Showcase, RunChart, UseColors), Left, Top);
        }

        internal static string RenderLineChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool run = false, bool useColor = true)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            int wholeLength = InteriorHeight - 1;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            var shownElementHeights = shownElements.Select((ce) => (ce, (int)(ce.Value * wholeLength / maxValue))).ToArray();
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double lineWidth = (double)(InteriorWidth - (showcaseLength + 3)) / shownElements.Length / 2;
            int median = (int)shownElements.Average((element) => element.Value);
            int medianPosition = (int)(median * wholeLength / maxValue);

            // Fill the line chart with the showcase first
            StringBuilder lineChart = new();
            for (int i = 0; i < InteriorHeight; i++)
            {
                // If showcase is on, show names and values.
                int processedWidth = 0;
                if (showcase && i < shownElements.Length)
                {
                    var element = shownElements[i];
                    int nameWidth = ConsoleChar.EstimateCellWidth(element.Name);
                    int spaces = showcaseLength - (" ■ ".Length + nameWidth + 2 + $"{element.Value}".Length);
                    spaces = spaces < 0 ? 0 : spaces;
                    lineChart.Append(
                        (useColor ? ColorTools.RenderSetConsoleColor(element.Color) : "") +
                        " ■ " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
                        element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
                        (useColor ? ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                        element.Value +
                        new string(' ', spaces) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else if (showcase)
                {
                    lineChart.Append(
                        new string(' ', showcaseLength) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else
                {
                    lineChart.Append(
                        " ┃ "
                    );
                    processedWidth += 3;
                }

                // Render all elements
                int inverse = InteriorHeight - i;
                int e = 0;
                while (processedWidth < InteriorWidth)
                {
                    var elementTuple = shownElementHeights[e];
                    var nextElementTuple = e + 1 < shownElementHeights.Length ? shownElementHeights[e + 1] : default;
                    ChartElement? element = elementTuple.ce;
                    int height = elementTuple.Item2;
                    ChartElement? nextElement = nextElementTuple.ce;
                    int nextHeight = nextElementTuple.Item2;

                    double threshold = nextElement is null ? 0 : (nextHeight - height) / lineWidth;
                    for (int w = 0; w < (int)lineWidth; w++)
                    {
                        int finalHeight = (int)Math.Round(height + (threshold * w));
                        var color =
                            inverse == finalHeight ? element.Color :
                            inverse == medianPosition && run ? ConsoleColors.Fuchsia :
                            (useColor ? ColorTools.CurrentBackgroundColor : "");
                        double value = element.Value;

                        // Render the element and its value
                        lineChart.Append(
                            (useColor ? ColorTools.RenderSetConsoleColor(color, true) : "") +
                            "  " +
                            (useColor ? ColorTools.RenderResetBackground() : "")
                        );
                        processedWidth += 2;
                        if (nextElement is null)
                            break;
                    }
                    e++;
                    if (e >= shownElements.Length)
                        break;
                }

                if (i < InteriorHeight - 1)
                    lineChart.AppendLine();
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
