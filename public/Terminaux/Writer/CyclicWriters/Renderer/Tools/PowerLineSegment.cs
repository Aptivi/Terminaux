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

using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Renderer.Tools
{
    /// <summary>
    /// PowerLine segment
    /// </summary>
    public class PowerLineSegment
    {
        /// <summary>
        /// Foreground color of the segment
        /// </summary>
        public Color SegmentForeground { get; } = ThemeColorsTools.GetColor(ThemeColorType.NeutralText);

        /// <summary>
        /// Background color or next segment transition color of the segment
        /// </summary>
        public Color SegmentBackground { get; } = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Segment icon. This should be an iconic character.
        /// </summary>
        public char SegmentIcon { get; } = default;

        /// <summary>
        /// Segment transition icon. This should be an iconic character.
        /// </summary>
        public char SegmentTransitionIcon { get; } = default;

        /// <summary>
        /// Segment text. Usually a status.
        /// </summary>
        public string SegmentText { get; }

        /// <summary>
        /// Installs the segment values to the instance
        /// </summary>
        public PowerLineSegment(Color segmentForeground, Color segmentBackground, string segmentText, char segmentIcon = default, char segmentTransitionIcon = default)
        {
            SegmentForeground = segmentForeground;
            SegmentBackground = segmentBackground;
            SegmentIcon = segmentIcon;
            SegmentText = segmentText;
            SegmentTransitionIcon = segmentTransitionIcon;
        }
    }
}
