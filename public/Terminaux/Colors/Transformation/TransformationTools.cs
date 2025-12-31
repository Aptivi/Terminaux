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
using Terminaux.Colors.Models;
using Terminaux.Colors.Models.Conversion;

namespace Terminaux.Colors.Transformation
{
    /// <summary>
    /// Color transformation tools
    /// </summary>
    public static class TransformationTools
    {
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
        /// Blends the two colors together
        /// </summary>
        /// <param name="source">Source color to be blended</param>
        /// <param name="target">Target color to blend</param>
        /// <param name="factor">Blending factor [0.0 to 1.0]</param>
        /// <returns>A color instance that represents a source color blended with the target color.</returns>
        public static Color BlendColor(Color source, Color target, double factor = 0.5) =>
            BlendColor(source.RGB, target.RGB, factor);

        /// <summary>
        /// Blends the two colors together
        /// </summary>
        /// <param name="source">Source color to be blended</param>
        /// <param name="target">Target color to blend</param>
        /// <param name="factor">Blending factor [0.0 to 1.0]</param>
        /// <returns>A color instance that represents a source color blended with the target color.</returns>
        public static Color BlendColor(RedGreenBlue source, RedGreenBlue target, double factor = 0.5) =>
            BlendColor((source.R, source.G, source.B), (target.R, target.G, target.B), factor);

        /// <summary>
        /// Blends the two colors together
        /// </summary>
        /// <param name="source">Source RGB levels to be blended</param>
        /// <param name="target">Target RGB levels to blend</param>
        /// <param name="factor">Blending factor [0.0 to 1.0]</param>
        /// <returns>A color instance that represents a source color blended with the target color.</returns>
        public static Color BlendColor((int r, int g, int b) source, (int r, int g, int b) target, double factor = 0.5) =>
            new(
                (byte)(source.r + ((target.r - source.r) * factor)),
                (byte)(source.g + ((target.g - source.g) * factor)),
                (byte)(source.b + ((target.b - source.b) * factor))
            );

        /// <summary>
        /// Gets the luminance of the color
        /// </summary>
        /// <param name="color">Color to obtain luminance from</param>
        /// <param name="sRgb">Convert linear RGB result to sRGB</param>
        /// <returns>Luminance level in linear RGB or sRGB</returns>
        public static double GetLuminance(Color color, bool sRgb = false) =>
            GetLuminance(color.RGB, sRgb);

        /// <summary>
        /// Gets the luminance of the color
        /// </summary>
        /// <param name="rgb">RGB levels to obtain luminance from</param>
        /// <param name="sRgb">Convert linear RGB result to sRGB</param>
        /// <returns>Luminance level in linear RGB or sRGB</returns>
        public static double GetLuminance(RedGreenBlue rgb, bool sRgb = false) =>
            GetLuminance(rgb.R, rgb.G, rgb.B, sRgb);

        /// <summary>
        /// Gets the luminance of the color
        /// </summary>
        /// <param name="r">Red RGB color level (sRGB)</param>
        /// <param name="g">Green RGB color level (sRGB)</param>
        /// <param name="b">Blue RGB color level (sRGB)</param>
        /// <param name="sRgb">Convert linear RGB result to sRGB</param>
        /// <returns>Luminance level in linear RGB or sRGB</returns>
        public static double GetLuminance(int r, int g, int b, bool sRgb = false)
        {
            double luminanceR = SRGBToLinearRGB(r);
            double luminanceG = SRGBToLinearRGB(g);
            double luminanceB = SRGBToLinearRGB(b);
            return GetLuminance(luminanceR, luminanceG, luminanceB, sRgb);
        }

        /// <summary>
        /// Gets the luminance of the color
        /// </summary>
        /// <param name="r">Red RGB color level (linear)</param>
        /// <param name="g">Green RGB color level (linear)</param>
        /// <param name="b">Blue RGB color level (linear)</param>
        /// <param name="sRgb">Convert linear RGB result to sRGB</param>
        /// <returns>Luminance level in linear RGB or sRGB</returns>
        public static double GetLuminance(double r, double g, double b, bool sRgb = false)
        {
            double luminanceLinear = 0.2126 * r + 0.7152 * g + 0.0722 * b;
            if (sRgb)
                return LinearRGBTosRGB(luminanceLinear);
            return luminanceLinear;
        }

        /// <summary>
        /// Gets the contrast between two colors
        /// </summary>
        /// <param name="firstColor">First color (usually foreground)</param>
        /// <param name="secondColor">Second color (usually background)</param>
        /// <returns>Contrast ratio</returns>
        public static double GetContrast(Color firstColor, Color secondColor)
        {
            double luminanceFirst = GetLuminance(firstColor);
            double luminanceSecond = GetLuminance(secondColor);
            double min = Math.Min(luminanceFirst, luminanceSecond);
            double max = Math.Max(luminanceFirst, luminanceSecond);
            return (max + 0.05) / (min + 0.05);
        }

        /// <summary>
        /// Gets the colorized dark background from the source color
        /// </summary>
        /// <param name="source">Source color to darken</param>
        /// <returns>A copy of the source color with darkness applied that suits the background</returns>
        public static Color GetDarkBackground(Color source)
        {
            int targetR = (int)(source.RGB.R / 4d);
            int targetG = (int)(source.RGB.G / 4d);
            int targetB = (int)(source.RGB.B / 4d);
            return new(targetR, targetG, targetB);
        }

        /// <summary>
        /// Gets the colorized light background from the source color
        /// </summary>
        /// <param name="source">Source color to lighten</param>
        /// <returns>A copy of the source color with lightness applied that suits the background</returns>
        public static Color GetLightBackground(Color source)
        {
            int targetR = (int)(source.RGB.R * 2d);
            targetR = targetR > 255 ? 255 : targetR;
            int targetG = (int)(source.RGB.G * 2d);
            targetG = targetG > 255 ? 255 : targetG;
            int targetB = (int)(source.RGB.B * 2d);
            targetB = targetB > 255 ? 255 : targetB;
            return new(targetR, targetG, targetB);
        }

        /// <summary>
        /// Desaturates the color using the desaturation factor
        /// </summary>
        /// <param name="source">Source color to desaturate</param>
        /// <param name="target">Target desaturation level from 0 to 100</param>
        /// <returns>Desaturated color</returns>
        public static Color Desaturate(Color source, int target)
        {
            // Check the target value
            target = target <= 0 ? 0 : target;
            target = target > 100 ? 100 : target;

            // Convert to HSL and reduce the saturation
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(source.RGB);
            double saturation = hsl.Saturation;
            saturation -= target / 100d;
            saturation = ClampZeroOne(saturation);
            return new Color($"hsl:{hsl.HueWhole};{saturation * 100};{hsl.LightnessWhole}");
        }

        /// <summary>
        /// Saturates the color using the saturation factor
        /// </summary>
        /// <param name="source">Source color to saturate</param>
        /// <param name="target">Target saturation level from 0 to 100</param>
        /// <returns>Saturated color</returns>
        public static Color Saturate(Color source, int target)
        {
            // Check the target value
            target = target <= 0 ? 0 : target;
            target = target > 100 ? 100 : target;

            // Convert to HSL and reduce the saturation
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(source.RGB);
            double saturation = hsl.Saturation;
            saturation += target / 100d;
            saturation = ClampZeroOne(saturation);
            return new Color($"hsl:{hsl.HueWhole};{saturation * 100};{hsl.LightnessWhole}");
        }

        /// <summary>
        /// Darkens the color using the darkening factor
        /// </summary>
        /// <param name="source">Source color to darken</param>
        /// <param name="target">Target darkening level from 0 to 100</param>
        /// <returns>Darkened color</returns>
        public static Color Darken(Color source, int target)
        {
            // Check the target value
            target = target <= 0 ? 0 : target;
            target = target > 100 ? 100 : target;

            // Convert to HSL and reduce the lightness
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(source.RGB);
            double lightness = hsl.Lightness;
            lightness -= target / 100d;
            lightness = ClampZeroOne(lightness);
            return new Color($"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{lightness * 100}");
        }

        /// <summary>
        /// Lightens the color using the lightening factor
        /// </summary>
        /// <param name="source">Source color to lighten</param>
        /// <param name="target">Target lightening level from 0 to 100</param>
        /// <returns>Lightened color</returns>
        public static Color Lighten(Color source, int target)
        {
            // Check the target value
            target = target <= 0 ? 0 : target;
            target = target > 100 ? 100 : target;

            // Convert to HSL and reduce the lightness
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(source.RGB);
            double lightness = hsl.Lightness;
            lightness += target / 100d;
            lightness = ClampZeroOne(lightness);
            return new Color($"hsl:{hsl.HueWhole};{hsl.SaturationWhole};{lightness * 100}");
        }

        /// <summary>
        /// Spins the color
        /// </summary>
        /// <param name="source">Source color to spin</param>
        /// <param name="target">Spin angle</param>
        /// <returns>Spinned color</returns>
        public static Color Spin(Color source, int target)
        {
            // Convert to HSL to spin
            var hsl = ConversionTools.ConvertFromRgb<HueSaturationLightness>(source.RGB);
            double hue = (hsl.HueWhole + target) % 360;
            hue = hue < 0 ? hue + 360 : hue;
            return new Color($"hsl:{hue};{hsl.SaturationWhole};{hsl.LightnessWhole}");
        }

        /// <summary>
        /// Gets the color complement
        /// </summary>
        /// <param name="source">Source color to get its complement</param>
        /// <returns>Complement color</returns>
        public static Color Complement(Color source) =>
            Spin(source, 180);

        internal static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput, ColorSettings settings)
        {
            (int r, int g, int b) = (rInput, gInput, bInput);
            if (settings.Transformations.Length > 0)
            {
                // We'll transform.
                foreach (var transform in settings.Transformations)
                    (r, g, b) = transform.Transform(r, g, b);
            }
            return (r, g, b);
        }

        private static double ClampZeroOne(double target) =>
            Math.Min(1, Math.Max(0, target));
    }
}
