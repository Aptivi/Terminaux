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

using System.Collections.Generic;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestDictWriterInt : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var NormalIntegerDict = new Dictionary<string, int>() { { "One", 1 }, { "Two", 2 }, { "Three", 3 } };
            var ArrayIntegerDict = new Dictionary<string, int[]>() { { "One", new int[] { 1, 2, 3 } }, { "Two", new int[] { 1, 2, 3 } }, { "Three", new int[] { 1, 2, 3 } } };
            var normalIntegers = new Listing()
            {
                Objects = NormalIntegerDict,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
            };
            var arrayIntegers = new Listing()
            {
                Objects = ArrayIntegerDict,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
                ValueStringifier = (arr) => string.Join(", ", (int[])arr)
            };
            TextWriterColor.Write("Normal integer dictionary:\n{0}", normalIntegers.Render());
            TextWriterColor.Write("Array integer dictionary:\n{0}", arrayIntegers.Render());
        }
    }
}
