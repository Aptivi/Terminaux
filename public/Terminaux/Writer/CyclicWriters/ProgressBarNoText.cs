//
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

using System.Text;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Writer.CyclicWriters.Builtins;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Progress bar without text
    /// </summary>
    public class ProgressBarNoText : IStaticRenderable
    {
        private int position = 0;
        private int maxPosition = 0;
        private int indeterminateStep = 0;
        private bool indeterminateBackwards = false;
        private bool useColors = true;
        private ColorGradients indeterminateGradient = ColorGradients.GetGradients(ConsoleColors.DarkGreen, ConsoleColors.Lime, 50);
        private Spinner progressSpinner = BuiltinSpinners.Dots;
        private Color progressForegroundColor = ConsoleColors.DarkGreen;
        private Color progressActiveForegroundColor = ConsoleColors.Lime;

        /// <summary>
        /// Left margin of the progress bar
        /// </summary>
        public int LeftMargin { get; set; }

        /// <summary>
        /// Right margin of the progress bar
        /// </summary>
        public int RightMargin { get; set; }

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
        /// Progress spinner color
        /// </summary>
        public Color ProgressSpinnerTextColor { get; set; } = ConsoleColors.Grey;

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
            // Check the width
            int finalWidth = ConsoleWrapper.WindowWidth - LeftMargin - RightMargin;
            int spinnerWidth = 2 + ConsoleChar.EstimateCellWidth(progressSpinner.Peek());
            int percentageWidth = 6;
            int progressWidth = finalWidth - spinnerWidth - percentageWidth + 1;
            if (finalWidth < spinnerWidth)
                return "";
            if (finalWidth < spinnerWidth + percentageWidth)
                return $" {progressSpinner.Render()}";

            // Render the spinner
            var rendered = new StringBuilder();
            rendered.Append(
                (UseColors ? ColorTools.RenderSetConsoleColor(ProgressSpinnerTextColor) : "") +
                $" {progressSpinner.Render()} "
            );

            // Render the actual bar
            var bar = new SimpleProgress(Position, maxPosition)
            {
                Indeterminate = Indeterminate,
                LeftMargin = LeftMargin + spinnerWidth,
                RightMargin = RightMargin,
                ShowPercentage = ShowPercentage,
                ProgressPercentageTextColor = ProgressPercentageTextColor,
                ProgressActiveForegroundColor = ProgressActiveForegroundColor,
                ProgressBackgroundColor = ProgressBackgroundColor,
                ProgressForegroundColor = ProgressForegroundColor,
                ProgressHorizontalActiveTrackChar = ProgressHorizontalActiveTrackChar,
                ProgressHorizontalInactiveTrackChar = ProgressHorizontalInactiveTrackChar,
                ProgressVerticalActiveTrackChar = ProgressVerticalActiveTrackChar,
                ProgressVerticalInactiveTrackChar = ProgressVerticalInactiveTrackChar,
                indeterminateStep = indeterminateStep,
                indeterminateGradient = indeterminateGradient,
                indeterminateBackwards = indeterminateBackwards,
                UseColors = UseColors,
            };
            rendered.Append(bar.Render());
            indeterminateStep = bar.indeterminateStep;
            indeterminateGradient = bar.indeterminateGradient;
            indeterminateBackwards = bar.indeterminateBackwards;

            // Return the result
            rendered.Append(
                (UseColors ? ColorTools.RenderResetColors() : "")
            );
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of progress bar
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="maxPosition">Max position</param>
        /// <param name="progressSpinner">Spinner instance to use, or <see cref="BuiltinSpinners.Dots"/></param>
        public ProgressBarNoText(int position, int maxPosition, Spinner? progressSpinner = null)
        {
            this.position = position;
            this.maxPosition = maxPosition;
            this.progressSpinner = progressSpinner ?? BuiltinSpinners.Dots;
        }
    }
}
