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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Terminaux.Colors;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters.Graphical
{
    /// <summary>
    /// Calendars renderable
    /// </summary>
    public class Calendars : GraphicalCyclicWriter
    {
        private int year = 0;
        private int month = 0;
        private CultureInfo culture = new("en-US");
        private Color separatorColor = ColorTools.CurrentForegroundColor;
        private Color headerColor = ColorTools.CurrentForegroundColor;
        private Color valueColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
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
        /// Table separator color
        /// </summary>
        public Color SeparatorColor
        {
            get => separatorColor;
            set => separatorColor = value;
        }

        /// <summary>
        /// Table header color
        /// </summary>
        public Color HeaderColor
        {
            get => headerColor;
            set => headerColor = value;
        }

        /// <summary>
        /// Table value color
        /// </summary>
        public Color ValueColor
        {
            get => valueColor;
            set => valueColor = value;
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
        /// Specifies the cell options to highlight days
        /// </summary>
        public List<CellOptions> CellOptions { get; set; } = [];

        /// <summary>
        /// Renders a calendar
        /// </summary>
        /// <returns>Rendered calendar that will be used by the renderer</returns>
        public override string Render()
        {
            var calendarRendered = new StringBuilder();
            var calendarDays = culture.DateTimeFormat.DayNames;
            var calendarMonths = culture.DateTimeFormat.MonthNames;
            var calendarWeek = culture.DateTimeFormat.FirstDayOfWeek;
            var calendarData = new string[7, calendarDays.Length];
            var maxDate = Calendar.GetDaysInMonth(year, month);
            var selectedDate = new DateTime(year, month, DateTime.Now.Day > maxDate ? 1 : DateTime.Now.Day);
            var (calYear, calMonth, _, _) = GetDateFromCalendar(selectedDate, culture);
            var DateTo = new DateTime(calYear, calMonth, Calendar.GetDaysInMonth(calYear, calMonth));
            int CurrentWeek = 1;
            string CalendarTitle = calendarMonths[calMonth - 1] + " " + calYear;

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
                calendarData[0, i] = $"{day}";
            }

            // Populate the calendar data
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                var CurrentDate = new DateTime(calYear, calMonth, CurrentDay, Calendar);
                if (CurrentDate.DayOfWeek == calendarWeek)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                int currentDay = mappedDays[CurrentDate.DayOfWeek] + 1;
                calendarData[CurrentWeekIndex + 1, currentDay - 1] = $"{CurrentDay}";
            }

            // Render the calendar
            var calendarTable = new Table()
            {
                Left = Left,
                Top = Top,
                Width = Width,
                Height = Height,
                Header = true,
                Settings = CellOptions,
                BorderSettings = borderSettings,
                BackgroundColor = backgroundColor,
                ValueColor = valueColor,
                HeaderColor = headerColor,
                SeparatorColor = separatorColor,
                Rows = calendarData,
                Title = CalendarTitle,
                UseColors = UseColors,
            };
            calendarRendered.Append(calendarTable.Render());
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
