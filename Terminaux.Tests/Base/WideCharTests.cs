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
using Terminaux.Base.Structures;

namespace Terminaux.Tests.Base
{

    [TestClass]
    public class WideCharTests
    {

        /// <summary>
        /// Tests getting a WideChar from the string and converting it back to the string
        /// </summary>
        [TestMethod]
        [DataRow("\0")]
        [DataRow("A")]
        [DataRow("\u200b")]
        [DataRow("\U000F200b")]
        [DataRow("😀")]
        [DataRow("🩷")]
        [Description("Querying")]
        public void TestWideCharString(string representation)
        {
            WideChar wideChar = (WideChar)representation;
            string str = wideChar.ToString();
            str.ShouldBe(representation);
        }

        /// <summary>
        /// Tests getting a WideChar from the string, converting this result to WideChar, and comparing between them
        /// </summary>
        [TestMethod]
        [DataRow("\0")]
        [DataRow("A")]
        [DataRow("\u200b")]
        [DataRow("\U000F200b")]
        [DataRow("😀")]
        [DataRow("🩷")]
        [Description("Querying")]
        public void TestWideCharComparison(string representation)
        {
            WideChar wideChar = (WideChar)representation;
            string str = wideChar.ToString();
            WideChar wideChar2 = (WideChar)str;
            wideChar2.ShouldBe(wideChar);
        }

        /// <summary>
        /// Tests getting a WideChar from the string, getting its integral code, and parsing it
        /// </summary>
        [TestMethod]
        [DataRow("\0")]
        [DataRow("A")]
        [DataRow("\u200b")]
        [DataRow("\U000F200b")]
        [DataRow("😀")]
        [DataRow("🩷")]
        [Description("Querying")]
        public void TestWideCharIntCode(string representation)
        {
            WideChar wideChar = (WideChar)representation;
            long code = wideChar;
            WideChar wideChar2 = (WideChar)code;
            wideChar2.ShouldBe(wideChar);
        }

        /// <summary>
        /// Tests getting a WideChar from the string, getting its integral code, and parsing it
        /// </summary>
        [TestMethod]
        [DataRow("\0", 1)]
        [DataRow("A", 1)]
        [DataRow("\u200b", 1)]
        [DataRow("\U000F200b", 2)]
        [DataRow("😀", 2)]
        [DataRow("🩷", 2)]
        [Description("Querying")]
        public void TestWideCharLength(string representation, int expected)
        {
            WideChar wideChar = (WideChar)representation;
            int length = wideChar.GetLength();
            length.ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting a WideChar from the string, getting its integral code, and parsing it
        /// </summary>
        [TestMethod]
        [DataRow("\0", 0)]
        [DataRow("A", 1)]
        [DataRow("\u200b", 0)]
        [DataRow("\U000F200b", 1)]
        [DataRow("😀", 2)]
        [DataRow("🩷", 2)]
        [Description("Querying")]
        public void TestWideCharWidth(string representation, int expected)
        {
            WideChar wideChar = (WideChar)representation;
            int width = wideChar.GetWidth();
            width.ShouldBe(expected);
        }

    }
}
