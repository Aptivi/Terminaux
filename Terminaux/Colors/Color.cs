
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terminaux.Colors.Models;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color information class
    /// </summary>
    [DebuggerDisplay("Color = {PlainSequenceEnclosed}, TrueColor = {PlainSequenceEnclosedTrueColor}")]
    public class Color : IEquatable<Color>
    {
        private static (ConsoleColor unapplicable16, ConsoleColors unapplicable255) unapplicable = ((ConsoleColor)(-1), (ConsoleColors)(-1));
        private string plainSequence;
        private string foreSequence;
        private string backSequence;

        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, depending on the usage of the terminal palette.
        /// </summary>
        public string PlainSequence
        {
            get => ColorTools.UseTerminalPalette ? PlainSequenceOriginal : PlainSequenceTrueColor;
            set => plainSequence = value;
        }
        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt; enclosed in quotes if necessary.
        /// </summary>
        public string PlainSequenceEnclosed =>
            Type == ColorType.TrueColor ? $"\"{PlainSequence}\"" : PlainSequence;
        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt; in its original form.
        /// </summary>
        public string PlainSequenceOriginal =>
            plainSequence;
        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public string VTSequenceForeground
        {
            get => ColorTools.UseTerminalPalette ? VTSequenceForegroundOriginal : VTSequenceForegroundTrueColor;
            set => foreSequence = value;
        }
        /// <summary>
        /// Parsable VT sequence (Foreground, original)
        /// </summary>
        public string VTSequenceForegroundOriginal =>
            foreSequence;
        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public string VTSequenceBackground
        {
            get => ColorTools.UseTerminalPalette ? VTSequenceBackgroundOriginal : VTSequenceBackgroundTrueColor;
            set => backSequence = value;
        }
        /// <summary>
        /// Parsable VT sequence (Background, original)
        /// </summary>
        public string VTSequenceBackgroundOriginal =>
            backSequence;
        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public string PlainSequenceTrueColor { get; private set; }
        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt; enclosed in quotes if necessary
        /// </summary>
        public string PlainSequenceEnclosedTrueColor { get; private set; }
        /// <summary>
        /// Parsable VT sequence (Foreground, true color)
        /// </summary>
        public string VTSequenceForegroundTrueColor { get; private set; }
        /// <summary>
        /// Parsable VT sequence (Background, true color)
        /// </summary>
        public string VTSequenceBackgroundTrueColor { get; private set; }
        /// <summary>
        /// The red color value
        /// </summary>
        public int R { get; private set; }
        /// <summary>
        /// The green color value
        /// </summary>
        public int G { get; private set; }
        /// <summary>
        /// The blue color value
        /// </summary>
        public int B { get; private set; }
        /// <summary>
        /// An instance of RGB
        /// </summary>
        public RedGreenBlue RGB =>
            new(R, G, B);
        /// <summary>
        /// An instance of CMYK
        /// </summary>
        public CyanMagentaYellowKey CMYK =>
            RGB.ConvertToCmyk();
        /// <summary>
        /// An instance of CMY
        /// </summary>
        public CyanMagentaYellow CMY =>
            RGB.ConvertToCmy();
        /// <summary>
        /// An instance of HSL
        /// </summary>
        public HueSaturationLightness HSL =>
            RGB.ConvertToHsl();
        /// <summary>
        /// An instance of HSV
        /// </summary>
        public HueSaturationValue HSV =>
            RGB.ConvertToHsv();
        /// <summary>
        /// An instance of RYB
        /// </summary>
        public RedYellowBlue RYB =>
            RGB.ConvertToRyb();
        /// <summary>
        /// Hexadecimal representation of the color
        /// </summary>
        public string Hex { get; private set; }
        /// <summary>
        /// Color type
        /// </summary>
        public ColorType Type { get; private set; }
        /// <summary>
        /// Determines the color brightness whether it indicates dark or light mode
        /// </summary>
        public ColorBrightness Brightness { get; private set; }
        /// <summary>
        /// The color value converted to <see cref="ConsoleColors"/>. Not applicable [-1] to true color
        /// </summary>
        public ConsoleColors ColorEnum255 { get; private set; } = unapplicable.unapplicable255;
        /// <summary>
        /// The color value converted to <see cref="ConsoleColor"/>. Not applicable [-1] to true color and 256 colors
        /// </summary>
        public ConsoleColor ColorEnum16 { get; private set; } = unapplicable.unapplicable16;
        /// <summary>
        /// Empty color singleton
        /// </summary>
        public static Color Empty
        {
            get
            {
                // Get cached value if cached
                if (ColorTools._empty is not null)
                    return ColorTools._empty;

                // Else, cache the empty value and return it
                bool orig = ColorTools.UseTerminalPalette;
                ColorTools.UseTerminalPalette = true;
                ColorTools._empty = new Color(0);
                ColorTools.UseTerminalPalette = orig;
                return ColorTools._empty;
            }
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <exception cref="ColorSeqException"></exception>
        public Color(int R, int G, int B)
            : this($"{R};{G};{B}")
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <exception cref="ColorSeqException"></exception>
        public Color(ConsoleColors ColorDef)
            : this(Convert.ToInt32(ColorDef))
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <exception cref="ColorSeqException"></exception>
        public Color(ConsoleColor ColorDef)
            : this(Convert.ToInt32(ColorTools.CorrectStandardColor(ColorDef)))
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="ColorSeqException"></exception>
        public Color(int ColorNum)
            : this($"{ColorNum}")
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <exception cref="ColorSeqException"></exception>
        public Color(string ColorSpecifier)
        {
            // Remove stray double quotes
            ColorSpecifier = ColorSpecifier.Replace("\"", "");

            // Now, parse the output
            if (ColorSpecifier.Contains(";"))
            {
                // Parse it
                var rgb =
                    ColorSpecifier.StartsWith("cmyk:") ? ColorParser.ParseSpecifierCmykValues(ColorSpecifier) :
                    ColorSpecifier.StartsWith("cmy:") ? ColorParser.ParseSpecifierCmyValues(ColorSpecifier) :
                    ColorSpecifier.StartsWith("hsl:") ? ColorParser.ParseSpecifierHslValues(ColorSpecifier) :
                    ColorSpecifier.StartsWith("hsv:") ? ColorParser.ParseSpecifierHsvValues(ColorSpecifier) :
                    ColorSpecifier.StartsWith("ryb:") ? ColorParser.ParseSpecifierRybValues(ColorSpecifier) :
                    ColorParser.ParseSpecifierRgbValues(ColorSpecifier);
                int r = rgb.R;
                int g = rgb.G;
                int b = rgb.B;

                // Form the sequences
                PlainSequence = PlainSequenceTrueColor = $"{r};{g};{b}";
                PlainSequenceEnclosedTrueColor = $"\"{r};{g};{b}\"";
                VTSequenceForeground = VTSequenceForegroundTrueColor = $"\u001b[38;2;{PlainSequence}m";
                VTSequenceBackground = VTSequenceBackgroundTrueColor = $"\u001b[48;2;{PlainSequence}m";

                // Populate color properties
                Type = ColorType.TrueColor;
                Brightness = DetectDark(r, g, b) ? ColorBrightness.Dark : ColorBrightness.Light;
                R = r;
                G = g;
                B = b;
            }
            else if (double.TryParse(ColorSpecifier, out double specifierNum) && specifierNum <= 255 || Enum.IsDefined(typeof(ConsoleColors), ColorSpecifier))
            {
                var parsedEnum = (ConsoleColors)Enum.Parse(typeof(ConsoleColors), ColorSpecifier);
                var parsedEnum16 = specifierNum <= 15 ? (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ColorSpecifier) : default;

                // Parse it
                var rgb = ColorParser.ParseSpecifierRgbName(ColorSpecifier);
                var colorsInfo = rgb.cci;
                int r = rgb.rgb.R;
                int g = rgb.rgb.G;
                int b = rgb.rgb.B;

                // Form the sequences
                PlainSequence = ColorTools.EnableColorTransformation ? $"{r};{g};{b}" : $"{colorsInfo.ColorID}";
                PlainSequenceTrueColor = $"{r};{g};{b}";
                PlainSequenceEnclosedTrueColor = $"\"{r};{g};{b}\"";
                VTSequenceForeground = ColorTools.EnableColorTransformation ? $"\u001b[38;2;{PlainSequence}m" : $"\u001b[38;5;{PlainSequence}m";
                VTSequenceBackground = ColorTools.EnableColorTransformation ? $"\u001b[48;2;{PlainSequence}m" : $"\u001b[48;5;{PlainSequence}m";
                VTSequenceForegroundTrueColor = $"\u001b[38;2;{PlainSequenceTrueColor}m";
                VTSequenceBackgroundTrueColor = $"\u001b[48;2;{PlainSequenceTrueColor}m";

                // Populate color properties
                Type = ColorTools.EnableColorTransformation ? ColorType.TrueColor : colorsInfo.ColorID >= 16 ? ColorType._255Color : ColorType._16Color;
                Brightness = DetectDark(r, g, b) ? ColorBrightness.Dark : ColorBrightness.Light;
                R = r;
                G = g;
                B = b;
                ColorEnum255 = Type == ColorType._255Color ? parsedEnum : (ConsoleColors)(-1);
                ColorEnum16 = Type == ColorType._16Color ? parsedEnum16 : (ConsoleColor)(-1);
            }
            else if (ColorSpecifier.StartsWith("#"))
            {
                // Parse it
                var rgb = ColorParser.ParseSpecifierRgbHash(ColorSpecifier);
                int r = rgb.R;
                int g = rgb.G;
                int b = rgb.B;

                // We got the RGB values! Form the sequences
                PlainSequence = PlainSequenceTrueColor = $"{r};{g};{b}";
                PlainSequenceEnclosedTrueColor = $"\"{r};{g};{b}\"";
                VTSequenceForeground = VTSequenceForegroundTrueColor = $"\u001b[38;2;{PlainSequence}m";
                VTSequenceBackground = VTSequenceBackgroundTrueColor = $"\u001b[48;2;{PlainSequence}m";

                // Populate color properties
                Type = ColorType.TrueColor;
                Brightness = DetectDark(r, g, b) ? ColorBrightness.Dark : ColorBrightness.Light;
                R = r;
                G = g;
                B = b;
            }
            else
            {
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
            }

            // Populate the hexadecimal representation of the color
            Hex = $"#{R:X2}{G:X2}{B:X2}";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <exception cref="ColorSeqException"></exception>
        public static implicit operator Color(ConsoleColors ColorDef) =>
            new(Convert.ToInt32(ColorDef));

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <exception cref="ColorSeqException"></exception>
        public static implicit operator Color(ConsoleColor ColorDef) =>
            new(Convert.ToInt32(ColorTools.CorrectStandardColor(ColorDef)));

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="ColorSeqException"></exception>
        public static implicit operator Color(int ColorNum) =>
            new($"{ColorNum}");

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <exception cref="ColorSeqException"></exception>
        public static implicit operator Color(string ColorSpecifier) =>
            new(ColorSpecifier);

        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, depending on the usage of the terminal palette.
        /// </summary>
        public override string ToString() =>
            PlainSequence;

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            base.Equals(obj);

        /// <summary>
        /// Checks to see if this instance of the color is equal to another instance of the color
        /// </summary>
        /// <param name="other">Another instance of the color to compare with this color</param>
        /// <returns>True if both the colors match; otherwise, false.</returns>
        public bool Equals(Color other)
            => Equals(this, other);

        /// <summary>
        /// Checks to see if the first instance of the color is equal to another instance of the color
        /// </summary>
        /// <param name="other">Another instance of the color to compare with another</param>
        /// <param name="other2">Another instance of the color to compare with another</param>
        /// <returns>True if both the colors match; otherwise, false.</returns>
        public bool Equals(Color other, Color other2)
        {
            // We can't perform this operation on null.
            if (other is null)
                return false;

            // Check all the properties
            return
                other.PlainSequence == other2.PlainSequence &&
                other.PlainSequenceEnclosed == other2.PlainSequenceEnclosed &&
                other.VTSequenceForeground == other2.VTSequenceForeground &&
                other.VTSequenceBackground == other2.VTSequenceBackground &&
                other.PlainSequenceTrueColor == other2.PlainSequenceTrueColor &&
                other.PlainSequenceEnclosedTrueColor == other2.PlainSequenceEnclosedTrueColor &&
                other.VTSequenceForegroundTrueColor == other2.VTSequenceForegroundTrueColor &&
                other.VTSequenceBackgroundTrueColor == other2.VTSequenceBackgroundTrueColor &&
                other.R == other2.R &&
                other.G == other2.G &&
                other.B == other2.B &&
                other.Hex == other2.Hex &&
                other.Type == other2.Type &&
                other.Brightness == other2.Brightness &&
                other.ColorEnum255 == other2.ColorEnum255 &&
                other.ColorEnum16 == other2.ColorEnum16
            ;
        }

        /// <inheritdoc/>
        public static bool operator ==(Color a, Color b)
            => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(Color a, Color b)
            => !a.Equals(b);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -1193100686;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequence);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequenceEnclosed);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceForeground);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceBackground);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequenceTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequenceEnclosedTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceForegroundTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceBackgroundTrueColor);
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hex);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Brightness.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum255.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum16.GetHashCode();
            return hashCode;
        }

        private bool DetectDark(int r, int g, int b) =>
            Convert.ToDouble(r) + 0.2126d + Convert.ToDouble(g) + 0.7152d + Convert.ToDouble(b) + 0.0722d < 255d / 2d;
    }
}
