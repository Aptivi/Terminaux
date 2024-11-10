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
using Terminaux.Colors.Data;
using Terminaux.Colors.Gradients;
using Terminaux.Colors.Interop;
using Terminaux.Colors.Transformation;
using Terminaux.Colors.Transformation.Contrast;

namespace Terminaux.Tests.Colors
{

    [TestClass]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
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
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, true)]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
        [DataRow("#EF4444", "#C0C0C0")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#C0C0C0")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#C0C0C0")]
        [DataRow("#D54799", "#C0C0C0")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#C0C0C0")]
        [DataRow("#333333", "#C0C0C0")]
        [DataRow("#FF0000", "#C0C0C0")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#C0C0C0")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#C0C0C0")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#C0C0C0")]
        [DataRow("#006666", "#C0C0C0")]
        [DataRow("#0099CC", "#C0C0C0")]
        [DataRow("#666600", "#C0C0C0")]
        [DataRow("#CC00CC", "#C0C0C0")]
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
        [TestMethod]
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#C0C0C0")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#C0C0C0")]
        [DataRow("#D54799", "#000000")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#C0C0C0")]
        [DataRow("#333333", "#C0C0C0")]
        [DataRow("#FF0000", "#000000")]
        [DataRow("#00FF00", "#C0C0C0")]
        [DataRow("#0000FF", "#C0C0C0")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#C0C0C0")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#C0C0C0")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#C0C0C0")]
        [DataRow("#0099CC", "#C0C0C0")]
        [DataRow("#666600", "#C0C0C0")]
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
        [TestMethod]
        [DataRow("#EF4444", "#C0C0C0")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#C0C0C0")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#C0C0C0")]
        [DataRow("#D54799", "#C0C0C0")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#C0C0C0")]
        [DataRow("#333333", "#C0C0C0")]
        [DataRow("#FF0000", "#C0C0C0")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#C0C0C0")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#C0C0C0")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#C0C0C0")]
        [DataRow("#006666", "#C0C0C0")]
        [DataRow("#0099CC", "#C0C0C0")]
        [DataRow("#666600", "#C0C0C0")]
        [DataRow("#CC00CC", "#C0C0C0")]
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
        [TestMethod]
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#C0C0C0")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#C0C0C0")]
        [DataRow("#D54799", "#000000")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#C0C0C0")]
        [DataRow("#333333", "#C0C0C0")]
        [DataRow("#FF0000", "#000000")]
        [DataRow("#00FF00", "#C0C0C0")]
        [DataRow("#0000FF", "#C0C0C0")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#C0C0C0")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#C0C0C0")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#C0C0C0")]
        [DataRow("#0099CC", "#C0C0C0")]
        [DataRow("#666600", "#C0C0C0")]
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
        [TestMethod]
        [DataRow("#EF4444", "#000000")]
        [DataRow("#FAA31B", "#000000")]
        [DataRow("#FFF000", "#000000")]
        [DataRow("#82C341", "#000000")]
        [DataRow("#009F75", "#000000")]
        [DataRow("#88C6ED", "#000000")]
        [DataRow("#394BA0", "#C0C0C0")]
        [DataRow("#D54799", "#C0C0C0")]
        [DataRow("#CCCCCC", "#000000")]
        [DataRow("#999999", "#000000")]
        [DataRow("#666666", "#C0C0C0")]
        [DataRow("#333333", "#C0C0C0")]
        [DataRow("#FF0000", "#C0C0C0")]
        [DataRow("#00FF00", "#000000")]
        [DataRow("#0000FF", "#C0C0C0")]
        [DataRow("#FFFF00", "#000000")]
        [DataRow("#FF00FF", "#000000")]
        [DataRow("#00FFFF", "#000000")]
        [DataRow("#FFCC00", "#000000")]
        [DataRow("#CCFF00", "#000000")]
        [DataRow("#00CCFF", "#000000")]
        [DataRow("#FF6600", "#000000")]
        [DataRow("#FF0066", "#000000")]
        [DataRow("#006666", "#C0C0C0")]
        [DataRow("#0099CC", "#000000")]
        [DataRow("#666600", "#C0C0C0")]
        [DataRow("#003045", "#C0C0C0")]
        [DataRow("#CC00CC", "#C0C0C0")]
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
        [TestMethod]
        [DataRow("#FFFFEE", 255, "255;255;238 @ 255")]
        [DataRow("#FFFFEE", 128, "255;255;238 @ 128")]
        [DataRow("#FFFFEE", 64, "255;255;238 @ 64")]
        [DataRow("#FFFFEE", 32, "255;255;238 @ 32")]
        [DataRow("#FFFFEE", 16, "255;255;238 @ 16")]
        [DataRow("#FFFFEE", 0, "255;255;238 @ 0")]
        [Description("Querying")]
        public void TestConvertToDrawing(string specifier, int alpha, string expected)
        {
            var color = new Color(specifier, new ColorSettings() { Opacity = alpha });
            var drawing = SystemColorConverter.ToDrawingColor(color);
            string result = $"{drawing.R};{drawing.G};{drawing.B} @ {drawing.A}";
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Drawing.Color to Color
        /// </summary>
        [TestMethod]
        [DataRow(255, 255, 238, 255, "255;255;238")]
        [DataRow(255, 255, 238, 128, "128;128;119")]
        [DataRow(255, 255, 238, 64, "64;64;59")]
        [DataRow(255, 255, 238, 32, "32;32;29")]
        [DataRow(255, 255, 238, 16, "16;16;14")]
        [DataRow(255, 255, 238, 0, "0;0;0")]
        [Description("Querying")]
        public void TestConvertFromDrawing(int r, int g, int b, int a, string expected)
        {
            var drawing = System.Drawing.Color.FromArgb(a, r, g, b);
            var our = SystemColorConverter.FromDrawingColor(drawing);
            string result = our.ToString();
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to convert from Color to Drawing.Color (null check)
        /// </summary>
        [TestMethod]
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
        [TestMethod]
        [DataRow("0")]
        [Description("Querying")]
        public void TestConvertFromDrawingWithNull(string expected)
        {
            var our = SystemColorConverter.FromDrawingColor(System.Drawing.Color.Empty);
            string result = our.ToString();
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get the color gradients
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetGradients()
        {
            var source = new Color(ConsoleColors.Green);
            var target = new Color(ConsoleColors.DarkGreen);
            int steps = 10;
            var grads = ColorGradients.GetGradients(source, target, steps);
            grads.ShouldNotBeNull();
            grads.ShouldNotBeEmpty();
            grads.Count.ShouldBe(steps + 1);
            grads[0].ShouldNotBeNull();
            grads[0].IntermediateColor.PlainSequenceTrueColor.ShouldBe(source.PlainSequenceTrueColor);
            grads[grads.Count - 1].ShouldNotBeNull();
            grads[grads.Count - 1].IntermediateColor.PlainSequenceTrueColor.ShouldBe(target.PlainSequenceTrueColor);
        }

        /// <summary>
        /// Tests trying to get the color gradients
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetSmoothGradients()
        {
            var source = new Color(ConsoleColors.Green);
            var target = new Color(ConsoleColors.DarkGreen);
            int steps = 200;
            var grads = ColorGradients.GetGradients(source, target, steps);
            grads.ShouldNotBeNull();
            grads.ShouldNotBeEmpty();
            grads.Count.ShouldBe(steps + 1);
            grads[0].ShouldNotBeNull();
            grads[0].IntermediateColor.PlainSequenceTrueColor.ShouldBe(source.PlainSequenceTrueColor);
            grads[grads.Count - 1].ShouldNotBeNull();
            grads[grads.Count - 1].IntermediateColor.PlainSequenceTrueColor.ShouldBe(target.PlainSequenceTrueColor);
        }

        /// <summary>
        /// Tests trying to get the color gradients
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void TestGetOneStepGradients()
        {
            var source = new Color(ConsoleColors.Green);
            var target = new Color(ConsoleColors.DarkGreen);
            int steps = 1;
            var grads = ColorGradients.GetGradients(source, target, steps);
            grads.ShouldNotBeNull();
            grads.ShouldNotBeEmpty();
            grads.Count.ShouldBe(2);
            grads[0].ShouldNotBeNull();
            grads[0].IntermediateColor.PlainSequenceTrueColor.ShouldBe(source.PlainSequenceTrueColor);
            grads[grads.Count - 1].ShouldNotBeNull();
            grads[grads.Count - 1].IntermediateColor.PlainSequenceTrueColor.ShouldBe(target.PlainSequenceTrueColor);
        }

        /// <summary>
        /// Tests trying to get a blended color
        /// </summary>
        [TestMethod]
        [Description("Querying")]
        public void GetBlendedColor()
        {
            var source = new Color(10, 20, 30);
            var target = new Color(30, 40, 50);
            var expected = new Color(20, 30, 40);
            var actual = TransformationTools.BlendColor(source, target);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to get a blended color
        /// </summary>
        [TestMethod]
        [DataRow("255;255;255", "255;255;0", 1.0738392309265699)]
        [DataRow("255;255;255", "0;0;255", 8.5924713584288046)]
        [Description("Querying")]
        public void GetContrast(string specifierFg, string specifierBg, double expected)
        {
            Color fg = specifierFg;
            Color bg = specifierBg;
            var actual = TransformationTools.GetContrast(fg, bg);
            actual.ShouldBe(expected);
        }
    }
}
