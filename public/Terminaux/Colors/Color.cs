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
using System.Diagnostics;
using DrawingColor = System.Drawing.Color;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;
using Terminaux.Colors.Models.Parsing;
using Terminaux.Colors.Interop;
using Terminaux.Colors.Transformation.Contrast;
using Newtonsoft.Json;
using Terminaux.Sequences.Builder;
using Terminaux.Base.TermInfo;
using Terminaux.Colors.Transformation;

namespace Terminaux.Colors
{
    /// <summary>
    /// Color information class
    /// </summary>
    [DebuggerDisplay("Color = {PlainSequence}, TrueColor = {PlainSequenceTrueColor}")]
    [JsonConverter(typeof(ColorSerializer))]
    public class Color : IEquatable<Color>
    {
        private ConsoleColorData? colorId;
        private string? vtSeqForeground;
        private string? vtSeqBackground;
        private readonly string hex = "";
        private readonly string seqTrueColor = "";
        private readonly string vtSeqForegroundTrue = "";
        private readonly string vtSeqBackgroundTrue = "";
        private readonly ColorSettings settings;

        /// <summary>
        /// An instance of RGB
        /// </summary>
        public RedGreenBlue RGB { get; private set; }

        /// <summary>
        /// The color ID for 256- and 16-color modes.
        /// </summary>
        public ConsoleColorData ColorId
        {
            get
            {
                colorId ??= ConsoleColorData.GetNearestColor(RGB);
                return colorId;
            }
        }

        /// <summary>
        /// Empty color singleton
        /// </summary>
        public static Color Empty =>
            ColorTools._empty;

        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, depending on the settings.
        /// </summary>
        public string PlainSequence =>
            IsOriginal ? PlainSequenceOriginal : PlainSequenceTrueColor;

        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt; in its original form.
        /// </summary>
        public string PlainSequenceOriginal =>
            Type == ColorType.TrueColor ? PlainSequenceTrueColor : $"{ColorId.ColorId}";

        /// <summary>
        /// Parsable VT sequence (Foreground)
        /// </summary>
        public string VTSequenceForeground =>
            IsOriginal ? VTSequenceForegroundOriginal : VTSequenceForegroundTrueColor;

        /// <summary>
        /// Parsable VT sequence (Foreground, original)
        /// </summary>
        public string VTSequenceForegroundOriginal
        {
            get
            {
                if (Type == ColorType.TrueColor)
                    return VTSequenceForegroundTrueColor;
                vtSeqForeground ??=
                    TermInfoDesc.Current.SetAForeground?.ProcessSequence(PlainSequence) ??
                    $"{VtSequenceBasicChars.EscapeChar}[38;5;{PlainSequence}m";
                return vtSeqForeground;
            }
        }

        /// <summary>
        /// Parsable VT sequence (Background)
        /// </summary>
        public string VTSequenceBackground =>
            IsOriginal ? VTSequenceBackgroundOriginal : VTSequenceBackgroundTrueColor;

        /// <summary>
        /// Parsable VT sequence (Background, original)
        /// </summary>
        public string VTSequenceBackgroundOriginal
        {
            get
            {
                if (Type == ColorType.TrueColor)
                    return VTSequenceBackgroundTrueColor;
                vtSeqBackground ??=
                    TermInfoDesc.Current.SetABackground?.ProcessSequence(PlainSequence) ??
                    $"{VtSequenceBasicChars.EscapeChar}[48;5;{PlainSequence}m";
                return vtSeqBackground;
            }
        }

        /// <summary>
        /// &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
        /// </summary>
        public string PlainSequenceTrueColor =>
            seqTrueColor;

        /// <summary>
        /// Parsable VT sequence (Foreground, true color)
        /// </summary>
        public string VTSequenceForegroundTrueColor =>
            vtSeqForegroundTrue;

        /// <summary>
        /// Parsable VT sequence (Background, true color)
        /// </summary>
        public string VTSequenceBackgroundTrueColor =>
            vtSeqBackgroundTrue;

        /// <summary>
        /// Hexadecimal representation of the color
        /// </summary>
        public string Hex =>
            hex;

        /// <summary>
        /// Color name representation
        /// </summary>
        public string Name =>
            ColorId.Name;

        /// <summary>
        /// Color type
        /// </summary>
        public ColorType Type =>
            !ColorId.HexString.Equals(Hex, StringComparison.OrdinalIgnoreCase) || !IsOriginal ? ColorType.TrueColor :
            ColorId.ColorId >= 16 ? ColorType.EightBitColor : ColorType.FourBitColor;

        /// <summary>
        /// Determines the color brightness whether it indicates dark or light mode
        /// </summary>
        public ColorBrightness Brightness
        {
            get
            {
                int monochromeFactor = (int)TransformationTools.GetLuminance(RGB.R, RGB.G, RGB.B, true);
                return monochromeFactor < 255d / 2d ? ColorBrightness.Dark : ColorBrightness.Light;
            }
        }

        /// <summary>
        /// The color value converted to <see cref="ConsoleColors"/>.
        /// </summary>
        public ConsoleColors ColorEnum255 =>
            (ConsoleColors)ColorId.ColorId;

        /// <summary>
        /// The color value converted to <see cref="ConsoleColor"/>. Not applicable [-1] to non-4-bit colors.
        /// </summary>
        public ConsoleColor ColorEnum16 =>
            ColorId.ColorId < 16 ? ConversionTools.CorrectStandardColor((ConsoleColor)ColorId.ColorId) : (ConsoleColor)(-1);

        /// <summary>
        /// Color opacity from 0 (transparent) to 255 (opaque)
        /// </summary>
        public int Opacity =>
            settings.Opacity;

        internal bool IsOriginal =>
            settings.UseTerminalPalette && settings.Opacity == 255 && settings.Transformations.Length == 0;

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(int R, int G, int B)
            : this($"{R};{G};{B}")
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="R">The red level</param>
        /// <param name="G">The green level</param>
        /// <param name="B">The blue level</param>
        /// <param name="settings">Color settings to use while building the color</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(int R, int G, int B, ColorSettings settings)
            : this($"{R};{G};{B}", settings)
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <exception cref="TerminauxException"></exception>
        public Color(ConsoleColors ColorDef)
            : this(ColorTools.GetColorIdStringFrom(ColorDef))
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <param name="settings">Color settings to use while building the color</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(ConsoleColors ColorDef, ColorSettings settings)
            : this(ColorTools.GetColorIdStringFrom(ColorDef), settings)
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <exception cref="TerminauxException"></exception>
        public Color(ConsoleColor ColorDef)
            : this((int)ConversionTools.CorrectStandardColor(ColorDef))
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <param name="settings">Color settings to use while building the color</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(ConsoleColor ColorDef, ColorSettings settings)
            : this((int)ConversionTools.CorrectStandardColor(ColorDef), settings)
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number or a decimal number that specifies the RGB values up to 16777215</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(int ColorNum)
            : this(ColorNum > 255 ? ColorTools.GetRgbSpecifierFromColorCode(ColorNum) : ColorTools.GetColorIdStringFrom(ColorNum))
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number or a decimal number that specifies the RGB values up to 16777215</param>
        /// <param name="settings">Color settings to use while building the color</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(int ColorNum, ColorSettings settings)
            : this(ColorNum > 255 ? ColorTools.GetRgbSpecifierFromColorCode(ColorNum) : ColorTools.GetColorIdStringFrom(ColorNum), settings)
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(string ColorSpecifier)
            : this(ColorSpecifier, ColorTools.GlobalSettings)
        { }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <param name="settings">Color settings to use while building the color</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(string ColorSpecifier, ColorSettings settings)
        {
            // Install the settings. This is necessary for ParseSpecifier.
            this.settings = settings;

            // Now, parse the output
            RGB = ParsingTools.ParseSpecifier(ColorSpecifier, settings);
            hex = $"#{RGB.R:X2}{RGB.G:X2}{RGB.B:X2}";
            seqTrueColor = $"{RGB.R};{RGB.G};{RGB.B}";
            vtSeqForegroundTrue = $"{VtSequenceBasicChars.EscapeChar}[38;2;{PlainSequenceTrueColor}m";
            vtSeqBackgroundTrue = $"{VtSequenceBasicChars.EscapeChar}[48;2;{PlainSequenceTrueColor}m";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="color">The color from Drawing</param>
        /// <exception cref="TerminauxException"></exception>
        public Color(DrawingColor color)
        {
            var result = SystemColorConverter.FromDrawingColor(color);
            settings = result.settings;
            colorId = result.ColorId;
            RGB = result.RGB;
            hex = $"#{RGB.R:X2}{RGB.G:X2}{RGB.B:X2}";
            seqTrueColor = $"{RGB.R};{RGB.G};{RGB.B}";
            vtSeqForegroundTrue = $"{VtSequenceBasicChars.EscapeChar}[38;2;{PlainSequenceTrueColor}m";
            vtSeqBackgroundTrue = $"{VtSequenceBasicChars.EscapeChar}[48;2;{PlainSequenceTrueColor}m";
        }

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColors"/></param>
        /// <exception cref="TerminauxException"></exception>
        public static implicit operator Color(ConsoleColors ColorDef) =>
            new(ColorDef);

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorDef">The color taken from <see cref="ConsoleColor"/></param>
        /// <exception cref="TerminauxException"></exception>
        public static implicit operator Color(ConsoleColor ColorDef) =>
            new(ColorDef);

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorNum">The color number</param>
        /// <exception cref="TerminauxException"></exception>
        public static implicit operator Color(int ColorNum) =>
            new(ColorNum);

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, or a hexadecimal representation of a number (#AABBCC for example)</param>
        /// <exception cref="TerminauxException"></exception>
        public static implicit operator Color(string ColorSpecifier) =>
            new(ColorSpecifier);

        /// <summary>
        /// Makes a new instance of color class from specifier.
        /// </summary>
        /// <param name="color">The color from Drawing</param>
        /// <exception cref="TerminauxException"></exception>
        public static implicit operator Color(DrawingColor color) =>
            new(color);

        /// <summary>
        /// Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;, depending on the usage of the terminal palette.
        /// </summary>
        public override string ToString() =>
            PlainSequence;

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Color color)
                return Equals(color);
            return base.Equals(obj);
        }

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
                other.VTSequenceForeground == other2.VTSequenceForeground &&
                other.VTSequenceBackground == other2.VTSequenceBackground &&
                other.PlainSequenceTrueColor == other2.PlainSequenceTrueColor &&
                other.VTSequenceForegroundTrueColor == other2.VTSequenceForegroundTrueColor &&
                other.VTSequenceBackgroundTrueColor == other2.VTSequenceBackgroundTrueColor &&
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
            int hashCode = -636468195;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequence);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceForeground);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceBackground);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PlainSequenceTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceForegroundTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VTSequenceBackgroundTrueColor);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hex);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + Brightness.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum255.GetHashCode();
            hashCode = hashCode * -1521134295 + ColorEnum16.GetHashCode();
            return hashCode;
        }
    }
}
