
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
using Terminaux.Colors.Accessibility;
using Terminaux.Colors.Models;

namespace Terminaux.Colors
{
    internal static class ColorParser
    {
        internal static RedGreenBlue ParseSpecifierRgbValues(string specifier)
        {
            if (!specifier.Contains(";"))
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the RGB values! First, check to see if we need to filter the color for the color-blind
                int r = Convert.ToInt32(specifierArray[0]);
                if (r < 0 || r > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                int g = Convert.ToInt32(specifierArray[1]);
                if (g < 0 || g > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
                int b = Convert.ToInt32(specifierArray[2]);
                if (b < 0 || b > 255)
                    throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

                // Now, transform
                var finalRgb = GetTransformedColor(r, g, b);

                // Make a new RGB class
                return new(finalRgb.r, finalRgb.g, finalRgb.b);
            }
            else
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
        }

        internal static (RedGreenBlue rgb, ConsoleColorsInfo cci) ParseSpecifierRgbName(string specifier)
        {
            if (!(double.TryParse(specifier, out double specifierNum) && specifierNum <= 255 || Enum.IsDefined(typeof(ConsoleColors), specifier)))
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Form the sequences using the information from the color details
            var parsedEnum = (ConsoleColors)Enum.Parse(typeof(ConsoleColors), specifier);
            var ColorsInfo = new ConsoleColorsInfo(parsedEnum);

            // Check to see if we need to transform color. Else, be sane.
            int r = Convert.ToInt32(ColorsInfo.R);
            if (r < 0 || r > 255)
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
            int g = Convert.ToInt32(ColorsInfo.G);
            if (g < 0 || g > 255)
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");
            int b = Convert.ToInt32(ColorsInfo.B);
            if (b < 0 || b > 255)
                throw new ColorSeqException("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows: <R>;<G>;<B>");

            // Now, transform
            var finalRgb = GetTransformedColor(r, g, b);

            // Make a new RGB class
            return (new(finalRgb.r, finalRgb.g, finalRgb.b), ColorsInfo);
        }

        internal static RedGreenBlue ParseSpecifierRgbHash(string specifier)
        {
            if (!specifier.StartsWith("#"))
                throw new ColorSeqException("Invalid color hex specifier. Ensure that it's on the correct format: #RRGGBB");

            // Get the integral value of the total color
            string finalSpecifier = specifier.Substring(1);
            int ColorDecimal = Convert.ToInt32(finalSpecifier, 16);

            // Convert the RGB values to numbers
            int r = (byte)((ColorDecimal & 0xFF0000) >> 0x10);
            int g = (byte)((ColorDecimal & 0xFF00) >> 8);
            int b = (byte)(ColorDecimal & 0xFF);

            // Now, transform
            var finalRgb = GetTransformedColor(r, g, b);

            // Make a new RGB class
            return new(finalRgb.r, finalRgb.g, finalRgb.b);
        }

        internal static RedGreenBlue ParseSpecifierCmykValues(string specifier)
        {
            if (!specifier.Contains(";") || !specifier.StartsWith("cmyk:"))
                throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(5).Split(';');
            if (specifierArray.Length == 4)
            {
                // We got the CMYK whole values! First, check to see if we need to filter the color for the color-blind
                int c = Convert.ToInt32(specifierArray[0]);
                if (c < 0 || c > 100)
                    throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");
                int m = Convert.ToInt32(specifierArray[1]);
                if (m < 0 || m > 100)
                    throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");
                int y = Convert.ToInt32(specifierArray[2]);
                if (y < 0 || y > 100)
                    throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");
                int k = Convert.ToInt32(specifierArray[3]);
                if (k < 0 || k > 100)
                    throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");

                // First, we need to convert from CMYK to RGB
                double cPart = (double)c / 100;
                double mPart = (double)m / 100;
                double yPart = (double)y / 100;
                double kPart = (double)k / 100;
                var cmyk = new CyanMagentaYellowKey(kPart, new(cPart, mPart, yPart));
                var rgb = cmyk.ConvertToRgb();
                int r = rgb.R;
                int g = rgb.G;
                int b = rgb.B;

                // Now, transform
                var finalRgb = GetTransformedColor(r, g, b);

                // Make a new RGB class
                return new(finalRgb.r, finalRgb.g, finalRgb.b);
            }
            else
                throw new ColorSeqException("Invalid CMYK color specifier. Ensure that it's on the correct format: cmyk:<C>;<M>;<Y>;<K>");
        }

        internal static RedGreenBlue ParseSpecifierHslValues(string specifier)
        {
            if (!specifier.Contains(";") || !specifier.StartsWith("hsl:"))
                throw new ColorSeqException("Invalid HSL color specifier. Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");

            // Split the VT sequence into three parts
            var specifierArray = specifier.Substring(4).Split(';');
            if (specifierArray.Length == 3)
            {
                // We got the HSL whole values! First, check to see if we need to filter the color for the color-blind
                int h = Convert.ToInt32(specifierArray[0]);
                if (h < 0 || h > 360)
                    throw new ColorSeqException("Invalid HSL color specifier. Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");
                int s = Convert.ToInt32(specifierArray[1]);
                if (s < 0 || s > 100)
                    throw new ColorSeqException("Invalid HSL color specifier. Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");
                int l = Convert.ToInt32(specifierArray[2]);
                if (l < 0 || l > 100)
                    throw new ColorSeqException("Invalid HSL color specifier. Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");

                // First, we need to convert from HSL to RGB
                double hPart = (double)h / 360;
                double sPart = (double)s / 100;
                double lPart = (double)l / 100;
                var hsl = new HueSaturationLightness(hPart, sPart, lPart);
                var rgb = hsl.ConvertToRgb();
                int r = rgb.R;
                int g = rgb.G;
                int b = rgb.B;

                // Now, transform
                var finalRgb = GetTransformedColor(r, g, b);

                // Make a new RGB class
                return new(finalRgb.r, finalRgb.g, finalRgb.b);
            }
            else
                throw new ColorSeqException("Invalid HSL color specifier. Ensure that it's on the correct format: hsl:<hue>;<sat>;<lig>");
        }

        private static (int r, int g, int b) GetTransformedColor(int rInput, int gInput, int bInput)
        {
            if (ColorTools.EnableColorTransformation)
            {
                // We'll transform.
                (int, int, int) transformed;
                if (ColorTools.ColorDeficiency == Deficiency.Monochromacy)
                    transformed = Monochromacy.Transform(rInput, gInput, bInput);
                else
                {
                    if (ColorTools.EnableSimpleColorTransformation)
                        transformed = Vienot1999.Transform(rInput, gInput, bInput, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                    else
                        transformed = Brettel1997.Transform(rInput, gInput, bInput, ColorTools.ColorDeficiency, ColorTools.ColorDeficiencySeverity);
                }
                return (transformed.Item1, transformed.Item2, transformed.Item3);
            }
            return (rInput, gInput, bInput);
        }
    }
}
