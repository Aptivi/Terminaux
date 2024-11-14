﻿//
// Terminaux  Copyright (C) 2023-2024  Aptivi
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

using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Markup;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class PrintMarkup : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            TextWriterColor.Write(MarkupTools.ParseMarkup("Hello [bold]world[/] in a default color! This is [Red]red[/] color with [#7711ff][bold]true[/] color[/] support!"));
            TextWriterColor.Write(MarkupTools.ParseMarkup("Hello [bold]world[/] in [[[standout underline]green[/]]]! This is [Red]red[/] color with [#7711ff][bold]true[/] color[/] support!", new Color(ConsoleColors.Green)));
        }
    }
}
