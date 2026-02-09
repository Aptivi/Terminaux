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

using Spectre.Console;
using Terminaux.Base;
using Colorimetry.Data;
using Terminaux.Spectre;
using Terminaux.Writer.ConsoleWriters;
using BreakdownChart = Terminaux.Writer.CyclicWriters.Graphical.BreakdownChart;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class BreakdownChartTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var chart = new BreakdownChart()
            {
                Width = ConsoleWrapper.WindowWidth - 4,
                Left = 2,
                Top = 4,
                Showcase = true,
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

            // Write our markup
            TextWriterColor.Write("Terminaux's breakdown chart:");
            TextWriterColor.Write(chart.Render());
            ConsoleWrapper.CursorTop = 12;

            // Now, convert them to Spectre's breakdown chart
            var spectreBreakdownChart = TranslationTools.GetBreakdownChart(chart);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's breakdown chart:");
            AnsiConsole.Write(spectreBreakdownChart);
            AnsiConsole.WriteLine();
        }
    }
}
