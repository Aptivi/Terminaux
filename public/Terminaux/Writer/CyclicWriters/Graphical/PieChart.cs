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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Pie chart renderable
    /// </summary>
    public class PieChart : GraphicalCyclicWriter
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
        /// Renders a pie chart
        /// </summary>
        /// <returns>Rendered text that will be used by the renderer</returns>
        public override string Render()
        {
            // Some variables
            int maxNameLength = Width / 4;
            var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
            double maxValue = shownElements.Max((element) => element.Value);
            double maxValueAngles = shownElements.Sum((element) => element.Value);
            int nameLength = shownElements.Max((element) => " ■ ".Length + ConsoleChar.EstimateCellWidth(element.Name) + $"  {element.Value}".Length);
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;

            // Fill the pie chart with the showcase first
            StringBuilder pieChart = new();
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
                    pieChart.Append(
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
                    pieChart.Append(
                        ConsolePositioning.RenderChangePosition(separatorCoord.X, separatorCoord.Y) +
                        " ┃ "
                    );
                }
            }

            // Get the appropriate start and sweep angles, depending on the element
            int startAngle = 0;
            int sweepAngle = 0;
            List<(int start, int sweep)> angles = [];
            for (int i = 0; i < shownElements.Length; i++)
            {
                var element = shownElements[i];

                // Set the sweep angle
                sweepAngle += (int)(element.Value / maxValueAngles * 360);

                // Add the start and the sweep angles to the list
                angles.Add((startAngle, sweepAngle));

                // Now, set the start angle
                startAngle = sweepAngle;

                // If this is the last element and the angle is not yet 360, we need to finish it
                if (i == shownElements.Length - 1 && startAngle < 360)
                    angles.Add((startAngle, 360));
            }
            
            // Create pies with element representations
            for (int i = 0; i < angles.Count; i++)
            {
                var (start, sweep) = angles[i];
                var element = i < shownElements.Length ? shownElements[i] : new()
                {
                    Name = "Others...",
                    Value = (sweep - start) / 360,
                    Color = ConsoleColors.Silver,
                };

                // Render the element using a pie
                ConsoleLogger.Debug("Rendering pie chart element {0}: (start angle: {1}, sweep angle: {2})", i, start, sweep);
                var pie = new Arc(Height, Left + showcaseLength, Top, element.Color)
                {
                    InnerRadius = 0,
                    OuterRadius = Height / 2,
                    AngleStart = start,
                    AngleEnd = sweep,
                };
                pieChart.Append(pie.Render());
            }

            // Return the result
            return pieChart.ToString();
        }

        /// <summary>
        /// Makes a new instance of the pie chart renderer
        /// </summary>
        public PieChart()
        { }
    }
}
