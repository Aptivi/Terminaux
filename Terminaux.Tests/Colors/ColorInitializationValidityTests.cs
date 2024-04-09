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
using System;
using System.Diagnostics;
using Terminaux.Colors;
using Terminaux.Colors.Models.Parsing;

namespace Terminaux.Tests.Colors
{
    [TestClass]
    public partial class ColorInitializationValidityTests
    {
        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [Description("Validity")]
        public void TestTryParseColorFromHex(string TargetHex, bool expected)
        {
            bool result = false;
            try
            {
                Debug.WriteLine($"Trying {TargetHex}...");
                var col = new Color(TargetHex);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, true)]
        [DataRow(-26, false)]
        [Description("Validity")]
        public void TestTryParseColorFromColorNum(int TargetColorNum, bool expected)
        {
            bool result = false;
            try
            {
                Debug.WriteLine($"Trying colornum {TargetColorNum}...");
                var col = new Color(TargetColorNum);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
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
        [Description("Validity")]
        public void TestTryParseColorFromRGB(int R, int G, int B, bool expected)
        {
            bool result = false;
            try
            {
                Debug.WriteLine($"Trying rgb {R}, {G}, {B}...");
                var col = new Color(R, G, B);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB color specifier
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
        [Description("Validity")]
        public void TestTryParseColorFromSpecifier(string specifier, bool expected)
        {
            bool result = false;
            try
            {
                Debug.WriteLine($"Trying rgb specifier {specifier}...");
                var col = new Color(specifier);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to check the color specifier syntax validity from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", true)]
        [DataRow("#FFF", true)]
        [DataRow("#GGG", true)]
        [Description("Validity")]
        public void TestIsSpecifierValidFromHex(string TargetHex, bool expected)
        {
            bool result = ParsingTools.IsSpecifierValidRgbHash(TargetHex);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to check the color specifier syntax validity from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, false)]
        [DataRow(-26, false)]
        [Description("Validity")]
        public void TestIsSpecifierValidFromColorNum(int TargetColorNum, bool expected)
        {
            bool result = ParsingTools.IsSpecifierConsoleColors($"{TargetColorNum}");
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to check the color specifier syntax validity from RGB
        /// </summary>
        [TestMethod]
        [DataRow(4, 4, 4, true)]
        [DataRow(400, 4, 4, true)]
        [DataRow(4, 400, 4, true)]
        [DataRow(4, 4, 400, true)]
        [DataRow(4, 400, 400, true)]
        [DataRow(400, 4, 400, true)]
        [DataRow(400, 400, 4, true)]
        [DataRow(400, 400, 400, true)]
        [DataRow(-4, 4, 4, true)]
        [DataRow(4, -4, 4, true)]
        [DataRow(4, 4, -4, true)]
        [DataRow(4, -4, -4, true)]
        [DataRow(-4, 4, -4, true)]
        [DataRow(-4, -4, 4, true)]
        [DataRow(-4, -4, -4, true)]
        [Description("Validity")]
        public void TestIsSpecifierValidFromRGB(int R, int G, int B, bool expected)
        {
            bool result = ParsingTools.IsSpecifierValid($"{R};{G};{B}");
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to check the color specifier syntax validity from RGB color specifier
        /// </summary>
        [TestMethod]
        [DataRow("4;4;4", true)]
        [DataRow("400;4;4", true)]
        [DataRow("4;400;4", true)]
        [DataRow("4;4;400", true)]
        [DataRow("4;400;400", true)]
        [DataRow("400;4;400", true)]
        [DataRow("400;400;4", true)]
        [DataRow("400;400;400", true)]
        [DataRow("-4;4;4", true)]
        [DataRow("4;-4;4", true)]
        [DataRow("4;4;-4", true)]
        [DataRow("4;-4;-4", true)]
        [DataRow("-4;4;-4", true)]
        [DataRow("-4;-4;4", true)]
        [DataRow("-4;-4;-4", true)]
        [Description("Validity")]
        public void TestIsSpecifierValidFromSpecifier(string specifier, bool expected)
        {
            bool result = ParsingTools.IsSpecifierValid(specifier);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [DataRow("#FFF", true)]
        [DataRow("#GGG", false)]
        [Description("Validity")]
        public void TestIsSpecifierAndValueValidFromHex(string TargetHex, bool expected)
        {
            bool result = ParsingTools.IsSpecifierAndValueValid(TargetHex);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, false)]
        [DataRow(-26, false)]
        [Description("Validity")]
        public void TestIsSpecifierAndValueValidFromColorNum(int TargetColorNum, bool expected)
        {
            bool result = ParsingTools.IsSpecifierAndValueValid($"{TargetColorNum}");
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
        [Description("Validity")]
        public void TestIsSpecifierAndValueValidFromRGB(int R, int G, int B, bool expected)
        {
            bool result = ParsingTools.IsSpecifierAndValueValid($"{R};{G};{B}");
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB color specifier
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
        [Description("Validity")]
        public void TestIsSpecifierAndValueValidFromSpecifier(string specifier, bool expected)
        {
            bool result = ParsingTools.IsSpecifierAndValueValid(specifier);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [DataRow("#FFF", true)]
        [DataRow("#GGG", false)]
        [Description("Validity")]
        public void TestParseSpecifierFromHex(string TargetHex, bool expected)
        {
            bool result = false;
            try
            {
                ParsingTools.ParseSpecifier(TargetHex);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, false)]
        [DataRow(-26, false)]
        [Description("Validity")]
        public void TestParseSpecifierFromColorNum(int TargetColorNum, bool expected)
        {
            bool result = false;
            try
            {
                ParsingTools.ParseSpecifier($"{TargetColorNum}");
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
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
        [Description("Validity")]
        public void TestParseSpecifierFromRGB(int R, int G, int B, bool expected)
        {
            bool result = false;
            try
            {
                ParsingTools.ParseSpecifier($"{R};{G};{B}");
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB color specifier
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
        [Description("Validity")]
        public void TestParseSpecifierFromSpecifier(string specifier, bool expected)
        {
            bool result = false;
            try
            {
                ParsingTools.ParseSpecifier(specifier);
                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestMethod]
        [DataRow("#0F0F0F", true)]
        [DataRow("#0G0G0G", false)]
        [DataRow("#FFF", true)]
        [DataRow("#GGG", false)]
        [Description("Validity")]
        public void TestTryParseSpecifierFromHex(string TargetHex, bool expected)
        {
            bool result = ParsingTools.TryParseSpecifier(TargetHex, out _);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestMethod]
        [DataRow(26, true)]
        [DataRow(260, false)]
        [DataRow(-26, false)]
        [Description("Validity")]
        public void TestTryParseSpecifierFromColorNum(int TargetColorNum, bool expected)
        {
            bool result = ParsingTools.TryParseSpecifier($"{TargetColorNum}", out _);
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
        [Description("Validity")]
        public void TestTryParseSpecifierFromRGB(int R, int G, int B, bool expected)
        {
            bool result = ParsingTools.TryParseSpecifier($"{R};{G};{B}", out _);
            result.ShouldBe(expected);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB color specifier
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
        [Description("Validity")]
        public void TestTryParseSpecifierFromSpecifier(string specifier, bool expected)
        {
            bool result = ParsingTools.TryParseSpecifier(specifier, out _);
            result.ShouldBe(expected);
        }
    }
}
