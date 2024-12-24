//
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
using Terminaux.Reader;
using Terminaux.Reader.History;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Reader
{
    internal class PromptLoopedHistories : IFixture
    {
        private readonly List<string> firstHistory =
        [
            "dotnet new",
            "dotnet build",
        ];
        private readonly List<string> secondHistory =
        [
            "git init",
            "git commit",
            "git push",
        ];

        public FixtureCategory Category => FixtureCategory.Reader;

        public void RunFixture()
        {
            TextWriterColor.Write("Write \"exit\" to get out of here.");
            string input = "";
            HistoryTools.Switch(HistoryTools.generalHistory, [.. firstHistory]);
            while (input != "exit")
            {
                input = TermReader.Read("[1] > ", "", false, false, false);
                TextWriterColor.Write("You said: " + input);
            }
            HistoryTools.Switch(HistoryTools.generalHistory, [.. secondHistory]);
            string input2 = "";
            while (input2 != "exit")
            {
                input2 = TermReader.Read("[2] > ", "", false, false, false);
                TextWriterColor.Write("You said: " + input2);
            }
            HistoryTools.Clear(HistoryTools.generalHistory);
            string input3 = "";
            while (input3 != "exit")
            {
                input3 = TermReader.Read("[3] > ", "", false, false, false);
                TextWriterColor.Write("You said: " + input3);
            }
        }
    }
}
