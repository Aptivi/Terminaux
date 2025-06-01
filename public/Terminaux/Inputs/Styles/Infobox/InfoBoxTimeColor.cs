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
using Terminaux.Colors;
using System.Text;
using Terminaux.Base.Buffered;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;
using Terminaux.Base.Checks;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Inputs.Styles.Infobox
{
    /// <summary>
    /// Info box writer with time and color support
    /// </summary>
    public static class InfoBoxTimeColor
    {
        private static readonly Keybinding[] keybindings =
        [
            new Keybinding("Decrements the current value", ConsoleKey.UpArrow),
            new Keybinding("Increments the current value", ConsoleKey.DownArrow),
            new Keybinding("Sets the value to the minimum value", ConsoleKey.Home),
            new Keybinding("Sets the value to the maximum value", ConsoleKey.End),
            new Keybinding("Sets the current focus to the next value element", ConsoleKey.Tab),
            new Keybinding("Sets the current focus to the previous value element", ConsoleKey.Tab, ConsoleModifiers.Shift),
            new Keybinding("Goes one line up", ConsoleKey.W),
            new Keybinding("Goes one line down", ConsoleKey.S),
            new Keybinding("Goes to the previous page of text", ConsoleKey.E),
            new Keybinding("Goes to the next page of text", ConsoleKey.D),
            new Keybinding("Submits the value", ConsoleKey.Enter),
            new Keybinding("Closes without submitting the value", ConsoleKey.Escape),
            new Keybinding("Performs an action or selects a choice", PointerButton.Left),
            new Keybinding("Decrements value or moves three lines of text up", PointerButton.WheelUp),
            new Keybinding("Increments value or moves three lines of text down", PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the time info box
        /// </summary>
        /// <param name="initialTime">Initial time</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static DateTimeOffset WriteInfoBoxTime(DateTimeOffset initialTime, string text, params object[] vars) =>
            WriteInfoBoxTime(initialTime, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the time info box
        /// </summary>
        /// <param name="initialTime">Initial time</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static DateTimeOffset WriteInfoBoxTime(DateTimeOffset initialTime, string text, InfoBoxSettings settings, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            DateTimeOffset selected = initialTime;
            TimeInputMode inputMode = TimeInputMode.Hours;
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxTimeColor), infoBoxScreenPart);
            try
            {
                int currIdx = 0;
                int increment = 0;
                infoBoxScreenPart.AddDynamicText(() =>
                {
                    // Deal with the lines to actually fit text in the infobox
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 1);

                    // Fill the info box with text inside it
                    var boxBuffer = new StringBuilder(
                        InfoBoxTools.RenderText(1, settings.Title, text, settings.BorderSettings, settings.ForegroundColor, settings.BackgroundColor, settings.UseColors, ref increment, currIdx, false, true, vars)
                    );

                    // Now, get the necessary positions and widths for the time parts
                    int timePosX = borderX + 2;
                    int timePosY = borderY + maxHeight - 2;
                    int maxTimeWidth = maxWidth - 4;
                    int maxTimePartWidth = maxTimeWidth / 3 - 2;

                    // Write hours
                    string hours = selected.Hour.ToString();
                    int hoursPos = timePosX + maxTimePartWidth / 2 - ConsoleChar.EstimateFullWidths(hours);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(hours, hoursPos, timePosY + 2, inputMode == TimeInputMode.Hours ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", timePosX, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", timePosX + maxTimePartWidth + 1, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor)
                    );

                    // Write minutes
                    string minutes = selected.Minute.ToString();
                    int minutesPos = timePosX + maxTimePartWidth + maxTimePartWidth / 2 + 3 - ConsoleChar.EstimateFullWidths(minutes);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(minutes, minutesPos, timePosY + 2, inputMode == TimeInputMode.Minutes ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", timePosX + maxTimePartWidth + 3, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", timePosX + maxTimePartWidth * 2 + 4, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor)
                    );

                    // Write seconds
                    string seconds = selected.Second.ToString();
                    int secondsPos = timePosX + maxTimePartWidth * 2 + maxTimePartWidth / 2 + 7 - ConsoleChar.EstimateFullWidths(seconds);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(seconds, secondsPos, timePosY + 2, inputMode == TimeInputMode.Seconds ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", timePosX + maxTimePartWidth * 2 + 6, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", timePosX + maxTimePartWidth * 3 + 8, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor)
                    );
                    return boxBuffer.ToString();
                });

                // Wait for input
                bool bail = false;
                bool cancel = false;
                while (!bail)
                {
                    // Render the screen
                    ScreenTools.Render();

                    // Handle keypress
                    InputEventInfo data = Input.ReadPointerOrKey();
                    string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
                    var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

                    // Get positions for arrows
                    int arrowLeft = maxWidth + borderX + 1;
                    int arrowTop = 2;
                    int arrowBottom = maxHeight + 1;

                    // Get positions for time buttons
                    int maxTimeWidth = maxWidth - 4;
                    int maxTimePartWidth = maxTimeWidth / 3 - 2;
                    int timeArrowTop = borderY + maxHeight - 1;
                    int timeArrowHoursLeft = borderX + 2;
                    int timeArrowHoursRight = timeArrowHoursLeft + maxTimePartWidth + 1;
                    int timeArrowMinutesLeft = timeArrowHoursLeft + maxTimePartWidth + 3;
                    int timeArrowMinutesRight = timeArrowHoursLeft + maxTimePartWidth * 2 + 4;
                    int timeArrowSecondsLeft = timeArrowHoursLeft + maxTimePartWidth * 2 + 6;
                    int timeArrowSecondsRight = timeArrowHoursLeft + maxTimePartWidth * 3 + 8;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => GoDown(ref currIdx, text, vars))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseHoursHitbox = new PointerHitbox(new(timeArrowHoursLeft, timeArrowTop), new Action<PointerEventContext>((_) => HoursModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseHoursHitbox = new PointerHitbox(new(timeArrowHoursRight, timeArrowTop), new Action<PointerEventContext>((_) => HoursModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseMinutesHitbox = new PointerHitbox(new(timeArrowMinutesLeft, timeArrowTop), new Action<PointerEventContext>((_) => MinutesModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseMinutesHitbox = new PointerHitbox(new(timeArrowMinutesRight, timeArrowTop), new Action<PointerEventContext>((_) => MinutesModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseSecondsHitbox = new PointerHitbox(new(timeArrowSecondsLeft, timeArrowTop), new Action<PointerEventContext>((_) => SecondsModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseSecondsHitbox = new PointerHitbox(new(timeArrowSecondsRight, timeArrowTop), new Action<PointerEventContext>((_) => SecondsModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

                    // Handle input
                    var mouse = data.PointerEventContext;
                    if (mouse is not null)
                    {
                        // Mouse input received.
                        switch (mouse.Button)
                        {
                            case PointerButton.WheelUp:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoUp(ref currIdx, 3);
                                else if (IsMouseWithinTime(text, vars, mouse))
                                {
                                    if (IsMouseWithinHours(text, vars, mouse))
                                        HoursModify(ref selected, ref inputMode, true);
                                    else if (IsMouseWithinMinutes(text, vars, mouse))
                                        MinutesModify(ref selected, ref inputMode, true);
                                    else if (IsMouseWithinSeconds(text, vars, mouse))
                                        SecondsModify(ref selected, ref inputMode, true);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoDown(ref currIdx, text, vars, 3);
                                else if (IsMouseWithinTime(text, vars, mouse))
                                {
                                    if (IsMouseWithinHours(text, vars, mouse))
                                        HoursModify(ref selected, ref inputMode);
                                    else if (IsMouseWithinMinutes(text, vars, mouse))
                                        MinutesModify(ref selected, ref inputMode);
                                    else if (IsMouseWithinSeconds(text, vars, mouse))
                                        SecondsModify(ref selected, ref inputMode);
                                }
                                break;
                            case PointerButton.Left:
                                if (mouse.ButtonPress != PointerButtonPress.Released)
                                    break;
                                if ((arrowUpHitbox.IsPointerWithin(mouse) || arrowDownHitbox.IsPointerWithin(mouse)) && splitFinalLines.Length > maxHeight)
                                {
                                    arrowUpHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowDownHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowTimeIncreaseHoursHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseHoursHitbox.IsPointerWithin(mouse)))
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
                                ValueGoDown(ref selected, inputMode);
                                break;
                            case ConsoleKey.RightArrow:
                                ValueGoUp(ref selected, inputMode);
                                break;
                            case ConsoleKey.Home:
                                ValueFirst(ref selected, inputMode);
                                break;
                            case ConsoleKey.End:
                                ValueLast(ref selected, inputMode);
                                break;
                            case ConsoleKey.E:
                                GoUp(ref currIdx, maxHeight);
                                break;
                            case ConsoleKey.D:
                                GoDown(ref currIdx, text, vars, increment);
                                break;
                            case ConsoleKey.W:
                                GoUp(ref currIdx);
                                break;
                            case ConsoleKey.S:
                                GoDown(ref currIdx, text, vars);
                                break;
                            case ConsoleKey.Tab:
                                if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                                    ChangeInputMode(ref inputMode, true);
                                else
                                    ChangeInputMode(ref inputMode);
                                break;
                            case ConsoleKey.Enter:
                                bail = true;
                                break;
                            case ConsoleKey.Escape:
                                bail = true;
                                cancel = true;
                                break;
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(keybindings);
                                break;
                        }
                    }
                }
                if (cancel)
                    selected = initialTime;
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, $"There is a serious error when printing text. {ex.Message}");
            }
            finally
            {
                if (settings.UseColors)
                {
                    TextWriterRaw.WriteRaw(
                        ColorTools.RenderRevertForeground() +
                        ColorTools.RenderRevertBackground()
                    );
                }
                ConsoleWrapper.CursorVisible = initialCursorVisible;
                ScreenTools.CurrentScreen?.RemoveBufferedPart(infoBoxScreenPart.Id);
                if (initialScreenIsNull)
                    ScreenTools.UnsetCurrent(screen);
            }
            return selected;
        }

        private static bool IsMouseWithinText(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);
            maxHeight -= 2;

            // Check the dimensions
            return PointerTools.PointerWithinRange(mouse, (borderX + 1, borderY + 1), (borderX + maxWidth, borderY + maxHeight));
        }

        private static bool IsMouseWithinTime(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int timeArrowTop = borderY + maxHeight - 1;
            int timeArrowLeft = borderX + 2;
            int timeArrowRight = timeArrowLeft + maxTimeWidth + 1;
            return PointerTools.PointerWithinRange(mouse, (timeArrowLeft, timeArrowTop), (timeArrowRight, timeArrowTop));
        }

        private static bool IsMouseWithinHours(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeHoursLeft = borderX + 3;
            int timeHoursRight = timeHoursLeft + maxTimePartWidth - 1;
            return PointerTools.PointerWithinRange(mouse, (timeHoursLeft, timeTop), (timeHoursRight, timeTop));
        }

        private static bool IsMouseWithinMinutes(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeHoursLeft = borderX + 3;
            int timeMinutesLeft = timeHoursLeft + maxTimePartWidth + 3;
            int timeMinutesRight = timeHoursLeft + maxTimePartWidth * 2 + 2;
            return PointerTools.PointerWithinRange(mouse, (timeMinutesLeft, timeTop), (timeMinutesRight, timeTop));
        }

        private static bool IsMouseWithinSeconds(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeHoursLeft = borderX + 3;
            int timeSecondsLeft = timeHoursLeft + maxTimePartWidth * 2 + 6;
            int timeSecondsRight = timeHoursLeft + maxTimePartWidth * 3 + 6;
            return PointerTools.PointerWithinRange(mouse, (timeSecondsLeft, timeTop), (timeSecondsRight, timeTop));
        }

        private static void GoUp(ref int currIdx, int level = 1)
        {
            currIdx -= level;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void GoDown(ref int currIdx, string text, object[] vars, int level = 1)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (_, maxHeight, _, _, _) = InfoBoxTools.GetDimensions(splitFinalLines, 2);
            currIdx += level;
            if (currIdx > splitFinalLines.Length - maxHeight)
                currIdx = splitFinalLines.Length - maxHeight;
            if (currIdx < 0)
                currIdx = 0;
        }

        private static void ChangeInputMode(ref TimeInputMode inputMode, bool backward = false)
        {
            if (backward)
            {
                inputMode--;
                if (inputMode < TimeInputMode.Hours)
                    inputMode = TimeInputMode.Seconds;
            }
            else
            {
                inputMode++;
                if (inputMode > TimeInputMode.Seconds)
                    inputMode = TimeInputMode.Hours;
            }
        }

        private static void ValueGoDown(ref DateTimeOffset selected, TimeInputMode inputMode)
        {
            switch (inputMode)
            {
                case TimeInputMode.Hours:
                    HoursModify(ref selected, ref inputMode);
                    break;
                case TimeInputMode.Minutes:
                    MinutesModify(ref selected, ref inputMode);
                    break;
                case TimeInputMode.Seconds:
                    SecondsModify(ref selected, ref inputMode);
                    break;
            }
        }

        private static void ValueGoUp(ref DateTimeOffset selected, TimeInputMode inputMode)
        {
            switch (inputMode)
            {
                case TimeInputMode.Hours:
                    HoursModify(ref selected, ref inputMode, true);
                    break;
                case TimeInputMode.Minutes:
                    MinutesModify(ref selected, ref inputMode, true);
                    break;
                case TimeInputMode.Seconds:
                    SecondsModify(ref selected, ref inputMode, true);
                    break;
            }
        }

        private static void HoursModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
        {
            if (inputMode != TimeInputMode.Hours)
                inputMode = TimeInputMode.Hours;
            (int min, int max) = GetMinMax(inputMode);
            int hour = selected.Hour;
            if (goUp)
            {
                hour++;
                if (hour > max)
                    hour = min;
            }
            else
            {
                hour--;
                if (hour < min)
                    hour = max;
            }
            selected = new(selected.Year, selected.Month, selected.Day, hour, selected.Minute, selected.Second, selected.Offset);
        }

        private static void MinutesModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
        {
            if (inputMode != TimeInputMode.Minutes)
                inputMode = TimeInputMode.Minutes;
            (int min, int max) = GetMinMax(inputMode);
            int minute = selected.Minute;
            if (goUp)
            {
                minute++;
                if (minute > max)
                    minute = min;
            }
            else
            {
                minute--;
                if (minute < min)
                    minute = max;
            }
            selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, minute, selected.Second, selected.Offset);
        }

        private static void SecondsModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
        {
            if (inputMode != TimeInputMode.Seconds)
                inputMode = TimeInputMode.Seconds;
            (int min, int max) = GetMinMax(inputMode);
            int second = selected.Second;
            if (goUp)
            {
                second++;
                if (second > max)
                    second = min;
            }
            else
            {
                second--;
                if (second < min)
                    second = max;
            }
            selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, selected.Minute, second, selected.Offset);
        }

        private static void ValueFirst(ref DateTimeOffset selected, TimeInputMode inputMode)
        {
            (int min, _) = GetMinMax(inputMode);
            switch (inputMode)
            {
                case TimeInputMode.Hours:
                    selected = new(selected.Year, selected.Month, selected.Day, min, selected.Minute, selected.Second, selected.Offset);
                    break;
                case TimeInputMode.Minutes:
                    selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, min, selected.Second, selected.Offset);
                    break;
                case TimeInputMode.Seconds:
                    selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, selected.Minute, min, selected.Offset);
                    break;
            }
        }

        private static void ValueLast(ref DateTimeOffset selected, TimeInputMode inputMode)
        {
            (_, int max) = GetMinMax(inputMode);
            switch (inputMode)
            {
                case TimeInputMode.Hours:
                    selected = new(selected.Year, selected.Month, selected.Day, max, selected.Minute, selected.Second, selected.Offset);
                    break;
                case TimeInputMode.Minutes:
                    selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, max, selected.Second, selected.Offset);
                    break;
                case TimeInputMode.Seconds:
                    selected = new(selected.Year, selected.Month, selected.Day, selected.Hour, selected.Minute, max, selected.Offset);
                    break;
            }
        }

        private static (int min, int max) GetMinMax(TimeInputMode inputMode) =>
            inputMode switch
            {
                TimeInputMode.Hours => (0, 23),
                TimeInputMode.Minutes => (0, 59),
                TimeInputMode.Seconds => (0, 59),
                _ => default,
            };

        static InfoBoxTimeColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
