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
using Terminaux.Writer.CyclicWriters.Graphical;
using Textify.Data.Figlet;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class FigletTextTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var aligned = new AlignedFigletText(FigletFonts.GetByName("small"), "Hello world!")
            {
                Left = 4,
                Top = 4,
                ForegroundColor = ConsoleColors.Green,
            };

            // Write our markup
            TextWriterColor.Write("Terminaux's figlet text:");
            TextWriterColor.Write(aligned.Render());
            ConsoleWrapper.CursorTop = 12;

            // Now, convert them to Spectre's figlet text
            var spectreFigletText = TranslationTools.GetFigletText(aligned);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's figlet text:");
            AnsiConsole.Write(spectreFigletText);
            AnsiConsole.WriteLine();
        }
    }
}
