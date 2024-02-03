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

using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class BorderText : IFixture
    {
        public string FixtureID => "BorderText";
        public void RunFixture()
        {
            ColorTools.LoadBack();
            BorderTextColor.WriteBorder("Hello world!", 2, 1, 6, 2, new Color(ConsoleColors.Green));
            BorderTextColor.WriteBorder("Hello world!", ConsoleWrapper.WindowWidth - 10, 1, 6, 2, new Color(ConsoleColors.Black), new Color(ConsoleColors.Yellow), new Color(ConsoleColors.Black));
            BorderTextColor.WriteBorder("Middle", "Hello world!", (ConsoleWrapper.WindowWidth / 2) - 6, 1, 12, 1, new Color(ConsoleColors.Red));
            BorderTextColor.WriteBorder("Center", "Hello world!");
            TextWriterWhereColor.WriteWhere("If you can see these, it's a success!", 0, ConsoleWrapper.WindowHeight - 1);
        }
    }
}
