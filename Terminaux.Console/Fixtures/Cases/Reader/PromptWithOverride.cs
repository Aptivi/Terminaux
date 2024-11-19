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

using System;
using Terminaux.Reader;
using Terminaux.Reader.Bindings;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Reader
{
    internal class PromptWithOverride : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Reader;

        public void RunFixture()
        {
            BindingsTools.Override(
                new ConsoleKeyInfo('\u0001', ConsoleKey.A, false, false, true),
                new ConsoleKeyInfo('\r', ConsoleKey.Enter, true, false, false)
            );
            string input = TermReader.Read("", "");
            TextWriterColor.Write("You said: " + input);
            BindingsTools.RemoveOverride(
                new ConsoleKeyInfo('\u0001', ConsoleKey.A, false, false, true),
                new ConsoleKeyInfo('\r', ConsoleKey.Enter, true, false, false)
            );
        }
    }
}
