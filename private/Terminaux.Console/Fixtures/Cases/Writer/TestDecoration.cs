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
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestDecoration : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var decoration1 = new Decoration()
            {
                ForegroundColor = ConsoleColors.Red,
                Start = "..:: ",
                End = " ::..",
            };
            var decoration2 = new Decoration()
            {
                ForegroundColor = ConsoleColors.Lime,
                Start = "..::",
                End = "::..",
            };
            var decoration3 = new Decoration()
            {
                ForegroundColor = ConsoleColors.Blue,
                Start = "..::",
                End = "::..",
            };
            TextWriterRaw.WritePlain("Full decoration:  " + decoration1.Render());
            TextWriterRaw.WritePlain("Start decoration: " + decoration2.RenderStart());
            TextWriterRaw.WritePlain("End decoration:   " + decoration3.RenderEnd() + "\n");

            var alignedTextUndecorated = new AlignedText("Aligned text")
            {
                ForegroundColor = ConsoleColors.Yellow,
                Top = 6,
                Left = "Aligned text without decoration: ".Length,
            };
            var alignedTextDecorated = new AlignedText("Aligned text")
            {
                ForegroundColor = ConsoleColors.Yellow,
                UseColors = true,
                Top = 7,
                Left = "Aligned text without decoration: ".Length,
                Decoration = decoration2,
            };
            TextWriterRaw.WritePlain("Aligned text without decoration: " + alignedTextUndecorated.Render());
            TextWriterRaw.WritePlain("Aligned text with decoration:    " + alignedTextDecorated.Render());
        }
    }
}
