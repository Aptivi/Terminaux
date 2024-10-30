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
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base.Checks;
using Terminaux.Writer.FancyWriters.Tools;
using System.Linq;
using Terminaux.Base.Extensions;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Bar chart writer with color support
    /// </summary>
    public static class BarChartColor
    {
        /// <summary>
        /// Writes the bar chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="showcase">Show the element list</param>
        public static void WriteBarChart(ChartElement[] elements, int InteriorWidth, bool showcase = false)
        {
            try
            {
                // Fill the bar chart with spaces inside it
                TextWriterRaw.WriteRaw(RenderBarChart(elements, InteriorWidth, showcase));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the bar chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="showcase">Show the element list</param>
        /// <returns>The rendered bar chart</returns>
        public static string RenderBarChart(ChartElement[] elements, int InteriorWidth, bool showcase = false)
        {
            // Some variables
            int maxNameLength = InteriorWidth / 4;
            int nameLength = elements.Max((element) => ConsoleChar.EstimateCellWidth(element.Name));
            nameLength = nameLength > maxNameLength ? maxNameLength : nameLength;
            int showcaseLength = showcase ? nameLength + 3 : 0;
            int wholeLength = InteriorWidth - showcaseLength;
            int maxNumLength = elements.Max((element) => $" {element.Value}".Length);
            int chartLength = wholeLength - maxNumLength;
            double maxValue = elements.Max((element) => element.Value);

            // Fill the bar chart with the elements first
            StringBuilder barChart = new();
            foreach (var element in elements)
            {
                var color = element.Color;
                string name = element.Name;
                bool hidden = element.Hidden;
                double value = element.Value;
                if (hidden)
                    continue;

                // Render the showcase
                if (showcase)
                {
                    int nameWidth = ConsoleChar.EstimateCellWidth(name);
                    int spaces = nameLength - nameWidth;
                    barChart.Append(
                        ColorTools.RenderSetConsoleColor(color) +
                        name.Truncate(nameLength) +
                        new string(' ', spaces) +
                        ColorTools.RenderResetForeground() +
                        " ┃ "
                    );
                }

                // Render the element and its value
                int length = (int)(value * chartLength / maxValue);
                barChart.AppendLine(
                    ColorTools.RenderSetConsoleColor(color, true) +
                    new string(' ', length) +
                    ColorTools.RenderSetConsoleColor(color) +
                    ColorTools.RenderResetBackground() +
                    $" {value}" +
                    ColorTools.RenderResetForeground()
                );
            }

            // Return the result
            return barChart.ToString();
        }

        static BarChartColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
