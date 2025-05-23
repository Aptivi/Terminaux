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
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class BoxFrameTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var boxFrame = new BoxFrame("[#7711ff][bold]Box[/] frame[/]")
            {
                Left = 8,
                Top = 4,
                Width = 20,
                Height = 5,
            };

            // Write our markup
            TextWriterColor.Write("Terminaux's Box Frame:");
            TextWriterColor.Write(boxFrame.Render());
            ConsoleWrapper.CursorTop = 12;

            // Now, convert them to Spectre's panel
            var spectrePanel = TranslationTools.GetPanel(boxFrame);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's Panel:");
            AnsiConsole.Write(spectrePanel);
            AnsiConsole.WriteLine();
        }
    }
}
