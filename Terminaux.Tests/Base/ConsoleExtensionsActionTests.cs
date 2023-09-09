
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

using Terminaux.Base;

namespace Terminaux.Tests.Base
{

    [TestFixture]
    public class ConsoleExtensionsActionTests
    {

        /// <summary>
        /// Tests formatting the string
        /// </summary>
        [TestCase("Hello, {0}!", "Alex", ExpectedResult = "Hello, Alex!")]
        [TestCase("We have 0x{0:X2} faults!", 15, ExpectedResult = "We have 0x0F faults!")]
        [TestCase("Destroy {0 ships!", 3, ExpectedResult = "Destroy {0 ships!")]
        [Description("Action")]
        public string TestFormatString(string Expression, params object[] Vars) =>
            ConsoleExtensions.FormatString(Expression, Vars);

    }
}
