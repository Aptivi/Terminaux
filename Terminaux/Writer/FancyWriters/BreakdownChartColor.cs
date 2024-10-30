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
using Terminaux.Base.Checks;
using Terminaux.Writer.FancyWriters.Tools;
using System.Linq;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Data;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Breakdown chart writer with color support
    /// </summary>
    public static class BreakdownChartColor
    {
        /// <summary>
        /// Writes the breakdown chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="showcase">Show the element list</param>
        public static void WriteBreakdownChart(ChartElement[] elements, int InteriorWidth, bool showcase = false)
        {
            try
            {
                // Fill the breakdown chart with spaces inside it
                TextWriterRaw.WriteRaw(RenderBreakdownChart(elements, InteriorWidth, showcase));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the breakdown chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="showcase">Show the element list</param>
        /// <returns>The rendered breakdown chart</returns>
        public static string RenderBreakdownChart(ChartElement[] elements, int InteriorWidth, bool showcase = false)
        {
            // Fill the breakdown chart with the element bars first
            double maxValue = elements.Sum((element) => element.Value);
            StringBuilder breakdownChart = new();
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

            // Return the result
            return breakdownChart.ToString();
        }

        static BreakdownChartColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
