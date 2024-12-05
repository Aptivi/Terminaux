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

using Terminaux.Colors.Data;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Input
{
    internal class TestInputInfoBoxColoredInput : IFixture
    {
        public void RunFixture()
        {
            string answer = InfoBoxInputColor.WriteInfoBoxInputColorBack(nameof(TestInputInfoBoxColoredInput), "Write your text.", ConsoleColors.Yellow, ConsoleColors.DarkBlue);
            TextWriterWhereColor.WriteWhere(answer, 0, 0);
        }
    }
}
