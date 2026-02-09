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
    internal class TestListWriterInt : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var NormalIntegerList = new List<int>() { 1, 2, 3 };
            var ArrayIntegerList = new List<int[]>() { { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } } };
            var normalIntegers = new Listing()
            {
                Objects = NormalIntegerList,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
            };
            var arrayIntegers = new Listing()
            {
                Objects = ArrayIntegerList,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
            };
            TextWriterColor.Write("Normal integer list:\n{0}", normalIntegers.Render());
            TextWriterColor.Write("Array integer list:\n{0}", arrayIntegers.Render());
        }
    }
}
