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

using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters.Builtins;
using Textify.General;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Progress bar without text
    /// </summary>
    public class ProgressBarNoText : ICyclicRenderer
    {
        private int position = 0;
        private int maxPosition = 0;
        private Spinner progressSpinner = BuiltinSpinners.Dots;

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
        /// Renders a scrolling text progress bar
        /// </summary>
        /// <returns>The result</returns>
        public string Render()
        {
            // Check the width
            int finalWidth = ConsoleWrapper.WindowWidth - LeftMargin - RightMargin;
            int spinnerWidth = 2 + ConsoleChar.EstimateCellWidth(progressSpinner.Peek());
            int percentageWidth = 6;
            int progressWidth = finalWidth - spinnerWidth - percentageWidth;
            if (finalWidth < spinnerWidth)
                return "";
            if (finalWidth < spinnerWidth + percentageWidth)
                return $" {progressSpinner.Render()}";

            // Render the spinner
            var rendered = new StringBuilder();
            rendered.Append(
                ColorTools.RenderSetConsoleColor(ConsoleColors.Grey) +
                $" {progressSpinner.Render()} "
            );

            // Estimate how many cells the progress bar takes
            int cells = (int)(position * progressWidth / (double)maxPosition);
            cells = cells > progressWidth ? progressWidth : cells;
            rendered.Append(
                ColorTools.RenderSetConsoleColor(ConsoleColors.Lime) +
                new string('━', cells) +
                ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen) +
                new string('━', progressWidth - cells)
            );

            // Write a progress percentage
            int percentage = (int)(position * 100 / (double)maxPosition);
            rendered.Append(
                ColorTools.RenderSetConsoleColor(ConsoleColors.Silver) +
                $" {percentage,3}%"
            );

            // Return the result
            return rendered.ToString();
        }

        /// <summary>
        /// Makes a new instance of progress bar
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxPosition"></param>
        /// <param name="progressSpinner"></param>
        public ProgressBarNoText(int position, int maxPosition, Spinner? progressSpinner = null)
        {
            this.position = position;
            this.maxPosition = maxPosition;
            this.progressSpinner = progressSpinner ?? BuiltinSpinners.Dots;
        }
    }
}
