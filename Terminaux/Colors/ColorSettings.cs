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
using Terminaux.Base;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors
{
    /// <summary>
    /// The color settings class
    /// </summary>
    public class ColorSettings
    {
        private double _blindnessSeverity = 0.6;

        /// <summary>
        /// Enables the color transformation to adjust to color blindness upon making a new instance of color
        /// </summary>
        public bool EnableColorTransformation { get; set; } = false;
        /// <summary>
        /// If enabled, calls to <see cref="Color.PlainSequence"/> and its siblings return color ID if said color is either a 256 color or a 16 color.
        /// Otherwise, calls to these properties are wrappers to <see cref="Color.PlainSequenceTrueColor"/> and its siblings. By default, it's enabled.
        /// </summary>
        public bool UseTerminalPalette { get; set; } = true;

        /// <summary>
        /// The color transformation formula to use when generating transformed colors, such as color blindness.
        /// </summary>
        public TransformationFormula ColorTransformationFormula { get; set; } = TransformationFormula.Protan;
        /// <summary>
        /// The color transformation method for color blindness.
        /// </summary>
        public TransformationMethod ColorTransformationMethod { get; set; } = TransformationMethod.Brettel1997;

        /// <summary>
        /// The color blindness severity (Only for color blindness formulas):<br></br>
        ///   - <see cref="TransformationFormula.Protan"/><br></br>
        ///   - <see cref="TransformationFormula.Deutan"/><br></br>
        ///   - <see cref="TransformationFormula.Tritan"/>
        /// </summary>
        public double ColorBlindnessSeverity
        {
            get => _blindnessSeverity;
            set
            {
                if (value < 0)
                    throw new TerminauxException("Blindness severity should not be less than zero.");
                if (value > 1)
                    throw new TerminauxException("Blindness severity should not be greater than one.");

                _blindnessSeverity = value;
            }
        }
    }
}
