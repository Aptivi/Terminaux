﻿//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Stick chart renderable
    /// </summary>
    public class StickChart : GraphicalCyclicWriter
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
        /// Whether to render the bars upside down or not
        /// </summary>
        public bool UpsideDown { get; set; }

        /// <summary>
        /// Renders a stick chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Some variables
            int maxNameLength = Width / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            var shownElementHeights = shownElements.Select((ce) => (ce, (int)(ce.Value * Height / maxValue))).ToArray();
            int showcaseLength = showcase ? nameLength + 3 : 0;
            double stickWidth = (double)(Width - (showcaseLength + 3)) / shownElements.Length / 2;

            // Fill the stick chart with the showcase first
            StringBuilder stickChart = new();
            if (Showcase)
            {
                for (int i = 0; i < shownElements.Length; i++)
                {
                    // Get the element showcase position and write it there
                    bool canShow = Height > i;
                    if (!canShow)
                        break;
                    Coordinate coord = new(Left, Top + i);
                    var element = shownElements[i];

                    // Now, write it at the selected position
                    stickChart.Append(
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
                for (int h = 0; h < Height; h++)
                {
                    Coordinate separatorCoord = new(Left + nameLength, Top + h);
                    stickChart.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ┃ "
                    );
                }
            }

            // Show the actual bar
            for (int e = 0; e < shownElementHeights.Length; e++)
            {
                // Get the element and the height
                var elementTuple = shownElementHeights[e];
                ChartElement element = elementTuple.ce;
                int height = elementTuple.Item2;

                // Use the chart height to draw the stick
                for (int h = 0; h < Height; h++)
                {
                    // Decide whether to draw this area or not
                    int stickWidthInt = (int)stickWidth * 2;
                    int inverse = UpsideDown ? h : Height - h;
                    Coordinate stickCoord = new(Left + showcaseLength + (stickWidthInt * e), Top + h);
                    ConsoleLogger.Debug("Rendering stick chart element {0}: ({1}, {2})", e, stickCoord.X, stickCoord.Y);
                    if (inverse <= height)
                    {
                        stickChart.Append(
                            ConsolePositioning.RenderChangePosition(stickCoord.X, stickCoord.Y) +
                            (UseColors ? ColorTools.RenderSetConsoleColor(element.Color, true) : "") +
                            new string(' ', stickWidthInt) +
                            (UseColors ? ColorTools.RenderResetBackground() : "")
                        );
                    }
                }
            }

            // Return the result
            return stickChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the stick chart renderer
        /// </summary>
        public StickChart()
        { }
    }
}
