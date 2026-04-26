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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintMarkupLinks : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            TextWriterColor.Write(MarkupTools.ParseMarkup("This is on [link]https://www.google.com[/], and you can click it."));
            TextWriterColor.Write(MarkupTools.ParseMarkup("This is on [#7711ff link]https://www.google.com[/], and you can click it."));
            TextWriterColor.Write(MarkupTools.ParseMarkup("This is on [link=https://www.google.com]Google[/], and you can click it."));
            TextWriterColor.Write(MarkupTools.ParseMarkup("This is on [#7711ff link=https://www.google.com]Google[/], and you can click it."));
        }
    }
}
