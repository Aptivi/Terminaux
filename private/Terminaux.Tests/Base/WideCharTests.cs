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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Terminaux.Base.Extensions;
using Textify.General.Structures;

namespace Terminaux.Tests.Base
{
    [TestClass]
    public class WideCharTests
    {
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
