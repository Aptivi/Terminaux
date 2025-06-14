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
    /// Info box writer with date and color support
    /// </summary>
    public static class InfoBoxDateColor
    {
        private static Keybinding[] Keybindings =>
        [
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_DECREMENTVALUE"), ConsoleKey.UpArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_INCREMENTVALUE"), ConsoleKey.DownArrow),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MINIMUMVALUE"), ConsoleKey.Home),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_MAXIMUMVALUE"), ConsoleKey.End),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTVALUEELEMENT"), ConsoleKey.Tab),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVVALUEELEMENT"), ConsoleKey.Tab, ConsoleModifiers.Shift),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEUP"), ConsoleKey.W),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_ONELINEDOWN"), ConsoleKey.S),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PREVPAGETEXT"), ConsoleKey.E),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_NEXTPAGETEXT"), ConsoleKey.D),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_SUBMIT_SINGULAR"), ConsoleKey.Enter),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_CANCEL_SINGULAR"), ConsoleKey.Escape),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_PERFORMORSELECT"), PointerButton.Left),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELUP"), PointerButton.WheelUp),
            new Keybinding(LanguageTools.GetLocalized("T_INPUT_STYLES_INFOBOX_KEYBINDING_WHEELDOWN"), PointerButton.WheelDown),
        ];

        /// <summary>
        /// Writes the date info box
        /// </summary>
        /// <param name="initialDate">Initial date</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static DateTimeOffset WriteInfoBoxDate(DateTimeOffset initialDate, string text, params object[] vars) =>
            WriteInfoBoxDate(initialDate, text, InfoBoxSettings.GlobalSettings, vars);

        /// <summary>
        /// Writes the date info box
        /// </summary>
        /// <param name="initialDate">Initial date</param>
        /// <param name="settings">Infobox settings to use</param>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static DateTimeOffset WriteInfoBoxDate(DateTimeOffset initialDate, string text, InfoBoxSettings settings, params object[] vars)
        {
            bool initialCursorVisible = ConsoleWrapper.CursorVisible;
            bool initialScreenIsNull = ScreenTools.CurrentScreen is null;
            var infoBoxScreenPart = new ScreenPart();
            var screen = new Screen();
            DateTimeOffset selected = initialDate;
            DateInputMode inputMode = DateInputMode.Years;
            if (initialScreenIsNull)
                ScreenTools.SetCurrent(screen);
            ScreenTools.CurrentScreen?.AddBufferedPart(nameof(InfoBoxDateColor), infoBoxScreenPart);
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

                    // Write years
                    string years = selected.Year.ToString();
                    int yearsPos = timePosX + maxTimePartWidth / 2 - ConsoleChar.EstimateFullWidths(years);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(years, yearsPos, timePosY + 2, inputMode == DateInputMode.Years ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", timePosX, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", timePosX + maxTimePartWidth + 1, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor)
                    );

                    // Write months
                    string months = selected.Month.ToString();
                    int monthsPos = timePosX + maxTimePartWidth + maxTimePartWidth / 2 + 3 - ConsoleChar.EstimateFullWidths(months);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(months, monthsPos, timePosY + 2, inputMode == DateInputMode.Months ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("◀", timePosX + maxTimePartWidth + 3, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor) +
                        TextWriterWhereColor.RenderWhereColorBack("▶", timePosX + maxTimePartWidth * 2 + 4, timePosY + 2, settings.ForegroundColor, settings.BackgroundColor)
                    );

                    // Write days
                    string days = selected.Day.ToString();
                    int daysPos = timePosX + maxTimePartWidth * 2 + maxTimePartWidth / 2 + 7 - ConsoleChar.EstimateFullWidths(days);
                    boxBuffer.Append(
                        TextWriterWhereColor.RenderWhereColorBack(days, daysPos, timePosY + 2, inputMode == DateInputMode.Days ? ConsoleColors.Lime : settings.ForegroundColor, settings.BackgroundColor) +
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
                    int timeArrowYearsLeft = borderX + 2;
                    int timeArrowYearsRight = timeArrowYearsLeft + maxTimePartWidth + 1;
                    int timeArrowMonthsLeft = timeArrowYearsLeft + maxTimePartWidth + 3;
                    int timeArrowMonthsRight = timeArrowYearsLeft + maxTimePartWidth * 2 + 4;
                    int timeArrowDaysLeft = timeArrowYearsLeft + maxTimePartWidth * 2 + 6;
                    int timeArrowDaysRight = timeArrowYearsLeft + maxTimePartWidth * 3 + 8;

                    // Make hitboxes for arrow and button presses
                    var arrowUpHitbox = new PointerHitbox(new(arrowLeft, arrowTop), new Action<PointerEventContext>((_) => GoUp(ref currIdx))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowDownHitbox = new PointerHitbox(new(arrowLeft, arrowBottom), new Action<PointerEventContext>((_) => GoDown(ref currIdx, text, vars))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseYearsHitbox = new PointerHitbox(new(timeArrowYearsLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.YearsModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseYearsHitbox = new PointerHitbox(new(timeArrowYearsRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.YearsModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseMonthsHitbox = new PointerHitbox(new(timeArrowMonthsLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.MonthsModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseMonthsHitbox = new PointerHitbox(new(timeArrowMonthsRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.MonthsModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeDecreaseDaysHitbox = new PointerHitbox(new(timeArrowDaysLeft, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.DaysModify(ref selected, ref inputMode))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };
                    var arrowTimeIncreaseDaysHitbox = new PointerHitbox(new(timeArrowDaysRight, timeArrowTop), new Action<PointerEventContext>((_) => TimeDateInputTools.DaysModify(ref selected, ref inputMode, true))) { Button = PointerButton.Left, ButtonPress = PointerButtonPress.Released };

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
                                    if (IsMouseWithinYears(text, vars, mouse))
                                        TimeDateInputTools.YearsModify(ref selected, ref inputMode, true);
                                    else if (IsMouseWithinMonths(text, vars, mouse))
                                        TimeDateInputTools.MonthsModify(ref selected, ref inputMode, true);
                                    else if (IsMouseWithinDays(text, vars, mouse))
                                        TimeDateInputTools.DaysModify(ref selected, ref inputMode, true);
                                }
                                break;
                            case PointerButton.WheelDown:
                                if (IsMouseWithinText(text, vars, mouse))
                                    GoDown(ref currIdx, text, vars, 3);
                                else if (IsMouseWithinTime(text, vars, mouse))
                                {
                                    if (IsMouseWithinYears(text, vars, mouse))
                                        TimeDateInputTools.YearsModify(ref selected, ref inputMode);
                                    else if (IsMouseWithinMonths(text, vars, mouse))
                                        TimeDateInputTools.MonthsModify(ref selected, ref inputMode);
                                    else if (IsMouseWithinDays(text, vars, mouse))
                                        TimeDateInputTools.DaysModify(ref selected, ref inputMode);
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
                                else if ((arrowTimeIncreaseYearsHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseYearsHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseYearsHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseYearsHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowTimeIncreaseMonthsHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseMonthsHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseMonthsHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseMonthsHitbox.ProcessPointer(mouse, out done);
                                }
                                else if ((arrowTimeIncreaseDaysHitbox.IsPointerWithin(mouse) || arrowTimeDecreaseDaysHitbox.IsPointerWithin(mouse)))
                                {
                                    arrowTimeIncreaseDaysHitbox.ProcessPointer(mouse, out bool done);
                                    if (!done)
                                        arrowTimeDecreaseDaysHitbox.ProcessPointer(mouse, out done);
                                }
                                break;
                        }
                    }
                    else if (data.ConsoleKeyInfo is ConsoleKeyInfo cki && !Input.PointerActive)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                TimeDateInputTools.ValueGoDown(ref selected, inputMode);
                                break;
                            case ConsoleKey.RightArrow:
                                TimeDateInputTools.ValueGoUp(ref selected, inputMode);
                                break;
                            case ConsoleKey.Home:
                                TimeDateInputTools.ValueFirst(ref selected, inputMode);
                                break;
                            case ConsoleKey.End:
                                TimeDateInputTools.ValueLast(ref selected, inputMode);
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
                            case ConsoleKey.K:
                                // Keys function
                                KeybindingTools.ShowKeybindingInfobox(Keybindings);
                                break;
                        }
                    }
                }
                if (cancel)
                    selected = initialDate;
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

        private static bool IsMouseWithinYears(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeYearsLeft = borderX + 3;
            int timeYearsRight = timeYearsLeft + maxTimePartWidth - 1;
            return PointerTools.PointerWithinRange(mouse, (timeYearsLeft, timeTop), (timeYearsRight, timeTop));
        }

        private static bool IsMouseWithinMonths(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeYearsLeft = borderX + 3;
            int timeMonthsLeft = timeYearsLeft + maxTimePartWidth + 3;
            int timeMonthsRight = timeYearsLeft + maxTimePartWidth * 2 + 2;
            return PointerTools.PointerWithinRange(mouse, (timeMonthsLeft, timeTop), (timeMonthsRight, timeTop));
        }

        private static bool IsMouseWithinDays(string text, object[] vars, PointerEventContext mouse)
        {
            string[] splitFinalLines = TextWriterTools.GetFinalLines(text, vars);
            var (maxWidth, maxHeight, _, borderX, borderY) = InfoBoxTools.GetDimensions(splitFinalLines, 2);

            // Check the dimensions
            int maxTimeWidth = maxWidth - 4;
            int maxTimePartWidth = maxTimeWidth / 3 - 2;
            int timeTop = borderY + maxHeight - 1;
            int timeYearsLeft = borderX + 3;
            int timeDaysLeft = timeYearsLeft + maxTimePartWidth * 2 + 6;
            int timeDaysRight = timeYearsLeft + maxTimePartWidth * 3 + 6;
            return PointerTools.PointerWithinRange(mouse, (timeDaysLeft, timeTop), (timeDaysRight, timeTop));
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

        static InfoBoxDateColor()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
