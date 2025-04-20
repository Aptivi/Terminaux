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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Masked text box module
    /// </summary>
    public class MaskedTextBoxModule : InputModule
    {
        /// <summary>
        /// Password mask character
        /// </summary>
        public char Mask { get; set; } = TermReader.GlobalReaderSettings.PasswordMaskChar;

        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            string valueString = Value?.ToString() ?? "";
            valueString = new(Mask, valueString.Length);
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords(valueString, width);
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
            string value = Value is string valueStr ? valueStr : "";
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                Value = InfoBoxInputPasswordColor.WriteInfoBoxInputPasswordColorBack(Name, Description, Foreground, Background);
            }
            else
            {
                // Render the popover. In this case, the whole input will be replaced with the text box that users can enter
                // input on
                TextWriterRaw.WriteRaw(
                    CsiSequences.GenerateCsiCursorPosition(inputPopoverPos.X + 1, inputPopoverPos.Y + 1) +
                    (UseColor ? ColorTools.RenderSetConsoleColor(Foreground) : "") +
                    (UseColor ? ColorTools.RenderSetConsoleColor(Background, true) : "")
                );

                // Wait until the user presses any key to close the box
                var readerSettings = new TermReaderSettings()
                {
                    RightMargin = ConsoleWrapper.WindowWidth - (inputPopoverPos.X + inputPopoverSize.Width) - 1,
                };
                if (UseColor)
                {
                    readerSettings.InputForegroundColor = Foreground;
                    readerSettings.InputBackgroundColor = Background;
                }
                Value = TermReader.Read("", value, readerSettings, true, true);
            }
        }
    }
}
