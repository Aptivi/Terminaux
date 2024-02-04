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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Transformation.Contrast;
using System.Text;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color tools and management
    /// </summary>
    public static class ColorTools
    {
        internal static Color currentForegroundColor = new(ConsoleColors.White);
        internal static Color currentBackgroundColor = Color.Empty;
        internal static Color _empty;
        internal static Random rng = new();
        private static readonly ColorSettings globalSettings = new();

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
        /// Converts from sRGB to Linear RGB using a color number
        /// </summary>
        /// <param name="colorNum">Color number from 0 to 255</param>
        /// <returns>Linear RGB number ranging from 0 to 1</returns>
        public static double SRGBToLinearRGB(int colorNum)
        {
            // Check the value
            if (colorNum < 0)
                colorNum = 0;
            if (colorNum > 255)
                colorNum = 255;

            // Now, convert sRGB to linear RGB (domain is [0, 1])
            double colorNumDbl = colorNum / 255d;
            if (colorNumDbl < 0.04045d)
                return colorNumDbl / 12.92d;
            return Math.Pow((colorNumDbl + 0.055d) / 1.055d, 2.4d);
        }

        /// <summary>
        /// Converts from Linear RGB to sRGB using a linear RGB number
        /// </summary>
        /// <param name="linear">Linear RGB number from 0 to 1</param>
        /// <returns>sRGB value from 0 to 255</returns>
        public static int LinearRGBTosRGB(double linear)
        {
            // Check the value
            if (linear <= 0)
                return 0;
            if (linear >= 1)
                return 255;

            // Now, convert linear value to RGB representation (domain is [0, 255])
            if (linear < 0.0031308d)
                return (int)(0.5d + (linear * 255d * 12.92));
            return (int)(255d * (Math.Pow(linear, 1d / 2.4d) * 1.055d - 0.055d));
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
                SetConsoleColor(ColorSequence, true, Force);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException($"Failed to set background: {ex.Message}");
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
                SetConsoleColorDry(ColorSequence, true, Force);
                ConsoleWrapper.Clear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException($"Failed to set background: {ex.Message}");
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
                        return new Color(ConsoleColors.Gray);
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSetBackground, true);

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background, ForceSet, canSetBackground);
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
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        public static void SetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true) =>
            SetConsoleColorInternal(ColorSequence, Background, ForceSet, canSetBackground, false);

        /// <summary>
        /// Sets the console color dryly
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColorDry(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true)
        {
            try
            {
                SetConsoleColorDry(ColorSequence, Background, ForceSet, canSetBackground);
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
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        public static string RenderSetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true)
        {
            if (ColorSequence is null)
                throw new TerminauxException("Color instance is not provided.");

            // Define reset background sequence
            string resetSequence = RenderResetBackground();

            // Render the background being set
            var builder = new StringBuilder();
            if (Background)
            {
                if (canSetBackground || ForceSet)
                    builder.Append(ColorSequence.VTSequenceBackground);
                else if (!canSetBackground)
                    builder.Append(resetSequence);
            }
            else
                builder.Append(ColorSequence.VTSequenceForeground);
            return builder.ToString();
        }

        /// <summary>
        /// Tries parsing the color from the specifier string
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(string ColorSpecifier)
        {
            try
            {
                var ColorInstance = new Color(ColorSpecifier);
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
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int ColorNum)
        {
            try
            {
                var ColorInstance = new Color(ColorNum);
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
        /// <returns>True if successful; False if failed</returns>
        public static bool TryParseColor(int R, int G, int B)
        {
            try
            {
                var ColorInstance = new Color(R, G, B);
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
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(bool selectBlack = true) =>
            GetRandomColor(ColorType.TrueColor, selectBlack);

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="selectBlack">Whether to select the black color or not</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, bool selectBlack = true)
        {
            int maxColor = type != ColorType._16Color ? 255 : 15;
            var color = GetRandomColor(type, 0, maxColor, 0, 255, 0, 255, 0, 255);
            int colorLevel = 0;
            var colorType = color.Type;
            if (colorType != ColorType.TrueColor)
                colorLevel = int.Parse(color.PlainSequence);
            while (!ColorContrast.IsSeeable(colorType, colorLevel, color.RGB.R, color.RGB.G, color.RGB.B) && !selectBlack)
            {
                color = GetRandomColor(type, 0, maxColor, 0, 255, 0, 255, 0, 255);
                if (colorType != ColorType.TrueColor)
                    colorLevel = int.Parse(color.PlainSequence);

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
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, int minColor, int maxColor, int minColorR, int maxColorR, int minColorG, int maxColorG, int minColorB, int maxColorB)
        {
            switch (type)
            {
                case ColorType._16Color:
                    int colorNum = rng.Next(minColor, maxColor);
                    return new Color(colorNum);
                case ColorType._255Color:
                    int colorNum2 = rng.Next(minColor, maxColor);
                    return new Color(colorNum2);
                case ColorType.TrueColor:
                    int colorNumR = rng.Next(minColorR, maxColorR);
                    int colorNumG = rng.Next(minColorG, maxColorG);
                    int colorNumB = rng.Next(minColorB, maxColorB);
                    return new Color(colorNumR, colorNumG, colorNumB);
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
            TextWriterRaw.WritePlain(RenderResetForeground(), false);
            currentForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Resets the background color without clearing screen
        /// </summary>
        public static void ResetBackground()
        {
            TextWriterRaw.WritePlain(RenderResetBackground(), false);
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
            $"\u001b[39m";

        /// <summary>
        /// Gets a sequence that resets the background color without clearing screen
        /// </summary>
        public static string RenderResetBackground() =>
            $"\u001b[49m";

        internal static string GetColorIdStringFrom(ConsoleColors colorDef) =>
            GetColorIdStringFrom((int)colorDef);

        internal static string GetColorIdStringFrom(int colorNum) =>
            colorNum >= 0 && colorNum <= (int)ConsoleColors.White ?
            $"{(int)ConversionTools.TranslateToX11ColorMap((ConsoleColor)colorNum)}" :
            $"{colorNum}";

        internal static void SetConsoleColorInternal(Color ColorSequence, bool Background, bool ForceSet, bool canSetBackground, bool needsToSetCurrentColors)
        {
            if (ColorSequence is null)
                throw new ArgumentNullException(nameof(ColorSequence));

            // Get the appropriate color setting sequence
            string sequence = RenderSetConsoleColor(ColorSequence, Background, ForceSet, canSetBackground);

            // Actually set the color
            TextWriterRaw.WritePlain(sequence, false);

            // Set current background color
            if (needsToSetCurrentColors)
            {
                if (Background)
                {
                    if (canSetBackground | ForceSet)
                        currentBackgroundColor = ColorSequence;
                    else if (!canSetBackground)
                        currentBackgroundColor = Color.Empty;
                }
                else
                    currentForegroundColor = ColorSequence;
            }
        }
    }
}
