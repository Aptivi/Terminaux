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
using Terminaux.Writer.FancyWriters;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Breakdown chart renderable
    /// </summary>
    public class BreakdownChart : IStaticRenderable
    {
        private ChartElement[] elements = [];
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private bool showcase = false;
        private bool vertical = false;

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
        /// Whether to render this chart in vertical mode
        /// </summary>
        public bool Vertical
        {
            get => vertical;
            set => vertical = value;
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
        /// Renders a breakdown chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public string Render()
        {
            return TextWriterWhereColor.RenderWhere(
                RenderBreakdownChart(
                    elements, InteriorWidth, InteriorHeight, Showcase, Vertical), Left, Top);
        }

        internal static string RenderBreakdownChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool vertical = false)
        {
            StringBuilder breakdownChart = new();
            if (vertical)
            {
                // Some variables
                int maxNameLength = InteriorWidth / 4;
                int wholeLength = InteriorHeight - 1;
                var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                double maxValue = elements.Sum((element) => element.Value);
                double maxValueDisplay = shownElements.Max((element) => element.Value);
                int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
                nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
                var shownElementHeights = shownElements.Select((ce) => (ce, (int)Math.Round(ce.Value * wholeLength / maxValue))).ToArray();
                int showcaseLength = showcase ? nameLength + 3 : 0;

                // Get the height starts
                for (int e = 0; e < shownElementHeights.Length; e++)
                {
                    var elementTuple = shownElementHeights[e];
                    var elementSecond = e + 1 >= shownElementHeights.Length ? (null, 0) : shownElementHeights[e + 1];
                    int sum = 0;
                    for (int eh = e - 1; eh <= e; eh++)
                    {
                        eh = eh < 0 ? 0 : eh;
                        var elementTupleCalc = shownElementHeights[eh];
                        sum += elementTupleCalc.Item2;
                    }
                    shownElementHeights[e] = (elementTuple.ce, sum);
                }

                // Fill the breakdown chart with the showcase first
                for (int i = 0; i < InteriorHeight; i++)
                {
                    // If showcase is on, show names and values.
                    if (showcase && i < shownElements.Length)
                    {
                        var element = shownElements[i];
                        int nameWidth = ConsoleChar.EstimateCellWidth(element.Name);
                        int spaces = showcaseLength - (" ■ ".Length + nameWidth + 2 + $"{element.Value}".Length);
                        spaces = spaces < 0 ? 0 : spaces;
                        breakdownChart.Append(
                            ColorTools.RenderSetConsoleColor(element.Color) +
                            " ■ " +
                            ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) +
                            element.Name.Truncate(nameLength - 4 - $"{maxValueDisplay}".Length) + "  " +
                            ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) +
                            element.Value +
                            new string(' ', spaces) +
                            " ┃ "
                        );
                    }
                    else if (showcase)
                    {
                        breakdownChart.Append(
                            new string(' ', showcaseLength) +
                            " ┃ "
                        );
                    }
                    else
                    {
                        breakdownChart.Append(
                            " ┃ "
                        );
                    }

                    // Render all elements
                    for (int e = 0; e < shownElementHeights.Length; e++)
                    {
                        var elementTuple = shownElementHeights[e];
                        ChartElement? element = elementTuple.ce;
                        int height = elementTuple.Item2;
                        if (i > height)
                            continue;

                        var color = element.Color;
                        string name = element.Name;
                        double value = element.Value;

                        // Render the element and its value
                        int length = (int)(value * wholeLength / maxValue);
                        breakdownChart.Append(
                            ColorTools.RenderSetConsoleColor(color, true) +
                            "  " +
                            ColorTools.RenderResetBackground()
                        );
                        break;
                    }

                    if (i < InteriorHeight - 1)
                        breakdownChart.AppendLine();
                }
            }
            else
            {
                // Fill the breakdown chart with the element bars first
                double maxValue = elements.Sum((element) => element.Value);
                foreach (var element in elements)
                {
                    var color = element.Color;
                    bool hidden = element.Hidden;
                    double value = element.Value;
                    if (hidden)
                        continue;

                    // Render the element bar
                    int length = (int)Math.Round(value * InteriorWidth / maxValue);
                    breakdownChart.Append(
                        ColorTools.RenderSetConsoleColor(color, true) +
                        new string(' ', length) +
                        ColorTools.RenderResetBackground()
                    );
                }

                // Then, if we're told to showcase the values and the names, write them below the breakdown chart
                if (showcase)
                {
                    // Render the showcase elements
                    int maxElementLength = InteriorWidth / 4;
                    var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                    int totalWidth = 0;
                    for (int i = 0; i < shownElements.Length; i++)
                    {
                        var element = elements[i];
                        var color = element.Color;
                        double value = element.Value;
                        string name = element.Name.Truncate(maxElementLength - 3 - $"  {value}".Length);
                        int width = 3 + ConsoleChar.EstimateCellWidth(name) + $"  {value}".Length;
                        string spaces = new(' ', 4);
                        totalWidth += width + spaces.Length;

                        // If the element would overflow, make a new line
                        if (totalWidth > InteriorWidth || i == 0)
                        {
                            breakdownChart.AppendLine();
                            totalWidth = width + spaces.Length;
                        }

                        // Render the showcase element
                        breakdownChart.Append(
                            ColorTools.RenderSetConsoleColor(color) +
                            " ■ " +
                            ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) +
                            name + "  " +
                            ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) +
                            value +
                            spaces
                        );
                    }
                }
            }

            // Return the result
            return breakdownChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the breakdown chart renderer
        /// </summary>
        public BreakdownChart()
        { }
    }
}
