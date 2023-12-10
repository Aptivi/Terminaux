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

using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class TestListWriterInt : IFixture
    {
        public string FixtureID => "TestListWriterInt";
        public void RunFixture()
        {
            var NormalIntegerList = new List<int>() { 1, 2, 3 };
            var ArrayIntegerList = new List<int[]>() { { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } } };
            TextWriterColor.Write("Normal integer list:");
            ListWriterColor.WriteList(NormalIntegerList, ConsoleColors.Gray, ConsoleColors.DarkGray, false);
            TextWriterColor.Write("Array integer list:");
            ListWriterColor.WriteList(ArrayIntegerList, ConsoleColors.Gray, ConsoleColors.DarkGray, false);
        }
    }
}
