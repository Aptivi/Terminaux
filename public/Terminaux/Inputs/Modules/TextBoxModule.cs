//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Reader;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Text box module
    /// </summary>
    public class TextBoxModule : InputModule
    {
        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            string valueString = Value?.ToString() ?? "";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords(valueString, width);
            valueString = wrappedValue[0];

            // Determine how many underscores we need to render
            int valueWidth = ConsoleChar.EstimateCellWidth(valueString);
            int diffWidth = width - valueWidth;
            string underscores = new('_', diffWidth);

            // Render the text box contents now
            string textBox =
                ConsoleColoring.RenderSetConsoleColor(Foreground) +
                ConsoleColoring.RenderSetConsoleColor(Background, true) +
                valueString +
                ConsoleColoring.RenderSetConsoleColor(BlankForeground) +
                underscores +
                ConsoleColoring.RenderRevertForeground() +
                ConsoleColoring.RenderRevertBackground();
            return textBox;
        }

        /// <inheritdoc/>
        public override void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default)
        {
            string value = Value is string valueStr ? valueStr : "";
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                Value = InfoBoxInputColor.WriteInfoBoxInput(value, Description, new InfoBoxSettings()
                {
                    Title = Name,
                    ForegroundColor = Foreground,
                    BackgroundColor = Background,
                });
            }
            else
            {
                // Render the popover. In this case, the whole input will be replaced with the text box that users can enter
                // input on
                TextWriterRaw.WriteRaw(
                    CsiSequences.GenerateCsiCursorPosition(inputPopoverPos.X + 1, inputPopoverPos.Y + 1) +
                    (UseColor ? ConsoleColoring.RenderSetConsoleColor(Foreground) : "") +
                    (UseColor ? ConsoleColoring.RenderSetConsoleColor(Background, true) : "")
                );

                // Wait until the user presses any key to close the box
                var readerSettings = new TermReaderSettings()
                {
                    Width = inputPopoverSize.Width,
                    WriteDefaultValue = true
                };
                if (UseColor)
                {
                    readerSettings.InputForegroundColor = Foreground;
                    readerSettings.InputBackgroundColor = Background;
                }
                Value = TermReader.Read("", value, readerSettings, false, true);
            }
            Provided = true;
        }
    }
}
