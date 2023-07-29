
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

using System.Collections.Generic;
using Terminaux.Colors;
using Terminaux.ConsoleDemo.Fixtures;
using Terminaux.Writer.ConsoleWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestDictWriterChar : IFixture
    {
        public string FixtureID => "TestDictWriterChar";
        public void RunFixture()
        {
            var NormalCharDict = new Dictionary<string, char>() { { "One", '1' }, { "Two", '2' }, { "Three", '3' } };
            var ArrayCharDict = new Dictionary<string, char[]>() { { "One", new char[] { '1', '2', '3' } }, { "Two", new char[] { '1', '2', '3' } }, { "Three", new char[] { '1', '2', '3' } } };
            TextWriterColor.Write("Normal char dictionary:");
            ListWriterColor.WriteList(NormalCharDict, ConsoleColors.Gray, ConsoleColors.DarkGray, false);
            TextWriterColor.Write("Array char dictionary:");
            ListWriterColor.WriteList(ArrayCharDict, ConsoleColors.Gray, ConsoleColors.DarkGray, false);
        }
    }
}
