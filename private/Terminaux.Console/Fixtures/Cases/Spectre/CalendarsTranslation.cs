//
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

using Spectre.Console;
using System;
using Terminaux.Base;
using Terminaux.Spectre;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class CalendarsTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var calendar = new Calendars()
            {
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,
                Left = 4,
                Top = 4,
                Width = ConsoleWrapper.WindowWidth - 8,
                Height = ConsoleWrapper.WindowHeight - 4,
            };

            // Write our markup
            TextWriterColor.Write("Terminaux's calendar:");
            TextWriterColor.Write(calendar.Render());
            ConsoleWrapper.CursorTop = 19;

            // Now, convert them to Spectre's calendar
            var spectreCalendar = TranslationTools.GetCalendar(calendar);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's calendar:");
            AnsiConsole.Write(spectreCalendar);
            AnsiConsole.WriteLine();
        }
    }
}
