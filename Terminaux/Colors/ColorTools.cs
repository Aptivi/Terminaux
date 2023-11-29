
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Terminaux.Colors.Accessibility;
using System;
using Terminaux.Sequences.Builder;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color type enumeration
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// Color is a true color
        /// </summary>
        TrueColor,
        /// <summary>
        /// Color is a 8-bit color
        /// </summary>
        _255Color,
        /// <summary>
        /// Color is a 4-bit color
        /// </summary>
        _16Color
    }

    /// <summary>
    /// Color tools and management
    /// </summary>
    public static class ColorTools
    {
        internal static Color currentForegroundColor = new(ConsoleColors.White);
        internal static Color currentBackgroundColor = Color.Empty;
        internal static Color _empty;
        internal static Random rng = new();
        private static double _deficiency = 0.6;

        /// <summary>
        /// Enables the color transformation to adjust to color blindness upon making a new instance of color
        /// </summary>
        public static bool EnableColorTransformation { get; set; } = false;
        /// <summary>
        /// Enables the simple color transformation. This changes formula from Brettel 1997 (value is false) to Vienot 1999 (value is true)
        /// </summary>
        public static bool EnableSimpleColorTransformation { get; set; } = false;
        /// <summary>
        /// If enabled, calls to <see cref="Color.PlainSequence"/> and its siblings return color ID if said color is either a 256 color or a 16 color.
        /// Otherwise, calls to these properties are wrappers to <see cref="Color.PlainSequenceTrueColor"/> and its siblings. By default, it's enabled.
        /// </summary>
        public static bool UseTerminalPalette { get; set; } = true;

        /// <summary>
        /// The color deficiency or color blindness type
        /// </summary>
        public static Deficiency ColorDeficiency { get; set; } = Deficiency.Protan;

        /// <summary>
        /// The color deficiency severity
        /// </summary>
        public static double ColorDeficiencySeverity { 
            get => _deficiency;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                if (value > 1)
                    throw new ArgumentOutOfRangeException("value");

                _deficiency = value;
            }
        }

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
                ConsoleWrappers.ActionClear();
            }
            catch (Exception ex)
            {
                throw new TerminauxException($"Failed to set background: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the gray color according to the brightness of the background color
        /// </summary>
        public static Color GetGray()
        {
            if (currentBackgroundColor.IsBright)
                return new Color(ConsoleColors.Black);
            else
                return new Color(ConsoleColors.Gray);
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <param name="ForceSet">Force set background even if background setting is disabled</param>
        /// <param name="canSetBackground">Can the console set the background?</param>
        public static void SetConsoleColor(Color ColorSequence, bool Background = false, bool ForceSet = false, bool canSetBackground = true)
        {
            if (ColorSequence is null)
                throw new ArgumentNullException(nameof(ColorSequence));

            // Define reset background sequence
            string resetSequence = VtSequenceBasicChars.EscapeChar + $"[49m";

            // Set background
            if (Background)
            {
                if (canSetBackground | ForceSet)
                {
                    TextWriterColor.WritePlain(ColorSequence.VTSequenceBackground, false);
                    currentBackgroundColor = ColorSequence;
                }
                else if (!canSetBackground)
                {
                    TextWriterColor.WritePlain(resetSequence, false);
                    currentBackgroundColor = Color.Empty;
                }
            }
            else
            {
                TextWriterColor.WritePlain(ColorSequence.VTSequenceForeground, false);
                currentForegroundColor = ColorSequence;
            }
        }

        /// <summary>
        /// Sets the console color
        /// </summary>
        /// <param name="ColorSequence">The color instance</param>
        /// <param name="Background">Whether to set background or not</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TrySetConsoleColor(Color ColorSequence, bool Background)
        {
            try
            {
                SetConsoleColor(ColorSequence, Background);
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
        /// Converts from the hexadecimal representation of a color to the RGB sequence
        /// </summary>
        /// <param name="Hex">A hexadecimal representation of a color (#AABBCC for example)</param>
        /// <returns>&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</returns>
        public static string ConvertFromHexToRGB(string Hex)
        {
            if (Hex.StartsWith("#"))
            {
                int ColorDecimal = Convert.ToInt32(Hex.Substring(1), 16);
                int R = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
                int G = (byte)((ColorDecimal & 0xFF00) >> 8);
                int B = (byte)(ColorDecimal & 0xFF);
                return $"{R};{G};{B}";
            }
            else
            {
                throw new TerminauxException("Invalid hex color specifier.");
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="RGBSequence">&lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(string RGBSequence)
        {
            if (RGBSequence.Contains(Convert.ToString(';')))
            {
                // Split the VT sequence into three parts
                var ColorSpecifierArray = RGBSequence.Split(';');
                if (ColorSpecifierArray.Length == 3)
                {
                    int R = Convert.ToInt32(ColorSpecifierArray[0]);
                    int G = Convert.ToInt32(ColorSpecifierArray[1]);
                    int B = Convert.ToInt32(ColorSpecifierArray[2]);
                    return $"#{R:X2}{G:X2}{B:X2}";
                }
                else
                {
                    throw new TerminauxException("Invalid RGB color specifier.");
                }
            }
            else
            {
                throw new TerminauxException("Invalid RGB color specifier.");
            }
        }

        /// <summary>
        /// Converts from the RGB sequence of a color to the hexadecimal representation
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <returns>A hexadecimal representation of a color (#AABBCC for example)</returns>
        public static string ConvertFromRGBToHex(int R, int G, int B)
        {
            if (R < 0 | R > 255)
                throw new TerminauxException("Invalid red color specifier.");
            if (G < 0 | G > 255)
                throw new TerminauxException("Invalid green color specifier.");
            if (B < 0 | B > 255)
                throw new TerminauxException("Invalid blue color specifier.");
            return $"#{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        /// Gets a random color instance
        /// </summary>
        /// <param name="type">Color type to generate</param>
        /// <param name="selectBlack">Whether to select the black color or not</param>
        /// <returns>A color instance</returns>
        public static Color GetRandomColor(ColorType type, bool selectBlack = true) =>
            GetRandomColor(type,
                selectBlack ? 0 : 1, type != ColorType._16Color ? 255 : 15,
                selectBlack ? 0 : 1, 255,
                selectBlack ? 0 : 1, 255,
                selectBlack ? 0 : 1, 255);

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
        /// Translates the color from .NET's <see cref="ConsoleColor"/> to X11's representation, <see cref="ConsoleColors"/>
        /// </summary>
        /// <param name="color">.NET's <see cref="ConsoleColor"/> to translate this color to</param>
        /// <returns>X11's representation of this color, <see cref="ConsoleColors"/></returns>
        public static ConsoleColors TranslateToX11ColorMap(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black          => ConsoleColors.Black,
                ConsoleColor.DarkBlue       => ConsoleColors.DarkRed,
                ConsoleColor.DarkGreen      => ConsoleColors.DarkGreen,
                ConsoleColor.DarkCyan       => ConsoleColors.DarkYellow,
                ConsoleColor.DarkRed        => ConsoleColors.DarkBlue,
                ConsoleColor.DarkMagenta    => ConsoleColors.DarkMagenta,
                ConsoleColor.DarkYellow     => ConsoleColors.DarkCyan,
                ConsoleColor.Gray           => ConsoleColors.Gray,
                ConsoleColor.DarkGray       => ConsoleColors.DarkGray,
                ConsoleColor.Blue           => ConsoleColors.Red,
                ConsoleColor.Green          => ConsoleColors.Green,
                ConsoleColor.Cyan           => ConsoleColors.Yellow,
                ConsoleColor.Red            => ConsoleColors.Blue,
                ConsoleColor.Magenta        => ConsoleColors.Magenta,
                ConsoleColor.Yellow         => ConsoleColors.Cyan,
                ConsoleColor.White          => ConsoleColors.White,
                _                           => ConsoleColors.Black,
            };
        }

        /// <summary>
        /// Translates the color from X11's <see cref="ConsoleColors"/> to .NET's representation, <see cref="ConsoleColor"/>
        /// </summary>
        /// <param name="color">X11's <see cref="ConsoleColors"/> to translate this color to</param>
        /// <returns>.NET's representation of this color, <see cref="ConsoleColor"/></returns>
        public static ConsoleColor TranslateToStandardColorMap(ConsoleColors color)
        {
            return color switch
            {
                ConsoleColors.Black         => ConsoleColor.Black,
                ConsoleColors.DarkRed       => ConsoleColor.DarkBlue,
                ConsoleColors.DarkGreen     => ConsoleColor.DarkGreen,
                ConsoleColors.DarkYellow    => ConsoleColor.DarkCyan,
                ConsoleColors.DarkBlue      => ConsoleColor.DarkRed,
                ConsoleColors.DarkMagenta   => ConsoleColor.DarkMagenta,
                ConsoleColors.DarkCyan      => ConsoleColor.DarkYellow,
                ConsoleColors.Gray          => ConsoleColor.Gray,
                ConsoleColors.DarkGray      => ConsoleColor.DarkGray,
                ConsoleColors.Red           => ConsoleColor.Blue,
                ConsoleColors.Green         => ConsoleColor.Green,
                ConsoleColors.Yellow        => ConsoleColor.Cyan,
                ConsoleColors.Blue          => ConsoleColor.Red,
                ConsoleColors.Magenta       => ConsoleColor.Magenta,
                ConsoleColors.Cyan          => ConsoleColor.Yellow,
                ConsoleColors.White         => ConsoleColor.White,
                _                           => ConsoleColor.Black,
            };
        }

        /// <summary>
        /// Corrects the color map for <see cref="ConsoleColor"/> according to the X11 specification
        /// </summary>
        /// <param name="color">.NET's <see cref="ConsoleColor"/> to correct this color</param>
        /// <returns>Corrected <see cref="ConsoleColor"/></returns>
        public static ConsoleColor CorrectStandardColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black          => ConsoleColor.Black,
                ConsoleColor.DarkBlue       => ConsoleColor.DarkRed,
                ConsoleColor.DarkGreen      => ConsoleColor.DarkGreen,
                ConsoleColor.DarkCyan       => ConsoleColor.DarkYellow,
                ConsoleColor.DarkRed        => ConsoleColor.DarkBlue,
                ConsoleColor.DarkMagenta    => ConsoleColor.DarkMagenta,
                ConsoleColor.DarkYellow     => ConsoleColor.DarkCyan,
                ConsoleColor.Gray           => ConsoleColor.Gray,
                ConsoleColor.DarkGray       => ConsoleColor.DarkGray,
                ConsoleColor.Blue           => ConsoleColor.Red,
                ConsoleColor.Green          => ConsoleColor.Green,
                ConsoleColor.Cyan           => ConsoleColor.Yellow,
                ConsoleColor.Red            => ConsoleColor.Blue,
                ConsoleColor.Magenta        => ConsoleColor.Magenta,
                ConsoleColor.Yellow         => ConsoleColor.Cyan,
                ConsoleColor.White          => ConsoleColor.White,
                _                           => ConsoleColor.Black,
            };
        }

        /// <summary>
        /// Provides you an easy way to generate new <see cref="Color"/> instances with color blindness applied
        /// </summary>
        /// <param name="color">Color to use</param>
        /// <param name="deficiency">Selected deficiency for color blindness</param>
        /// <param name="severity">Severity of the color blindness</param>
        /// <param name="useSimple">Uses the simple method to calculate color values based on color blindness deficiency and severity</param>
        /// <returns>An instance of <see cref="Color"/> with adjusted color values for color-blindness</returns>
        public static Color RenderColorBlindnessAware(Color color, Deficiency deficiency, double severity, bool useSimple = false)
        {
            // Get some old values
            var oldDeficiency = ColorDeficiency;
            var oldSeverity = ColorDeficiencySeverity;
            var oldTransform = EnableColorTransformation;
            var oldTransformSimple = EnableSimpleColorTransformation;

            // Now, enable transformation prior to rendering
            EnableColorTransformation = true;
            EnableSimpleColorTransformation = useSimple;
            ColorDeficiencySeverity = severity;
            ColorDeficiency = deficiency;

            // Get the resulting color
            var result = new Color(color.PlainSequence);

            // Restore the values
            ColorDeficiency = oldDeficiency;
            ColorDeficiencySeverity = oldSeverity;
            EnableColorTransformation = oldTransform;
            EnableSimpleColorTransformation = oldTransformSimple;

            // Return the resulting color
            return result;
        }
    }
}
