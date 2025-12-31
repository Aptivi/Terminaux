//
// Terminaux  Copyright (C) 2023-2026  Aptivi
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

using System.Linq;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.Console.Fixtures.Cases.Reader
{
    internal class PromptLoopedCompletionMultiLinePrompt : IFixture
    {
        public FixtureCategory Category => FixtureCategory.Reader;

        public void RunFixture()
        {
            TextWriterColor.Write("Write \"exit\" to get out of here.");
            string input = "";
            while (input != "exit")
            {
                var settings = new TermReaderSettings(TermReader.GlobalReaderSettings)
                {
                    Suggestions = GetSuggestions
                };
                input = TermReader.Read("| Say something!\n->> ", "", settings, false, false, false);
                TextWriterColor.Write("You said: " + input);
            }
        }

        public string[] GetSuggestions(string text, int index, char[] delims)
        {
            text = text.Substring(0, index);
            string[] parts = text.Split(delims);
            if (parts.Length == 0)
                return [];
            else
            {
                if (parts[0] == "dotnet")
                    return new string[] { "build", "restore", "run", "nuget", "new", "pack", "publish" }
                        .Where((str) => str.StartsWith(parts[1]))
                        .Select((str) => str.Substring(parts[1].Length))
                        .ToArray();
                else if (parts[0] == "git")
                    return new string[] { "fetch", "pull", "push" }
                        .Where((str) => str.StartsWith(parts[1]))
                        .Select((str) => str.Substring(parts[1].Length))
                        .ToArray();
            }
            return [];
        }
    }
}
