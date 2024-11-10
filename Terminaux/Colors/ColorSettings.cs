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

using Terminaux.Base;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors
{
    /// <summary>
    /// The color settings class
    /// </summary>
    public class ColorSettings
    {
        private int _alpha = 255;
        private Color? _opacityColor = null;

        /// <summary>
        /// List of transformations that will be used when making a new instance of <see cref="Color"/>
        /// </summary>
        public BaseTransformationFormula[] Transformations { get; set; } = [];

        /// <summary>
        /// If enabled, calls to <see cref="Color.PlainSequence"/> and its siblings return color ID if said color is either a 256 color or a 16 color.
        /// Otherwise, calls to these properties are wrappers to <see cref="Color.PlainSequenceTrueColor"/> and its siblings. By default, it's enabled.
        /// </summary>
        public bool UseTerminalPalette { get; set; } = true;

        /// <summary>
        /// The opacity at which the color will be calculated.
        /// </summary>
        /// <remarks>
        /// This fades the color to the current background color of the console currently set by
        /// <see cref="ColorTools.SetConsoleColor(Color, bool, bool, bool)"/>. That color can be overridable by
        /// setting the <see cref="OpacityColor"/> property to any color you want. Set it to 0, and you'll get the
        /// color that is the same as the value of the <see cref="OpacityColor"/> property. Set it to 255, and you'll
        /// get the color that you've created using the <see cref="Color(int, int, int, ColorSettings)"/> constructor
        /// and its siblings.
        /// <br></br>
        /// <br></br>
        /// This is still fake transparency for GUI applications, but should behave like real transparency in console
        /// applications that use background colors.
        /// </remarks>
        public int Opacity
        {
            get => _alpha;
            set
            {
                // Check the ranges
                if (value < 0)
                    value = 0;
                if (value > 255)
                    value = 255;

                // Now, set it!
                _alpha = value;
            }
        }

        /// <summary>
        /// The opacity color to fade the new instances of colors to using the opacity as the threshold.
        /// </summary>
        /// <remarks>
        /// See the Remarks section of <see cref="Opacity"/> for more info about how we use this color to calculate
        /// the transparency.
        /// </remarks>
        public Color OpacityColor
        {
            get => _opacityColor ?? ColorTools.CurrentBackgroundColor;
            set => _opacityColor = value ?? ColorTools.CurrentBackgroundColor;
        }

        /// <summary>
        /// Makes a new instance of color settings
        /// </summary>
        public ColorSettings()
        { }

        /// <summary>
        /// Makes a new instance of color settings
        /// </summary>
        /// <param name="settings">Settings instance to copy the color configuration from</param>
        public ColorSettings(ColorSettings settings)
        {
            if (settings is null)
                return;

            Transformations = settings.Transformations;
            UseTerminalPalette = settings.UseTerminalPalette;
            Opacity = settings.Opacity;
            OpacityColor = settings.OpacityColor;
        }
    }
}
