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
using System.IO;
using System.Linq;
using Calendrier;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminaux.Base;
using Colorimetry;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Terminaux.Themes
{
    /// <summary>
    /// Theme information class
    /// </summary>
    public class ThemeInfo
    {

        internal readonly Dictionary<string, Color> themeColors = ThemeColorsTools.PopulateColorsEmpty();
        internal readonly List<string> themeExtraColors = [];
        internal readonly DateTime start = DateTime.Today;
        internal readonly DateTime end = DateTime.Today;
        private string[] useAccentTypes = [];
        private readonly ThemeMetadata metadata;
        private readonly JToken metadataToken;

        /// <summary>
        /// Theme name
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Theme description
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Is true color required?
        /// </summary>
        public bool TrueColorRequired { get; }
        /// <summary>
        /// Whether this theme celebrates a specific event
        /// </summary>
        public bool IsEvent { get; }
        /// <summary>
        /// The month in which the event starts
        /// </summary>
        public int StartMonth { get; }
        /// <summary>
        /// The day in which the event starts
        /// </summary>
        public int StartDay { get; }
        /// <summary>
        /// The start <see cref="DateTime"/> instance representing the start of the event
        /// </summary>
        public DateTime Start =>
            start;
        /// <summary>
        /// The month in which the event ends
        /// </summary>
        public int EndMonth { get; }
        /// <summary>
        /// The day in which the event ends
        /// </summary>
        public int EndDay { get; }
        /// <summary>
        /// The end <see cref="DateTime"/> instance representing the end of the event
        /// </summary>
        public DateTime End =>
            end;
        /// <summary>
        /// Whether you can set this theme or not. Always false in non-event themes. False if the theme is an event and the current
        /// time and date is between <see cref="StartMonth"/>/<see cref="StartDay"/> and <see cref="EndMonth"/>/<see cref="EndDay"/>
        /// </summary>
        public bool IsExpired =>
            IsEvent && (DateTime.Now < Start || DateTime.Now > End);
        /// <summary>
        /// The category in which the theme is categorized
        /// </summary>
        public ThemeCategory Category { get; }
        /// <summary>
        /// The calendar name in which the event is assigned to
        /// </summary>
        public string Calendar { get; }
        /// <summary>
        /// Kernel color type list to use accent color
        /// </summary>
        public string[] UseAccentTypes =>
            useAccentTypes;

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(ThemeColorType type) =>
            GetColor(type.ToString());

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public Color GetColor(string type)
        {
            if (UseAccentTypes.Contains(type) && ThemeColorsTools.UseAccentColors)
                return type.EndsWith("BackgroundColor") || type.EndsWith("BackColor") ?
                    new Color(ThemeColorsTools.AccentBackgroundColor) :
                    new Color(ThemeColorsTools.AccentForegroundColor);
            return themeColors[type];
        }

        /// <summary>
        /// Sets a color in the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to edit</param>
        public void SetColor(ThemeColorType type, Color color) =>
            SetColor(type.ToString(), color);

        /// <summary>
        /// Sets a color in the color type. If the color type doesn't exist, it adds a new one to the theme.
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to edit</param>
        public void SetColor(string type, Color color)
        {
            // Add the extra color type, if it's defined
            if (!Enum.IsDefined(typeof(ThemeColorType), type) && !themeExtraColors.Contains(type))
                themeExtraColors.Add(type);

            // Set the color
            themeColors[type] = color;
        }

        internal void UpdateColors()
        {
            // Populate the colors
            ConsoleLogger.Debug($"Updating color according to theme info for {Name}...");
            useAccentTypes = [.. metadata.UseAccentTypes.Where((type) => Enum.IsDefined(typeof(ThemeColorType), type.Remove(type.Length - 5)) || themeExtraColors.Contains(type))];

            // Deal with base color types
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
            {
                string type = themeColors.Keys.ElementAt(typeIndex);

                // Get the color value and check to see if it's null
                string fullTypeName = $"{type}Color";
                var colorToken = metadataToken.SelectToken(fullTypeName);
                if (colorToken is null)
                {
                    ConsoleLogger.Warning($"{fullTypeName} is not defined in the theme metadata. Using defaults...");
                    themeColors[type] = ThemeColorsTools.PopulateColorsDefault()[type];
                }
                else
                    themeColors[type] = new Color(colorToken.ToString());
            }

            // Deal with extra color types
            foreach (string themeExtraColor in themeExtraColors)
            {
                // Get the color value and check to see if it's null
                var colorToken = metadataToken.SelectToken(themeExtraColor);
                if (colorToken is not null)
                    themeColors[themeExtraColor] = new Color(colorToken.ToString());
                else
                    themeColors[themeExtraColor] = Color.Empty;
            }

            // Let the theme-based color tools know about this new type
            UpdateColorsTools();
        }

        internal void UpdateColorsTools()
        {
            foreach (string themeExtraColor in themeExtraColors)
            {
                if (!ThemeColorsTools.themeColors?.ContainsKey(themeExtraColor) ?? false)
                    ThemeColorsTools.themeColors?.Add(themeExtraColor, themeColors[themeExtraColor]);
                if (!ThemeColorsTools.themeDefaultColors?.ContainsKey(themeExtraColor) ?? false)
                    ThemeColorsTools.themeDefaultColors?.Add(themeExtraColor, themeColors[themeExtraColor]);
                if (!ThemeColorsTools.themeEmptyColors?.ContainsKey(themeExtraColor) ?? false)
                    ThemeColorsTools.themeEmptyColors?.Add(themeExtraColor, Color.Empty);
            }
        }

        /// <summary>
        /// Generates a new theme info from default resources
        /// </summary>
        public ThemeInfo() :
            this(JToken.Parse(ThemeTools.GetThemeInfoJsonFromResources("Default")) ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_DEFAULTTHEMEFAILED")))
        { }

        /// <summary>
        /// Generates a new theme info from file path
        /// </summary>
        /// <param name="themePath">Theme file path</param>
        public ThemeInfo(string themePath) :
            this(JToken.Parse(File.ReadAllText(themePath)))
        { }

        /// <summary>
        /// Generates a new theme info from file stream
        /// </summary>
        /// <param name="ThemeFileStream">Theme file stream reader</param>
        public ThemeInfo(StreamReader ThemeFileStream) :
            this(JToken.Parse(ThemeFileStream.ReadToEnd()))
        { }

        /// <summary>
        /// Generates a new theme info from theme resource JSON
        /// </summary>
        /// <param name="ThemeResourceJson">Theme resource JSON</param>
        public ThemeInfo(JToken ThemeResourceJson)
        {
            // Parse the metadata
            var metadataObj = ThemeResourceJson["Metadata"] ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_NOTHEMEMETADATA"));
            metadata = JsonConvert.DeserializeObject<ThemeMetadata>(metadataObj.ToString()) ??
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMEMETADATAINVALID"));
            metadataToken = ThemeResourceJson;

            // Populate colors
            Name = metadata.Name;
            foreach (JProperty token in ThemeResourceJson.Cast<JProperty>())
            {
                if (Enum.IsDefined(typeof(ThemeColorType), token.Name.RemoveSuffix("Color")))
                    continue;
                if (token.Name == "Metadata")
                    continue;
                themeExtraColors.Add(token.Name);
            }
            UpdateColors();

            // Install some info to the class
            Description = metadata.Description;
            TrueColorRequired = ThemeTools.MinimumTypeRequired(themeColors, ColorType.TrueColor);
            Category = metadata.Category;

            // Parse event-related info
            IsEvent = metadata.IsEvent;
            StartMonth = metadata.StartMonth;
            StartDay = metadata.StartDay;
            EndMonth = metadata.EndMonth;
            EndDay = metadata.EndDay;
            Calendar = metadata.Calendar;
            if (!Enum.TryParse(Calendar, out CalendarTypes calendar))
                calendar = CalendarTypes.Gregorian;

            // If the calendar is not Gregorian (for example, Hijri), convert that to Gregorian using the current date
            if (calendar != CalendarTypes.Gregorian)
            {
                var calendarInstance = CalendarTools.GetCalendar(calendar);
                int year = calendarInstance.Culture.DateTimeFormat.Calendar.GetYear(DateTime.Now);
                int yearEnd = year;
                int monthStart = StartMonth;
                int monthEnd = EndMonth;
                var dayStart = StartDay;
                var dayEnd = EndDay;
                if (monthEnd < monthStart)
                    yearEnd++;
                var dateTimeStart = new DateTime(year, monthStart, dayStart, calendarInstance.Culture.DateTimeFormat.Calendar);
                var dateTimeEnd = new DateTime(yearEnd, monthEnd, dayEnd, calendarInstance.Culture.DateTimeFormat.Calendar);
                StartMonth = dateTimeStart.Month;
                EndMonth = dateTimeEnd.Month;
                StartDay = dateTimeStart.Day;
                EndDay = dateTimeEnd.Day;
            }

            // Month sanity checks
            StartMonth =
                StartMonth < 1 ? 1 :
                StartMonth > 12 ? 12 :
                StartMonth;
            EndMonth =
                EndMonth < 1 ? 1 :
                EndMonth > 12 ? 12 :
                EndMonth;

            // Day sanity checks
            int maxDayNumStart = DateTime.DaysInMonth(DateTime.Now.Year, StartMonth);
            int maxDayNumEnd = DateTime.DaysInMonth(DateTime.Now.Year, EndMonth);
            StartDay =
                StartDay < 1 ? 1 :
                StartDay > maxDayNumStart ? maxDayNumStart :
                StartDay;
            EndDay =
                EndDay < 1 ? 1 :
                EndDay > maxDayNumEnd ? maxDayNumEnd :
                EndDay;

            // Check to see if the end is earlier than the start
            start = new(DateTime.Now.Year, StartMonth, StartDay);
            end = new(DateTime.Now.Year, EndMonth, EndDay);
            if (start > end)
            {
                // End is earlier than start! Swap the two values so that:
                //    start = end;
                //    end = start;
                (end, start) = (start, end);

                // Deal with the start and the end
                if (StartMonth > EndMonth)
                    end = end.AddYears(1);
                else if (StartDay > EndDay)
                    (EndDay, StartDay) = (StartDay, EndDay);
            }
        }

    }
}
