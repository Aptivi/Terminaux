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

using System;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Breakdown chart renderable
    /// </summary>
    public class BreakdownChart : GraphicalCyclicWriter
    {
        private ChartElement[] elements = [];
        private bool showcase = false;
        private bool vertical = false;
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
        /// Whether to render this chart in vertical mode
        /// </summary>
        public bool Vertical
        {
            get => vertical;
            set => vertical = value;
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
        /// Renders a breakdown chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            StringBuilder breakdownChart = new();
            if (vertical)
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

                // Fill the breakdown chart with the elements first
                if (Showcase)
                {
                    showcaseLength = showcase.Length;
                    breakdownChart.Append(showcase.Render());
                }

                // Some variables
                var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                double maxValue = elements.Sum((element) => element.Value);
                var shownElementHeightsSelect = shownElements.Select((ce) => (ce, (int)Math.Round(ce.Value * Height / maxValue)));
                var shownElementHeights = (UpsideDown ? shownElementHeightsSelect.OrderBy((ce) => ce.Item2) : shownElementHeightsSelect.OrderByDescending((ce) => ce.Item2)).ToArray();

                // Show the actual bar
                int processedY = 0;
                for (int e = 0; e < shownElementHeights.Length; e++)
                {
                    // Get the element and the height
                    var elementTuple = shownElementHeights[e];
                    ChartElement element = elementTuple.ce;
                    int height = elementTuple.Item2;

                    // Use the chart height to draw the stick
                    for (int h = 0; h < height; h++)
                    {
                        // Decide whether to draw this area or not
                        Coordinate stickCoord = new(Left + showcaseLength, Top + processedY);
                        ConsoleLogger.Debug("Rendering breakdown chart element {0}: ({1} + {2}, {3} + {4})", e, Left, showcaseLength, Top, processedY);
                        breakdownChart.Append(
                            ConsolePositioning.RenderChangePosition(stickCoord.X, stickCoord.Y) +
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(element.Color, true) : "") +
                            "  " +
                            (UseColors ? ConsoleColoring.RenderResetBackground() : "")
                        );
                        processedY += 1;
                    }
                }
            }
            else
            {
                // Fill the breakdown chart with the element bars first
                double maxValue = elements.Sum((element) => element.Value);
                breakdownChart.Append(ConsolePositioning.RenderChangePosition(Left, Top));
                foreach (var element in elements)
                {
                    var color = element.Color;
                    bool hidden = element.Hidden;
                    double value = element.Value;
                    if (hidden)
                        continue;

                    // Render the element bar
                    int length = (int)Math.Round(value * Width / maxValue);
                    breakdownChart.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(color, true) : "") +
                        new string(' ', length) +
                        (UseColors ? ConsoleColoring.RenderResetBackground() : "")
                    );
                }

                // Then, if we're told to showcase the values and the names, write them below the breakdown chart
                if (Showcase)
                {
                    // Render the showcase elements
                    int maxElementLength = Width / 4;
                    var shownElements = elements.Where((ce) => !ce.Hidden).OrderByDescending((ce) => ce.Value).ToArray();
                    int totalWidth = 0;
                    int height = Top + 1;
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
                        if (totalWidth > Width || i == 0)
                        {
                            breakdownChart.Append(ConsolePositioning.RenderChangePosition(Left, height));
                            height++;
                            totalWidth = width + spaces.Length;
                        }

                        // Render the showcase element
                        breakdownChart.Append(
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(color) : "") +
                            " ■ " +
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Silver) : "") +
                            name + "  " +
                            (UseColors ? ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Grey) : "") +
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
