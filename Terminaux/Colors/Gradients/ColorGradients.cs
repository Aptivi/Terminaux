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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Base;

namespace Terminaux.Colors.Gradients
{
    /// <summary>
    /// The color gradients class
    /// </summary>
    [DebuggerDisplay("{Count} gradients")]
    public class ColorGradients : IEnumerable<ColorGradient>
    {
        private readonly ColorGradientEnumerator enumerator = new();

        /// <inheritdoc/>
        public IEnumerator<ColorGradient> GetEnumerator() =>
            enumerator;

        /// <summary>
        /// Gets the collection of color gradients
        /// </summary>
        /// <param name="sourceColor">Source color to transtition from</param>
        /// <param name="targetColor">Target color to transition to</param>
        /// <param name="steps">Number of steps to advance</param>
        /// <returns>An instance of <see cref="ColorGradients"/> that you can enumerate.</returns>
        public static ColorGradients GetGradients(Color sourceColor, Color targetColor, int steps)
        {
            // Some variables
            ColorGradients gradients = new();

            // Check the values
            if (steps <= 0)
                steps = 1;
            if (steps == 1)
            {
                var source = new ColorGradient(1, sourceColor);
                var target = new ColorGradient(2, targetColor);
                gradients.enumerator.gradients.Add(source);
                gradients.enumerator.gradients.Add(target);
                return gradients;
            }

            // Check for nulls
            if (sourceColor.RGB is null)
                throw new TerminauxException("Source color's RGB instance is null");
            if (targetColor.RGB is null)
                throw new TerminauxException("Target color's RGB instance is null");

            // Now, form the gradients
            int colorRedThreshold = sourceColor.RGB.R - targetColor.RGB.R;
            int colorGreenThreshold = sourceColor.RGB.G - targetColor.RGB.G;
            int colorBlueThreshold = sourceColor.RGB.B - targetColor.RGB.B;
            double colorRedSteps = (double)colorRedThreshold / steps;
            double colorGreenSteps = (double)colorGreenThreshold / steps;
            double colorBlueSteps = (double)colorBlueThreshold / steps;
            double currentColorRed = sourceColor.RGB.R;
            double currentColorGreen = sourceColor.RGB.G;
            double currentColorBlue = sourceColor.RGB.B;
            for (int x = 0; x < steps; x++)
            {
                // Make a new instance of Color to indicate the intermediate gradient color
                var currentColorInstance = new Color($"{Convert.ToInt32(currentColorRed)};{Convert.ToInt32(currentColorGreen)};{Convert.ToInt32(currentColorBlue)}");
                var gradient = new ColorGradient(x + 1, currentColorInstance);
                gradients.enumerator.gradients.Add(gradient);

                // Change the colors
                currentColorRed -= colorRedSteps;
                currentColorGreen -= colorGreenSteps;
                currentColorBlue -= colorBlueSteps;
            }

            // Make a new instance of Color to indicate the final gradient color
            var finalColorInstance = new Color($"{Convert.ToInt32(currentColorRed)};{Convert.ToInt32(currentColorGreen)};{Convert.ToInt32(currentColorBlue)}");
            var finalGradient = new ColorGradient(steps + 1, finalColorInstance);
            gradients.enumerator.gradients[steps - 1] = finalGradient;

            // Return the final instance
            return gradients;
        }

        /// <summary>
        /// Gets the number of gradients
        /// </summary>
        public int Count =>
            enumerator.gradients.Count;

        /// <summary>
        /// Gets the gradient in the specified index
        /// </summary>
        /// <param name="index">Index of a gradient</param>
        /// <returns>A gradient instance</returns>
        public ColorGradient this[int index] =>
            enumerator.gradients[index];

        internal ColorGradients()
        { }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
