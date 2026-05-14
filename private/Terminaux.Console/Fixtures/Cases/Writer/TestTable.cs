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

using System;
using Terminaux.Base;
using Colorimetry.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestTable : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var Rows = new TableCellOptions[,]
            {
                { new("Ubuntu Version"), new("Release Date"), new("Support End"), new("ESM Support End") },
                { new("12.04 (Precise Pangolin)"), new(new DateTime(2012, 4, 26).ToString()), new(new DateTime(2017, 4, 28).ToString()), new(new DateTime(2019, 4, 28).ToString()) },
                { new("14.04 (Trusty Tahr)"), new(new DateTime(2014, 4, 17).ToString()), new(new DateTime(2019, 4, 25).ToString()), new(new DateTime(2024, 4, 25).ToString()) },
                { new("16.04 (Xenial Xerus)"), new(new DateTime(2016, 4, 21).ToString()), new(new DateTime(2021, 4, 30).ToString()), new(new DateTime(2026, 4, 30).ToString()) },
                { new("18.04 (Bionic Beaver)"), new(new DateTime(2018, 4, 26).ToString()), new(new DateTime(2023, 4, 30).ToString()), new(new DateTime(2028, 4, 30).ToString()) },
                { new("20.04 (Focal Fossa)"), new(new DateTime(2020, 4, 23).ToString()), new(new DateTime(2025, 4, 25).ToString()), new(new DateTime(2030, 4, 25).ToString()) },
                { new("22.04 (Jammy Jellyfish)"), new(new DateTime(2022, 4, 26).ToString()), new(new DateTime(2027, 4, 25).ToString()), new(new DateTime(2032, 4, 25).ToString()) }
            };
            Rows[1, 1].CellColor = ConsoleColors.Red;
            Rows[1, 1].CellBackgroundColor = ConsoleColors.DarkRed;
            Rows[1, 1].ColoredCell = true;
            var table = new Table()
            {
                Rows = Rows,
                Left = 4,
                Top = 2,
                Width = ConsoleWrapper.WindowWidth - 7,
                Height = ConsoleWrapper.WindowHeight - 5,
                Header = true,
            };
            TextWriterRaw.WriteRaw(table.Render());
        }
    }
}
