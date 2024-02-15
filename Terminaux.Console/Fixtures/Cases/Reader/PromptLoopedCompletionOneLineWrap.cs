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
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.Reader
{
    internal class PromptLoopedCompletionOneLineWrap : IFixture
    {
        public string FixtureID => "PromptLoopedCompletionOneLineWrap";

        public void RunFixture()
        {
            TextWriterColor.Write("Write \"exit\" to get out of here.");
            string input = "";
            while (input != "exit")
            {
                var settings = TermReader.GlobalReaderSettings;
                settings.Suggestions = GetSuggestions;
                settings.HistoryEnabled = true;
                input = TermReader.Read(">> ", "", settings, false, true, false);
                TextWriterColor.Write("You said: " + input);
            }
        }

        public string[] GetSuggestions(string text, int index, char[] delims)
        {
            string[] parts = text.Split(delims);
            if (parts.Length == 0)
                return [];
            else
            {
                if (parts[0] == "dotnet")
                    return ["build", "restore", "run"];
                else if (parts[0] == "git")
                    return ["fetch", "pull", "push"];
            }
            return [];
        }
    }
}
