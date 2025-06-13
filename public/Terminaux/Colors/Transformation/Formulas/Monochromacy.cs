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
using Terminaux.Base;
using Terminaux.Colors.Transformation.Tools;

namespace Terminaux.Colors.Transformation.Formulas
{
    /// <summary>
    /// Full color blindness, can only see grayscale.
    /// </summary>
    public class Monochromacy : BaseTransformationFormula, ITransformationFormula
    {
        /// <summary>
        /// Monochromacy type
        /// </summary>
        public MonochromacyType Type { get; set; } = MonochromacyType.Monochrome;

        /// <inheritdoc/>
        public override (int, int, int) Transform(int r, int g, int b)
        {
            // Check values
            if (r < 0 || r > 255)
                throw new ArgumentOutOfRangeException("r");
            if (g < 0 || g > 255)
                throw new ArgumentOutOfRangeException("g");
            if (b < 0 || b > 255)
                throw new ArgumentOutOfRangeException("b");

            // Transform the color linear values by applying monochromacy
            int sMono = (int)TransformationTools.GetLuminance(r, g, b, true);
            (int r, int g, int b) mono = Type switch
            {
                MonochromacyType.Monochrome =>  (sMono, sMono, sMono),
                MonochromacyType.Red =>         (sMono, 0,     0    ),
                MonochromacyType.Green =>       (0,     sMono, 0    ),
                MonochromacyType.Blue =>        (0,     0,     sMono),
                MonochromacyType.Cyan =>        (0,     sMono, sMono),
                MonochromacyType.Magenta =>     (sMono, 0,     sMono),
                MonochromacyType.Yellow =>      (sMono, sMono, 0    ),
                _ => throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_FORMULAS_EXCEPTION_INVALIDMONOCHROMACYTYPE")),
            };
            var final = TransformationTools.BlendColor((r, g, b), (mono.r, mono.g, mono.b), Frequency);
            ConsoleLogger.Debug("Transformed to {0} using parameters: freq: {1}, blend: {2}", (final.RGB.R, final.RGB.G, final.RGB.B), Frequency, (mono.r, mono.g, mono.b));
            return (final.RGB.R, final.RGB.G, final.RGB.B);
        }
    }
}
