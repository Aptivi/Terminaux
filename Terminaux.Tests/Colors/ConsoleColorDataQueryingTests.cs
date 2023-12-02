
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
using System;
using Terminaux.Colors.Data;

namespace Terminaux.Tests.Colors
{
    [TestFixture]
    public partial class ConsoleColorDataQueryingTests
    {
        /// <summary>
        /// Tests querying 255-color data from JSON (parses only needed data)
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestQueryColorDataFromJson()
        {
            for (int ColorIndex = 0; ColorIndex <= 255; ColorIndex++)
            {
                var ColorData = ConsoleColorData.GetColorData()[ColorIndex];
                ColorData.ColorId.ShouldBe(ColorIndex);
            }
        }
    }
}
