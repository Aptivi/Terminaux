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

using Shouldly;
using Terminaux.Colors;

namespace Terminaux.Tests.Colors
{
    [TestFixture]
    public partial class ConsoleColorsInfoInitializationTests
    {
        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo from a bright color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeConsoleColorsInfoInstanceBright()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey85);

            // Check for property correctness
            ConsoleColorsInfoInstance.ColorID.ShouldBe(253);
            ConsoleColorsInfoInstance.R.ShouldBe(218);
            ConsoleColorsInfoInstance.G.ShouldBe(218);
            ConsoleColorsInfoInstance.B.ShouldBe(218);
        }

        /// <summary>
        /// Tests initializing an instance of ConsoleColorsInfo from a dark color
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestInitializeConsoleColorsInfoInstanceDark()
        {
            // Create instance
            var ConsoleColorsInfoInstance = new ConsoleColorsInfo(ConsoleColors.Grey11);

            // Check for property correctness
            ConsoleColorsInfoInstance.ColorID.ShouldBe(234);
            ConsoleColorsInfoInstance.R.ShouldBe(28);
            ConsoleColorsInfoInstance.G.ShouldBe(28);
            ConsoleColorsInfoInstance.B.ShouldBe(28);
        }
    }
}
