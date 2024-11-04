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
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestDictWriterCharWithStringifiers : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var NormalCharDict = new Dictionary<string, char>() { { "One", '1' }, { "Two", '2' }, { "Three", '3' } };
            var ArrayCharDict = new Dictionary<string, char[]>() { { "One", new char[] { '1', '2', '3' } }, { "Two", new char[] { '1', '2', '3' } }, { "Three", new char[] { '1', '2', '3' } } };
            TextWriterColor.Write("Normal char dictionary:");
            ListWriterColor.WriteList(NormalCharDict, ConsoleColors.Silver, ConsoleColors.Grey, keyStringifier: null, (character) => $"{character} [{(int)character}]");
            TextWriterColor.Write("Array char dictionary:");
            ListWriterColor.WriteList(ArrayCharDict, ConsoleColors.Silver, ConsoleColors.Grey, keyStringifier: null, null, (character) => $"{(char)character} [{(int)(char)character}]");
        }
    }
}
