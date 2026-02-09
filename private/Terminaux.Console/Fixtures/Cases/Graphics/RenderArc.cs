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
using Terminaux.Writer.CyclicWriters.Renderer;

namespace Terminaux.Console.Fixtures.Cases.Graphics
{
    internal class RenderArc : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Graphics;

        public void RunFixture()
        {
            var container = new Container();
            var arc = new Arc(20, 4, 2, ConsoleColors.Red)
            {
                InnerRadius = 0,
                OuterRadius = 9,
                AngleStart = 170,
                AngleEnd = 120,
            };
            var arc2 = new Arc(20, 4, 2, ConsoleColors.Aqua)
            {
                InnerRadius = 0,
                OuterRadius = 9,
                AngleStart = 120,
                AngleEnd = 270,
            };
            var arc3 = new Arc(20, 46, 2, ConsoleColors.Yellow)
            {
                InnerRadius = 6,
                OuterRadius = 9,
                AngleStart = 360,
                AngleEnd = 150,
            };
            var arc4 = new Arc(20, 46, 2, ConsoleColors.Fuchsia)
            {
                InnerRadius = 6,
                OuterRadius = 9,
                AngleStart = 150,
                AngleEnd = 360,
            };
            var arc5 = new Arc(20, 46, 2, ConsoleColors.Olive)
            {
                InnerRadius = 6,
                OuterRadius = 9,
                AngleStart = 150,
                AngleEnd = 210,
            };
            container.AddRenderable("Arc 1", arc);
            container.AddRenderable("Arc 2", arc2);
            container.AddRenderable("Arc 3", arc3);
            container.AddRenderable("Arc 4", arc4);
            container.AddRenderable("Arc 5", arc5);
            TextWriterRaw.WriteRaw(ContainerTools.RenderContainer(container));
        }
    }
}
