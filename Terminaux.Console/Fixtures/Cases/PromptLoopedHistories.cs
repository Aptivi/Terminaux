
// Terminaux  Copyright (C) 2023  Aptivi
// 
// This file is part of Terminaux
// 
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using Terminaux.Reader;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class PromptLoopedHistories : IFixture
    {
        private readonly List<string> firstHistory = new()
        {
            "dotnet new",
            "dotnet build",
        };
        private readonly List<string> secondHistory = new()
        {
            "git init",
            "git commit",
            "git push",
        };

        public string FixtureID => "PromptLoopedHistories";

        public void RunFixture()
        {
            Console.WriteLine("Write \"exit\" to get out of here.");
            string input = "";
            TermReaderTools.SetHistory(firstHistory);
            while (input != "exit")
            {
                input = TermReader.Read("[1] > ");
                Console.WriteLine("You said: " + input);
            }
            TermReaderTools.SetHistory(secondHistory);
            string input2 = "";
            while (input2 != "exit")
            {
                input2 = TermReader.Read("[2] > ");
                Console.WriteLine("You said: " + input2);
            }
            TermReaderTools.ClearHistory();
            string input3 = "";
            while (input3 != "exit")
            {
                input3 = TermReader.Read("[3] > ");
                Console.WriteLine("You said: " + input3);
            }
        }
    }
}
