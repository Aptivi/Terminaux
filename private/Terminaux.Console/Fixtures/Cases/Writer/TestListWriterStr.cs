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

namespace Terminaux.Console.Fixtures.Cases.Writer
{
    internal class TestListWriterStr : IFixture
    {
        public void RunFixture()
        {
            var NormalStringList = new List<string>() { "String 1", "String 2", "String 3" };
            var ArrayStringList = new List<string[]>() { { new string[] { "String 1", "String 2", "String 3" } }, { new string[] { "String 1", "String 2", "String 3" } }, { new string[] { "String 1", "String 2", "String 3" } } };
            TextWriterColor.Write("Normal string list:");
            ListWriterColor.WriteList(NormalStringList, ConsoleColors.Silver, ConsoleColors.Grey, false);
            TextWriterColor.Write("Array string list:");
            ListWriterColor.WriteList(ArrayStringList, ConsoleColors.Silver, ConsoleColors.Grey, false);
        }
    }
}
