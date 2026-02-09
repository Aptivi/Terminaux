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
            var Rows = new string[,]
            {
                { "Ubuntu Version", "Release Date", "Support End", "ESM Support End" },
                { "12.04 (Precise Pangolin)", new DateTime(2012, 4, 26).ToString(), new DateTime(2017, 4, 28).ToString(), new DateTime(2019, 4, 28).ToString() },
                { "14.04 (Trusty Tahr)", new DateTime(2014, 4, 17).ToString(), new DateTime(2019, 4, 25).ToString(), new DateTime(2024, 4, 25).ToString() },
                { "16.04 (Xenial Xerus)", new DateTime(2016, 4, 21).ToString(), new DateTime(2021, 4, 30).ToString(), new DateTime(2026, 4, 30).ToString() },
                { "18.04 (Bionic Beaver)", new DateTime(2018, 4, 26).ToString(), new DateTime(2023, 4, 30).ToString(), new DateTime(2028, 4, 30).ToString() },
                { "20.04 (Focal Fossa)", new DateTime(2020, 4, 23).ToString(), new DateTime(2025, 4, 25).ToString(), new DateTime(2030, 4, 25).ToString() },
                { "22.04 (Jammy Jellyfish)", new DateTime(2022, 4, 26).ToString(), new DateTime(2027, 4, 25).ToString(), new DateTime(2032, 4, 25).ToString() }
            };
            var table = new Table()
            {
                Rows = Rows,
                Left = 4,
                Top = 2,
                Width = ConsoleWrapper.WindowWidth - 7,
                Height = ConsoleWrapper.WindowHeight - 5,
                Header = true,
                Settings =
                [
                    new CellOptions(2, 2)
                    {
                        CellColor = ConsoleColors.Red,
                        CellBackgroundColor = ConsoleColors.DarkRed,
                        ColoredCell = true
                    }
                ]
            };
            TextWriterRaw.WriteRaw(table.Render());
        }
    }
}
