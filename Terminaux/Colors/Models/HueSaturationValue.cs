
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
using Terminaux.Base;

namespace Terminaux.Colors.Models
{
    /// <summary>
    /// The Hue, Saturation, and Value (HSV) model
    /// </summary>
    public class HueSaturationValue : IEquatable<HueSaturationValue>
    {
        /// <summary>
        /// The position on the color wheel, known as the Hue [0.0 -> 1.0, 0.5 being 180 degrees]
        /// </summary>
        public double Hue { get; private set; }
        /// <summary>
        /// The saturation of the color, indicating how intense the color is [0.0 -> 1.0]
        /// </summary>
        public double Saturation { get; private set; }
        /// <summary>
        /// The value of the color [0.0 -> 1.0]
        /// </summary>
        public double Value { get; private set; }
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0.0 -> 1.0, 0.5 being 180 degrees]
        /// </summary>
        public double ReverseHue { get; private set; }
        /// <summary>
        /// The position on the color wheel, known as the Hue [0 -> 360]
        /// </summary>
        public int HueWhole { get; private set; }
        /// <summary>
        /// The saturation of the color, indicating how intense the color is [0 -> 100]
        /// </summary>
        public int SaturationWhole { get; private set; }
        /// <summary>
        /// The value of the color [0 -> 100]
        /// </summary>
        public int ValueWhole { get; private set; }
        /// <summary>
        /// The reverse hue position on the color wheel, known as the Reverse Hue [0 -> 360]
        /// </summary>
        public double ReverseHueWhole { get; private set; }

        /// <summary>
        /// Converts this instance of HSV color to RGB model
        /// </summary>
        /// <returns>An instance of <see cref="RedGreenBlue"/></returns>
        public RedGreenBlue ConvertToRgb() =>
            new(this);

        /// <summary>
        /// Converts this instance of HSV color to CMYK model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellowKey"/></returns>
        public CyanMagentaYellowKey ConvertToCmyk() =>
            new(this);

        /// <summary>
        /// Converts this instance of HSV color to CMY model
        /// </summary>
        /// <returns>An instance of <see cref="CyanMagentaYellow"/></returns>
        public CyanMagentaYellow ConvertToCmy() =>
            new(this);

        /// <summary>
        /// Converts this instance of HSV color to HSL model
        /// </summary>
        /// <returns>An instance of <see cref="HueSaturationLightness"/></returns>
        public HueSaturationLightness ConvertToHsl() =>
            new(this);

        /// <summary>
        /// hsv:&lt;H&gt;;&lt;S&gt;;&lt;V&gt;
        /// </summary>
        public override string ToString() =>
            $"hsv:{HueWhole};{SaturationWhole};{ValueWhole}";

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            Equals(obj as HueSaturationValue);

        /// <inheritdoc/>
        public bool Equals(HueSaturationValue other) =>
            other is not null &&
            Hue == other.Hue &&
            Saturation == other.Saturation &&
            Value == other.Value &&
            HueWhole == other.HueWhole &&
            SaturationWhole == other.SaturationWhole &&
            ValueWhole == other.ValueWhole;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -871917995;
            hashCode = hashCode * -1521134295 + Hue.GetHashCode();
            hashCode = hashCode * -1521134295 + Saturation.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + HueWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + SaturationWhole.GetHashCode();
            hashCode = hashCode * -1521134295 + ValueWhole.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(HueSaturationValue left, HueSaturationValue right) =>
            EqualityComparer<HueSaturationValue>.Default.Equals(left, right);

        /// <inheritdoc/>
        public static bool operator !=(HueSaturationValue left, HueSaturationValue right) =>
            !(left == right);

        /// <summary>
        /// Converts the CMYK color model to HSV
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationValue(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException("Can't convert a null CMYK instance to HSV!");

            // Get the RGB values
            var rgb = cmyk.ConvertToRgb();

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }

        /// <summary>
        /// Converts the RGB color model to HSV
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationValue(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSV!");

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }

        /// <summary>
        /// Converts the CMY color model to HSV
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationValue(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException("Can't convert a null CMY instance to HSV!");

            // Get the RGB values
            var rgb = cmy.ConvertToRgb();

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }

        /// <summary>
        /// Converts the HSL color model to HSV
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public HueSaturationValue(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException("Can't convert a null HSL instance to HSV!");

            // Get the RGB values
            var rgb = hsl.ConvertToRgb();

            // Do the conversion
            var (hue, saturation, value) = GetHsvFromRgb(rgb);
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }

        internal HueSaturationValue(double hue, double saturation, double value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
            ReverseHue = hue + 0.5;
            if (ReverseHue > 1)
                ReverseHue--;

            SaturationWhole = (int)(Saturation * 100);
            ValueWhole = (int)(Value * 100);
            HueWhole = (int)(Hue * 360);
            ReverseHueWhole = (int)(ReverseHue * 360);
        }

        private (double hue, double saturation, double value) GetHsvFromRgb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException("Can't convert a null RGB instance to HSV!");

            // Get the level of each color
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Get the minimum and maximum color level. .NET's Math.Max doesn't support three variables, so this workaround is added
            double minRgLevel = Math.Min(levelR, levelG);
            double minLevel = Math.Min(minRgLevel, levelB);
            double maxRgLevel = Math.Max(levelR, levelG);
            double maxLevel = Math.Max(maxRgLevel, levelB);

            // Get the delta color level
            double deltaLevel = maxLevel - minLevel;

            // Get the value
            double value = maxLevel;

            // Get the saturation
            double saturation =
                value == 0 ?
                0.0d :
                deltaLevel / maxLevel;
            
            // Get the hue
            double hue = 0.0d;
            if (saturation != 0)
            {
                if (value == levelR)
                    hue = 0.0 + (levelG - levelB) / deltaLevel;
                else if (value == levelG)
                    hue = 2.0 + (levelB - levelR) / deltaLevel;
                else
                    hue = 4.0 + (levelR - levelG) / deltaLevel;
                hue *= 60;
                if (hue < 0)
                    hue += 360;
                hue /= 360;
            }

            // Return the resulting values
            return (hue, saturation, value);
        }
    }
}
