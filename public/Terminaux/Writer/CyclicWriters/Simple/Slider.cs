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
using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Transformation;
using Terminaux.Themes.Colors;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Simple slider with and without percentage support
    /// </summary>
    public class Slider : SimpleCyclicWriter
    {
        private bool useColors = true;
        private int position = 0;
        private int minPosition = 0;
        private int maxPosition = 0;

        /// <summary>
        /// Width of the horizontal slider
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the vertical slider
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Position of the slider
        /// </summary>
        public int Position
        {
            get => position;
            set
            {
                position = value;
                if (position < minPosition)
                    position = minPosition;
                else if (position > maxPosition)
                    position = maxPosition;
            }
        }

        /// <summary>
        /// Specifies whether the slider is vertical or horizontal
        /// </summary>
        public bool Vertical { get; set; }

        /// <summary>
        /// Slider foreground
        /// </summary>
        public Color SliderForegroundColor { get; set; } = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.Progress));

        /// <summary>
        /// Slider active foreground
        /// </summary>
        public Color SliderActiveForegroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Progress);

        /// <summary>
        /// Slider background
        /// </summary>
        public Color SliderBackgroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Slider vertical inactive track character for drawing
        /// </summary>
        public char SliderVerticalInactiveTrackChar { get; set; } = '┃';

        /// <summary>
        /// Slider vertical active track character for drawing
        /// </summary>
        public char SliderVerticalActiveTrackChar { get; set; } = '┃';

        /// <summary>
        /// Slider horizontal inactive track character for drawing
        /// </summary>
        public char SliderHorizontalInactiveTrackChar { get; set; } = '━';

        /// <summary>
        /// Slider horizontal active track character for drawing
        /// </summary>
        public char SliderHorizontalActiveTrackChar { get; set; } = '━';

        /// <summary>
        /// Slider vertical inactive track character for drawing (uncolored)
        /// </summary>
        public char SliderUncoloredVerticalInactiveTrackChar { get; set; } = '▒';

        /// <summary>
        /// Slider vertical active track character for drawing (uncolored)
        /// </summary>
        public char SliderUncoloredVerticalActiveTrackChar { get; set; } = '█';

        /// <summary>
        /// Slider horizontal inactive track character for drawing (uncolored)
        /// </summary>
        public char SliderUncoloredHorizontalInactiveTrackChar { get; set; } = '▒';

        /// <summary>
        /// Slider horizontal active track character for drawing (uncolored)
        /// </summary>
        public char SliderUncoloredHorizontalActiveTrackChar { get; set; } = '█';

        /// <summary>
        /// Renders a scrolling text slider
        /// </summary>
        /// <returns>The result</returns>
        public override string Render()
        {
            var rendered = new StringBuilder();
            if (UseColors)
                rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderBackgroundColor, true));
            if (Vertical)
            {
                // Estimate how many cells the slider takes
                int one = ConsoleMisc.PercentRepeatTargeted(minPosition > 0 ? minPosition : 1, maxPosition, Height);
                one = one == 0 ? 1 : one;
                int times = ConsoleMisc.PercentRepeatTargeted(position - minPosition, maxPosition, Height - 1);
                times = times + one >= Height ? Height - one : times;
                int rest = Height - (one + times);
                rest = rest < 0 ? 0 : rest;
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderForegroundColor));
                for (int i = 0; i < times; i++)
                    rendered.AppendLine($"{(UseColors ? SliderVerticalInactiveTrackChar : SliderUncoloredVerticalInactiveTrackChar)}");
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderActiveForegroundColor));
                for (int i = 0; i < one; i++)
                    rendered.AppendLine($"{(UseColors ? SliderVerticalActiveTrackChar : SliderUncoloredVerticalActiveTrackChar)}");
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderForegroundColor));
                for (int i = 0; i < rest; i++)
                    rendered.AppendLine($"{(UseColors ? SliderVerticalInactiveTrackChar : SliderUncoloredVerticalInactiveTrackChar)}");
            }
            else
            {
                // Estimate how many cells the slider takes
                int one = ConsoleMisc.PercentRepeatTargeted(minPosition > 0 ? minPosition : 1, maxPosition, Width);
                one = one == 0 ? 1 : one;
                int times = ConsoleMisc.PercentRepeatTargeted(position - minPosition, maxPosition, Width - 1);
                times = times + one >= Width ? Width - one : times;
                int rest = Width - (one + times);
                rest = rest < 0 ? 0 : rest;
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderForegroundColor));
                rendered.Append(new string(UseColors ? SliderHorizontalInactiveTrackChar : SliderUncoloredHorizontalInactiveTrackChar, times));
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderActiveForegroundColor));
                rendered.Append(new string(UseColors ? SliderHorizontalActiveTrackChar : SliderUncoloredHorizontalActiveTrackChar, one));
                if (UseColors)
                    rendered.Append(ConsoleColoring.RenderSetConsoleColor(SliderForegroundColor));
                rendered.Append(new string(UseColors ? SliderHorizontalInactiveTrackChar : SliderUncoloredHorizontalInactiveTrackChar, rest));
            }

            // Return the result
            rendered.Append(
                UseColors ? ConsoleColoring.RenderResetColors() : ""
            );
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of simple slider
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="minPosition">Min position</param>
        /// <param name="maxPosition">Max position</param>
        public Slider(int position, int minPosition, int maxPosition)
        {
            this.position = position;
            this.minPosition = Math.Min(minPosition, maxPosition);
            this.maxPosition = Math.Max(minPosition, maxPosition);
        }
    }
}
