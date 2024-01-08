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

using System.Diagnostics;
using Terminaux.Colors;

namespace Terminaux.Tests.Colors
{

    [TestFixture]
    public class ColorQueryingTests
    {

        /// <summary>
        /// Tests trying to parse the color from hex
        /// </summary>
        [TestCase("#0F0F0F", ExpectedResult = true)]
        [TestCase("#0G0G0G", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromHex(string TargetHex)
        {
            Debug.WriteLine($"Trying {TargetHex}...");
            return ColorTools.TryParseColor(TargetHex);
        }

        /// <summary>
        /// Tests trying to parse the color from color numbers
        /// </summary>
        [TestCase(26, ExpectedResult = true)]
        [TestCase(260, ExpectedResult = false)]
        [TestCase(-26, ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromColorNum(int TargetColorNum)
        {
            Debug.WriteLine($"Trying colornum {TargetColorNum}...");
            return ColorTools.TryParseColor(TargetColorNum);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestCase(4, 4, 4, ExpectedResult = true)]
        [TestCase(400, 4, 4, ExpectedResult = false)]
        [TestCase(4, 400, 4, ExpectedResult = false)]
        [TestCase(4, 4, 400, ExpectedResult = false)]
        [TestCase(4, 400, 400, ExpectedResult = false)]
        [TestCase(400, 4, 400, ExpectedResult = false)]
        [TestCase(400, 400, 4, ExpectedResult = false)]
        [TestCase(400, 400, 400, ExpectedResult = false)]
        [TestCase(-4, 4, 4, ExpectedResult = false)]
        [TestCase(4, -4, 4, ExpectedResult = false)]
        [TestCase(4, 4, -4, ExpectedResult = false)]
        [TestCase(4, -4, -4, ExpectedResult = false)]
        [TestCase(-4, 4, -4, ExpectedResult = false)]
        [TestCase(-4, -4, 4, ExpectedResult = false)]
        [TestCase(-4, -4, -4, ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromRGB(int R, int G, int B)
        {
            Debug.WriteLine($"Trying rgb {R}, {G}, {B}...");
            return ColorTools.TryParseColor(R, G, B);
        }

        /// <summary>
        /// Tests trying to parse the color from RGB
        /// </summary>
        [TestCase("4;4;4", ExpectedResult = true)]
        [TestCase("400;4;4", ExpectedResult = false)]
        [TestCase("4;400;4", ExpectedResult = false)]
        [TestCase("4;4;400", ExpectedResult = false)]
        [TestCase("4;400;400", ExpectedResult = false)]
        [TestCase("400;4;400", ExpectedResult = false)]
        [TestCase("400;400;4", ExpectedResult = false)]
        [TestCase("400;400;400", ExpectedResult = false)]
        [TestCase("-4;4;4", ExpectedResult = false)]
        [TestCase("4;-4;4", ExpectedResult = false)]
        [TestCase("4;4;-4", ExpectedResult = false)]
        [TestCase("4;-4;-4", ExpectedResult = false)]
        [TestCase("-4;4;-4", ExpectedResult = false)]
        [TestCase("-4;-4;4", ExpectedResult = false)]
        [TestCase("-4;-4;-4", ExpectedResult = false)]
        [Description("Querying")]
        public bool TestTryParseColorFromSpecifier(string specifier)
        {
            Debug.WriteLine($"Trying rgb specifier {specifier}...");
            return ColorTools.TryParseColor(specifier);
        }

        /// <summary>
        /// Tests trying to get the contrast color (NTSC)
        /// </summary>
        [TestCase("#EF4444", ExpectedResult = "#FFFFFF")]
        [TestCase("#FAA31B", ExpectedResult = "#000000")]
        [TestCase("#FFF000", ExpectedResult = "#000000")]
        [TestCase("#82C341", ExpectedResult = "#000000")]
        [TestCase("#009F75", ExpectedResult = "#FFFFFF")]
        [TestCase("#88C6ED", ExpectedResult = "#000000")]
        [TestCase("#394BA0", ExpectedResult = "#FFFFFF")]
        [TestCase("#D54799", ExpectedResult = "#FFFFFF")]
        [TestCase("#CCCCCC", ExpectedResult = "#000000")]
        [TestCase("#999999", ExpectedResult = "#000000")]
        [TestCase("#666666", ExpectedResult = "#FFFFFF")]
        [TestCase("#333333", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF0000", ExpectedResult = "#FFFFFF")]
        [TestCase("#00FF00", ExpectedResult = "#000000")]
        [TestCase("#0000FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFFF00", ExpectedResult = "#000000")]
        [TestCase("#FF00FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#00FFFF", ExpectedResult = "#000000")]
        [TestCase("#FFCC00", ExpectedResult = "#000000")]
        [TestCase("#CCFF00", ExpectedResult = "#000000")]
        [TestCase("#00CCFF", ExpectedResult = "#000000")]
        [TestCase("#FF6600", ExpectedResult = "#000000")]
        [TestCase("#FF0066", ExpectedResult = "#FFFFFF")]
        [TestCase("#006666", ExpectedResult = "#FFFFFF")]
        [TestCase("#0099CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#666600", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC00CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC6666", ExpectedResult = "#000000")]
        [Description("Querying")]
        public string TestGetContrastColorNtsc(string speciifer) =>
            ColorContrast.GetContrastColorNtsc(speciifer).Hex;

        /// <summary>
        /// Tests trying to get the contrast color (Half)
        /// </summary>
        [TestCase("#EF4444", ExpectedResult = "#000000")]
        [TestCase("#FAA31B", ExpectedResult = "#000000")]
        [TestCase("#FFF000", ExpectedResult = "#000000")]
        [TestCase("#82C341", ExpectedResult = "#000000")]
        [TestCase("#009F75", ExpectedResult = "#FFFFFF")]
        [TestCase("#88C6ED", ExpectedResult = "#000000")]
        [TestCase("#394BA0", ExpectedResult = "#FFFFFF")]
        [TestCase("#D54799", ExpectedResult = "#000000")]
        [TestCase("#CCCCCC", ExpectedResult = "#000000")]
        [TestCase("#999999", ExpectedResult = "#000000")]
        [TestCase("#666666", ExpectedResult = "#FFFFFF")]
        [TestCase("#333333", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF0000", ExpectedResult = "#000000")]
        [TestCase("#00FF00", ExpectedResult = "#FFFFFF")]
        [TestCase("#0000FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFFF00", ExpectedResult = "#000000")]
        [TestCase("#FF00FF", ExpectedResult = "#000000")]
        [TestCase("#00FFFF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFCC00", ExpectedResult = "#000000")]
        [TestCase("#CCFF00", ExpectedResult = "#000000")]
        [TestCase("#00CCFF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF6600", ExpectedResult = "#000000")]
        [TestCase("#FF0066", ExpectedResult = "#000000")]
        [TestCase("#006666", ExpectedResult = "#FFFFFF")]
        [TestCase("#0099CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#666600", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC00CC", ExpectedResult = "#000000")]
        [TestCase("#CC6666", ExpectedResult = "#000000")]
        [Description("Querying")]
        public string TestGetContrastColorHalf(string speciifer) =>
            ColorContrast.GetContrastColorHalf(speciifer).Hex;

        /// <summary>
        /// Tests trying to get the contrast color (NTSC)
        /// </summary>
        [TestCase("#EF4444", ExpectedResult = "#FFFFFF")]
        [TestCase("#FAA31B", ExpectedResult = "#000000")]
        [TestCase("#FFF000", ExpectedResult = "#000000")]
        [TestCase("#82C341", ExpectedResult = "#000000")]
        [TestCase("#009F75", ExpectedResult = "#FFFFFF")]
        [TestCase("#88C6ED", ExpectedResult = "#000000")]
        [TestCase("#394BA0", ExpectedResult = "#FFFFFF")]
        [TestCase("#D54799", ExpectedResult = "#FFFFFF")]
        [TestCase("#CCCCCC", ExpectedResult = "#000000")]
        [TestCase("#999999", ExpectedResult = "#000000")]
        [TestCase("#666666", ExpectedResult = "#FFFFFF")]
        [TestCase("#333333", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF0000", ExpectedResult = "#FFFFFF")]
        [TestCase("#00FF00", ExpectedResult = "#000000")]
        [TestCase("#0000FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFFF00", ExpectedResult = "#000000")]
        [TestCase("#FF00FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#00FFFF", ExpectedResult = "#000000")]
        [TestCase("#FFCC00", ExpectedResult = "#000000")]
        [TestCase("#CCFF00", ExpectedResult = "#000000")]
        [TestCase("#00CCFF", ExpectedResult = "#000000")]
        [TestCase("#FF6600", ExpectedResult = "#000000")]
        [TestCase("#FF0066", ExpectedResult = "#FFFFFF")]
        [TestCase("#006666", ExpectedResult = "#FFFFFF")]
        [TestCase("#0099CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#666600", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC00CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC6666", ExpectedResult = "#000000")]
        [Description("Querying")]
        public string TestGetContrastColorUsingGetGrayNtsc(string speciifer) =>
            ColorTools.GetGray(speciifer, ColorContrastType.Ntsc).Hex;

        /// <summary>
        /// Tests trying to get the contrast color (Half)
        /// </summary>
        [TestCase("#EF4444", ExpectedResult = "#000000")]
        [TestCase("#FAA31B", ExpectedResult = "#000000")]
        [TestCase("#FFF000", ExpectedResult = "#000000")]
        [TestCase("#82C341", ExpectedResult = "#000000")]
        [TestCase("#009F75", ExpectedResult = "#FFFFFF")]
        [TestCase("#88C6ED", ExpectedResult = "#000000")]
        [TestCase("#394BA0", ExpectedResult = "#FFFFFF")]
        [TestCase("#D54799", ExpectedResult = "#000000")]
        [TestCase("#CCCCCC", ExpectedResult = "#000000")]
        [TestCase("#999999", ExpectedResult = "#000000")]
        [TestCase("#666666", ExpectedResult = "#FFFFFF")]
        [TestCase("#333333", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF0000", ExpectedResult = "#000000")]
        [TestCase("#00FF00", ExpectedResult = "#FFFFFF")]
        [TestCase("#0000FF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFFF00", ExpectedResult = "#000000")]
        [TestCase("#FF00FF", ExpectedResult = "#000000")]
        [TestCase("#00FFFF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FFCC00", ExpectedResult = "#000000")]
        [TestCase("#CCFF00", ExpectedResult = "#000000")]
        [TestCase("#00CCFF", ExpectedResult = "#FFFFFF")]
        [TestCase("#FF6600", ExpectedResult = "#000000")]
        [TestCase("#FF0066", ExpectedResult = "#000000")]
        [TestCase("#006666", ExpectedResult = "#FFFFFF")]
        [TestCase("#0099CC", ExpectedResult = "#FFFFFF")]
        [TestCase("#666600", ExpectedResult = "#FFFFFF")]
        [TestCase("#CC00CC", ExpectedResult = "#000000")]
        [TestCase("#CC6666", ExpectedResult = "#000000")]
        [Description("Querying")]
        public string TestGetContrastColorUsingGetGrayHalf(string speciifer) =>
            ColorTools.GetGray(speciifer, ColorContrastType.Half).Hex;

        /// <summary>
        /// Tests trying to get the contrast color (Light)
        /// </summary>
        [TestCase("#EF4444", ExpectedResult = "#000000")]
        [TestCase("#FAA31B", ExpectedResult = "#000000")]
        [TestCase("#FFF000", ExpectedResult = "#000000")]
        [TestCase("#82C341", ExpectedResult = "#000000")]
        [TestCase("#009F75", ExpectedResult = "#000000")]
        [TestCase("#88C6ED", ExpectedResult = "#000000")]
        [TestCase("#394BA0", ExpectedResult = "#000000")]
        [TestCase("#D54799", ExpectedResult = "#000000")]
        [TestCase("#CCCCCC", ExpectedResult = "#000000")]
        [TestCase("#999999", ExpectedResult = "#000000")]
        [TestCase("#666666", ExpectedResult = "#000000")]
        [TestCase("#333333", ExpectedResult = "#000000")]
        [TestCase("#FF0000", ExpectedResult = "#000000")]
        [TestCase("#00FF00", ExpectedResult = "#000000")]
        [TestCase("#0000FF", ExpectedResult = "#000000")]
        [TestCase("#FFFF00", ExpectedResult = "#000000")]
        [TestCase("#FF00FF", ExpectedResult = "#000000")]
        [TestCase("#00FFFF", ExpectedResult = "#000000")]
        [TestCase("#FFCC00", ExpectedResult = "#000000")]
        [TestCase("#CCFF00", ExpectedResult = "#000000")]
        [TestCase("#00CCFF", ExpectedResult = "#000000")]
        [TestCase("#FF6600", ExpectedResult = "#000000")]
        [TestCase("#FF0066", ExpectedResult = "#000000")]
        [TestCase("#006666", ExpectedResult = "#000000")]
        [TestCase("#0099CC", ExpectedResult = "#000000")]
        [TestCase("#666600", ExpectedResult = "#000000")]
        [TestCase("#003045", ExpectedResult = "#C0C0C0")]
        [TestCase("#CC00CC", ExpectedResult = "#000000")]
        [TestCase("#CC6666", ExpectedResult = "#000000")]
        [Description("Querying")]
        public string TestGetContrastColorUsingGetGrayLight(string speciifer) =>
            ColorTools.GetGray(speciifer, ColorContrastType.Light).Hex;

    }
}
