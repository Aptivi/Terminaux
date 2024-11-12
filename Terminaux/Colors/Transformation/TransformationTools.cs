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
using System.Collections.Generic;
using System.Linq;
using Terminaux.Base;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation.Formulas;

namespace Terminaux.Colors.Transformation
{
    /// <summary>
    /// Color transformation tools
    /// </summary>
    public static class TransformationTools
    {
        internal static readonly Dictionary<TransformationFormula, BaseTransformationFormula> formulas = new()
        {
            { TransformationFormula.Monochromacy, new Monochromacy() },
            { TransformationFormula.Inverse, new Inverse() },
            { TransformationFormula.Protan, new Protan() },
            { TransformationFormula.Deutan, new Deutan() },
            { TransformationFormula.Tritan, new Tritan() },
            { TransformationFormula.ProtanVienot, new ProtanVienot() },
            { TransformationFormula.DeutanVienot, new DeutanVienot() },
            { TransformationFormula.TritanVienot, new TritanVienot() },
            { TransformationFormula.BlueScale, new BlueScale() },
            { TransformationFormula.GreenScale, new GreenScale() },
            { TransformationFormula.RedScale, new RedScale() },
            { TransformationFormula.YellowScale, new YellowScale() },
            { TransformationFormula.AquaScale, new AquaScale() },
            { TransformationFormula.PinkScale, new PinkScale() },
        };

        /// <summary>
        /// Converts from sRGB to Linear RGB using a color number
        /// </summary>
        /// <param name="colorNum">Color number from 0 to 255</param>
        /// <returns>Linear RGB number ranging from 0 to 1</returns>
        public static double SRGBToLinearRGB(int colorNum)
        {
            // Check the value
            if (colorNum < 0)
                colorNum = 0;
            if (colorNum > 255)
                colorNum = 255;

            // Now, convert sRGB to linear RGB (domain is [0, 1])
            double colorNumDbl = colorNum / 255d;
            if (colorNumDbl < 0.04045d)
                return colorNumDbl / 12.92d;
            return Math.Pow((colorNumDbl + 0.055d) / 1.055d, 2.4d);
        }

        /// <summary>
        /// Converts from Linear RGB to sRGB using a linear RGB number
        /// </summary>
        /// <param name="linear">Linear RGB number from 0 to 1</param>
        /// <returns>sRGB value from 0 to 255</returns>
        public static int LinearRGBTosRGB(double linear)
        {
            // Check the value
            if (linear <= 0)
                return 0;
            if (linear >= 1)
                return 255;

            // Now, convert linear value to RGB representation (domain is [0, 255])
            if (linear < 0.0031308d)
                return (int)(0.5d + (linear * 255d * 12.92));
            return (int)(255d * (Math.Pow(linear, 1d / 2.4d) * 1.055d - 0.055d));
        }

        /// <summary>
        /// Blends the two colors together
        /// </summary>
        /// <param name="source">Source color to be blended</param>
        /// <param name="target">Target color to blend</param>
        /// <param name="factor">Blending factor [0.0 to 1.0]</param>
        /// <returns>A color instance that represents a source color blended with the target color.</returns>
        public static Color BlendColor(Color source, Color target, double factor = 0.5) =>
            new(
                (byte)(source.RGB.R + ((target.RGB.R - source.RGB.R) * factor)),
                (byte)(source.RGB.G + ((target.RGB.G - source.RGB.G) * factor)),
                (byte)(source.RGB.B + ((target.RGB.B - source.RGB.B) * factor))
            );

        /// <summary>
        /// Gets the luminance of the color
        /// </summary>
        /// <param name="color">Color to obtain luminance from</param>
        /// <returns>Luminance level</returns>
        public static double GetLuminance(Color color)
        {
            double luminanceR = SRGBToLinearRGB(color.RGB.R);
            double luminanceG = SRGBToLinearRGB(color.RGB.G);
            double luminanceB = SRGBToLinearRGB(color.RGB.B);
            return 0.2126 * luminanceR + 0.7152 * luminanceG + 0.0722 * luminanceB;
        }

        /// <summary>
        /// Gets the contrast between two colors
        /// </summary>
        /// <param name="firstColor">First color (usually foreground)</param>
        /// <param name="secondColor">Second color (usually background)</param>
        /// <returns>Contrast ratio</returns>
        public static double GetContrast(Color firstColor, Color secondColor)
        {
            double luminanceFirst = GetLuminance(firstColor);
            double luminanceSecond = GetLuminance(secondColor);
            double min = Math.Min(luminanceFirst, luminanceSecond);
            double max = Math.Max(luminanceFirst, luminanceSecond);
            return (max + 0.05) / (min + 0.05);
        }

        /// <summary>
        /// Gets the colorized dark background from the source color
        /// </summary>
        /// <param name="source">Source color to darken</param>
        /// <returns>A copy of the source color with darkness applied that suits the background</returns>
        public static Color GetDarkBackground(Color source)
        {
            int targetR = (int)(source.RGB.R / 4d);
            int targetG = (int)(source.RGB.G / 4d);
            int targetB = (int)(source.RGB.B / 4d);
            return new(targetR, targetG, targetB);
        }

        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput, ColorSettings settings)
        {
            (int r, int g, int b) = (rInput, gInput, bInput);
            if (settings.Transformations.Length > 0)
            {
                // We'll transform.
                foreach (var transform in settings.Transformations)
                    (r, g, b) = transform.Transform(r, g, b);
            }
            return (r, g, b);
        }
    }
}
