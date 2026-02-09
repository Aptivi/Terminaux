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

using System;
using System.Text;
using Terminaux.Base.Extensions;
using Terminaux.Base.Structures;
using Colorimetry.Data;
using Terminaux.Inputs.Pointer;
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
            string valueString = $"{value:T}";
            string[] wrappedValue = ConsoleMisc.GetWrappedSentencesByWords($"▼ {valueString}", width);
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
                    int timeArrowTop = inputPopoverPos.Y;
                    int timeArrowHoursLeft = inputPopoverPos.X;
                    int timeArrowHoursRight = timeArrowHoursLeft + maxTimePartWidth + 1;
                    int timeArrowMinutesLeft = timeArrowHoursLeft + maxTimePartWidth + 3;
                    int timeArrowMinutesRight = timeArrowHoursLeft + maxTimePartWidth * 2 + 4;
                    int timeArrowSecondsLeft = timeArrowHoursLeft + maxTimePartWidth * 2 + 6;
                    int timeArrowSecondsRight = timeArrowHoursLeft + maxTimePartWidth * 3 + 8;
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Make hitboxes for arrow presses
                        var arrowTimeDecreaseHoursHitbox = new PointerHitbox(new(timeArrowHoursLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.HoursModify(ref value, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowTimeIncreaseHoursHitbox = new PointerHitbox(new(timeArrowHoursRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.HoursModify(ref value, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowTimeDecreaseMinutesHitbox = new PointerHitbox(new(timeArrowMinutesLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.MinutesModify(ref value, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowTimeIncreaseMinutesHitbox = new PointerHitbox(new(timeArrowMinutesRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.MinutesModify(ref value, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowTimeDecreaseSecondsHitbox = new PointerHitbox(new(timeArrowSecondsLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.SecondsModify(ref value, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                        var arrowTimeIncreaseSecondsHitbox = new PointerHitbox(new(timeArrowSecondsRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.SecondsModify(ref value, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (PointerTools.PointerWithinRange(mouse, (timeArrowHoursLeft, timeArrowTop), (timeArrowSecondsRight, timeArrowTop)))
                                {
                                    if (PointerTools.PointerWithinRange(mouse, (timeArrowHoursLeft, timeArrowTop), (timeArrowHoursRight, timeArrowTop)))
                                        TimeDateInputTools.HoursModify(ref value, ref inputMode, true);
                                    else if (PointerTools.PointerWithinRange(mouse, (timeArrowMinutesLeft, timeArrowTop), (timeArrowMinutesRight, timeArrowTop)))
                                        TimeDateInputTools.MinutesModify(ref value, ref inputMode, true);
                                    else if (PointerTools.PointerWithinRange(mouse, (timeArrowSecondsLeft, timeArrowTop), (timeArrowSecondsRight, timeArrowTop)))
                                        TimeDateInputTools.SecondsModify(ref value, ref inputMode, true);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (PointerTools.PointerWithinRange(mouse, (timeArrowHoursLeft, timeArrowTop), (timeArrowSecondsRight, timeArrowTop)))
                                {
                                    if (PointerTools.PointerWithinRange(mouse, (timeArrowHoursLeft, timeArrowTop), (timeArrowHoursRight, timeArrowTop)))
                                        TimeDateInputTools.HoursModify(ref value, ref inputMode);
                                    else if (PointerTools.PointerWithinRange(mouse, (timeArrowMinutesLeft, timeArrowTop), (timeArrowMinutesRight, timeArrowTop)))
                                        TimeDateInputTools.MinutesModify(ref value, ref inputMode);
                                    else if (PointerTools.PointerWithinRange(mouse, (timeArrowSecondsLeft, timeArrowTop), (timeArrowSecondsRight, timeArrowTop)))
                                        TimeDateInputTools.SecondsModify(ref value, ref inputMode);
                                }
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowTimeIncreaseHoursHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseHoursHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseHoursHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseHoursHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowTimeIncreaseMinutesHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseMinutesHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseMinutesHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseMinutesHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowTimeIncreaseSecondsHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseSecondsHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseSecondsHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseSecondsHitbox.ProcessPointer(mouse, out done);
                                }
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
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
