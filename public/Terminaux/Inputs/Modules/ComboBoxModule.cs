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
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Combo box (selection) module
    /// </summary>
    public class ComboBoxModule : InputModule
    {
        /// <summary>
        /// Choices to render
        /// </summary>
        public InputChoiceInfo[] Choices { get; set; } = [];

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            InputChoiceInfo? choice = Value is not null ? Choices[(int)Value] : null;
            string valueString = choice?.ChoiceName ?? "";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($" ▼ {valueString}", width);
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
        public override void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default)
        {
            // TODO: Temporarily use the infobox until reliability is proven.
            int choiceIndex = InfoBoxSelectionColor.WriteInfoBoxSelection(Name, Choices, Description);
            Value = choiceIndex;
        }
    }
}
