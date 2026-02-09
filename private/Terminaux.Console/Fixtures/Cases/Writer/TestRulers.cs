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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestRulers : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var ruler1 = new Ruler()
            {
                ForegroundColor = ConsoleColors.White
            };
            var ruler2 = new Ruler()
            {
                ForegroundColor = ConsoleColors.Red,
                Text = "Ruler at left alignment"
            };
            var ruler3 = new Ruler()
            {
                ForegroundColor = ConsoleColors.Lime,
                Text = "Ruler at center alignment",
                Alignment = TextAlignment.Middle
            };
            var ruler4 = new Ruler()
            {
                ForegroundColor = ConsoleColors.Blue,
                Text = "Ruler at right alignment",
                Alignment = TextAlignment.Right
            };

            // Render the rulers
            TextWriterRaw.WritePlain(ruler1.Render());
            TextWriterRaw.WritePlain(ruler2.Render());
            TextWriterRaw.WritePlain(ruler3.Render());
            TextWriterRaw.WritePlain(ruler4.Render());
        }
    }
}
