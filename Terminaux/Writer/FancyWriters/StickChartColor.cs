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
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Writer.FancyWriters.Tools;
using System.Linq;
using Terminaux.Base.Extensions;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Stick chart writer with color support
    /// </summary>
    public static class StickChartColor
    {
        /// <summary>
        /// Writes the stick chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window</param>
        /// <param name="InteriorHeight">The height of the interior window</param>
        /// <param name="showcase">Show the element list</param>
        public static void WriteStickChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false)
        {
            try
            {
                // Fill the stick chart with spaces inside it
                TextWriterRaw.WriteRaw(RenderStickChart(elements, InteriorWidth, InteriorHeight, showcase));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the stick chart
        /// </summary>
        /// <param name="elements">Elements to render</param>
        /// <param name="InteriorWidth">The width of the interior window</param>
        /// <param name="InteriorHeight">The height of the interior window</param>
        /// <param name="showcase">Show the element list</param>
        /// <returns>The rendered stick chart</returns>
        public static string RenderStickChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false)
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

            // Fill the stick chart with the showcase first
            StringBuilder stickChart = new();
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
                    stickChart.Append(
                        ColorTools.RenderSetConsoleColor(element.Color) +
                        " ■ " +
                        ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) +
                        element.Name.Truncate(nameLength - 4 - $"{maxValue}".Length) + "  " +
                        ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) +
                        element.Value +
                        new string(' ', spaces) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else if (showcase)
                {
                    stickChart.Append(
                        new string(' ', showcaseLength) +
                        " ┃ "
                    );
                    processedWidth += showcaseLength;
                }
                else
                {
                    stickChart.Append(
                        " ┃ "
                    );
                    processedWidth += 3;
                }

                // Render all elements
                int inverse = InteriorHeight - i;
                for (int e = 0; e < shownElementHeights.Length && processedWidth < InteriorWidth; e++)
                {
                    var elementTuple = shownElementHeights[e];
                    ChartElement? element = elementTuple.ce;
                    int height = elementTuple.Item2;
                    var color = inverse <= height ? element.Color : ColorTools.CurrentBackgroundColor;
                    string name = element.Name;
                    double value = element.Value;

                    // Render the element and its value
                    int length = (int)(value * wholeLength / maxValue);
                    stickChart.Append(
                        ColorTools.RenderSetConsoleColor(color, true) +
                        "  " +
                        ColorTools.RenderResetBackground()
                    );
                    processedWidth += 2;
                }

                if (i < InteriorHeight - 1)
                    stickChart.AppendLine();
            }

            // Return the result
            return stickChart.ToString();
        }

        static StickChartColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
