﻿//
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
    internal class RenderRectangle : IFixture
    {
        public void RunFixture()
        {
            var rect = new Rectangle(15, 5, 4, 2, true, ConsoleColors.Red);
            var rect2 = new Rectangle(15, 5, 21, 2, false, ConsoleColors.Aqua);
            TextWriterRaw.WriteRaw(rect.Render());
            TextWriterRaw.WriteRaw(rect2.Render());
        }
    }
}
