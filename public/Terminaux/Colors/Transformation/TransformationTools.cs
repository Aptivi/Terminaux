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
using System.Collections.Generic;
using Terminaux.Base;
using Terminaux.Colors.Transformation.Formulas;

namespace Terminaux.Colors.Transformation
{
    /// <summary>
    /// Color transformation tools
    /// </summary>
    public static class TransformationTools
    {
        private static readonly Dictionary<TransformationFormula, BaseTransformationFormula> formulas = new()
        {
            { TransformationFormula.Monochromacy, new Monochromacy() },
            { TransformationFormula.Inverse, new Inverse() },
            { TransformationFormula.Protan, new BrettelColorBlind() },
            { TransformationFormula.Deutan, new BrettelColorBlind() },
            { TransformationFormula.Tritan, new BrettelColorBlind() },
            { TransformationFormula.ProtanVienot, new VienotColorBlind() },
            { TransformationFormula.DeutanVienot, new VienotColorBlind() },
            { TransformationFormula.TritanVienot, new VienotColorBlind() },
            { TransformationFormula.BlueScale, new BlueScale() },
            { TransformationFormula.GreenScale, new GreenScale() },
            { TransformationFormula.RedScale, new RedScale() },
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
        /// Provides you an easy way to generate new <see cref="Color"/> instances with color blindness applied
        /// </summary>
        /// <param name="color">Color to use</param>
        /// <param name="formula">Selected formula for color blindness</param>
        /// <param name="severity">Severity of the color blindness</param>
        /// <returns>An instance of <see cref="Color"/> with adjusted color values for color-blindness</returns>
        public static Color RenderColorBlindnessAware(Color color, TransformationFormula formula, double severity)
        {
            // Get the resulting color
            var settings = new ColorSettings()
            {
                EnableColorTransformation = true,
                ColorBlindnessSeverity = severity,
                ColorTransformationFormula = formula,
            };
            var result = new Color(color.PlainSequence, settings);

            // Return the resulting color
            return result;
        }

        internal static BaseTransformationFormula GetTransformationFormula(TransformationFormula formula)
        {
            if (!formulas.TryGetValue(formula, out var result))
                throw new TerminauxException("Transformation formula {0} not found.", formula);
            return result;
        }

        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput, ColorSettings settings)
        {
            if (settings.EnableColorTransformation)
            {
                // We'll transform.
                var formula = GetTransformationFormula(settings.ColorTransformationFormula);
                return GetTransformedColor(formula, rInput, gInput, bInput, settings);
            }
            return (rInput, gInput, bInput);
        }

        internal static (int r, int g, int b) GetTransformedColor(BaseTransformationFormula formula, int rInput, int gInput, int bInput, ColorSettings settings)
        {
            if (settings.EnableColorTransformation)
            {
                // We'll transform.
                var transformed = formula.Transform(rInput, gInput, bInput, settings);
                return transformed;
            }
            return (rInput, gInput, bInput);
        }
    }
}
