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

using Colorimetry.Data;
using System;
using Terminaux.Base;
using Terminaux.Inputs;
using Terminaux.Base.Structures;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Graphics
{
    internal class RenderLinesSmooth : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Graphics;

        public void RunFixture()
        {
            bool bail = false;
            var rng = new Random();
            while (!bail)
            {
                Coordinate startPoint1 = new(rng.Next(ConsoleWrapper.WindowWidth / 2), rng.Next(ConsoleWrapper.WindowHeight));
                Coordinate endPoint1 = new(rng.Next(ConsoleWrapper.WindowWidth / 2), rng.Next(ConsoleWrapper.WindowHeight));
                Coordinate startPoint2 = new(rng.Next(ConsoleWrapper.WindowWidth / 2), rng.Next(ConsoleWrapper.WindowHeight));
                Coordinate endPoint2 = new(rng.Next(ConsoleWrapper.WindowWidth / 2), rng.Next(ConsoleWrapper.WindowHeight));
                var line1 = new Line()
                {
                    StartPos = startPoint1,
                    EndPos = endPoint1,
                    Color = ConsoleColors.Green,
                    AntiAlias = true
                };
                var line2 = new Line()
                {
                    StartPos = startPoint2,
                    EndPos = endPoint2,
                    Color = ConsoleColors.Green,
                    AntiAlias = true
                };
                var lines = new Container();
                lines.AddRenderable("Line 1", line1);
                lines.AddRenderable("Line 2", line2);
                ContainerTools.WriteContainer(lines);
                var cki = Input.ReadKey();
                if (cki.Key == ConsoleKey.Escape)
                    bail = true;
            }
        }
    }
}
