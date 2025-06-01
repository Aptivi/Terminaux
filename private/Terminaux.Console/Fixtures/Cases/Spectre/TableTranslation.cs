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
using Terminaux.Base;
using Terminaux.Spectre;
using Terminaux.Writer.ConsoleWriters;
using Table = Terminaux.Writer.CyclicWriters.Graphical.Table;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class TableTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var table = new Table()
            {
                Left = 8,
                Top = 4,
                Width = 40,
                Height = 5,
                Header = true,
                Rows = new string[,]
                {
                    { "Header", "Header 2", "Header 3" },
                    { "Hello", "World", "!!!" },
                    { "!!!", "Hello", "World" },
                }
            };

            // Write our markup
            TextWriterColor.Write("Terminaux's Table:");
            TextWriterColor.Write(table.Render());
            ConsoleWrapper.CursorTop = 12;

            // Now, convert them to Spectre's table
            var spectreTable = TranslationTools.GetTable(table);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's Table:");
            AnsiConsole.Write(spectreTable);
            AnsiConsole.WriteLine();
        }
    }
}
