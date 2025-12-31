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
using System.Globalization;

namespace Terminaux.Inputs.Styles.Infobox.Tools
{
    internal static class TimeDateInputTools
    {
        #region Date
        internal static void ChangeInputMode(ref DateInputMode inputMode, bool backward = false)
        {
            if (backward)
            {
                inputMode--;
                if (inputMode < DateInputMode.Years)
                    inputMode = DateInputMode.Days;
            }
            else
            {
                inputMode++;
                if (inputMode > DateInputMode.Days)
                    inputMode = DateInputMode.Years;
            }
        }

        internal static void ValueGoDown(ref DateTimeOffset selected, DateInputMode inputMode)
        {
            switch (inputMode)
            {
                case DateInputMode.Years:
                    YearsModify(ref selected, ref inputMode);
                    break;
                case DateInputMode.Months:
                    MonthsModify(ref selected, ref inputMode);
                    break;
                case DateInputMode.Days:
                    DaysModify(ref selected, ref inputMode);
                    break;
            }
        }

        internal static void ValueGoUp(ref DateTimeOffset selected, DateInputMode inputMode)
        {
            switch (inputMode)
            {
                case DateInputMode.Years:
                    YearsModify(ref selected, ref inputMode, true);
                    break;
                case DateInputMode.Months:
                    MonthsModify(ref selected, ref inputMode, true);
                    break;
                case DateInputMode.Days:
                    DaysModify(ref selected, ref inputMode, true);
                    break;
            }
        }

        internal static void YearsModify(ref DateTimeOffset selected, ref DateInputMode inputMode, bool goUp = false)
        {
            if (inputMode != DateInputMode.Years)
                inputMode = DateInputMode.Years;
            (int min, int max) = GetMinMax(selected, inputMode);
            int year = selected.Year;
            if (goUp)
            {
                year++;
                if (year > max)
                    year = min;
            }
            else
            {
                year--;
                if (year < min)
                    year = max;
            }

            // Check the day
            (_, int maxDay) = GetMinMax(year, selected.Month, DateInputMode.Days);
            selected = new(year, selected.Month, selected.Day > maxDay ? maxDay : selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
        }

        internal static void MonthsModify(ref DateTimeOffset selected, ref DateInputMode inputMode, bool goUp = false)
        {
            if (inputMode != DateInputMode.Months)
                inputMode = DateInputMode.Months;
            (int min, int max) = GetMinMax(selected, inputMode);
            int month = selected.Month;
            if (goUp)
            {
                month++;
                if (month > max)
                    month = min;
            }
            else
            {
                month--;
                if (month < min)
                    month = max;
            }

            // Check the day
            (_, int maxDay) = GetMinMax(selected.Year, month, DateInputMode.Days);
            selected = new(selected.Year, month, selected.Day > maxDay ? maxDay : selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
        }

        internal static void DaysModify(ref DateTimeOffset selected, ref DateInputMode inputMode, bool goUp = false)
        {
            if (inputMode != DateInputMode.Days)
                inputMode = DateInputMode.Days;
            (int min, int max) = GetMinMax(selected, inputMode);
            int day = selected.Day;
            if (goUp)
            {
                day++;
                if (day > max)
                    day = min;
            }
            else
            {
                day--;
                if (day < min)
                    day = max;
            }
            selected = new(selected.Year, selected.Month, day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
        }

        internal static void ValueFirst(ref DateTimeOffset selected, DateInputMode inputMode)
        {
            (int min, _) = GetMinMax(selected, inputMode);
            switch (inputMode)
            {
                case DateInputMode.Years:
                    selected = new(min, selected.Month, selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
                case DateInputMode.Months:
                    selected = new(selected.Year, min, selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
                case DateInputMode.Days:
                    selected = new(selected.Year, selected.Month, min, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
            }
        }

        internal static void ValueLast(ref DateTimeOffset selected, DateInputMode inputMode)
        {
            (_, int max) = GetMinMax(selected, inputMode);
            switch (inputMode)
            {
                case DateInputMode.Years:
                    selected = new(max, selected.Month, selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
                case DateInputMode.Months:
                    selected = new(selected.Year, max, selected.Day, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
                case DateInputMode.Days:
                    selected = new(selected.Year, selected.Month, max, selected.Hour, selected.Minute, selected.Second, selected.Offset);
                    break;
            }
        }

        internal static (int min, int max) GetMinMax(DateTimeOffset selected, DateInputMode inputMode) =>
            inputMode switch
            {
                DateInputMode.Years => (1, 9999),
                DateInputMode.Months => (1, 12),
                DateInputMode.Days => (1, CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(selected.Year, selected.Month)),
                _ => default,
            };

        internal static (int min, int max) GetMinMax(int year, int month, DateInputMode inputMode) =>
            GetMinMax(new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero), inputMode);
        #endregion

        #region Time
        internal static void ChangeInputMode(ref TimeInputMode inputMode, bool backward = false)
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

        internal static void ValueGoDown(ref DateTimeOffset selected, TimeInputMode inputMode)
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

        internal static void ValueGoUp(ref DateTimeOffset selected, TimeInputMode inputMode)
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

        internal static void HoursModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
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

        internal static void MinutesModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
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

        internal static void SecondsModify(ref DateTimeOffset selected, ref TimeInputMode inputMode, bool goUp = false)
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

        internal static void ValueFirst(ref DateTimeOffset selected, TimeInputMode inputMode)
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

        internal static void ValueLast(ref DateTimeOffset selected, TimeInputMode inputMode)
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

        internal static (int min, int max) GetMinMax(TimeInputMode inputMode) =>
            inputMode switch
            {
                TimeInputMode.Hours => (0, 23),
                TimeInputMode.Minutes => (0, 59),
                TimeInputMode.Seconds => (0, 59),
                _ => default,
            };
        #endregion
    }
}
