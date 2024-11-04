//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Writer.CyclicWriters
{
    /// <summary>
    /// Calendars renderable
    /// </summary>
    public class Calendars : IStaticRenderable
    {
        private int year = 0;
        private int month = 0;
        private int left = 0;
        private int top = 0;
        private int interiorWidth = 0;
        private int interiorHeight = 0;
        private CultureInfo culture = new("en-US");
        private Color separatorColor = ColorTools.CurrentForegroundColor;
        private Color headerColor = ColorTools.CurrentForegroundColor;
        private Color valueColor = ColorTools.CurrentForegroundColor;
        private Color backgroundColor = ColorTools.CurrentBackgroundColor;
        private BorderSettings borderSettings = new();

        /// <summary>
        /// Left position
        /// </summary>
        public int Left
        {
            get => left;
            set => left = value;
        }

        /// <summary>
        /// Top position
        /// </summary>
        public int Top
        {
            get => top;
            set => top = value;
        }

        /// <summary>
        /// Interior width
        /// </summary>
        public int InteriorWidth
        {
            get => interiorWidth;
            set => interiorWidth = value;
        }

        /// <summary>
        /// Interior height
        /// </summary>
        public int InteriorHeight
        {
            get => interiorHeight;
            set => interiorHeight = value;
        }

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
        public string Render() =>
            RenderCalendar(Left, Top, InteriorWidth, InteriorHeight, Year, Month, Culture, Calendar, CellOptions, SeparatorColor, HeaderColor, ValueColor, BackgroundColor, BorderSettings);

        internal static string RenderCalendar(int left, int top, int width, int height, int year, int month, CultureInfo culture, Calendar calendar, List<CellOptions> cellOptions, Color separatorColor, Color headerColor, Color valueColor, Color backgroundColor, BorderSettings borderSettings)
        {
            var calendarRendered = new StringBuilder();
            var calendarDays = culture.DateTimeFormat.DayNames;
            var calendarMonths = culture.DateTimeFormat.MonthNames;
            var calendarWeek = culture.DateTimeFormat.FirstDayOfWeek;
            var calendarData = new string[7, calendarDays.Length];
            var maxDate = calendar.GetDaysInMonth(year, month);
            var selectedDate = new DateTime(year, month, DateTime.Now.Day > maxDate ? 1 : DateTime.Now.Day);
            var (calYear, calMonth, _, _) = GetDateFromCalendar(selectedDate, culture);
            var DateTo = new DateTime(calYear, calMonth, calendar.GetDaysInMonth(calYear, calMonth));
            int CurrentWeek = 1;
            string CalendarTitle = calendarMonths[month - 1] + " " + calYear;

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
                var CurrentDate = new DateTime(year, month, CurrentDay);
                if (CurrentDate.DayOfWeek == calendarWeek)
                    CurrentWeek += 1;
                int CurrentWeekIndex = CurrentWeek - 1;
                int currentDay = mappedDays[CurrentDate.DayOfWeek] + 1;
                calendarData[CurrentWeekIndex + 1, currentDay - 1] = $"{CurrentDay}";
            }

            // Render the calendar
            var calendarTable = new Table()
            {
                Left = left,
                Top = top,
                InteriorWidth = width,
                InteriorHeight = height,
                Header = true,
                Settings = cellOptions,
                BorderSettings = borderSettings,
                BackgroundColor = backgroundColor,
                ValueColor = valueColor,
                HeaderColor = headerColor,
                SeparatorColor = separatorColor,
                Rows = calendarData,
                Title = CalendarTitle,
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
        /// Makes a new instance of the calendars renderer
        /// </summary>
        public Calendars()
        { }
    }
}
