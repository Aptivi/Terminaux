﻿//
// Terminaux  Copyright (C) 2023-2025  Aptivi
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

using Textify.Data.Figlet;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintFigletAlignMiddlePositionalF : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var font = FigletFonts.TryGetByName("banner3");
            if (font is null)
                return;
            var aligned = new AlignedFigletText(font, "Hello, {0}!", "Terminaux")
            {
                ForegroundColor = ConsoleColors.Green,
                Settings = new() { Alignment = TextAlignment.Middle },
                Top = 5,
                Left = 20,
                Margins = new(0, 0, 30, 0)
            };
            TextWriterRaw.WriteRaw(aligned.Render());
        }
    }
}
