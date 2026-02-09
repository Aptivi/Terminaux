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
using Newtonsoft.Json.Linq;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Terminaux.Themes.Colors;

namespace Terminaux.Themes
{
    /// <summary>
    /// Theme tools module
    /// </summary>
    public static class ThemeTools
    {
        internal readonly static object locker = new();
        internal readonly static Dictionary<string, ThemeInfo> themes = new()
        {
            { "Default", new ThemeInfo(JToken.Parse(GetThemeInfoJsonFromResources("Default"))) },
            { "Dynamic", new ThemeInfo(JToken.Parse(GetThemeInfoJsonFromResources("Dynamic"))) },
            { "NitricAcid", new ThemeInfo(JToken.Parse(GetThemeInfoJsonFromResources("NitricAcid"))) },
        };

        /// <summary>
        /// Gets the installed themes
        /// </summary>
        /// <returns>List of installed themes and their <see cref="ThemeInfo"/> instances</returns>
        public static Dictionary<string, ThemeInfo> GetInstalledThemes() =>
            new(themes);

        /// <summary>
        /// Gets the installed themes by category
        /// </summary>
        /// <param name="category">Category to look for themes</param>
        /// <returns>List of installed themes and their <see cref="ThemeInfo"/> instances</returns>
        public static Dictionary<string, ThemeInfo> GetInstalledThemesByCategory(ThemeCategory category) =>
            themes
                .Where((kvp) => kvp.Value.Category == category)
                .ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);

        /// <summary>
        /// Gets the theme information
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <param name="throwNotFound">Throws an exception if the theme is not found</param>
        public static ThemeInfo GetThemeInfo(string theme, bool throwNotFound = false)
        {
            var themes = GetInstalledThemes();
            if (themes.TryGetValue(theme, out ThemeInfo? resultingTheme))
                return resultingTheme;
            if (throwNotFound)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_TEMPLATEINVALID"), theme);
            return themes["Default"];
        }

        /// <summary>
        /// Checks to see if a theme is found
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static bool IsThemeFound(string theme)
        {
            var themes = GetInstalledThemes();
            return themes.ContainsKey(theme);
        }

        /// <summary>
        /// Registers a theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <param name="themeInfo">Theme info instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void RegisterTheme(string theme, ThemeInfo themeInfo)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENAMEEMPTY"));
                if (IsThemeFound(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMEEXISTS"));
                themes.Add(theme, themeInfo);
            }
        }

        /// <summary>
        /// Edits a theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <param name="themeInfo">Theme info instance</param>
        /// <exception cref="TerminauxException"></exception>
        public static void EditTheme(string theme, ThemeInfo themeInfo)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENAMEEMPTY"));
                if (!IsThemeFound(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENOTEXIST"));
                themes[theme] = themeInfo;
                themes[theme].UpdateColorsTools();
            }
        }

        /// <summary>
        /// Resets a theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <exception cref="TerminauxException"></exception>
        public static void ResetTheme(string theme)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENAMEEMPTY"));
                if (!IsThemeFound(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENOTEXIST"));
                themes[theme].UpdateColors();
            }
        }

        /// <summary>
        /// Unregisters a theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        /// <exception cref="TerminauxException"></exception>
        public static void UnregisterTheme(string theme)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENAMEEMPTY"));
                if (!IsThemeFound(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMENOTEXIST"));
                if (!themes.Remove(theme))
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_THEMEREMFAILED"));
            }
        }

        /// <summary>
        /// Gets the colors from the theme
        /// </summary>
        /// <param name="theme">Theme name</param>
        public static Dictionary<string, Color> GetColorsFromTheme(string theme) =>
            GetColorsFromTheme(GetThemeInfo(theme));

        /// <summary>
        /// Gets the colors from the theme
        /// </summary>
        /// <param name="themeInfo">Theme instance</param>
        public static Dictionary<string, Color> GetColorsFromTheme(ThemeInfo themeInfo)
        {
            if (themeInfo.UseAccentTypes.Length > 0 && ThemeColorsTools.UseAccentColors)
            {
                var colors = new Dictionary<string, Color>();
                foreach (string type in themeInfo.themeColors.Keys)
                    colors.Add(type, themeInfo.GetColor(type));
                return colors;
            }
            return themeInfo.themeColors;
        }

        /// <summary>
        /// Sets system colors according to the theme information instance
        /// </summary>
        /// <param name="themeInfo">A specified theme information instance</param>
        public static void ApplyTheme(ThemeInfo themeInfo)
        {
            // Check if the console supports true color
            if (ConsoleColoring.ConsoleSupportsTrueColor && themeInfo.TrueColorRequired || !themeInfo.TrueColorRequired)
            {
                // Check to see if the event is finished
                if (themeInfo.IsExpired)
                {
                    ConsoleLogger.Error("Setting event theme in a day that the event finished...");
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_TIMELIMITEDTHEME"), themeInfo.Name, themeInfo.StartMonth, themeInfo.StartDay, themeInfo.EndMonth, themeInfo.EndDay);
                }

                // Set colors as appropriate
                ConsoleLogger.Debug("Setting colors as appropriate...");
                SetColorsTheme(themeInfo);
            }
            else
            {
                // We're trying to apply true color on unsupported console
                ConsoleLogger.Error("Unsupported console or the terminal doesn't support true color.");
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_NOTRUECOLOR"), themeInfo.Name);
            }
        }

        /// <summary>
        /// Sets system colors according to the registered themes
        /// </summary>
        /// <param name="theme">A specified theme</param>
        public static void ApplyThemeFromRegistered(string theme)
        {
            ConsoleLogger.Debug("Theme: {0}", theme);
            if (GetInstalledThemes().ContainsKey(theme))
            {
                ConsoleLogger.Debug("Theme found.");

                // Populate theme info and use it
                var ThemeInfo = GetThemeInfo(theme);
                ApplyTheme(ThemeInfo);
            }
            else
            {
                ConsoleLogger.Error("Theme not found.");
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_TEMPLATEINVALID"), theme);
            }
        }

        /// <summary>
        /// Sets system colors according to the theme file
        /// </summary>
        /// <param name="ThemeFile">Theme file</param>
        public static void ApplyThemeFromFile(string ThemeFile)
        {
            try
            {
                // Populate theme info and use it
                var ThemeInfo = new ThemeInfo(ThemeFile);
                ApplyTheme(ThemeInfo);
            }
            catch (FileNotFoundException ex)
            {
                ConsoleLogger.Error(ex, "Theme not found.");
                throw;
            }
        }

        /// <summary>
        /// Sets custom colors.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetColorsTheme(ThemeInfo ThemeInfo)
        {
            if (ThemeInfo is null)
                throw new TerminauxException(nameof(ThemeInfo));

            // Check to see if we're trying to preview theme on non-true color console
            if (MinimumTypeRequired(ThemeInfo, ColorType.TrueColor) && !ConsoleColoring.ConsoleSupportsTrueColor)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_NEEDSTRUECOLOR"));

            // Set the colors
            try
            {
                // Set the built-in color types
                for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
                {
                    string type = ThemeColorsTools.themeColors.Keys.ElementAt(typeIndex);
                    var themeColor = ThemeInfo.themeColors[type];
                    ConsoleLogger.Debug("Theme color type {0}, setting theme color {1}...", type.ToString(), themeColor.PlainSequence);
                    ThemeColorsTools.themeColors[type] = themeColor;
                }

                // Set the extra color types
                foreach (var themeType in ThemeInfo.themeExtraColors)
                {
                    var themeColor = ThemeInfo.GetColor(themeType);
                    ConsoleLogger.Debug("Custom theme color type {0}, setting theme color {1}...", themeType, themeColor.PlainSequence);
                    ThemeColorsTools.themeColors[themeType] = themeColor;
                }

                // Load the background
                ThemeColorsTools.LoadBackground();
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to apply theme.");
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_COLORINVALID") + " {0}", ex, ex.Message);
            }
        }

        /// <summary>
        /// Sets custom colors. It only works if colored shell is enabled.
        /// </summary>
        /// <param name="ThemeInfo">Theme information</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool TrySetColorsTheme(ThemeInfo ThemeInfo)
        {
            try
            {
                SetColorsTheme(ThemeInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(string theme, ColorType type) =>
            MinimumTypeRequired(GetThemeInfo(theme), type);

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="theme">Theme instance</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(ThemeInfo theme, ColorType type) =>
            MinimumTypeRequired(GetColorsFromTheme(theme), type);

        /// <summary>
        /// Is the 255 color support required?
        /// </summary>
        /// <param name="colors">Dictionary of colors</param>
        /// <param name="type">Minimum required color type</param>
        /// <returns>If required, then true. Always false when <see cref="ColorType.FourBitColor"/> is passed.</returns>
        public static bool MinimumTypeRequired(Dictionary<string, Color> colors, ColorType type)
        {
            if (type == ColorType.FourBitColor)
                return false;

            // Now, check for 255-color requirement
            for (int key = 0; key < colors.Count; key++)
                if (colors.Values.ElementAt(key).Type <= type)
                    return true;

            // Else, 255 color support is not required
            return false;
        }

        internal static string GetThemeInfoJsonFromResources(string themeName)
        {
            var resourceStream = typeof(ThemeTools).Assembly.GetManifestResourceStream($"Terminaux.Resources.Themes.{themeName}.json") ??
                throw new TerminauxInternalException(LanguageTools.GetLocalized("T_COLORS_THEMES_EXCEPTION_DEFAULTTHEMEFAILED"));
            using var resourceStreamReader = new StreamReader(resourceStream);
            string content = resourceStreamReader.ReadToEnd();
            return content;
        }
    }
}
