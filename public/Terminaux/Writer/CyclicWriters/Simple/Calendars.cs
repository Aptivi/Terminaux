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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Calendrier;
using Colorimetry;
using Terminaux.Base.Extensions;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Simple
{
    /// <summary>
    /// Calendars renderable
    /// </summary>
    public class Calendars : SimpleCyclicWriter
    {
        private int year = 0;
        private int month = 0;
        private CultureInfo culture = new("en-US");
        private Color headerColor = ThemeColorsTools.GetColor(ThemeColorType.TableHeader);
        private Color weekendColor = ThemeColorsTools.GetColor(ThemeColorType.WeekendDay);
        private Color todayColor = ThemeColorsTools.GetColor(ThemeColorType.TodayDay);
        private Color eventColor = ThemeColorsTools.GetColor(ThemeColorType.EventDay);
        private Color foregroundColor = ThemeColorsTools.GetColor(ThemeColorType.TableValue);
        private Color backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);
        private BorderSettings borderSettings = new();
        private bool useColors = true;

        /// <summary>
        /// Year of the calendar
        /// </summary>
        public int Year
        {
            get => year;
            set => year = value;
        }

        /// <summary>
        /// Month of the calendar
        /// </summary>
        public int Month
        {
            get => month;
            set => month = value;
        }

        /// <summary>
        /// Culture info that the calendar uses
        /// </summary>
        public CultureInfo Culture
        {
            get => culture;
            set => culture = value;
        }

        /// <summary>
        /// Calendar header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set => headerColor = value;
        }

        /// <summary>
        /// Calendar day weekend indicator color
        /// </summary>
        public Color WeekendColor
        {
            get => weekendColor;
            set => weekendColor = value;
        }

        /// <summary>
        /// Current day color
        /// </summary>
        public Color TodayColor
        {
            get => todayColor;
            set => todayColor = value;
        }

        /// <summary>
        /// Event day color
        /// </summary>
        public Color EventColor
        {
            get => eventColor;
            set => eventColor = value;
        }

        /// <summary>
        /// Foreground color
        /// </summary>
        public Color ForegroundColor
        {
            get => foregroundColor;
            set => foregroundColor = value;
        }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        /// <summary>
        /// Whether to use colors or not
        /// </summary>
        public bool UseColors
        {
            get => useColors;
            set => useColors = value;
        }

        /// <summary>
        /// Border settings to use
        /// </summary>
        public BorderSettings BorderSettings
        {
            get => borderSettings;
            set => borderSettings = value;
        }

        /// <summary>
        /// Calendar instance that is obtained from a culture
        /// </summary>
        public Calendar Calendar =>
            Culture.DateTimeFormat.Calendar;

        /// <summary>
        /// Whether to highlight the current day or to highlight a specified day
        /// </summary>
        public bool HighlightToday { get; set; } = true;

        /// <summary>
        /// Highlight a specified day
        /// </summary>
        public DateTime HighlightedDay { get; set; } = DateTime.Today;

        /// <summary>
        /// Sets the event dates
        /// </summary>
        public DateTime[]? EventDates { get; set; }

        /// <summary>
        /// Sets the reminder dates
        /// </summary>
        public DateTime[]? ReminderDates { get; set; }

        /// <summary>
        /// Maximum width of the table (0 to automatically determine based on content)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Maximum height of the table (0 to automatically determine based on content)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Renders a calendar
        /// </summary>
        /// <returns>Rendered calendar that will be used by the renderer</returns>
        public override string Render()
        {
            var calendarRendered = new StringBuilder();
            var calendarDays = culture.DateTimeFormat.DayNames;
            var calendarAbbreviatedDays = culture.DateTimeFormat.AbbreviatedDayNames;
            var calendarMonths = culture.DateTimeFormat.MonthNames;
            var calendarWeek = culture.DateTimeFormat.FirstDayOfWeek;
            var maxDate = Calendar.GetDaysInMonth(year, month);
            var selectedDate = new DateTime(year, month, DateTime.Now.Day > maxDate ? 1 : DateTime.Now.Day);
            var (calYear, calMonth, calDay, _) = GetDateFromCalendar(selectedDate, culture);
            var dateTo = new DateTime(calYear, calMonth, Calendar.GetDaysInMonth(calYear, calMonth));
            int processedWeeks = 1;
            string calendarTitle = calendarMonths[calMonth - 1] + " " + calYear;

            // Re-arrange the days according to the first day of week
            Dictionary<DayOfWeek, int> mappedDays = [];
            int dayOfWeek = (int)calendarWeek;
            for (int i = 0; i < calendarDays.Length; i++)
            {
                var day = (DayOfWeek)dayOfWeek;
                mappedDays.Add(day, i);
                dayOfWeek++;
                if (dayOfWeek > 6)
                    dayOfWeek = 0;
            }

            // Make a calendar array
            for (int d = 1; d <= dateTo.Day; d++)
            {
                // Populate some variables
                var currentDate = new DateTime(calYear, calMonth, d);
                if (currentDate.DayOfWeek == calendarWeek)
                    processedWeeks += 1;
            }
            TableCellOptions[,] calendarArray = new TableCellOptions[processedWeeks + 1, 7];

            // Actually make a calendar
            int currentWeek = 1;
            for (int d = 1; d <= dateTo.Day; d++)
            {
                // Some flags
                var currentDate = new DateTime(calYear, calMonth, d);
                if (currentDate.DayOfWeek == calendarWeek)
                    currentWeek += 1;
                int currentDay = mappedDays[currentDate.DayOfWeek] + 1;
                var dateHighlight = HighlightToday ? new DateTime(calYear, calMonth, calDay) : HighlightedDay;
                bool isWeekend = currentDay > 5;
                bool isToday = currentDate == dateHighlight;
                var foreground = isToday ? TodayColor : isWeekend ? WeekendColor : ForegroundColor;

                // Highlight reminders and events
                bool reminderMarked = false;
                bool eventMarked = false;
                foreach (var reminderDate in ReminderDates ?? [])
                {
                    var rDate = reminderDate;
                    var (rYear, rMonth, rDay, _) = GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), culture);
                    rDate = new(rYear, rMonth, rDay);
                    if (rDate == currentDate & !reminderMarked)
                        reminderMarked = true;
                }
                foreach (var eventDate in EventDates ?? [])
                {
                    var eDate = eventDate;
                    var (eYear, eMonth, eDay, _) = GetDateFromCalendar(new DateTime(eDate.Year, eDate.Month, eDate.Day), culture);
                    eDate = new(eYear, eMonth, eDay);
                    if (eDate == currentDate & !eventMarked)
                    {
                        foreground = EventColor;
                        eventMarked = true;
                    }
                }
                string markStart = reminderMarked && eventMarked ? "[" : reminderMarked ? "(" : eventMarked ? "<" : " ";
                string markEnd = reminderMarked && eventMarked ? "]" : reminderMarked ? ")" : eventMarked ? ">" : " ";

                // Know where and how to put the day number
                calendarArray[currentWeek, currentDay - 1] = new($"{markStart}{d}{markEnd}")
                {
                    CellBackgroundColor = BackgroundColor,
                    CellColor = foreground,
                    ColoredCell = true,
                    TextSettings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
            }

            // Make day indicators
            for (int i = 0; i < mappedDays.Count; i++)
            {
                string dayName = $"{calendarAbbreviatedDays[(int)mappedDays.Keys.ElementAt(i)]}";
                calendarArray[0, i] = new(dayName)
                {
                    CellBackgroundColor = BackgroundColor,
                    CellColor = HeaderColor,
                    ColoredCell = true,
                    TextSettings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
            }

            // Create a table
            var table = new Table()
            {
                SeparatorColor = ForegroundColor,
                ValueColor = ForegroundColor,
                HeaderColor = HeaderColor,
                BackgroundColor = BackgroundColor,
                UseColors = UseColors,
                BorderSettings = BorderSettings,
                Title = calendarTitle,
                Header = true,
                Rows = calendarArray,
                Width = Width,
                Height = Height,
            };
            calendarRendered.Append(table.Render());
            return calendarRendered.ToString();
        }

        private static (int year, int month, int day, Calendar calendarInfo) GetDateFromCalendar(DateTime dt, CultureInfo culture)
        {
            var calendarInfo = culture.DateTimeFormat.Calendar;
            int year = calendarInfo.GetYear(dt);
            int month = calendarInfo.GetMonth(dt);
            int day = calendarInfo.GetDayOfMonth(dt);
            return (year, month, day, calendarInfo);
        }

        /// <summary>
        /// Makes a new instance of the calendar renderer
        /// </summary>
        public Calendars()
        { }
    }
}
