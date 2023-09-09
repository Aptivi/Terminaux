
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

using System.Collections.Generic;
using Terminaux.Reader.Inputs;
using Terminaux.Reader.Inputs.Styles;

namespace Terminaux.ConsoleDemo.Fixtures.Cases
{
    internal class TestInputChoice : IFixture
    {
        public string FixtureID => "TestInputChoice";
        public void RunFixture()
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new List<InputChoiceInfo>()
            {
                new InputChoiceInfo("f", "20.04 (Focal Fossa)", "Ubuntu 20.04 LTS, codenamed Focal Fossa, is a long-term support release and was released on 23 April 2020."),
                new InputChoiceInfo("j", "22.04 (Jammy Jellyfish)", "Ubuntu 22.04, codenamed Jammy Jellyfish, was released on 21 April 2022, and is a long-term support release, supported for five years, until April 2027."),
            };
            var altChoices = new List<InputChoiceInfo>()
            {
                new InputChoiceInfo("k", "22.10 (Kinetic Kudu)", "Ubuntu 22.10, codenamed Kinetic Kudu, is an interim release and was made on 20 October 2022."),
                new InputChoiceInfo("l", "23.04 (Lunar Lobster)", "Ubuntu 23.04 Lunar Lobster is an interim release, scheduled 20 April 2023."),
            };
            ChoiceStyle.PromptChoice("Which Ubuntu version would you like to run?", choices, altChoices, ChoiceOutputType.Modern);
        }
    }
}
