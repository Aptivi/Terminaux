﻿//
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
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Slider box module
    /// </summary>
    public class SliderBoxModule : InputModule
    {
        /// <summary>
        /// Minimum position
        /// </summary>
        public int MinPos { get; set; }

        /// <summary>
        /// Maximum position
        /// </summary>
        public int MaxPos { get; set; }

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            int value = Value is int valueInt ? valueInt : MinPos;
            string valueString = $"{value} of {MinPos}/{MaxPos}";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($" ◎ {valueString}", width);
            valueString = wrappedValue[0];

            // Determine how many underscores we need to render
            int valueWidth = ConsoleChar.EstimateCellWidth(valueString);
            int diffWidth = width - valueWidth;
            string underscores = new('_', diffWidth);

            // Render the text box contents now
            string textBox =
                ColorTools.RenderSetConsoleColor(Foreground) +
                ColorTools.RenderSetConsoleColor(Background, true) +
                valueString +
                ColorTools.RenderSetConsoleColor(BlankForeground) +
                underscores +
                ColorTools.RenderRevertForeground() +
                ColorTools.RenderRevertBackground();
            return textBox;
        }

        /// <inheritdoc/>
        public override void ProcessInput()
        {
            // TODO: Temporarily use the infobox until reliability is proven.
            int value = Value is int valueInt ? valueInt : MinPos;
            int choiceIndex = InfoBoxSliderColor.WriteInfoBoxSlider(Name, value, MaxPos, Description, MinPos);
            Value = choiceIndex;
        }
    }
}
