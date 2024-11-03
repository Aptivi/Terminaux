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
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="showcase">Show the element list</param>
        /// <param name="vertical">Whether to render this chart vertically or horizontally</param>
        public static void WriteBreakdownChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool vertical = false)
        {
            try
            {
                // Fill the breakdown chart with spaces inside it
                TextWriterRaw.WriteRaw(RenderBreakdownChart(elements, InteriorWidth, InteriorHeight, showcase, vertical));
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
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="showcase">Show the element list</param>
        /// <param name="vertical">Whether to render this chart vertically or horizontally</param>
        /// <returns>The rendered breakdown chart</returns>
        public static string RenderBreakdownChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool vertical = false)
        {
            StringBuilder breakdownChart = new();
            if (vertical)
            {
                // Some variables
                int maxNameLength = InteriorWidth / 4;
                int wholeLength = InteriorHeight - 1;
                var shownElements = elements.Where((ce) => !ce.Hidden).ToArray();
                double maxValue = elements.Sum((element) => element.Value);
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
                            element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
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

        static BreakdownChartColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
