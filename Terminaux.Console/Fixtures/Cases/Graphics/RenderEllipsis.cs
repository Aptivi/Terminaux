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

using Terminaux.Writer.CyclicWriters.Shapes;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Data;

namespace Terminaux.Console.Fixtures.Cases.Graphics
{
    internal class RenderEllipsis : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Graphics;

        public void RunFixture()
        {
            var ellipsis = new Ellipsis(20, 20, 4, 2, true, ConsoleColors.Red);
            var ellipsis2 = new Ellipsis(10, 20, 46, 2, false, ConsoleColors.Aqua);
            TextWriterRaw.WriteRaw(ellipsis.Render());
            TextWriterRaw.WriteRaw(ellipsis2.Render());
        }
    }
}
