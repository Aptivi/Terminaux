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
    internal class TestBarChart : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Taken from https://gs.statcounter.com/os-version-market-share/android/mobile-tablet/worldwide
            TextWriterColor.WriteColor("Here's the market share for Android 11 (2020) to 14 (2023) as of September 2024 according to Statcounter:", true, new Color(ConsoleColors.Green));
            var chart = new BarChart()
            {
                Width = ConsoleWrapper.WindowWidth - 4,
                Height = 4,
                Left = 2,
                Top = 4,
                Showcase = true,
                Elements =
                [
                    new()
                    {
                        Color = "cmyk:7;82;100;1",
                        Name = "Android 14",
                        Value = 33.68,
                    },
                    new()
                    {
                        Color = "84;214;133",
                        Name = "Android 13",
                        Value = 19.86,
                    },
                    new()
                    {
                        Name = "Android 12",
                        Value = 14.25,
                    },
                    new()
                    {
                        Name = "Android 11",
                        Value = 12.71,
                    },
                ],
            };
            TextWriterRaw.WriteRaw(chart.Render());
        }
    }
}
