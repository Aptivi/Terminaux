
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
using Terminaux.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class PrintWithNulls : IFixture
    {
        public string FixtureID => "PrintWithNulls";
        public void RunFixture()
        {
            TextWriterColor.WriteColor("Hello world!\nHow's your day going? \0Should be after this:\0\0\0", false, new Color(ConsoleColors.Green));
            TextWriterColor.WriteColor(" [{0}, {1}] ", true, new Color(ConsoleColors.Gray), vars: new object[] { ConsoleWrappers.ActionCursorLeft(), ConsoleWrappers.ActionCursorTop() });
        }
    }
}
