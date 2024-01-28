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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Diagnostics;
using Terminaux.Colors;
using Terminaux.Colors.Interop;

namespace Terminaux.Tests.Colors
{

    [TestClass]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [Description("Querying")]
        public void TestTryParseColorFromHex(string TargetHex, bool expected)
        {
            Debug.WriteLine($"Trying {TargetHex}...");
            bool result = ColorTools.TryParseColor(TargetHex);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [DataRow(26, true)]
        [DataRow(260, false)]
        [DataRow(-26, false)]
        [Description("Querying")]
        public void TestTryParseColorFromColorNum(int TargetColorNum, bool expected)
        {
            Debug.WriteLine($"Trying colornum {TargetColorNum}...");
            bool result = ColorTools.TryParseColor(TargetColorNum);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [DataRow(4, 4, 4, true)]
        [DataRow(400, 4, 4, false)]
        [DataRow(4, 400, 4, false)]
        [DataRow(4, 4, 400, false)]
        [DataRow(4, 400, 400, false)]
        [DataRow(400, 4, 400, false)]
        [DataRow(400, 400, 4, false)]
        [DataRow(400, 400, 400, false)]
        [DataRow(-4, 4, 4, false)]
        [DataRow(4, -4, 4, false)]
        [DataRow(4, 4, -4, false)]
        [DataRow(4, -4, -4, false)]
        [DataRow(-4, 4, -4, false)]
        [DataRow(-4, -4, 4, false)]
        [DataRow(-4, -4, -4, false)]
        [Description("Querying")]
        public void TestTryParseColorFromRGB(int R, int G, int B, bool expected)
        {
            Debug.WriteLine($"Trying rgb {R}, {G}, {B}...");
            bool result = ColorTools.TryParseColor(R, G, B);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [DataRow("4;4;4", true)]
        [DataRow("400;4;4", false)]
        [DataRow("4;400;4", false)]
        [DataRow("4;4;400", false)]
        [DataRow("4;400;400", false)]
        [DataRow("400;4;400", false)]
        [DataRow("400;400;4", false)]
        [DataRow("400;400;400", false)]
        [DataRow("-4;4;4", false)]
        [DataRow("4;-4;4", false)]
        [DataRow("4;4;-4", false)]
        [DataRow("4;-4;-4", false)]
        [DataRow("-4;4;-4", false)]
        [DataRow("-4;-4;4", false)]
        [DataRow("-4;-4;-4", false)]
        [Description("Querying")]
        public void TestTryParseColorFromSpecifier(string specifier, bool expected)
        {
            Debug.WriteLine($"Trying rgb specifier {specifier}...");
            bool result = ColorTools.TryParseColor(specifier);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the contrast color (NTSC)
        /// </summary>
        [DataRow("#EF4444", "#FFFFFF")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#FFFFFF")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#FFFFFF")]
        [DataRow("#D54799", "#FFFFFF")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#FFFFFF")]
        [DataRow("#333333", "#FFFFFF")]
        [DataRow("#FF0000", "#FFFFFF")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#FFFFFF")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#FFFFFF")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#FFFFFF")]
        [DataRow("#006666", "#FFFFFF")]
        [DataRow("#0099CC", "#FFFFFF")]
        [DataRow("#666600", "#FFFFFF")]
        [DataRow("#CC00CC", "#FFFFFF")]
        [DataRow("#CC6666", "#000000")]
        [Description("Querying")]
        public void TestGetContrastColorNtsc(string specifier, string expected)
        {
            string result = ColorContrast.GetContrastColorNtsc(specifier).Hex;
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the contrast color (Half)
        /// </summary>
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#FFFFFF")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#FFFFFF")]
        [DataRow("#D54799", "#000000")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#FFFFFF")]
        [DataRow("#333333", "#FFFFFF")]
        [DataRow("#FF0000", "#000000")]
        [DataRow("#00FF00", "#FFFFFF")]
        [DataRow("#0000FF", "#FFFFFF")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#FFFFFF")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#FFFFFF")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#FFFFFF")]
        [DataRow("#0099CC", "#FFFFFF")]
        [DataRow("#666600", "#FFFFFF")]
        [DataRow("#CC00CC", "#000000")]
        [DataRow("#CC6666", "#000000")]
        [Description("Querying")]
        public void TestGetContrastColorHalf(string specifier, string expected)
        {
            string result = ColorContrast.GetContrastColorHalf(specifier).Hex;
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the contrast color (NTSC)
        /// </summary>
        [DataRow("#EF4444", "#FFFFFF")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#FFFFFF")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#FFFFFF")]
        [DataRow("#D54799", "#FFFFFF")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#FFFFFF")]
        [DataRow("#333333", "#FFFFFF")]
        [DataRow("#FF0000", "#FFFFFF")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#FFFFFF")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#FFFFFF")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#FFFFFF")]
        [DataRow("#006666", "#FFFFFF")]
        [DataRow("#0099CC", "#FFFFFF")]
        [DataRow("#666600", "#FFFFFF")]
        [DataRow("#CC00CC", "#FFFFFF")]
        [DataRow("#CC6666", "#000000")]
        [Description("Querying")]
        public void TestGetContrastColorUsingGetGrayNtsc(string specifier, string expected)
        {
            string result = ColorTools.GetGray(specifier, ColorContrastType.Ntsc).Hex;
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the contrast color (Half)
        /// </summary>
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#FFFFFF")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#FFFFFF")]
        [DataRow("#D54799", "#000000")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#FFFFFF")]
        [DataRow("#333333", "#FFFFFF")]
        [DataRow("#FF0000", "#000000")]
        [DataRow("#00FF00", "#FFFFFF")]
        [DataRow("#0000FF", "#FFFFFF")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#FFFFFF")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#FFFFFF")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#FFFFFF")]
        [DataRow("#0099CC", "#FFFFFF")]
        [DataRow("#666600", "#FFFFFF")]
        [DataRow("#CC00CC", "#000000")]
        [DataRow("#CC6666", "#000000")]
        [Description("Querying")]
        public void TestGetContrastColorUsingGetGrayHalf(string specifier, string expected)
        {
            string result = ColorTools.GetGray(specifier, ColorContrastType.Half).Hex;
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the contrast color (Light)
        /// </summary>
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#000000")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#000000")]
        [DataRow("#D54799", "#000000")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#000000")]
        [DataRow("#333333", "#000000")]
        [DataRow("#FF0000", "#000000")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#000000")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#000000")]
        [DataRow("#0099CC", "#000000")]
        [DataRow("#666600", "#000000")]
        [DataRow("#003045", "#C0C0C0")]
        [DataRow("#CC00CC", "#000000")]
        [DataRow("#CC6666", "#000000")]
        [Description("Querying")]
        public void TestGetContrastColorUsingGetGrayLight(string specifier, string expected)
        {
            string result = ColorTools.GetGray(specifier, ColorContrastType.Light).Hex;
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Color to Drawing.Color
        /// </summary>
        [DataRow("#FFFFEE", "255;255;238")]
        [Description("Querying")]
        public void TestConvertToDrawing(string specifier, string expected)
        {
            var drawing = SystemColorConverter.ToDrawingColor(specifier);
            string result = $"{drawing.R};{drawing.G};{drawing.B}";
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Drawing.Color to Color
        /// </summary>
        [DataRow(255, 255, 238, "255;255;238")]
        [Description("Querying")]
        public void TestConvertFromDrawing(int r, int g, int b, string expected)
        {
            var drawing = System.Drawing.Color.FromArgb(r, g, b);
            var our = SystemColorConverter.FromDrawingColor(drawing);
            string result = our.ToString();
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Color to Drawing.Color (null check)
        /// </summary>
        [DataRow("0;0;0")]
        [Description("Querying")]
        public void TestConvertToDrawingWithNull(string expected)
        {
            var drawing = SystemColorConverter.ToDrawingColor(null);
            string result = $"{drawing.R};{drawing.G};{drawing.B}";
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Drawing.Color to Color (null check)
        /// </summary>
        [DataRow("0")]
        [Description("Querying")]
        public void TestConvertFromDrawingWithNull(string expected)
        {
            var our = SystemColorConverter.FromDrawingColor(System.Drawing.Color.Empty);
            string result = our.ToString();
            result.ShouldBe(expected);
        }
    }
}
