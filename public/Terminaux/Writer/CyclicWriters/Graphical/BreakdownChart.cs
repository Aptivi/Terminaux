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
using Colorimetry.Data;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

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
        /// Whether to color the values or not
        /// </summary>
        public bool ColorValues { get; set; }

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
                    Width = Width / 4,
                    Height = Height,
                    UseColors = UseColors,
                    ColorValues = ColorValues,
                    Elements = Elements,
                };
                int showcaseLength = 0;

                // Fill the breakdown chart with the elements first
                if (Showcase)
                {
                    showcaseLength = showcase.Length;
                    breakdownChart.Append(RendererTools.RenderRenderable(showcase, new(Left, Top)));
                }

                // Some variables
                var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                double maxValue = shownElements.Sum((element) => element.Value);
                var orderedElements = (UpsideDown ? shownElements.OrderBy((ce) => ce.Value) : shownElements.OrderByDescending((ce) => ce.Value)).ToArray();

                // Show the actual bar
                int processedY = 0;
                double cumulative = 0;
                for (int e = 0; e < orderedElements.Length && maxValue > 0; e++)
                {
                    // Get the element and compute where its segment ends
                    ChartElement element = orderedElements[e];
                    cumulative += element.Value;
                    int boundary = e == orderedElements.Length - 1 ? Height : (int)Math.Round(cumulative * Height / maxValue);
                    int height = boundary - processedY;

                    // Use the chart height to draw the stick
                    for (int h = 0; h < height; h++)
                    {
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
                // Showcase variables
                var showcase = new ValueShowcaseHorizontal()
                {
                    Width = Width,
                    Height = Height - 1,
                    UseColors = UseColors,
                    ColorValues = ColorValues,
                    Elements = Elements,
                };

                // Fill the breakdown chart with the element bars first
                var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                double maxValue = shownElements.Sum((element) => element.Value);
                breakdownChart.Append(ConsolePositioning.RenderChangePosition(Left, Top));
                int processedX = 0;
                double cumulative = 0;
                for (int e = 0; e < shownElements.Length && maxValue > 0; e++)
                {
                    // Get the element and compute where its segment ends
                    var element = shownElements[e];
                    cumulative += element.Value;
                    int boundary = e == shownElements.Length - 1 ? Width : (int)Math.Round(cumulative * Width / maxValue);
                    int length = boundary - processedX;
                    processedX = boundary;

                    // Use the chart length to draw the bar
                    breakdownChart.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(element.Color, true) : "") +
                        new string(' ', length) +
                        (UseColors ? ConsoleColoring.RenderResetBackground() : "")
                    );
                }

                // Then, if we're told to showcase the values and the names, write them below the breakdown chart
                if (Showcase)
                    breakdownChart.Append(RendererTools.RenderRenderable(showcase, new(Left, Top + 1)));
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
