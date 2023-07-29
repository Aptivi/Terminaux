
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

using Shouldly;
using Terminaux.Sequences.Builder;

namespace Terminaux.Base.Tests
{

    [TestFixture]
    public class ConsoleQueryingTests
    {

        /// <summary>
        /// Tests getting how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestPercentRepeatTargeted() =>
            ConsoleExtensions.PercentRepeatTargeted(25, 200, 100).ShouldBe(12);

        /// <summary>
        /// Tests filtering the VT sequences that matches the regex
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestFilterVTSequences()
        {
            char BellChar = VtSequenceBasicChars.BellChar;
            char EscapeChar = VtSequenceBasicChars.EscapeChar;
            ConsoleExtensions.FilterVTSequences($"Hello!{EscapeChar}[38;5;43m").ShouldBe("Hello!");
            ConsoleExtensions.FilterVTSequences($"{EscapeChar}]0;This is the title{BellChar}Hello!").ShouldBe("Hello!");
        }

    }
}
