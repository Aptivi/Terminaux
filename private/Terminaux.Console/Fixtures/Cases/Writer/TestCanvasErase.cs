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

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestCanvasErase : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Show a canvas
            var canvas = new Canvas()
            {
                Left = 2,
                Top = 2,
                Color = ConsoleColors.Green,
                Width = 20,
                Height = 20,
                Pixels =
                [
                    // Draw the top part of the "T" letter
                    new(2, 2) { CellColor = ConsoleColors.Yellow },
                    new(3, 2) { CellColor = ConsoleColors.Yellow },
                    new(4, 2) { CellColor = ConsoleColors.Yellow },
                    new(5, 2) { CellColor = ConsoleColors.Yellow },
                    new(6, 2) { CellColor = ConsoleColors.Yellow },
                    new(7, 2) { CellColor = ConsoleColors.Yellow },
                    new(8, 2) { CellColor = ConsoleColors.Yellow },
                    new(9, 2) { CellColor = ConsoleColors.Yellow },
                    new(10, 2) { CellColor = ConsoleColors.Yellow },
                    new(11, 2) { CellColor = ConsoleColors.Yellow },
                    new(12, 2) { CellColor = ConsoleColors.Yellow },
                    new(13, 2) { CellColor = ConsoleColors.Yellow },
                    new(14, 2) { CellColor = ConsoleColors.Yellow },
                    new(15, 2) { CellColor = ConsoleColors.Yellow },
                    new(16, 2) { CellColor = ConsoleColors.Yellow },
                    new(17, 2) { CellColor = ConsoleColors.Yellow },
                    new(18, 2) { CellColor = ConsoleColors.Yellow },
                    new(2, 3) { CellColor = ConsoleColors.Yellow },
                    new(3, 3) { CellColor = ConsoleColors.Yellow },
                    new(4, 3) { CellColor = ConsoleColors.Yellow },
                    new(5, 3) { CellColor = ConsoleColors.Yellow },
                    new(6, 3) { CellColor = ConsoleColors.Yellow },
                    new(7, 3) { CellColor = ConsoleColors.Yellow },
                    new(8, 3) { CellColor = ConsoleColors.Yellow },
                    new(9, 3) { CellColor = ConsoleColors.Yellow },
                    new(10, 3) { CellColor = ConsoleColors.Yellow },
                    new(11, 3) { CellColor = ConsoleColors.Yellow },
                    new(12, 3) { CellColor = ConsoleColors.Yellow },
                    new(13, 3) { CellColor = ConsoleColors.Yellow },
                    new(14, 3) { CellColor = ConsoleColors.Yellow },
                    new(15, 3) { CellColor = ConsoleColors.Yellow },
                    new(16, 3) { CellColor = ConsoleColors.Yellow },
                    new(17, 3) { CellColor = ConsoleColors.Yellow },
                    new(18, 3) { CellColor = ConsoleColors.Yellow },
                    
                    // Draw the line of the "T" letter
                    new(9, 3) { CellColor = ConsoleColors.Yellow },
                    new(9, 4) { CellColor = ConsoleColors.Yellow },
                    new(9, 5) { CellColor = ConsoleColors.Yellow },
                    new(9, 6) { CellColor = ConsoleColors.Yellow },
                    new(9, 7) { CellColor = ConsoleColors.Yellow },
                    new(9, 8) { CellColor = ConsoleColors.Yellow },
                    new(9, 9) { CellColor = ConsoleColors.Yellow },
                    new(9, 10) { CellColor = ConsoleColors.Yellow },
                    new(9, 11) { CellColor = ConsoleColors.Yellow },
                    new(9, 12) { CellColor = ConsoleColors.Yellow },
                    new(9, 13) { CellColor = ConsoleColors.Yellow },
                    new(9, 14) { CellColor = ConsoleColors.Yellow },
                    new(9, 15) { CellColor = ConsoleColors.Yellow },
                    new(9, 16) { CellColor = ConsoleColors.Yellow },
                    new(9, 17) { CellColor = ConsoleColors.Yellow },
                    new(9, 18) { CellColor = ConsoleColors.Yellow },
                    new(9, 19) { CellColor = ConsoleColors.Yellow },
                    new(10, 3) { CellColor = ConsoleColors.Yellow },
                    new(10, 4) { CellColor = ConsoleColors.Yellow },
                    new(10, 5) { CellColor = ConsoleColors.Yellow },
                    new(10, 6) { CellColor = ConsoleColors.Yellow },
                    new(10, 7) { CellColor = ConsoleColors.Yellow },
                    new(10, 8) { CellColor = ConsoleColors.Yellow },
                    new(10, 9) { CellColor = ConsoleColors.Yellow },
                    new(10, 10) { CellColor = ConsoleColors.Yellow },
                    new(10, 11) { CellColor = ConsoleColors.Yellow },
                    new(10, 12) { CellColor = ConsoleColors.Yellow },
                    new(10, 13) { CellColor = ConsoleColors.Yellow },
                    new(10, 14) { CellColor = ConsoleColors.Yellow },
                    new(10, 15) { CellColor = ConsoleColors.Yellow },
                    new(10, 16) { CellColor = ConsoleColors.Yellow },
                    new(10, 17) { CellColor = ConsoleColors.Yellow },
                    new(10, 18) { CellColor = ConsoleColors.Yellow },
                    new(10, 19) { CellColor = ConsoleColors.Yellow },
                    new(11, 3) { CellColor = ConsoleColors.Yellow },
                    new(11, 4) { CellColor = ConsoleColors.Yellow },
                    new(11, 5) { CellColor = ConsoleColors.Yellow },
                    new(11, 6) { CellColor = ConsoleColors.Yellow },
                    new(11, 7) { CellColor = ConsoleColors.Yellow },
                    new(11, 8) { CellColor = ConsoleColors.Yellow },
                    new(11, 9) { CellColor = ConsoleColors.Yellow },
                    new(11, 10) { CellColor = ConsoleColors.Yellow },
                    new(11, 11) { CellColor = ConsoleColors.Yellow },
                    new(11, 12) { CellColor = ConsoleColors.Yellow },
                    new(11, 13) { CellColor = ConsoleColors.Yellow },
                    new(11, 14) { CellColor = ConsoleColors.Yellow },
                    new(11, 15) { CellColor = ConsoleColors.Yellow },
                    new(11, 16) { CellColor = ConsoleColors.Yellow },
                    new(11, 17) { CellColor = ConsoleColors.Yellow },
                    new(11, 18) { CellColor = ConsoleColors.Yellow },
                    new(11, 19) { CellColor = ConsoleColors.Yellow },
                ]
            };
            var eraser1 = new Eraser()
            {
                Left = 2,
                Top = 2,
                Width = 40,
                Height = 1,
            };
            var eraser2 = new Eraser()
            {
                Left = 2,
                Top = 3,
                Width = 2,
                Height = 19,
            };
            var eraser3 = new Eraser()
            {
                Left = 2,
                Top = 21,
                Width = 40,
                Height = 1,
            };
            var eraser4 = new Eraser()
            {
                Left = 38,
                Top = 3,
                Width = 4,
                Height = 19,
            };
            TextWriterRaw.WriteRaw(canvas.Render());
            TextWriterRaw.WriteRaw(eraser1.Render());
            TextWriterRaw.WriteRaw(eraser2.Render());
            TextWriterRaw.WriteRaw(eraser3.Render());
            TextWriterRaw.WriteRaw(eraser4.Render());
        }
    }
}
