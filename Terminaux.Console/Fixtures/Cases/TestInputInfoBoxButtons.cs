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

using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class TestInputInfoBoxButtons : IFixture
    {
        public string FixtureID => "TestInputInfoBoxButtons";
        public void RunFixture()
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new string[]
            {
                "20.04 (Focal Fossa)",
                "22.04 (Jammy Jellyfish)",
                "24.04 (Noble Numbat)",
            };
            int selected = InfoBoxButtonsColor.WriteInfoBoxButtons(FixtureID, choices, "Which Ubuntu version would you like to run?");
            TextWriterWhereColor.WriteWhere($"{selected}", 0, 0);
        }
    }
}
