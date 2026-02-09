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
using Terminaux.Base.Extensions;
using Colorimetry.Data;
using Terminaux.Spectre;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;

namespace Terminaux.Console.Fixtures.Cases.Spectre
{
    internal class MarkupTranslation : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Spectre;

        public void RunFixture()
        {
            var firstMarkup = new Mark("Hello [bold]world[/] in a default color! This is [Red]red[/] color with [#7711ff][bold]true[/] color[/] support!");
            var secondMarkup = new Mark("Hello [bold]world[/] in [[[standout underline]green[/]]]! This is [Red]red[/] color with [#7711ff][underline]true[/] color[/] support!");

            // Write our markup
            TextWriterColor.Write("Terminaux's Markup:");
            TextWriterColor.Write(firstMarkup);
            TextWriterColor.WriteColor(secondMarkup, ConsoleColors.Green);

            // Now, convert them to Spectre's markup
            var spectreFirstMarkup = TranslationTools.GetMarkup(firstMarkup);
            var spectreSecondMarkup = TranslationTools.GetMarkup(secondMarkup);

            // Write Spectre's markup
            TextWriterRaw.Write();
            TextWriterColor.Write("Spectre.Console's Markup:");
            AnsiConsole.Write(spectreFirstMarkup);
            AnsiConsole.WriteLine();
            AnsiConsole.Write(ConsoleColoring.RenderSetConsoleColor(ConsoleColors.Green));
            AnsiConsole.Write(spectreSecondMarkup);
            AnsiConsole.WriteLine();
        }
    }
}
