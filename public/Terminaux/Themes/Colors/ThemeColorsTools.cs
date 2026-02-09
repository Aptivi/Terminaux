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
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;

namespace Terminaux.Themes.Colors
{
    /// <summary>
    /// Tools for theme colors
    /// </summary>
    public static class ThemeColorsTools
    {
        // Variables for theme colors
        internal static Dictionary<string, Color> themeColors = PopulateColorsDefault();
        internal static Dictionary<string, Color>? themeDefaultColors = PopulateColorsCurrent();
        internal static Dictionary<string, Color>? themeEmptyColors = PopulateColorsEmpty();

        // Variables for accent background and foreground colors
        internal static Color accentForegroundColor = GetColor(ThemeColorType.Warning);
        internal static Color accentBackgroundColor = GetColor(ThemeColorType.Background);

        /// <summary>
        /// Whether to use accent colors for themes that support accents
        /// </summary>
        public static bool UseAccentColors { get; set; }

        /// <summary>
        /// Accent color (foreground)
        /// </summary>
        public static string AccentForegroundColor
        {
            get => accentForegroundColor.PlainSequence;
            set => accentForegroundColor = new Color(value);
        }

        /// <summary>
        /// Accent color (background)
        /// </summary>
        public static string AccentBackgroundColor
        {
            get => accentBackgroundColor.PlainSequence;
            set => accentBackgroundColor = new Color(value);
        }

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(ThemeColorType type) =>
            GetColor(type.ToString());

        /// <summary>
        /// Gets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        public static Color GetColor(string type)
        {
            UpdateColorList();
            string plainColorSeq = themeColors[type].PlainSequence;
            ConsoleLogger.Debug("Getting color type {0}: {1}", type.ToString(), plainColorSeq);
            return new(plainColorSeq);
        }

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(ThemeColorType type, Color color) =>
            SetColor(type.ToString(), color);

        /// <summary>
        /// Sets a color from the color type
        /// </summary>
        /// <param name="type">Color type</param>
        /// <param name="color">Color to be set</param>
        public static Color SetColor(string type, Color color)
        {
            UpdateColorList();
            ConsoleLogger.Debug("Setting color type {0} to color sequence {1}...", type.ToString(), color.PlainSequence);
            return themeColors[type] = color;
        }

        /// <summary>
        /// Populate the empty color dictionary
        /// </summary>
        public static Dictionary<string, Color> PopulateColorsEmpty() =>
            PopulateColors(ThemeColorPopulationType.Empty);

        /// <summary>
        /// Populate the default color dictionary
        /// </summary>
        public static Dictionary<string, Color> PopulateColorsDefault() =>
            PopulateColors(ThemeColorPopulationType.Default);

        /// <summary>
        /// Populate the current color dictionary
        /// </summary>
        public static Dictionary<string, Color> PopulateColorsCurrent() =>
            PopulateColors(ThemeColorPopulationType.Current);

        private static Dictionary<string, Color> PopulateColors(ThemeColorPopulationType populationType)
        {
            Dictionary<string, Color> colors = [];
            ThemeInfo? themeInfo = default;

            // Check for cached default and empty colors
            if (populationType == ThemeColorPopulationType.Empty && themeEmptyColors?.Count > 0)
                return new(themeEmptyColors);
            if (populationType == ThemeColorPopulationType.Default && themeDefaultColors?.Count > 0)
                return new(themeDefaultColors);

            // Select population type
            for (int typeIndex = 0; typeIndex < Enum.GetValues(typeof(ThemeColorType)).Length; typeIndex++)
            {
                // Necessary variables
                ThemeColorType type = ThemeColorType.NeutralText;
                Color color = Color.Empty;

                // Now, change the two above variables depending on the type.
                switch (populationType)
                {
                    case ThemeColorPopulationType.Empty:
                        // Population type is empty colors
                        type = (ThemeColorType)Enum.Parse(typeof(ThemeColorType), typeIndex.ToString());
                        color = type != ThemeColorType.Background ? new Color(ConsoleColors.White) : Color.Empty;
                        break;
                    case ThemeColorPopulationType.Default:
                        // Population type is default colors
                        themeInfo ??= new();
                        type = (ThemeColorType)Enum.Parse(typeof(ThemeColorType), typeIndex.ToString());
                        color = themeInfo.GetColor(type);
                        ConsoleLogger.Debug("[DEFAULT] Adding color type {0} with color {1}...", type, color.PlainSequence);
                        break;
                    case ThemeColorPopulationType.Current:
                        // Population type is current colors
                        type = (ThemeColorType)Enum.Parse(typeof(ThemeColorType), typeIndex.ToString());
                        color = GetColor(type);
                        ConsoleLogger.Debug("[CURRENT] Adding color type {0} with color {1}...", type, color.PlainSequence);
                        break;
                }
                colors.Add(type.ToString(), color);
            }

            // Return it
            ConsoleLogger.Debug("Populated {0} colors.", colors.Count);
            return colors;
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(ThemeColorType colorType, bool resetBack = true) =>
            SetConsoleColor(colorType.ToString(), false, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(ThemeColorType colorType, bool Background, bool resetBack = true) =>
            SetConsoleColor(colorType.ToString(), Background, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(string colorType, bool resetBack = true) =>
            SetConsoleColor(colorType, false, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColor(string colorType, bool Background, bool resetBack = true)
        {
            ConsoleColoring.SetConsoleColor(GetColor(colorType), Background);
            if (!Background && resetBack)
                ConsoleColoring.SetConsoleColor(GetColor(ThemeColorType.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ThemeColorType colorType) =>
            TrySetConsoleColor(colorType.ToString(), false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(ThemeColorType colorType, bool Background) =>
            TrySetConsoleColor(colorType.ToString(), Background);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(string colorType) =>
            TrySetConsoleColor(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(string colorType, bool Background)
        {
            try
            {
                SetConsoleColor(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColorDry(ThemeColorType colorType, bool resetBack = true) =>
            SetConsoleColorDry(colorType.ToString(), false, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColorDry(ThemeColorType colorType, bool Background, bool resetBack = true) =>
            SetConsoleColorDry(colorType.ToString(), Background, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColorDry(string colorType, bool resetBack = true) =>
            SetConsoleColorDry(colorType, false, resetBack);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <param name="resetBack">If the color is not a background, do we reset the background color?</param>
        public static void SetConsoleColorDry(string colorType, bool Background, bool resetBack = true)
        {
            ConsoleColoring.SetConsoleColorDry(GetColor(colorType), Background);
            if (!Background && resetBack)
                ConsoleColoring.SetConsoleColorDry(GetColor(ThemeColorType.Background), true);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(ThemeColorType colorType) =>
            TrySetConsoleColorDry(colorType.ToString(), false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(ThemeColorType colorType, bool Background) =>
            TrySetConsoleColorDry(colorType.ToString(), Background);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(string colorType) =>
            TrySetConsoleColorDry(colorType, false);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="Background">Is the color a background color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(string colorType, bool Background)
        {
            try
            {
                SetConsoleColorDry(colorType, Background);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        /// <param name="useThemeColors">Whether to use the theme colors or to use the default terminal colors</param>
        public static void ResetColors(bool useThemeColors = false)
        {
            ResetBackground(useThemeColors);
            ResetForeground(useThemeColors);
        }

        /// <summary>
        /// Resets the background console color without clearing screen
        /// </summary>
        /// <param name="useThemeColors">Whether to use the theme colors or to use the default terminal colors</param>
        public static void ResetBackground(bool useThemeColors = false)
        {
            if (useThemeColors)
                SetConsoleColor(ThemeColorType.Background, Background: true);
            else
                ConsoleColoring.ResetBackground();
        }

        /// <summary>
        /// Resets the foreground console color without clearing screen
        /// </summary>
        /// <param name="useThemeColors">Whether to use the theme colors or to use the default terminal colors</param>
        public static void ResetForeground(bool useThemeColors = false)
        {
            if (useThemeColors)
                SetConsoleColor(ThemeColorType.NeutralText);
            else
                ConsoleColoring.ResetForeground();
        }

        /// <summary>
        /// Loads the background color by clearing the screen to the theme background color
        /// </summary>
        public static void LoadBackground() =>
            ConsoleColoring.LoadBack(GetColor(ThemeColorType.Background));

        private static void UpdateColorList()
        {
            lock (ThemeTools.locker)
            {
                if (ThemeTools.themes is null)
                    return;
                foreach (var theme in ThemeTools.themes.Values)
                    theme.UpdateColorsTools();
            }
        }
    }
}
