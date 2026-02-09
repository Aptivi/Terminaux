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

using Terminaux.Base.Extensions;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintAlignMiddleOff : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var aligned = new AlignedText()
            {
                Text = $"Hello world! This is located in the {ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Blue))}middle of the console{ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Green))} and is intentionally very long to demonstrate the off-centering with right margin offset and left margin offset.",
                ForegroundColor = ConsoleColors.Green,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle
                },
                Left = 15,
                Top = 2,
                Margins = new(0, 0, 45, 0)
            };
            TextWriterRaw.WriteRaw(aligned.Render());
        }
    }
}
