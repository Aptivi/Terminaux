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

using Terminaux.Base;
using Terminaux.Base.Extensions;
using Colorimetry.Data;
using Terminaux.Inputs;

namespace Terminaux.Console.Fixtures.Cases.Colors
{
    internal class ColorBackTest : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Color;

        public void RunFixture()
        {
            ConsoleColoring.AllowBackground = true;
            ConsoleColoring.SetConsoleColor(ConsoleColors.Green, true);
            ConsoleWrapper.ClearLoadBack();
            Input.ReadKey();
            ConsoleColoring.SetConsoleColor(ConsoleColors.Yellow, true);
            ConsoleWrapper.Clear();
            Input.ReadKey();
            ConsoleColoring.SetConsoleColor(ConsoleColors.Red, true);
            ConsoleWrapper.ClearLoadBack();
            Input.ReadKey();
            ConsoleColoring.AllowBackground = false;
            ConsoleWrapper.ClearLoadBack();
        }
    }
}
