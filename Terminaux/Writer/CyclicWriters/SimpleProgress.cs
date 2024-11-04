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
        private int position = 0;
        private int maxPosition = 0;

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
                        ColorTools.RenderSetConsoleColor(gradientColor.IntermediateColor)
                    );
                    for (int i = 0; i < Height; i++)
                        rendered.AppendLine("┃");
                }
                else
                {
                    int remaining = Height - cells;
                    rendered.Append(
                        ColorTools.RenderSetConsoleColor(ConsoleColors.Lime)
                    );
                    for (int i = 0; i < cells; i++)
                        rendered.AppendLine("┃");
                    rendered.Append(
                        ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen)
                    );
                    for (int i = 0; i < remaining; i++)
                        rendered.AppendLine("┃");
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
                        ColorTools.RenderSetConsoleColor(gradientColor.IntermediateColor) +
                        new string('━', cells + percentageWidth - 1)
                    );
                }
                else
                {
                    rendered.Append(
                        ColorTools.RenderSetConsoleColor(ConsoleColors.Lime) +
                        new string('━', cells) +
                        ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen) +
                        new string('━', progressWidth - cells)
                    );

                    // Write a progress percentage
                    if (ShowPercentage)
                    {
                        int percentage = (int)(position * 100 / (double)maxPosition);
                        rendered.Append(
                            ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) +
                            $" {percentage,3}%"
                        );
                    }
                }
            }

            // Return the result
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
