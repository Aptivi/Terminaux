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
    internal class TestLineChartWithAvg : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            // Taken from https://gs.statcounter.com/os-version-market-share/android/mobile-tablet/worldwide
            TextWriterColor.WriteColor("This chart describes Android 13's market share from 9/2023 to 9/2024:", true, new Color(ConsoleColors.Green));
            var chart = new LineChart()
            {
                Width = ConsoleWrapper.WindowWidth - 4,
                Height = ConsoleWrapper.WindowHeight - 8,
                Left = 2,
                Top = 4,
                Showcase = true,
                RunChart = true,
                Elements =
                [
                    new()
                    {
                        Name = "September 2023",
                        Value = 34.92,
                    },
                    new()
                    {
                        Name = "October 2023",
                        Value = 36.46,
                    },
                    new()
                    {
                        Name = "November 2023",
                        Value = 37.63,
                    },
                    new()
                    {
                        Name = "December 2023",
                        Value = 35.44,
                    },
                    new()
                    {
                        Name = "January 2024",
                        Value = 32.27,
                    },
                    new()
                    {
                        Name = "February 2024",
                        Value = 28.83,
                    },
                    new()
                    {
                        Name = "March 2024",
                        Value = 26.26,
                    },
                    new()
                    {
                        Name = "April 2024",
                        Value = 24.42,
                    },
                    new()
                    {
                        Name = "May 2024",
                        Value = 23.34,
                    },
                    new()
                    {
                        Name = "June 2024",
                        Value = 22.28,
                    },
                    new()
                    {
                        Name = "July 2024",
                        Value = 21.31,
                    },
                    new()
                    {
                        Name = "August 2024",
                        Value = 20.56,
                    },
                    new()
                    {
                        Name = "September 2024",
                        Value = 19.86,
                    },
                ],
            };
            TextWriterRaw.WriteRaw(chart.Render());
        }
    }
}
