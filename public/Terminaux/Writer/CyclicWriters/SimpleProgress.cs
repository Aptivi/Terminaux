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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Simple progress bar with and without percentage support
    /// </summary>
    public class SimpleProgress : IStaticRenderable
    {
        internal int indeterminateStep = 0;
        internal ColorGradients indeterminateGradient = ColorGradients.GetGradients(ConsoleColors.DarkGreen, ConsoleColors.Lime, 50);
        internal bool indeterminateBackwards = false;
        private bool useColors = true;
        private int position = 0;
        private int maxPosition = 0;
        private Color progressActiveForegroundColor = ConsoleColors.Lime;
        private Color progressForegroundColor = ConsoleColors.DarkGreen;

        /// <summary>
        /// Left margin of the progress bar
        /// </summary>
        public int LeftMargin { get; set; }

        /// <summary>
        /// Right margin of the progress bar
        /// </summary>
        public int RightMargin { get; set; }

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
        /// Specifies whether the progress bar is vertical or horizontal
        /// </summary>
        public bool Vertical { get; set; }

        /// <summary>
        /// Progress foreground
        /// </summary>
        public Color ProgressForegroundColor
        {
            get => progressForegroundColor;
            set
            {
                progressForegroundColor = value;
                indeterminateGradient = ColorGradients.GetGradients(progressForegroundColor, progressActiveForegroundColor, 50);
            }
        }

        /// <summary>
        /// Progress active foreground
        /// </summary>
        public Color ProgressActiveForegroundColor
        {
            get => progressActiveForegroundColor;
            set
            {
                progressActiveForegroundColor = value;
                indeterminateGradient = ColorGradients.GetGradients(progressForegroundColor, progressActiveForegroundColor, 50);
            }
        }

        /// <summary>
        /// Progress background
        /// </summary>
        public Color ProgressBackgroundColor { get; set; } = ColorTools.CurrentBackgroundColor;

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
        /// Renders a scrolling text progress bar
        /// </summary>
        /// <returns>The result</returns>
        public string Render()
        {
            var rendered = new StringBuilder();
            if (Vertical)
            {
                // Estimate how many cells the progress bar takes
                int cells = (int)Math.Round(position * Height / (double)maxPosition);
                cells = cells > Height ? Height : cells;
                cells = Indeterminate ? Height : cells;
                if (Indeterminate)
                {
                    // Step the indeterminate steps
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

                    // Get the gradient and render it
                    var gradientColor = indeterminateGradient[indeterminateStep - 1 < 0 ? 0 : indeterminateStep - 1];
                    rendered.Append(
                        (UseColors ? ColorTools.RenderSetConsoleColor(gradientColor.IntermediateColor) : "")
                    );
                    for (int i = 0; i < Height; i++)
                        rendered.AppendLine($"{ProgressVerticalInactiveTrackChar}");
                }
                else
                {
                    int remaining = Height - cells;
                    rendered.Append(
                        (UseColors ? ColorTools.RenderSetConsoleColor(ProgressActiveForegroundColor) : "")
                    );
                    for (int i = 0; i < cells; i++)
                        rendered.AppendLine($"{ProgressVerticalActiveTrackChar}");
                    rendered.Append(
                        (UseColors ? ColorTools.RenderSetConsoleColor(ProgressForegroundColor) : "")
                    );
                    for (int i = 0; i < remaining; i++)
                        rendered.AppendLine($"{ProgressVerticalInactiveTrackChar}");
                }
            }
            else
            {
                // Check the width
                int finalWidth = ConsoleWrapper.WindowWidth - LeftMargin - RightMargin;
                int percentageWidth = ShowPercentage ? 6 : 1;
                int progressWidth = finalWidth - percentageWidth + 1;
                if (finalWidth < progressWidth)
                    return "";

                // Estimate how many cells the progress bar takes
                int cells = (int)Math.Round(position * progressWidth / (double)maxPosition);
                cells = cells > progressWidth ? progressWidth : cells;
                cells = Indeterminate ? progressWidth : cells;
                if (Indeterminate)
                {
                    // Step the indeterminate steps
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

                    // Get the gradient and render it
                    var gradientColor = indeterminateGradient[indeterminateStep - 1 < 0 ? 0 : indeterminateStep - 1];
                    rendered.Append(
                        (UseColors ? ColorTools.RenderSetConsoleColor(gradientColor.IntermediateColor) : "") +
                        new string(ProgressHorizontalInactiveTrackChar, cells + percentageWidth - 1)
                    );
                }
                else
                {
                    rendered.Append(
                        (UseColors ? ColorTools.RenderSetConsoleColor(ProgressActiveForegroundColor) : "") +
                        new string(ProgressHorizontalActiveTrackChar, cells) +
                        (UseColors ? ColorTools.RenderSetConsoleColor(ProgressForegroundColor) : "") +
                        new string(ProgressHorizontalInactiveTrackChar, progressWidth - cells)
                    );

                    // Write a progress percentage
                    if (ShowPercentage)
                    {
                        int percentage = (int)(position * 100 / (double)maxPosition);
                        rendered.Append(
                            (UseColors ? ColorTools.RenderSetConsoleColor(ProgressPercentageTextColor) : "") +
                            $" {percentage,3}%"
                        );
                    }
                }
            }

            // Return the result
            rendered.Append(
                (UseColors ? ColorTools.RenderResetColors() : "")
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
