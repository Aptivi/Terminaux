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
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation.Contrast;
using Terminaux.Colors.Models;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color tools and management
    /// </summary>
    public static class ColorTools
    {
        internal static Color _empty = new(0, new());
        internal static Random rng = new();
        internal static readonly ColorSettings globalSettings = new();

        /// <summary>
        /// Global color settings
        /// </summary>
        public static ColorSettings GlobalSettings =>
            globalSettings ?? new();

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
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to parse color specifier {0}", ColorSpecifier);
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
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to parse color number {0}", ColorNum);
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
            catch (Exception ex)
            {
                ConsoleLogger.Error(ex, "Failed to parse color RGB {0}, {1}, {2}", R, G, B);
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
    }
}
