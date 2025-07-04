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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terminaux.Base;
using Terminaux.Colors.Data;

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
                ConsoleLogger.Warning("Gradients in one step is not possible.");
                var source = new ColorGradient(1, sourceColor);
                var target = new ColorGradient(2, targetColor);
                gradients.enumerator.gradients.Add(source);
                gradients.enumerator.gradients.Add(target);
                return gradients;
            }

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
            ConsoleLogger.Debug("{0} steps: {1}, {2}, {3}", steps, colorRedSteps, colorGreenSteps, colorBlueSteps);
            for (int x = 0; x <= steps; x++)
            {
                // Make a new instance of Color to indicate the intermediate gradient color
                var currentColorInstance = new Color($"{Convert.ToInt32(currentColorRed)};{Convert.ToInt32(currentColorGreen)};{Convert.ToInt32(currentColorBlue)}");
                var gradient = new ColorGradient(x + 1, currentColorInstance);
                gradients.enumerator.gradients.Add(gradient);
                ConsoleLogger.Info("Adding gradient with levels {0}, {1}, {2}...", currentColorRed, currentColorGreen, currentColorBlue);

                // Change the colors
                currentColorRed -= colorRedSteps;
                currentColorGreen -= colorGreenSteps;
                currentColorBlue -= colorBlueSteps;
            }

            // Return the final instance
            return gradients;
        }

        /// <summary>
        /// Gets the collection of color gradients
        /// </summary>
        /// <param name="colors">Transitioning colors with fractional percentage of position (automatically sorted, must be from 0.0 to 1.0)</param>
        /// <param name="ending">Ending color</param>
        /// <param name="steps">Number of steps to advance</param>
        /// <returns>An instance of <see cref="ColorGradients"/> that you can enumerate.</returns>
        public static ColorGradients GetGradients((double, Color)[] colors, Color ending, int steps)
        {
            // Sanity check
            if (colors.Length == 0)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_GRADIENT_EXCEPTION_COLORNEEDED"));

            // Some variables
            ColorGradients gradients = new();

            // Check the values
            if (colors.Length == 1)
            {
                ConsoleLogger.Warning("Gradients in one color is not possible.");
                var gradient = new ColorGradient(1, colors[0].Item2);
                gradients.enumerator.gradients.Add(gradient);
                return gradients;
            }
            if (steps <= 0)
                steps = 1;
            if (steps == 1)
            {
                ConsoleLogger.Warning("Gradients in one step is not possible.");
                var source = new ColorGradient(1, colors[0].Item2);
                var target = new ColorGradient(2, colors[colors.Length - 1].Item2);
                gradients.enumerator.gradients.Add(source);
                gradients.enumerator.gradients.Add(target);
                return gradients;
            }

            // Determine the step ranges according to the number of steps and color position, but we need to sort the
            // fractional positions first in case we encounter starts that are bigger than ends to keep the gradients
            // sane, then, we'll check their values one by one.
            colors = [.. colors.OrderBy((tuple) => tuple.Item1)];
            for (int i = 0; i < colors.Length; i++)
            {
                var colorTuple = colors[i];
                var nextColorTuple = i + 1 < colors.Length ? colors[i + 1] : (1, ending);
                double colorPos = colorTuple.Item1;
                double nextColorPos = nextColorTuple.Item1;
                int targetSteps = (int)Math.Round((nextColorPos - colorPos) * steps) - 1;
                targetSteps = targetSteps < 0 ? 0 : targetSteps;
                var gradient = GetGradients(colorTuple.Item2, nextColorTuple.Item2, targetSteps);
                gradients.enumerator.gradients.AddRange(gradient);
            }
            return gradients;
        }

        /// <summary>
        /// Get shades of colors (to the darkest)
        /// </summary>
        /// <param name="color">Color to be shaded</param>
        /// <param name="steps">Steps for shades</param>
        /// <returns>A gradient collection instance containing shade information</returns>
        public static ColorGradients GetShades(Color color, int steps = 10) =>
            GetGradients(color, ConsoleColors.Black, steps);

        /// <summary>
        /// Get tints of colors (to the brightest)
        /// </summary>
        /// <param name="color">Color to be tinted</param>
        /// <param name="steps">Steps for tints</param>
        /// <returns>A gradient collection instance containing tint information</returns>
        public static ColorGradients GetTints(Color color, int steps = 10) =>
            GetGradients(color, ConsoleColors.White, steps);

        /// <summary>
        /// Gets a color that represents a stage
        /// </summary>
        /// <param name="low">Low stage color (that is, indicates low level)</param>
        /// <param name="mid">Middle stage color (that is, indicates middle level)</param>
        /// <param name="high">High stage color (that is, indicates high level)</param>
        /// <param name="minLevel">Minimum level</param>
        /// <param name="currentLevel">Current level</param>
        /// <param name="maxLevel">Maximum level</param>
        /// <param name="smooth">Whether to use gradients or one of the three static colors</param>
        /// <param name="lowLevel">Low level position where the middle color starts</param>
        /// <param name="midLevel">Middle level position where the high color starts</param>
        /// <returns></returns>
        public static Color StageLevelSmooth(Color low, Color mid, Color high, double currentLevel, double minLevel = 0, double maxLevel = 1, bool smooth = false, double lowLevel = .33, double midLevel = .66)
        {
            // Swap the values if any
            double finalMinLevel = Math.Min(minLevel, maxLevel);
            double finalMaxLevel = Math.Max(minLevel, maxLevel);

            // Check the current level
            double CheckLevel(double level)
            {
                if (level < finalMinLevel)
                    level = finalMinLevel;
                if (level > finalMaxLevel)
                    level = finalMaxLevel;
                return level;
            }
            currentLevel = CheckLevel(currentLevel);

            // Check the low, mid, and high levels
            lowLevel = CheckLevel(lowLevel);
            midLevel = CheckLevel(midLevel);

            // Build the gradient or determine the color level
            Color level;
            if (smooth)
            {
                // Create gradient list
                var gradients = GetGradients([(0, low), (lowLevel, mid), (midLevel, high)], high, 100);

                // Get the color according to the step that represents the level
                int levelIdx = (int)(currentLevel * 100 / finalMaxLevel);
                level = gradients[levelIdx].IntermediateColor;
                ConsoleLogger.Debug("Got color {0} from level index {1} / {2}.", level.RGB.ToString(), levelIdx, finalMaxLevel);
            }
            else
            {
                // Get the color according to the current level
                ConsoleLogger.Debug("Level: {0} (comparing against low {1} and mid {2})", currentLevel, lowLevel, midLevel);
                if (currentLevel >= midLevel)
                    level = high;
                else if (currentLevel >= lowLevel)
                    level = mid;
                else
                    level = low;
            }

            // Return the representative color
            return level;
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
