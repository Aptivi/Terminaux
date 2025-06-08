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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation.Contrast;
using System.Text;
using Terminaux.Base.Checks;
using Terminaux.Colors.Models;
using Terminaux.Sequences.Builder;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color tools and management
    /// </summary>
    public static class ColorTools
    {
        internal static readonly Color _empty = new(0, new());
        internal static Color currentForegroundColor = new(ConsoleColors.White);
        internal static Color currentBackgroundColor = Color.Empty;
        internal static Random rng = new();
        private static readonly ColorSettings globalSettings = new();
        private static bool allowBackground;

        /// <summary>
        /// Global color settings
        /// </summary>
        public static ColorSettings GlobalSettings =>
            globalSettings ?? new();

        /// <summary>
        /// Current foreground color
        /// </summary>
        public static Color CurrentForegroundColor =>
            currentForegroundColor;

        /// <summary>
        /// Current background color
        /// </summary>
        public static Color CurrentBackgroundColor =>
            currentBackgroundColor;

        /// <summary>
        /// Whether applications are allowed to set the current background color or not
        /// </summary>
        public static bool AllowBackground
        {
            get => allowBackground;
            set => allowBackground = value;
        }

        /// <summary>
        /// Loads the background
        /// </summary>
        public static void LoadBack() =>
            LoadBack(currentBackgroundColor);

        /// <summary>
        /// Loads the background
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        /// <param name="Force">Force set background even if background setting is disabled</param>
        public static void LoadBack(Color ColorSequence, bool Force = false)
        {
            try
            {
                SetConsoleColor(CurrentForegroundColor, false, Force, true);
                SetConsoleColor(ColorSequence, true, Force, AllowBackground);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException("Failed to set background" + $": {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the background dryly
        /// </summary>
        public static void LoadBackDry() =>
            LoadBackDry(currentBackgroundColor);

        /// <summary>
        /// Loads the background dryly
        /// </summary>
        /// <param name="ColorSequence">Color sequence used to load background</param>
        /// <param name="Force">Force set background even if background setting is disabled</param>
        public static void LoadBackDry(Color ColorSequence, bool Force = false)
        {
            try
            {
                SetConsoleColorDry(ColorSequence, true, Force, AllowBackground);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException("Failed to set background" + $": {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        /// <param name="contrastType">Contrast type</param>
        public static Color GetGray(ColorContrastType contrastType = ColorContrastType.Light) =>
            GetGray(currentBackgroundColor, contrastType);

        /// <summary>
        /// Gets the gray color according to the brightness of the specified color
        /// </summary>
        /// <param name="color">Target color to use when getting the gray color</param>
        /// <param name="contrastType">Contrast type</param>
        public static Color GetGray(Color color, ColorContrastType contrastType = ColorContrastType.Light)
        {
            switch (contrastType)
            {
                case ColorContrastType.Half:
                    return ColorContrast.GetContrastColorHalf(color);
                case ColorContrastType.Ntsc:
                    return ColorContrast.GetContrastColorNtsc(color);
                default:
                    if (color.Brightness == ColorBrightness.Light)
                        return new Color(ConsoleColors.Black);
                    else
                        return new Color(ConsoleColors.Silver);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background) =>
            SetConsoleColorInternal(ColorSequence, Background, ColorSequence != CurrentBackgroundColor, !Background || AllowBackground, true);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSet, true);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background, ForceSet, canSet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static void SetConsoleColorDry(Color ColorSequence, bool Background) =>
            SetConsoleColorInternal(ColorSequence, Background, ColorSequence != CurrentBackgroundColor, !Background || AllowBackground, false);

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static void SetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSet, false);

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            try
            {
                SetConsoleColorDry(ColorSequence, Background, ForceSet, canSet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        public static string RenderSetConsoleColor(Color ColorSequence) =>
            RenderSetConsoleColor(ColorSequence, false, ColorSequence != CurrentForegroundColor, true);

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        public static string RenderSetConsoleColor(Color ColorSequence, bool Background) =>
            RenderSetConsoleColor(ColorSequence, Background, ColorSequence != CurrentBackgroundColor, !Background || AllowBackground);

        /// <summary>
        /// Gets the console color setting sequence
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled or if the current background color doesn't match the provided color</param>
        /// <param name="canSet">Can the console set this color?</param>
        public static string RenderSetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSet = true)
        {
            if (ColorSequence is null)
                throw new TerminauxException("Color instance is not provided.");

            // Define reset background sequence
            string resetBgSequence = RenderResetBackground();
            string resetFgSequence = RenderResetForeground();

            // Render the background being set
            var builder = new StringBuilder();
            if (Background)
            {
                if (canSet || ForceSet)
                    builder.Append(ColorSequence.VTSequenceBackground);
                else if (!canSet)
                    builder.Append(resetBgSequence);
            }
            else
            {
                if (canSet || ForceSet)
                    builder.Append(ColorSequence.VTSequenceForeground);
                else if (!canSet)
                    builder.Append(resetFgSequence);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? GlobalSettings;

            try
            {
                var ColorInstance = new Color(ColorSpecifier, finalSettings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? GlobalSettings;

            try
            {
                var ColorInstance = new Color(ColorNum, finalSettings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? GlobalSettings;

            try
            {
                var ColorInstance = new Color(R, G, B, finalSettings);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a random color instance (true color)
        /// </summary>
        /// <param name="selectBlack">Whether to select the black color or not</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(bool selectBlack = true, ColorSettings? settings = null) =>
            GetRandomColor(ColorType.TrueColor, selectBlack, settings);

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="selectBlack">Whether to select the black color or not</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, bool selectBlack = true, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? GlobalSettings;
            int maxColor = type != ColorType.FourBitColor ? 256 : 16;
            Color color = Color.Empty;
            bool initial = true;
            while (initial || (!ColorContrast.IsSeeable(color) && !selectBlack))
            {
                color = GetRandomColor(type, 0, maxColor, 0, maxColor, 0, maxColor, 0, maxColor, finalSettings);
                initial = false;
            }
            return color;
        }

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="minColor">The minimum color level</param>
        /// <param name="maxColor">The maximum color level</param>
        /// <param name="minColorR">The minimum red color level</param>
        /// <param name="maxColorR">The maximum red color level</param>
        /// <param name="minColorG">The minimum green color level</param>
        /// <param name="maxColorG">The maximum green color level</param>
        /// <param name="minColorB">The minimum blue color level</param>
        /// <param name="maxColorB">The maximum blue color level</param>
        /// <param name="settings">Settings to use</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, int minColor, int maxColor, int minColorR, int maxColorR, int minColorG, int maxColorG, int minColorB, int maxColorB, ColorSettings? settings = null)
        {
            // Select appropriate settings
            var finalSettings = settings ?? GlobalSettings;
            int maxColorLevel = type == ColorType.FourBitColor ? 16 : 256;
            maxColor = maxColor > maxColorLevel ? maxColorLevel : maxColor;
            switch (type)
            {
                case ColorType.FourBitColor:
                case ColorType.EightBitColor:
                    int colorNum = rng.Next(minColor, maxColor);
                    return new Color(colorNum, finalSettings);
                case ColorType.TrueColor:
                    int colorNumR = rng.Next(minColorR, maxColorR);
                    int colorNumG = rng.Next(minColorG, maxColorG);
                    int colorNumB = rng.Next(minColorB, maxColorB);
                    return new Color(colorNumR, colorNumG, colorNumB, finalSettings);
                default:
                    return Color.Empty;
            }
        }

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        public static void ResetColors()
        {
            ResetForeground();
            ResetBackground();
        }

        /// <summary>
        /// Resets the foreground color without clearing screen
        /// </summary>
        public static void ResetForeground()
        {
            TextWriterRaw.WriteRaw(RenderResetForeground());
            currentForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Resets the background color without clearing screen
        /// </summary>
        public static void ResetBackground()
        {
            TextWriterRaw.WriteRaw(RenderResetBackground());
            currentBackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Gets a sequence that resets the console colors without clearing screen
        /// </summary>
        public static string RenderResetColors() =>
            RenderResetForeground() + RenderResetBackground();

        /// <summary>
        /// Gets a sequence that resets the foreground color without clearing screen
        /// </summary>
        public static string RenderResetForeground() =>
            $"{VtSequenceBasicChars.EscapeChar}[39m";

        /// <summary>
        /// Gets a sequence that resets the background color without clearing screen
        /// </summary>
        public static string RenderResetBackground() =>
            $"{VtSequenceBasicChars.EscapeChar}[49m";

        /// <summary>
        /// Reverts the console colors without clearing screen
        /// </summary>
        public static void RevertColors()
        {
            RevertForeground();
            RevertBackground();
        }

        /// <summary>
        /// Reverts the foreground color without clearing screen
        /// </summary>
        public static void RevertForeground() =>
            TextWriterRaw.WriteRaw(RenderRevertForeground());

        /// <summary>
        /// Reverts the background color without clearing screen
        /// </summary>
        public static void RevertBackground() =>
            TextWriterRaw.WriteRaw(RenderRevertBackground());

        /// <summary>
        /// Gets a sequence that reverts the console colors without clearing screen
        /// </summary>
        public static string RenderRevertColors() =>
            RenderRevertForeground() + RenderRevertBackground();

        /// <summary>
        /// Gets a sequence that reverts the foreground color without clearing screen
        /// </summary>
        public static string RenderRevertForeground() =>
            RenderSetConsoleColor(currentForegroundColor);

        /// <summary>
        /// Gets a sequence that reverts the background color without clearing screen
        /// </summary>
        public static string RenderRevertBackground() =>
            RenderSetConsoleColor(currentBackgroundColor, true);

        /// <summary>
        /// Gets the RGB specifier from the color code
        /// </summary>
        /// <param name="colorCode">The color code to get the RGB specifier from</param>
        /// <returns>The RGB specifier string</returns>
        public static string GetRgbSpecifierFromColorCode(int colorCode)
        {
            (int r, int g, int b) = GetRgbIntFromColorCode(colorCode);
            return $"{r};{g};{b}";
        }

        /// <summary>
        /// Gets the RGB instance from the color code
        /// </summary>
        /// <param name="colorCode">The color code to get the RGB specifier from</param>
        /// <returns>The RGB specifier string</returns>
        public static RedGreenBlue GetRgbFromColorCode(int colorCode)
        {
            (int r, int g, int b) = GetRgbIntFromColorCode(colorCode);
            return new(r, g, b);
        }

        /// <summary>
        /// Gets the RGB numbers from the color code
        /// </summary>
        /// <param name="colorCode">The color code to get the RGB specifier from</param>
        /// <returns>The RGB specifier string</returns>
        public static (int R, int G, int B) GetRgbIntFromColorCode(int colorCode) =>
            ((colorCode) & 0xff, (colorCode >> 8) & 0xff, (colorCode >> 16) & 0xff);

        internal static string GetColorIdStringFrom(ConsoleColors colorDef) =>
            GetColorIdStringFrom((int)colorDef);

        internal static string GetColorIdStringFrom(int colorNum) =>
            colorNum >= 0 && colorNum <= (int)ConsoleColors.White ?
            $"{(int)ConversionTools.TranslateToX11ColorMap((ConsoleColor)colorNum)}" :
            $"{colorNum}";

        internal static void SetConsoleColorInternal(Color ColorSequence, bool Background, bool ForceSet, bool canSet, bool needsToSetCurrentColors)
        {
            if (ColorSequence is null)
                throw new ArgumentNullException(nameof(ColorSequence));

            // Get the appropriate color setting sequence
            string sequence = RenderSetConsoleColor(ColorSequence, Background, ForceSet, canSet);

            // Actually set the color
            TextWriterRaw.WriteRaw(sequence);

            // Set current background color
            if (needsToSetCurrentColors)
            {
                if (Background)
                {
                    if (canSet | ForceSet)
                        currentBackgroundColor = ColorSequence;
                }
                else
                {
                    if (canSet | ForceSet)
                        currentForegroundColor = ColorSequence;
                }
            }
        }

        static ColorTools()
        {
            if (!ConsoleChecker.busy)
                ConsoleChecker.CheckConsole();
        }
    }
}
