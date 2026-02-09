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
    internal class TestBreakdownChartVertical : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            TextWriterColor.WriteColor("This chart describes a list of languages in an example project:", true, new Color(ConsoleColors.Green));
            var chart = new BreakdownChart()
            {
                Width = ConsoleWrapper.WindowWidth - 4,
                Height = ConsoleWrapper.WindowHeight - 8,
                Left = 2,
                Top = 4,
                Showcase = true,
                Vertical = true,
                Elements =
                [
                    new()
                    {
                        Color = ConsoleColors.Red,
                        Name = "SCSS",
                        Value = 80,
                    },
                    new()
                    {
                        Color = ConsoleColors.Blue,
                        Name = "HTML",
                        Value = 28.3,
                    },
                    new()
                    {
                        Color = ConsoleColors.Lime,
                        Name = "C#",
                        Value = 22.6,
                    },
                    new()
                    {
                        Color = ConsoleColors.Yellow,
                        Name = "JavaScript",
                        Value = 6,
                    },
                    new()
                    {
                        Color = ConsoleColors.DarkViolet,
                        Name = "Ruby",
                        Value = 6,
                    },
                    new()
                    {
                        Color = ConsoleColors.Aqua,
                        Name = "Shell",
                        Value = 0.1,
                    },
                ],
            };
            TextWriterRaw.WriteRaw(chart.Render());
        }
    }
}
