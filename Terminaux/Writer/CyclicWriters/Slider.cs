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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Writer.CyclicWriters.Builtins;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Simple slider with and without percentage support
    /// </summary>
    public class Slider : IStaticRenderable
    {
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
        /// Renders a scrolling text slider
        /// </summary>
        /// <returns>The result</returns>
        public string Render()
        {
            var rendered = new StringBuilder();
            if (Vertical)
            {
                // Estimate how many cells the slider takes
                int one = ConsoleMisc.PercentRepeatTargeted(1, maxPosition, Height);
                one = one == 0 ? 1 : one;
                int times = ConsoleMisc.PercentRepeatTargeted(position, maxPosition, Height);
                times = times >= Height ? Height - one : times;
                int rest = Height - (one + times);
                rest = rest < 0 ? 0 : rest;
                rendered.Append(
                    ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen)
                );
                for (int i = 0; i < times; i++)
                    rendered.AppendLine("┃");
                rendered.Append(
                    ColorTools.RenderSetConsoleColor(ConsoleColors.Lime)
                );
                for (int i = 0; i < one; i++)
                    rendered.AppendLine("┃");
                rendered.Append(
                    ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen)
                );
                for (int i = 0; i < rest; i++)
                    rendered.AppendLine("┃");
            }
            else
            {
                // Estimate how many cells the slider takes
                int one = ConsoleMisc.PercentRepeatTargeted(1, maxPosition, Width);
                one = one == 0 ? 1 : one;
                int times = ConsoleMisc.PercentRepeatTargeted(position, maxPosition, Width);
                times = times >= Width ? Width - one : times;
                int rest = Width - (one + times);
                rest = rest < 0 ? 0 : rest;
                rendered.Append(
                    ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen) +
                    new string('━', times) +
                    ColorTools.RenderSetConsoleColor(ConsoleColors.Lime) +
                    new string('━', one) +
                    ColorTools.RenderSetConsoleColor(ConsoleColors.DarkGreen) +
                    new string('━', rest)
                );
            }

            // Return the result
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
