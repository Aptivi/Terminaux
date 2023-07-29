
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
using Terminaux.Reader;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class PromptLoopedCompletionOneLineWrap : IFixture
    {
        public string FixtureID => "PromptLoopedCompletionOneLineWrap";

        public void RunFixture()
        {
            Console.WriteLine("Write \"exit\" to get out of here.");
            string input = "";
            while (input != "exit")
            {
                TermReaderSettings.Suggestions = GetSuggestions;
                TermReaderSettings.HistoryEnabled = true;
                input = TermReader.Read(">> ", "", false, true);
                Console.WriteLine("You said: " + input);
            }
        }

        public string[] GetSuggestions(string text, int index, char[] delims)
        {
            string[] parts = text.Split(delims);
            if (parts.Length == 0)
                return Array.Empty<string>();
            else
            {
                if (parts[0] == "dotnet")
                    return new string[] { "build", "restore", "run" };
                else if (parts[0] == "git")
                    return new string[] { "fetch", "pull", "push" };
            }
            return Array.Empty<string>();
        }
    }
}
