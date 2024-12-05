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
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// Breakdown chart writer with color support
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
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
        public static string RenderBreakdownChart(ChartElement[] elements, int InteriorWidth, int InteriorHeight, bool showcase = false, bool vertical = false) =>
            BreakdownChart.RenderBreakdownChart(elements, InteriorWidth, InteriorHeight, showcase, vertical);

        static BreakdownChartColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
