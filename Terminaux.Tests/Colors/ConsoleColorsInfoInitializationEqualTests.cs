
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

namespace Terminaux.Colors.Tests
{
    [TestFixture]
    public partial class ConsoleColorsInfoInitializationEqualTests
    {
        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the equality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyEqualUsingSymbols()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for equality
            (ConsoleColorsInfoInstance == SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the inequality using the operator
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyDontEqualUsingSymbols()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey15);

            // Check for equality
            (ConsoleColorsInfoInstance != SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the equality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyEqualUsingEquals()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for equality
            ConsoleColorsInfoInstance.Equals(SecondConsoleColorsInfoInstance).ShouldBeTrue();
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo and comparing the inequality using the Equals function
        /// </summary>
        [Test]
        [Description("Equality")]
        public void TestInitializeConsoleColorsInfoWhetherTheyDontEqualUsingEquals()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);
            var SecondConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey15);

            // Check for equality
            ConsoleColorsInfoInstance.Equals(SecondConsoleColorsInfoInstance).ShouldBeFalse();
        }
    }
}
