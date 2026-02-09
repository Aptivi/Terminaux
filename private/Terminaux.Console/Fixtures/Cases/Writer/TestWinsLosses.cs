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

using Terminaux.Base;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestWinsLosses : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            TextWriterColor.WriteColor("This chart describes wins and losses of a fictional company, 'McKenzie Electronics':", true, new Color(ConsoleColors.Green));
            var chart = new WinsLosses()
            {
                Width = ConsoleWrapper.WindowWidth - 4,
                Height = ConsoleWrapper.WindowHeight - 8,
                Left = 2,
                Top = 4,
                Showcase = true,
                Elements =
                [
                    ("January 2023", new(){ Name = "Win", Value = 85.29 }, new(){ Name = "Loss", Value = 43.46 }),
                    ("February 2023", new(){ Name = "Win", Value = 86.22 }, new(){ Name = "Loss", Value = 44.22 }),
                    ("March 2023", new(){ Name = "Win", Value = 89.32 }, new(){ Name = "Loss", Value = 40.20 }),
                    ("April 2023", new(){ Name = "Win", Value = 90.01 }, new(){ Name = "Loss", Value = 39.85 }),
                    ("May 2023", new(){ Name = "Win", Value = 89.43 }, new(){ Name = "Loss", Value = 42.02 }),
                    ("June 2023", new(){ Name = "Win", Value = 87.49 }, new(){ Name = "Loss", Value = 46.22 }),
                ],
            };
            TextWriterRaw.WriteRaw(chart.Render());
        }
    }
}
