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

using System.Collections.Generic;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;

namespace Terminaux.ConsoleDemo.Fixtures.Cases.Input
{
    internal class TestInputSelectionMultipleDisabled : IFixture
    {
        public string FixtureID => "TestInputSelectionMultipleDisabled";
        public void RunFixture()
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            InputChoiceInfo[] choices =
            [
                new("focal", "20.04 (Focal Fossa)", "Ubuntu 20.04 LTS, codenamed Focal Fossa, is a long-term support release and was released on 23 April 2020."),
                new("jammy", "22.04 (Jammy Jellyfish)", "Ubuntu 22.04, codenamed Jammy Jellyfish, was released on 21 April 2022, and is a long-term support release, supported for five years, until April 2027.", false, true),
                new("noble", "24.04 (Noble Numbat)", "Ubuntu 24.04, codenamed Noble Numbat, is planned to be released on April 2024, and is a long-term support release, supported for five years, until April 2029.", true, true),
            ];
            InputChoiceInfo[] altChoices =
            [
                new("eoan", "19.10 (Eoan Ermine)", "Ubuntu 19.10 Eoan Ermine is an interim release, released 17 October 2019.", false, false, true),
                new("lunar", "23.04 (Lunar Lobster)", "Ubuntu 23.04 Lunar Lobster is an interim release, released 20 April 2023."),
                new("mantic", "23.10 (Mantic Minotaur)", "Ubuntu 23.10 Mantic Minotaur is an interim release, scheduled 12 October 2023."),
            ];
            var answers = SelectionMultipleStyle.PromptMultipleSelection("Which Ubuntu version would you like to run?", choices, altChoices);
            TextWriterColor.Write(string.Join(", ", answers));
        }
    }
}
