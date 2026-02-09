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

using System.Text;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Colorimetry.Transformation;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Builtins;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Progress bar without text
    /// </summary>
    public class ProgressBarNoText : SimpleCyclicWriter
    {
        private int position = 0;
        private int maxPosition = 0;
        private int indeterminateStep = 0;
        private bool indeterminateBackwards = false;
        private bool useColors = true;
        private Spinner progressSpinner = BuiltinSpinners.SpinMore;

        /// <summary>
        /// Width of the progress bar
        /// </summary>
        public int Width { get; set; }

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
        /// Progress spinner color
        /// </summary>
        public Color ProgressSpinnerTextColor { get; set; } = ConsoleColors.Silver;

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
            // Check the width
            int spinnerWidth = 2 + ConsoleChar.EstimateCellWidth(progressSpinner.Peek());
            int percentageWidth = 6;
            if (Width < spinnerWidth)
                return "";
            if (Width < spinnerWidth + percentageWidth)
                return $" {progressSpinner.Render()}";

            // Render the spinner
            var rendered = new StringBuilder();
            progressSpinner.UseColors = UseColors;
            progressSpinner.ForegroundColor = ProgressSpinnerTextColor;
            progressSpinner.BackgroundColor = ProgressBackgroundColor;
            rendered.Append(
                $" {progressSpinner.Render()} "
            );

            // Render the actual bar
            var bar = new SimpleProgress(Position, maxPosition)
            {
                Indeterminate = Indeterminate,
                Width = Width - spinnerWidth,
                ShowPercentage = ShowPercentage,
                State = State,
                ProgressPercentageTextColor = ProgressPercentageTextColor,
                ProgressForegroundColor = ProgressForegroundColor,
                ProgressActiveForegroundColor = ProgressActiveForegroundColor,
                ProgressFailedForegroundColor = ProgressFailedForegroundColor,
                ProgressFailedActiveForegroundColor = ProgressFailedActiveForegroundColor,
                ProgressPausedForegroundColor = ProgressPausedForegroundColor,
                ProgressPausedActiveForegroundColor = ProgressPausedActiveForegroundColor,
                ProgressWarningForegroundColor = ProgressWarningForegroundColor,
                ProgressWarningActiveForegroundColor = ProgressWarningActiveForegroundColor,
                ProgressBackgroundColor = ProgressBackgroundColor,
                ProgressHorizontalActiveTrackChar = ProgressHorizontalActiveTrackChar,
                ProgressHorizontalInactiveTrackChar = ProgressHorizontalInactiveTrackChar,
                ProgressVerticalActiveTrackChar = ProgressVerticalActiveTrackChar,
                ProgressVerticalInactiveTrackChar = ProgressVerticalInactiveTrackChar,
                ProgressUncoloredHorizontalActiveTrackChar = ProgressUncoloredHorizontalActiveTrackChar,
                ProgressUncoloredHorizontalInactiveTrackChar = ProgressUncoloredHorizontalInactiveTrackChar,
                ProgressUncoloredVerticalActiveTrackChar = ProgressUncoloredVerticalActiveTrackChar,
                ProgressUncoloredVerticalInactiveTrackChar = ProgressUncoloredVerticalInactiveTrackChar,
                indeterminateStep = indeterminateStep,
                indeterminateBackwards = indeterminateBackwards,
                UseColors = UseColors,
            };
            rendered.Append(bar.Render());
            indeterminateStep = bar.indeterminateStep;
            indeterminateBackwards = bar.indeterminateBackwards;

            // Return the result
            rendered.Append(
                UseColors ? ConsoleColoring.RenderResetColors() : ""
            );
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of progress bar
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="maxPosition">Max position</param>
        /// <param name="progressSpinner">Spinner instance to use, or <see cref="BuiltinSpinners.SpinMore"/></param>
        public ProgressBarNoText(int position, int maxPosition, Spinner? progressSpinner = null)
        {
            this.position = position;
            this.maxPosition = maxPosition;
            this.progressSpinner = progressSpinner ?? BuiltinSpinners.SpinMore;
        }
    }
}
