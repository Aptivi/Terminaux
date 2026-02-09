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
    internal class TestListWriterCharWithStringifiers : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var NormalCharList = new List<char>() { '1', '2', '3' };
            var ArrayCharList = new List<char[]>() { { new char[] { '1', '2', '3' } }, { new char[] { '1', '2', '3' } }, { new char[] { '1', '2', '3' } } };
            var normalChars = new Listing()
            {
                Objects = NormalCharList,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
                Stringifier = (character) => $"{(char)character} [{(int)(char)character}]",
            };
            var arrayChars = new Listing()
            {
                Objects = ArrayCharList,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
                RecursiveStringifier = (character) => $"{(char)character} [{(int)(char)character}]"
            };
            TextWriterColor.Write("Normal char list:\n{0}", normalChars.Render());
            TextWriterColor.Write("Array char list:\n{0}", arrayChars.Render());
        }
    }
}
