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

using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.Writer
{
    internal class PrintWithNewLines : IFixture
    {
        public string FixtureID => "PrintWithNewLines";
        public void RunFixture()
        {
            TextWriterColor.WriteColor("Hello world!\nHow's your day going?\nShould be directly after this:", false, new Color(ConsoleColors.Green));
            TextWriterColor.WriteColor(" [{0}, {1}] ", true, new Color(ConsoleColors.Gray), vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
        }
    }
}