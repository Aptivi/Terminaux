//
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

using Terminaux.Reader;
using Terminaux.Reader.History;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Reader
{
    internal class PromptLoopedHistoriesNamed : IFixture
    {
        private readonly string secondHistoryInfo =
            """
            {
                "HistoryName": "History 2",
                "HistoryEntries": [
                    "git init",
                    "git commit",
                    "git push"
                ]
            }
            """;

        public void RunFixture()
        {
            TextWriterColor.Write("Write \"exit\" to get out of here.");
            string input = "";
            HistoryInfo firstHistoryInfo = new("History 1",
            [
                "dotnet new",
                "dotnet build",
            ]);
            HistoryTools.LoadFromInstance(firstHistoryInfo);
            while (input != "exit")
            {
                input = TermReader.Read("[4] > ", "", new TermReaderSettings() { HistoryName = "History 1" }, false, false, false);
                TextWriterColor.Write("You said: " + input);
            }
            string saved = HistoryTools.SaveToString(firstHistoryInfo);
            TextWriterColor.Write(saved);
            HistoryTools.LoadFromJson(secondHistoryInfo);
            string input2 = "";
            while (input2 != "exit")
            {
                input2 = TermReader.Read("[5] > ", "", new TermReaderSettings() { HistoryName = "History 2" }, false, false, false);
                TextWriterColor.Write("You said: " + input2);
            }
            string saved2 = HistoryTools.SaveToString("History 2");
            TextWriterColor.Write(saved2);
            string input3 = "";
            while (input3 != "exit")
            {
                input3 = TermReader.Read("[6] > ", "", false, false, false);
                TextWriterColor.Write("You said: " + input3);
            }
            string saved3 = HistoryTools.SaveToString(HistoryTools.generalHistory);
            TextWriterColor.Write(saved3);
            HistoryTools.Unload(firstHistoryInfo);
            HistoryTools.Unload("History 2");
        }
    }
}
