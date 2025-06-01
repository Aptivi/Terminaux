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
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Inputs.Modules
{
    /// <summary>
    /// Time box module
    /// </summary>
    public class TimeBoxModule : InputModule
    {
        /// <inheritdoc/>
        public override string RenderInput(int width)
        {
            // Render an input text box with selected value and blanks as underscores.
            DateTimeOffset value = Value is DateTimeOffset valueTime ? valueTime : DateTimeOffset.Now;
            string valueString = $"{value}";
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
        public override void ProcessInput(Coordinate inputPopoverPos = default, Size inputPopoverSize = default)
        {
            if (inputPopoverPos == default || inputPopoverSize == default)
            {
                // Use the input info box, since the caller needs to provide info about the popover, which doesn't exist
                DateTimeOffset value = Value is DateTimeOffset valueTime ? valueTime : DateTimeOffset.Now;
                Value = InfoBoxTimeColor.WriteInfoBoxTime(value, Description, new InfoBoxSettings()
                {
                    Title = Name,
                    ForegroundColor = Foreground,
                    BackgroundColor = Background,
                });
            }
            else
            {
                bool bail = false;
                bool cancel = false;
                TimeInputMode inputMode = TimeInputMode.Hours;
                DateTimeOffset value = Value is DateTimeOffset valueTime ? valueTime : DateTimeOffset.Now;
                while (!bail)
                {
                    // Clear the popover. A slider will appear on the input.
                    var boxBuffer = new StringBuilder();
                    int maxTimeWidth = inputPopoverSize.Width;
                    int maxTimePartWidth = (maxTimeWidth - 4) / 3 - 2;
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(new(' ', maxTimeWidth), inputPopoverPos.X, inputPopoverPos.Y, Foreground, Background)
                    );

                    // Write hours
                    string hours = value.Hour.ToString();
                    int hoursPos = inputPopoverPos.X + maxTimePartWidth / 2 - ConsoleChar.EstimateFullWidths(hours);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(hours, hoursPos, inputPopoverPos.Y, inputMode == TimeInputMode.Hours ? ConsoleColors.Lime : Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", inputPopoverPos.X, inputPopoverPos.Y, Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", inputPopoverPos.X + maxTimePartWidth + 1, inputPopoverPos.Y, Foreground, Background)
                    );

                    // Write minutes
                    string minutes = value.Minute.ToString();
                    int minutesPos = inputPopoverPos.X + maxTimePartWidth + maxTimePartWidth / 2 + 3 - ConsoleChar.EstimateFullWidths(minutes);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(minutes, minutesPos, inputPopoverPos.Y, inputMode == TimeInputMode.Minutes ? ConsoleColors.Lime : Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", inputPopoverPos.X + maxTimePartWidth + 3, inputPopoverPos.Y, Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", inputPopoverPos.X + maxTimePartWidth * 2 + 4, inputPopoverPos.Y, Foreground, Background)
                    );

                    // Write seconds
                    string seconds = value.Second.ToString();
                    int secondsPos = inputPopoverPos.X + maxTimePartWidth * 2 + maxTimePartWidth / 2 + 7 - ConsoleChar.EstimateFullWidths(seconds);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(seconds, secondsPos, inputPopoverPos.Y, inputMode == TimeInputMode.Seconds ? ConsoleColors.Lime : Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", inputPopoverPos.X + maxTimePartWidth * 2 + 6, inputPopoverPos.Y, Foreground, Background) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", inputPopoverPos.X + maxTimePartWidth * 3 + 8, inputPopoverPos.Y, Foreground, Background)
                    );

                    // Write the whole popover to replace the input field
                    TextWriterRaw.WriteRaw(boxBuffer.ToString());

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                TimeDateInputTools.ValueGoDown(ref value, inputMode);
                                break;
                            case ConsoleKey.RightArrow:
                                TimeDateInputTools.ValueGoUp(ref value, inputMode);
                                break;
                            case ConsoleKey.Home:
                                TimeDateInputTools.ValueFirst(ref value, inputMode);
                                break;
                            case ConsoleKey.End:
                                TimeDateInputTools.ValueLast(ref value, inputMode);
                                break;
                            case ConsoleKey.Tab:
                                if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                                    TimeDateInputTools.ChangeInputMode(ref inputMode, true);
                                else
                                    TimeDateInputTools.ChangeInputMode(ref inputMode);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                        }
                    }
                }
                if (!cancel)
                    Value = value;
            }
            Provided = true;
        }
    }
}
