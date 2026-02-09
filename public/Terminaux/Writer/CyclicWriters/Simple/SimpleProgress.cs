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
using Colorimetry.Data;
using Colorimetry.Transformation;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Simple progress bar with and without percentage support
    /// </summary>
    public class SimpleProgress : SimpleCyclicWriter
    {
        internal int indeterminateStep = 0;
        internal bool indeterminateBackwards = false;
        private bool useColors = true;
        private int position = 0;
        private int maxPosition = 0;

        /// <summary>
        /// Width of the progress bar
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the vertical progress bar
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Position of the progress bar
        /// </summary>
        public int Position
        {
            get => position;
            set
            {
                position = value;
                if (position < 0)
                    position = 0;
                else if (position > maxPosition)
                    position = maxPosition;
            }
        }

        /// <summary>
        /// Specifies whether the progress is indeterminate
        /// </summary>
        public bool Indeterminate { get; set; }

        /// <summary>
        /// Specifies whether the progress bar shows percentage or not (ignored in indeterminate progress bars)
        /// </summary>
        public bool ShowPercentage { get; set; } = true;

        /// <summary>
        /// State of the progress bar
        /// </summary>
        public ProgressState State { get; set; }

        /// <summary>
        /// Specifies whether the progress bar is vertical or horizontal
        /// </summary>
        public bool Vertical { get; set; }

        /// <summary>
        /// Progress foreground
        /// </summary>
        public Color ProgressForegroundColor { get; set; } = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.Progress));

        /// <summary>
        /// Progress active foreground
        /// </summary>
        public Color ProgressActiveForegroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Progress);

        /// <summary>
        /// Progress failed foreground
        /// </summary>
        public Color ProgressFailedForegroundColor { get; set; } = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.ProgressFailed));

        /// <summary>
        /// Progress failed active foreground
        /// </summary>
        public Color ProgressFailedActiveForegroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.ProgressFailed);

        /// <summary>
        /// Progress paused foreground
        /// </summary>
        public Color ProgressPausedForegroundColor { get; set; } = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.ProgressPaused));

        /// <summary>
        /// Progress paused active foreground
        /// </summary>
        public Color ProgressPausedActiveForegroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.ProgressPaused);

        /// <summary>
        /// Progress warning foreground
        /// </summary>
        public Color ProgressWarningForegroundColor { get; set; } = TransformationTools.GetDarkBackground(ThemeColorsTools.GetColor(ThemeColorType.ProgressWarning));

        /// <summary>
        /// Progress warning active foreground
        /// </summary>
        public Color ProgressWarningActiveForegroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.ProgressWarning);

        /// <summary>
        /// Progress background
        /// </summary>
        public Color ProgressBackgroundColor { get; set; } = ThemeColorsTools.GetColor(ThemeColorType.Background);

        /// <summary>
        /// Progress percentage text color
        /// </summary>
        public Color ProgressPercentageTextColor { get; set; } = ConsoleColors.Silver;

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Progress vertical inactive track character for drawing
        /// </summary>
        public char ProgressVerticalInactiveTrackChar { get; set; } = '┃';

        /// <summary>
        /// Progress vertical active track character for drawing
        /// </summary>
        public char ProgressVerticalActiveTrackChar { get; set; } = '┃';

        /// <summary>
        /// Progress horizontal inactive track character for drawing
        /// </summary>
        public char ProgressHorizontalInactiveTrackChar { get; set; } = '━';

        /// <summary>
        /// Progress horizontal active track character for drawing
        /// </summary>
        public char ProgressHorizontalActiveTrackChar { get; set; } = '━';

        /// <summary>
        /// Progress vertical inactive track character for drawing (uncolored)
        /// </summary>
        public char ProgressUncoloredVerticalInactiveTrackChar { get; set; } = '▒';

        /// <summary>
        /// Progress vertical active track character for drawing (uncolored)
        /// </summary>
        public char ProgressUncoloredVerticalActiveTrackChar { get; set; } = '█';

        /// <summary>
        /// Progress horizontal inactive track character for drawing (uncolored)
        /// </summary>
        public char ProgressUncoloredHorizontalInactiveTrackChar { get; set; } = '▒';

        /// <summary>
        /// Progress horizontal active track character for drawing (uncolored)
        /// </summary>
        public char ProgressUncoloredHorizontalActiveTrackChar { get; set; } = '█';

        /// <summary>
        /// Renders a scrolling text progress bar
        /// </summary>
        /// <returns>The result</returns>
        public override string Render()
        {
            var rendered = new StringBuilder();
            if (UseColors)
                rendered.Append(ConsoleColoring.RenderSetConsoleColor(ProgressBackgroundColor, true));

            // Choose the color, depending on the progress state
            var activeProgressColor =
                State == ProgressState.Failed ? ProgressFailedActiveForegroundColor :
                State == ProgressState.Paused ? ProgressPausedActiveForegroundColor :
                State == ProgressState.Warning ? ProgressWarningActiveForegroundColor :
                ProgressActiveForegroundColor;
            var progressColor =
                State == ProgressState.Failed ? ProgressFailedForegroundColor :
                State == ProgressState.Paused ? ProgressPausedForegroundColor :
                State == ProgressState.Warning ? ProgressWarningForegroundColor :
                ProgressForegroundColor;

            // Make an indeterminate slider in case we need it
            var indeterminateSlider = new Slider(0, 0, 50)
            {
                SliderActiveForegroundColor = activeProgressColor,
                SliderForegroundColor = progressColor,
                SliderBackgroundColor = ProgressBackgroundColor,
                UseColors = UseColors,
                Vertical = Vertical,
                Height = Height,
                Width = Width,
            };

            if (Indeterminate)
            {
                // Step the indeterminate steps
                if (State != ProgressState.Failed && State != ProgressState.Paused)
                {
                    if (indeterminateBackwards)
                    {
                        indeterminateStep--;
                        if (indeterminateStep == 0)
                            indeterminateBackwards = false;
                    }
                    else
                    {
                        indeterminateStep++;
                        if (indeterminateStep == 50)
                            indeterminateBackwards = true;
                    }
                }

                // Render the slider that indicates indeterminate progress
                indeterminateSlider.Position = indeterminateStep;
                rendered.Append(indeterminateSlider.Render());
            }
            else if (Vertical)
            {
                // Estimate how many cells the progress bar takes
                int cells = (int)Math.Round(position * Height / (double)maxPosition);
                cells = cells > Height ? Height : cells;
                int remaining = Height - cells;
                rendered.Append(
                    UseColors ? ConsoleColoring.RenderSetConsoleColor(activeProgressColor) : ""
                );
                for (int i = 0; i < cells; i++)
                    rendered.AppendLine($"{(UseColors ? ProgressVerticalActiveTrackChar : ProgressUncoloredVerticalActiveTrackChar)}");
                rendered.Append(
                    UseColors ? ConsoleColoring.RenderSetConsoleColor(progressColor) : ""
                );
                for (int i = 0; i < remaining; i++)
                    rendered.AppendLine($"{(UseColors ? ProgressVerticalInactiveTrackChar : ProgressUncoloredVerticalInactiveTrackChar)}");
            }
            else
            {
                // Check the width
                int percentageWidth = ShowPercentage ? 6 : 1;
                int progressWidth = Width - percentageWidth + 1;
                if (Width < progressWidth)
                    return "";

                // Estimate how many cells the progress bar takes
                int cells = (int)Math.Round(position * progressWidth / (double)maxPosition);
                cells = cells > progressWidth ? progressWidth : cells;
                rendered.Append(
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(activeProgressColor) : "") +
                    new string(UseColors ? ProgressHorizontalActiveTrackChar : ProgressUncoloredHorizontalActiveTrackChar, cells) +
                    (UseColors ? ConsoleColoring.RenderSetConsoleColor(progressColor) : "") +
                    new string(UseColors ? ProgressHorizontalInactiveTrackChar : ProgressUncoloredHorizontalInactiveTrackChar, progressWidth - cells)
                );

                // Write a progress percentage
                if (ShowPercentage)
                {
                    int percentage = (int)(position * 100 / (double)maxPosition);
                    rendered.Append(
                        (UseColors ? ConsoleColoring.RenderSetConsoleColor(ProgressPercentageTextColor) : "") +
                        $" {percentage,3}%"
                    );
                }
            }

            // Return the result
            rendered.Append(
                UseColors ? ConsoleColoring.RenderResetColors() : ""
            );
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of simple progress bar
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="maxPosition">Max position</param>
        public SimpleProgress(int position, int maxPosition)
        {
            this.position = position;
            this.maxPosition = maxPosition;
        }
    }
}
