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
using System.Collections.Generic;
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// PowerLine tools
    /// </summary>
    public static class PowerLineTools
    {
        /// <summary>
        /// Renders the segments
        /// </summary>
        /// <param name="segments">List of segments to render</param>
        public static string RenderSegments(List<PowerLineSegment> segments) =>
            RenderSegments(segments, ThemeColorsTools.GetColor(ThemeColorType.Background));

        /// <summary>
        /// Renders the segments
        /// </summary>
        /// <param name="segments">List of segments to render</param>
        /// <param name="EndingColor">Ending background color for the last segment transition</param>
        public static string RenderSegments(List<PowerLineSegment> segments, Color EndingColor)
        {
            // PowerLine glyphs
            char transitionChar = Convert.ToChar(0xE0B0);

            // Builder
            var SegmentStringBuilder = new StringBuilder();

            for (int segmentIdx = 0; segmentIdx < segments.Count; segmentIdx++)
            {
                // Get the segment
                var segment = segments[segmentIdx];

                // If we're in segment index 1 or higher, this means that we're going to have to make a transition, so we need
                // to get the last segment.
                if (segmentIdx > 0)
                {
                    // Get the last segment
                    var lastsegment = segments[segmentIdx - 1];

                    // Get the colors for the transition
                    var backAsFore = lastsegment.SegmentBackground;
                    var nextBack = segment.SegmentBackground;

                    // Now, put transition to our string
                    SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(backAsFore));
                    SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(nextBack, true));
                    SegmentStringBuilder.AppendFormat("{0}", segment.SegmentTransitionIcon != default ? segment.SegmentTransitionIcon : transitionChar);
                }

                // Now, try to append the PowerLine segment and its contents
                bool appendIcon = segment.SegmentIcon != default;
                SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(segment.SegmentForeground));
                SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(segment.SegmentBackground, true));
                if (appendIcon)
                    SegmentStringBuilder.AppendFormat(" {0}", segment.SegmentIcon);
                SegmentStringBuilder.AppendFormat(" {0} ", segment.SegmentText);

                // If the segment is the last one, make the final transition!
                if (segmentIdx == segments.Count - 1)
                {
                    SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(segment.SegmentBackground));
                    SegmentStringBuilder.Append(ConsoleColoring.RenderSetConsoleColor(EndingColor, true));
                    SegmentStringBuilder.AppendFormat("{0} ", transitionChar);
                }
            }

            // Return the final string
            SegmentStringBuilder.Append(
                ConsoleColoring.RenderRevertForeground() +
                ConsoleColoring.RenderRevertBackground()
            );
            return SegmentStringBuilder.ToString();
        }
    }
}
