﻿//
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

using System.Collections.Generic;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters;
using Textify.Data.NameGen;

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestListWriterHuge : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Writer;

        public void RunFixture()
        {
            var choices = new List<string>();
            var arrayChoices = new List<string[]>();
            var names = NameGenerator.FindFirstNames("");
            for (int i = 0; i < names.Length; i++)
            {
                choices.Add(names[i]);
                arrayChoices.Add([$"Number #{i + 1}", names[i]]);
            }
            var normalStrings = new Listing()
            {
                Objects = choices,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
            };
            var arrayStrings = new Listing()
            {
                Objects = arrayChoices,
                KeyColor = ConsoleColors.Silver,
                ValueColor = ConsoleColors.Grey,
            };
            TextWriterColor.Write("Normal string list:\n{0}", normalStrings.Render());
            TextWriterColor.Write("Array string list:\n{0}", arrayStrings.Render());
        }
    }
}
