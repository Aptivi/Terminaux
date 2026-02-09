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

using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Writer.ConsoleWriters;
using Colorimetry.Data;

namespace Terminaux.Console.Fixtures.Cases.Graphics
{
    internal class RenderEllipsis : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Graphics;

        public void RunFixture()
        {
            var ellipsis = new Ellipsis(20, 20, 4, 2, true, ConsoleColors.Red);
            var ellipsis2 = new Ellipsis(10, 20, 46, 2, false, ConsoleColors.Aqua);
            var ellipsis3 = new Ellipsis(20, 10, 68, 2, true, ConsoleColors.Lime);
            var ellipsis4 = new Ellipsis(10, 10, 68, 12, false, ConsoleColors.Blue);
            var ellipsis5 = new Ellipsis(10, 10, 90, 12, true, ConsoleColors.Blue3);
            TextWriterRaw.WriteRaw(ellipsis.Render());
            TextWriterRaw.WriteRaw(ellipsis2.Render());
            TextWriterRaw.WriteRaw(ellipsis3.Render());
            TextWriterRaw.WriteRaw(ellipsis4.Render());
            TextWriterRaw.WriteRaw(ellipsis5.Render());
        }
    }
}
