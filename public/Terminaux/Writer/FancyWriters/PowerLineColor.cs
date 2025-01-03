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

using System;
using System.Collections.Generic;
using Terminaux.Colors;
using System.Text;
using Terminaux.Writer.ConsoleWriters;
using System.Diagnostics;
using Terminaux.Base.Checks;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.FancyWriters
{
    /// <summary>
    /// PowerLine writer
    /// </summary>
    [Obsolete("This is considered a legacy method of writing this fancy text and will be removed in a future version of Terminaux. Please use its cyclic writer equivalent.")]
    public static class PowerLineColor
    {
        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLinePlain(List<PowerLineSegment> Segments, bool Line = false)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderPowerLine(Segments, Line));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">List of PowerLine segments</param>
        /// <param name="EndingColor">A color that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLine(List<PowerLineSegment> Segments, Color EndingColor, bool Line = false)
        {
            try
            {
                TextWriterRaw.WriteRaw(RenderPowerLine(Segments, EndingColor, Line));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine($"There is a serious error when printing text. {ex.Message}");
            }
        }

        /// <summary>
        /// Renders the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static string RenderPowerLine(List<PowerLineSegment> Segments, bool Line = false) =>
            RenderPowerLine(Segments, ColorTools.currentBackgroundColor, Line);

        /// <summary>
        /// Renders the PowerLine text
        /// </summary>
        /// <param name="Segments">Segments to write</param>
        /// <param name="EndingColor">A color that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static string RenderPowerLine(List<PowerLineSegment> Segments, Color EndingColor, bool Line = false)
        {
            var segment = new StringBuilder();
            segment.Append(PowerLineTools.RenderSegments(Segments, EndingColor));
            if (Line)
                segment.AppendLine();
            return segment.ToString();
        }

        static PowerLineColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
