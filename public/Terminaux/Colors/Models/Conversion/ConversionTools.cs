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
using System.Linq;
using Magico.Reflection;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Colors.Models.Parsing;

namespace Terminaux.Colors.Models.Conversion
{
    /// <summary>
    /// Color conversion tools
    /// </summary>
    public static class ConversionTools
    {
        #region Standard color conversion functions
        /// <summary>
        /// Translates the color from .NET's <see cref="ConsoleColor"/> to X11's representation, <see cref="ConsoleColors"/>
        /// </summary>
        /// <param name="color">.NET's <see cref="ConsoleColor"/> to translate this color to</param>
        /// <returns>X11's representation of this color, <see cref="ConsoleColors"/></returns>
        public static ConsoleColors TranslateToX11ColorMap(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Black => ConsoleColors.Black,
                ConsoleColor.DarkBlue => ConsoleColors.Maroon,
                ConsoleColor.DarkGreen => ConsoleColors.Green,
                ConsoleColor.DarkCyan => ConsoleColors.Olive,
                ConsoleColor.DarkRed => ConsoleColors.Navy,
                ConsoleColor.DarkMagenta => ConsoleColors.Purple,
                ConsoleColor.DarkYellow => ConsoleColors.Teal,
                ConsoleColor.Gray => ConsoleColors.Silver,
                ConsoleColor.DarkGray => ConsoleColors.Grey,
                ConsoleColor.Blue => ConsoleColors.Red,
                ConsoleColor.Green => ConsoleColors.Lime,
                ConsoleColor.Cyan => ConsoleColors.Yellow,
                ConsoleColor.Red => ConsoleColors.Blue,
                ConsoleColor.Magenta => ConsoleColors.Fuchsia,
                ConsoleColor.Yellow => ConsoleColors.Aqua,
                ConsoleColor.White => ConsoleColors.White,
                _ => ConsoleColors.Black,
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
                ConsoleColors.Black => ConsoleColor.Black,
                ConsoleColors.Maroon => ConsoleColor.DarkBlue,
                ConsoleColors.Green => ConsoleColor.DarkGreen,
                ConsoleColors.Olive => ConsoleColor.DarkCyan,
                ConsoleColors.Navy => ConsoleColor.DarkRed,
                ConsoleColors.Purple => ConsoleColor.DarkMagenta,
                ConsoleColors.Teal => ConsoleColor.DarkYellow,
                ConsoleColors.Silver => ConsoleColor.Gray,
                ConsoleColors.Grey => ConsoleColor.DarkGray,
                ConsoleColors.Red => ConsoleColor.Blue,
                ConsoleColors.Lime => ConsoleColor.Green,
                ConsoleColors.Yellow => ConsoleColor.Cyan,
                ConsoleColors.Blue => ConsoleColor.Red,
                ConsoleColors.Fuchsia => ConsoleColor.Magenta,
                ConsoleColors.Aqua => ConsoleColor.Yellow,
                ConsoleColors.White => ConsoleColor.White,
                _ => ConsoleColor.Black,
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
                ConsoleColor.Black => ConsoleColor.Black,
                ConsoleColor.DarkBlue => ConsoleColor.DarkRed,
                ConsoleColor.DarkGreen => ConsoleColor.DarkGreen,
                ConsoleColor.DarkCyan => ConsoleColor.DarkYellow,
                ConsoleColor.DarkRed => ConsoleColor.DarkBlue,
                ConsoleColor.DarkMagenta => ConsoleColor.DarkMagenta,
                ConsoleColor.DarkYellow => ConsoleColor.DarkCyan,
                ConsoleColor.Gray => ConsoleColor.Gray,
                ConsoleColor.DarkGray => ConsoleColor.DarkGray,
                ConsoleColor.Blue => ConsoleColor.Red,
                ConsoleColor.Green => ConsoleColor.Green,
                ConsoleColor.Cyan => ConsoleColor.Yellow,
                ConsoleColor.Red => ConsoleColor.Blue,
                ConsoleColor.Magenta => ConsoleColor.Magenta,
                ConsoleColor.Yellow => ConsoleColor.Cyan,
                ConsoleColor.White => ConsoleColor.White,
                _ => ConsoleColor.Black,
            };
        }
        #endregion

        #region Generic color model conversion functions
        /// <summary>
        /// Gets the converted color model from the source model of type <typeparamref name="TModel"/> to the target color model of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TModel">Source color model in which it will be converted from</typeparam>
        /// <typeparam name="TResult">Target color model in which it will be converted to</typeparam>
        /// <param name="source">The source color model instance</param>
        /// <returns>Converted color model that is of type <typeparamref name="TResult"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static TResult GetConvertedColorModel<TModel, TResult>(TModel source)
            where TModel : BaseColorModel
            where TResult : BaseColorModel =>
            (TResult)GetConvertedColorModel(source, typeof(TResult));

        /// <summary>
        /// Gets the converted color model from the source model of type <typeparamref name="TModel"/> to the target color model of type <see cref="RedGreenBlue"/>
        /// </summary>
        /// <typeparam name="TModel">Source color model in which it will be converted from</typeparam>
        /// <param name="source">The source color model instance</param>
        /// <returns>Converted color model that is of type <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ConvertToRgb<TModel>(TModel source)
            where TModel : BaseColorModel =>
            ConvertToRgb((BaseColorModel)source);

        /// <summary>
        /// Gets the converted color model from the source model of type <see cref="RedGreenBlue"/> to the target color model of type <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TResult">Target color model in which it will be converted to</typeparam>
        /// <param name="source">The source color model instance</param>
        /// <returns>Converted color model that is of type <typeparamref name="TResult"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static TResult ConvertFromRgb<TResult>(RedGreenBlue source)
            where TResult : BaseColorModel =>
            (TResult)ConvertFromRgb(source, typeof(TResult));
        #endregion

        #region Non-generic color model conversion functions
        /// <summary>
        /// Gets the converted color model from the source model to the target color model
        /// </summary>
        /// <param name="source">The source color model instance</param>
        /// <param name="targetType">The target color model type</param>
        /// <returns>Converted color model</returns>
        /// <exception cref="TerminauxException"></exception>
        public static BaseColorModel GetConvertedColorModel(BaseColorModel source, Type targetType)
        {
            // Check for value
            if (source is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOSOURCE"));
            if (targetType is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOTYPE"));
            Type sourceType = source.GetType();

            // Check for type
            if (sourceType == typeof(BaseColorModel))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_SPECIFYSOURCEMODEL"));
            if (!BaseClassTools.GetBaseTypes(sourceType).Contains(typeof(BaseColorModel)))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOTSOURCEMODEL"));
            if (targetType == typeof(BaseColorModel))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_SPECIFYTARGETMODEL"));
            if (!BaseClassTools.GetBaseTypes(targetType).Contains(typeof(BaseColorModel)))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOTTARGETMODEL"));

            // Determine whether the conversion is needed or not
            bool needsConvertTarget = targetType != typeof(RedGreenBlue);
            if (sourceType == targetType)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_SAMEMODEL"));

            // Convert the source color first to RGB if we don't have an RGB instance
            if (source is not RedGreenBlue rgb)
                rgb = ConvertToRgb(source);
            if (!needsConvertTarget)
            {
                return rgb ??
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBFAILED"));
            }
            else
            {
                var converted = ConvertFromRgb(rgb, targetType);
                return converted;
            }
        }

        /// <summary>
        /// Gets the converted color model from the source model to the target color model of type <see cref="RedGreenBlue"/>
        /// </summary>
        /// <param name="source">The source color model instance</param>
        /// <returns>Converted color model that is of type <see cref="RedGreenBlue"/></returns>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ConvertToRgb(BaseColorModel source)
        {
            // Check for value
            if (source is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOSOURCE"));

            // Now, convert from the specified source model to RGB
            if (source.GetType() == typeof(BaseColorModel))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_FROMBASEFAILED"));
            if (source is not RedGreenBlue rgb)
            {
                if (source is CyanMagentaYellowKey cmyk)
                    rgb = ToRgb(cmyk);
                else if (source is CyanMagentaYellow cmy)
                    rgb = ToRgb(cmy);
                else if (source is HueSaturationLightness hsl)
                    rgb = ToRgb(hsl);
                else if (source is HueSaturationValue hsv)
                    rgb = ToRgb(hsv);
                else if (source is RedYellowBlue ryb)
                    rgb = ToRgb(ryb);
                else if (source is LumaInPhaseQuadrature yiq)
                    rgb = ToRgb(yiq);
                else if (source is LumaChromaUv yuv)
                    rgb = ToRgb(yuv);
                else if (source is Xyz xyz)
                    rgb = ToRgb(xyz);
                else if (source is Yxy yxy)
                    rgb = ToRgb(yxy);
                else if (source is HunterLab hunterLab)
                    rgb = ToRgb(hunterLab);
                else if (source is CieLab cieLab)
                    rgb = ToRgb(cieLab);
                else if (source is CieLuv cieLuv)
                    rgb = ToRgb(cieLuv);
                else if (source is CieLch cieLch)
                    rgb = ToRgb(cieLch);
                else if (source is HueWhiteBlack hwb)
                    rgb = ToRgb(hwb);
                else
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBFAILED"));
                return rgb;
            }
            else
                return rgb;
        }

        /// <summary>
        /// Gets the converted color model from the source model of type <see cref="RedGreenBlue"/> to the target color model
        /// </summary>
        /// <param name="source">The source color model instance</param>
        /// <param name="targetType">Target color model in which it will be converted to</param>
        /// <returns>Converted color model</returns>
        /// <exception cref="TerminauxException"></exception>
        public static BaseColorModel ConvertFromRgb(RedGreenBlue source, Type targetType)
        {
            // Check for value
            if (source is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOSOURCE"));
            if (targetType is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_NOTYPE"));

            // Now, convert from RGB to the specified source model
            if (source.GetType() == typeof(BaseColorModel))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_FROMBASEFAILED"));
            if (targetType == typeof(BaseColorModel))
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_FROMBASEFAILED"));
            if (targetType == typeof(RedGreenBlue))
                return source ??
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_FROMBASEFAILED"));
            else
            {
                if (targetType == typeof(CyanMagentaYellowKey))
                    return ToCmyk(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCMYKFAILED"));
                else if (targetType == typeof(CyanMagentaYellow))
                    return ToCmy(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCMYFAILED"));
                else if (targetType == typeof(HueSaturationLightness))
                    return ToHsl(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHSLFAILED"));
                else if (targetType == typeof(HueSaturationValue))
                    return ToHsv(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHSVFAILED"));
                else if (targetType == typeof(RedYellowBlue))
                    return ToRyb(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORYBFAILED"));
                else if (targetType == typeof(LumaInPhaseQuadrature))
                    return ToYiq(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYIQFAILED"));
                else if (targetType == typeof(LumaChromaUv))
                    return ToYuv(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYUVFAILED"));
                else if (targetType == typeof(Xyz))
                    return ToXyz(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOXYZFAILED"));
                else if (targetType == typeof(Yxy))
                    return ToYxy(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYXYFAILED"));
                else if (targetType == typeof(HunterLab))
                    return ToHunterLab(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHUNTERLABFAILED"));
                else if (targetType == typeof(CieLab))
                    return ToCieLab(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELABFAILED"));
                else if (targetType == typeof(CieLuv))
                    return ToCieLuv(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELUVFAILED"));
                else if (targetType == typeof(CieLch))
                    return ToCieLch(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELCHFAILED"));
                else if (targetType == typeof(HueWhiteBlack))
                    return ToHwb(source) ??
                        throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHWBFAILED"));
                else
                    throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_FROMRGBFAILED"));
            }
        }
        #endregion

        #region Model-specific conversion functions
        #region Translate from RGB to...
        /// <summary>
        /// Converts the RGB color model to CMY
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellow ToCmy(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCMYNULLRGB"));

            // Get the level of each color
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Now, get the Cyan, Magenta, and Yellow values
            double c = 1 - levelR;
            double m = 1 - levelG;
            double y = 1 - levelB;
            if (double.IsNaN(c))
                c = 0;
            if (double.IsNaN(m))
                m = 0;
            if (double.IsNaN(y))
                y = 0;

            // Install the values
            return new(c, m, y);
        }

        /// <summary>
        /// Converts the RGB color model to CMYK
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CyanMagentaYellowKey ToCmyk(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCMYKNULLRGB"));

            // Get the level of each color
            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Get the black key (K). .NET's Math.Max doesn't support three variables, so this workaround is added
            double maxRgLevel = Math.Max(levelR, levelG);
            double maxLevel = Math.Max(maxRgLevel, levelB);
            double key = 1 - maxLevel;

            // Now, get the Cyan, Magenta, and Yellow values
            double c = (1 - levelR - key) / (1 - key);
            double m = (1 - levelG - key) / (1 - key);
            double y = (1 - levelB - key) / (1 - key);
            if (double.IsNaN(c))
                c = 0;
            if (double.IsNaN(m))
                m = 0;
            if (double.IsNaN(y))
                y = 0;

            // Install the values
            var cmy = new CyanMagentaYellow(c, m, y);
            return new(key, cmy);
        }

        /// <summary>
        /// Converts the RGB color model to HSL
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationLightness ToHsl(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHSLNULLRGB"));

            // Do the conversion
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

            // Get the lightness
            double lightness = (maxLevel + minLevel) / 2;

            // Get the hue and the saturation
            double hue = 0.0d;
            double saturation = 0.0d;
            if (deltaLevel != 0)
            {
                // First, the saturation based on the lightness value
                saturation =
                    lightness < 0.5d ?
                    deltaLevel / (maxLevel + minLevel) :
                    deltaLevel / (2 - maxLevel - minLevel);

                // Now, get the delta of R, G, and B values so that we can calculate the hue
                double deltaR = (((maxLevel - levelR) / 6) + (deltaLevel / 2)) / deltaLevel;
                double deltaG = (((maxLevel - levelG) / 6) + (deltaLevel / 2)) / deltaLevel;
                double deltaB = (((maxLevel - levelB) / 6) + (deltaLevel / 2)) / deltaLevel;

                // Now, calculate the hue
                if (levelR == maxLevel)
                    hue = deltaB - deltaG;
                else if (levelG == maxLevel)
                    hue = (1 / 3.0d) + deltaR - deltaB;
                else if (levelB == maxLevel)
                    hue = (2 / 3.0d) + deltaG - deltaR;

                // Verify the hue value so that we don't overflow
                if (hue < 0)
                    hue++;
                if (hue > 1)
                    hue--;
            }

            // Return the result
            return new(hue, saturation, lightness);
        }

        /// <summary>
        /// Converts the RGB color model to HSV
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueSaturationValue ToHsv(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHSVNULLRGB"));

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
            return new(hue, saturation, value);
        }

        /// <summary>
        /// Converts the RGB color model to RYB
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedYellowBlue ToRyb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORYBNULLRGB"));

            // Get the whiteness and remove it from all the colors
            double white = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            double redNoWhite = rgb.R - white;
            double greenNoWhite = rgb.G - white;
            double blueNoWhite = rgb.B - white;

            // Get the resulting max value
            double maxRgb = Math.Max(Math.Max(redNoWhite, greenNoWhite), blueNoWhite);

            // Now, remove the yellow from the red and the green values
            double yellowNoWhite = Math.Min(redNoWhite, greenNoWhite);
            redNoWhite -= yellowNoWhite;
            greenNoWhite -= yellowNoWhite;

            // Now, check to see if the blue and the green colors are not zeroes. If true, cut these values to half.
            if (greenNoWhite != 0 && blueNoWhite != 0)
            {
                greenNoWhite /= 2.0;
                blueNoWhite /= 2.0;
            }

            // Now, redistribute the green remnants to the yellow and the blue level
            yellowNoWhite += greenNoWhite;
            blueNoWhite += greenNoWhite;

            // Do some normalization
            double maxRyb = Math.Max(Math.Max(redNoWhite, yellowNoWhite), blueNoWhite);
            if (maxRyb != 0)
            {
                double normalizationFactor = maxRgb / maxRyb;
                redNoWhite *= normalizationFactor;
                yellowNoWhite *= normalizationFactor;
                blueNoWhite *= normalizationFactor;
            }

            // Now, restore the white color to the three components
            redNoWhite += white;
            yellowNoWhite += white;
            blueNoWhite += white;

            // Cast everything to integers
            int red = (int)Math.Round(redNoWhite);
            int yellow = (int)Math.Round(yellowNoWhite);
            int blue = (int)Math.Round(blueNoWhite);

            // Check the values
            if (red < 0)
                red = 0;
            if (red > 255)
                red = 255;
            if (yellow < 0)
                yellow = 0;
            if (yellow > 255)
                yellow = 255;
            if (blue < 0)
                blue = 0;
            if (blue > 255)
                blue = 255;

            // Return the values
            return new(red, yellow, blue);
        }

        /// <summary>
        /// Converts the RGB color model to YIQ
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaInPhaseQuadrature ToYiq(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYIQNULLRGB"));

            // Get the YIQ values
            int y = (int)((0.299d * rgb.R) + (0.587d * rgb.G) + (0.114d * rgb.B));
            int i = (int)((0.596d * rgb.R) + (-0.275d * rgb.G) + (-0.321d * rgb.B) + 128);
            int q = (int)((0.212d * rgb.R) + (-0.523d * rgb.G) + (0.311d * rgb.B) + 128);

            // Return the resulting values
            return new(y, i, q);
        }

        /// <summary>
        /// Converts the RGB color model to YUV
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static LumaChromaUv ToYuv(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYUVNULLRGB"));

            // Get the YUV values
            int y = (int)((0.299d * rgb.R) + (0.587d * rgb.G) + (0.114d * rgb.B));
            int u = (int)((-0.168736d * rgb.R) + (-0.331264d * rgb.G) + (0.500000d * rgb.B) + 128);
            int v = (int)((0.500000d * rgb.R) + (-0.418688d * rgb.G) + (-0.081312d * rgb.B) + 128);

            // Return the resulting values
            return new(y, u, v);
        }

        /// <summary>
        /// Converts the RGB color model to XYZ
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static Xyz ToXyz(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOXYZNULLRGB"));

            // Get the XYZ values
            double r = rgb.RNormalized;
            double g = rgb.GNormalized;
            double b = rgb.BNormalized;
            r = ((r > 0.04045d) ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92d) * 100;
            g = ((g > 0.04045d) ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92d) * 100;
            b = ((b > 0.04045d) ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92d) * 100;
            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            // Return the resulting values
            return new(x, y, z);
        }

        /// <summary>
        /// Converts the RGB color model to YXY
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static Yxy ToYxy(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOYXYNULLRGB"));

            // Get the XYZ values first
            var xyz = ToXyz(rgb);

            // Get the YXY values
            double y1 = xyz.Y;
            double x = xyz.X / (xyz.X + xyz.Y + xyz.Z);
            double y2 = xyz.Y / (xyz.X + xyz.Y + xyz.Z);

            // Return the resulting values
            return new(y1, x, y2);
        }

        /// <summary>
        /// Converts the RGB color model to HunterLab
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HunterLab ToHunterLab(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHUNTERLABNULLRGB"));

            // Get the XYZ values first
            var xyz = ToXyz(rgb);

            // Get the HunterLab values
            double l = 10.0 * Math.Sqrt(xyz.Y);
            double a = 17.5 * (((1.02 * xyz.X) - xyz.Y) / Math.Sqrt(xyz.Y));
            double b = 7 * ((xyz.Y - (0.847 * xyz.Z)) / Math.Sqrt(xyz.Y));

            // Return the resulting values
            return new(l, a, b);
        }

        /// <summary>
        /// Converts the RGB color model to CieLab
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLab ToCieLab(RedGreenBlue rgb)
        {
            if (rgb.parameters.Length != 2)
                return ToCieLab(rgb, 2, IlluminantType.D65);
            else
            {
                // Validate the values
                int observer = (int)rgb.parameters[0];
                IlluminantType illuminant = (IlluminantType)rgb.parameters[0];
                if ((observer != 2 && observer != 10) || illuminant < 0 || illuminant > IlluminantType.F12)
                    return ToCieLab(rgb, 2, IlluminantType.D65);
                return ToCieLab(rgb, observer, illuminant);
            }
        }

        /// <summary>
        /// Converts the RGB color model to CieLab
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLab ToCieLab(RedGreenBlue rgb, int observer, IlluminantType illuminant)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELABNULLRGB"));

            // Get the illuminant references
            (double refX, double refY, double refZ) = IlluminanceTools.GetIlluminantReferences(observer, illuminant);

            // Get the XYZ values first, then get their variants
            var xyz = ToXyz(rgb);
            double varX = xyz.X / refX;
            double varY = xyz.Y / refY;
            double varZ = xyz.Z / refZ;

            // Adjust their variants
            static double Adjust(double var)
            {
                if (var > 0.008856)
                    var = Math.Pow(var, 1d / 3);
                else
                    var = (7.787 * var) + (16d / 116);
                return var;
            }
            varX = Adjust(varX);
            varY = Adjust(varY);
            varZ = Adjust(varZ);

            // Get the CieLab values
            double l = (116 * varY) - 16;
            double a = 500 * (varX - varY);
            double b = 200 * (varY - varZ);

            // Return the resulting values
            return new(l, a, b, observer, illuminant);
        }

        /// <summary>
        /// Converts the RGB color model to CieLuv
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLuv ToCieLuv(RedGreenBlue rgb)
        {
            if (rgb.parameters.Length != 2)
                return ToCieLuv(rgb, 2, IlluminantType.D65);
            else
            {
                // Validate the values
                int observer = (int)rgb.parameters[0];
                IlluminantType illuminant = (IlluminantType)rgb.parameters[0];
                if ((observer != 2 && observer != 10) || illuminant < 0 || illuminant > IlluminantType.F12)
                    return ToCieLuv(rgb, 2, IlluminantType.D65);
                return ToCieLuv(rgb, observer, illuminant);
            }
        }

        /// <summary>
        /// Converts the RGB color model to CieLuv
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLuv ToCieLuv(RedGreenBlue rgb, int observer, IlluminantType illuminant)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELUVNULLRGB"));

            // Get the illuminant references
            (double refX, double refY, double refZ) = IlluminanceTools.GetIlluminantReferences(observer, illuminant);

            // Get the XYZ values first
            var xyz = ToXyz(rgb);

            // Get the variable U, V, and Y 
            double varU = xyz.X * 4 / (xyz.X + (xyz.Y * 15) + (xyz.Z * 3));
            double varV = xyz.Y * 9 / (xyz.X + (xyz.Y * 15) + (xyz.Z * 3));
            double varY = xyz.Y / 100;
            if (varY > 0.008856)
                varY = Math.Pow(varY, 1d / 3);
            else
                varY = (7.787 * varY) + (16d / 116);

            // Get the reference U and V values
            double refU = refX * 4 / (refX + (refY * 15) + (refZ * 3));
            double refV = refY * 9 / (refX + (refY * 15) + (refZ * 3));

            // Get the CieLuv values
            double l = (116 * varY) - 16;
            double u = 13 * l * (varU - refU);
            double v = 13 * l * (varV - refV);

            // Return the resulting values
            return new(l, u, v, observer, illuminant);
        }

        /// <summary>
        /// Converts the RGB color model to CieLch
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLch ToCieLch(RedGreenBlue rgb)
        {
            if (rgb.parameters.Length != 2)
                return ToCieLch(rgb, 2, IlluminantType.D65);
            else
            {
                // Validate the values
                int observer = (int)rgb.parameters[0];
                IlluminantType illuminant = (IlluminantType)rgb.parameters[0];
                if ((observer != 2 && observer != 10) || illuminant < 0 || illuminant > IlluminantType.F12)
                    return ToCieLch(rgb, 2, IlluminantType.D65);
                return ToCieLch(rgb, observer, illuminant);
            }
        }

        /// <summary>
        /// Converts the RGB color model to CieLch
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static CieLch ToCieLch(RedGreenBlue rgb, int observer, IlluminantType illuminant)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOCIELCHNULLRGB"));

            // Get the CIE-L*ab values first
            var lab = ToCieLab(rgb, observer, illuminant);

            // Get the hue angle
            double h = Math.Atan2(lab.B, lab.A);
            h =
                h > 0 ?
                h / Math.PI * 180 :
                360 - Math.Abs(h) / Math.PI * 180;

            // Get the CieLch values
            double l = lab.L;
            double c = Math.Sqrt(Math.Pow(lab.A, 2) + Math.Pow(lab.B, 2));

            // Return the resulting values
            return new(l, c, h, observer, illuminant);
        }

        /// <summary>
        /// Converts the RGB color model to HWB
        /// </summary>
        /// <param name="rgb">Instance of RGB</param>
        /// <exception cref="TerminauxException"></exception>
        public static HueWhiteBlack ToHwb(RedGreenBlue rgb)
        {
            if (rgb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TOHWBNULLRGB"));

            double levelR = (double)rgb.R / 255;
            double levelG = (double)rgb.G / 255;
            double levelB = (double)rgb.B / 255;

            // Get the minimum and maximum color level. .NET's Math.Max doesn't support three variables, so this workaround is added
            double minRgLevel = Math.Min(levelR, levelG);
            double minLevel = Math.Min(minRgLevel, levelB);
            double maxRgLevel = Math.Max(levelR, levelG);
            double maxLevel = Math.Max(maxRgLevel, levelB);

            // Store the initial value of the hue and the delta color level
            double hue = 0.0d;
            double deltaLevel = maxLevel - minLevel;

            // Get the hue
            if (deltaLevel != 0)
            {
                if (maxLevel == levelR)
                    hue = (levelG - levelB) / deltaLevel + (levelG < levelB ? 6 : 0);
                else if (maxLevel == levelG)
                    hue = (levelB - levelR) / deltaLevel + 2;
                else
                    hue = (levelR - levelG) / deltaLevel + 4;
                hue *= 60;
            }

            // Check the hue value
            if (hue >= 360)
                hue -= 360;

            // Get the whiteness and the blackness
            double epsilon = 1 / 100000d;
            double white = minLevel;
            double black = 1 - maxLevel;
            if (white + black >= 1 - epsilon)
                hue = 0;

            // Return the resulting values
            hue /= 360;
            return new(hue, white, black);
        }
        #endregion
        #region Translate to RGB from...
        /// <summary>
        /// Converts the CMY color model to RGB
        /// </summary>
        /// <param name="cmy">Instance of CMY</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CyanMagentaYellow cmy)
        {
            if (cmy is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLCMY"));

            // Get the level of each color
            double levelC = 1 - (double)cmy.CWhole / 100;
            double levelM = 1 - (double)cmy.MWhole / 100;
            double levelY = 1 - (double)cmy.YWhole / 100;

            // Now, get the Cyan, Magenta, and Yellow values
            int r = (int)Math.Round(255 * levelC);
            int g = (int)Math.Round(255 * levelM);
            int b = (int)Math.Round(255 * levelY);

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the CMYK color model to RGB
        /// </summary>
        /// <param name="cmyk">Instance of CMYK</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CyanMagentaYellowKey cmyk)
        {
            if (cmyk is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLCMYK"));

            // Get the level of each color
            double levelC = 1 - (double)cmyk.CMY.CWhole / 100;
            double levelM = 1 - (double)cmyk.CMY.MWhole / 100;
            double levelY = 1 - (double)cmyk.CMY.YWhole / 100;
            double levelK = 1 - (double)cmyk.KWhole / 100;

            // Now, get the Cyan, Magenta, and Yellow values
            int r = (int)Math.Round(255 * levelC * levelK);
            int g = (int)Math.Round(255 * levelM * levelK);
            int b = (int)Math.Round(255 * levelY * levelK);

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the HSL color model to RGB
        /// </summary>
        /// <param name="hsl">Instance of HSL</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(HueSaturationLightness hsl)
        {
            if (hsl is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLHSL"));

            // Adjust the RGB values according to saturation
            int r, g, b;
            if (hsl.Saturation == 0)
            {
                // The saturation is zero, so no need to do actual conversion. Just use the lightness.
                r = (int)Math.Round(hsl.Lightness * 255);
                g = (int)Math.Round(hsl.Lightness * 255);
                b = (int)Math.Round(hsl.Lightness * 255);
            }
            else
            {
                // First, get the required variables needed for conversion
                double variable1, variable2;
                if (hsl.Lightness < 0.5)
                    variable2 = hsl.Lightness * (1 + hsl.Saturation);
                else
                    variable2 = (hsl.Lightness + hsl.Saturation) - (hsl.Saturation * hsl.Lightness);
                variable1 = 2 * hsl.Lightness - variable2;

                // Now, do the actual work
                r = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue + (1 / 3.0d)));
                g = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue));
                b = (int)Math.Round(255 * GetRgbValueFromHue(variable1, variable2, hsl.Hue - (1 / 3.0d)));
            }

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the HSV color model to RGB
        /// </summary>
        /// <param name="hsv">Instance of HSV</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(HueSaturationValue hsv)
        {
            if (hsv is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLHSV"));

            // Get the saturation
            double rFractional = 0.0d, gFractional = 0.0d, bFractional = 0.0d;
            int r, g, b;
            double saturation = hsv.Saturation;
            double value = hsv.Value;
            if (saturation <= 0)
            {
                rFractional = hsv.Value;
                gFractional = hsv.Value;
                bFractional = hsv.Value;
            }
            else
            {
                double hue = hsv.Hue * 6;
                if (hue == 6)
                    hue = 0;
                double i = Math.Floor(hue);
                double colorVal1 = value * (1 - saturation);
                double colorVal2 = value * (1 - saturation * (hue - i));
                double colorVal3 = value * (1 - saturation * (1 - (hue - i)));

                switch (i)
                {
                    case 0:
                        rFractional = value;
                        gFractional = colorVal3;
                        bFractional = colorVal1;
                        break;
                    case 1:
                        rFractional = colorVal2;
                        gFractional = value;
                        bFractional = colorVal1;
                        break;
                    case 2:
                        rFractional = colorVal1;
                        gFractional = value;
                        bFractional = colorVal3;
                        break;
                    case 3:
                        rFractional = colorVal1;
                        gFractional = colorVal2;
                        bFractional = value;
                        break;
                    case 4:
                        rFractional = colorVal3;
                        gFractional = colorVal1;
                        bFractional = value;
                        break;
                    case 5:
                        rFractional = value;
                        gFractional = colorVal1;
                        bFractional = colorVal2;
                        break;
                }
            }

            // Now, get the Cyan, Magenta, and Yellow values
            r = (int)Math.Round(255 * rFractional);
            g = (int)Math.Round(255 * gFractional);
            b = (int)Math.Round(255 * bFractional);

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the RYB color model to RGB
        /// </summary>
        /// <param name="ryb">Instance of RYB</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(RedYellowBlue ryb)
        {
            if (ryb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLRYB"));

            // Get the whiteness and remove it from all the colors
            double white = Math.Min(Math.Min(ryb.R, ryb.Y), ryb.B);
            double redNoWhite = ryb.R - white;
            double yellowNoWhite = ryb.Y - white;
            double blueNoWhite = ryb.B - white;

            // Get the resulting max value
            double maxRyb = Math.Max(Math.Max(redNoWhite, yellowNoWhite), blueNoWhite);

            // Now, remove the green from the yellow and the blue values
            double greenNoWhite = Math.Min(yellowNoWhite, blueNoWhite);
            yellowNoWhite -= greenNoWhite;
            blueNoWhite -= greenNoWhite;

            // Now, check to see if the blue and the green colors are not zeroes. If true, cut these values to half.
            if (blueNoWhite != 0 && greenNoWhite != 0)
            {
                blueNoWhite /= 2.0;
                greenNoWhite /= 2.0;
            }

            // Now, redistribute the yellow remnants to the red and the green level
            redNoWhite += yellowNoWhite;
            greenNoWhite += yellowNoWhite;

            // Do some normalization
            double maxRgb = Math.Max(Math.Max(redNoWhite, greenNoWhite), blueNoWhite);
            if (maxRgb != 0)
            {
                double normalizationFactor = maxRyb / maxRgb;
                redNoWhite *= normalizationFactor;
                greenNoWhite *= normalizationFactor;
                blueNoWhite *= normalizationFactor;
            }

            // Now, restore the white color to the three components
            redNoWhite += white;
            greenNoWhite += white;
            blueNoWhite += white;

            // Install the values
            return new((int)redNoWhite, (int)greenNoWhite, (int)blueNoWhite);
        }

        /// <summary>
        /// Converts the YIQ color model to RGB
        /// </summary>
        /// <param name="yiq">Instance of YIQ</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(LumaInPhaseQuadrature yiq)
        {
            if (yiq is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLYIQ"));

            // Get the RGB by matrix transform
            int r = (int)Math.Round(yiq.Luma + (0.956 * (yiq.InPhase - 128)) + (0.621 * (yiq.Quadrature - 128)));
            int g = (int)Math.Round(yiq.Luma + (-0.272 * (yiq.InPhase - 128)) + (-0.647 * (yiq.Quadrature - 128)));
            int b = (int)Math.Round((yiq.Luma + (-1.105 * (yiq.InPhase - 128)) + (1.702 * (yiq.Quadrature - 128))));

            // Verify that we don't go out of bounds
            if (r < 0)
                r = 0;
            else if (r > 255)
                r = 255;
            if (g < 0)
                g = 0;
            else if (g > 255)
                g = 255;
            if (b < 0)
                b = 0;
            else if (b > 255)
                b = 255;

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the YUV color model to RGB
        /// </summary>
        /// <param name="yuv">Instance of YUV</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(LumaChromaUv yuv)
        {
            if (yuv is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLYUV"));

            // Get the RGB by matrix transform
            int r = (int)Math.Round(yuv.Luma + 1.4075 * (yuv.ChromaV - 128));
            int g = (int)Math.Round(yuv.Luma - 0.3455 * (yuv.ChromaU - 128) - (0.7169 * (yuv.ChromaV - 128)));
            int b = (int)Math.Round(yuv.Luma + 1.7790 + (yuv.ChromaU - 128));

            // Verify that we don't go out of bounds
            if (r < 0)
                r = 0;
            else if (r > 255)
                r = 255;
            if (g < 0)
                g = 0;
            else if (g > 255)
                g = 255;
            if (b < 0)
                b = 0;
            else if (b > 255)
                b = 255;

            // Install the values
            return new(r, g, b);
        }

        /// <summary>
        /// Converts the XYZ color model to RGB
        /// </summary>
        /// <param name="xyz">Instance of XYZ</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(Xyz xyz)
        {
            if (xyz is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLXYZ"));

            // Get the normalized xyz values
            double x = xyz.X / 100d;
            double y = xyz.Y / 100d;
            double z = xyz.Z / 100d;

            // Now, convert them to RGB
            double r = x * 3.2406d + y * -1.5372d + z * -0.4986d;
            double g = x * -0.9689d + y * 1.8758d + z * 0.0415d;
            double b = x * 0.0557d + y * -0.2040d + z * 1.0570d;
            r = (r > 0.0031308) ? 1.055d * Math.Pow(r, 1 / 2.4d) - 0.055 : r * 12.92d;
            g = (g > 0.0031308) ? 1.055d * Math.Pow(g, 1 / 2.4d) - 0.055 : g * 12.92d;
            b = (b > 0.0031308) ? 1.055d * Math.Pow(b, 1 / 2.4d) - 0.055 : b * 12.92d;

            int rWhole = (int)(r * 255);
            int gWhole = (int)(g * 255);
            int bWhole = (int)(b * 255);

            // Install the values
            return new(rWhole, gWhole, bWhole);
        }

        /// <summary>
        /// Converts the YXY color model to RGB
        /// </summary>
        /// <param name="yxy">Instance of YXY</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(Yxy yxy)
        {
            if (yxy is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLYXY"));

            // Get the normalized xyz values
            double x = yxy.X * (yxy.Y2 / yxy.Y1) / 100d;
            double y = yxy.Y2 / 100d;
            double z = (1 - yxy.X - yxy.Y1) * (yxy.Y2 / yxy.Y1) / 100d;

            // Now, convert them to RGB
            double r = x * 3.2406d + y * -1.5372d + z * -0.4986d;
            double g = x * -0.9689d + y * 1.8758d + z * 0.0415d;
            double b = x * 0.0557d + y * -0.2040d + z * 1.0570d;
            r = (r > 0.0031308) ? 1.055d * Math.Pow(r, 1 / 2.4d) - 0.055 : r * 12.92d;
            g = (g > 0.0031308) ? 1.055d * Math.Pow(g, 1 / 2.4d) - 0.055 : g * 12.92d;
            b = (b > 0.0031308) ? 1.055d * Math.Pow(b, 1 / 2.4d) - 0.055 : b * 12.92d;

            int rWhole = (int)(r * 255);
            int gWhole = (int)(g * 255);
            int bWhole = (int)(b * 255);

            // Install the values
            return new(rWhole, gWhole, bWhole);
        }

        /// <summary>
        /// Converts the HunterLab color model to RGB
        /// </summary>
        /// <param name="hunterLab">Instance of HunterLab</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(HunterLab hunterLab)
        {
            if (hunterLab is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLHUNTERLAB"));

            // Get the normalized xyz values
            double varY = hunterLab.L / 10;
            double varX = hunterLab.A / 17.5 * hunterLab.L / 10;
            double varZ = hunterLab.B / 7 * hunterLab.L / 10;
            double y = Math.Pow(varY, 2);
            double x = (varX + y) / 1.02 / 100d;
            double z = -(varZ - y) / 0.847 / 100d;
            y /= 100;

            // Now, convert them to RGB
            double r = x * 3.2406d + y * -1.5372d + z * -0.4986d;
            double g = x * -0.9689d + y * 1.8758d + z * 0.0415d;
            double b = x * 0.0557d + y * -0.2040d + z * 1.0570d;
            r = (r > 0.0031308) ? 1.055d * Math.Pow(r, 1 / 2.4d) - 0.055 : r * 12.92d;
            g = (g > 0.0031308) ? 1.055d * Math.Pow(g, 1 / 2.4d) - 0.055 : g * 12.92d;
            b = (b > 0.0031308) ? 1.055d * Math.Pow(b, 1 / 2.4d) - 0.055 : b * 12.92d;

            int rWhole = (int)(r * 255);
            int gWhole = (int)(g * 255);
            int bWhole = (int)(b * 255);

            // Install the values
            return new(rWhole, gWhole, bWhole);
        }

        /// <summary>
        /// Converts the CieLab color model to RGB
        /// </summary>
        /// <param name="cieLab">Instance of CieLab</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLab cieLab) =>
            ToRgb(cieLab, cieLab.Observer, cieLab.Illuminant);

        /// <summary>
        /// Converts the CieLab color model to RGB
        /// </summary>
        /// <param name="cieLab">Instance of CieLab</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLab cieLab, int observer, IlluminantType illuminant)
        {
            if (cieLab is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLCIELAB"));

            // Get the illuminant references
            (double refX, double refY, double refZ) = IlluminanceTools.GetIlluminantReferences(observer, illuminant);

            // Get the normalized xyz values
            double varY = (cieLab.L + 16) / 116;
            double varX = cieLab.A / 500 + varY;
            double varZ = varY - cieLab.B / 200;

            // Adjust the variable values
            static double Adjust(double var)
            {
                double varCubed = Math.Pow(var, 3);
                if (varCubed > 0.008856)
                    var = varCubed;
                else
                    var = (var - 16d / 116) / 7.787;
                return var;
            }
            double x = Adjust(varX) * refX / 100d;
            double y = Adjust(varY) * refY / 100d;
            double z = Adjust(varZ) * refZ / 100d;

            // Now, convert them to RGB
            double r = x * 3.2406d + y * -1.5372d + z * -0.4986d;
            double g = x * -0.9689d + y * 1.8758d + z * 0.0415d;
            double b = x * 0.0557d + y * -0.2040d + z * 1.0570d;
            r = (r > 0.0031308) ? 1.055d * Math.Pow(r, 1 / 2.4d) - 0.055 : r * 12.92d;
            g = (g > 0.0031308) ? 1.055d * Math.Pow(g, 1 / 2.4d) - 0.055 : g * 12.92d;
            b = (b > 0.0031308) ? 1.055d * Math.Pow(b, 1 / 2.4d) - 0.055 : b * 12.92d;

            int rWhole = (int)(r * 255);
            int gWhole = (int)(g * 255);
            int bWhole = (int)(b * 255);

            // Install the values
            return new(rWhole, gWhole, bWhole)
            {
                parameters = [observer, illuminant]
            };
        }

        /// <summary>
        /// Converts the CieLuv color model to RGB
        /// </summary>
        /// <param name="cieLuv">Instance of CieLuv</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLuv cieLuv) =>
            ToRgb(cieLuv, cieLuv.Observer, cieLuv.Illuminant);

        /// <summary>
        /// Converts the CieLuv color model to RGB
        /// </summary>
        /// <param name="cieLuv">Instance of CieLuv</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLuv cieLuv, int observer, IlluminantType illuminant)
        {
            if (cieLuv is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLCIELUV"));

            // Get a variable Y value
            double varY = (cieLuv.L + 16) / 116;
            double varYCubed = Math.Pow(varY, 3);
            if (varYCubed > 0.008856)
                varY = varYCubed;
            else
                varY = (varY - 16 / 116d) / 7.787;

            // Get the illuminant references
            (double refX, double refY, double refZ) = IlluminanceTools.GetIlluminantReferences(observer, illuminant);

            // Get the reference U and V values
            double refU = refX * 4 / (refX + (refY * 15) + (refZ * 3));
            double refV = refY * 9 / (refX + (refY * 15) + (refZ * 3));

            // Get the variable U and V values
            double varU = cieLuv.U / (13 * cieLuv.L) + refU;
            double varV = cieLuv.V / (13 * cieLuv.L) + refV;

            // Get the normalized xyz values
            double y = varY * 100d;
            double x = -(y * 9 * varU) / ((varU - 4) * varV - varU * varV) / 100d;
            double z = (y * 9 - (15 * varV * y) - (varV * x)) / (varV * 3) / 100d;
            y /= 100d;

            // Now, convert them to RGB
            double r = x * 3.2406d + y * -1.5372d + z * -0.4986d;
            double g = x * -0.9689d + y * 1.8758d + z * 0.0415d;
            double b = x * 0.0557d + y * -0.2040d + z * 1.0570d;
            r = (r > 0.0031308) ? 1.055d * Math.Pow(r, 1 / 2.4d) - 0.055 : r * 12.92d;
            g = (g > 0.0031308) ? 1.055d * Math.Pow(g, 1 / 2.4d) - 0.055 : g * 12.92d;
            b = (b > 0.0031308) ? 1.055d * Math.Pow(b, 1 / 2.4d) - 0.055 : b * 12.92d;

            int rWhole = (int)(r * 255);
            int gWhole = (int)(g * 255);
            int bWhole = (int)(b * 255);

            // Install the values
            return new(rWhole, gWhole, bWhole)
            {
                parameters = [observer, illuminant]
            };
        }

        /// <summary>
        /// Converts the CieLch color model to RGB
        /// </summary>
        /// <param name="cieLch">Instance of CieLch</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLch cieLch) =>
            ToRgb(cieLch, cieLch.Observer, cieLch.Illuminant);

        /// <summary>
        /// Converts the CieLch color model to RGB
        /// </summary>
        /// <param name="cieLch">Instance of CieLch</param>
        /// <param name="observer">Observer (2 degs or 10 degs)</param>
        /// <param name="illuminant">Illuminant type</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(CieLch cieLch, int observer, IlluminantType illuminant)
        {
            if (cieLch is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLCIELCH"));

            static double DegToRad(double angle) =>
                Math.PI / 180 * angle;

            // Get the CIE-L*ab value
            double l = cieLch.L;
            double a = Math.Cos(DegToRad(cieLch.H)) * cieLch.C;
            double b = Math.Sin(DegToRad(cieLch.H)) * cieLch.C;
            var lab = new CieLab(l, a, b, observer, illuminant);

            // Return RGB from parsed CIE-L*ab value
            return ToRgb(lab);
        }

        /// <summary>
        /// Converts the HWB color model to RGB
        /// </summary>
        /// <param name="hwb">Instance of HWB</param>
        /// <exception cref="TerminauxException"></exception>
        public static RedGreenBlue ToRgb(HueWhiteBlack hwb)
        {
            if (hwb is null)
                throw new TerminauxException(LanguageTools.GetLocalized("T_COLOR_MODEL_EXCEPTION_TORGBNULLHWB"));

            // Grayscale if both the whiteness and the blackness exceed a threshold
            double whiteBlack = hwb.Whiteness + hwb.Blackness;
            if (whiteBlack >= 1)
            {
                var grayscale = hwb.Whiteness / (whiteBlack);
                int grayscaleWhole = (int)(grayscale * 255);
                return new(grayscaleWhole, grayscaleWhole, grayscaleWhole);
            }

            // Convert HSL to RGB
            var hsl = new HueSaturationLightness(hwb.Hue, 1, 0.5);
            var rgbFromHsl = ToRgb(hsl);

            // Perform final calculations
            int r = (int)((rgbFromHsl.RNormalized * (1 - whiteBlack) + hwb.Whiteness) * 255);
            int g = (int)((rgbFromHsl.GNormalized * (1 - whiteBlack) + hwb.Whiteness) * 255);
            int b = (int)((rgbFromHsl.BNormalized * (1 - whiteBlack) + hwb.Whiteness) * 255);

            // Install the values
            return new(r, g, b);
        }

        private static double GetRgbValueFromHue(double variable1, double variable2, double variableHue)
        {
            // Check the hue
            if (variableHue < 0)
                variableHue++;
            if (variableHue > 1)
                variableHue--;

            // Now, get the actual value according to the hue
            if ((6 * variableHue) < 1)
                return variable1 + (variable2 - variable1) * 6 * variableHue;
            else if ((2 * variableHue) < 1)
                return variable2;
            else if ((3 * variableHue) < 2)
                return variable1 + (variable2 - variable1) * ((2 / 3.0d - variableHue) * 6);
            return variable1;
        }
        #endregion
        #endregion
    }
}
